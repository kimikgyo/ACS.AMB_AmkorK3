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
    public partial class WaitMissionScreen : Form
    {
        MainForm main;
        IUnitOfWork uow;

        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);

        public WaitMissionScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_WaitMission.AlternatingRowsDefaultCellStyle = null;
            dtg_WaitMission.AlternatingRowsDefaultCellStyle = null;

            Init();
            TextBoxInIt();
            WaitMissionInit();
            WaitMission_Display();
        }

        private void Init()
        {
            this.BackColor = main.skinColor;

            groupBox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            btn_WaitMssionBackup.BackColor = main.skinColor;
            btn_WaitMssionBackup.ForeColor = Color.White;

            dtg_WaitMission.ScrollBars = ScrollBars.Both;
            dtg_WaitMission.AllowUserToResizeColumns = true;
            dtg_WaitMission.ColumnHeadersHeight = 70;
            dtg_WaitMission.RowTemplate.Height = 82;
            dtg_WaitMission.ReadOnly = false;
            dtg_WaitMission.BackgroundColor = main.skinColor;
            dtg_WaitMission.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_WaitMission.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_WaitMission.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_WaitMission.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_WaitMission.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_WaitMission.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_WaitMission.EnableHeadersVisualStyles = false;
            dtg_WaitMission.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void TextBoxInIt()
        {
            txt_WaitMissionMaxNum.ReadOnly = true;
            txt_WaitMissionMaxNum.BackColor = Color.White;
            txt_WaitMissionMaxNum.Text = ConfigData.WaitMission_MaxNum.ToString();

        }

        private void WaitMissionInit()
        {

            txt_WaitMissionMaxNum.Text = ConfigData.WaitMission_MaxNum.ToString();
            dtg_WaitMission.ScrollBars = ScrollBars.Both;
            dtg_WaitMission.AllowUserToResizeColumns = true;
            dtg_WaitMission.ColumnHeadersHeight = 50;
            dtg_WaitMission.RowTemplate.Height = 40;
            dtg_WaitMission.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_WaitMission.Columns[0].CellTemplate;
            dtg_WaitMission.Columns.Clear();
            dtg_WaitMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_WaitMission_Id", HeaderText = "Index", Width = 70, CellTemplate = currentCellTemplate });
            dtg_WaitMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_WaitMission_WaitMissionUse", HeaderText = "UseUnUse", Width = 150, CellTemplate = currentCellTemplate });
            dtg_WaitMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_WaitMission_EndPositionZone", HeaderText = "EndZoneName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WaitMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_WaitMission_WaitMissionName", HeaderText = "MissionName", Width = 250, CellTemplate = currentCellTemplate });
            dtg_WaitMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_WaitMission_EnableBattery", HeaderText = "EnableBattery", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WaitMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_WaitMission_RobotName", HeaderText = "RobotName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_WaitMission.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_WaitMission_ProductValue", HeaderText = "ProductValue", Width = 150 });
            dtg_WaitMission.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_WaitMission_ProductAcitve", HeaderText = "ProductAcitve", Width = 150 });
            dtg_WaitMission.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_WaitMission.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_WaitMission.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            //dtg_WaitMission.Columns["DGV_WaitMission_WaitMissionUse"].HeaderText = "Use" + "\r\n" + "Unuse";
            //dtg_WaitMission.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;                       //자동 높이 조절        
            //dtg_WaitMission.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;                             //자동 높이 조절        
            dtg_WaitMission.Rows.Clear();
        }

        private void WaitMission_Display()
        {
            dtg_WaitMission.Columns["DGV_WaitMission_Id"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_WaitMissionUse"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_EndPositionZone"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_WaitMissionName"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_EnableBattery"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_RobotName"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_ProductValue"].ReadOnly = true;
            dtg_WaitMission.Columns["DGV_WaitMission_ProductAcitve"].ReadOnly = true;

            int firstRowIndex = dtg_WaitMission.FirstDisplayedScrollingColumnIndex;
            dtg_WaitMission.Rows.Clear();
            dtg_WaitMission.RowTemplate.Height = 40;
            foreach (var waitMissionConfig in uow.WaitMissionConfigs.GetAll())
            {
                int newRowIndex = dtg_WaitMission.Rows.Add();
                var newRow = dtg_WaitMission.Rows[newRowIndex];
                newRow.Cells["DGV_WaitMission_Id"].Value = waitMissionConfig.Id.ToString();
                newRow.Cells["DGV_WaitMission_WaitMissionUse"].Value = waitMissionConfig.WaitMissionUse;
                newRow.Cells["DGV_WaitMission_EndPositionZone"].Value = waitMissionConfig.PositionZone;
                newRow.Cells["DGV_WaitMission_WaitMissionName"].Value = waitMissionConfig.WaitMissionName;
                newRow.Cells["DGV_WaitMission_EnableBattery"].Value = waitMissionConfig.EnableBattery + "%";
                newRow.Cells["DGV_WaitMission_RobotName"].Value = waitMissionConfig.RobotName;
                newRow.Cells["DGV_WaitMission_ProductValue"].Value = waitMissionConfig.ProductValue;
                newRow.Cells["DGV_WaitMission_ProductAcitve"].Value = waitMissionConfig.ProductActive;
                newRow.Tag = waitMissionConfig;

                if (waitMissionConfig.WaitMissionUse == "Unuse")
                {
                    dtg_WaitMission.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    dtg_WaitMission.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dtg_WaitMission.Rows[newRowIndex].DefaultCellStyle.BackColor = main.skinColor;
                    dtg_WaitMission.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.White;
                }

                if (waitMissionConfig.ProductActive == true) dtg_WaitMission.Rows[newRowIndex].Cells[newRow.Cells["DGV_WaitMission_ProductValue"].ColumnIndex].Style.BackColor = Color.White;
                else dtg_WaitMission.Rows[newRowIndex].Cells[newRow.Cells["DGV_WaitMission_ProductValue"].ColumnIndex].Style.BackColor = Color.Black;
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_WaitMission.Rows.Count)
            {
                dtg_WaitMission.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_WaitMission.ClearSelection();
        }

        private void dtg_WaitMission_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            WaitMissionConfigModel selectedRowTag = (WaitMissionConfigModel)selectedRow.Tag;
            WaitMissionConfigModel targetConfig = uow.WaitMissionConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_WaitMission_WaitMissionUse"].Index)
                {
                    var insert = new UseSelectForm();
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.WaitMissionUse != newValue)
                        {
                            string oldValue = targetConfig.WaitMissionUse;
                            targetConfig.WaitMissionUse = newValue;
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} WaitMissionUse Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                            if (targetConfig.WaitMissionUse == "Use")
                            {
                                dtg_WaitMission.Rows[e.RowIndex].DefaultCellStyle.BackColor = main.skinColor;
                                dtg_WaitMission.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                            else
                            {
                                dtg_WaitMission.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                dtg_WaitMission.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }

                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WaitMission_EndPositionZone"].Index)
                {
                    var insert = new PositionZoneSelectForm(null,uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.PositionZone != newValue)
                        {
                            string oldValue = targetConfig.PositionZone;
                            targetConfig.PositionZone = newValue;
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} EndPositionZone Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WaitMission_WaitMissionName"].Index)
                {
                    var insert = new WaitMissionSelectForm(main);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.WaitMissionName != newValue)
                        {
                            string oldValue = targetConfig.WaitMissionName;
                            targetConfig.WaitMissionName = newValue;
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} WaitMissionName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WaitMission_EnableBattery"].Index)
                {
                    NumPadForm insert = new NumPadForm(dtg_WaitMission.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_WaitMission", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (Convert.ToString(targetConfig.EnableBattery) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.EnableBattery);
                            targetConfig.EnableBattery = double.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} EnableBattery Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}%", ChangeDatamsg);

                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WaitMission_RobotName"].Index)
                {
                    var insert = new RobotNameSelectForm(main, uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.RobotName != newValue)
                        {
                            string oldValue = targetConfig.RobotName;
                            targetConfig.RobotName = newValue;
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} RobotName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WaitMission_ProductValue"].Index)
                {
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_WaitMission_ProductValue"].Value);

                    if (targetConfig.ProductValue != newValue)
                    {
                        bool oldValue = targetConfig.ProductValue;
                        targetConfig.ProductValue = newValue;
                        ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} ProductValue Change from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);

                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_WaitMission_ProductAcitve"].Index)
                {
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_WaitMission_ProductAcitve"].Value);
                    if (targetConfig.ProductActive != newValue)
                    {
                        bool oldValue = targetConfig.ProductActive;
                        targetConfig.ProductActive = newValue;

                        ChangeDatamsg = $"Wait_ChangeMission,WaitMissionConfig {targetConfig.Id} ProductActive Change from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);


                        if (targetConfig.ProductActive == true) dtg_WaitMission.Rows[e.RowIndex].Cells["DGV_WaitMission_ProductValue"].Style.BackColor = Color.White;
                        else dtg_WaitMission.Rows[e.RowIndex].Cells["DGV_WaitMission_ProductValue"].Style.BackColor = Color.Black;
                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                WaitMission_Display();
            }

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.WaitMissionConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }

        }

        private void txt_WaitMissionMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("WaitMission_MaxNum", newValue);
                    ConfigData.WaitMission_MaxNum = int.Parse(AppConfiguration.GetAppConfig("WaitMission_MaxNum"));
                    insertNum.Close();
                    uow.WaitMissionConfigs.Validate_DB_Items();
                    WaitMission_Display();
                    main.UserLog("Position", $"WaitMission_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            DataGridView BackUpData = null;
            string ButtonName = ((Button)sender).Name;

            switch (ButtonName)
            {
                case "btn_WaitMssionBackup":
                    if (dtg_WaitMission.Rows.Count == 0) return;
                    BackUpData = dtg_WaitMission;
                    break;
            }
            if (BackUpData != null)
            {
                main.SaveAsDataGridviewToCSV(BackUpData);
                main.UserLog("RobotGroup_Waitting Screen", $"{ButtonName} BackUp Click ");
            }
        }


    }
}
