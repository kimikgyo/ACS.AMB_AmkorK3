using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class ElevatorService
    {
        private readonly ILog ElevatorEventlogger = LogManager.GetLogger("ElevatorEvent");
        private readonly IUnitOfWork uow;
        private readonly Elevator elevators;
        private readonly MainForm main;
        public event EventHandler<bool> ElevatorConnected;  //Elevator Connect 이벤트
        public event EventHandler<string> ElevatorMode;  //Elevator Mode
        bool Connected = false;
        int ConnectedCount = 0;
        public ElevatorService(IUnitOfWork uow)
        {
            this.uow = uow;
            elevators = new Elevator();
        }

        public void Start()
        {
            Task.Run(() => Loop());
        }

        private async void Loop()
        {
            while (true)
            {
                try
                {
                    //await Task.Delay(500); // <=========================== 노드간 통신 간격
                    await Task.Delay(500); // <=========================== 노드간 통신 간격

                    bool recv_good = false;

                    recv_good = await SendRecvAsync();

                    if (recv_good)                         //정상적인 데이터를 읽어왔는지 확인 
                    {
                        ConnectedCount = 0;
                        if (Connected == false)
                        {
                            Connected = true;
                            //컨넥트 이벤트
                            ElevatorConnected?.Invoke(this, Connected);
                            ElevatorEventlogger.Info("Elevator Connect!");
                        }
                    }
                    else
                    {
                        if (ConnectedCount == 10)
                        {
                            if (Connected == true)
                            {
                                Connected = false;
                                ElevatorConnected?.Invoke(this, Connected);
                                ElevatorEventlogger.Info("Elevator DisConnect!");
                                ConnectedCount = 0;
                               var elevatorModeChange =  uow.ACSModeInfo.Find(e=>e.Location == "Elevator" && e.ElevatorMode != "NotAGVMode").FirstOrDefault();
                                if(elevatorModeChange !=null)
                                {
                                    elevatorModeChange.ElevatorMode = "NotAGVMode";
                                    uow.ACSModeInfo.Update(elevatorModeChange);
                                    ElevatorMode?.Invoke(this, elevatorModeChange.ElevatorMode);
                                }
                            }
                        }
                        else ConnectedCount++;
                    }

                    await Task.Delay(1); // <=========================== 루프 통신 딜레이
                }
                catch (Exception ex)
                {
                    LogExceptionMessage(ex);
                }
            }

        }

        private void LogExceptionMessage(Exception ex)
        {
            string message = ex.InnerException?.Message ?? ex.Message;
            Debug.WriteLine(message);
            ElevatorEventlogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
            main.ACS_UI_Log(message);
        }

        private async Task<bool> SendRecvAsync()
        {
            //ILog("ip = " + PlcIpAddress);
            byte[] sendData = MakeSendingData();

            try
            {
                using (var client = new TcpClient())
                {
                    var cancelTask = Task.Delay(1000); // <=========================== 연결타임아웃 시간
                    var connectTask = client.ConnectAsync(ConfigData.sElevator_IP_Address_SV, Convert.ToInt32(ConfigData.sElevator_PortNumber));

                    //double await so if cancelTask throws exception, this throws it
                    await await Task.WhenAny(connectTask, cancelTask);
                    if (cancelTask.IsCompleted)
                    {
                        //throw new TimeoutException("connection time out");
                        ElevatorEventlogger.Info("Elevator SendRecvAsync() : connection time out");

                        return false;
                    }

                    using (var stream = client.GetStream())
                    {
                        var elevatorrunsignel = uow.ElevatorState.Find(m => m.ElevatorMissionName != null).FirstOrDefault();
                        var elevatorMode = uow.ACSModeInfo.Find(n => n.Location == "Elevator" && n.ElevatorMode != null).FirstOrDefault();

                        String response = String.Empty;
                        byte[] recvBuff = new Byte[1024];
                        int recvLength = 0;

                        stream.ReadTimeout = 1000; // <=========================== 수신타임아웃 시간
                        // send message
                        stream.Write(sendData, 0, sendData.Length);


                        string message = Encoding.ASCII.GetString(sendData);

                        if (elevatorrunsignel != null) ElevatorEventlogger.Info($"ElevaotrStatus: {elevatorrunsignel.ElevatorState.ToString()}");
                        else if (elevatorrunsignel == null && elevatorMode != null) ElevatorEventlogger.Info($"ElevaotrStatus: {elevatorMode.ElevatorMode.ToString()}");
                        ElevatorEventlogger.Info($"Elevator Sent: {message}");

                        sendData = null;

                        //
                        // recv response
                        while ((recvLength = stream.Read(recvBuff, 0, recvBuff.Length)) != 0)
                        {
                            response += Encoding.ASCII.GetString(recvBuff, 0, recvLength);

                            if (response.IndexOf("\r\n") != -1) // ETX 수신시 루프 탈출
                                break;

                        }
                        ElevatorEventlogger.Info($"Elevator Recv: {response}");
                        if (response.Length > 0)
                            return MakeRecvData(recvBuff);
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);

                ElevatorEventlogger.Info(ex.InnerException?.Message ?? ex.Message);
                ElevatorEventlogger.Info(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);
            }
            return false;
        }
        private byte[] MakeSendingData()
        {
            string sendMsg = "";

            string fmt = "00000000";
            string sendMsgLength = string.Empty;
            string sendData = string.Empty;

            //elevator 상태값으로 하여 보낼 데이터 선택하기
            sendMsg = SendingElevator_Control();
            sendMsgLength = sendMsg.Length.ToString(fmt);
            sendData = sendMsgLength + sendMsg;
            return SendingServerStream(sendData);
        }

        private bool MakeRecvData(byte[] data)
        {
            string recvDataLength = string.Empty; //EnCodig Data
            int recvLengthIndex = 8;        //index 0~7까지 8개의 객체가 객체 수량임
            bool returnValue = false;
            string strBuffer = string.Empty;
            try
            {
                //Recv Data 객체 수량을 확인 
                //Recv Data 중 index 0~7까지 8개의 객체가 객체 수량임
                recvDataLength += Encoding.ASCII.GetString(data, 0, recvLengthIndex);

                //Recv Data 객체 추출
                //index 7~n 객체 수량까지가 사용할 Data
                strBuffer += Encoding.ASCII.GetString(data, recvLengthIndex, Convert.ToInt32(recvDataLength));

                if (Convert.ToInt32(recvDataLength) == strBuffer.Length)
                {
                    RecvElevator_Control(strBuffer);
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);

                ElevatorEventlogger.Info(ex.InnerException?.Message ?? ex.Message);
                ElevatorEventlogger.Info(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);
            }
            return returnValue;
        }

        private string SendingElevator_Control()
        {
            string SendMsg = "";

            var mirstateElevator = uow.ElevatorState.Find(m => m.ElevatorMissionName != null).FirstOrDefault();
            if (mirstateElevator != null)
            {
                switch (mirstateElevator.ElevatorState)
                {
                    //Cmd = 상태요청번호 Aid = ACS장비ID Did = 해당호기 사용시 해당호기 입력(0)일경우 전체호기
                    //case "Status":
                    //    SendMsg = "Cmd=10&AId=1&DId=1";
                    //    break;

                    //case "MiRContorlSignal":
                    //    //MiR 제어 요청 신호
                    //    //Param = 09 AGV운전 명령 , Data = 1이면 제어 , Dest = 00으로 고정
                    //    SendMsg = "Cmd=20&AId=1&DId=1&Param=09&Data=01&Dest=00";
                    //    break;

                    case "CallStartFloor":
                        if (mirstateElevator.ElevatorMissionName.Contains("Elevator_Up"))
                        {
                            //[목적지층이 Up일때]Elevator Call요청 신호(목적지 층도 함께 입력해야함)
                            //Param = 01 승강장 호출 , Data = 부름층 , Dest = 목적지층(외부 up/Down 스위치로 생각하면됨)
                            //현재 1층에서 2층으로 가는 SendMsg 로 입력해둠(Test)
                            //최하층이 1이기때문에 내가 원하는 층에 1을 더해야한다
                            SendMsg = "Cmd=20&AId=1&DId=1&Param=03&Data=04&Dest=04";
                        }
                        else
                        {
                            //[목적지층이 Down일때]Elevator Call요청 신호(목적지 층도 함께 입력해야함)
                            //Param = 01 승강장 호출 , Data = 부름층 , Dest = 목적지층(외부 up/Down 스위치로 생각하면됨)
                            //현재 1층에서 2층으로 가는 SendMsg 로 입력해둠(Test)
                            //최하층이 1이기때문에 내가 원하는 층에 1을 더해야한다

                            SendMsg = "Cmd=20&AId=1&DId=1&Param=03&Data=07&Dest=07";
                        }
                        break;

                    case "CallStartDoorOpen":
                        //Elevator Call요청 층에서 Door Open 신호
                        //Param : 5 -Front Door Open 명령 , Data, Dest : 0으로 고정
                        SendMsg = "Cmd=20&AId=1&DId=1&Param=05&Data=00&Dest=00";
                        break;

                    case "CallEndFloorSelect":
                        //[목적지층이 Up일때]Elevator 목적지층 전달 신호
                        if (mirstateElevator.ElevatorMissionName.Contains("Elevator_Up"))
                            //Param = 03 승강장 호출 , Data = 목적지층 , Dest = 목적지층(외부 up/Down 스위치로 생각하면됨)
                            //현재 2층으로 목적지층 설정(Test)
                            //목적지층을 선택하는것이기때문에 Data와 Dest가 같아야한다
                            SendMsg = "Cmd=20&AId=1&DId=1&Param=03&Data=07&Dest=07";
                        else
                            //[목적지층이 Down일때]Elevator 목적지층 전달 신호
                            //Param = 03 승강장 호출 , Data = 목적지층 , Dest = 목적지층(외부 up/Down 스위치로 생각하면됨)
                            //현재 2층으로 목적지층 설정(Test)
                            //목적지층을 선택하는것이기때문에 Data와 Dest가 같아야한다

                            SendMsg = "Cmd=20&AId=1&DId=1&Param=03&Data=04&Dest=04";
                        break;

                    case "CallStartDoorClose":
                        //Elevator Door Close 신호
                        SendMsg = "Cmd=20&AId=1&DId=1&Param=06&Data=00&Dest=00";
                        break;

                    case "CallStartFloorStatus":
                        //Elevator Door Close 신호
                        SendMsg = "Cmd=10&AId=1&DId=1";
                        break;

                    case "CallEndFloorStatus":
                        //Elevator Door Close 신호
                        SendMsg = "Cmd=10&AId=1&DId=1";
                        break;

                    case "CallEndDoorOpen":
                        //목적지층 도착후 Door Open 신호
                        SendMsg = "Cmd=20&AId=1&DId=1&Param=05&Data=00&Dest=00";
                        break;


                    case "CallEndDoorClose":
                        //목적지층 도착후 MiR 진출완료 후 DoorClose 신호
                        SendMsg = "Cmd=20&AId=1&DId=1&Param=06&Data=00&Dest=00";
                        break;

                        //case "MiRUnContorlSignal":
                        //    //MiR 운전 제어 해제 신호
                        //    SendMsg = "Cmd=20&AId=1&DId=1&Param=09&Data=00&Dest=00";
                        //    break;
                }

            }
            else
            {
                //진행중인 미션을 다 마무리 후 모드 변경 프로토콜 전송한다
                var missionName = uow.Missions.Find(m => m.MissionName.Contains("Elevator")).FirstOrDefault();
                var elevatorContorlMode = uow.ACSModeInfo.Find(m => m.Location == "Elevator" && m.ACSMode != null).FirstOrDefault();
                if (missionName == null && elevatorContorlMode.ACSMode == "MiRUnContorlMode")
                {
                    //"MiRUnContorlMode":
                    SendMsg = "Cmd=20&AId=1&DId=1&Param=09&Data=00&Dest=00";

                }
                else
                {
                    // "MiRContorlMode":
                    SendMsg = "Cmd=20&AId=1&DId=1&Param=09&Data=01&Dest=00";
                }
            }

            return SendMsg;
        }

        int elevatorOpenRetry = 0;
        private void RecvElevator_Control(string recvMsg)
        {
            //Recv데이터를 ElevatorModel 변수에 넣기
            GetElevatorModelData(recvMsg);

            // 1.상시 엘리베이터 상태 요청
            // 2.상시 엘리베이터 상태 응답
            // 2.MiR 운전 제어 요청
            // 3.MiR 운전 제어 응답
            // 4.출발층 Elevator Call요청
            // 5.출발층 Elevator Call요청 응답
            // 6.출발층 으로 Elevator 도착시까지 상태요청
            // 7.출발층 Elevator 상태 응답중 Floor / Door / Dir /Hall_Up및 Hall_Dn 신호로 도착확인
            // 8.출발층 Elevator 도착 후 MiR 진입 신호 전달
            // 9.출발층 Elevator MiR 진입 완료까지 door Open 신호 요청
            // 10.출발층 MiR 진입완료후 목적지층으로 이동 요청 신호
            // 11.출발층 Elevator 목적지층 이동 요청신호 응답
            // 12.출발층 Elevator Door Close 요청 신호
            // 13.출발층 Elevator Door Close 요청 응답
            // 14.목적지층 으로 Elevator 도착시까지 상태요청
            // 15.목적지층 Elevator 상태 응답중 Floor / Door / Dir /Hall_Up및 Hall_Dn 신호로 도착확인
            // 16.목적지층 Elevator MiR 진출 완료까지 Door Open 신호 요청
            // 17.목적지층 MiR 진출 완료후 Door Close요청
            // 18.MiR 운전 제어 해제 신호
            // 19.MiR 운전 제어 해제 신호 응답

            //if (elevators.Status == 2 || elevators.Status == 9)

            {
                var mirstateElevator = uow.ElevatorState.Find(m => m.ElevatorMissionName != null).FirstOrDefault();
                if (mirstateElevator != null)
                {
                    switch (mirstateElevator.ElevatorState)
                    {
                        //case "Status":
                        //    {
                        //        //엘리베이터 상태가 MiR 제어 신호가 들어올경우
                        //        var elevatorMode = uow.ElevatorState.GetByElevatorMode();
                        //        if (elevatorMode.ElevatorMode == ElevatorMode.MiRContorlMode.ToString())    //Elevator Mode 변경 버튼으로 사람이 변경한다.
                        //        {
                        //            //MiR 상태가 엘리베이터를 부를경우 엘리베이터 상태를 변경한다.
                        //            mirstateElevator.ElevatorState = ElevatorState.CallStartFloor.ToString();
                        //            uow.ElevatorState.Update(mirstateElevator);
                        //        }
                        //    }
                        //    break;

                        //case "MiRContorlSignal":
                        //    //MiR 운전 제어 응답
                        //    if (elevators.Param == 9 && elevators.Cmd == 21 && elevators.Dest == 0 && elevators.Result == "ok")
                        //    {
                        //        mirstateElevator.ElevatorState = ElevatorState.CallStartFloor.ToString();   //[목적지층이 Down일때]
                        //        uow.ElevatorState.Update(mirstateElevator);

                        //    }
                        //    break;

                        case "CallStartFloor":
                            //[목적지층이 Up일때] 출발층 Elevator Call요청 응답
                            if (elevators.Param == 3 && elevators.Cmd == 21 && elevators.Result == "ok")
                            {
                                //출발층까지 도착시 까지 상태를 확인한다.
                                mirstateElevator.ElevatorState = ElevatorState.CallStartFloorStatus.ToString();
                                uow.ElevatorState.Update(mirstateElevator);
                            }
                            break;

                        case "CallStartFloorStatus":
                            {

                                //   bool StartFloor_protocol = elevators.Status == 2 && elevators.Dld == 1 && elevators.Dir == 0
                                //&& (elevators.Door == 2 || elevators.Door == 3) && elevators.car_f == 0 && elevators.car_r == 0 && elevators.Hallup_f == 0 && elevators.Hallup_r == 0
                                //&& elevators.HallDn_f == 0 && elevators.HallDn_r == 0 && elevators.ErrCode == 0;

                                bool StartFloor_protocol = elevators.Status == 2 && elevators.Dld == 1 && elevators.Dir == 0
                                                          && elevators.car_f == 0 && elevators.car_r == 0 && elevators.Hallup_f == 0 && elevators.Hallup_r == 0
                                                          && elevators.HallDn_f == 0 && elevators.HallDn_r == 0 && elevators.ErrCode == 0;

                                //출발층 도착확인 상태값이 으로 판단하기 오류가 많으므로 Elevator MiRContorlSignal진행했을시에만 하게한다.
                                //if (mirstateElevator.ElevatorMissionName == "Elevator_Up" && elevators.Floor == 4 && (elevators.Hallup_f == 0 || elevators.HallDn_f == 0)
                                //    && elevators.Door == 2 && elevators.Dld == 1)

                                //엘리베이터 통신 끈키는 증상발견으로 해당층에 도착후 엘리베이터가 움직이는상태가 아니고 Door닫혀있으면 10번이상확인후 Door Open신호를 던진다 [2023-03-07]
                                //엘리베이터 업체와 프로토콜이 정해지거나 엘리베이터 수정후 다시 수정해야함 [2023-03-07]
                                if (StartFloor_protocol)
                                {
                                    if (mirstateElevator.ElevatorMissionName.Contains("Elevator_Up") && elevators.Floor == 4)

                                    {
                                        if (elevators.Door == 2 || elevatorOpenRetry > 10)
                                        {
                                            mirstateElevator.ElevatorState = ElevatorState.CallStartDoorOpen.ToString();
                                            uow.ElevatorState.Update(mirstateElevator);
                                            elevatorOpenRetry = 0;
                                        }
                                        else if (elevators.Door == 3) elevatorOpenRetry++;

                                        else elevatorOpenRetry = 0;

                                    }

                                    //else if (mirstateElevator.ElevatorMissionName == "Elevator_Down" && elevators.Floor == 7 && (elevators.Hallup_f == 0 || elevators.HallDn_f == 0)
                                    //       && elevators.Door == 2 && elevators.Dld == 1) 

                                    //엘리베이터 통신 끈키는 증상발견으로 해당층에 도착후 엘리베이터가 움직이는상태가 아니고 Door닫혀있으면 10번이상확인후 Door Open신호를 던진다 [2023-03-07]
                                    //엘리베이터 업체와 프로토콜이 정해지거나 엘리베이터 수정후 다시 수정해야함 [2023-03-07]
                                    else if (mirstateElevator.ElevatorMissionName.Contains("Elevator_Down") && elevators.Floor == 7)
                                    {
                                        if (elevators.Door == 2 || elevatorOpenRetry > 10)
                                        {
                                            mirstateElevator.ElevatorState = ElevatorState.CallStartDoorOpen.ToString();
                                            uow.ElevatorState.Update(mirstateElevator);
                                            elevatorOpenRetry = 0;
                                        }
                                        else if (elevators.Door == 3) elevatorOpenRetry++;

                                        else elevatorOpenRetry = 0;
                                    }
                                    else elevatorOpenRetry = 0;
                                }
                                else elevatorOpenRetry = 0;
                            }
                            break;

                        case "CallStartDoorOpen":
                            //MiR 진입완료 전까지 DoorOpen
                            if (mirstateElevator.MiRStateElevator == MiRStateElevator.MiRStateElevatorLoaderComplet.ToString())
                            {
                                //MiR Elevator 진입했을경우
                                //목적지층으로 요청
                                mirstateElevator.ElevatorState = ElevatorState.CallEndFloorSelect.ToString();  //[목적지층이 Down일때]
                                uow.ElevatorState.Update(mirstateElevator);

                            }
                            else
                            {   //MiR 진입 신호
                                if (mirstateElevator.MiRStateElevator != MiRStateElevator.MiRStateElevatorLoaderStart.ToString())
                                {
                                    mirstateElevator.MiRStateElevator = MiRStateElevator.MiRStateElevatorLoaderStart.ToString();
                                    uow.ElevatorState.Update(mirstateElevator);
                                }

                            }
                            break;

                        case "CallEndFloorSelect":
                            //[목적지층이 Up일때]출발층 Elevator 목적지선택 요청
                            if (elevators.Param == 3 && elevators.Cmd == 21 && elevators.Result == "ok")
                            {
                                //출발층 도어 닫기
                                mirstateElevator.ElevatorState = ElevatorState.CallStartDoorClose.ToString();
                                uow.ElevatorState.Update(mirstateElevator);
                            }
                            break;

                        case "CallStartDoorClose":
                            //출발층 Elevator Door Close 요청 신호 응답
                            if (elevators.Param == 6 && elevators.Cmd == 21 && elevators.Result == "ok")
                            {
                                //목적지층 도착시까지 Elevator 상태값을 요청한다.
                                mirstateElevator.ElevatorState = ElevatorState.CallEndFloorStatus.ToString();
                                uow.ElevatorState.Update(mirstateElevator);
                            }
                            break;

                        case "CallEndFloorStatus":

                            //bool EndFloorStatus_protocol = elevators.Status == 2 && elevators.Dld == 1 && elevators.Dir == 0
                            //   && (elevators.Door == 2 || elevators.Door == 3) && elevators.car_f == 0 && elevators.car_r == 0 && elevators.Hallup_f == 0 && elevators.Hallup_r == 0
                            //   && elevators.HallDn_f == 0 && elevators.HallDn_r == 0 && elevators.ErrCode == 0;

                            bool EndFloorStatus_protocol = elevators.Status == 2 && elevators.Dld == 1 && elevators.Dir == 0
                               && elevators.car_f == 0 && elevators.car_r == 0 && elevators.Hallup_f == 0 && elevators.Hallup_r == 0
                               && elevators.HallDn_f == 0 && elevators.HallDn_r == 0 && elevators.ErrCode == 0;

                            //목적지층 도착확인 
                            //if (mirstateElevator.ElevatorMissionName == "Elevator_Up" && elevators.Floor == 7 && (elevators.Hallup_f == 0 || elevators.HallDn_f == 0)
                            //        && elevators.Door == 2 && elevators.Dld == 1)

                            //엘리베이터 통신 끈키는 증상발견으로 해당층에 도착후 엘리베이터가 움직이는상태가 아니고 Door닫혀있으면 10번이상확인후 Door Open신호를 던진다 [2023-03-07]
                            //엘리베이터 업체와 프로토콜이 정해지거나 엘리베이터 수정후 다시 수정해야함 [2023-03-07]
                            if (EndFloorStatus_protocol)
                            {
                                if (mirstateElevator.ElevatorMissionName.Contains("Elevator_Up") && elevators.Floor == 7)
                                {
                                    if (elevators.Door == 2 || elevatorOpenRetry > 10)
                                    {
                                        mirstateElevator.ElevatorState = ElevatorState.CallEndDoorOpen.ToString();
                                        uow.ElevatorState.Update(mirstateElevator);
                                        elevatorOpenRetry = 0;
                                    }
                                    else if (elevators.Door == 3) elevatorOpenRetry++;
                                    else elevatorOpenRetry = 0;

                                }


                                //else if (mirstateElevator.ElevatorMissionName == "Elevator_Down" && elevators.Floor == 4 && (elevators.Hallup_f == 0 || elevators.HallDn_f == 0)
                                //       && elevators.Door == 2 && elevators.Dld == 1)

                                //엘리베이터 통신 끈키는 증상발견으로 해당층에 도착후 엘리베이터가 움직이는상태가 아니고 Door닫혀있으면 10번이상확인후 Door Open신호를 던진다 [2023-03-07]
                                //엘리베이터 업체와 프로토콜이 정해지거나 엘리베이터 수정후 다시 수정해야함 [2023-03-07]
                                else if (mirstateElevator.ElevatorMissionName.Contains("Elevator_Down") && elevators.Floor == 4)
                                {
                                    if (elevators.Door == 2 || elevatorOpenRetry > 10)
                                    {
                                        mirstateElevator.ElevatorState = ElevatorState.CallEndDoorOpen.ToString();
                                        uow.ElevatorState.Update(mirstateElevator);
                                        elevatorOpenRetry = 0;
                                    }
                                    else if (elevators.Door == 3) elevatorOpenRetry++;
                                    else elevatorOpenRetry = 0;
                                }
                                else elevatorOpenRetry = 0;

                            }
                            else elevatorOpenRetry = 0;

                            break;

                        case "CallEndDoorOpen":
                            //목적지층 도착시 DoorOpen변경
                            if (mirstateElevator.MiRStateElevator == MiRStateElevator.MiRStateElevatorUnLoaderComplet.ToString())
                            {
                                //MiR Elevator 진출이 완료되었을때 DoorClose 요청
                                //목적지층 도착했을경우
                                mirstateElevator.ElevatorState = ElevatorState.CallEndDoorClose.ToString();
                                uow.ElevatorState.Update(mirstateElevator);
                            }
                            else
                            {
                                //MiR 진출 신호
                                if (mirstateElevator.MiRStateElevator != MiRStateElevator.MiRStateElevatorUnLoaderStart.ToString())
                                {
                                    mirstateElevator.MiRStateElevator = MiRStateElevator.MiRStateElevatorUnLoaderStart.ToString();
                                    uow.ElevatorState.Update(mirstateElevator);
                                }
                            }
                            break;

                        case "CallEndDoorClose":
                            //목적지층 DoorClose 요청 신호 응답
                            if (elevators.Param == 6 && elevators.Cmd == 21 && elevators.Result == "ok")
                            {
                                //완료
                                uow.ElevatorState.Remove(mirstateElevator); //MiR 운전 제어 해제 신호

                                //mirstateElevator.ElevatorState = ElevatorState.MiRUnContorlSignal.ToString();
                                //uow.ElevatorState.Update(mirstateElevator);
                            }
                            break;

                            //case "MiRUnContorlSignal":
                            //    //MiR 운전 제어 해제 신호 응답
                            //    if (elevators.Param == 9 && elevators.Cmd == 21 && elevators.Data == 0 && elevators.Result == "ok")
                            //    {
                            //        //완료
                            //        uow.ElevatorState.Remove(mirstateElevator);
                            //    }
                            //    break;
                    }
                }
                else
                {
                    var missionName = uow.Missions.Find(m => m.MissionName.Contains("Elevator")).FirstOrDefault();
                    var elevatorMode = uow.ACSModeInfo.Find(m => m.Location == "Elevator" && m.ElevatorMode != null).FirstOrDefault();

                    if (missionName == null && elevatorMode != null)
                    {
                        //"MiRContorlSignal":
                        //    //MiR 운전 제어 응답
                        if (elevatorMode.ACSMode == "MiRContorlMode" && elevators.Param == 9 && elevators.Cmd == 21 && elevators.Dest == 0 && elevators.Result == "ok")
                        {
                            elevatorMode.ElevatorMode = "AGVMode";   // AGV 모드

                            uow.ACSModeInfo.Update(elevatorMode);
                        }
                        else
                        {
                            elevatorMode.ElevatorMode = "NotAGVMode";   // NotAGV 모드
                            uow.ACSModeInfo.Update(elevatorMode);

                        }
                        ElevatorMode?.Invoke(this, elevatorMode.ElevatorMode);
                    }
                    else ElevatorMode?.Invoke(this, elevatorMode.ElevatorMode);



                }

            }
        }

        private void GetElevatorModelData(string recvMsg)
        {
            //"&","^"두개의 문자 잘라서 배열로 반환한다
            string[] splitData = recvMsg.Split(new string[] { "&", "^", "\r\n" }, StringSplitOptions.None);

            foreach (var Data in splitData)
            {

                if (Data.Contains("Cmd="))
                {
                    //Data에 해당 문자가 있으면 초기화 후 진행
                    //문자 바꾸기 하여 Cmd=문자를 ""빈문자로 변경
                    elevators.Cmd = 0;
                    elevators.Cmd = Convert.ToInt32(Data.Replace("Cmd=", ""));
                    continue;
                }
                else if (Data.Contains("AId="))
                {
                    elevators.Ald = 0;
                    elevators.Ald = Convert.ToInt32(Data.Replace("AId=", ""));
                    continue;
                }
                else if (Data.Contains("Count="))
                {
                    elevators.Count = 0;
                    elevators.Count = Convert.ToInt32(Data.Replace("Count=", ""));
                    continue;
                }
                else if (Data.Contains("DId="))
                {
                    elevators.Dld = 0;
                    elevators.Dld = Convert.ToInt32(Data.Replace("DId=", ""));
                    continue;
                }
                else if (Data.Contains("Status="))
                {
                    elevators.Status = 0;
                    elevators.Status = Convert.ToInt32(Data.Replace("Status=", ""));
                    continue;
                }
                else if (Data.Contains("Floor="))
                {
                    elevators.Floor = 0;
                    elevators.Floor = Convert.ToInt32(Data.Replace("Floor=", ""));
                    continue;
                }
                else if (Data.Contains("Dir="))
                {
                    elevators.Dir = 0;
                    elevators.Dir = Convert.ToInt32(Data.Replace("Dir=", ""));
                    continue;
                }
                else if (Data.Contains("Door="))
                {
                    elevators.Door = 0;
                    elevators.Door = Convert.ToInt32(Data.Replace("Door=", ""));
                    continue;
                }
                else if (Data.Contains("car_f="))
                {
                    elevators.car_f = 0;
                    elevators.car_f = Convert.ToInt32(Data.Replace("car_f=", ""));
                    continue;
                }
                else if (Data.Contains("car_r="))
                {
                    elevators.car_r = 0;
                    elevators.car_r = Convert.ToInt32(Data.Replace("car_r=", ""));
                    continue;
                }
                else if (Data.Contains("Hallup_f="))
                {
                    elevators.Hallup_f = 0;
                    elevators.Hallup_f = Convert.ToInt32(Data.Replace("Hallup_f=", ""));
                    continue;
                }
                else if (Data.Contains("Hallup_r="))
                {
                    elevators.Hallup_r = 0;
                    elevators.Hallup_r = Convert.ToInt32(Data.Replace("Hallup_r=", ""));
                    continue;
                }
                else if (Data.Contains("HallDn_f="))
                {
                    elevators.HallDn_f = 0;
                    elevators.HallDn_f = Convert.ToInt32(Data.Replace("HallDn_f=", ""));
                    continue;
                }
                else if (Data.Contains("HallDn_r="))
                {
                    elevators.HallDn_r = 0;
                    elevators.HallDn_r = Convert.ToInt32(Data.Replace("HallDn_r=", ""));
                    continue;
                }
                else if (Data.Contains("ErrCode="))
                {
                    elevators.ErrCode = 0;
                    elevators.ErrCode = Convert.ToInt32(Data.Replace("ErrCode=", ""));
                    continue;
                }
                else if (Data.Contains("Param="))
                {
                    elevators.Param = 0;
                    elevators.Param = Convert.ToInt32(Data.Replace("Param=", ""));
                    continue;
                }
                else if (Data.Contains("Data="))
                {
                    elevators.Data = 0;
                    elevators.Data = Convert.ToInt32(Data.Replace("Data=", ""));
                    continue;
                }
                else if (Data.Contains("Dest="))
                {
                    elevators.Dest = 0;
                    elevators.Dest = Convert.ToInt32(Data.Replace("Dest=", ""));
                    continue;
                }
                else if (Data.Contains("Result="))
                {
                    elevators.Result = "";
                    elevators.Result = Data.Replace("Result=", "");
                    continue;
                }
            }
        }

        private byte[] SendingServerStream(string strBuffer)//Sending Data 변환
        {
            byte[] tempBuff = Encoding.ASCII.GetBytes(strBuffer);
            byte[] sendData = null;
            using (var ms = new MemoryStream())
            {
                ms.Write(tempBuff, 0, tempBuff.Length);
                sendData = ms.ToArray();
            }
            return sendData;
        }


        private void Log(string comment)
        {
            // Console.WriteLine(comment);          
            //logger.InfoFormat("{0:00}, {1}", ButtonIndex, comment);
        }


    }
}
