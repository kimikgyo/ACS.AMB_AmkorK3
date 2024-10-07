using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace INA_ACS_Server
{
    public class TcpSvr
    {
        private readonly ILog EventLogger = LogManager.GetLogger("Event");
        private readonly MainForm main;

        private TcpListener tcpListener = null;


        public TcpSvr(MainForm mainForm)
        {
            this.main = mainForm;
        }

        protected void TcpSvr_Thread()
        {
            while (true)
            {
                // 서버 시작
                try
                {
                    tcpListener = new TcpListener(IPAddress.Any, 5555);
                    tcpListener.Start(); // Listener 동작 시작
                    Log("server start");

                    while (true)
                    {
                        TcpClient acceptClient = tcpListener.AcceptTcpClient(); // 동기
                                                                                //TcpClient acceptClient = await tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);  // 비동기
                        string ip = acceptClient.Client.RemoteEndPoint.ToString();

                        main.TCP_Connect_Client++;
                        Log(ip, "connected", new string(' ', 28), "연결갯수=", main.TCP_Connect_Client.ToString());

                        Receiver receiver = new Receiver(main);
                        receiver.Start(acceptClient);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
                    Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
                }
                finally
                {
                    tcpListener.Stop();

                    main.TCP_Connect_Client = 0;
                    Log("server stop");
                }

                // 서버가 예외로 멈추면 5초후 재시작
                Thread.Sleep(5000);
            }
        }

        public void Start()
        {
            Task.Run(() => TcpSvr_Thread());
        }

        private string GetIP()
        {
            //본인 PC의 IP Address를 가져오는 함수
            string strHostName = "";

            strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 3].ToString();
        }

        private void Log(params string[] msg)
        {
            string fullMsg = string.Join(" ", msg);
            Console.WriteLine("{0:yyyy-MM-dd_HH:mm:ss} {1}", DateTime.Now, fullMsg);
        }
    }

    //public static void SendingClientStream(string strBuffer, TcpClient client)
    //{
    //    try
    //    {
    //        if (client.Connected)
    //        {
    //            byte[] sndMsg = Encoding.ASCII.GetBytes(PublicConst.STX + strBuffer + PublicConst.ETX); //전송할 문자열의 시작,끝에 STX,ETX를 붙인다

    //            NetworkStream ns = client.GetStream();
    //            ns.Write(sndMsg, 0, sndMsg.Length);
    //            ns.Flush(); // 버퍼 제거
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message);
    //        Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
    //    }
    //}
}