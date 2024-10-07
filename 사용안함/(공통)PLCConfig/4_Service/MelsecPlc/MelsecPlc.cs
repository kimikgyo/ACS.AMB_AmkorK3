using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace INA_ACS_Server
{
    public class MelsecPlc : MelsecIo, IDevice
    {
        static readonly object obj = new object();

        private readonly static ILog Log = LogManager.GetLogger("PlcEvent"); //Function 실행관련 Log

        #region ---InternalMemory---
        //private byte[] SM = new byte[0x10000 * 2];     // 64K
        //private byte[] X = new byte[0x10000 * 2];     // 64K
        //private byte[] Y = new byte[0x10000 * 2];     // 64K
        //private byte[] M = new byte[0x10000 * 2];     // 64K
        //private byte[] L = new byte[0x10000 * 2];     // 64K
        //private byte[] F = new byte[0x10000 * 2];     // 64K
        //private byte[] V = new byte[0x10000 * 2];     // 64K
        //private byte[] B = new byte[0x10000 * 2];     // 64K
        private byte[] D = new byte[0x10000 * 2];       // 64K
        private byte[] W = new byte[0x10000 * 2];       // 64K
        //private byte[] S = new byte[0x10000 * 2];     // 64K
        //private byte[] Z = new byte[0x10000 * 2];     // 64K
        //private byte[] R = new byte[0x10000 * 2];     // 64K
        #endregion ---InternalMemory---

        #region ---PLC Interface---
        class ReadConfig
        {
            public MelsecIo.DeviceCode dc;
            public UInt16 startAddr;
            public int size;
            public UInt16[] readDatas;
            public ReadConfig(MelsecIo.DeviceCode dc, int startAddr, int size)
            {
                this.dc = dc;
                this.startAddr = (UInt16)startAddr;
                this.size = size;
                readDatas = new UInt16[size];
            }
        }

        private class PlcWriteWords
        {
            public MelsecIo.DeviceCode dc;
            public UInt32 Addr;
            public UInt16[] data;

            public PlcWriteWords(MelsecIo.DeviceCode dc, UInt32 Addr, UInt16[] data)
            {
                this.dc = dc;
                this.Addr = Addr;
                this.data = data;
            }
        }

        private string deviceName;           // PLC Name
        private string ip;                // IP Address
        private int portNo;               // Port No
        private int readIntervalMSec;     // PLC 읽기 시간 간격
        private bool isConnected;         // 연결상태 정보

        private List<ReadConfig> cfgs = new List<ReadConfig>();
        private ConcurrentQueue<PlcWriteWords> writeQueueData = new ConcurrentQueue<PlcWriteWords>();

        public MelsecPlc(string deviceName, string ip, int port, int readIntervalMSec)
        {
            this.deviceName = deviceName;
            this.ip = ip;
            this.portNo = port;
            this.readIntervalMSec = readIntervalMSec;
        }

        public bool AddRead(MelsecIo.DeviceCode dc, int startAddr, int size)
        {
            cfgs.Add(new ReadConfig(dc, startAddr, size));
            return true;
        }

        public async Task Run()
        {
            while (true)
            {
                isConnected = false;
                try
                {
                    Open(ip, portNo);
                    isConnected = true;
                    Log.InfoFormat("Connected deviceName={0}, ip={1}, port={2}, read interval={3}(ms)", PlcName, ip, portNo, readIntervalMSec);
                    while (true)
                    {
                        WriteProc();
                        foreach (ReadConfig cfg in cfgs)
                        {
                            SendReadWords(cfg.dc, cfg.startAddr, cfg.readDatas);
                            CopyToMemory(cfg);
                            WriteProc();
                        }
                        await Task.Delay(readIntervalMSec);
                    }
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    Log.ErrorFormat("#### Disconnected deviceName={0}, ip={1}, port={2}, message={3}", PlcName, ip, portNo, ex.Message);
                    try
                    {
                        Close();
                    }
                    catch
                    {
                    }
                }
                await Task.Delay(5000);
            }
        }

        private void CopyToMemory(ReadConfig cfg)
        {
            lock (obj)
            {
                switch (cfg.dc)
                {
                    case DeviceCode.D: Buffer.BlockCopy(cfg.readDatas, 0, D, cfg.startAddr * 2, cfg.size * 2); break;
                    case DeviceCode.W: Buffer.BlockCopy(cfg.readDatas, 0, W, cfg.startAddr * 2, cfg.size * 2); break;
                    default: throw new Exception(string.Format("#### No Code - PlcMelsec.CopyToMemory() - DeviceCode dc = {0}", cfg.dc.ToString()));
                }
            }
        }

        private void WriteProc()
        {
            while (writeQueueData.TryDequeue(out PlcWriteWords wd))
            {
                base.SendWriteWords((DeviceCode)wd.dc, (UInt16)wd.Addr, wd.data);
            }
        }

        #endregion ---PLC Interface---

        #region ---Client Interface---

        public string PlcName
        {
            get
            {
                lock (obj)
                {
                    return deviceName;
                }
            }
        }

        public bool IsConnect
        {
            get
            {
                lock (obj)
                {
                    return isConnected;
                }
            }
        }

        public UInt16 GetWordD(int Addr) { return ReadWord(DeviceCode.D, Addr, 1)[0]; }
        public UInt16 GetWordW(int Addr) { return ReadWord(DeviceCode.W, Addr, 1)[0]; }
        public UInt16[] ReadWordD(int Addr, int size) { return ReadWord(DeviceCode.D, Addr, size); }
        public UInt16[] ReadWordW(int Addr, int size) { return ReadWord(DeviceCode.W, Addr, size); }
        private UInt16[] ReadWord(DeviceCode dc, int Addr, int size)
        {
            lock (obj)
            {
                UInt16[] d = new UInt16[size];
                switch (dc)
                {
                    case DeviceCode.D: Buffer.BlockCopy(D, Addr * 2, d, 0, size * 2); break;
                    case DeviceCode.W: Buffer.BlockCopy(W, Addr * 2, d, 0, size * 2); break;
                    default: throw new Exception(string.Format("#### No Code - PlcMelsec.ReadWord() - DeviceCode dc = {0}", dc.ToString()));
                }
                return d;
            }
        }

        public bool WriteWordsD(UInt32 addr, UInt16[] data) { return WriteWords(DeviceCode.D, addr, data); }
        public bool WriteWordsW(UInt32 addr, UInt16[] data) { return WriteWords(DeviceCode.W, addr, data); }
        private bool WriteWords(DeviceCode dc, UInt32 addr, UInt16[] data)
        {
            if (IsConnect)
            {
                PlcWriteWords wd;
                switch (dc)
                {
                    case DeviceCode.D: wd = new PlcWriteWords(MelsecIo.DeviceCode.D, addr, data); break;
                    case DeviceCode.W: wd = new PlcWriteWords(MelsecIo.DeviceCode.W, addr, data); break;
                    default: throw new Exception(string.Format("#### No Code - PlcMelsec.WriteWord() - DeviceCode dc = {0}", dc.ToString()));
                }
                writeQueueData.Enqueue(wd);
            }
            return IsConnect;
        }

        #endregion ---Client Interface---
    }
}
