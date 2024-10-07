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
    public partial class ElevatorModeHistoryScreen : Form
    {
        private static readonly string connectionString = ConnectionStrings.DB3;

        private readonly MainForm mainForm;
        private readonly IUnitOfWork uow;
        private BindingSource _bindingSource = null;


        public ElevatorModeHistoryScreen(MainForm mainForm, IUnitOfWork uow)
        {
            InitializeComponent();
            this.mainForm = mainForm;
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
            dtg_ElevatorModeHistory.DoubleBuffered(true);
            // 날짜픽커 포맷 설정
            dateTimePicker1.CustomFormat = "yyyy-MM-dd (dddd) HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd (dddd) HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;

            dateTimePicker1.Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 06:00:00"));
            dateTimePicker2.Value = Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 06:00:00"));

            #region 현대트랜시스
            //=============================================== 현대트랜시스 =================================================//
            // 체크박스 이벤트 설정
            //chkboxRobotName.CheckedChanged += (s, e2) => cboboxRobotName.Enabled = chkboxRobotName.Checked;
            //chkboxCallName.CheckedChanged += (s, e2) => cboboxCallName.Enabled = chkboxCallName.Checked;
            //chkboxLineName.CheckedChanged += (s, e2) => txtLineName.Enabled = chkboxLineName.Checked;
            //chkboxPartCode.CheckedChanged += (s, e2) => txtPartCode.Enabled = chkboxPartCode.Checked;
            //=================================================================================================================//
            #endregion

            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            // 체크박스 이벤트 설정
            chkboxUserNumber.CheckedChanged += chkbox_CheckedChanged;
            btnClear.Click += btnClear_Click;
            btnSearchCount2.Visible = false;
            //=================================================================================================================//
            #endregion
        }


        public void Init()
        {
            //
        }

        private void InitGrid()
        {
            // 데이터그리드뷰 설정
            dtg_ElevatorModeHistory.RowHeadersVisible = false;
            dtg_ElevatorModeHistory.AllowUserToResizeRows = false;
            dtg_ElevatorModeHistory.AllowUserToResizeColumns = true;
            dtg_ElevatorModeHistory.AllowUserToAddRows = false;
            dtg_ElevatorModeHistory.AllowUserToDeleteRows = false;
            dtg_ElevatorModeHistory.EditMode = DataGridViewEditMode.EditProgrammatically;
            dtg_ElevatorModeHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtg_ElevatorModeHistory.MultiSelect = true;
            //dataGridView1.CellDoubleClick += (s, e) => { btnEdit_Click(this, null); };

            // 데이터그리드뷰 헤더/셀 스타일 설정
            dtg_ElevatorModeHistory.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            {
                Padding = new Padding(5, 3, 5, 3),
                //Alignment = DataGridViewContentAlignment.MiddleCenter,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                //WrapMode = DataGridViewTriState.True,
            };
            dtg_ElevatorModeHistory.DefaultCellStyle = dtg_ElevatorModeHistory.ColumnHeadersDefaultCellStyle;

            var rowTemplate = dtg_ElevatorModeHistory.RowTemplate;
            rowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            rowTemplate.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;// Bisque;
            rowTemplate.Height = 30; // 35;

            // ==========================

            // row number 컬럼 추가
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.Name = "No";
            textboxColumn.HeaderText = "No";
            dtg_ElevatorModeHistory.Columns.Insert(0, textboxColumn);

            // row number 컬럼 데이터 처리
            dtg_ElevatorModeHistory.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) //Row Index에 표시되는 문자 서식을 지정하기 위함 
        {
            #region 현대트랜시스
            //=============================================== 현대트랜시스 ====================================================//
            //var dgv = (DataGridView)sender;

            //if (e.RowIndex != dgv.NewRowIndex)
            //{
            //    string colName = dgv.Columns[e.ColumnIndex].Name;

            //    if (colName == "No")
            //    {
            //        e.Value = (e.RowIndex + 1).ToString();
            //    }
            //    else if (colName == "ResultCD")
            //    {
            //        if (e.Value != null)
            //        {
            //            if (e.Value.ToString() == "1") e.Value = "성공";
            //            else if (e.Value.ToString() == "2") e.Value = "실패";
            //            else if (e.Value.ToString() == "3") e.Value = "취소";
            //        }
            //        //else if (colName == "CallType")
            //        //{
            //        //    if (e.Value != null)
            //        //    {
            //        //        if (e.Value.ToString() == "N") e.Value = "공급";
            //        //        else if (e.Value.ToString() == "Y") e.Value = "회수";
            //        //    }
            //        //}
            //        else if (colName.EndsWith("Date") || colName.EndsWith("날짜"))
            //        {
            //            try
            //            {
            //                DateTime dateTime = (DateTime)e.Value;
            //                e.Value = dateTime.ToString("yyyy-MM-dd");
            //            }
            //            catch (Exception ex)
            //            {
            //                e.Value = e.Value.ToString();
            //                Console.WriteLine(ex);
            //            }
            //        }
            //        else if (colName.EndsWith("Time") || colName.EndsWith("시간"))
            //        {
            //            try
            //            {
            //                DateTime dateTime = (DateTime)e.Value;
            //                e.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //            }
            //            catch (Exception ex)
            //            {
            //                e.Value = e.Value.ToString();
            //                Console.WriteLine(ex);
            //            }
            //        }
            //    }

            //}
            //=================================================================================================================//
            #endregion

            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
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
                        if (e.Value.ToString() == "1") e.Value = "성공";
                        else if (e.Value.ToString() == "2") e.Value = "실패";
                        else if (e.Value.ToString() == "3") e.Value = "취소";
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
                            Console.WriteLine(ex);
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
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            //=================================================================================================================//
            #endregion
        }

        private void DisplayData()
        {


            #region 현대트랜시스
            //=============================================== 현대트랜시스 ====================================================//
            //// 데이터소스 바인딩
            //dataGridView1.DataSource = null; // 초간단 Refresh
            //dataGridView1.DataSource = GetBindingSource_AnynymousType();

            ////// id 컬럼 숨김
            ////dataGridView1.Columns["Id"].Visible = false;
            ////dataGridView1.Columns["PosName"].Visible = false;

            //// 전체 컬럼폭 조정
            ////dataGridView1.AutoResizeColumns(); // 데이터 많을 경우 느리다

            // 개별 컬럼폭 설정
            //                 0   CallName [CallName]
            //                 1  ,CallType [타입]
            //                 2  ,LineName [설비명]
            //                 3  ,PartCD [품번]
            //                 4  ,PartNM [품명]
            //                 5  ,PartOutQ [수량]
            //                 6  ,ResultCD [처리결과]
            //                 7  ,RobotName [처리로봇]
            //                 8  ,JobState [Job상태]
            //                 9  ,CallTime [Call시간]
            //                10  ,JobCreateTime [Job생성시간]
            //                11  ,JobFinishTime [Job완료시간]
            //                12  ,MissionNames [미션네임]
            //                13  ,MissionStates [미션상태]
            //int n = 0;
            //dataGridView1.Columns[n++].Width = 50;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 80;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 80;
            //dataGridView1.Columns[n++].Width = 80;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 150;
            //dataGridView1.Columns[n++].Width = 500;
            //dataGridView1.Columns[n++].Width = 200;
            //=================================================================================================================//
            #endregion

            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            // 데이터소스 바인딩
            dtg_ElevatorModeHistory.DataSource = null; // 초간단 Refresh
            dtg_ElevatorModeHistory.DataSource = GetBindingSource_AnynymousType();

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

            int n = 0;
            dtg_ElevatorModeHistory.Columns[n++].Width = 50;
            dtg_ElevatorModeHistory.Columns[n++].Width = 150;
            dtg_ElevatorModeHistory.Columns[n++].Width = 350;
            dtg_ElevatorModeHistory.Columns[n++].Width = 200;
            //=================================================================================================================//
            #endregion
        }
        private void DisplayData_Count(DateTime dt1, DateTime dt2)
        {
            // 데이터소스 바인딩
            dtg_ElevatorModeHistory.DataSource = null; // 초간단 Refresh
            dtg_ElevatorModeHistory.DataSource = GetBindingSource_Count_From_To(dt1, dt2);

            // 전체 컬럼폭 조정
            dtg_ElevatorModeHistory.AutoResizeColumns(); // 데이터 많을 경우 느리다
        }

        private BindingSource GetBindingSource()
        {
            #region 현대트랜시스
            //const string SELECT_SQL = @"
            //               SELECT *
            //                FROM JobHistory 
            //                WHERE JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
            //                ORDER BY JobFinishTime";

            //var searchDate1 = DateTime.Today.AddDays(-30);
            //var searchDate2 = DateTime.Today.AddDays(1);

            //using (var con = new SqlConnection(connectionString))
            //{
            //    //var list = await con.QueryAsync<JobLog>(SELECT_SQL, new { searchDate1, searchDate2 });
            //    //var bindingList = new BindingList<JobLog>((IList<JobLog>)list);
            //    //_bindingSource = new BindingSource(bindingList, null);
            //    //return _bindingSource;

            //    IEnumerable<dynamic> list = con.Query(SELECT_SQL);
            //    var bindingList = list.ToList().ToBindingList();
            //    _bindingSource = new BindingSource(bindingList, null);
            //    return _bindingSource;
            //}
            #endregion

            #region AmkorK5 M3F_T3F
            const string SELECT_SQL = @"
                           SELECT *
                            FROM UserChangeModeLog
                            WHERE CreateTime >= @searchDate1 AND CreatehTime < @searchDate2)
                            ORDER BY CreateTime";

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
            #endregion
        }

        private BindingSource GetBindingSource_AnynymousType()
        {
            #region 현대트랜시스
            //=============================================== 현대트랜시스 ====================================================//
            //string SELECT_SQL = @"
            //                SELECT *
            //                FROM JobHistory 
            //                WHERE JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2";

            //if (chkboxRobotName.Checked) SELECT_SQL += " AND RobotName LIKE @robotName";
            //if (chkboxCallName.Checked) SELECT_SQL += " AND CallName LIKE @callName";
            //if (chkboxLineName.Checked) SELECT_SQL += " AND LineName LIKE @lineName";
            //if (chkboxPartCode.Checked) SELECT_SQL += " AND PartCD LIKE @partCode";

            //if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == false) SELECT_SQL += " AND ResultCD = 1";
            //if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == true) SELECT_SQL += " AND ResultCD = 2";
            //if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == true) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 2)";

            //SELECT_SQL += " ORDER BY JobFinishTime";

            //// 자정기준이므로 +1 day 해줘야 제대로 검색된다
            //// ex. 10/28~10/28 검색: 2022.10.28 00:00:00 ~ 2022.10.29 00:00:00
            //// ex. 10/28~10/29 검색: 2022.10.28 00:00:00 ~ 2022.10.30 00:00:00
            //var searchDate1 = dateTimePicker1.Value;
            //var searchDate2 = dateTimePicker2.Value;
            ////var searchDate1 = dateTimePicker1.Value.Date;
            ////var searchDate2 = dateTimePicker2.Value.Date.AddDays(1);

            //var robotName = "%" + cboboxRobotName.Text.Trim() + "%";
            //var callName = "%" + cboboxCallName.Text.Trim() + "%";
            //var lineName = "%" + txtLineName.Text.Trim() + "%";
            //var partCode = "%" + txtPartCode.Text.Trim() + "%";

            //using (var con = new SqlConnection(connectionString))
            //{
            //object queryParams = new { searchDate1, searchDate2, robotName, callName, lineName, partCode };
            //var list = con.Query(SELECT_SQL, queryParams);

            //    //var viewList = list.Select(x => new
            //    //{
            //         CallName [CallName]
            //         CallType [타입]
            //         LineName [설비명]
            //         PartCD [품번]
            //         PartNM [품명]
            //         PartOutQ [수량]
            //         ResultCD [처리결과]
            //         RobotName [처리로봇]
            //         JobState [Job상태]
            //         CallTime [Call시간]
            //         JobCreateTime [Job생성시간]
            //         JobFinishTime [Job완료시간]
            //         MissionNames [미션네임]
            //         MissionStates [미션상태]
            //    //.ToList()
            //    //.ToBindingList();

            //    var viewList = list.Select(x => new
            //    {
            //        x.CallName, 
            //        x.CallType,
            //        x.LineName, 
            //        x.PartCD, 
            //        x.PartNM, 
            //        x.PartOutQ, 
            //        x.ResultCD, 
            //        x.RobotName, 
            //        x.JobState, 
            //        x.CallTime, 
            //        x.JobCreateTime,
            //        x.JobFinishTime,
            //        x.MissionName,
            //        x.MissionStates,
            //    })
            //     .ToList()
            //     .ToBindingList();

            //    txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

            //    _bindingSource = new BindingSource(viewList, null);
            //    return _bindingSource;
            //=================================================================================================================//

            #endregion

            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//

            string SELECT_SQL = @"
                            SELECT *
                            FROM UserChangeModeLog 
                            WHERE CreateTime >= @searchDate1 AND CreateTime < @searchDate2";

            if (chkboxUserNumber.Checked) SELECT_SQL += " AND UserNumber LIKE @UserNumber";

            SELECT_SQL += " ORDER BY CreateTime";

            // 자정기준이므로 +1 day 해줘야 제대로 검색된다
            // ex. 10/28~10/28 검색: 2022.10.28 00:00:00 ~ 2022.10.29 00:00:00
            // ex. 10/28~10/29 검색: 2022.10.28 00:00:00 ~ 2022.10.30 00:00:00
            var searchDate1 = dateTimePicker1.Value;
            var searchDate2 = dateTimePicker2.Value;
            //var searchDate1 = dateTimePicker1.Value.Date;
            //var searchDate2 = dateTimePicker2.Value.Date.AddDays(1);

            var UserNumber = "%" + cboboxUserNumber.Text.Trim() + "%";

            using (var con = new SqlConnection(connectionString))
            {


                object queryParams = new { searchDate1, searchDate2, UserNumber };
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
                //.ToList()
                //.ToBindingList();

                var viewList = list.Select(x => new
                {
                    x.UserNumber,
                    x.ChangeModeLog,
                    x.CreateTime
                })
                 .ToList()
                 .ToBindingList();

                txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

                _bindingSource = new BindingSource(viewList, null);
                return _bindingSource;

                //=================================================================================================================//
                #endregion

                //if (chkboxRobotName.Checked)
                //    queryParams = new { searchDate1, searchDate2, robotName };
                //else
                //    queryParams = new { searchDate1, searchDate2 };


            }

        }

        private BindingSource GetBindingSource_Count_From_To(DateTime searchDate1, DateTime searchDate2)
        {

            #region 현대트랜시스

            //=============================================== 현대트랜시스 =================================================//
            //string SELECT_SQL = @"
            //                SELECT 
            //                     CONVERT(date,JobFinishTime) [JobFinishDate]
            //                    ,SUBSTRING(CallName,1,5) [LineCD]
            //                    --,SUBSTRING(LineName,1,CHARINDEX('_',LineName)-1) [LineName]
            //                    ,LineName
            //                    ,ResultCD
            //                    ,COUNT(*) [JobCount]
            //                FROM 
            //                    JobHistory 
            //                WHERE 
            //                    JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2
            //                    --AND CHARINDEX('_',LineName) > 0
            //                ";

            //if (chkboxRobotName.Checked) SELECT_SQL += " AND RobotName LIKE @robotName";
            //if (chkboxCallName.Checked) SELECT_SQL += " AND CallName LIKE @callName";
            //if (chkboxLineName.Checked) SELECT_SQL += " AND LineName LIKE @lineName";
            //if (chkboxPartCode.Checked) SELECT_SQL += " AND PartCD LIKE @partCode";

            //if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == false) SELECT_SQL += " AND ResultCD = 1";
            //if (chkboxResultOK.Checked == false && chkboxResultNG.Checked == true) SELECT_SQL += " AND ResultCD = 2";
            //if (chkboxResultOK.Checked == true && chkboxResultNG.Checked == true) SELECT_SQL += " AND (ResultCD = 1 OR ResultCD = 2)";

            //SELECT_SQL += "\nGROUP BY CONVERT(date,JobFinishTime), SUBSTRING(CallName,1,5), SUBSTRING(LineName,1,CHARINDEX('_',LineName)-1), ResultCD";
            //SELECT_SQL += "\nGROUP BY CONVERT(date,JobFinishTime), SUBSTRING(CallName,1,5), LineName, ResultCD";
            //SELECT_SQL += "\nORDER BY JobFinishDate";

            //var robotName = "%" + cboboxRobotName.Text.Trim() + "%";
            //var callName = "%" + cboboxCallName.Text.Trim() + "%";
            //var lineName = "%" + txtLineName.Text.Trim() + "%";
            //var partCode = "%" + txtPartCode.Text.Trim() + "%";

            //using (var con = new SqlConnection(connectionString))

            //var list = con.Query(SELECT_SQL, new { searchDate1, searchDate2, robotName, callName, lineName, partCode});

            //var viewList = list.Select(x => new
            //{
            //    x.JobFinishDate,
            //    x.LineCD,
            //    x.LineName,
            //    x.ResultCD,
            //    x.JobCount,
            //})
            //.ToList()
            //.ToBindingList();
            //txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

            //_bindingSource = new BindingSource(viewList, null);
            //return _bindingSource;


            //=================================================================================================================//

            #endregion

            #region Amkor K5 M3F_T3F

            //=============================================== AmkorK5 M3F_T3F =================================================//

            string SELECT_SQL = @"
                            SELECT 
                                 CONVERT(date,CreateTime) [CreateDate]
                                ,UserNumber
                                ,ChangeModeLog
                                ,COUNT(*) [ChangeCount]
                            FROM 
                                UserChangeModeLog 
                            WHERE 
                                CreateTime >= @searchDate1 AND CreateTime < @searchDate2
                            ";

            if (chkboxUserNumber.Checked) SELECT_SQL += " AND UserNumber LIKE @userNumber";

            //SELECT_SQL += "\nGROUP BY CONVERT(date,JobFinishTime), SUBSTRING(CallName,1,5), SUBSTRING(LineName,1,CHARINDEX('_',LineName)-1), ResultCD";
            SELECT_SQL += "\nGROUP BY CONVERT(date,CreateTime),UserNumber, ChangeModeLog";
            SELECT_SQL += "\nORDER BY CreateDate";

            var userNumber = "%" + cboboxUserNumber.Text.Trim() + "%";

            using (var con = new SqlConnection(connectionString))
            {
                var list = con.Query(SELECT_SQL, new { searchDate1, searchDate2, userNumber });

                //var viewList = list.Select(x => new
                //{
                // x.JobFinishDate [Job 마지막 처리]
                // x.CallName [CallName]
                // x.LineName [설비명]
                // x.PostName [목적지]
                // x.ResultCD [처리결과]
                // x.JobCount [해당 카운터]
                //})
                //.ToList()
                //.ToBindingList();

                var viewList = list.Select(x => new
                {
                    x.CreateDate,
                    x.UserNumber,
                    x.ChangeModeLog,
                    x.ChangeCount,
                })
               .ToList()
               .ToBindingList();
                txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

                _bindingSource = new BindingSource(viewList, null);
                return _bindingSource;

                //=================================================================================================================//

                #endregion

                //const string SELECT_SQL = @"
                //                SELECT 
                //                     CONVERT(date,JobFinishTime) [JobFinishDate]
                //                    ,SUBSTRING(CallName,1,5) [LineCD]
                //                    ,ResultCD
                //                    ,COUNT(*) [JobCount]
                //                FROM 
                //                    JobHistory 
                //                WHERE 
                //                    JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2
                //                    --AND CallName LIKE 'MC%' 
                //                    --AND ResultCD IS NOT NULL 
                //                    --AND ResultCD = 1
                //                GROUP BY 
                //                    CONVERT(date,JobFinishTime), SUBSTRING(CallName,1,5), ResultCD
                //                ORDER BY 
                //                    JobFinishDate";
                //const string SELECT_SQL = @"
                //                SELECT 
                //                     CONVERT(date,JobFinishTime) [JobFinishDate]
                //                    ,SUBSTRING(CallName,1,5) [LineCD]
                //                    ,SUBSTRING(LineName,1,CHARINDEX('_',LineName)-1) [LineName]
                //                    ,ResultCD
                //                    ,COUNT(*) [JobCount]
                //                FROM 
                //                    JobHistory 
                //                WHERE 
                //                    JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2
                //                    AND CHARINDEX('_',LineName) > 0
                //                    AND CallName LIKE 'MC%' 
                //                    --AND ResultCD IS NOT NULL 
                //                GROUP BY 
                //                    CONVERT(date,JobFinishTime), SUBSTRING(CallName,1,5), SUBSTRING(LineName,1,CHARINDEX('_',LineName)-1), ResultCD
                //                ORDER BY 
                //                    JobFinishDate";


                //=============================================================================================== save to file
                //var lineCD = new string[] { "MC110", "MC111", "MC112", "MC310", "MC311", "MC350", "MC351" };
                //var lineNM = new string[] { "BDM", "2nd", "NQ5A", "MANUAL1", "MANUAL2", "POWER1", "POWER2" };

                //var itemGroupsByDate = viewList.GroupBy(x => x.JobFinishDate);
                //foreach (var itemGroup in itemGroupsByDate)
                //{
                //    var dt = itemGroup.Key;
                //    var items = itemGroup;

                //    {
                //        var sb = new StringBuilder();
                //        sb.Append(dt.ToString("yyyy-MM-dd"));

                //        for (int i = 0; i < lineNM.Length; i++)
                //        {
                //            string searchLineCD = lineCD[i];
                //            string searchLineNM = lineNM[i];
                //            int count1 = items.Where(x => x.LineCD == searchLineCD).FirstOrDefault(x => x.ResultCD == 1)?.JobCount ?? 0; // 성공카운트
                //            int count2 = items.Where(x => x.LineCD == searchLineCD).FirstOrDefault(x => x.ResultCD == 2)?.JobCount ?? 0; // 실패카운트
                //            sb.Append($",{searchLineCD},{searchLineNM},{count1},{count2}");
                //        }
                //        sb.AppendLine();
                //        File.AppendAllText(@"/log/JobDailyCount.txt", sb.ToString());
                //    }
                //}
                //===============================================================================================

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            #region 현대트랜시스
            //=============================================== 현대트랜시스 =================================================//
            //var fromDate = dateTimePicker1.Value.Date;
            //var toDate = dateTimePicker2.Value.Date;

            //if (toDate > fromDate.AddMonths(1))
            //{
            //    MessageBox.Show("한번에 조회할 수 있는 범위는 1개월까지 입니다!");
            //    return;
            //}

            //btnSearch.Enabled = false;
            //DisplayData();
            //btnSearch.Enabled = true;
            //=================================================================================================================//

            #endregion

            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
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
            //=================================================================================================================//
            #endregion

        }

        private void btnSearchCount1_Click(object sender, EventArgs e)
        {

            #region 현대트랜시스
            //=============================================== 현대트랜시스 =================================================//
            //var fromDate = dateTimePicker1.Value;
            //var toDate = dateTimePicker2.Value;

            //if (toDate > fromDate.AddMonths(1))
            //{
            //    MessageBox.Show("한번에 조회할 수 있는 범위는 1개월까지 입니다!");
            //    return;
            //}

            //btnSearchCount1.Enabled = false;
            //DisplayData_Count(fromDate, toDate);
            //btnSearchCount1.Enabled = true;
            //=================================================================================================================//
            #endregion

            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
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
            //=================================================================================================================//
            #endregion

            //var today = DateTime.Today;

            //var fromDate = today.AddDays(-1);
            //var toDate = today;

            //btnSearchCount1.Enabled = false;
            //DisplayData_Count(fromDate, toDate);
            //btnSearchCount1.Enabled = true;


        }

        private void btnSearchCount2_Click(object sender, EventArgs e)
        {
            #region 현대트랜시스
            //=============================================== 현대트랜시스 =================================================//
            //var today = DateTime.Today;

            //var fromDate = today.AddMonths(-1);
            //var toDate = today;

            //btnSearchCount2.Enabled = false;
            //DisplayData_Count(fromDate, toDate);
            //btnSearchCount2.Enabled = true;
            //=================================================================================================================//
            #endregion

            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            var today = DateTime.Today;

            var fromDate = today.AddMonths(-1);
            var toDate = today;

            btnSearchCount2.Enabled = false;
            DisplayData_Count(fromDate, toDate);
            btnSearchCount2.Enabled = true;
            //=================================================================================================================//
            #endregion

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtg_ElevatorModeHistory.Rows.Count == 0) return;
            mainForm.SaveAsDataGridviewToCSV(dtg_ElevatorModeHistory);
            mainForm.UserLog("Report Screen"," BackUp Click ");
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
            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            switch (((CheckBox)sender).Name)
            {
                case "chkboxUserNumber":
                    cboboxUserNumber.Enabled = chkboxUserNumber.Checked;
                    if (cboboxUserNumber.Enabled == true) ComboBoxUserNumberAdd();
                    else
                    {
                        cboboxUserNumber.Text = "";
                        cboboxUserNumber.Items.Clear();
                    }
                    break;
                    break;
            }
            //=================================================================================================================//
            #endregion
        }

        private void ComboBoxUserNumberAdd()  //RobotName  조회
        {
            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            foreach (var UserNumber in uow.UserNumber.GetAll())
            {
                cboboxUserNumber.Items.Add(UserNumber.UserNumber);
            }
            //=================================================================================================================//
            #endregion
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            dtg_ElevatorModeHistory.Rows.Clear();
            cboboxUserNumber.Items.Clear();
            cboboxUserNumber.Enabled = false;
            chkboxUserNumber.Checked = false;
           // cboboxCallName.Items.Clear();
            //cboboxCallName.Enabled = false;
            //chkboxCallName.Checked = false;
            txtSearchCount.Text = "";
            //=================================================================================================================//
            #endregion
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {

        }
    }


}

