using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using INA_ACS_Server.UI;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class JobConfigScreen : Form
    {
        private readonly MainForm main;
        private readonly IUnitOfWork uow;
        private DataTable GridDT = new DataTable();

        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);
        private readonly Font textFont2 = new Font("Arial", 12);
        private readonly Font textFont3 = new Font("굴림", 9, FontStyle.Bold);

        public JobConfigScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None; //윈도우(상단) 테두리 제거 source code

            Init();
            JobConfigTextBoxInit();
            JobConfigsInit();
            JobConfigDisplay();
        }

        private void Init()
        {
            this.BackColor = main.skinColor;

            groupBox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            btn_JobConfigBackup.BackColor = main.skinColor;
            btn_JobConfigBackup.ForeColor = Color.White;
            gridView1.Appearance.Empty.BackColor = main.skinColor;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
        }

        private void JobConfigTextBoxInit()
        {
            txt_JobConfigsMaxNum.ReadOnly = true;
            txt_JobConfigsMaxNum.BackColor = Color.White;
            txt_JobConfigsMaxNum.Text = ConfigData.JobConfig_MaxNum.ToString();
        }

        private void JobConfigsInit()
        {
            GridDT.Columns.AddRange(new DataColumn[] {
                    new DataColumn("DGV_JobConfig_Id"),
                    new DataColumn("DGV_JobConfig_JobConfigUse"),
                    new DataColumn("DGV_JobConfig_ACSMissionGroup"),
                    new DataColumn("DGV_JobConfig_StartZone"),
                    new DataColumn("DGV_JobConfig_EndZone"),
                    new DataColumn("DGV_JobConfig_JobMissionName1"),
                    new DataColumn("DGV_JobConfig_JobMissionName2"),
                    new DataColumn("DGV_JobConfig_JobMissionName3"),
                    new DataColumn("DGV_JobConfig_JobMissionName4"),
                    new DataColumn("DGV_JobConfig_jobCallSignal"),
                    new DataColumn("DGV_JobConfig_ExecuteBattery"),
                    new DataColumn("DGV_JobConfig_Priority"),
                    new DataColumn("DGV_JobConfig_TransportCountActive"),
                    new DataColumn("DGV_JobConfig_SourceFloor"),
                    new DataColumn("DGV_JobConfig_DestFloor"),
                    new DataColumn("DGV_JobConfig_CallSystem")
                });

            GridDT.Columns[0].Caption = "Index";
            GridDT.Columns[1].Caption = "Use" + "\r\n" + "Unuse";
            GridDT.Columns[2].Caption = "ACS" + "\r\n" + "Mission Group";
            GridDT.Columns[3].Caption = "StartZone";
            GridDT.Columns[4].Caption = "EndZone";
            GridDT.Columns[5].Caption = "JobMissionName1";
            GridDT.Columns[6].Caption = "JobMissionName2";
            GridDT.Columns[7].Caption = "JobMissionName3";
            GridDT.Columns[8].Caption = "JobMissionName4";
            GridDT.Columns[9].Caption = "JobCall" + "\r\n" + "Signal";
            GridDT.Columns[10].Caption = "Execute" + "\r\n" + "Battery";
            GridDT.Columns[11].Caption = "Priority";
            GridDT.Columns[12].Caption = "Transport" + "\r\n" + "Count" + "\r\n" + "Acitve";
            GridDT.Columns[13].Caption = "SourceFloor";
            GridDT.Columns[14].Caption = "DestFloor";
            GridDT.Columns[15].Caption = "Call" + "\r\n" + "System";

            gridView1.ColumnPanelRowHeight = 70;
            gridView1.RowHeight = 82;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowHorizontalLines = DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DefaultBoolean.False;
            gridView1.FocusRectStyle = DrawFocusRectStyle.None;
            gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            dtg_JobConfig.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            dtg_JobConfig.LookAndFeel.UseDefaultLookAndFeel = false;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            gridView1.Appearance.HeaderPanel.BackColor = Color.FromArgb(80, 89, 96);
            gridView1.Appearance.HeaderPanel.ForeColor = Color.White;
            gridView1.Appearance.Row.Font = textFont1;
            gridView1.Appearance.HeaderPanel.Font = textFont1;
        }

        private bool flag = true;
        private void JobConfigDisplay()
        {
            int GridCount = 0;
            dtg_JobConfig.RepositoryItems.Clear();
            GridDT.Rows.Clear();

            RepositoryItemButtonEdit CallItem = new RepositoryItemButtonEdit();
            CallItem.Buttons.Clear();

            foreach (var JobCfg in uow.JobConfigs.GetAll())
            {
                DataRow row = GridDT.NewRow();

                string[] ZoneName = JobCfg.CallName.Split('_');
                row["DGV_JobConfig_Id"] = JobCfg.Id;
                row["DGV_JobConfig_JobConfigUse"] = JobCfg.JobConfigUse;
                row["DGV_JobConfig_ACSMissionGroup"] = JobCfg.ACSMissionGroup;
                row["DGV_JobConfig_StartZone"] = ZoneName[0];
                row["DGV_JobConfig_EndZone"] = ZoneName[1];
                row["DGV_JobConfig_ExecuteBattery"] = JobCfg.ExecuteBattery + "%";
                row["DGV_JobConfig_Priority"] = JobCfg.JobPriority;
                row["DGV_JobConfig_jobCallSignal"] = JobCfg.jobCallSignal;
                row["DGV_JobConfig_TransportCountActive"] = JobCfg.TransportCountActive;
                row["DGV_JobConfig_SourceFloor"] = JobCfg.SourceFloor;
                row["DGV_JobConfig_DestFloor"] = JobCfg.DestFloor;

                CallItem = new RepositoryItemButtonEdit();
                CallItem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                CallItem.Buttons.Clear();
                CallItem.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinRight));
                dtg_JobConfig.RepositoryItems.Add(CallItem);
                row["DGV_JobConfig_CallSystem"] = "Call".ToString();

                for (int i = 0; i < JobConfigModel.JOB_MISSION_TOTAL_COUNT; i++)
                {
                    if (i <= 3) row[$"DGV_JobConfig_JobMissionName{i + 1}"] = JobCfg.GetJobMissionName(i);
                }

                GridDT.Rows.InsertAt(row, GridCount);
                GridCount++;
            }

            if (flag)
            {
                dtg_JobConfig.DataSource = GridDT;
                flag = false;
            }

            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.Columns["DGV_JobConfig_CallSystem"].ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            gridView1.Columns["DGV_JobConfig_CallSystem"].ColumnEdit = CallItem;
            gridView1.ClearSelection();
            gridView1.BestFitColumns();
        }

        private void CallSystem(string CallName)
        {
            var Mission = uow.MissionsSpecific.GetAll(0).FirstOrDefault(x => x.CallName == CallName);

            if (Mission == null)
            {
                MissionsSpecific missionsSpecific = new MissionsSpecific();
                missionsSpecific.CallName = CallName;
                missionsSpecific.CallState = "wait";
                missionsSpecific.CallTime = DateTime.Now;
                missionsSpecific.Priority = 0;
                missionsSpecific.JobSection = "ACS";

                uow.MissionsSpecific.Add(missionsSpecific);

                MessageBox.Show("스케줄 생성 완료!");
            }
            else
            {
                MessageBox.Show("스케줄이 이미 있습니다.");
            }
        }

        private void btn_BackUpSaveFile_Click(object sender, EventArgs e)
        {

            if (gridView1.RowCount == 0) return;
            main.SaveAsDataGridviewToCSV(gridView1);
            main.UserLog("JobConfig Screen", " BackUp Click ");

        }

        private void txt_JobConfigsMaxNum_Click(object sender, EventArgs e)
        {
            NumPadForm insertNum = new NumPadForm(((TextBox)sender).Text, ((TextBox)sender).Name, "RT10");
            if (insertNum.ShowDialog() == DialogResult.OK)
            {
                string newValue = int.Parse(insertNum.InputValue).ToString();
                if (((TextBox)sender).Text != newValue)
                {
                    string oldValue = ((TextBox)sender).Text;

                    ((TextBox)sender).Text = newValue;
                    AppConfiguration.ConfigDataSetting("JobConfig_MaxNum", newValue);
                    ConfigData.JobConfig_MaxNum = Convert.ToInt32(AppConfiguration.GetAppConfig("JobConfig_MaxNum"));
                    insertNum.Close();
                    uow.JobConfigs.Validate_DB_Items();
                    JobConfigDisplay();
                    main.UserLog("JobConfig Screen", $"JobConfig_MaxNum changed from {oldValue} to {newValue}.");


                }
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            e.Appearance.Options.UseBackColor = true;

            GridView view = sender as GridView;

            string Use = view.GetRowCellDisplayText(e.RowHandle, "DGV_JobConfig_JobConfigUse");

            if (Use == "Use")
            {
                e.Appearance.BackColor = main.skinColor;
                e.Appearance.ForeColor = Color.White;
            }
            else
            {
                e.Appearance.BackColor = Color.Yellow;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0) return;

            GridView grid = sender as GridView;

            object selectedCell = grid.GetRowCellValue(e.RowHandle, grid.Columns[e.Column.ColumnHandle]);

            string Id = grid.GetRowCellDisplayText(e.RowHandle, "DGV_JobConfig_Id");
            JobConfigModel targetConfig = uow.JobConfigs.Find(m => m.Id == Convert.ToInt32(Id)).FirstOrDefault();

            string ChangeDatamsg = null;

            if (targetConfig != null)
            {

                if (e.Column == grid.Columns["DGV_JobConfig_JobConfigUse"])
                {
                    var insert = new UseSelectForm();
                    if (insert.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insert.InputValue.Trim();
                        if (targetConfig.JobConfigUse != newValue)
                        {
                            string oldValue = targetConfig.JobConfigUse;
                            targetConfig.JobConfigUse = newValue;
                            insert.Close();
                            ChangeDatamsg = $"JobConfig,Job Config{targetConfig.Id} jobConfigUse Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_ACSMissionGroup"])
                {
                    var insertNum = new GroupSelectForm(main, uow);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (targetConfig.ACSMissionGroup != newValue)
                        {
                            string oldValue = targetConfig.ACSMissionGroup;
                            targetConfig.ACSMissionGroup = newValue;
                            insertNum.Close();
                            ChangeDatamsg = $"JobConfig,Job Config{targetConfig.Id} ACSMissionGroup Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_StartZone"])
                {
                    string[] ZoneName = targetConfig.CallName.Split('_');
                    string oldCallName = "";
                    string newCallName = "";
                    PositionZoneSelectForm startZoneSelectForm = new PositionZoneSelectForm(targetConfig.ACSMissionGroup, uow);
                    if (startZoneSelectForm.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = startZoneSelectForm.InputValue.Trim();
                        if (ZoneName[0] != newValue)
                        {
                            oldCallName = targetConfig.CallName;
                            ZoneName[0] = newValue;

                            targetConfig.CallName = $"{ZoneName[0]}_{ZoneName[1]}";
                            newCallName = targetConfig.CallName;
                            startZoneSelectForm.Close();
                            ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} CallName Change from {oldCallName} to {newCallName}";
                            ChageData(newValue, ChangeDatamsg);

                        }
                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_EndZone"])
                {
                    string[] ZoneName = targetConfig.CallName.Split('_');
                    string oldCallName = "";
                    string newCallName = "";

                    PositionZoneSelectForm endZoneSelectForm = new PositionZoneSelectForm(targetConfig.ACSMissionGroup, uow);
                    if (endZoneSelectForm.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = endZoneSelectForm.InputValue.Trim();
                        if (ZoneName[1] != newValue)
                        {
                            oldCallName = targetConfig.CallName;
                            ZoneName[1] = newValue;

                            targetConfig.CallName = $"{ZoneName[0]}_{ZoneName[1]}";
                            newCallName = targetConfig.CallName;
                            endZoneSelectForm.Close();
                            ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} CallName Change from {oldCallName} to {newCallName}";
                            ChageData(newValue, ChangeDatamsg);

                        }
                    }
                }
                else if ((e.Column == grid.Columns["DGV_JobConfig_JobMissionName1"])
                         || (e.Column == grid.Columns["DGV_JobConfig_JobMissionName2"])
                         || (e.Column == grid.Columns["DGV_JobConfig_JobMissionName3"])
                         || (e.Column == grid.Columns["DGV_JobConfig_JobMissionName4"]))
                {
                    var insertNum = new MissionSelectForm(main);
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue;
                        string oldValue = selectedCell.ToString();
                        targetConfig.SetJobMissionName(e.Column.ColumnHandle - 5, insertNum.InputValue);
                        insertNum.Close();
                        ChangeDatamsg = $"JobConfig,Job Config{targetConfig.Id} SubMission{e.Column.ColumnHandle - 4} Change from {oldValue} to {newValue}";
                        ChageData(newValue, ChangeDatamsg);
                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_jobCallSignal"])
                {
                    var JobSignalSelect = new JobSignalSelectForm(main);
                    if (JobSignalSelect.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = JobSignalSelect.InputValue.Trim();
                        if (targetConfig.jobCallSignal != newValue)
                        {
                            string oldValue = targetConfig.jobCallSignal;
                            targetConfig.jobCallSignal = newValue;
                            JobSignalSelect.Close();
                            ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} jobCallSignal RegiStarValue Change from {oldValue} to {newValue}";
                            ChageData(newValue, ChangeDatamsg);
                        }
                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_ExecuteBattery"])
                {
                    string category = grid.GetRowCellDisplayText(e.RowHandle, "DGV_JobConfig_ExecuteBattery");

                    var insertNum = new NumPadForm(category, "dtg_JobConfig", "RT15");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (Convert.ToString(targetConfig.ExecuteBattery) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.ExecuteBattery);
                            targetConfig.ExecuteBattery = double.Parse(newValue);
                            insertNum.Close();
                            ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} ExecuteBattery Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}%", ChangeDatamsg);
                        }
                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_TransportCountActive"]) //Elevator Mode 사용(true) 미사용 (false)
                {
                    string category = grid.GetRowCellDisplayText(e.RowHandle, "DGV_JobConfig_TransportCountActive");

                    bool newValue = !Convert.ToBoolean(category);

                    if (targetConfig.TransportCountActive != newValue)
                    {

                        bool oldValue = targetConfig.TransportCountActive;
                        targetConfig.TransportCountActive = newValue;

                        ChangeDatamsg = $"JobConfig,Job Config{targetConfig.Id} TransportCountActive Change from {oldValue} to {newValue}";
                        ChageData(newValue.ToString(), ChangeDatamsg);

                    }
                }
                else if (e.Column == grid.Columns["DGV_JobConfig_Priority"]) //Job우선순위
                {
                    string category = grid.GetRowCellDisplayText(e.RowHandle, "DGV_JobConfig_Priority");

                    var insertNum = new NumPadForm(category, "dtg_JobConfig", "RT9");
                    if (insertNum.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = insertNum.InputValue.Trim();

                        if (Convert.ToString(targetConfig.JobPriority) != newValue)
                        {
                            string oldValue = Convert.ToString(targetConfig.JobPriority);
                            targetConfig.JobPriority = int.Parse(newValue);
                            insertNum.Close();
                            ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} JobPriority Change from {oldValue} to {newValue}";
                            ChageData($"{newValue}", ChangeDatamsg);
                        }
                    }
                }

                else if (e.Column == grid.Columns["DGV_JobConfig_SourceFloor"])
                {
                    DestFloorSelectForm floorSelectForm = new DestFloorSelectForm(targetConfig.ACSMissionGroup, uow);
                    if (floorSelectForm.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = floorSelectForm.InputValue.Trim();
                        targetConfig.SourceFloor = newValue;
                        floorSelectForm.Close();
                        ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} CallName Change from {targetConfig.SourceFloor} to {newValue}";
                        ChageData(newValue, ChangeDatamsg);
                    }
                }

                else if (e.Column == grid.Columns["DGV_JobConfig_DestFloor"])
                {
                    DestFloorSelectForm floorSelectForm = new DestFloorSelectForm(targetConfig.ACSMissionGroup, uow);
                    if (floorSelectForm.ShowDialog() == DialogResult.OK)
                    {
                        string newValue = floorSelectForm.InputValue.Trim();
                        targetConfig.DestFloor = newValue;
                        floorSelectForm.Close();
                        ChangeDatamsg = $"JobConfig,Job Config {targetConfig.Id} CallName Change from {targetConfig.DestFloor} to {newValue}";
                        ChageData(newValue, ChangeDatamsg);
                    }
                }

                else if (e.Column == grid.Columns["DGV_JobConfig_CallSystem"])
                {
                    string StartZone = grid.GetRowCellValue(e.RowHandle, grid.Columns["DGV_JobConfig_StartZone"]).ToString();
                    string EndZone = grid.GetRowCellValue(e.RowHandle, grid.Columns["DGV_JobConfig_EndZone"]).ToString();

                    string CallName = StartZone + "_" + EndZone;
                    CallSystem(CallName);
                }
            }
            else
            {
                MessageBox.Show("확인후 등록 해주시기 바랍니다.");
                JobConfigDisplay();
            }

            JobConfigDisplay();

            void ChageData(string newValue, string changeDatamsg)
            {
                if (changeDatamsg != null)
                {
                    uow.JobConfigs.Update(targetConfig);
                    string[] ChangeDatamsgSplit = ChangeDatamsg.Split(',');
                    main.UserLog(ChangeDatamsgSplit[0], ChangeDatamsgSplit[1]);
                }
            }
        }

        private void gridView1_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, 89, 96)), e.Bounds);
            using (Pen pen = new Pen(Color.FromArgb(80, 89, 96), 3))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds);
                e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect);
            }

            e.Info.InnerElements.DrawObjects(e.Info, e.Cache, Point.Empty);
            e.Handled = true;
        }
    }
}
