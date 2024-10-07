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
    public partial class PLCConfigScreen : Form
    {
        MainForm mainForm;
        IUnitOfWork uow;

        public PLCConfigScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_PLCModule.AlternatingRowsDefaultCellStyle = null;
            dtg_PLCModule.DoubleBuffered(true);
            PLCModuleConfigInit();
            PLCModuleConfigTextBoxInit();
            PLCModuleConfig_Display();
        }
        private void PLCModuleConfigTextBoxInit()
        {
            txt_PLCModuleMaxNum.ReadOnly = true;
            txt_PLCModuleMaxNum.BackColor = Color.White;
            txt_PLCModuleMaxNum.Text = ConfigData.PlcModule_MaxNum.ToString();

        }
        private void PLCModuleConfigInit()
        {
            btn_Display.BackColor = Color.WhiteSmoke;

            dtg_PLCModule.ScrollBars = ScrollBars.Both;
            dtg_PLCModule.AllowUserToResizeColumns = true;
            dtg_PLCModule.ColumnHeadersHeight = 50;
            dtg_PLCModule.RowTemplate.Height = 50;
            dtg_PLCModule.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_PLCModule.Columns[0].CellTemplate;
            dtg_PLCModule.Columns.Clear();
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_Mode", HeaderText = "ControlMode", Width = 130, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_Use", HeaderText = "Use/UnUse", Width = 80, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_IpAddress", HeaderText = "IpAddress", Width = 110, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_PortNumber", HeaderText = "PortNumber", Width = 110, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_Name", HeaderText = "ModuleName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_MapType", HeaderText = "MapType", Width = 200, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_ReadFirstMapAddress", HeaderText = "ReadFirstMapAddress", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_ReadSecondMapAddress", HeaderText = "ReadSecondMapAddress", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_WriteFirstMapAddress", HeaderText = "WriteFirstMapAddress", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCModule.Columns.Add(new DataGridViewColumn() { Name = "DGV_PLCModule_WriteSecondMapAddress", HeaderText = "WriteSecondMapAddress", Width = 150, CellTemplate = currentCellTemplate });
            dtg_PLCModule.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_PLCModule.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_PLCModule.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtg_PLCModule.Columns["DGV_PLCModule_Use"].HeaderText = "Use" + "\r\n" + "Unuse";
            dtg_PLCModule.Columns["DGV_PLCModule_ReadFirstMapAddress"].HeaderText = "ReadFirst" + "\r\n" + "MapAddress";
            dtg_PLCModule.Columns["DGV_PLCModule_ReadSecondMapAddress"].HeaderText = "ReadSecond" + "\r\n" + "MapAddress";
            dtg_PLCModule.Columns["DGV_PLCModule_WriteFirstMapAddress"].HeaderText = "WriteFirst" + "\r\n" + "MapAddress";
            dtg_PLCModule.Columns["DGV_PLCModule_WriteSecondMapAddress"].HeaderText = "WriteSecond" + "\r\n" + "MapAddress";

            dtg_PLCModule.Rows.Clear();
        }

        private void PLCModuleConfig_Display()
        {
            var ControlValue = uow.PlcConfigs.Find(m => m.ControlMode == "AutoControl").FirstOrDefault();
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

            dtg_PLCModule.Columns["DGV_PLCModule_Use"].ReadOnly = true;
            dtg_PLCModule.Columns["DGV_PLCModule_MapType"].ReadOnly = true;
            dtg_PLCModule.Columns["DGV_PLCModule_Mode"].ReadOnly = true;

            int firstRowIndex = dtg_PLCModule.FirstDisplayedScrollingColumnIndex;
            dtg_PLCModule.Rows.Clear();
            dtg_PLCModule.RowTemplate.Height = 50;
            foreach (var plcConfigs in uow.PlcConfigs.GetAll())
            {
                int newRowIndex = dtg_PLCModule.Rows.Add();
                var newRow = dtg_PLCModule.Rows[newRowIndex];
                newRow.Cells["DGV_PLCModule_Id"].Value = plcConfigs.Id.ToString();
                newRow.Cells["DGV_PLCModule_Mode"].Value = plcConfigs.ControlMode;
                newRow.Cells["DGV_PLCModule_Use"].Value = plcConfigs.PlcModuleUse;
                newRow.Cells["DGV_PLCModule_IpAddress"].Value = plcConfigs.PlcIpAddress;
                newRow.Cells["DGV_PLCModule_PortNumber"].Value = plcConfigs.PortNumber;
                newRow.Cells["DGV_PLCModule_Name"].Value = plcConfigs.PlcModuleName;
                newRow.Cells["DGV_PLCModule_MapType"].Value = plcConfigs.PlcMapType;
                newRow.Cells["DGV_PLCModule_ReadFirstMapAddress"].Value = plcConfigs.ReadFirstMapAddress;
                newRow.Cells["DGV_PLCModule_ReadSecondMapAddress"].Value = plcConfigs.ReadSecondMapAddress;
                newRow.Cells["DGV_PLCModule_WriteFirstMapAddress"].Value = plcConfigs.WriteFirstMapAddress;
                newRow.Cells["DGV_PLCModule_WriteSecondMapAddress"].Value = plcConfigs.WriteSecondMapAddress;
                newRow.Tag = plcConfigs;

                if (plcConfigs.PlcModuleUse == "Unuse" || plcConfigs.ControlMode == "ManualControl") dtg_PLCModule.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                else dtg_PLCModule.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            if (firstRowIndex >= 0 && firstRowIndex < dtg_PLCModule.Rows.Count)
            {
                dtg_PLCModule.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_PLCModule.ClearSelection();
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
                    btn_Display.BackColor = Color.WhiteSmoke;
                    break;

                case "btn_ManualControl":
                    ControlValue = "ManualControl";
                    btn_ManualControl.BackColor = Color.YellowGreen;
                    btn_AutoControl.BackColor = Color.WhiteSmoke;
                    btn_Display.BackColor = Color.WhiteSmoke;
                    break;

                case "btn_Display":
                    var pLCMonitoringScreen = new PLCMonitoringScreen(uow);
                    pLCMonitoringScreen.Show(this);
                    break;

                case "btn_PLCModuleBackUp":
                    if (dtg_PLCModule.Rows.Count == 0) return;
                    mainForm.SaveAsDataGridviewToCSV(dtg_PLCModule);
                    break;

            }
            if (ControlValue.Length > 0)
            {
                var ControlMode = uow.PlcConfigs.Find(m => m.ControlMode != ControlValue).FirstOrDefault();
                if (ControlMode != null)
                {
                    ControlMode.ControlMode = ControlValue;
                    uow.PlcConfigs.ControlModeUpdate(ControlValue);
                    PLCModuleConfig_Display();

                }
                if (ControlValue == "ManualControl")    //버튼을 누른후 show 꺼야지만 DataBase 업데이트 하는 문제발생하여 조건을 아래 조건으로 추가
                {
                    ControlMode.ControlMode = ControlValue;
                    uow.PlcConfigs.ControlModeUpdate(ControlValue);
                    PLCModuleConfig_Display();

                    var pLCManualScreen = new PLCManualScreen(mainForm, uow);
                    pLCManualScreen.Show(this);
                }
                mainForm.UserLog("PLCConfig Screen", $"PLCConfig Control Change Click {ControlValue}");

            }
        }

        private void txt_txt_PLCModuleMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT21");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string TextBoxName = ((TextBox)sender).Name;
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;
                    ((TextBox)sender).Text = newValue;

                    AppConfiguration.ConfigDataSetting("PlcModule_MaxNum", newValue);
                    ConfigData.PlcModule_MaxNum = int.Parse(AppConfiguration.GetAppConfig("PlcModule_MaxNum"));
                    insertNum.Close();
                    uow.PlcConfigs.Validate_DB_Items();
                    PLCModuleConfig_Display();
                    mainForm.UserLog("PLCConfig Screen", $"PLCModuleMaxNum Change from {oldValue} to {newValue}");
                }
            }
        }

        private void dtg_PLCModule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

                DataGridView grid = (DataGridView)sender;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

                PlcConfig selectedRowTag = (PlcConfig)selectedRow.Tag;
                PlcConfig targetConfig = uow.PlcConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

                string ChangeDatamsg = null;

                //Convert.ToString 사용시 변수가 null인경우 빈문자열을 반환함 
                //.Tostring() 사용시 변수가 null인경우 에러발생
                if (!string.IsNullOrEmpty(Convert.ToString(selectedCell.Value)) && targetConfig != null)
                {
                    string newValue = String.Concat(selectedCell.Value.ToString().Where(c => !char.IsWhiteSpace(c)));    //문자열 빈칸없애기

                    if (e.ColumnIndex == grid.Columns["DGV_PLCModule_IpAddress"].Index)
                    {

                        bool flag = System.Net.IPAddress.TryParse(newValue, out var newIp); // check ip address
                        string newIpAddress = flag ? newIp.ToString() : targetConfig.PlcIpAddress;

                        var PlcConfigsnewIpAddress = uow.PlcConfigs.Find(m => m.PlcIpAddress == newIpAddress).FirstOrDefault();
                        if (PlcConfigsnewIpAddress == null || (newValue == "0.0.0.0"))
                        {
                            if (targetConfig.PlcIpAddress != newValue)
                            {
                                string oldIpAddress = targetConfig.PlcIpAddress;
                                targetConfig.PlcIpAddress = newIpAddress;
                                ChangeDatamsg = $"PLCConfigs, PLC Config{targetConfig.Id} PLC IpAddress Change from {oldIpAddress} to {newIpAddress}";
                                ChageData(newValue, ChangeDatamsg);
                            }
                        }
                        else MessageBox.Show("같은 IpAddress가 있습니다." + "\r\n" + "다시확인후 입력해주시기 바랍니다.");
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_PortNumber"].Index)
                    {
                        if (targetConfig.PortNumber != Convert.ToInt32(newValue))
                        {
                            string oldValue = targetConfig.PortNumber.ToString();
                            targetConfig.PortNumber = Convert.ToInt32(newValue);
                            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC PortNumber changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_Name"].Index)
                    {
                        if (targetConfig.PlcModuleName != newValue)
                        {
                            string oldValue = targetConfig.PlcModuleName;
                            targetConfig.PlcModuleName = newValue;
                            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC ModuleName changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_ReadFirstMapAddress"].Index)
                    {
                        if (targetConfig.ReadFirstMapAddress != newValue)
                        {
                            string oldValue = targetConfig.ReadFirstMapAddress;
                            targetConfig.ReadFirstMapAddress = newValue;
                            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC ReadFirstMapAddress changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_ReadSecondMapAddress"].Index)
                    {
                        if (targetConfig.ReadSecondMapAddress != newValue)
                        {
                            string oldValue = targetConfig.ReadSecondMapAddress;
                            targetConfig.ReadSecondMapAddress = newValue;
                            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC ReadSecondMapAddress changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_WriteFirstMapAddress"].Index)
                    {
                        if (targetConfig.WriteFirstMapAddress != newValue)
                        {
                            string oldValue = targetConfig.WriteFirstMapAddress;
                            targetConfig.WriteFirstMapAddress = newValue;
                            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC WriteFirstMapAddress changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                    else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_WriteSecondMapAddress"].Index)
                    {
                        if (targetConfig.WriteSecondMapAddress != newValue)
                        {
                            string oldValue = targetConfig.WriteSecondMapAddress;
                            targetConfig.WriteSecondMapAddress = newValue;
                            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC WriteSecondMapAddress changed from {oldValue} to {newValue}.";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                    PLCModuleConfig_Display();
                }

                void ChageData(string newValue, string changeDatamsg)
                {
                    if (changeDatamsg != null)
                    {
                        selectedCell.Value = newValue;
                        selectedRowTag = targetConfig;
                        uow.PlcConfigs.Update(targetConfig);
                        string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                        mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                        MessageBox.Show("저장되었습니다");
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

        private void dtg_PLCModule_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];


            PlcConfig selectedRowTag = (PlcConfig)selectedRow.Tag;
            PlcConfig targetConfig = uow.PlcConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_PLCModule_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();
                        if (targetConfig.PlcModuleUse != newValue)
                        {
                            string oldValue = targetConfig.PlcModuleUse;
                            targetConfig.PlcModuleUse = newValue;
                            targetConfig.Connect = false;
                            insertUse.Close();
                            ChangeDatamsg = $"PLCConfigs, PLC Config{targetConfig.Id} PLC Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                            if (targetConfig.PlcModuleUse == "Unuse" || targetConfig.ControlMode == "ManualControl") dtg_PLCModule.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                            else dtg_PLCModule.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        }
                    }
                }
                //else if (e.ColumnIndex == grid.Columns["DGV_PLCModule_Name"].Index)
                //{
                //    var tabletZoneSelectForm = new TabletZoneSelectForm(mainForm, uow);
                //    if (tabletZoneSelectForm.ShowDialog() == DialogResult.OK)
                //    {
                //        string newValue = tabletZoneSelectForm.InputValue.Trim();

                //        if (targetConfig.PlcModuleName != newValue)
                //        {
                //            string oldValue = targetConfig.PlcModuleName;
                //            targetConfig.PlcModuleName = newValue;
                //            tabletZoneSelectForm.Close();
                //            ChangeDatamsg = $"PLCConfigs,PLC Config{targetConfig.Id} PLC ModuleName changed from {oldValue} to {newValue}.";
                //            ChageData(newValue, ChangeDatamsg);
                //        }
                //    }
                //}
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                PLCModuleConfig_Display();
            }

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.PlcConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    mainForm.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }
        }
    }
}
