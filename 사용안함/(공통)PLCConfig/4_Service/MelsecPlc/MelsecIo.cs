using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace INA_ACS_Server
{
    public class MelsecIo
    {
        static readonly object obj = new object();

        // MELSEC PLC MC3EBin
        public enum DeviceCode
        {
            SM = 0x91, SD = 0xa9, X = 0x9c, Y = 0x9d, M = 0x90, L = 0x92,
            F = 0x93, V = 0x94, B = 0xa0, D = 0xa8, W = 0xb4, TS = 0xc1, TC = 0xc0, TN = 0xc2,
            SS = 0xc7, SC = 0xc6, SN = 0xc8, CS = 0xc4, CC = 0xc3, CN = 0xc5, SB = 0xa1, SW = 0xb5,
            S = 0x98, DX = 0xa2, DY = 0xa3, Z = 0xcc, R = 0xaf, ZR = 0xb0
        };

        public class AbnormalResponseException : Exception
        {
            public AbnormalResponseException(string msg)
                : base(msg)
            {
            }
        };

        public class ResponseException : Exception
        {
            public ResponseException(string msg)
                : base(msg)
            {
            }
        };

        byte station = 0;
        private byte Station
        {
            get { return station; }
            set { station = value; }
        }

        byte network = 0;
        private byte Network
        {
            get { return network; }
            set { network = value; }
        }

        byte pc = 0xff;
        private byte PC
        {
            get { return pc; }
            set { pc = value; }
        }

        UInt16 cpu = 0x3ff;
        private UInt16 Cpu
        {
            get { return cpu; }
            set { cpu = value; }
        }

        private TcpClient client;
        private NetworkStream stream = null;
        BinaryReader srd;
        BinaryWriter swr;

        private UTF8Encoding enc = new UTF8Encoding();

        bool useChecksum = false;
        private bool UseChecksum
        {
            get { return useChecksum; }
            set { useChecksum = value; }
        }

        protected MelsecIo()
        {
        }

        protected void Open(String equipID, int PortNo)
        {
            //client = new TcpClient(equipID, PortNo);
            client = new TcpClient();
            IAsyncResult result = client.BeginConnect(equipID, PortNo, null, null);

            bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            if (!success) throw new Exception("Timeout TcpClient.");

            client.ReceiveTimeout = 1000;
            //client.ReceiveTimeout = -1;
            stream = client.GetStream();
            srd = new BinaryReader(stream);
            swr = new BinaryWriter(stream);
        }

        protected void Close()
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            if (client != null)
            {
                client.Close();
                client = null; // 20110718
            }
        }

        private int HexToInt(string b, int start, int len)
        {
            return Convert.ToInt32(b.Substring(start, len), 16);
        }

        private void CheckHeader(byte[] br)
        {
            if (br[0] != 0xD0 || br[1] != 0x0)
                throw new ResponseException("Incorrect sub-header code");


            if (br[2] != network)
                throw new ResponseException("Incorrect network number");

            if (br[3] != pc)
                throw new ResponseException("Incorrect PC number");

            if (br[4] != (byte)(cpu & 0xff) || br[5] != (byte)((cpu >> 8) & 0xff))
                throw new ResponseException("Incorrect CPU number");
        }

        private void SendReadPacket(DeviceCode prefix, UInt32 addr, UInt16[] d)
        {
            lock (obj)
            {
                byte[] b = new byte[21];
                byte[] br, br1;
                UInt16 sz = 12;
                int i, j;

                //Sub-header
                b[0] = 0x50;
                b[1] = 0x0;
                b[2] = network;
                b[3] = pc;
                b[4] = (byte)(cpu & 0xff);
                b[5] = (byte)((cpu >> 8) & 0xff);
                b[6] = station;
                b[7] = (byte)(sz & 0xff);
                b[8] = (byte)((sz >> 8) & 0xff);
                //Application data
                //Response timeout
                b[9] = 0x20;    //5s timeout
                b[10] = 0x0;
                //Message content
                b[11] = 0x01;   //Read words command 0x0401 (0x0000)
                b[12] = 0x04;
                b[13] = 0x0;
                b[14] = 0x0;
                //3 byte head address
                b[15] = (byte)(addr & 0xff);
                b[16] = (byte)((addr >> 8) & 0xff);
                b[17] = (byte)((addr >> 16) & 0xff);
                //Area prefix
                b[18] = (byte)prefix;
                //Number of points
                b[19] = (byte)(d.Length & 0xff);
                b[20] = (byte)((d.Length >> 8) & 0xff);
                swr.Write(b);

                //Read header back
                br = srd.ReadBytes(11);
                i = (((int)br[8] << 8) + br[7]) - 2;
                if (i > 0)
                {
                    br1 = srd.ReadBytes(i);
                }
                else
                    br1 = new byte[0];

                CheckHeader(br);

                i = ((int)br[10] << 8) + br[9];
                if (i != 0)
                    throw new AbnormalResponseException("Abnormal response code <" + i.ToString() + ">");

                if (br1.Length != 2 * d.Length)
                    throw new ResponseException("Invalid response data size");

                for (i = 0, j = 0; i < d.Length; i++, j += 2)
                    d[i] = (UInt16)(((UInt16)br1[j + 1] << 8) + br1[j]);
            }
        }

        //private void SendLimitedReadWords(DeviceCode prefix, UInt32 addr, UInt16[] d)
        //{
        //    // 길이가 960보다 작을 때 처리
        //    int length = d.Length;
        //    int MaxLength = 960;
        //    int sp = 0;
        //    for (int i = 0; length > sp; ++i)
        //    {
        //        int rl = length - sp;
        //        if (rl > MaxLength) rl = MaxLength;

        //        UInt16[] temp = new UInt16[rl];
        //        SendReadPacket(prefix, (uint)(addr + sp), temp);

        //        Buffer.BlockCopy(temp, 0, d, sp * 2, temp.Length * 2);
        //        sp += rl;
        //    }
        //}

        protected void SendReadWords(DeviceCode prefix, UInt32 start_Addr, UInt16[] d)
        {
            // 길이가 960이상 일 때 처리 (960으로 나누어서 여러번 반복 처리 한다)
            const int MAX_READ_LENGTH = 960;
            int totalReadLength = d.Length; // 읽어올 전체 워드 수
            int readLength = 0;
            int sp = 0;

            while (sp < totalReadLength)
            {
                // 남은 데이터 수에 따라, 다음 읽을 데이터 수 결정
                if (totalReadLength - sp < MAX_READ_LENGTH)
                    readLength = totalReadLength - sp;
                else
                    readLength = MAX_READ_LENGTH;

                // 읽을 어드레스/버퍼 설정
                var readAddress = start_Addr + sp;
                var readBuffer = new UInt16[readLength];

                SendReadPacket(prefix, (uint)readAddress, readBuffer);
                Buffer.BlockCopy(readBuffer, 0, d, sp * 2, readBuffer.Length * 2);

                sp += readLength;
            }
        }

        protected void SendWriteWords(DeviceCode prefix, UInt32 addr, UInt16[] d)
        {
            lock (obj)
            {
                byte[] b = new byte[d.Length * 2 + 21];
                byte[] br, br1;
                UInt16 sz = (UInt16)(d.Length * 2 + 12);
                int i, j;

                //Sub-header
                b[0] = 0x50;
                b[1] = 0x0;
                b[2] = network;
                b[3] = pc;
                b[4] = (byte)(cpu & 0xff);
                b[5] = (byte)((cpu >> 8) & 0xff);
                b[6] = station;
                b[7] = (byte)(sz & 0xff);
                b[8] = (byte)((sz >> 8) & 0xff);
                //Application data
                //Response timeout
                b[9] = 0x20;    //5s timeout
                b[10] = 0x0;
                //Message content
                b[11] = 0x01;   //Write words command 0x1401 (0x0000)
                b[12] = 0x14;
                b[13] = 0x0;
                b[14] = 0x0;
                //3 byte head address
                b[15] = (byte)(addr & 0xff);
                b[16] = (byte)((addr >> 8) & 0xff);
                b[17] = (byte)((addr >> 16) & 0xff);
                //Area prefix
                b[18] = (byte)prefix;
                //Number of points
                b[19] = (byte)(d.Length & 0xff);
                b[20] = (byte)((d.Length >> 8) & 0xff);
                for (i = 0, j = 21; i < d.Length; i++, j += 2)
                {
                    b[j] = (byte)(d[i] & 0xff);
                    b[j + 1] = (byte)((d[i] >> 8) & 0xff);
                }
                //for (i = 0; i < b.Length; i++)
                //    Debug.Write(b[i].ToString("X") + ", ");
                //Debug.WriteLine("");
                swr.Write(b);

                //Read header back
                br = srd.ReadBytes(11);
                //for (i = 0; i < br.Length; i++)
                //    Debug.Write(br[i].ToString("X") + ", ");
                //Debug.WriteLine("");
                i = (((int)br[8] << 8) + br[7]) - 2;
                if (i > 0)
                {
                    br1 = srd.ReadBytes(i);
                }

                CheckHeader(br);

                i = ((int)br[10] << 8) + br[9];
                if (i != 0)
                    throw new AbnormalResponseException("Abnormal response code <" + i.ToString() + ">");
            }
        }

        protected void SendWriteBits(DeviceCode prefix, UInt32 addr, bool[] d)
        {
            lock (obj)
            {
                int len = (d.Length + 1) / 2;
                byte[] b = new byte[len + 21];
                byte[] br, br1;
                UInt16 sz = (UInt16)(len + 12);
                int i;

                //Sub-header
                b[0] = 0x50;
                b[1] = 0x0;
                b[2] = network;
                b[3] = pc;
                b[4] = (byte)(cpu & 0xff);
                b[5] = (byte)((cpu >> 8) & 0xff);
                b[6] = station;
                b[7] = (byte)(sz & 0xff);
                b[8] = (byte)((sz >> 8) & 0xff);
                //Application data
                //Response timeout
                b[9] = 0x20;    //5s timeout
                b[10] = 0x0;
                //Message content
                b[11] = 0x01;   //Write words command 0x1402 (0x0000)
                b[12] = 0x14;
                b[13] = 0x01;
                b[14] = 0x00;
                //3 byte head address
                b[15] = (byte)(addr & 0xff);
                b[16] = (byte)((addr >> 8) & 0xff);
                b[17] = (byte)((addr >> 16) & 0xff);
                //Area prefix
                b[18] = (byte)prefix;
                //Number of points
                b[19] = (byte)(d.Length);// (byte)(d.Length & 0xff);
                b[20] = (byte)((d.Length >> 8) & 0xff);
                for (i = 0; i < d.Length; i++)
                {
                    int j = 21 + i / 2;
                    if( (i & 0x1) == 0 )
                        b[j] = (byte)(d[i] ? 0x10 : 0x00);
                    else
                        b[j] += (byte)(d[i] ? 0x01 : 0x00);
                }
                //for (i = 0; i < b.Length; i++)
                //    Debug.Write(b[i].ToString("X") + ", ");
                //Debug.WriteLine("");
                swr.Write(b);

                //Read header back
                br = srd.ReadBytes(11);
                //for (i = 0; i < br.Length; i++)
                //    Debug.Write(br[i].ToString("X") + ", ");
                //Debug.WriteLine("");
                i = (((int)br[8] << 8) + br[7]) - 2;
                if (i > 0)
                {
                    br1 = srd.ReadBytes(i);
                }

                CheckHeader(br);

                i = ((int)br[10] << 8) + br[9];
                if (i != 0)
                    throw new AbnormalResponseException("Abnormal response code <" + i.ToString() + ">");
            }
        }
    }
}
