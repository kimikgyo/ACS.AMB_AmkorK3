using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace INA_ACS_Server
{
    public class DeviceManager
    {
        private readonly static ILog Log = LogManager.GetLogger("PlcEvent"); //Function 실행관련 Log

        #region - Constructor -
        private static DeviceManager inst = null;
        private static readonly object padLock = new object();
        private static readonly object dataLock = new object();
        public static DeviceManager Instance
        {
            get
            {
                lock (padLock)
                {
                    if (inst == null)
                    {
                        inst = new DeviceManager();
                    }
                    return inst;
                }
            }
        }

        private DeviceManager()
        {
        }

        private static Dictionary<String, IDevice> devices = new Dictionary<String, IDevice>();
        public IDevice GetDevice(string deviceName)
        {
            try
            {
                return devices[deviceName];
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("#### Failed DeviceMan.GetDevice( deviceName='{0}')", deviceName), ex);
            }

            return null;
        }

        public static void Start(IUnitOfWork uow)
        {
            try
            {
                if (inst == null) // 이미 시작됨
                {
                    DeviceManager inst = Instance;
                    Setup(uow);
                    foreach (KeyValuePair<string, IDevice> p in devices)
                    {
                        Task.Run(async () => await p.Value.Run());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("#### Fialed PlcMan.Start()"), ex);
            }
        }
        #endregion - Constructor -

        //private static void Setup()
        //{
        //    // 각 PLC별로 객체 생성

        //    //// Plc1
        //    //MelsecPlc plc1 = new MelsecPlc("MelsecPlc1", "127.0.0.1", 8000, 50); // PLC 객체 생성
        //    //plc1.AddRead(MelsecIo.DeviceCode.D, 0, 20);   // 읽기 구간 설정
        //    //plc1.AddRead(MelsecIo.DeviceCode.W, 0, 20);   // 읽기 구간 설정
        //    //plc1.AddRead(MelsecIo.DeviceCode.D, 100, 20); // 읽기 구간 설정
        //    //plc1.AddRead(MelsecIo.DeviceCode.W, 100, 20); // 읽기 구산 설정
        //    //devices.Add(plc1.PlcName, plc1);                  // PLC 등록

        //    // Plc 16ea
        //    MelsecPlc[] plc = new MelsecPlc[16];
        //    for (int i = 0; i < 16; i++)
        //    {
        //        plc[i] = new MelsecPlc($"MelsecPlc{i + 1}", $"127.0.0.{i + 1}", 8000 + i, 50);
        //        plc[i].AddRead(MelsecIo.DeviceCode.D, 20 * i, 20);   // 읽기 구간 설정
        //        devices.Add(plc[i].PlcName, plc[i]);                  // PLC 등록
        //    }
        //}

        private static void Setup(IUnitOfWork uow)
        {
            // 전체 PLC Read Configuration 중에서 MELSEC PLC만 등록한다
            // 같은 PLC에 여러개의 읽기구간이 있을 경우 각각 다른 PLC객체로 처리한다

            foreach (PlcConfig config in uow.PlcConfigs.GetAll().Where(w => w.PlcModuleUse == "Use" && w.PlcModuleName.StartsWith("Melsec")))
            {
                // PLC 객체 생성
                string deviceName = config.PlcModuleName;
                string ip = config.PlcIpAddress;
                int port = config.PortNumber;
                MelsecPlc plc1 = new MelsecPlc(deviceName, ip, port, 50); // PLC 객체 생성

                // 읽기 구간 설정
                int startAddr = int.Parse(config.ReadFirstMapAddress);
                //int size = 10;
                int size = int.Parse(config.ReadSecondMapAddress) - int.Parse(config.ReadFirstMapAddress) + 1;
                plc1.AddRead(MelsecIo.DeviceCode.D, startAddr, size);   // 읽기 구간 설정

                // PLC 등록
                devices.Add(plc1.PlcName, plc1);
            }
        }
    }
}
