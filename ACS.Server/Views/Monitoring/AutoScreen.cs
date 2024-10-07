using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using log4net;

namespace INA_ACS_Server.OPWindows
{
    public partial class AutoScreen : Form
    {
        private readonly MainForm main;
        private readonly IUnitOfWork uow;
        private readonly static ILog TabletLogger = LogManager.GetLogger("TabletEvent");

        private readonly Font textFont1 = new Font("맑은 고딕", 15, FontStyle.Bold);
        private readonly Font textFont2 = new Font("맑은 고딕", 15, FontStyle.Bold);
        private readonly Font textFont3 = new Font("Arial", 10, FontStyle.Bold);
        private readonly Font textFont4 = new Font("Arial", 9, FontStyle.Bold);
        private readonly Color backColorControl = SystemColors.Control;
        private readonly Color backColorGreen = Color.Green;
        private readonly Color backColorRed = Color.Red;
        private readonly Color backColorWhite = Color.White;
        private readonly Color backColorYellow = Color.Yellow;
        private DataTable GridDT = new DataTable();

        public AutoScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                    e.Cancel = true;
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Auto_ScreenInit();
            //subFuncMiRGridInit();
            MiRStatusMonitorInit();
            CallStatusMonitorInit();
            SchedulerMonitorInit();

            dtgAuto_Scheduler_Status.DoubleBuffered(true);
            dtgAuto_Scheduler_Status.AlternatingRowsDefaultCellStyle = null;
            dtgAuto_Scheduler_Status.Columns["DGV_Schedule_Status_MissionReturnID"].Visible = false;

            dtgAuto_Job_Status.DoubleBuffered(true);
            dtgAuto_Job_Status.AlternatingRowsDefaultCellStyle = null;
            dtgAuto_Job_Status.Columns["DGV_Job_Status_ReturnType"].Visible = false;    //job 상태표시 Reurn Type 사용안함

            //btn_Cancel_All_Job.Visible = true;
            btn_Cancel_All_Job.Visible = false;

        }

        public void Auto_ScreenInit()
        {
            this.BackColor = Color.FromArgb(80, 89, 96);

            P_Robot.BackColor = main.skinColor;
            lbl_Robot.ForeColor = Color.White;
            lbl_Robot.BackColor = main.skinColor;
            lbl_Robot.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
            gridView1.Appearance.Empty.BackColor = main.skinColor;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;

            p_possible.BackColor = main.skinColor;
            lbl_possible.ForeColor = Color.White;
            lbl_possible.BackColor = main.skinColor;
            lbl_possible.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
            dtgAuto_Job_Status.BackgroundColor = main.skinColor;
            dtgAuto_Job_Status.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtgAuto_Job_Status.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            p_mission.BackColor = main.skinColor;
            lbl_mission.ForeColor = Color.White;
            lbl_mission.BackColor = main.skinColor;
            lbl_mission.Font = new Font("맑은 고딕", 15, FontStyle.Bold);
            dtgAuto_Scheduler_Status.BackgroundColor = main.skinColor;
            dtgAuto_Scheduler_Status.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtgAuto_Scheduler_Status.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }

        private void MiRStatusMonitorInit()
        {
            GridDT.Columns.AddRange(new DataColumn[] {
                    new DataColumn("DGV_MiR_Status_Robot_Name"),
                    new DataColumn("DGV_MiR_State"),
                    new DataColumn("DGV_Fleet_State"),
                    new DataColumn("DGV_MiR_Status_Robot_Product"),
                    new DataColumn("DGV_MiR_Status_Robot_PositionName"),
                    new DataColumn("DGV_MiR_Status_Battery_Percent")
                });

            GridDT.Columns[0].Caption = "RobotName";
            GridDT.Columns[1].Caption = "RobotState";
            GridDT.Columns[2].Caption = "FleetState";
            GridDT.Columns[3].Caption = "Product";
            GridDT.Columns[4].Caption = "PositionName";
            GridDT.Columns[5].Caption = "Battery";

            GridDT.Rows.Clear();

            gridView1.ColumnPanelRowHeight = 70;
            gridView1.RowHeight = 82;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowHorizontalLines = DefaultBoolean.False;
            gridView1.OptionsView.ShowVerticalLines = DefaultBoolean.False;
            gridView1.FocusRectStyle = DrawFocusRectStyle.None;
            gridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            
            dtgAuto_MiR_Status.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            dtgAuto_MiR_Status.LookAndFeel.UseDefaultLookAndFeel = false;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            gridView1.Appearance.HeaderPanel.BackColor = Color.FromArgb(80, 89, 96);
            gridView1.Appearance.HeaderPanel.ForeColor = Color.White;
            gridView1.Appearance.Row.Font = textFont1;
            gridView1.Appearance.HeaderPanel.Font = textFont1;
        }

        private void CallStatusMonitorInit()
        {

            dtgAuto_Job_Status.ScrollBars = ScrollBars.Both;
            dtgAuto_Job_Status.AllowUserToResizeColumns = true;
            dtgAuto_Job_Status.ColumnHeadersHeight = 70;
            dtgAuto_Job_Status.RowTemplate.Height = 55;
            dtgAuto_Job_Status.ReadOnly = false;

            dtgAuto_Job_Status.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtgAuto_Job_Status.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtgAuto_Job_Status.ColumnHeadersDefaultCellStyle.Font = textFont1;                                         //Font 변경
            //dtgAuto_Job_Status.RowsDefaultCellStyle.ForeColor = Color.Black;
            dtgAuto_Job_Status.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬

            DataGridViewCell currentCellTemplate = dtgAuto_Job_Status.Columns[0].CellTemplate;
            dtgAuto_Job_Status.Columns.Clear();
            dtgAuto_Job_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Job_Status_LINECD", HeaderText = "LINECD", Width = 125, CellTemplate = currentCellTemplate });
            dtgAuto_Job_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Job_Status_POSTCD", HeaderText = "POSTCD", Width = 125, CellTemplate = currentCellTemplate });
            dtgAuto_Job_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Job_Status_MissionName", HeaderText = "MissionName", Width = 180, CellTemplate = currentCellTemplate });
            dtgAuto_Job_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Job_Status_MissionState", HeaderText = "MissionState", Width = 135, CellTemplate = currentCellTemplate });
            dtgAuto_Job_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Job_Status_CreateTime", HeaderText = "CreateTime", Width = 140, CellTemplate = currentCellTemplate });
            dtgAuto_Job_Status.Columns.Add(new DataGridViewButtonColumn() { Name = "DGV_Job_Status_JobCancelBtn", HeaderText = "Cancel", Text = "Cancel", Width = 80 });
            dtgAuto_Job_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Job_Status_ReturnType", HeaderText = "CreateTime", Width = 150, CellTemplate = currentCellTemplate });
            dtgAuto_Job_Status.Columns["DGV_Job_Status_MissionName"].HeaderText = "Mission" + "\r\n" + "Name";
            dtgAuto_Job_Status.Columns["DGV_Job_Status_MissionState"].HeaderText = "Mission" + "\r\n" + "State";

            dtgAuto_Scheduler_Status.Rows.Clear();
        }

        private void SchedulerMonitorInit()
        {
            dtgAuto_Scheduler_Status.ScrollBars = ScrollBars.Both;
            dtgAuto_Scheduler_Status.AllowUserToResizeColumns = true;
            dtgAuto_Scheduler_Status.ColumnHeadersHeight = 70;
            dtgAuto_Scheduler_Status.RowTemplate.Height = 82;
            dtgAuto_Scheduler_Status.ReadOnly = false;

            dtgAuto_Scheduler_Status.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    // 헤더 값을 가운데로 정렬
            dtgAuto_Scheduler_Status.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;                     // 헤더 값 두줄로 정렬
            dtgAuto_Scheduler_Status.ColumnHeadersDefaultCellStyle.Font = textFont1;                                         //Font 변경
            //dtgAuto_Scheduler_Status.RowsDefaultCellStyle.ForeColor = Color.Black;
            dtgAuto_Scheduler_Status.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                 // 셀 값을 가운데로 정렬

            DataGridViewCell currentCellTemplate = dtgAuto_Scheduler_Status.Columns[0].CellTemplate;
            dtgAuto_Scheduler_Status.Columns.Clear();
            dtgAuto_Scheduler_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Schedule_Status_MiR_Name", HeaderText = "RobotName", Width = 150, CellTemplate = currentCellTemplate });
            dtgAuto_Scheduler_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Schedule_Status_CallName", HeaderText = "CallName", Width = 225, CellTemplate = currentCellTemplate });
            dtgAuto_Scheduler_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Schedule_Status_MissionName", HeaderText = "MissionName", Width = 260, CellTemplate = currentCellTemplate });
            dtgAuto_Scheduler_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Schedule_Status_MissionState", HeaderText = "MissionState", Width = 200, CellTemplate = currentCellTemplate });
            dtgAuto_Scheduler_Status.Columns.Add(new DataGridViewColumn() { Name = "DGV_Schedule_Status_MissionReturnID", HeaderText = "ReturnID", Width = 155, CellTemplate = currentCellTemplate });

            dtgAuto_Scheduler_Status.Rows.Clear();
        }

        private void subFuncMiRGridInit()
        {
            //try
            //{
            //    MiRStatusMonitorInit();
            //    for (int iMiR_No = 0; iMiR_No < uow.Robots.GetCount(); iMiR_No++)
            //    {
            //        //AMB만 표시
            //        var robot = uow.Robots[iMiR_No];

            //        if (robot.RobotID > 0)
            //        {
            //            //화면상의 표기 하는 부분
            //            var newRowIndex = dtgAuto_MiR_Status.Rows.Add();
            //            var newRow = dtgAuto_MiR_Status.Rows[newRowIndex];
            //            newRow.Cells["DGV_MiR_Status_Robot_Name"].Value = "-";
            //            newRow.Cells["DGV_MiR_State"].Value = "Disconnect";
            //            newRow.Cells["DGV_Fleet_State"].Value = "unavailable";
            //            newRow.Cells["DGV_MiR_Status_Battery_Percent"].Value = "-";
            //            //newRow.Cells["DGV_MiR_Status_IP"].Value = "-";
            //            newRow.Cells["DGV_MiR_Status_Robot_Product"].Value = "-";
            //            newRow.Cells["DGV_MiR_Status_Robot_PositionName"].Value = "-";
            //            DataGridViewRowAddDateFontChange(newRow, textFont2);
            //        }
            //    }
            //    void DataGridViewRowAddDateFontChange(DataGridViewRow newRow, Font font)
            //    {
            //        //Font변경
            //        newRow.Cells["DGV_MiR_Status_Robot_Name"].Style.Font = font;
            //        newRow.Cells["DGV_MiR_State"].Style.Font = font;
            //        newRow.Cells["DGV_MiR_Status_Battery_Percent"].Style.Font = font;
            //        newRow.Cells["DGV_MiR_Status_Robot_Product"].Style.Font = font;
            //        newRow.Cells["DGV_Fleet_State"].Style.Font = font;
            //        newRow.Cells["DGV_MiR_Status_Robot_PositionName"].Style.Font = font;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    main.LogExceptionMessage(ex);
            //}
        }


        #region Status Display 관련
        private void subFunc_MiR_Status_Display() //MiR 상태 표기 하는 함수
        {
            try
            {
                int GridCount = 0;
                GridDT.Rows.Clear();

                foreach (var robot in uow.Robots.GetAll())
                {
                    DataRow row = GridDT.NewRow();

                    if (robot.FleetState == FleetState.unavailable)
                        row["DGV_Fleet_State"] = "Disconnect";

                    if (robot.RobotID > 0 && robot.ACSRobotGroup == "AMB" && robot.ConnectState == true)
                    {
                        if (robot.StateText == "Disconnect")
                        {
                            main.EventLog("AutoScreen", robot.RobotName + "Connect");
                        }

                        //화면상의 표기 하는 부분(Fleet 접속)
                        row["DGV_MiR_Status_Robot_Name"] = robot.RobotName;
                        //row.Cells["DGV_MiR_State"].Value = robot.StateText;

                        if (!robot.ConnectState)
                        {
                            row["DGV_MiR_State"] = "unavailable";
                        }
                        else
                        {
                            row["DGV_MiR_State"] = robot.StateText;
                        }

                        row["DGV_MiR_Status_Battery_Percent"] = robot.BatteryPercent.ToString("0.00") + "%";
                        row["DGV_Fleet_State"] = robot.FleetStateText.ToString();
                        //row.Cells["DGV_MiR_Status_IP"].Value = robot.RobotIp.ToString();
                        row["DGV_MiR_Status_Robot_Product"] = robot.Product != "Null" ? robot.Product : "";
                        row["DGV_MiR_Status_Robot_PositionName"] = robot.PositionZoneName != "Null" ? robot.PositionZoneName : "";

                        GridDT.Rows.InsertAt(row, GridCount);
                        GridCount++;
                    }
                    else if (robot.ConnectState == false)
                    {
                        if (robot.StateText == "Connect")
                        {
                            main.EventLog("AutoScreen", robot.RobotName + "Disconnect");
                        }

                        //화면상의 표기 하는 부분(Fleet 미접속)
                        row["DGV_MiR_Status_Robot_Name"] = robot.RobotName;
                        row["DGV_MiR_State"] = "Disconnect";
                        row["DGV_MiR_Status_Battery_Percent"] = "-";
                        //row.Cells["DGV_MiR_Status_IP"].Value = robot.RobotIp.ToString(); ;
                        row["DGV_MiR_Status_Robot_Product"] = "-";
                        row["DGV_MiR_Status_Robot_PositionName"] = "-";
                    }
                }

                dtgAuto_MiR_Status.DataSource = GridDT;

                gridView1.ClearSelection();
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void subFunc_Scheduler_Status_Display()
        {
            try
            {
                dtgAuto_Scheduler_Status.Columns["DGV_Schedule_Status_MiR_Name"].ReadOnly = true;
                dtgAuto_Scheduler_Status.Columns["DGV_Schedule_Status_CallName"].ReadOnly = true;
                dtgAuto_Scheduler_Status.Columns["DGV_Schedule_Status_MissionName"].ReadOnly = true;
                dtgAuto_Scheduler_Status.Columns["DGV_Schedule_Status_MissionState"].ReadOnly = true;
                dtgAuto_Scheduler_Status.Columns["DGV_Schedule_Status_MissionReturnID"].ReadOnly = true;

                //var runMissions = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState == "Executing"); // 완료된 미션을 검색한다
                //var runMissions = uow.Missions.Find(m => m.ReturnID > 0).ToList(); // 완료된 미션을 검색한다
                var runMissions = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState != "Done"); // 완료된 미션을 검색한다

                // 실행중인 미션수 변경시, 그리드 갱신한다
                //if (runMissions.Count() != preDoneMissionsCount)
                {
                    dtgAuto_Scheduler_Status.Rows.Clear();
                    dtgAuto_Scheduler_Status.RowTemplate.Height = 79;

                    foreach (var m in runMissions)
                    {
                        int newRowIndex = dtgAuto_Scheduler_Status.Rows.Add();
                        var newRow = dtgAuto_Scheduler_Status.Rows[newRowIndex];

                        switch (m.MissionState)
                        {
                            case "Sending":
                            case "wait":
                                newRow.Cells["DGV_Schedule_Status_MissionState"].Style.ForeColor = Color.Yellow;
                                break;
                            case "Executing":
                                newRow.Cells["DGV_Schedule_Status_MissionState"].Style.ForeColor = Color.Chartreuse;
                                break;
                            case "Abort":
                            case "Aborted":
                                newRow.Cells["DGV_Schedule_Status_MissionState"].Style.ForeColor = Color.OrangeRed;
                                break;
                            default:
                                break;
                        }
                        if (m.MissionName != null)
                        {
                            string callName = m.CallName.Replace("_", "\r\n");

                            newRow.Cells["DGV_Schedule_Status_MiR_Name"].Value = m.RobotName;
                            //newRow.Cells["DGV_Schedule_Status_CallName"].Value = m.CallName;
                            newRow.Cells["DGV_Schedule_Status_CallName"].Value = callName;
                            newRow.Cells["DGV_Schedule_Status_MissionName"].Value = m.MissionName;
                            newRow.Cells["DGV_Schedule_Status_MissionState"].Value = m.MissionState;
                            newRow.Cells["DGV_Schedule_Status_MissionReturnID"].Value = m.ReturnID;

                            newRow.Tag = m; // 특정 행 미션을 삭제시 참조하기 위해 태그 설정
                        }
                        else
                        {
                            string callName = m.CallName.Replace("_", "\r\n");

                            newRow.Cells["DGV_Schedule_Status_MiR_Name"].Value = m.RobotName;
                            //newRow.Cells["DGV_Schedule_Status_CallName"].Value = m.CallName;
                            newRow.Cells["DGV_Schedule_Status_CallName"].Value = callName;
                            newRow.Cells["DGV_Schedule_Status_MissionName"].Value = m.ErrorMissionName;
                            newRow.Cells["DGV_Schedule_Status_MissionState"].Value = m.MissionState;
                            newRow.Cells["DGV_Schedule_Status_MissionReturnID"].Value = m.ReturnID;

                            newRow.Tag = m; // 특정 행 미션을 삭제시 참조하기 위해 태그 설정
                        }
                        DataGridViewRowAddDateFontChange(newRow, textFont2);
                    }
                    void DataGridViewRowAddDateFontChange(DataGridViewRow newRow, Font font)
                    {
                        // font 설정
                        newRow.Cells["DGV_Schedule_Status_MiR_Name"].Style.Font = font;
                        newRow.Cells["DGV_Schedule_Status_CallName"].Style.Font = font;
                        newRow.Cells["DGV_Schedule_Status_MissionName"].Style.Font = font;
                        newRow.Cells["DGV_Schedule_Status_MissionState"].Style.Font = font;
                        newRow.Cells["DGV_Schedule_Status_MissionReturnID"].Style.Font = font;
                    }
                    dtgAuto_Scheduler_Status.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        private void subFunc_Job_Status_Display() //CallButton 상태 표기 하는 함수
        {
            try
            {
                dtgAuto_Job_Status.Columns["DGV_Job_Status_LINECD"].ReadOnly = true;
                dtgAuto_Job_Status.Columns["DGV_Job_Status_POSTCD"].ReadOnly = true;
                dtgAuto_Job_Status.Columns["DGV_Job_Status_MissionState"].ReadOnly = true;
                dtgAuto_Job_Status.Columns["DGV_Job_Status_MissionName"].ReadOnly = true;
                dtgAuto_Job_Status.Columns["DGV_Job_Status_CreateTime"].ReadOnly = true;

                var jobs = uow.Jobs.GetAll().OrderByDescending(j => j.JobPriority);

                // 그리드 갱신한다 (전체행 삭제후 다시 추가)
                {
                    int firstRowIndex = dtgAuto_Job_Status.FirstDisplayedScrollingRowIndex;

                    dtgAuto_Job_Status.Rows.Clear();
                    dtgAuto_Job_Status.RowTemplate.Height = 90;//55; // 32;// 21; // 32;

                    //for (int n = 0; n < dtgAuto_Job_Status.Columns.Count; n++) // 컬럼 폭 설정
                    //{
                    //    //dtgAuto_Job_Status.Columns[n].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //}

                    foreach (var job in jobs)
                    {
                        int newRowIndex = dtgAuto_Job_Status.Rows.Add();
                        var newRow = dtgAuto_Job_Status.Rows[newRowIndex];

                        string linecd = "";
                        string postcd = "";

                        int index = job.CallName.LastIndexOf('_');
                        if (index < 0)
                        {
                            linecd = job.CallName;
                            postcd = "";
                        }
                        else
                        {
                            var FloowName = uow.FloorMapIDConfigs.GetAll().FirstOrDefault();
                            if (FloowName != null)
                            {
                                linecd = job.CallName.Substring(0, index);
                                if (linecd.StartsWith(FloowName.FloorName))
                                {
                                    linecd = linecd.Replace(FloowName.FloorName, "");
                                    linecd = FloowName.FloorName + "\r\n" + linecd;
                                }
                                postcd = job.CallName.Substring(index + 1);
                                if (postcd.StartsWith(FloowName.FloorName))
                                {
                                    postcd = postcd.Replace(FloowName.FloorName, "");
                                    postcd = FloowName.FloorName + "\r\n" + postcd;
                                }
                            }
                            else
                            {
                                linecd = job.CallName.Substring(0, index);
                                postcd = job.CallName.Substring(index + 1);
                            }

                        }

                        newRow.Cells["DGV_Job_Status_LINECD"].Value = linecd;
                        newRow.Cells["DGV_Job_Status_POSTCD"].Value = postcd;
                        //newRow.Cells["DGV_Job_Status_ReturnType"].Value = job.PopCallReturnType;
                        //newRow.Cells["DGV_Job_Status_ReturnType"].Value = job.PopCallReturnType == "N" ? "투입" : "회수";
                        newRow.Cells["DGV_Job_Status_MissionState"].Value = job.Missions.Count == job.MissionTotalCount ? job.JobState.ToString() : "!ERR!";
                        newRow.Cells["DGV_Job_Status_MissionName"].Value = job.Missions != null ? string.Join(" / ", job.Missions.Select(x => x.MissionName)) : "";
                        newRow.Cells["DGV_Job_Status_CreateTime"].Value = job.JobCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        newRow.Cells["DGV_Job_Status_JobCancelBtn"].Value = "Cancel";

                        newRow.Tag = job; // 특정 행 미션을 삭제시 참조하기 위해 태그 설정

                        newRow.Cells["DGV_Job_Status_MissionName"].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                        Color cellColor;
                        switch (job.JobState)
                        {
                            case JobState.JobAborted:
                            case JobState.JobInvalid:
                                cellColor = Color.Red;
                                break;
                            case JobState.JobExecuting:
                                cellColor = Color.Chartreuse;
                                break;
                            case JobState.JobDone:
                                cellColor = Color.DimGray;
                                break;
                            default:
                                cellColor = Color.WhiteSmoke;
                                break;
                        }
                        newRow.Cells["DGV_Job_Status_MissionState"].Style.ForeColor = cellColor;

                        DataGridViewRowAddDateFontChange(newRow, textFont2);
                    }

                    if (firstRowIndex >= 0 && firstRowIndex < dtgAuto_Job_Status.Rows.Count)
                    {
                        dtgAuto_Job_Status.FirstDisplayedScrollingRowIndex = firstRowIndex;
                    }

                    dtgAuto_Job_Status.ClearSelection();
                }
                void DataGridViewRowAddDateFontChange(DataGridViewRow newRow, Font font)
                {
                    // font 설정
                    newRow.Cells["DGV_Job_Status_LINECD"].Style.Font = font;
                    newRow.Cells["DGV_Job_Status_POSTCD"].Style.Font = font;
                    newRow.Cells["DGV_Job_Status_MissionName"].Style.Font = font;
                    newRow.Cells["DGV_Job_Status_MissionState"].Style.Font = font;
                    newRow.Cells["DGV_Job_Status_CreateTime"].Style.Font = font;
                    newRow.Cells["DGV_Job_Status_JobCancelBtn"].Style.Font = font;
                    //newRow.Cells["DGV_Job_Status_LINENM"].Style.Font = font;
                    //newRow.Cells["DGV_Job_Status_ReturnType"].Style.Font = font;
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        #endregion

        #region Timer

        private void AutoDisplay_Timer_Tick(object sender, EventArgs e)
        {
            AutoDisplay_Timer.Enabled = false;
            subFunc_MiR_Status_Display();
            if (uow.Missions.NeedUpdateUI == true || uow.Jobs.NeedUpdateUI == true)
            {
                subFunc_Scheduler_Status_Display();
                subFunc_Job_Status_Display();

                uow.Missions.NeedUpdateUI = false;
                uow.Jobs.NeedUpdateUI = false;
            }

            AutoDisplay_Timer.Enabled = true;
        }

        #endregion

        private void btn_Cancel_All_Job_Click(object sender, EventArgs e)
        {
            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE_ALL });
            //MissionCommandQueue.Enqueue(new MissionCommand { Code = MissionCommandCode.FORCE_REMOVE });
        }

        #region ClickEvent 관련

        private void dtgAuto_Job_Status_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 5)//미션삭제 가능
                {
                    if (ConfigData.sAccessLevel == "INATECH" || ConfigData.sAccessLevel == "Engineer")
                    {
                        // 클릭한 행의 태그에서 삭제할 미션 알아낸다
                        var job = (Job)dtgAuto_Job_Status.Rows[e.RowIndex].Tag;
                        if (job != null)
                        {
                            // 유저에게 확인후 미션 삭제한다
                            DialogResult result = MessageBox.Show(" Job을 Cancel 하시겠습니까 ? ", "Job_Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                main.UserLog("AutoScreen", " 미션 이름 : " + job.CallName + " Job Cancel Click / JobCancel");
                                main.EventLog("AutoScreen", " 미션 이름 : " + job.CallName + " Job Cancel Click / JobCancel");
                                main.ACS_UI_Log("AutoScreen" + " / " + " 미션 이름 : " + job.CallName + " Job Cancel Click / JobCancel");

                                JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE_BY_ACS, Text = job.CallName, Extra1 = job.PopServerId });

                                var Missions = uow.MissionsSpecific.GetAll(0).Where(x => x.CallName == job.CallName);

                                foreach (var item in Missions)
                                {
                                    MissionsSpecific missionsSpecific = new MissionsSpecific();
                                    missionsSpecific.No = item.No;

                                    uow.MissionsSpecific.Remove(missionsSpecific);
                                }
                                

                                subFunc_Job_Status_Display(); // 바로 갱신해 준다.
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Job Cancel 권한 Level이 아닙니다." + "\r\n" + "확인후 다시 시도해주세요");
                    }
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }


        #endregion

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (gridView1.GetDataRow(e.RowHandle) == null) return;

            if (e.Column.FieldName == "DGV_MiR_Status_Robot_Name")
            {
                e.Appearance.ForeColor = Color.White;
            }

            if (e.Column.FieldName == "DGV_Fleet_State")
            {
                string str = gridView1.GetDataRow(e.RowHandle)["DGV_Fleet_State"].ToString();

                if (str == "unavailable")
                    e.Appearance.ForeColor = Color.OrangeRed;
            }

            if (e.Column.FieldName == "DGV_Fleet_State")
            {
                string str = gridView1.GetDataRow(e.RowHandle)["DGV_Fleet_State"].ToString();

                if (str == "error" || str == "emergency_stop")
                    e.Appearance.ForeColor = Color.OrangeRed;
                else
                    e.Appearance.ForeColor = Color.White;
            }

            if (e.Column.FieldName == "DGV_MiR_State")
            {
                string str = gridView1.GetDataRow(e.RowHandle)["DGV_MiR_State"].ToString();

                if (str == "Disconnect")
                    e.Appearance.ForeColor = Color.OrangeRed;
                else if (str == "Ready")
                    e.Appearance.ForeColor = Color.LightBlue;
                else if (str == "Pause" || str == "ManualControl")
                    e.Appearance.ForeColor = Color.Yellow;
                else if (str == "Executing")
                    e.Appearance.ForeColor = Color.Chartreuse;
                else if (str == "Error" || str == "EmergencyStop")
                    e.Appearance.ForeColor = Color.OrangeRed;
                else
                    e.Appearance.ForeColor = Color.DimGray;
            }

            if (e.Column.FieldName == "DGV_MiR_Status_Robot_Product")
            {
                e.Appearance.ForeColor = Color.White;
            }

            if (e.Column.FieldName == "DGV_MiR_Status_Robot_PositionName")
            {
                e.Appearance.ForeColor = Color.White;
            }

            if (e.Column.FieldName == "DGV_MiR_Status_Battery_Percent")
            {
                var str = gridView1.GetDataRow(e.RowHandle)["DGV_MiR_Status_Battery_Percent"].ToString();
                var value = str.Split('%')[0];

                var result = double.TryParse(value, out double Result);

                if (result)
                {
                    if (Result < 20.00)
                        e.Appearance.ForeColor = Color.OrangeRed;
                    else if (Result > 20.00 && Result < 60.00)
                        e.Appearance.ForeColor = Color.Yellow;
                    else
                        e.Appearance.ForeColor = Color.LimeGreen;
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

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;

            e.Appearance.BackColor = main.skinColor;
        }
    }
}