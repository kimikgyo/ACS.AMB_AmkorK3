using log4net;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class WirelessSensorRS485
    {
        private readonly ILog logger = LogManager.GetLogger("Event");
        private readonly MainForm main;
        private readonly IUnitOfWork uow;
        private bool Connected = false;
        
        public bool FlagData = false;
        public int Floor = -1;
        /// <summary>
        /// 1 = 문열림, 2 = 문닫힘, 3 = 목적지층수, 4 = AGVMode, 5 = Not AGVMode
        /// </summary>
        public int FlagWrite = 0;
        public string ElevatorState = "";

        SerialPort sp = new SerialPort();
        public WirelessSensorRS485(MainForm main, IUnitOfWork uow)
        {
            this.main = main;
            this.uow = uow;
        }

        public void Start()
        {
            Task.Run(() => Loop());

        }

        private async void Loop()
        {

            while (true)
            {
                Connected = false;
                try
                {
                    _PortOpen();
                    Connected = true;

                    while (Connected)
                    {
                        String response = String.Empty;
                        byte[] recvBuff = new Byte[1024];

                        int recvLength = 0;
                        string message = "";

                        byte[] senddata = _MakeSendingData();
                        if (senddata != null) sp.Write(senddata, 0, senddata.Length);

                        await Task.Delay(50);

                        sp.ReadTimeout = 1000;
                        recvLength = sp.Read(recvBuff, 0, recvBuff.Length);
                        if (recvLength > 0)
                        {
                            if (recvBuff[0] == Constants.SensorRecvDA && recvBuff[1] == Constants.SensorRecvSA &&
                                recvBuff[2] == Constants.SensorRecvFCMD)
                            {
                                for (int i = 0; i < recvLength; i++)
                                {
                                    //16진수[Hax값을] 문자열로 변환한다.
                                    response += string.Format("{0:X2}", recvBuff[i]);
                                }
                            }
                        }
                        if (recvBuff.Length > 0) MakeRecvData(recvBuff);
                    }
                    await Task.Delay(200);
                }
                catch (Exception ex)
                {
                    sp.Close();
                    await Task.Delay(1000);
                    LogExceptionMessage(ex);
                }
            }

        }

        private void LogExceptionMessage(Exception ex)
        {
            string message = ex.InnerException?.Message ?? ex.Message;
            Debug.WriteLine(message);
            logger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
            //main.ACS_UI_Log(message);
        }


        private void _PortOpen()
        {
            //시리얼포트 열기
            if (!sp.IsOpen)
            {
                sp.PortName = "COM3";
                sp.BaudRate = 19200;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.StopBits = StopBits.One;
                sp.Open();
            }
        }


        private byte[] _MakeSendingData()
        {
            //16진수로 (송신 또는 수신한다)
            byte[] sendbyte = null;
            if (FlagData)
            {
                //데이터 쓰기 요청
                if (FlagWrite == 1)
                {
                    //문열림
                    byte[] CRCBody = new byte[] { 0x00, 0x06, 0x03, 0xEB, 0x00, 0xC1 };
                    var CRC16 = Calc(CRCBody, CRCBody.Length);
                    byte[] CRCLH = BitConverter.GetBytes(CRC16);

                    sendbyte = new byte[] { 0x00, 0x06, 0x03, 0xEB, 0x00, 0xC1, CRCLH[0], CRCLH[1] };
                }
                else if (FlagWrite == 2)
                {
                    //문닫힘
                    byte[] CRCBody = new byte[] { 0x00, 0x06, 0x03, 0xEB, 0x00, 0xC2 };
                    var CRC16 = Calc(CRCBody, CRCBody.Length);
                    byte[] CRCLH = BitConverter.GetBytes(CRC16);

                    sendbyte = new byte[] { 0x00, 0x06, 0x03, 0xEB, 0x00, 0xC2, CRCLH[0], CRCLH[1] };
                }
                else if (FlagWrite == 3)
                {
                    //목적지 층수
                    byte[] B_Floor = BitConverter.GetBytes(Floor);

                    byte[] CRCBody = new byte[] { 0x00, 0xfe, 0x24, 0x04, 0x86, 0x00, B_Floor[0], 0x00 };
                    var CRC16 = Calc(CRCBody, CRCBody.Length);
                    byte[] CRCLH = BitConverter.GetBytes(CRC16);

                    sendbyte = new byte[] { 0x00, 0xfe, 0x24, 0x04, 0x86, 0x00, B_Floor[0], 0x00, CRCLH[0], CRCLH[1] };
                }
                else if (FlagWrite == 4)
                {
                    //AGVMode
                    //byte[] CRCBody = new byte[] { 0x00, 0xfe, 0x24, 0x04, 0x88, 0x00, 0x01, 0x00 };
                    //var CRC16 = Calc(CRCBody, CRCBody.Length);
                    //byte[] CRCLH = BitConverter.GetBytes(CRC16);

                    //sendbyte = new byte[] { 0x00, 0xfe, 0x24, 0x04, 0x88, 0x00, 0x01, 0x00, CRCLH[0], CRCLH[1] };
                }
                else if (FlagWrite == 5)
                {
                    //Not AGVMode
                    //byte[] CRCBody = new byte[] { 0x00, 0xfe, 0x24, 0x04, 0x88, 0x00, 0x00, 0x00 };
                    //var CRC16 = Calc(CRCBody, CRCBody.Length);
                    //byte[] CRCLH = BitConverter.GetBytes(CRC16);

                    //sendbyte = new byte[] { 0x00, 0xfe, 0x24, 0x04, 0x88, 0x00, 0x00, 0x00, CRCLH[0], CRCLH[1] };
                }

                FlagData = false;
            }
            else
            {
                //데이터 요청
                sendbyte = new byte[] { 0x00, 0xfe, 0x23, 0x02, 0x00, 0x00, 0x83, 0x8b };

            }
            if (sendbyte != null) return SendingServerStream(sendbyte);
            else return sendbyte;
        }

        private UInt32 Calc(byte[] data, int nLength)
        {
            UInt32 crc16 = 0xFFFF;
            byte[] ptr = data;

            for(int i=0; i<nLength; i++)
            {
                crc16 ^= ptr[i];
                for (int n=0; n<8; n++)
                {
                    if ((crc16 & 0x0001) > 0)
                        crc16 = (crc16 >> 1) ^ 0xA001;
                    else
                        crc16 >>= 1;
                }
            }

            return crc16;
        }

        public static bool Check_Bit(byte _data, int loc)
        {
            BitArray bits = new BitArray(BitConverter.GetBytes(_data).ToArray());

            return bits[loc];
        }

        private bool MakeRecvData(byte[] data)
        {
            try
            {
                bool ElevatorMove = Check_Bit(data[6], 1);
                bool DoorOpen = Check_Bit(data[5], 0);//열림 true
                bool DoorClose = Check_Bit(data[5], 1);//닫힘 true
                if (DoorOpen && !ElevatorMove)
                    main.ElevatorState = "문열림";//문상태
                else if (DoorClose && !ElevatorMove)
                    main.ElevatorState = "문닫힘";//문상태
                else if (DoorClose && ElevatorMove)
                    main.ElevatorState = "운행중";//엘리베이터 상태

                int Floor = data[4] - 1;
                if (Floor == 0)
                    main.ElevatorFloor = "B1F";//층수
                else
                    main.ElevatorFloor = Floor.ToString() + "F";//층수
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                Console.WriteLine(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);

                logger.Info(ex.InnerException?.Message ?? ex.Message);
                logger.Info(ex.GetFullMessage() + Environment.NewLine + ex.StackTrace);
            }
            return true;
        }

        private byte[] SendingServerStream(byte[] byteBuff)//Sending Data 변환
        {
            //byte[] tempBuff = Encoding.ASCII.GetBytes(strBuffer);
            byte[] sendData = null;
            using (var ms = new MemoryStream())
            {
                ms.Write(byteBuff, 0, byteBuff.Length);
                sendData = ms.ToArray();
            }
            return sendData;
        }
    }
}
