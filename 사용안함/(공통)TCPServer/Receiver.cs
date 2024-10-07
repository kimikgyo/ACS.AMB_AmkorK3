using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class Receiver
    {
        private readonly ILog EventLogger = LogManager.GetLogger("Event");
        private readonly MainForm main;

        private readonly string[] sRequest_CallZoenNo = new string[ConfigData.CallButton_MaxNum];          //접속 요청 구역
        private readonly string[] sRequest_CallButtonIndex = new string[ConfigData.CallButton_MaxNum];     //접속 요청 CallButton 번호 
        private readonly string[] sRequest_CallButtonState = new string[ConfigData.CallButton_MaxNum];     //접속 요청 상태 (Connect/Disconnect)
        private readonly string[] sRequest_CallButtonName = new string[ConfigData.CallButton_MaxNum];      //접속 요청 상태 (Connect/Disconnect)
        private readonly DateTime[] dRequest_Call_Time = new DateTime[ConfigData.CallButton_MaxNum];

        private TcpClient client = null;
        private NetworkStream stream = null;
        private string ip = string.Empty;
        private string strBuffer = string.Empty; // Listener로 부터 받아오는 buffer   


        public Receiver(MainForm mainForm)
        {
            this.main = mainForm;

            for (int i = 0; i < ConfigData.CallButton_MaxNum; i++)
            {
                this.dRequest_Call_Time[i] = DateTime.Now;
                this.sRequest_CallZoenNo[i] = string.Empty;
                this.sRequest_CallButtonIndex[i] = string.Empty;
                this.sRequest_CallButtonState[i] = string.Empty;  //접속 요청 상태 (Connect/Disconnect)
                this.sRequest_CallButtonName[i] = string.Empty;
            }
        }

        public void Start(TcpClient clientCocket)
        {
            client = clientCocket;
            stream = client.GetStream(); // 소켓에서 메시지를 가져오는 스트림
            ip = client.Client.RemoteEndPoint.ToString();

            Task.Run(ThreadProc);
        }

        private async void ThreadProc()
        {
            try
            {
                while (client.Connected == true) //클라이언트 메시지받기
                {
                    byte[] rcvMsg = new byte[1024];

                    // 동기 처리 =================================
                    {
                        //stream.ReadTimeout = 1500; // 수신타임아웃 설정

                        int tSize = stream.Read(rcvMsg, 0, rcvMsg.Length);
                        if (tSize > 0) // 수신데이터 있음
                        {
                            TCPdataFrameParsing(rcvMsg, tSize);
                        }
                        else // 연결 끊김
                        {
                            break;
                        }
                    }

                    //// 비동기 처리 =================================
                    //{
                    //    var readTask = stream.ReadAsync(rcvMsg, 0, rcvMsg.Length);
                    //    int tSize = readTask.Result;
                    //    if (tSize > 0) // 수신데이터 있음
                    //    {
                    //        TCPdataFrameParsing(rcvMsg, tSize);
                    //    }
                    //    else // 연결 끊김
                    //    {
                    //        break;
                    //    }
                    //}

                    //// 비동기 처리 (with 타임아웃) =================================
                    //{
                    //    var readTask = stream.ReadAsync(rcvMsg, 0, rcvMsg.Length);
                    //    var timeoutTask = Task.Delay(3000); // 수신타임아웃 설정
                    //    var doneTask = await Task.WhenAny(readTask, timeoutTask).ConfigureAwait(false);
                    //    if (doneTask == timeoutTask) // 타임아웃이면
                    //    {
                    //        Log("수신타임아웃");
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        int tSize = readTask.Result;
                    //        if (tSize > 0) // 수신데이터 있음
                    //        {
                    //            TCPdataFrameParsing(rcvMsg, tSize);
                    //        }
                    //        else // 연결 끊김
                    //        {
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            // task.run 으로 실행시 여기 진입가능한지 확인하기..
            catch (IOException ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine("\n수신타임아웃!!!!!!!!!!!!!!!!!!!!!!!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
            }
            finally
            {
                if (client.Connected)
                    client.Close();

                main.TCP_Connect_Client--;
                Log("disconnected", new string(' ', 25), "연결갯수=", main.TCP_Connect_Client.ToString());
            }
        }

        private void TCPdataFrameParsing(byte[] data, int byteRead)
        {
            bool bSTX_OK = false;
            bool bETX_OK = false;
            try
            {
                for (int i = 0; i < byteRead; i++)
                {
                    //if (data[i] == 2) bSTX_OK = true;  // STX 검출
                    //else if (data[i] == 3) bETX_OK = true;  // ETX 검출
                    if (data[i] == 3) 
                        bETX_OK = true;  // ETX 검출
                }

                strBuffer += Encoding.ASCII.GetString(data, 0, byteRead); //  버퍼 누적 ( 잘려서 들어오는 경우가 있을 경우)
                Log("data recv   ", strBuffer);

                if (bETX_OK)
                {
                    if (strBuffer[0] == 2) bSTX_OK = true;
                }

                if (bSTX_OK == true && bETX_OK == true)
                {
                    strBuffer = strBuffer.Replace(Constants.STX, ' ');
                    strBuffer = strBuffer.Replace(Constants.ETX, ' ');
                    strBuffer = strBuffer.Replace(" ", "");
                    SubFuncMessageExecute(strBuffer);
                    strBuffer = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
            }
        }

        private void SendingClientStream(string strBuffer)
        {
            try
            {
                if (client.Connected)
                {
                    byte[] sndMsg = Encoding.ASCII.GetBytes(Constants.STX + strBuffer + Constants.ETX); //전송할 문자열의 시작,끝에 STX,ETX를 붙인다

                    NetworkStream ns = client.GetStream();
                    ns.Write(sndMsg, 0, sndMsg.Length);
                    ns.Flush(); // 버퍼 비움
                    Log("data send   ", Encoding.ASCII.GetString(sndMsg));

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
            }
        }

        private void SubFuncMessageExecute(string sMessageText)
        {
            try
            {
                main.ACS_UI_Log(ip);

                //EventLog("Call Button >> ACS", sMessageText);
                //mainForm.ACS_Log_Write("Call Button >> ACS :: " + sMessageText);

                //ACS 연결상태 요청 수신완료
                if (sMessageText.Contains("ACS_CONNECT,"))
                {
                    string[] sConnectName = sMessageText.Split(',');

                    int iCallZoneNo = int.Parse(sConnectName[1]);
                    int iCallButtonIndex = int.Parse(sConnectName[2]);

                    SendingClientStream("OK," + sMessageText);

                    //EventLog("ACS >> Call Button ", "OK," + sMessageText);
                    main.ACS_UI_Log("ACS >> Call Button ::" + "OK," + sMessageText);
                    main.ACS_UI_Log("client Close ip = " + ip);

                    //client.Close();
                    //Log("disconnected", "------------------------------ on Receiver.SubFuncMessageExecute");


                    for (int i = 0; i < ConfigData.CallButton_MaxNum; i++)
                    {
                        if (sRequest_CallZoenNo[i].Length > 0 && sRequest_CallZoenNo[i].Equals(iCallZoneNo.ToString()) && sRequest_CallButtonIndex[i].Equals(iCallButtonIndex.ToString()))
                        {
                            sRequest_CallButtonState[i] = "Connect";
                            dRequest_Call_Time[i] = DateTime.Now;
                            break;
                        }
                        else
                        {
                            if (sRequest_CallButtonIndex[i].Length == 0)
                            {
                                sRequest_CallZoenNo[i] = iCallZoneNo.ToString();
                                sRequest_CallButtonIndex[i] = iCallButtonIndex.ToString();
                                sRequest_CallButtonState[i] = "Connect";
                                dRequest_Call_Time[i] = DateTime.Now;
                                EventLog("CallButton Connect ", "sRequest_CallButtonIndex = " + sRequest_CallButtonIndex[i] + "sRequest_CallButtonState = " + sRequest_CallButtonState[i]);
                                break;
                            }
                        }
                    }
                }

                ////Post Mission 요청 수신완료
                //if (sMessageText.Contains("POST_MISSION,"))
                //{
                //    string[] sJobName = sMessageText.Split(',');

                //    int iCallButton_ID = int.Parse(sJobName[1]);
                //    int iCallButton_Index = int.Parse(sJobName[2]);

                //    if (sRequest_CallButtonIndex[iCallButton_ID].Length == 0 && sRequest_MissionState[iCallButton_ID].Length == 0)
                //    {
                //        sRequest_CallButtonIndex[iCallButton_ID] = sJobName[2];
                //        sRequest_MissionState[iCallButton_ID] = "Waiting";
                //        mainForm.dRequest_Call_Time[iCallButton_ID] = DateTime.Now;
                //        MainForm.SendingClientStream("OK," + sMessageText, client);
                //        EventLog("ACS >> Call Button ", "OK," + sMessageText);
                //        mainForm.ACS_Log_Write("ACS >> Call Button ::" + "OK," + sMessageText);
                //    }
                //    else //동일한 미션존재
                //    {
                //        MainForm.SendingClientStream("NG," + sMessageText, client);
                //        EventLog("ACS >> Call Button ", "NG," + sMessageText);
                //        mainForm.ACS_Log_Write("ACS >> Call Button ::" + "NG," + sMessageText);
                //    }
                //}

                ////Mission 상태 요청 수신완료
                //if (sMessageText.Contains("GET_MISSIONQUEUE,"))
                //{
                //    string[] sJobName = sMessageText.Split(',');

                //    int iCallButton_ID = int.Parse(sJobName[1]);
                //    int iCallButton_Index = int.Parse(sJobName[2]);

                //    if (sRequest_MissionState[iCallButton_ID].Length > 0)
                //    {
                //        MainForm.SendingClientStream(sRequest_MissionState[iCallButton_ID] + "," + sMessageText, client);
                //        EventLog("ACS >> Call Button ", sRequest_MissionState[iCallButton_ID] + "," + sMessageText);
                //        mainForm.ACS_Log_Write("ACS >> Call Button ::" + sRequest_MissionState[iCallButton_ID] + "," + sMessageText);
                //    }
                //    else //요청한 미션없음
                //    {
                //        MainForm.SendingClientStream("Empty," + sMessageText, client);
                //        EventLog("ACS >> Call Button ", "Empty," + sMessageText);
                //        mainForm.ACS_Log_Write("ACS >> Call Button ::" + "Empty," + sMessageText);
                //    }
                //}

                ////Mission 삭제 요청 수신완료
                //if (sMessageText.Contains("DELETE_MISSION,"))
                //{
                //    string[] sJobName = sMessageText.Split(',');

                //    int iCallButton_ID = int.Parse(sJobName[1]);
                //    int iCallButton_Index = int.Parse(sJobName[2]);

                //    if (sRequest_MissionState[iCallButton_ID].Length > 0)
                //    {
                //        sRequest_CallButtonIndex[iCallButton_ID] = sJobName[2];

                //        MainForm.SendingClientStream("OK," + sMessageText, client);
                //        EventLog("ACS >> Call Button ", "OK," + sMessageText);
                //        mainForm.ACS_Log_Write("ACS >> Call Button ::" + "OK," + sMessageText);
                //    }
                //    else
                //    {
                //        MainForm.SendingClientStream("NG," + sMessageText, client);
                //        EventLog("ACS >> Call Button ", "NG," + sMessageText);
                //        mainForm.ACS_Log_Write("ACS >> Call Button ::" + "NG," + sMessageText);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
            }
        }

        private void Log(params string[] msg)
        {
            string fullMsg = string.Join(" ", msg);

            if (fullMsg.StartsWith("연결갯수"))
                Console.WriteLine("{0:yyyy-MM-dd_HH:mm:ss} {1}", DateTime.Now, fullMsg);
            else
                Console.WriteLine("{0:yyyy-MM-dd_HH:mm:ss} {1} {2}", DateTime.Now, ip, fullMsg);
        }

        private void EventLog(string ScreenName, string Comment)
        {
            EventLogger.InfoFormat("{0}, {1}", ScreenName, Comment);
        }
    }
}
