using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {

        private void Setup_WiseTowerLampModuleEventHandler() //Wise Module Event 선언
        {
            wiseTowerLampSvc.WiseTowerLampModuleConnected += WiseTowerLampModuleConnected;
            wiseTowerLampSvc.WiseTowerLampModuleInPutReadData += WiseTowerLampModuleInPutReadData;
            wiseTowerLampSvc.WiseTowerLampModuleOutPutReadData += WiseTowerLampModuleOutPutReadData;
        }

        private void WiseTowerLampModuleConnected(object sender, WiseTowerLampModuleConnectEventArgs e)
        {
            var wiseTowerLampModuleSelect = uow.WiseTowerLampConfigs.Find(m => m.NameSetting.StartsWith($"TowerLampModule#{e.NodeNo}")).ToList();

            if (wiseTowerLampModuleSelect.Count() > 0)
            {
                foreach (var wiseTowerLampConnectChState in wiseTowerLampModuleSelect)
                {
                    if (e.ConnectionState) wiseTowerLampConnectChState.serviceData.Status = "Connect";
                    else wiseTowerLampConnectChState.serviceData.Status = "DisConnect";
                }
            }
        }
        

        private void WiseTowerLampModuleInPutReadData(object sender, WiseTowerLampModuleInPutReadEventArgs e)
        {
            var wiseTowerLampModuleSelect = uow.WiseTowerLampConfigs.Find(m => m.NameSetting.StartsWith($"TowerLampModule#{e.NodeNo}_Ch#{e.InPutchannel}")).FirstOrDefault();
            if (wiseTowerLampModuleSelect != null) wiseTowerLampModuleSelect.serviceData.Module_InValue = Convert.ToInt32(e.InPutValue);
        }

        private void WiseTowerLampModuleOutPutReadData(object sender, WiseTowerLampModuleOutPutReadEventArgs e)
        {
            var wiseTowerLampModuleSelect = uow.WiseTowerLampConfigs.Find(m => m.NameSetting.StartsWith($"TowerLampModule#{e.NodeNo}_Ch#{e.OutPutchannel}")).FirstOrDefault();
            if (wiseTowerLampModuleSelect != null) wiseTowerLampModuleSelect.serviceData.Module_OutValue = Convert.ToInt32(e.OutPutValue);
        }

    }

    #region Wise TowerLamp Module Data 값을 Read 또는 Write 하기 위한 Event

    public class WiseTowerLampModuleInPutReadEventArgs : EventArgs           //WiseModule InPut Data Event (Read)
    {
        public int NodeNo;
        public string InPutchannel;
        public string InPutValue;

        public WiseTowerLampModuleInPutReadEventArgs(int nodeNo, string inputchannel, string inputValue)
        {
            this.NodeNo = nodeNo;
            this.InPutchannel = inputchannel;
            this.InPutValue = inputValue;
        }
    }

    public class WiseTowerLampModuleOutPutReadEventArgs : EventArgs          //WiseModule OutPut Data Event (Read)
    {
        public int NodeNo;
        public string OutPutchannel;
        public string OutPutValue;

        public WiseTowerLampModuleOutPutReadEventArgs(int nodeNo, string outPutchannel, string outPutValue)
        {
            this.NodeNo = nodeNo;
            this.OutPutchannel = outPutchannel;
            this.OutPutValue = outPutValue;
        }
    }
    public class WiseTowerLampModuleConnectEventArgs : EventArgs           //WiseModule InPut Data Event (Read)
    {
        public int NodeNo;
        public bool ConnectionState;

        public WiseTowerLampModuleConnectEventArgs(int nodeNo, bool connectionState)
        {
            this.NodeNo = nodeNo;
            this.ConnectionState = connectionState;
        }
    }
    #endregion
}
