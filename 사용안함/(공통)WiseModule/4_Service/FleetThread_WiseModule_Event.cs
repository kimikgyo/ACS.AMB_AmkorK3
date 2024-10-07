using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {

        private void Setup_WiseModuleEventHandler() //Wise Module Event 선언
        {
            wiseModuleSvc.WiseModuleInPutReadData += WiseModuleInPutReadData;
            wiseModuleSvc.WiseModuleOutPutReadData += WiseModuleOutPutReadData;
            wiseModuleSvc.WiseModuleConnected += WiseModuleConnected;
        }

        private void WiseModuleConnected(object sender, WiseModuleConnectEventArgs e) //WiseModule Connect Data Event (Read)
        {
            var wisModuleSelect = uow.WiseModuleConfig.Find(m => m.ModuleName.StartsWith($"WiseModule#{e.NodeNo}")).FirstOrDefault();
            if (wisModuleSelect != null)
            {
                foreach (var target in uow.WiseModuleConfig.Find(m => m.ModuleName.StartsWith($"WiseModule#{e.NodeNo}")))
                {
                    if (e.ConnectionState) target.ModuleStatus = "Connect";
                    else target.ModuleStatus = "DisConnect";
                }
                uow.WiseModuleConfig.ModuleUpdate(wisModuleSelect);
            }
        }

        private void WiseModuleInPutReadData(object sender, WiseModuleInPutReadEventArgs e) //WiseModule InPut Data Event (Read)
        {
            var wiseModuleSelect = uow.WiseModuleConfig.Find(m => m.ModuleName.StartsWith($"WiseModule#{e.NodeNo}_Ch#{e.InPutchannel}")).FirstOrDefault();
            if (wiseModuleSelect != null)
            {
                wiseModuleSelect.ModuleIn_Value = Convert.ToInt32(e.InPutValue);
                uow.WiseModuleConfig.Update(wiseModuleSelect);
            }
        }

        private void WiseModuleOutPutReadData(object sender, WiseModuleOutPutReadEventArgs e) //WiseModule OutPut Data Event (Read)
        {
            var wiseModuleSelect = uow.WiseModuleConfig.Find(m => m.ModuleName.StartsWith($"WiseModule#{e.NodeNo}_Ch#{e.OutPutchannel}")).FirstOrDefault();
            if (wiseModuleSelect != null)
            {
                wiseModuleSelect.ModuleOut_Value = Convert.ToInt32(e.OutPutValue);
                uow.WiseModuleConfig.Update(wiseModuleSelect);
            }
        }
    }







    #region Wise Module Data 값을 Read 또는 Write 하기 위한 Event
    public class WiseModuleConnectEventArgs : EventArgs           //WiseModule InPut Data Event (Read)
    {
        public int NodeNo;
        public bool ConnectionState;

        public WiseModuleConnectEventArgs(int nodeNo, bool connectionState)
        {
            this.NodeNo = nodeNo;
            this.ConnectionState = connectionState;
        }
    }

    public class WiseModuleInPutReadEventArgs : EventArgs           //WiseModule InPut Data Event (Read)
    {
        public int NodeNo;
        public string InPutchannel;
        public string InPutValue;

        public WiseModuleInPutReadEventArgs(int nodeNo, string inputchannel, string inputValue)
        {
            this.NodeNo = nodeNo;
            this.InPutchannel = inputchannel;
            this.InPutValue = inputValue;
        }
    }

    public class WiseModuleOutPutReadEventArgs : EventArgs          //WiseModule OutPut Data Event (Read)
    {
        public int NodeNo;
        public string OutPutchannel;
        public string OutPutValue;

        public WiseModuleOutPutReadEventArgs(int nodeNo, string outPutchannel, string outPutValue)
        {
            this.NodeNo = nodeNo;
            this.OutPutchannel = outPutchannel;
            this.OutPutValue = outPutValue;
        }
    }
    #endregion
}
