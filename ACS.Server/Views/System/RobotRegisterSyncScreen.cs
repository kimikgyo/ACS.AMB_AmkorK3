using INA_ACS_Server.UI;
using log4net;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Drawing;

namespace INA_ACS_Server
{

    public partial class RobotRegisterSyncScreen : Form
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log

        MainForm main;
        IUnitOfWork uow;

        private readonly Font textFont1 = new Font("맑은 고딕", 15, FontStyle.Bold);
        private readonly Font textFont2 = new Font("맑은 고딕", 15, FontStyle.Bold);
        private readonly Font textFont3 = new Font("Arial", 10, FontStyle.Bold);
        private readonly Font textFont4 = new Font("Arial", 9, FontStyle.Bold);

        public RobotRegisterSyncScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code
            dtg_RobotRegisterSync.AlternatingRowsDefaultCellStyle = null;
            dtg_RobotRegisterSync.DoubleBuffered(true);

            Init();
            RobotRegisterSyncTextBoxInit();
            RobotRegisterSyncInit();
            RobotRegisterSync_Display();
        }

        private void Init()
        {
            this.BackColor = main.skinColor;

            groupBox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            btn_RegisterSync.BackColor = main.skinColor;
            btn_RegisterSync.ForeColor = Color.White;

            dtg_RobotRegisterSync.ScrollBars = ScrollBars.Both;
            dtg_RobotRegisterSync.AllowUserToResizeColumns = true;
            dtg_RobotRegisterSync.ColumnHeadersHeight = 70;
            dtg_RobotRegisterSync.RowTemplate.Height = 82;
            dtg_RobotRegisterSync.ReadOnly = false;
            dtg_RobotRegisterSync.BackgroundColor = main.skinColor;
            dtg_RobotRegisterSync.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_RobotRegisterSync.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_RobotRegisterSync.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_RobotRegisterSync.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_RobotRegisterSync.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_RobotRegisterSync.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_RobotRegisterSync.EnableHeadersVisualStyles = false;
            dtg_RobotRegisterSync.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void RobotRegisterSyncTextBoxInit()
        {
            txt_RobotRegistarSyncMaxNum.ReadOnly = true;
            txt_RobotRegistarSyncMaxNum.BackColor = Color.White;
            txt_RobotRegistarSyncMaxNum.Text = ConfigData.RobotRegistarSync_MaxNum.ToString();
        }

        private void RobotRegisterSyncInit()
        {
            dtg_RobotRegisterSync.ScrollBars = ScrollBars.Both;
            dtg_RobotRegisterSync.AllowUserToResizeColumns = true;
            dtg_RobotRegisterSync.ColumnHeadersHeight = 40;
            dtg_RobotRegisterSync.RowTemplate.Height = 50;
            dtg_RobotRegisterSync.ReadOnly = false;           //편집 사용
            DataGridViewCell currentCellTemplate = dtg_RobotRegisterSync.Columns[0].CellTemplate;
            dtg_RobotRegisterSync.Columns.Clear();
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_Index", HeaderText = "Index", Width = 150, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_Use", HeaderText = "RegisterSyncUse", Width = 200, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_RegiaterSyncGroup", HeaderText = "RegiaterSyncPOSGroup", Width = 250, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_RegiaterSyncPOSName", HeaderText = "RegiaterSyncPOSName", Width = 250, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_ACSRobotGroup", HeaderText = "ACSRobotGoup", Width = 180, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_RegisterNo", HeaderText = "RegitserNo", Width = 150, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.Columns.Add(new DataGridViewColumn() { Name = "DGV_RobotRegisterSync_RegisterValue", HeaderText = "RegisterValue", Width = 150, CellTemplate = currentCellTemplate });
            dtg_RobotRegisterSync.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtg_RobotRegisterSync.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬
            dtg_RobotRegisterSync.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬

            dtg_RobotRegisterSync.Rows.Clear();


        }

        private void RobotRegisterSync_Display()
        {
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_Index"].ReadOnly = true;
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_Use"].ReadOnly = true;
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_RegiaterSyncGroup"].ReadOnly = true;
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_RegiaterSyncPOSName"].ReadOnly = true;
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_ACSRobotGroup"].ReadOnly = true;
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_RegisterNo"].ReadOnly = true;
            dtg_RobotRegisterSync.Columns["DGV_RobotRegisterSync_RegisterValue"].ReadOnly = true;

            int firstRowIndex = dtg_RobotRegisterSync.FirstDisplayedScrollingColumnIndex;
            dtg_RobotRegisterSync.Rows.Clear();
            dtg_RobotRegisterSync.RowTemplate.Height = 40;
            foreach (var robotRegisterSync in uow.RobotRegisterSyncs.GetAll())
            {
                int newRowIndex = dtg_RobotRegisterSync.Rows.Add();
                var newRow = dtg_RobotRegisterSync.Rows[newRowIndex];
                newRow.Cells["DGV_RobotRegisterSync_Index"].Value = $"RegisterSync{robotRegisterSync.Id}";
                newRow.Cells["DGV_RobotRegisterSync_Use"].Value = robotRegisterSync.RegisterSyncUse;
                newRow.Cells["DGV_RobotRegisterSync_RegiaterSyncGroup"].Value = robotRegisterSync.PositionGroup;
                newRow.Cells["DGV_RobotRegisterSync_RegiaterSyncPOSName"].Value = robotRegisterSync.PositionName;
                newRow.Cells["DGV_RobotRegisterSync_ACSRobotGroup"].Value = robotRegisterSync.ACSRobotGroup;
                newRow.Cells["DGV_RobotRegisterSync_RegisterNo"].Value = robotRegisterSync.RegisterNo;
                newRow.Cells["DGV_RobotRegisterSync_RegisterValue"].Value = robotRegisterSync.RegisterValue;
                newRow.Tag = robotRegisterSync;


                if (robotRegisterSync.RegisterSyncUse == "Unuse")
                {
                    dtg_RobotRegisterSync.Rows[newRowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                    dtg_RobotRegisterSync.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dtg_RobotRegisterSync.Rows[newRowIndex].DefaultCellStyle.BackColor = main.skinColor;
                    dtg_RobotRegisterSync.Rows[newRowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            if (firstRowIndex >= 0 && firstRowIndex < dtg_RobotRegisterSync.Rows.Count)
            {
                dtg_RobotRegisterSync.FirstDisplayedScrollingRowIndex = firstRowIndex;
            }
            dtg_RobotRegisterSync.ClearSelection();
        }

        private void dtg_RobotRegisterSync_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return; // 헤더 클릭한 경우는 제외

            DataGridView grid = (DataGridView)sender;
            DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
            DataGridViewCell selectedCell = grid[e.ColumnIndex, e.RowIndex];

            RobotRegisterSyncModel selectedRowTag = (RobotRegisterSyncModel)selectedRow.Tag;
            RobotRegisterSyncModel targetConfig = uow.RobotRegisterSyncs.Find(m => m.Id == selectedRowTag.Id).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {

                if (e.ColumnIndex == grid.Columns["DGV_RobotRegisterSync_Use"].Index)
                {
                    var insertUse = new UseSelectForm();
                    if (insertUse.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertUse.InputValue.Trim();
                        if (targetConfig.RegisterSyncUse != newValue)
                        {
                            string oldValue = targetConfig.RegisterSyncUse;
                            targetConfig.RegisterSyncUse = newValue;

                            insertUse.Close();
                            ChangeDatamsg = $"RobotRegisterSync,RobotRegisterSync Config{targetConfig.Id} Use Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                            if (targetConfig.RegisterSyncUse == "Use")
                            {
                                dtg_RobotRegisterSync.Rows[e.RowIndex].DefaultCellStyle.BackColor = main.skinColor;
                                dtg_RobotRegisterSync.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                            }
                            else
                            {
                                dtg_RobotRegisterSync.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                                dtg_RobotRegisterSync.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                            }
                        }

                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_RobotRegisterSync_RegiaterSyncGroup"].Index)
                {
                    var insertNum = new GroupSelectForm(main, uow);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetConfig.PositionGroup != newValue)
                        {
                            string oldValue = targetConfig.PositionGroup;
                            targetConfig.PositionGroup = newValue;
                            insertNum.Close();
                            ChangeDatamsg = $"RobotRegisterSync,RobotRegisterSync Config{targetConfig.Id} RegiaterSyncGroup from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }


                    }
                }


                else if (e.ColumnIndex == grid.Columns["DGV_RobotRegisterSync_RegiaterSyncPOSName"].Index)
                {
                    var insert = new PositionZoneSelectForm(targetConfig.PositionGroup, uow);
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.PositionName != newValue)
                        {
                            string oldValue = targetConfig.PositionName;
                            targetConfig.PositionName = newValue;

                            insert.Close();
                            ChangeDatamsg = $"RobotRegisterSync,RobotRegisterSync Config {targetConfig.Id} PositionZone Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_RobotRegisterSync_ACSRobotGroup"].Index)
                {
                    var insertNum = new GroupSelectForm(main, uow);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetConfig.ACSRobotGroup != newValue)
                        {
                            string oldValue = targetConfig.ACSRobotGroup;
                            targetConfig.ACSRobotGroup = newValue;
                            insertNum.Close();
                            ChangeDatamsg = $"RobotRegisterSync,RobotRegisterSync Config{targetConfig.Id} ACSRobotGroup from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }


                    }
                }
                else if (e.ColumnIndex == grid.Columns["DGV_RobotRegisterSync_RegisterNo"].Index)
                {
                    var insert = new NumPadForm(dtg_RobotRegisterSync.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_RobotRegisterSync", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (Convert.ToString(targetConfig.RegisterNo) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.RegisterNo);
                            targetConfig.RegisterNo = int.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"RobotRegisterSync,RobotRegisterSync Config {targetConfig.Id} RegisterNo Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}", ChangeDatamsg);

                        }
                    }
                }

                else if (e.ColumnIndex == grid.Columns["DGV_RobotRegisterSync_RegisterValue"].Index)
                {
                    var insert = new NumPadForm(dtg_RobotRegisterSync.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "dtg_RobotRegisterSync", "RT15");
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (Convert.ToString(targetConfig.RegisterValue) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.RegisterValue);
                            targetConfig.RegisterValue = int.Parse(newValue);
                            insert.Close();
                            ChangeDatamsg = $"RobotRegisterSync,RobotRegisterSync Config {targetConfig.Id} RegisterValue Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}", ChangeDatamsg);

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                RobotRegisterSync_Display();
            }


            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    selectedCell.Value = newValue;
                    selectedRowTag = targetConfig;
                    uow.RobotRegisterSyncs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = changeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }
        }


        private void txt_RobotRegistarSyncMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, ((TextBox)sender).AccessibleDescription);
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();

                if (((TextBox)sender).Text != int.Parse(insertNum.InputValue).ToString())
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("RobotRegistarSync_MaxNum", newValue);
                    ConfigData.RobotRegistarSync_MaxNum = int.Parse(AppConfiguration.GetAppConfig("RobotRegistarSync_MaxNum"));
                    insertNum.Close();
                    uow.RobotRegisterSyncs.Validate_DB_Items();
                    RobotRegisterSync_Display();
                    main.UserLog("RobotRegisterSync", $"RobotRegistarSyncMaxNum changed from {oldValue} to {newValue}.");

                }
            }
        }

        private void btn_RegisterSyncBackUpSaveFile_Click(object sender, EventArgs e)
        {
            if (dtg_RobotRegisterSync.Rows.Count == 0) return;
            main.SaveAsDataGridviewToCSV(dtg_RobotRegisterSync);
            main.UserLog("RobotRegisterSync Screen", " BackUp Click ");
        }

    }
}
