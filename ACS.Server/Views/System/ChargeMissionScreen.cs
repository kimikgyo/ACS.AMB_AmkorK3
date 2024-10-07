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
    public partial class ChargeMissionScreen : Form
    {
        MainForm main;
        IUnitOfWork uow;

        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);

        public ChargeMissionScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_ChargeMission.AlternatingRowsDefaultCellStyle = null;
            dtg_ChargeMission.DoubleBuffered(true);

            Init();
            TextBoxInIt();
            ChargeMissionInlt();
            ChargeMission_Display();
        }

        private void Init()
        {
            this.BackColor = main.skinColor;

            groupBox2.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            btn_ChargeMssionBackup.BackColor = main.skinColor;
            btn_ChargeMssionBackup.ForeColor = Color.White;

            dtg_ChargeMission.ScrollBars = ScrollBars.Both;
            dtg_ChargeMission.AllowUserToResizeColumns = true;
            dtg_ChargeMission.ColumnHeadersHeight = 70;
            dtg_ChargeMission.RowTemplate.Height = 82;
            dtg_ChargeMission.ReadOnly = false;
            dtg_ChargeMission.BackgroundColor = main.skinColor;
            dtg_ChargeMission.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_ChargeMission.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_ChargeMission.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_ChargeMission.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_ChargeMission.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_ChargeMission.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_ChargeMission.EnableHeadersVisualStyles = false;
            dtg_ChargeMission.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void TextBoxInIt()
        {
            txt_ChargeMissionMaxNum.ReadOnly = true;
            txt_ChargeMissionMaxNum.BackColor = Color.White;
            txt_ChargeMissionMaxNum.Text = ConfigData.ChargeMission_MaxNum.ToString();

        }

        private void ChargeMissionInlt()
        {
            txt_ChargeMissionMaxNum.Text = ConfigData.ChargeMission_MaxNum.ToString();
            dtg_ChargeMission.ScrollBars = ScrollBars.Both;
            dtg_ChargeMission.AllowUserToResizeColumns = true;
            dtg_ChargeMission.ColumnHeadersHeight = 50;
            dtg_ChargeMission.RowTemplate.Height = 40;
            dtg_ChargeMission.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_ChargeMission.Columns[0].CellTemplate;
            dtg_ChargeMission.Columns.Clear();
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_Id", HeaderText = "Index", Width = 50, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_ChargerGroupName", HeaderText = "ChargerGroupName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_ChargeMissionUse", HeaderText = "Use/UnUse", Width = 150, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_StartPositionZone", HeaderText = "StartZoneName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_ChargeMissionName", HeaderText = "MissionName", Width = 200, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_StartBattery", HeaderText = "StartBattery", Width = 110, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_SwitchaingBattery", HeaderText = "SwitchaingBattery", Width = 120, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_EndBattery", HeaderText = "EndBattery", Width = 110, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewColumn() { Name = "DGV_ChargeMission_RobotName", HeaderText = "RobotName", Width = 170, CellTemplate = currentCellTemplate });
            dtg_ChargeMission.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_ChargeMission_ProductValue", HeaderText = "ProductValue", Width = 125 });
            dtg_ChargeMission.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "DGV_ChargeMission_ProductAcitve", HeaderText = "ProductAcitve", Width = 125 });
            dtg_ChargeMission.Columns["DGV_ChargeMission_StartBattery"].HeaderText = "Start" + "\r\n" + "Battery";
            dtg_ChargeMission.Columns["DGV_ChargeMission_SwitchaingBattery"].HeaderText = "Switchaing" + "\r\n" + "Battery";
            dtg_ChargeMission.Columns["DGV_ChargeMission_EndBattery"].HeaderText = "End" + "\r\n" + "Battery";

            dtg_ChargeMission.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_ChargeMission.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_ChargeMission.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬

            dtg_ChargeMission.Rows.Clear();
        }

        private void ChargeMission_Display()
        {
            dtg_ChargeMission.Columns["DGV_ChargeMission_Id"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_ChargerGroupName"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_ChargeMissionUse"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_StartPositionZone"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_ChargeMissionName"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_StartBattery"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_SwitchaingBattery"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_EndBattery"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_RobotName"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_ProductValue"].ReadOnly = true;
            dtg_ChargeMission.Columns["DGV_ChargeMission_ProductAcitve"].ReadOnly = true;

            int firstRowIndex = dtg_ChargeMission.FirstDisplayedScrollingColumnIndex;
            dtg_ChargeMission.Rows.Clear();
            dtg_ChargeMission.RowTemplate.Height = 40;
            foreach (var chargeMissionConfig in uow.ChargeMissionConfigs.GetAll())
            {
                int newRowIndex = dtg_ChargeMission.Rows.Add();
                var newRow = dtg_ChargeMission.Rows[newRowIndex];
                newRow.Cells["DGV_ChargeMission_Id"].Value = chargeMissionConfig.Id.ToString();
                newRow.Cells["DGV_ChargeMission_ChargerGroupName"].Value = chargeMissionConfig.ChargerGroupName;
                newRow.Cells["DGV_ChargeMission_ChargeMissionUse"].Value = chargeMissionConfig.ChargeMissionUse;
                newRow.Cells["DGV_ChargeMission_StartPositionZone"].Value = chargeMissionConfig.PositionZone;
                newRow.Cells["DGV_ChargeMission_ChargeMissionName"].Value = chargeMissionConfig.ChargeMissionName;
                newRow.Cells["DGV_ChargeMission_StartBattery"].Value = chargeMissionConfig.StartBattery + "%";
                newRow.Cells["DGV_ChargeMission_SwitchaingBattery"].Value = chargeMissionConfig.SwitchaingBattery + "%";
                newRow.Cells["DGV_ChargeMission_EndBattery"].Value = chargeMissionConfig.EndBattery + "%";
                newRow.Cells["DGV_ChargeMission_RobotName"].Value = chargeMissionConfig.RobotName;
                newRow.Cells["DGV_ChargeMission_ProductValue"].Value = chargeMissionConfig.ProductValue;
                newRow.Cells["DGV_ChargeMission_ProductAcitve"].Value = chargeMissionConfig.ProductActive;
                newRow.Tag = chargeMissionConfig;

                if (chargeMissionConfig.ChargeMissionUse == "Unuse")
                {
                    dtg_ChargeMission.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    dtg_ChargeMission.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dtg_ChargeMission.Rows[newRowIndex].DefaultCellStyle.BackColor = main.skinColor;
                    dtg_ChargeMission.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.White;
                    if (chargeMissionConfig.ProductActive == true) dtg_ChargeMission.Rows[newRowIndex].Cells[newRow.Cells["DGV_ChargeMission_ProductValue"].ColumnIndex].Style.BackColor = Color.White;
                    else dtg_ChargeMission.Rows[newRowIndex].Cells[newRow.Cells["DGV_ChargeMission_ProductValue"].ColumnIndex].Style.BackColor = Color.Black;
                }
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_ChargeMission.Rows.Count)
            {
                dtg_ChargeMission.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_ChargeMission.ClearSelection();
        }

        private void dtg_ChargeMission_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            ChargeMissionConfigModel selectedRowTag = (ChargeMissionConfigModel)selectedRow.Tag;
            ChargeMissionConfigModel targetConfig = uow.ChargeMissionConfigs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {
                if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_ChargerGroupName"].Index)
                {
                    var insert = new GroupSelectForm(main, uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.ChargerGroupName != newValue)
                        {
                            string oldValue = targetConfig.ChargerGroupName;
                            targetConfig.ChargerGroupName = newValue;
                            insert.Close();
                            ChangeDatamsg = $"hangeMission,ChargeMissionConfig {targetConfig.Id} ChargerGroupName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                        }

                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_ChargeMissionUse"].Index)
                {
                    var insert = new UseSelectForm();
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.ChargeMissionUse != newValue)
                        {
                            string oldValue = targetConfig.ChargeMissionUse;
                            targetConfig.ChargeMissionUse = newValue;
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} ChargeMissionUse Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);

                            if (targetConfig.ChargeMissionUse == "Use")
                            {
                                dtg_ChargeMission.Rows[e.RowIndex].DefaultCellStyle.BackColor = main.skinColor;
                                dtg_ChargeMission.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                            else
                            {
                                dtg_ChargeMission.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                dtg_ChargeMission.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }
                        }

                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_StartPositionZone"].Index)
                {
                    var insert = new PositionZoneSelectForm(null, uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.PositionZone != newValue)
                        {
                            string oldValue = targetConfig.PositionZone;
                            targetConfig.PositionZone = newValue;

                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} PositionZone Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_ChargeMissionName"].Index)
                {
                    var insert = new ChargeMissionSelectForm(main);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.ChargeMissionName != newValue)
                        {
                            string oldValue = targetConfig.ChargeMissionName;

                            targetConfig.ChargeMissionName = newValue;
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} ChargeMissionName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_StartBattery"].Index)
                {
                    var insert = new NumPadForm(dtg_ChargeMission.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ChargeMission", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();

                        if (Convert.ToString(targetConfig.StartBattery) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.StartBattery);

                            targetConfig.StartBattery = double.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} StartBattery Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}%", ChangeDatamsg);

                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_SwitchaingBattery"].Index)
                {
                    var insert = new NumPadForm(dtg_ChargeMission.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ChargeMission", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();

                        if (Convert.ToString(targetConfig.SwitchaingBattery) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.SwitchaingBattery);

                            targetConfig.SwitchaingBattery = double.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} SwitchaingBattery Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}%", ChangeDatamsg);

                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_EndBattery"].Index)
                {

                    var insert = new NumPadForm(dtg_ChargeMission.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_ChargeMission", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (Convert.ToString(targetConfig.EndBattery) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.EndBattery);
                            targetConfig.EndBattery = double.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} EndBattery Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}%", ChangeDatamsg);

                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_RobotName"].Index)
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
                            ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} RobotName Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_ProductValue"].Index)
                {
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_ChargeMission_ProductValue"].Value);
                    if (targetConfig.ProductValue != newValue)
                    {
                        bool oldValue = !targetConfig.ProductValue;
                        targetConfig.ProductValue = newValue;
                        ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} ProductValue Change from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_ChargeMission_ProductAcitve"].Index)
                {
                    bool newValue = !Convert.ToBoolean(selectedRow.Cells["DGV_ChargeMission_ProductAcitve"].Value);

                    if (targetConfig.ProductActive != newValue)
                    {
                        bool oldValue = targetConfig.ProductActive;
                        targetConfig.ProductActive = newValue;

                        if (targetConfig.ProductActive == true) dtg_ChargeMission.Rows[e.RowIndex].Cells["DGV_ChargeMission_ProductValue"].Style.BackColor = Color.White;
                        else dtg_ChargeMission.Rows[e.RowIndex].Cells["DGV_ChargeMission_ProductValue"].Style.BackColor = Color.Black;

                        ChangeDatamsg = $"Wait_ChangeMission,ChargeMissionConfig {targetConfig.Id} ProductActive Change from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);


                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                ChargeMission_Display();
            }

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.ChargeMissionConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }

        }

        private void txt_ChargeMissionMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("ChargeMission_MaxNum", newValue);
                    ConfigData.ChargeMission_MaxNum = int.Parse(AppConfiguration.GetAppConfig("ChargeMission_MaxNum"));
                    insertNum.Close();
                    uow.ChargeMissionConfigs.Validate_DB_Items();
                    ChargeMission_Display();
                    main.UserLog("Position", $"ChargeMission_MaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {
            DataGridView BackUpData = null;
            string ButtonName = ((Button)sender).Name;

            switch (ButtonName)
            {
                case "btn_ChargeMssionBackup":
                    if (dtg_ChargeMission.Rows.Count == 0) return;
                    BackUpData = dtg_ChargeMission;
                    break;
            }
            if (BackUpData != null)
            {
                main.SaveAsDataGridviewToCSV(BackUpData);
                main.UserLog("RobotGroup_Charge Screen", $"{ButtonName} BackUp Click ");
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
