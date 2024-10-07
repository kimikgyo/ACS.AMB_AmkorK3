using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using log4net;

namespace INA_ACS_Server
{
    public partial class WiseTowerLampService
    {
        private readonly static ILog WiseTowerLampLogger = LogManager.GetLogger("WiseTowerLampEvent"); //Function 실행관련 Log
        private readonly MainForm main;
        private readonly IUnitOfWork uow;


        public event EventHandler<WiseTowerLampModuleInPutReadEventArgs> WiseTowerLampModuleInPutReadData;    //WiseModule InPut Data Event
        public event EventHandler<WiseTowerLampModuleOutPutReadEventArgs> WiseTowerLampModuleOutPutReadData;  //WiseModule OutPut Data Event
        public event EventHandler<WiseTowerLampModuleConnectEventArgs> WiseTowerLampModuleConnected;          //WiseModule Connect Data Event


        public WiseTowerLampService(MainForm main, IUnitOfWork uow)
        {
            this.main = main;
            this.uow = uow;
        }
        public void Start()
        {
            Task.Run(() => Loop());
        }

        protected void Loop()
        {
            while (true)
            {
                try
                {
                    var WriteOutPutSignals = uow.WiseTowerLampConfigs.Find(m => m.TowerLampUseSetting == "Use" && m.serviceData.WriteOutputSignalFlag == 1 && !string.IsNullOrEmpty(m.IpAddressSetting)).ToList();

                    if (WriteOutPutSignals.Count() > 0)
                    {
                        foreach (var writeOutPutSignal in WriteOutPutSignals)
                        {
                            string[] writeOutPutSignalName = writeOutPutSignal.NameSetting.Split('#');
                            int writeOutPutSignalModuleNo = Convert.ToInt32(writeOutPutSignalName[1].Replace("_Ch", string.Empty));
                            string writeOutPutSignalChannelNo = writeOutPutSignalName[2];

                            if (WiseModule_ReST_Send(writeOutPutSignalModuleNo, writeOutPutSignal.IpAddressSetting, "PUT_DO_VALUE", writeOutPutSignalChannelNo, writeOutPutSignal.serviceData.WriteOutputSignalValue.ToString()) == 1)
                            {
                                writeOutPutSignal.serviceData.WriteOutputSignalFlag = 0;
                                WiseTowerLampLogger.Info($"WiseTowerLampMode ={writeOutPutSignal.ControlSetting}  , WiseTowerLampName = {writeOutPutSignal.NameSetting} " +
                                                         $", WiseTowerLampNameOutPutValue = {writeOutPutSignal.serviceData.WriteOutputSignalValue} , WriteOutputOnSignalTime = {writeOutPutSignal.serviceData.WriteOutputOnSignalTime} " +
                                                         $", WriteOutputOffSignalTime = {writeOutPutSignal.serviceData.WriteOutputOffSignalTime}");
                                if (writeOutPutSignal.serviceData.Status != "Connect")
                                {
                                    WiseTowerLampModuleConnected?.Invoke(this, new WiseTowerLampModuleConnectEventArgs(writeOutPutSignalModuleNo, true));
                                    WiseTowerLampLogger.Info($"WiseModule_{writeOutPutSignalModuleNo} Connect!");
                                }
                            }
                        }
                    }

                    for (int WiseTowerLampIndex = 0; WiseTowerLampIndex < ConfigData.WiseTowerLamp_MaxNum; WiseTowerLampIndex++)
                    {
                        int ModuleIndex = WiseTowerLampIndex + 1;

                        var wiseTowerLamp = uow.WiseTowerLampConfigs.Find(m => m.NameSetting.StartsWith($"TowerLampModule#{ModuleIndex}") && !string.IsNullOrWhiteSpace(m.IpAddressSetting) && m.TowerLampUseSetting == "Use").FirstOrDefault();

                        if (wiseTowerLamp != null)// 입력값 read
                        {
                            if (WiseModule_ReST_Send(ModuleIndex, wiseTowerLamp.IpAddressSetting, "GET_DI_VALUE", "", "") == 1)
                            {
                                if (wiseTowerLamp.serviceData.Status != "Connect")
                                {
                                    WiseTowerLampModuleConnected?.Invoke(this, new WiseTowerLampModuleConnectEventArgs(ModuleIndex, true));
                                    WiseTowerLampLogger.Info($"WiseModule_{ModuleIndex} Connect!");
                                }
                            }

                            // 출력값 read 
                            if (WiseModule_ReST_Send(ModuleIndex, wiseTowerLamp.IpAddressSetting, "GET_DO_VALUE", "", "") == 1)
                            {

                                if (wiseTowerLamp.serviceData.Status != "Connect")
                                {
                                    WiseTowerLampModuleConnected?.Invoke(this, new WiseTowerLampModuleConnectEventArgs(ModuleIndex, true));
                                    WiseTowerLampLogger.Info($"WiseModule_{ModuleIndex} Connect!");
                                }
                            }
                            else
                            {
                                if (wiseTowerLamp.serviceData.Status != "DisConnect")
                                {
                                    WiseTowerLampModuleConnected?.Invoke(this, new WiseTowerLampModuleConnectEventArgs(ModuleIndex, false));
                                    WiseTowerLampLogger.Info($"WiseModule_{ModuleIndex} DisConnect!");
                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException?.Message ?? ex.Message;
                    Debug.WriteLine(message);
                    WiseTowerLampLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                    main.ACS_UI_Log(message);
                }

            }

        }



        private bool preWiseModuleConnectionStatus = false; //Wise Module 상태 이전 값

        //public int WiseModule_ReST_Send(int nodeNo, string sRequestType, string sValue1, string sValue2)
        private int WiseModule_ReST_Send(int nodeNo, string ipAddress, string sRequestType, string sValue1, string sValue2)
        {

            int iJudge = 0;
            HttpClient WiseModuel_client = new HttpClient();

            try
            {
                object data;
                string sRecvMessage = string.Empty;
                HttpResponseMessage response = null;

                var byteArray = Encoding.ASCII.GetBytes("root:00000000"); //username:password
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                WiseModuel_client.DefaultRequestHeaders.Authorization = header;
                WiseModuel_client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en_US"));
                WiseModuel_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                WiseModuel_client.Timeout = TimeSpan.FromSeconds(ConfigData.WiseTowerLamp_ResponseTime); // 설정시간 이후에 타임아웃 에러
                WiseModuel_client.BaseAddress = new Uri("http://" + ipAddress + "/");

                switch (sRequestType)
                {
                    case "GET_DI_VALUE":
                        response = WiseModuel_client.GetAsync("di_value/slot_0").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            sRecvMessage = response.Content.ReadAsStringAsync().Result;
                            _Thread_ReST_Parsing_GetDiValue(nodeNo, sRecvMessage);
                            iJudge = 1;
                        }

                        break;

                    case "GET_DO_VALUE":
                        response = WiseModuel_client.GetAsync("do_value/slot_0").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            sRecvMessage = response.Content.ReadAsStringAsync().Result;

                            _Thread_ReST_Parsing_GetDoValue(nodeNo, sRecvMessage);
                            iJudge = 1;
                        }

                        break;

                    case "PUT_DO_VALUE":
                        data = new
                        {
                            Ch = int.Parse(sValue1),
                            //Md = 0,
                            Stat = int.Parse(sValue2),
                            Val = int.Parse(sValue2),
                            //PsCtn = 0,
                            //PsStop = 0,
                            //PsIV = 0
                        };

                        var myContent = JsonConvert.SerializeObject(data);
                        var buffer = Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        response = WiseModuel_client.PutAsync("do_value/slot_0/ch_" + sValue1, byteContent).Result;

                        if (response.IsSuccessStatusCode) iJudge = 1;

                        break;
                }
                if (iJudge != 1)
                {
                    string errorMessage = response != null
                        ? $"WiseModule REST API Error Log Req/Code/Reason = {sRequestType}/{response.StatusCode}/{response.ReasonPhrase}"
                        : $"WiseModule REST API Error Log Req/Code/Reason = {sRequestType}/None";

                    Console.WriteLine(errorMessage);

                }
                //Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(message);
                WiseTowerLampLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                main.ACS_UI_Log(message);
                iJudge = -1;
            }

            finally
            {
                WiseModuel_client.Dispose();

            }

            return iJudge;
        }

        #region Parsing관련

        private void _Thread_ReST_Parsing_GetDiValue(int nodeNo, string json)
        {

            string sCh = string.Empty;
            string InputChannel = string.Empty;
            string InputValue = string.Empty;
            try
            {
                JObject array = JObject.Parse(json);

                foreach (var item in array)
                {
                    if (item.Key.Equals("DIVal"))
                    {
                        JArray array1 = JArray.Parse(item.Value.ToString());
                        JObject inner;

                        foreach (JToken jToken in array1)
                        {
                            inner = jToken.Value<JObject>();

                            //sCh = inner["Ch"].ToString();
                            //wiseModuleModel.wise_InputSignal[int.Parse(sCh)] = inner["Stat"].ToString();
                            //WiseModuleInPutReadData?.Invoke(this, wiseModuleModel.wise_InputSignal[int.Parse(sCh)]);

                            InputChannel = inner["Ch"].ToString();
                            InputValue = inner["Stat"].ToString();
                            WiseTowerLampModuleInPutReadData?.Invoke(this, new WiseTowerLampModuleInPutReadEventArgs(nodeNo, InputChannel, InputValue));
                            WiseTowerLampLogger.Info($"Read , WiseTowerLampName = TowerLampModule#{nodeNo}_Ch{InputChannel} , WiseTowerLampNameInPutValue = {InputValue}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(message);
                WiseTowerLampLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                main.ACS_UI_Log(message);
            }
        }

        public void _Thread_ReST_Parsing_GetDoValue(int nodeNo, string json)
        {
            string sCh = string.Empty;
            string OutputChannel = string.Empty;
            string OutputValue = string.Empty;
            try
            {
                JObject array = JObject.Parse(json);

                foreach (var item in array)
                {
                    if (item.Key.Equals("DOVal"))
                    {
                        JArray array1 = JArray.Parse(item.Value.ToString());
                        JObject inner;

                        foreach (JToken jToken in array1)
                        {
                            inner = jToken.Value<JObject>();

                            //sCh = inner["Ch"].ToString();
                            //wiseModuleModel.wise_OutputSignal[int.Parse(sCh)] = inner["Stat"].ToString();

                            OutputChannel = inner["Ch"].ToString();
                            OutputValue = inner["Stat"].ToString();

                            WiseTowerLampModuleOutPutReadData?.Invoke(this, new WiseTowerLampModuleOutPutReadEventArgs(nodeNo, OutputChannel, OutputValue));
                            WiseTowerLampLogger.Info($"Read , WiseTowerLampName = TowerLampModule#{nodeNo}_Ch{OutputChannel} , WiseTowerLampNameOutPutValue = {OutputValue}");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(message);
                WiseTowerLampLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                main.ACS_UI_Log(message);
            }
        }

    }






    #endregion


}



