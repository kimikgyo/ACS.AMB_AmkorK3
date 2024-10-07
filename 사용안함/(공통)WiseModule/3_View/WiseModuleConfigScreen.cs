using INA_ACS_Server.Models.AmkorK5_M3F_T3F;
using INA_ACS_Server.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server.Views
{
    public partial class WiseModuleConfigScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        public WiseModuleConfigScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_WiseModuleConfig.AlternatingRowsDefaultCellStyle = null;
            dtg_WiseModuleMonitoring.AlternatingRowsDefaultCellStyle = null;
            dtg_WiseModuleConfig.DoubleBuffered(true);
            dtg_WiseModuleMonitoring.DoubleBuffered(true);

            ButtonInit();
            TextBoxInit();
            WiseModuleInit();
            WiseModule_Display();
            WiseModuleMonitoringInit();
            WiseModuleMonitoringDisplay();
        }

        private void ButtonInit()
        {
            btn_AutoControl.BackColor = Color.WhiteSmoke;
            btn_ManualControl.BackColor = Color.WhiteSmoke;
        }

        private void TextBoxInit()
        {
            txt_WiseModule_MaxNum.Text = ConfigData.WiseModule_MaxNum.ToString();
            txt_WiseModuleResponseTime.Text = ConfigData.WiseModuleResponseTime.ToString();
        }

        private void WiseModuleInit()
        {
            dtg_WiseModuleConfig.ScrollBars = ScrollBars.Both;
            dtg_WiseModuleConfig.AllowUserToResizeColumns = true;
            dtg_WiseModuleConfig.ColumnHeadersHeight = 40;
            dtg_WiseModuleConfig.RowTemplate.Height = 40;
            dtg_WiseModuleConfig.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WiseModuleConfig.Columns[0].CellTemplate;
            dtg_WiseModuleConfig.Columns.Clear();
            dtg_WiseModuleConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModule_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WiseModuleConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModule_Mode", HeaderText = "ControlMode", Width = 130, CellTemplate = currentCellTemplate });
            dtg_WiseModuleConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModule_Use", HeaderText = "Use/UnUse", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseModuleConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModule_Name", HeaderText = "TowerLampModuleName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseModuleConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModule_DisplayName", HeaderText = "TowerLampDisplayName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseModuleConfig.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModule_IpAddress", HeaderText = "IpAddress", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseModuleConfig.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WiseModuleConfig.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WiseModuleConfig.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_WiseModuleConfig.Columns["DGV_WiseModule_Use"].HeaderText = "Use" + "\r\n" + "Unuse";
            dtg_WiseModuleConfig.Columns["DGV_WiseModule_Name"].HeaderText = "TowerLamp" + "\r\n" + "ModuleName";
            dtg_WiseModuleConfig.Columns["DGV_WiseModule_DisplayName"].HeaderText = "TowerLamp" + "\r\n" + "DisplayName";
            dtg_WiseModuleConfig.Rows.Clear();
        }

        private void WiseModule_Display()
        {
            var ControlValue = uow.WiseModuleConfig.Find(m => m.ModuleControlMode == "AutoControl").FirstOrDefault();
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

            dtg_WiseModuleConfig.Columns["DGV_WiseModule_Id"].ReadOnly = true;
            dtg_WiseModuleConfig.Columns["DGV_WiseModule_Use"].ReadOnly = true;
            dtg_WiseModuleConfig.Columns["DGV_WiseModule_Mode"].ReadOnly = true;
            dtg_WiseModuleConfig.Columns["DGV_WiseModule_Name"].ReadOnly = true;

            int firstRowIndex = dtg_WiseModuleConfig.FirstDisplayedScrollingColumnIndex;
            dtg_WiseModuleConfig.Rows.Clear();
            dtg_WiseModuleConfig.RowTemplate.Height = 40;
            foreach (var wiseModuleConfigs in uow.WiseModuleConfig.GetAll())
            {
                int newRowIndex = dtg_WiseModuleConfig.Rows.Add();
                var newRow = dtg_WiseModuleConfig.Rows[newRowIndex];
                newRow.Cells["DGV_WiseModule_Id"].Value = wiseModuleConfigs.Id.ToString();
                newRow.Cells["DGV_WiseModule_Use"].Value = wiseModuleConfigs.ModuleUse;
                newRow.Cells["DGV_WiseModule_Mode"].Value = wiseModuleConfigs.ModuleControlMode;
                newRow.Cells["DGV_WiseModule_Name"].Value = wiseModuleConfigs.ModuleName;
                newRow.Cells["DGV_WiseModule_DisplayName"].Value = wiseModuleConfigs.DisplayName;
                newRow.Cells["DGV_WiseModule_IpAddress"].Value = wiseModuleConfigs.ModuleIpAddress;
                newRow.Tag = wiseModuleConfigs;

                if (wiseModuleConfigs.ModuleControlMode == "ManualControl" || wiseModuleConfigs.ModuleUse == "Unuse") dtg_WiseModuleConfig.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                else dtg_WiseModuleConfig.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;

                //if (wiseTowerLampConfigs.ProductActiveSetting == true) dtg_WiseTowerLampModule.Rows[newRowIndex].Cells[newRow.Cells["DGV_WiseTowerLampModule_ProductValue"].ColumnIndex].Style.BackColor = Color.White;
                //else dtg_WiseTowerLampModule.Rows[newRowIndex].Cells[newRow.Cells["DGV_WiseTowerLampModule_ProductValue"].ColumnIndex].Style.BackColor = Color.Black;

            }
            if (firstRowIndex >= 0 && firstRowIndex < dtg_WiseModuleConfig.Rows.Count)
            {
                dtg_WiseModuleConfig.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_WiseModuleConfig.ClearSelection();
        }

        private void dtg_WiseModule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                WiseModuleConfigModel selectedRowTag = (WiseModuleConfigModel)selectedRow.Tag;
                WiseModuleConfigModel targetConfig = uow.WiseModuleConfig.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_WiseModule_DisplayName"].Index)
                    {
                        if (targetConfig.DisplayName != newValue)
                        {
                            string oldValue = targetConfig.DisplayName;
                            targetConfig.DisplayName = newValue;
                            ChangeDatamsg = $"WiseModuleConfig,WiseModuleConfig Config{targetConfig.Id} WiseModule DisplayName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }

                    else if (e.ColumnIndex == grid.Columns["DGV_WiseModule_IpAddress"].Index)
                    {

                        bool flag = System.Net.IPAddress.TryParse(newValue, out var newIp); // check ip address
                        string newIpAddress = flag ? newIp.ToString() : targetConfig.ModuleIpAddress;

                        var WiseModulenewIpAddress = uow.WiseModuleConfig.Find(m => m.ModuleIpAddress == newIpAddress).FirstOrDefault();
                        if (WiseModulenewIpAddress == null || (newValue == "0.0.0.0"))
                        {
                            if (targetConfig.ModuleIpAddress != newIpAddress)
                            {
                                string oldIpAddress = targetConfig.ModuleIpAddress;
                                foreach (var target in uow.WiseModuleConfig.Find(m => m.ModuleName.Split('_')[0] == targetConfig.ModuleName.Split('_')[0]))
                                {
                                    target.ModuleIpAddress = newIpAddress; // uow(리스트) 내용 변경
                                }
                                uow.WiseModuleConfig.ModuleUpdate(targetConfig);
                                ChangeDatamsg = $"WiseModuleConfig,WiseModuleConfig Config{targetConfig.Id} WiseModule IpAddress Change from {oldIpAddress} to {newIpAddress}";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show("같은 IpAddress가 있습니다." + "\r\n" + "다시확인후 입력해주시기 바랍니다.");
                            WiseModule_Display();
                        }

                    }

                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    WiseModule_Display();
                }
                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.WiseModuleConfig.Update(targetConfig);
                        string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                        mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");
                        WiseModule_Display();
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

        private void dtg_WiseModule_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            WiseModuleConfigModel selectedRowTag = (WiseModuleConfigModel)selectedRow.Tag;
            WiseModuleConfigModel targetConfig = uow.WiseModuleConfig.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_WiseModule_Use"].Index)
                {
                    var insert = new UseSelectForm();
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.ModuleUse != newValue)
                        {
                            string oldValue = targetConfig.ModuleUse;
                            targetConfig.ModuleUse = newValue;
                            insert.Close();
                            ChangeDatamsg = $"WiseModuleConfig,WiseModuleConfig Config{targetConfig.Id} WiseMouduleUse Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                            if (targetConfig.ModuleControlMode == "ManualControl" || targetConfig.ModuleUse == "Unuse") dtg_WiseModuleConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                            else dtg_WiseModuleConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                WiseModule_Display();
            }

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.WiseModuleConfig.Update(targetConfig);
                    uow.WiseModuleConfig.Validate_DB_Items();
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }

        }

        private void txt_WiseModuleConfig_Click(object sender, EventArgs e)
        {
            string ChangeDatamsg = null;
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT21");
            DialogResult result = insertNum.ShowDialog();
            if (result == DialogResult.OK)
            {
                string ButtonName = ((TextBox)sender).Name;

                string newValue = int.Parse(insertNum.InputValue).ToString();
                switch (ButtonName)
                {
                    case "txt_WiseModule_MaxNum":
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;
                            AppConfiguration.ConfigDataSetting("WiseModule_MaxNum", newValue);
                            ConfigData.WiseModule_MaxNum = int.Parse(AppConfiguration.GetAppConfig("WiseModule_MaxNum"));
                            insertNum.Close();
                            mainForm.UserLog("WiseModuleConfig Screen", $"WiseModule_MaxNum Change from {oldValue} to {newValue}");
                            uow.WiseModuleConfig.Validate_DB_Items();
                            WiseModule_Display();
                        }
                        break;
                    case "txt_WiseModuleResponseTime":
                        if (((TextBox)sender).Text != newValue)
                        {
                            string oldValue = ((TextBox)sender).Text;
                            ((TextBox)sender).Text = newValue;
                            AppConfiguration.ConfigDataSetting("WiseModuleResponseTime", ((TextBox)sender).Text);
                            ConfigData.WiseModuleResponseTime = int.Parse(AppConfiguration.GetAppConfig("WiseModuleResponseTime"));
                            insertNum.Close();
                            mainForm.UserLog("WiseModuleConfig Screen", $"WiseModuleResponseTime Change from {oldValue} to {newValue}");
                            WiseModule_Display();
                        }
                        break;
                }
            }

        }

        private void btn_ScreenButton_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            string ControlValue = "";

            switch (buttonName)
            {
                case "btn_AutoControl":
                    ControlValue = "AutoControl";
                    btn_AutoControl.BackColor = Color.YellowGreen;
                    btn_ManualControl.BackColor = Color.WhiteSmoke;
                    break;

                case "btn_ManualControl":
                    ControlValue = "ManualControl";
                    btn_ManualControl.BackColor = Color.YellowGreen;
                    btn_AutoControl.BackColor = Color.WhiteSmoke;
                    break;

                case "btn_WiseModuleConfigBackUp":
                    if (dtg_WiseModuleConfig.Rows.Count == 0) return;
                    mainForm.SaveAsDataGridviewToCSV(dtg_WiseModuleConfig);
                    break;

            }
            if (ControlValue.Length > 0)
            {
                var ControlMode = uow.WiseModuleConfig.Find(m => m.ModuleControlMode == ControlValue).FirstOrDefault();
                if (ControlMode == null)
                {
                    uow.WiseModuleConfig.ControlModeUpdate(ControlValue);
                    uow.WiseModuleConfig.Validate_DB_Items();
                    WiseModule_Display();

                }
                if (ControlValue == "ManualControl")    //버튼을 누른후 show 꺼야지만 DataBase 업데이트 하는 문제발생하여 조건을 아래 조건으로 추가
                {
                    var wiseModuleManualScreen = new WiseModuleManualScreen(mainForm, uow);
                    wiseModuleManualScreen.Show(this);
                }
                mainForm.UserLog("WiseModuleConfig Screen", $"WiseModuleConfig Control Change Click {ControlValue}");

            }
        }

        private void WiseModuleMonitoringInit()
        {
            dtg_WiseModuleMonitoring.ScrollBars = ScrollBars.Both;
            dtg_WiseModuleMonitoring.AllowUserToResizeColumns = true;
            dtg_WiseModuleMonitoring.ColumnHeadersHeight = 40;
            dtg_WiseModuleMonitoring.RowTemplate.Height = 40;
            dtg_WiseModuleMonitoring.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WiseModuleMonitoring.Columns[0].CellTemplate;
            dtg_WiseModuleMonitoring.Columns.Clear();
            dtg_WiseModuleMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleMonitoring_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WiseModuleMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleMonitoring_DisplayName", HeaderText = "DisplayName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WiseModuleMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleMonitoring_ModuleStatus", HeaderText = "Status", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseModuleMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleMonitoring_ModuleInValue", HeaderText = "ModuleInValue", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseModuleMonitoring.Columns.Add(new DataGridViewColumn() { Name = "DGV_WiseModuleMonitoring_ModuleOutValue", HeaderText = "ModuleOutValue", Width = 100, CellTemplate = currentCellTemplate });
            dtg_WiseModuleMonitoring.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WiseModuleMonitoring.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WiseModuleMonitoring.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_ModuleInValue"].HeaderText = "Module" + "\r\n" + "InValue";
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_ModuleOutValue"].HeaderText = "Module" + "\r\n" + "OutValue";
            dtg_WiseModuleMonitoring.Rows.Clear();
        }

        private void WiseModuleMonitoringDisplay()
        {
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_Id"].ReadOnly = true;
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_DisplayName"].ReadOnly = true;
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_ModuleStatus"].ReadOnly = true;
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_ModuleInValue"].ReadOnly = true;
            dtg_WiseModuleMonitoring.Columns["DGV_WiseModuleMonitoring_ModuleOutValue"].ReadOnly = true;

            int firstRowIndex = dtg_WiseModuleMonitoring.FirstDisplayedScrollingColumnIndex;
            dtg_WiseModuleMonitoring.Rows.Clear();
            dtg_WiseModuleMonitoring.RowTemplate.Height = 40;

            foreach (var wiseModuleConfig in uow.WiseModuleConfig.GetAll())
            {
                int newRowIndex = dtg_WiseModuleMonitoring.Rows.Add();
                var newRow = dtg_WiseModuleMonitoring.Rows[newRowIndex];
                newRow.Cells["DGV_WiseModuleMonitoring_Id"].Value = wiseModuleConfig.Id.ToString();
                newRow.Cells["DGV_WiseModuleMonitoring_DisplayName"].Value = wiseModuleConfig.DisplayName;
                newRow.Cells["DGV_WiseModuleMonitoring_ModuleStatus"].Value = wiseModuleConfig.ModuleStatus;
                newRow.Cells["DGV_WiseModuleMonitoring_ModuleInValue"].Value = wiseModuleConfig.ModuleIn_Value;
                newRow.Cells["DGV_WiseModuleMonitoring_ModuleOutValue"].Value = wiseModuleConfig.ModuleOut_Value;
                newRow.Tag = wiseModuleConfig;

                if (wiseModuleConfig.ModuleStatus == "Connect") dtg_WiseModuleMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;
                else dtg_WiseModuleMonitoring.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;


                if (firstRowIndex >= 0 && firstRowIndex < dtg_WiseModuleMonitoring.Rows.Count)
                {
                    dtg_WiseModuleMonitoring.FirstDisplayedScrollingRowIndex = firstRowIndex;
                }
                dtg_WiseModuleMonitoring.ClearSelection();
            }
        }

        private void DisPlayTimer_Tick(object sender, EventArgs e)
        {
            DisPlayTimer.Enabled = false;
            WiseModuleMonitoringDisplay();
            DisPlayTimer.Interval = 1000; //타이머 인터벌 1초
            DisPlayTimer.Enabled = true;
        }
    }
}
