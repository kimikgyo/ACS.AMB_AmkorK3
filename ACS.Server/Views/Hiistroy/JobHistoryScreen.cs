using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class JobHistoryScreen : Form
    {
        private static readonly string connectionString = ConnectionStrings.DB1;

        private readonly MainForm main;
        private readonly IUnitOfWork uow;
        private BindingSource _bindingSource = null;
        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);


        public JobHistoryScreen(MainForm main, IUnitOfWork uow)
        {
            InitializeComponent();
            this.main = main;
            this.uow = uow;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                    e.Cancel = true;
            };

            this.Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            InitGrid();
            //DisplayData();
            dtg_JobHistory.DoubleBuffered(true);
            // 날짜픽커 포맷 설정
            dateTimePicker1.CustomFormat = "yyyy-MM-dd (dddd) HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd (dddd) HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;

            dateTimePicker1.Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 06:00:00"));
            dateTimePicker2.Value = Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 06:00:00"));

            chkboxRobotName.CheckedChanged += chkbox_CheckedChanged;
            chkboxCallName.CheckedChanged += chkbox_CheckedChanged;
            chkboxLineName.CheckedChanged += chkbox_CheckedChanged;
            chkboxPostName.CheckedChanged += chkbox_CheckedChanged;
            chkboxRobotName.Visible = true;
            chkboxLineName.Visible = false;
            chkboxPostName.Visible = false;
            chkboxCallName.Visible = true;
            chkboxResultReset.Visible = false;
            cboboxRobotName.Visible = true;
            cboboxLineName.Visible = false;
            cboboxPostName.Visible = false;
            cboboxCallName.Visible = true;
            btnSearchCount2.Visible = false;
            txt_TransportValue.Visible = false;
            lbl_TransportValue.Visible = false;


            this.BackColor = main.skinColor;
            btnSearch.BackColor = main.skinColor;
            btnSearch.ForeColor = Color.White;
            btnSearchCount1.BackColor = main.skinColor;
            btnSearchCount1.ForeColor = Color.White;
            btnSave.BackColor = main.skinColor;
            btnSave.ForeColor = Color.White;
            btnClear.BackColor = main.skinColor;
            btnClear.ForeColor = Color.White;

            label1.ForeColor = Color.White;
            txtSearchCount.BackColor = main.skinColor;
            txtSearchCount.ForeColor = Color.White;
            txtSearchCount.BorderStyle = BorderStyle.None;

            groupbox1.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            button1.BackColor = main.skinColor;
            button1.ForeColor = Color.White;
            button2.BackColor = main.skinColor;
            button2.ForeColor = Color.White;
            button3.BackColor = main.skinColor;
            button3.ForeColor = Color.White;

            groupBox2.ForeColor = Color.White;
            chkboxRobotName.ForeColor = Color.White;
            chkboxCallName.ForeColor = Color.White;
            chkboxResultOK.ForeColor = Color.White;
            chkboxResultNG.ForeColor = Color.White;
            //cboboxRobotName.BackColor = main.skinColor;
            //cboboxRobotName.ForeColor = Color.White;
            //cboboxCallName.BackColor = main.skinColor;
            //cboboxCallName.ForeColor = Color.White;

            dtg_JobHistory.ScrollBars = ScrollBars.Both;
            dtg_JobHistory.AllowUserToResizeColumns = true;
            dtg_JobHistory.ColumnHeadersHeight = 70;
            dtg_JobHistory.RowTemplate.Height = 82;
            dtg_JobHistory.ReadOnly = false;
            dtg_JobHistory.BackgroundColor = main.skinColor;
            dtg_JobHistory.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_JobHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_JobHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_JobHistory.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_JobHistory.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_JobHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_JobHistory.EnableHeadersVisualStyles = false;
            dtg_JobHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void InitGrid()
        {
            //// 데이터그리드뷰 설정
            dtg_JobHistory.RowHeadersVisible = false;
            //dtg_JobHistory.AllowUserToResizeRows = false;
            //dtg_JobHistory.AllowUserToResizeColumns = true;
            //dtg_JobHistory.AllowUserToAddRows = false;
            //dtg_JobHistory.AllowUserToDeleteRows = false;
            dtg_JobHistory.EditMode = DataGridViewEditMode.EditProgrammatically;
            dtg_JobHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtg_JobHistory.MultiSelect = true;
            ////dataGridView1.CellDoubleClick += (s, e) => { btnEdit_Click(this, null); };

            //// 데이터그리드뷰 헤더/셀 스타일 설정
            //dtg_JobHistory.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            //{
            //    Padding = new Padding(5, 3, 5, 3),
            //    //Alignment = DataGridViewContentAlignment.MiddleCenter,
            //    Alignment = DataGridViewContentAlignment.MiddleLeft,
            //    //WrapMode = DataGridViewTriState.True,
            //};
            //dtg_JobHistory.DefaultCellStyle = dtg_JobHistory.ColumnHeadersDefaultCellStyle;

            //var rowTemplate = dtg_JobHistory.RowTemplate;
            //rowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            //rowTemplate.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;// Bisque;
            //rowTemplate.Height = 30; // 35;

            // ==========================

            // row number 컬럼 추가
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.Name = "No";
            textboxColumn.HeaderText = "No";
            dtg_JobHistory.Columns.Insert(0, textboxColumn);

            // row number 컬럼 데이터 처리
            dtg_JobHistory.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) //Row Index에 표시되는 문자 서식을 지정하기 위함 
        {

            var dgv = (DataGridView)sender;

            if (e.RowIndex != dgv.NewRowIndex)
            {
                string colName = dgv.Columns[e.ColumnIndex].Name;

                if (colName == "No")
                {
                    e.Value = (e.RowIndex + 1).ToString();
                }
                else if (colName == "ResultCD")
                {
                    if (e.Value != null)
                    {
                        if (e.Value.ToString() == "0" || e.Value.ToString() == "10") e.Value = "삭제";
                        else if (e.Value.ToString() == "1" || e.Value.ToString() == "11") e.Value = "성공";
                        else if (e.Value.ToString() == "2" || e.Value.ToString() == "12") e.Value = "실패";
                        else if (e.Value.ToString() == "3" || e.Value.ToString() == "13") e.Value = "초기화";
                    }
                    else if (colName.EndsWith("Date") || colName.EndsWith("날짜"))
                    {
                        try
                        {
                            DateTime dateTime = (DateTime)e.Value;
                            e.Value = dateTime.ToString("yyyy-MM-dd");
                        }
                        catch (Exception ex)
                        {
                            e.Value = e.Value.ToString();
                            main.LogExceptionMessage(ex);
                        }
                    }
                    else if (colName.EndsWith("Time") || colName.EndsWith("시간"))
                    {
                        try
                        {
                            DateTime dateTime = (DateTime)e.Value;
                            e.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        catch (Exception ex)
                        {
                            e.Value = e.Value.ToString();
                            main.LogExceptionMessage(ex);
                        }
                    }
                }
            }
        }

        private void DisplayData()                                                                      //DisPlay
        {
            // 데이터소스 바인딩
            dtg_JobHistory.DataSource = null; // 초간단 Refresh
            dtg_JobHistory.DataSource = GetBindingSource_AnynymousType();

            //// id 컬럼 숨김
            //dataGridView1.Columns["Id"].Visible = false;
            //dataGridView1.Columns["PosName"].Visible = false;

            // 전체 컬럼폭 조정
            //dataGridView1.AutoResizeColumns(); // 데이터 많을 경우 느리다

            // 개별 컬럼폭 설정
            //                 0   CallName [CallName]
            //                 1  ,LineName [설비명]
            //                 2  ,PostName [목적지]
            //                 3  ,ResultCD [처리결과]
            //                 4  ,RobotName [처리로봇]
            //                 5  ,JobState [Job상태]
            //                 6  ,CallTime [Call시간]
            //                 7  ,JobCreateTime [Job생성시간]
            //                 8  ,JobFinishTime [Job완료시간]
            //                 9  ,JobElapsedTime [Job완료시간]
            //                10  ,MissionNames [미션네임]
            //                11  ,MissionStates [미션상태]
            //                11  ,TransportCountValue[반송량]

            int n = 0;
            dtg_JobHistory.Columns[n++].Width = 50;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 100;
            dtg_JobHistory.Columns[n++].Width = 150;
            dtg_JobHistory.Columns[n++].Width = 150;
            dtg_JobHistory.Columns[n++].Width = 150;
            dtg_JobHistory.Columns[n++].Width = 150;
            dtg_JobHistory.Columns[n++].Width = 100;
        }

        private void DisplayData_Count(DateTime dt1, DateTime dt2)                                      //Data 수량
        {
            // 데이터소스 바인딩
            dtg_JobHistory.DataSource = null; // 초간단 Refresh
            dtg_JobHistory.DataSource = GetBindingSource_Count_From_To(dt1, dt2);

            // 전체 컬럼폭 조정
            dtg_JobHistory.AutoResizeColumns(); // 데이터 많을 경우 느리다
        }

        private BindingSource GetBindingSource()
        {

            const string SELECT_SQL = @"
                           SELECT *
                            FROM JobHistory 
                            WHERE JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
                            ORDER BY JobFinishTime";

            var searchDate1 = DateTime.Today.AddDays(-30);
            var searchDate2 = DateTime.Today.AddDays(1);

            using (var con = new SqlConnection(connectionString))
            {
                //var list = await con.QueryAsync<JobLog>(SELECT_SQL, new { searchDate1, searchDate2 });
                //var bindingList = new BindingList<JobLog>((IList<JobLog>)list);
                //_bindingSource = new BindingSource(bindingList, null);
                //return _bindingSource;

                IEnumerable<dynamic> list = con.Query(SELECT_SQL);
                var bindingList = list.ToList().ToBindingList();
                _bindingSource = new BindingSource(bindingList, null);
                return _bindingSource;
            }
        }

        private BindingSource GetBindingSource_AnynymousType()
        {
            string SELECT_SQL = @"
                            SELECT *
                            FROM JobHistory 
                            WHERE JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2";

            if (chkboxRobotName.Checked) SELECT_SQL += " AND RobotName LIKE @robotName";
            if (chkboxCallName.Checked) SELECT_SQL += " AND CallName LIKE @callName";
            if (chkboxLineName.Checked) SELECT_SQL += " AND LineName LIKE @LineName";
            if (chkboxPostName.Checked) SELECT_SQL += " AND PostName LIKE @PostName";

            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == false && chkboxResultReset.Checked == false) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11)";
            if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == true && chkboxResultReset.Checked == false) SELECT_SQL += " AND (ResultCD = 2 OR ResultCD = 12)";
            if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == false && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 3 OR ResultCD = 13)";
            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == true && chkboxResultReset.Checked == false) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11 OR ResultCD = 2 OR ResultCD = 12)";
            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == false && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11 OR ResultCD = 3 OR ResultCD = 13)";
            if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == true && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 2 OR ResultCD = 12 OR ResultCD = 3 OR ResultCD = 13)";

            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == true && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11 OR ResultCD = 2 OR ResultCD = 12 OR ResultCD = 3 OR ResultCD = 13)";

            SELECT_SQL += " ORDER BY JobFinishTime";

            // 자정기준이므로 +1 day 해줘야 제대로 검색된다
            // ex. 10/28~10/28 검색: 2022.10.28 00:00:00 ~ 2022.10.29 00:00:00
            // ex. 10/28~10/29 검색: 2022.10.28 00:00:00 ~ 2022.10.30 00:00:00
            var searchDate1 = dateTimePicker1.Value;
            var searchDate2 = dateTimePicker2.Value;
            //var searchDate1 = dateTimePicker1.Value.Date;
            //var searchDate2 = dateTimePicker2.Value.Date.AddDays(1);

            var robotName = "%" + cboboxRobotName.Text.Trim() + "%";
            var callName = "%" + cboboxCallName.Text.Trim() + "%";
            var LineName = cboboxLineName.Text.Trim() + "%";
            var PostName = cboboxPostName.Text.Trim() + "%";

            using (var con = new SqlConnection(connectionString))
            {
                object queryParams = new { searchDate1, searchDate2, robotName, callName, LineName, PostName };
                var list = con.Query(SELECT_SQL, queryParams);

                //var viewList = list.Select(x => new
                //{
                // CallName [CallName]
                // LineName [설비명]
                // PostName [목적지]
                // ResultCD [처리결과]
                // RobotName [처리로봇]
                // JobState [Job상태]
                // CallTime [Call시간]
                // JobCreateTime [Job생성시간]
                // JobFinishTime [Job완료시간]
                // JobElapsedTime [Job완료시간]
                // MissionNames [미션네임]
                // MissionStates [미션상태]
                // TransportCountValue[반송량]
                //.ToList()
                //.ToBindingList();

                var viewList = list.Select(x => new
                {
                    x.CallName,
                    x.LineName,
                    x.PostName,
                    x.ResultCD,
                    x.RobotName,
                    x.JobState,
                    x.CallTime,
                    x.JobCreateTime,
                    x.JobFinishTime,
                    x.JobElapsedTime,
                    x.MissionNames,
                    x.MissionStates,
                    //x.TransportCountValue,
                })
                 .ToList()
                 .ToBindingList();
                //int TransportCountValue = 0;
                //foreach (var view in viewList)
                //{
                //    if (view.TransportCountValue != null) TransportCountValue = TransportCountValue + view.TransportCountValue;
                //}

                //txt_TransportValue.Text = TransportCountValue.ToString(); //반송량 개수 표시
                txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

                _bindingSource = new BindingSource(viewList, null);
                return _bindingSource;
            }

        }

        private BindingSource GetBindingSource_Count_From_To(DateTime searchDate1, DateTime searchDate2)
        {
            string SELECT_SQL = @"
                            SELECT 
                                 CONVERT(date,JobFinishTime) [JobFinishDate]
                                --,SUBSTRING(CallName,1,CHARINDEX('_',LineName)-1) [LineName]
                                --,SUBSTRING(CallName,1,REVERSE('_',LineName)-1) [PostName]
                                ,CallName
                                ,LineName
                                ,PostName
                                ,ResultCD
                                ,COUNT(*) [JobCount]
                            FROM 
                                JobHistory 
                            WHERE 
                                JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2
                                --AND CHARINDEX('_',LineName) > 0
                            ";

            if (chkboxRobotName.Checked) SELECT_SQL += " AND RobotName LIKE @robotName";
            if (chkboxCallName.Checked) SELECT_SQL += " AND CallName LIKE @callName";
            if (chkboxLineName.Checked) SELECT_SQL += " AND LineName LIKE @LineName";
            if (chkboxPostName.Checked) SELECT_SQL += " AND PostName LIKE @PostName";

            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == false && chkboxResultReset.Checked == false) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11)";
            if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == true && chkboxResultReset.Checked == false) SELECT_SQL += " AND (ResultCD = 2 OR ResultCD = 12)";
            if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == false && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 3 OR ResultCD = 13)";
            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == true && chkboxResultReset.Checked == false) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11 OR ResultCD = 2 OR ResultCD = 12)";
            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == false && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11 OR ResultCD = 3 OR ResultCD = 13)";
            if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == true && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 2 OR ResultCD = 12 OR ResultCD = 3 OR ResultCD = 13)";

            if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == true && chkboxResultReset.Checked == true) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 11 OR ResultCD = 2 OR ResultCD = 12 OR ResultCD = 3 OR ResultCD = 13)";

            //SELECT_SQL += "\nGROUP BY CONVERT(date,JobFinishTime), SUBSTRING(CallName,1,5), SUBSTRING(LineName,1,CHARINDEX('_',LineName)-1), ResultCD";
            SELECT_SQL += "\nGROUP BY CONVERT(date,JobFinishTime),CallName, LineName, PostName, ResultCD";
            SELECT_SQL += "\nORDER BY JobFinishDate";

            var robotName = "%" + cboboxRobotName.Text.Trim() + "%";
            var callName = "%" + cboboxCallName.Text.Trim() + "%";
            var LineName = cboboxLineName.Text.Trim() + "%";
            var PostName = cboboxPostName.Text.Trim() + "%";

            using (var con = new SqlConnection(connectionString))
            {
                var list = con.Query(SELECT_SQL, new { searchDate1, searchDate2, robotName, callName, LineName, PostName });

                //var viewList = list.Select(x => new
                //{
                // x.JobFinishDate [Job 마지막 처리]
                // x.CallName [CallName]
                // x.LineName [설비명]
                // x.PostName [목적지]
                // x.ResultCD [처리결과]
                // x.JobCount [해당 카운터]
                // x.TransportCountValue[반송량]
                //})
                //.ToList()
                //.ToBindingList();

                var viewList = list.Select(x => new
                {
                    x.JobFinishDate,
                    x.CallName,
                    x.LineName,
                    x.PostName,
                    x.ResultCD,
                    x.JobCount,
                    //x.TransportCountValue,
                })
               .ToList()
               .ToBindingList();

                //int TransportCountValue = 0;
                //foreach (var view in viewList)
                //{
                //    if (view.TransportCountValue != null) TransportCountValue = TransportCountValue + view.TransportCountValue;
                //}
                //txt_TransportValue.Text = TransportCountValue.ToString(); //반송량 개수 표시
                txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

                _bindingSource = new BindingSource(viewList, null);
                return _bindingSource;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)            //전체 JobData
        {
            var fromDate = dateTimePicker1.Value.Date;
            var toDate = dateTimePicker2.Value.Date;

            if (toDate > fromDate.AddYears(1))
            {
                MessageBox.Show("한번에 조회할 수 있는 범위는 1년까지 입니다!");
                return;
            }

            btnSearch.Enabled = false;
            DisplayData();
            btnSearch.Enabled = true;
        }

        private void btnSearchCount1_Click(object sender, EventArgs e)      //일정기간 JobCount
        {
            var fromDate = dateTimePicker1.Value;
            var toDate = dateTimePicker2.Value;

            if (toDate > fromDate.AddYears(1))
            {
                MessageBox.Show("한번에 조회할 수 있는 범위는 1년까지 입니다!");
                return;
            }

            btnSearchCount1.Enabled = false;
            DisplayData_Count(fromDate, toDate);
            btnSearchCount1.Enabled = true;
        }

        private void btnSearchCount2_Click(object sender, EventArgs e)      //월별 JobCount
        {
            var today = DateTime.Today;

            var fromDate = today.AddMonths(-1);
            var toDate = today;

            btnSearchCount2.Enabled = false;
            DisplayData_Count(fromDate, toDate);
            btnSearchCount2.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtg_JobHistory.Rows.Count == 0) return;
            main.SaveAsDataGridviewToCSV(dtg_JobHistory);
            main.UserLog("Report Screen", " BackUp Click ");
        }

        private void btnRange1_Click(object sender, EventArgs e)    // yesterday 
        {
            var today = DateTime.Today;
            dateTimePicker1.Value = today.AddDays(-1) + new TimeSpan(6, 0, 0);
            dateTimePicker2.Value = today.AddDays(0) + new TimeSpan(6, 0, 0);
        }

        private void btnRange2_Click(object sender, EventArgs e)    // today 
        {
            var today = DateTime.Today;
            dateTimePicker1.Value = today.AddDays(0) + new TimeSpan(6, 0, 0);
            dateTimePicker2.Value = today.AddDays(1) + new TimeSpan(6, 0, 0);
        }

        private void btnRange3_Click(object sender, EventArgs e)    // week 
        {
            var today = DateTime.Today;
            dateTimePicker1.Value = today.AddDays(-7) + new TimeSpan(6, 0, 0);
            dateTimePicker2.Value = today.AddDays(0) + new TimeSpan(6, 0, 0);
        }

        private void chkbox_CheckedChanged(object sender, EventArgs e)   //OnLoad에서 이벤트 설정 
        {
            #region KGC_Buyeo
            //=============================================== AmkorK5 M3F_T3F =================================================//
            switch (((CheckBox)sender).Name)
            {
                case "chkboxRobotName":
                    cboboxRobotName.Enabled = chkboxRobotName.Checked;
                    if (cboboxRobotName.Enabled == true) ComboBoxRobotNameAdd();
                    else
                    {
                        cboboxRobotName.Text = "";
                        cboboxRobotName.Items.Clear();
                    }
                    break;
                case "chkboxCallName":
                    cboboxCallName.Enabled = chkboxCallName.Checked;
                    if (cboboxCallName.Enabled == true) ComboBoxCallNameAdd();
                    else
                    {
                        cboboxCallName.Text = "";
                        cboboxCallName.Items.Clear();
                    }
                    break;
                    //case "chkboxLineName":
                    //    cboboxLineName.Enabled = chkboxLineName.Checked;
                    //    if (cboboxLineName.Enabled == true) ComboBoxLineCDAdd();
                    //    else
                    //    {
                    //        cboboxLineName.Text = "";
                    //        cboboxLineName.Items.Clear();
                    //    }
                    //    break;
                    //case "chkboxPostName":
                    //    cboboxPostName.Enabled = chkboxPostName.Checked;
                    //    if (cboboxPostName.Enabled == true) ComboBoxPostCDAdd();
                    //    else
                    //    {
                    //        cboboxPostName.Text = "";
                    //        cboboxPostName.Items.Clear();
                    //    }
                    //    break;
            }
            //=================================================================================================================//
            #endregion
        }

        private void ComboBoxLineCDAdd()        //출발지 조회
        {
            //var ZoneNameList = uow.TabletZoneConfig.GetAll().Select(x => x.ZONENAME).ToList();
            ////목적지에 설정된 Zone이 있다면 제외후 ADD한다
            //if (ZoneNameList.Count > 0)
            //{
            //    var PostCDdifferentZoneNameList = ZoneNameList.Where(Name => !string.IsNullOrEmpty(cboboxPostName.Text) && Name != cboboxPostName.Text).ToList();
            //    if (cboboxPostName.Enabled == true && PostCDdifferentZoneNameList.Count > 0)
            //    {
            //        foreach (var PostCDdifferentZoneName in PostCDdifferentZoneNameList)
            //        {
            //            cboboxLineName.Items.Add(PostCDdifferentZoneName);
            //        }
            //    }
            //    else
            //    {
            //        foreach (var ZoneName in ZoneNameList)
            //        {
            //            cboboxLineName.Items.Add(ZoneName);
            //        }
            //    }
            //}
        }

        private void ComboBoxPostCDAdd()        //목적지 조회
        {
            //var ZoneNameList = uow.TabletZoneConfig.GetAll().Select(x => x.ZONENAME).ToList();
            ////목적지에 설정된 Zone이 있다면 제외후 ADD한다
            //if (ZoneNameList.Count > 0)
            //{
            //    var LineCDdifferentZoneNameList = ZoneNameList.Where(Name => !string.IsNullOrEmpty(cboboxLineName.Text) && Name != cboboxLineName.Text).ToList();
            //    if (cboboxLineName.Enabled == true && LineCDdifferentZoneNameList.Count > 0)
            //    {

            //        foreach (var LineCDdifferentZoneName in LineCDdifferentZoneNameList)
            //        {
            //            cboboxPostName.Items.Add(LineCDdifferentZoneName);
            //        }
            //    }
            //    else
            //    {
            //        foreach (var ZoneName in ZoneNameList)
            //        {
            //            cboboxPostName.Items.Add(ZoneName);
            //        }
            //    }
            //}
        }

        private void ComboBoxCallNameAdd()       //CallName 조회
        {
            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            //CallName조회후 일치되는 값이 있으면 지운후 1개의 값만 ADD한다
            var JobCallNames = uow.JobConfigs.GetAll().Select(x => x.CallName).Distinct().ToList();
            foreach (var jobCallName in JobCallNames)
            {
                cboboxCallName.Items.Add(jobCallName);
            }

            //foreach (var jobConfig in uow.JobConfigs.GetAll()) //전체대한 CallName조회
            //{
            //    cboboxCallName.Items.Add(jobConfig.CallName);
            //}
            //=================================================================================================================//
            #endregion
        }

        private void ComboBoxRobotNameAdd()     //RobotName  조회
        {
            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            foreach (var robotnames in uow.Robots.GetAll())
            {
                cboboxRobotName.Items.Add(robotnames.RobotName);
            }
            //=================================================================================================================//
            #endregion
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            dtg_JobHistory.Rows.Clear();
            cboboxRobotName.Items.Clear();
            cboboxRobotName.Enabled = false;
            chkboxRobotName.Checked = false;
            cboboxCallName.Items.Clear();
            cboboxCallName.Enabled = false;
            chkboxCallName.Checked = false;
            txtSearchCount.Text = "";
            txt_TransportValue.Text = "";
        }
    }


}

