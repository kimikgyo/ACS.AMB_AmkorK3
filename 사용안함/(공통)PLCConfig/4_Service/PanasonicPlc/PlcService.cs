using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class PlcService
    {
        private readonly ILog Plclogger = LogManager.GetLogger("PlcEvent");
        private readonly IUnitOfWork uow;
        private PanasonicPlc plc;
        private List<PlcConfig> plcConfigs;

        string oldPlcMapValue = "";
        public PlcService(IUnitOfWork uow)
        {
            this.uow = uow;
            this.plc = new PanasonicPlc();
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
                    //foreach (PlcConfig plcConfig in uow.PlcConfigs.GetAll().Where(w => w.PlcModuleUse == "Use"))
                    foreach (PlcConfig plcConfig in uow.PlcConfigs.GetAll().Where(w => w.PlcModuleUse == "Use" && !w.PlcModuleName.StartsWith("Melsec"))) // melsec plc는 제외하고 처리
                    {
                        await Task.Delay(50); // <=========================== 노드간 통신 간격

                        // PLC 데이타 송수신 처리
                        plc.SetConfig(plcConfig);

                        bool recv_good = false;

                        if (!string.IsNullOrEmpty(plc.ReadFirstMapAddress))      //평상시에는 Plc Configs(DB)등록된 데이터를 읽어서 Read인 부분만 읽어서 FleetThread로 전송합니다.
                        {
                            recv_good = await plc.SendRecvAsync("");

                            if (recv_good)                                        //정상적인 데이터를 읽어왔는지 확인 
                            {
                                plcConfig.PlcMapValue = plc.PlcMapValue;
                                if (oldPlcMapValue != plcConfig.PlcMapValue)
                                {
                                    oldPlcMapValue = plcConfig.PlcMapValue;
                                    PLCReadDataParsing(plcConfig);
                                    PlcConneted(plcConfig);
                                }
                                else PlcDisConnected(plcConfig);
                            }
                            else PlcDisConnected(plcConfig);
                        }
                        else Plclogger.Info("plc read error");

                        if (!string.IsNullOrEmpty(plc.WriteFirstMapAddress))                     //FleetThread Plc 보낼데이터를 이벤트를 발생시켜서 PLC에 전송합니다.
                        {
                            recv_good = await plc.SendRecvAsync(PLCWriteData(plcConfig).ToString());
                            if (recv_good)
                            {
                                PlcConneted(plcConfig);
                            }
                            else PlcDisConnected(plcConfig);
                        }

                        else Plclogger.Info("plc write error");

                    }
                    await Task.Delay(1); // <=========================== 루프 통신 딜레이
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException?.Message ?? ex.Message;
                    Debug.WriteLine(message);
                    Plclogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                    Console.WriteLine(message);

                }
            }

        }

        private void PLCReadDataParsing(PlcConfig plcConfig)
        {
            string[] plcConfigReadParsing = plcConfig.PlcMapValue.Split('/');
            plcConfig.serviceReadData.DT11000 = Convert.ToInt32(plcConfigReadParsing[0]);
            plcConfig.serviceReadData.DT11001 = Convert.ToInt32(plcConfigReadParsing[1]);
            plcConfig.serviceReadData.DT11002 = Convert.ToInt32(plcConfigReadParsing[2]);
            plcConfig.serviceReadData.DT11003 = Convert.ToInt32(plcConfigReadParsing[3]);
            plcConfig.serviceReadData.DT11004 = Convert.ToInt32(plcConfigReadParsing[4]);
            plcConfig.serviceReadData.DT11005 = Convert.ToInt32(plcConfigReadParsing[5]);
            plcConfig.serviceReadData.DT11006 = Convert.ToInt32(plcConfigReadParsing[6]);
            plcConfig.serviceReadData.DT11007 = Convert.ToInt32(plcConfigReadParsing[7]);
            plcConfig.serviceReadData.DT11008 = Convert.ToInt32(plcConfigReadParsing[8]);
            plcConfig.serviceReadData.DT11009 = Convert.ToInt32(plcConfigReadParsing[9]);
            plcConfig.serviceReadData.DT11010 = Convert.ToInt32(plcConfigReadParsing[10]);

            Plclogger.Info($"PLCReadData , ModuleName = {plc.PlcModuleName}" +
                           $"/DT11000 = {plcConfig.serviceReadData.DT11000}" +
                           $"/DT11001 = {plcConfig.serviceReadData.DT11001}" +
                           $"/DT11002 = {plcConfig.serviceReadData.DT11002}" +
                           $"/DT11003 = {plcConfig.serviceReadData.DT11003}" +
                           $"/DT11004 = {plcConfig.serviceReadData.DT11004}" +
                           $"/DT11005 = {plcConfig.serviceReadData.DT11005}" +
                           $"/DT11006 = {plcConfig.serviceReadData.DT11006}" +
                           $"/DT11007 = {plcConfig.serviceReadData.DT11007}" +
                           $"/DT11008 = {plcConfig.serviceReadData.DT11008}" +
                           $"/DT11009 = {plcConfig.serviceReadData.DT11009}" +
                           $"/DT11010 = {plcConfig.serviceReadData.DT11010}");
        }
        private string PLCWriteData(PlcConfig plcConfig)
        {
            //DT12000경우 통신체크라서 메뉴얼 및 Auto 상관없이 보낸다
            plcConfig.serviceWriteData.DT12000 = 1 - plcConfig.serviceWriteData.DT12000;    //Connect 확인하기위한 0과 1 반복

            Plclogger.Info($"PLCWriteData , ModuleName = {plc.PlcModuleName}" +
                           $"/DT12000 = {plcConfig.serviceWriteData.DT12000}" +
                           $"/DT12001 = {plcConfig.serviceWriteData.DT12001}" +
                           $"/DT12002 = {plcConfig.serviceWriteData.DT12002}" +
                           $"/DT12003 = {plcConfig.serviceWriteData.DT12003}" +
                           $"/DT12004 = {plcConfig.serviceWriteData.DT12004}" +
                           $"/DT12005 = {plcConfig.serviceWriteData.DT12005}" +
                           $"/DT12006 = {plcConfig.serviceWriteData.DT12006}" +
                           $"/DT12007 = {plcConfig.serviceWriteData.DT12007}" +
                           $"/DT12008 = {plcConfig.serviceWriteData.DT12008}" +
                           $"/DT12009 = {plcConfig.serviceWriteData.DT12009}" +
                           $"/DT12010 = {plcConfig.serviceWriteData.DT12010}");

            return $"{plcConfig.serviceWriteData.DT12000}/" +
                   $"{plcConfig.serviceWriteData.DT12001}/" +
                   $"{plcConfig.serviceWriteData.DT12002}/" +
                   $"{plcConfig.serviceWriteData.DT12003}/" +
                   $"{plcConfig.serviceWriteData.DT12004}/" +
                   $"{plcConfig.serviceWriteData.DT12005}/" +
                   $"{plcConfig.serviceWriteData.DT12006}/" +
                   $"{plcConfig.serviceWriteData.DT12007}/" +
                   $"{plcConfig.serviceWriteData.DT12008}/" +
                   $"{plcConfig.serviceWriteData.DT12009}/" +
                   $"{plcConfig.serviceWriteData.DT12010}";
        }





        private void PlcConneted(PlcConfig plcConfig)
        {
            if (plcConfig.Connect == false)
            {
                plcConfig.Connect = true;
                plcConfig.ConnectRetry = 0;
            }
            else plcConfig.ConnectRetry = 0;
        }

        private void PlcDisConnected(PlcConfig plcConfig)
        {
            if (plcConfig.ConnectRetry >= 30)
            {
                if (plcConfig.Connect)
                {
                    plcConfig.Connect = false;
                }
            }
            else plcConfig.ConnectRetry++;
        }
    }


}
