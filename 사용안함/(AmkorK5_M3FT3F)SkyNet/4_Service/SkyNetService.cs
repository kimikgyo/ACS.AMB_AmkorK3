using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    class SkyNetService
    {
        private readonly static ILog SkyNetEventlogger = LogManager.GetLogger("SkyNetEvent");
        private readonly MainForm main;
        private readonly IUnitOfWork uow;

        int skyNetAliveConnectWriteSignal = 1;
        int SkyNet_SM_Alive_StopwatchTest = 1;
        public SkyNetService(MainForm main, IUnitOfWork uow)
        {
            this.main = main;
            this.uow = uow;
        }

        public void Start()
        {
            Task.Run(() => Loop());
        }

        private void Loop()
        {

            Stopwatch SkyNet_SM_Alive_Stopwatch = new Stopwatch();
            SkyNet_SM_Alive_Stopwatch.Start();

            Stopwatch SkyNet_SM_Charge_Stopwatch = new Stopwatch();
            SkyNet_SM_Charge_Stopwatch.Start();

            while (true)
            {
                //try
                {
                    if (SkyNet_SM_Alive_Stopwatch.ElapsedMilliseconds >= 60000)
                    {    //3.통신확인:
                         //Skynet_SM_Alive(string linecode, string processcode, string equipmentid, int nAlive)
                         //예제) 1분마다 간격으로 0과 1을 업데이트해준다
                         //Skyent_SM_Alive(“AB12345”,”1000”,”AMT1”, 0);
                         //Skyent_SM_Alive(“AB12345”,”1000”,”AMT1”, 1);

                        skyNetAliveConnectWriteSignal = 1 - skyNetAliveConnectWriteSignal;
                        Console.WriteLine("Skyent_SM_Alive_Stopwatch = [OK]" + SkyNet_SM_Alive_Stopwatch.ElapsedMilliseconds + "/" + DateTime.Now);
                        Console.WriteLine($"skyNetAliveConnectWriteSignal = {skyNetAliveConnectWriteSignal}");
                        SkyNetEventlogger.Info("Skyent_SM_Alive_Stopwatch = [OK]" + SkyNet_SM_Alive_Stopwatch.ElapsedMilliseconds + "/" + DateTime.Now);
                        SkyNetEventlogger.Info($"skyNetAliveConnectWriteSignal = {skyNetAliveConnectWriteSignal}");
                        SkyNet_SM_Alive_Stopwatch.Reset();
                        SkyNet_SM_Alive_Stopwatch.Start();
                    }
                    if (SkyNet_SM_Charge_Stopwatch.ElapsedMilliseconds >= 180000)
                    {
                        Skynet_SM_Run_ChargeUpdate();
                        SkyNet_SM_Charge_Stopwatch.Reset();
                        SkyNet_SM_Charge_Stopwatch.Start();
                    }

                    Skynet_SM_Run();
                    Skynet_Position();
                    Skynet_Alarm();
                }
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                //    Console.WriteLine(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);
                //    SkyNetEventlogger.Info(ex.InnerException?.Message ?? ex.Message);
                //    SkyNetEventlogger.Info(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);
                //}

            }
            Thread.Sleep(500);
        }





        private void Skynet_SM_Run()     //1.물류 이송정보
        {
            string skyNetMode = "SkyNet_SM_Run";
            string linecode = "";
            string processcode = "";

            foreach (var robot in uow.Robots.GetAll())
            {
                //MiR 전원이 꺼져있을경우
                if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                    continue;
                //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                    continue;

                if (robot.ACSRobotGroup != "None")
                {
                    var SkyNet_Sm_Run = uow.SkyNets.Find(R => R.SkyNetMode == skyNetMode && R.RobotName == robot.RobotName).SingleOrDefault();

                    //SkyNet_SM_Run 모드에서 Robot정보가 없으면추가해준다
                    if (SkyNet_Sm_Run == null)
                    {
                        var newskyNetModelsAdd = new SkyNetModel
                        {
                            SkyNetMode = skyNetMode,
                            Linecode = linecode,
                            Processcode = processcode,
                            RobotName = robot.RobotName,
                            RobotState = robot.StateText,
                            MissionName = "",
                            MissionState = ""
                        };
                        uow.SkyNets.Add(newskyNetModelsAdd);
                        Console.WriteLine($"Skynet_SM_Run_Add = {newskyNetModelsAdd}");
                        SkyNetEventlogger.Info($"Skynet_SM_Run_Add = {newskyNetModelsAdd}");
                    }
                    else
                    {
                        //var runMissions = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState != "Done"
                        //                                    && m.RobotName == SkyNet_Sm_Run.RobotName
                        //                                    && m.MissionState != SkyNet_Sm_Run.MissionState
                        //                                    && m.MissionName != SkyNet_Sm_Run.MissionName).SingleOrDefault();

                        
                        var runMissions = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState != "Done"
                                                         && m.RobotName == SkyNet_Sm_Run.RobotName).SingleOrDefault();


                        //진행중인 미션이 있을때
                        if (runMissions != null)
                        {
                            if (runMissions.MissionState != SkyNet_Sm_Run.MissionState)
                            {
                                SkyNet_Sm_Run.MissionName = runMissions.MissionName;
                                SkyNet_Sm_Run.MissionState = runMissions.MissionState;

                                uow.SkyNets.Update(SkyNet_Sm_Run);

                                if (SkyNet_Sm_Run.MissionState == "Executing")
                                {
                                    Console.WriteLine($"이송시작 / RobotName = {robot.RobotName} / Move / MissionName = {SkyNet_Sm_Run.MissionName}");
                                    SkyNetEventlogger.Info($"이송시작 / RobotName = {robot.RobotName} / Move / MissionName = {SkyNet_Sm_Run.MissionName}");
                                }
                            }
                        }
                        //진행중인 미션이 없을때
                        else
                        {
                            if (SkyNet_Sm_Run.MissionName.Length > 0)
                            {
                                //Skyent_SM_Run(“AB12345”,”1000”,”AMT1”,”Complete”,”출발지”,”목적지”); //이송 완료.
                                Console.WriteLine($"이송완료 / RobotName = {SkyNet_Sm_Run.RobotName} / Conplete / MissionName = {SkyNet_Sm_Run.MissionName}");
                                SkyNetEventlogger.Info($"이송완료 / RobotName = {SkyNet_Sm_Run.RobotName} / Conplete / MissionName = {SkyNet_Sm_Run.MissionName}");

                                SkyNet_Sm_Run.MissionName = "";
                                SkyNet_Sm_Run.MissionState = "";
                                uow.SkyNets.Update(SkyNet_Sm_Run);

                                //MiR 상태 변경시 UpDate
                                //else
                                //{
                                //    SkyNet_Sm_Run.RobotName = Robot.RobotName;
                                //    SkyNet_Sm_Run.RobotState = Robot.StateText;

                                //    uow.SkyNets.Update(SkyNet_Sm_Run);

                                //    //Skyent_SM_Run(“AB12345”,”1000”,”AMT1”,”Ready”); //이송 대기
                                //    Console.WriteLine($"이송대기 /RobotName = {SkyNet_Sm_Run.RobotName} / State = {SkyNet_Sm_Run.RobotState} ");
                                //    SkyNetEventlogger.Info($"이송대기 /RobotName = {SkyNet_Sm_Run.RobotName} / State = {SkyNet_Sm_Run.RobotState} ");
                                //}

                            }
                            //else continue;
                        }
                    }

                }

            }
        }


        private void Skynet_SM_Run_ChargeUpdate()  //2.충전정보
        {
            string skyNetMode = "SkyNet_SM_Run";
            string linecode = "";
            string processcode = "";

            foreach (var robot in uow.Robots.GetAll())
            {
                if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                    continue;
                //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                    continue;

                if (robot.ACSRobotGroup != "None")
                {
                    var SkyNet_Sm_Run_ChargeUpdate = uow.SkyNets.Find(R => R.SkyNetMode == skyNetMode && R.RobotName == robot.RobotName).SingleOrDefault();

                    if (SkyNet_Sm_Run_ChargeUpdate != null)
                    {
                        var runMissions = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState != "Done"
                                         && m.RobotName == SkyNet_Sm_Run_ChargeUpdate.RobotName).SingleOrDefault();

                        if (runMissions != null && runMissions.MissionName.Contains("Charge"))
                        {

                            Console.WriteLine($"ChargeUpdate / RobotName = {robot.RobotName} / RECHARGE /BatteryPercent = {robot.BatteryPercent}");
                            SkyNetEventlogger.Info($"ChargeUpdate / RobotName = {robot.RobotName} / RECHARGE /BatteryPercent = {robot.BatteryPercent}");
                            //Skynet_SM_Run(string linecode, string processcode, string equipmentid, string remote, string departure, string arrival)
                            //예제) 3분마다 충전 정보 업데이트
                            //Skyent_SM_Run(“AB12345”,”1000”,”AMT1”,”RECHARGE”,”99”,””); //충전
                            //충전이 종료 및 취소될시 메세지 전송(이벤트)
                        }
                        else continue;
                    }
                    else continue;
                }
            }
        }

        private void Skynet_Position()      //5.위치정보
        {
            string skyNetMode = "Skynet_Position";
            string linecode = "";
            string processcode = "";

            foreach (var robot in uow.Robots.GetAll())
            {
                //MiR 전원이 꺼져있을경우
                if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                    continue;
                //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                    continue;

                if (robot.ACSRobotGroup != "None")
                {
                    var Skynet_Position = uow.SkyNets.Find(R => R.SkyNetMode == skyNetMode && R.RobotName == robot.RobotName).SingleOrDefault();

                    //SkyNet_Position 모드에서 Robot정보가 없으면추가해준다
                    if (Skynet_Position == null)
                    {
                        var newskyNetModelsAdd = new SkyNetModel
                        {
                            SkyNetMode = skyNetMode,
                            Linecode = linecode,
                            Processcode = processcode,
                            RobotName = robot.RobotName,
                            RobotState = robot.StateText
                        };
                        uow.SkyNets.Add(newskyNetModelsAdd);
                        Console.WriteLine($"Skynet_Position_Add = {newskyNetModelsAdd}");
                        SkyNetEventlogger.Info($"Skynet_Position_Add = {newskyNetModelsAdd}");
                    }
                    else
                    {
                        //로봇 상태가 변경되면 업데이트 한다.
                        if (Skynet_Position.RobotState != robot.StateText)
                        {
                            Skynet_Position.RobotName = robot.RobotName;
                            Skynet_Position.RobotState = robot.StateText;

                            uow.SkyNets.Update(Skynet_Position);


                            if (Skynet_Position.RobotState == "Ready")
                            {
                                Console.WriteLine($"포지션 상태 = IDLE / RobotName = {Skynet_Position.RobotName} / Skynet_Position_X = {robot.Position_X} / Skynet_Position_Y = {robot.Position_Y}");
                                SkyNetEventlogger.Info($"포지션 상태 = IDLE / RobotName = {Skynet_Position.RobotName} / Skynet_Position_X = {robot.Position_X} / Skynet_Position_Y = {robot.Position_Y}");
                            }
                            else if (Skynet_Position.RobotState == "Executing")
                            {
                                Console.WriteLine($"포지션 상태 = Executing / RobotName = {Skynet_Position.RobotName} / Skynet_Position_X = {robot.Position_X} / Skynet_Position_Y = {robot.Position_Y}");
                                SkyNetEventlogger.Info($"포지션 상태 = Executing / RobotName = {Skynet_Position.RobotName} / Skynet_Position_X = {robot.Position_X} / Skynet_Position_Y = {robot.Position_Y}");

                            }
                            else if (Skynet_Position.RobotState == "Done")
                            {
                                Console.WriteLine($"포지션 상태 = Complete / RobotName = {Skynet_Position.RobotName} / Skynet_Position_X = {robot.Position_X} / Skynet_Position_Y = {robot.Position_Y}");
                                SkyNetEventlogger.Info($"포지션 상태 = Complete / RobotName = {Skynet_Position.RobotName} / Skynet_Position_X = {robot.Position_X} / Skynet_Position_Y = {robot.Position_Y}");

                            }
                            else continue;
                            //Skynet_Position(string linecode, string equipmentid, string X - coordinate, string Y - coordinate, string status)
                            // 예제 ) 상태변경될때 이벤트로 전송
                            // Skyent_Position(“AB12345”,”AMR1”,”-10008”, “85555”,”MOVE”);
                            //Skyent_Position(“AB12345”,”AMR1”,”-10008”, “85555”,”ALARM”);
                            //Skyent_Position(“AB12345”,”AMR1”,”-10008”, “85555”,”IDLE”);
                            //문의사항: 업데이트 간격? 
                            //레디 퍼즈 상태를 전부 보내야하는지 ?
                        }
                        else
                        {
                            continue;
                        }

                    }

                }
                else continue;
            }
        }
        private void Skynet_Alarm()
        {
            //메세지 추가 = main.SkyNetAlarmMessageQueue.Enqueue("POP CALL ERROR 메시지 테스트")
            if (main.SkyNetAlarmMessageQueue.Count > 0)
            {
                if (main.SkyNetAlarmMessageQueue.TryDequeue(out string msg))
                {

                    //4.알람 이벤트 :
                    //Skynet_EM_DataSend(string linecode, string processcode, string equipid, string error_code, string type, string error_name, string error_descript, string error_action)

                    //예제) 현장에서 Test 진행후 맞추어 보기로함
                    //Skynet_EM_DataSend(“AB12345”,”1000”,”AMT1”,”순간 정지”,”EC0020”,”목적지 이동 실패”,”X구간 이동을 실패 하였습니다.”,”AMT 설비 위치 및 상태를 확인 하여 주십시오.”)
                }
            }
        }
    }
}





