using INA_ACS_Server.OPWindows;
using INA_ACS_Server.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class WiseTowerLampConfigsScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        public WiseTowerLampConfigsScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_WiseTowerLampModule.AlternatingRowsDefaultCellStyle = null;
            dtg_WiseTowerLampModule.DoubleBuffered(true);
            WiseTowerLampModuleSettingInit();
            WiseTowerLampModuleSetting_Display();
        }

        private void WiseTowerLampModuleSettingInit()
        {
            btn_RegistarMode.BackColor = Color.WhiteSmoke;
            btn_BarCodeMode.BackColor = Color.WhiteSmoke;
            btn_AutoControl.BackColor = Color.WhiteSmoke;
            btn_ManualControl.BackColor = Color.WhiteSmoke;
            btn_Display.BackColor = Color.WhiteSmoke;
            btn_ProductNameInfo.BackColor = Color.WhiteSmoke;

            txt_TowerLampWiseModuleMaxNum.Text = ConfigData.WiseTowerLamp_MaxNum.ToString();
            txt_WiseTowerLampResponseTime.Text = ConfigData.WiseTowerLamp_ResponseTime.ToString();
            txt_TowerLampWiseModuleGroup.Text = ConfigData.WiseTowerLamp_ModuleGroup;
            dtg_WiseTowerLampModule.ScrollBars = ScrollBars.Both;
            dtg_WiseTowerLampModule.AllowUserToResizeColumns = true;
            dtg_WiseTowerLampModule.ColumnHeadersHeight = 40;
            dtg_WiseTowerLampModule.RowTemplate.Height = 40;
            dtg_WiseTowerLampModule.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WiseTowerLampModule.Columns[0].CellTemplate;
            dtg_WiseTowerLampModule.Columns.Clear();
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_Mode", HeaderText = "ControlMode", Width = 130, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_Use", HeaderText = "Use/UnUse", Width = 80, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_Name", HeaderText = "TowerLampModuleName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_DisplayName", HeaderText = "TowerLampDisplayName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_Zone", HeaderText = "Zone", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_IpAddress", HeaderText = "IpAddress", Width = 110, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_Operatingtime", HeaderText = "Operatingtime", Width = 100, CellTemplate = currentCellTemplate });
            //dtg_WiseTowerLampModule.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_WiseTowerLampModule_ProductValue", HeaderText = "ProductValue", Width = 125 });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_WiseTowerLampModule_ProductAcitve", HeaderText = "ProductAcitve", Width = 100 });
            dtg_WiseTowerLampModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseTowerLampModule_ProductName", HeaderText = "ProductName", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseTowerLampModule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WiseTowerLampModule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WiseTowerLampModule.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Use"].HeaderText = "Use" + "\r\n" + "Unuse";
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Name"].HeaderText = "TowerLamp" + "\r\n" + "ModuleName";
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Operatingtime"].HeaderText = "Operating" + "\r\n" + "time";
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_DisplayName"].HeaderText = "TowerLamp" + "\r\n" + "DisplayName";
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_ProductAcitve"].HeaderText = "Product" + "\r\n" + "Acitve";
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_ProductName"].HeaderText = "Product" + "\r\n" + "Name";

            dtg_WiseTowerLampModule.Rows.Clear();
        }

        private void WiseTowerLampModuleSetting_Display()
        {
            var ControlValue = uow.WiseTowerLampConfigs.Find(m => m.ControlSetting == "AutoControl").FirstOrDefault();
            if (ControlValue != null)
            {
                btn_AutoControl.BackColor = Color.YellowGreen;
                btn_ManualControl.BackColor = Color.WhiteSmoke;
            }
            else
            {
                btn_AutoControl.BackColor = Color.WhiteSmoke;
                btn_ManualControl.BackColor = Color.YellowGreen;
            }
            var Mode = uow.ACSModeInfo.Find(a => a.Location == "WiseTowerLamp" && a.ACSMode == "RegistarMode").FirstOrDefault();
            if (Mode != null)
            {
                btn_RegistarMode.BackColor = Color.YellowGreen;
                btn_BarCodeMode.BackColor = Color.WhiteSmoke;
            }
            else
            {
                btn_RegistarMode.BackColor = Color.WhiteSmoke;
                btn_BarCodeMode.BackColor = Color.YellowGreen;
            }

            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Id"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Use"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Mode"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Name"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Zone"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Operatingtime"].ReadOnly = true;
            //dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_ProductValue"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_ProductAcitve"].ReadOnly = true;
            dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_ProductName"].ReadOnly = true;
            //dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Register22Value"].ReadOnly = true;
            //dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Register32Value"].ReadOnly = true;
            //dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Register33Value"].ReadOnly = true;
            //dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Register34Value"].ReadOnly = true;
            //dtg_WiseTowerLampModule.Columns["DGV_WiseTowerLampModule_Register35Value"].ReadOnly = true;

            int firstRowIndex = dtg_WiseTowerLampModule.FirstDisplayedScrollingColumnIndex;
            dtg_WiseTowerLampModule.Rows.Clear();
            dtg_WiseTowerLampModule.RowTemplate.Height = 40;
            foreach (var wiseTowerLampConfigs in uow.WiseTowerLampConfigs.GetAll())
            {
                int newRowIndex = dtg_WiseTowerLampModule.Rows.Add();
                var newRow = dtg_WiseTowerLampModule.Rows[newRowIndex];
                newRow.Cells["DGV_WiseTowerLampModule_Id"].Value = wiseTowerLampConfigs.Id.ToString();
                newRow.Cells["DGV_WiseTowerLampModule_Use"].Value = wiseTowerLampConfigs.TowerLampUseSetting;
                newRow.Cells["DGV_WiseTowerLampModule_Mode"].Value = wiseTowerLampConfigs.ControlSetting;
                newRow.Cells["DGV_WiseTowerLampModule_Name"].Value = wiseTowerLampConfigs.NameSetting;
                newRow.Cells["DGV_WiseTowerLampModule_DisplayName"].Value = wiseTowerLampConfigs.DisplayNameSetting;
                newRow.Cells["DGV_WiseTowerLampModule_Zone"].Value = wiseTowerLampConfigs.PositionZoneSetting;
                newRow.Cells["DGV_WiseTowerLampModule_IpAddress"].Value = wiseTowerLampConfigs.IpAddressSetting;
                newRow.Cells["DGV_WiseTowerLampModule_Operatingtime"].Value = wiseTowerLampConfigs.OperationtimeSetting + "Sec";
                //newRow.Cells["DGV_WiseTowerLampModule_ProductValue"].Value = wiseTowerLampConfigs.ProductValueSetting;
                newRow.Cells["DGV_WiseTowerLampModule_ProductAcitve"].Value = wiseTowerLampConfigs.ProductActiveSetting;
                newRow.Cells["DGV_WiseTowerLampModule_ProductName"].Value = wiseTowerLampConfigs.productName;

                newRow.Tag = wiseTowerLampConfigs;

                if (wiseTowerLampConfigs.ControlSetting == "ManualControl" || wiseTowerLampConfigs.TowerLampUseSetting == "Unuse") dtg_WiseTowerLampModule.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                else dtg_WiseTowerLampModule.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;

                //if (wiseTowerLampConfigs.ProductActiveSetting == true) dtg_WiseTowerLampModule.Rows[newRowIndex].Cells[newRow.Cells["DGV_WiseTowerLampModule_ProductValue"].ColumnIndex].Style.BackColor = Color.White;
                //else dtg_WiseTowerLampModule.Rows[newRowIndex].Cells[newRow.Cells["DGV_WiseTowerLampModule_ProductValue"].ColumnIndex].Style.BackColor = Color.Black;



            }
            if (firstRowIndex >= 0 && firstRowIndex < dtg_WiseTowerLampModule.Rows.Count)
            {
                dtg_WiseTowerLampModule.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_WiseTowerLampModule.ClearSelection();
        }


        private void dtg_TowerLampWiseModule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                WiseTowerLampConfigModel selectedRowTag = (WiseTowerLampConfigModel)selectedRow.Tag;
                WiseTowerLampConfigModel targetConfig = uow.WiseTowerLampConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_DisplayName"].Index)
                    {
                        if (targetConfig.DisplayNameSetting != newValue)
                        {
                            string oldValue = targetConfig.DisplayNameSetting;
                            targetConfig.DisplayNameSetting = newValue;
                            ChangeDatamsg = $"WiseTowerLampConfigs,WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp DisplayName changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }

                    else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_IpAddress"].Index)
                    {

                        bool flag = System.Net.IPAddress.TryParse(newValue, out var newIp); // check ip address
                        string newIpAddress = flag ? newIp.ToString() : targetConfig.IpAddressSetting;

                        //var TowerLampnewIpAddress = uow.WiseTowerLampConfigs.Find(m => m.IpAddressSetting == newIpAddress).FirstOrDefault();
                        //if (TowerLampnewIpAddress == null || (newValue == "0.0.0.0"))
                        if (newValue != "0.0.0.0")
                        {
                            if (targetConfig.IpAddressSetting != newValue)
                            {
                                string oldIpAddress = targetConfig.IpAddressSetting;
                                foreach (var target in uow.WiseTowerLampConfigs.Find(m => m.NameSetting.Split('_')[0] == targetConfig.NameSetting.Split('_')[0]))
                                {
                                    target.IpAddressSetting = newIpAddress;
                                }
                                uow.WiseTowerLampConfigs.IpAddressUpdate(targetConfig);
                                ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp IpAddress Change from {oldIpAddress} to {newIpAddress}";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show("다시확인후 입력해주시기 바랍니다.");
                            WiseTowerLampModuleSetting_Display();
                        }

                    }

                }

                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    WiseTowerLampModuleSetting_Display();
                }
                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.WiseTowerLampConfigs.Update(targetConfig);
                        string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                        mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");
                        WiseTowerLampModuleSetting_Display();
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(message);
                //EventLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
                mainForm.ACS_UI_Log(message);
            }
        }

        private void dtg_TowerLampWiseModule_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];


            WiseTowerLampConfigModel selectedRowTag = (WiseTowerLampConfigModel)selectedRow.Tag;
            WiseTowerLampConfigModel targetConfig = uow.WiseTowerLampConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();
                        if (targetConfig.TowerLampUseSetting != newValue)
                        {
                            string oldValue = targetConfig.TowerLampUseSetting;
                            targetConfig.TowerLampUseSetting = newValue;
                            insertUse.Close();
                            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                            if (targetConfig.ControlSetting == "ManualControl" || targetConfig.TowerLampUseSetting == "Unuse") dtg_WiseTowerLampModule.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                            else dtg_WiseTowerLampModule.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Zone"].Index)
                {
                    var insert = new PositionZoneSelectForm(ConfigData.WiseTowerLamp_ModuleGroup,uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.PositionZoneSetting != newValue)
                        {
                            string oldValue = targetConfig.PositionZoneSetting;
                            targetConfig.PositionZoneSetting = newValue;
                            insert.Close();
                            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Module_Zone Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }

                }
                else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Operatingtime"].Index)
                {
                    //TowerLamp On시간
                    NumPadForm insertNum = new NumPadForm(dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WiseTowerLampModule", "0", "86400");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();
                        if (Convert.ToString(targetConfig.OperationtimeSetting) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.OperationtimeSetting);
                            targetConfig.OperationtimeSetting = Convert.ToInt32(newValue);
                            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Module_Operatingtime Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}Sec", ChangeDatamsg);
                        }

                    }
                }
                //else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_ProductValue"].Index)
                //{
                //    bool oldValue = Convert.ToBoolean(selectedRow.Cells["DGV_WiseTowerLampModule_ProductValue"].Value);
                //    bool newValue = !oldValue;

                //    selectedCell.Value = newValue;
                //    targetConfig.ProductValueSetting = newValue;
                //    uow.WiseTowerLampConfigs.Update(targetConfig);

                //    mainForm.UserLog("TowerLamp Screen", $"WiseTowerLamp Config {targetConfig.Id} WiseTowerLamp ProductValue Change from {oldValue} to {newValue}");
                //}
                else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_ProductAcitve"].Index)
                {
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_WiseTowerLampModule_ProductAcitve"].Value);
                    if (targetConfig.ProductActiveSetting != newValue)
                    {
                        bool oldValue = targetConfig.ProductActiveSetting;
                        targetConfig.ProductActiveSetting = newValue;

                        //if (targetConfig.ProductActiveSetting == true) dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells["DGV_WiseTowerLampModule_ProductValue"].Style.BackColor = Color.White;
                        //else dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells["DGV_WiseTowerLampModule_ProductValue"].Style.BackColor = Color.Black;
                        ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Module_ProductAcitve Change from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_ProductName"].Index)
                {
                    var insert = new ProductNameSelectForm(mainForm, uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.productName != newValue)
                        {
                            string oldValue = targetConfig.productName;
                            targetConfig.productName = newValue;
                            insert.Close();
                            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Module_ProductName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                //else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Register22Value"].Index)
                //{
                //    var insertNum = new NumPadForm(dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WiseTowerLampModule", "RT31");
                //    if (insertNum.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertNum.InputValue.Trim();
                //        if (Convert.ToString(targetConfig.Register22ValueSetting) != newValue)
                //        {
                //            string oldValue = Convert.ToString(targetConfig.Register22ValueSetting);
                //            targetConfig.Register22ValueSetting = int.Parse(newValue);
                //            insertNum.Close();
                //            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Register22Value Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
                //else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Register32Value"].Index)
                //{
                //    var insertNum = new NumPadForm(dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WiseTowerLampModule", "RT31");
                //    if (insertNum.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertNum.InputValue.Trim();
                //        if (Convert.ToString(targetConfig.Register32ValueSetting) != newValue)
                //        {
                //            string oldValue = Convert.ToString(targetConfig.Register32ValueSetting);
                //            targetConfig.Register32ValueSetting = int.Parse(newValue);
                //            insertNum.Close();
                //            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Register32Value Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
                //else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Register33Value"].Index)
                //{
                //    var insertNum = new NumPadForm(dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WiseTowerLampModule", "RT31");
                //    if (insertNum.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertNum.InputValue.Trim();
                //        if (Convert.ToString(targetConfig.Register33ValueSetting) != newValue)
                //        {
                //            string oldValue = Convert.ToString(targetConfig.Register33ValueSetting);
                //            targetConfig.Register33ValueSetting = int.Parse(newValue);
                //            insertNum.Close();
                //            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Register33Value Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
                //else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Register34Value"].Index)
                //{
                //    var insertNum = new NumPadForm(dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WiseTowerLampModule", "RT31");
                //    if (insertNum.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertNum.InputValue.Trim();
                //        if (Convert.ToString(targetConfig.Register34ValueSetting) != newValue)
                //        {
                //            string oldValue = Convert.ToString(targetConfig.Register34ValueSetting);
                //            targetConfig.Register34ValueSetting = int.Parse(newValue);
                //            insertNum.Close();
                //            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Register34Value Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
                //else if (e.ColumnIndex == grid.Columns["DGV_WiseTowerLampModule_Register35Value"].Index)
                //{
                //    var insertNum = new NumPadForm(dtg_WiseTowerLampModule.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WiseTowerLampModule", "RT31");
                //    if (insertNum.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = insertNum.InputValue.Trim();
                //        if (Convert.ToString(targetConfig.Register35ValueSetting) != newValue)
                //        {
                //            string oldValue = Convert.ToString(targetConfig.Register35ValueSetting);
                //            targetConfig.Register35ValueSetting = int.Parse(newValue);
                //            insertNum.Close();
                //            ChangeDatamsg = $"WiseTowerLampConfigs, WiseTowerLamp Config{targetConfig.Id} WiseTowerLamp Register35Value Change from {oldValue} to {newValue}";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                WiseTowerLampModuleSetting_Display();
            }
            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.WiseTowerLampConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }
        }

        private void txt_WiseTowerLampConfig_Click(object sender, EventArgs e)
        {

            string TextBoxName = ((TextBox)sender).Name;
            switch (TextBoxName)
            {
                case "txt_TowerLampWiseModuleMaxNum":
                    NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT21");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = int.Parse(insertNum.InputValue).ToString();
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;

                            AppConfiguration.ConfigDataSetting("WiseTowerLamp_MaxNum", newValue);
                            ConfigData.WiseTowerLamp_MaxNum = int.Parse(AppConfiguration.GetAppConfig("WiseTowerLamp_MaxNum"));
                            insertNum.Close();

                            mainForm.UserLog("TowerLamp Screen", $"WiseTowerLamp_MaxNum Change from {oldValue} to {newValue}");
                            uow.WiseTowerLampConfigs.Validate_DB_Items();
                            WiseTowerLampModuleSetting_Display();
                        }
                    }
                    break;
                case "txt_WiseTowerLampResponseTime":

                    NumPadForm insertNumResponseTime = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT21");
                    if (insertNumResponseTime.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = int.Parse(insertNumResponseTime.InputValue).ToString();
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;

                            AppConfiguration.ConfigDataSetting("WiseTowerLamp_ResponseTime", newValue);
                            ConfigData.WiseTowerLamp_ResponseTime = int.Parse(AppConfiguration.GetAppConfig("WiseTowerLamp_ResponseTime"));
                            insertNumResponseTime.Close();
                            mainForm.UserLog("TowerLamp Screen", $"WiseTowerLampResponseTime Change from {oldValue} to {newValue}");

                            WiseTowerLampModuleSetting_Display();
                        }
                    }
                    break;
                case "txt_TowerLampWiseModuleGroup":

                    var insertNumModuleGroup = new GroupSelectForm(mainForm, uow);
                    if (insertNumModuleGroup.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNumModuleGroup.InputValue.Trim();

                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;

                            AppConfiguration.ConfigDataSetting("WiseTowerLamp_ModuleGroup", newValue);
                            ConfigData.WiseTowerLamp_ModuleGroup = AppConfiguration.GetAppConfig("WiseTowerLamp_ModuleGroup");
                            insertNumModuleGroup.Close();
                            mainForm.UserLog("TowerLamp Screen", $"TowerLampWiseModuleGroup Change from {oldValue} to {newValue}");

                            WiseTowerLampModuleSetting_Display();
                        }
                       
                    }
                    break;
            }
        }

        private void btn_ScreenButton_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            string ModeChange = "";

            switch (buttonName)
            {
                case "btn_AutoControl":
                    ModeChange = "AutoControl";
                    btn_AutoControl.BackColor = Color.YellowGreen;
                    btn_ManualControl.BackColor = Color.WhiteSmoke;
                    btn_Display.BackColor = Color.WhiteSmoke;
                    break;

                case "btn_ManualControl":
                    ModeChange = "ManualControl";
                    btn_ManualControl.BackColor = Color.YellowGreen;
                    btn_AutoControl.BackColor = Color.WhiteSmoke;
                    btn_Display.BackColor = Color.WhiteSmoke;
                    break;
                case "btn_Display":
                    var wiseTowerLampMonitoringScreen = new WiseTowerLampMonitoringScreen(uow);
                    wiseTowerLampMonitoringScreen.Show(this);
                    break;

                case "btn_WiseTowerLampModuleBackUp":
                    if (dtg_WiseTowerLampModule.Rows.Count == 0) return;
                    mainForm.SaveAsDataGridviewToCSV(dtg_WiseTowerLampModule);
                    break;

                case "btn_ProductNameInfo":
                    var productNameInfoScreen = new ProductNameInfoScreen(mainForm, uow);
                    productNameInfoScreen.Show(this);
                    break;
                case "btn_RegistarMode":
                    ModeChange = "RegistarMode";
                    btn_RegistarMode.BackColor = Color.YellowGreen;
                    btn_BarCodeMode.BackColor = Color.WhiteSmoke;
                    break;
                case "btn_BarCodeMode":
                    ModeChange = "BarCodeMode";
                    btn_RegistarMode.BackColor = Color.WhiteSmoke;
                    btn_BarCodeMode.BackColor = Color.YellowGreen;
                    break;
            }
            if (ModeChange.Length > 0)
            {
                if (ModeChange == "AutoControl" || ModeChange == "ManualControl")
                {
                    var ControlMode = uow.WiseTowerLampConfigs.Find(m => m.ControlSetting == ModeChange).FirstOrDefault();
                    if (ControlMode == null)
                    {
                        uow.WiseTowerLampConfigs.ControlModeUpdate(ModeChange);
                        uow.WiseTowerLampConfigs.Validate_DB_Items();
                        WiseTowerLampModuleSetting_Display();
                        mainForm.UserLog("TowerLamp Screen", $"WiseTowerLamp Control Change Click {ModeChange}");

                    }
                    if (ModeChange == "ManualControl")    //버튼을 누른후 show 꺼야지만 DataBase 업데이트 하는 문제발생하여 조건을 아래 조건으로 추가
                    {
                        var wiseTowerLampManualScreen = new WiseTowerLampManualScreen(mainForm, uow);
                        wiseTowerLampManualScreen.Show(this);
                    }
                }
                else if (ModeChange == "RegistarMode" || ModeChange == "BarCodeMode")
                {
                    var modeChange = uow.ACSModeInfo.Find(a => a.Location == "WiseTowerLamp").FirstOrDefault();
                    if (modeChange != null && modeChange.ACSMode != ModeChange)
                    {
                        modeChange.ACSMode = ModeChange;
                        uow.ACSModeInfo.Update(modeChange);
                        WiseTowerLampModuleSetting_Display();
                        mainForm.UserLog("TowerLamp Screen", $"WiseTowerLamp Mode Change Click {ModeChange}");
                    }
                }

            }

        }
    }
}
