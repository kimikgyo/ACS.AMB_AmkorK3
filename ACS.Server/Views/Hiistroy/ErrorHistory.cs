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
    public partial class ErrorHistoryScreen : Form
    {
        private static readonly string connectionString = ConnectionStrings.DB1;

        private readonly MainForm main;
        private readonly IUnitOfWork uow;
        private BindingSource _bindingSource = null;
        private readonly Font textFont1 = new Font("맑은 고딕", 12, FontStyle.Bold);


        public ErrorHistoryScreen(MainForm main, IUnitOfWork uow)
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
            dtg_ErrorHistory.DoubleBuffered(true);
            // 날짜픽커 포맷 설정
            dateTimePicker1.CustomFormat = "yyyy-MM-dd (dddd) HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd (dddd) HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;

            dateTimePicker1.Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 06:00:00"));
            dateTimePicker2.Value = Convert.ToDateTime(DateTime.Now.AddDays(0).ToString("yyyy-MM-dd 06:00:00"));

            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            // 체크박스 이벤트 설정
            chkboxRobotName.CheckedChanged += chkbox_CheckedChanged;
            btnClear.Click += btnClear_Click;
            btnSearchCount2.Visible = false;
            //=================================================================================================================//
            #endregion

            this.BackColor = main.skinColor;

            btnSearch.BackColor = main.skinColor;
            btnSearch.ForeColor = Color.White;
            btnSearchCount1.BackColor = main.skinColor;
            btnSearchCount1.ForeColor = Color.White;
            btnSave.BackColor = main.skinColor;
            btnSave.ForeColor = Color.White;
            btnSearchCount2.BackColor = main.skinColor;
            btnSearchCount2.ForeColor = Color.White;
            btnClear.BackColor = main.skinColor;
            btnClear.ForeColor = Color.White;

            groupBox2.ForeColor = Color.White;
            chkboxRobotName.ForeColor = Color.White;
            label1.ForeColor = Color.White;
            txtSearchCount.BackColor = main.skinColor;
            txtSearchCount.ForeColor = Color.White;
            txtSearchCount.ReadOnly = true;
            txtSearchCount.BorderStyle = BorderStyle.None;

            button1.BackColor = main.skinColor;
            button1.ForeColor = Color.White;
            button2.BackColor = main.skinColor;
            button2.ForeColor = Color.White;
            button3.BackColor = main.skinColor;
            button3.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            groupBox1.ForeColor = Color.White;

            dtg_ErrorHistory.ScrollBars = ScrollBars.Both;
            dtg_ErrorHistory.AllowUserToResizeColumns = true;
            dtg_ErrorHistory.ColumnHeadersHeight = 70;
            dtg_ErrorHistory.RowTemplate.Height = 82;
            dtg_ErrorHistory.ReadOnly = false;
            dtg_ErrorHistory.BackgroundColor = main.skinColor;
            dtg_ErrorHistory.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dtg_ErrorHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtg_ErrorHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg_ErrorHistory.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dtg_ErrorHistory.ColumnHeadersDefaultCellStyle.Font = textFont1;
            dtg_ErrorHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 89, 96);
            dtg_ErrorHistory.EnableHeadersVisualStyles = false;
            dtg_ErrorHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }


        public void Init()
        {
            //
        }

        private void InitGrid()
        {
            // 데이터그리드뷰 설정
            dtg_ErrorHistory.RowHeadersVisible = false;
            dtg_ErrorHistory.AllowUserToResizeRows = false;
            dtg_ErrorHistory.AllowUserToResizeColumns = true;
            dtg_ErrorHistory.AllowUserToAddRows = false;
            dtg_ErrorHistory.AllowUserToDeleteRows = false;
            dtg_ErrorHistory.EditMode = DataGridViewEditMode.EditProgrammatically;
            dtg_ErrorHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtg_ErrorHistory.MultiSelect = true;
            //dataGridView1.CellDoubleClick += (s, e) => { btnEdit_Click(this, null); };

            // 데이터그리드뷰 헤더/셀 스타일 설정
            //dtg_ErrorHistory.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            //{
            //    Padding = new Padding(5, 3, 5, 3),
            //    //Alignment = DataGridViewContentAlignment.MiddleCenter,
            //    Alignment = DataGridViewContentAlignment.MiddleLeft,
            //    //WrapMode = DataGridViewTriState.True,
            //};
            //dtg_ErrorHistory.DefaultCellStyle = dtg_ErrorHistory.ColumnHeadersDefaultCellStyle;

            //var rowTemplate = dtg_ErrorHistory.RowTemplate;
            //rowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            //rowTemplate.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;// Bisque;
            //rowTemplate.Height = 30; // 35;

            // ==========================

            // row number 컬럼 추가
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.Name = "No";
            textboxColumn.HeaderText = "No";
            dtg_ErrorHistory.Columns.Insert(0, textboxColumn);

            // row number 컬럼 데이터 처리
            dtg_ErrorHistory.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) //Row Index에 표시되는 문자 서식을 지정하기 위함 
        {

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
            //=================================================================================================================//
            #endregion
        }

        private void DisplayData()
        {
            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            // 데이터소스 바인딩
            dtg_ErrorHistory.DataSource = null; // 초간단 Refresh
            dtg_ErrorHistory.DataSource = GetBindingSource_AnynymousType();

            //// id 컬럼 숨김
            //dataGridView1.Columns["Id"].Visible = false;
            //dataGridView1.Columns["PosName"].Visible = false;

            // 전체 컬럼폭 조정
            //dataGridView1.AutoResizeColumns(); // 데이터 많을 경우 느리다

            // 개별 컬럼폭 설정
            //                 0   ErrorCreateTime [Error 발생시간]
            //                 1  ,RobotName [Robot이름]
            //                 2  ,ErrorCode 
            //                 3  ,ErrorDescription 
            //                 4  ,ErrorModule 

            int n = 0;
            dtg_ErrorHistory.Columns[n++].Width = 150;
            dtg_ErrorHistory.Columns[n++].Width = 150;
            dtg_ErrorHistory.Columns[n++].Width = 150;
            dtg_ErrorHistory.Columns[n++].Width = 150;
            dtg_ErrorHistory.Columns[n++].Width = 150;
            dtg_ErrorHistory.Columns[n++].Width = 150;
            //=================================================================================================================//
            #endregion
        }
        private void DisplayData_Count(DateTime dt1, DateTime dt2)
        {
            // 데이터소스 바인딩
            dtg_ErrorHistory.DataSource = null; // 초간단 Refresh
            dtg_ErrorHistory.DataSource = GetBindingSource_Count_From_To(dt1, dt2);

            // 전체 컬럼폭 조정
            dtg_ErrorHistory.AutoResizeColumns(); // 데이터 많을 경우 느리다
        }

        private BindingSource GetBindingSource()
        {

            #region AmkorK5 M3F_T3F
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
            #endregion
        }

        private BindingSource GetBindingSource_AnynymousType()
        {

            #region AmkorK5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//

            string SELECT_SQL = @"
                            SELECT *
                            FROM ErrorHistory 
                            WHERE ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2";

            if (chkboxRobotName.Checked) SELECT_SQL += " AND RobotName LIKE @robotName";


            SELECT_SQL += " ORDER BY ErrorCreateTime";

            // 자정기준이므로 +1 day 해줘야 제대로 검색된다
            // ex. 10/28~10/28 검색: 2022.10.28 00:00:00 ~ 2022.10.29 00:00:00
            // ex. 10/28~10/29 검색: 2022.10.28 00:00:00 ~ 2022.10.30 00:00:00
            var searchDate1 = dateTimePicker1.Value;
            var searchDate2 = dateTimePicker2.Value;
            //var searchDate1 = dateTimePicker1.Value.Date;
            //var searchDate2 = dateTimePicker2.Value.Date.AddDays(1);

            var robotName = "%" + cboboxRobotName.Text.Trim() + "%";
            using (var con = new SqlConnection(connectionString))
            {


                object queryParams = new { searchDate1, searchDate2, robotName };
                var list = con.Query(SELECT_SQL, queryParams);

             
                // ErrorCreateTime [Error 발생시간]
                // RobotName [로봇이름]
                // ErrorCode [에러코드]
                // ErrorDescription [에러메세지?]
                // ErrorModule [에러메세지?]
               

                var viewList = list.Select(x => new
                {
                    x.ErrorCreateTime,
                    x.RobotName,
                    x.ErrorCode,
                    x.ErrorDescription,
                    x.ErrorModule,
                    x.ErrorMessage,
                    x.ErrorType,
                    x.Explanation,
                    x.POSMessage
                })
                 .ToList()
                 .ToBindingList();

                txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

                _bindingSource = new BindingSource(viewList, null);
                return _bindingSource;

            }
            #endregion
        }

        //같은 값을 카운터하여 보여준다
        private BindingSource GetBindingSource_Count_From_To(DateTime searchDate1, DateTime searchDate2)
        {

            #region Amkor K5 M3F_T3F

            //=============================================== AmkorK5 M3F_T3F =================================================//

            string SELECT_SQL = @"
                            SELECT 
                                 CONVERT(date,ErrorCreateTime) [ErrorHistoryDate]
                                ,ErrorCode
                                ,ErrorDescription
                                ,ErrorModule
                                ,ErrorMessage
                                ,ErrorType
                                ,Explanation
                                ,POSMessage
                                ,COUNT(*) [ErrorCount]
                            FROM 
                                ErrorHistory 
                            WHERE 
                                ErrorCreateTime >= @searchDate1 AND ErrorCreateTime < @searchDate2
                                --AND ErrorCode > 0
                            ";

            if (chkboxRobotName.Checked) SELECT_SQL += " AND RobotName LIKE @robotName";


            SELECT_SQL += "\nGROUP BY CONVERT(date,ErrorCreateTime),ErrorCode,ErrorDescription,ErrorModule,ErrorMessage,ErrorType,Explanation,POSMessage";
            SELECT_SQL += "\nORDER BY ErrorHistoryDate";

            var robotName = "%" + cboboxRobotName.Text.Trim() + "%";
            using (var con = new SqlConnection(connectionString))
            {
                var list = con.Query(SELECT_SQL, new { searchDate1, searchDate2, robotName});

                // ErrorHistoryDate [그룹시간]
                // ErrorCode [에러코드]
                // ErrorDescription [에러메세지?]
                // ErrorModule [에러메세지?]
                // ErrorCount [같은 에러 메세지]

                var viewList = list.Select(x => new
                {
                    x.ErrorHistoryDate,
                    x.ErrorCode,
                    x.ErrorDescription,
                    x.ErrorModule,
                    x.ErrorMessage,
                    x.ErrorType,
                    x.Explanation,
                    x.POSMessage,
                    x.ErrorCount,
                })
               .ToList()
               .ToBindingList();
                txtSearchCount.Text = viewList.Count.ToString(); // 레코드 개수 표시

                _bindingSource = new BindingSource(viewList, null);
                return _bindingSource;
            }
            #endregion
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
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
        }

        private void btnSearchCount2_Click(object sender, EventArgs e)
        {
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
            if (dtg_ErrorHistory.Rows.Count == 0) return;
            main.SaveAsDataGridviewToCSV(dtg_ErrorHistory);
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
            #region Amkor K5 M3F_T3F
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
            }
            //=================================================================================================================//
            #endregion
        }

        private void ComboBoxRobotNameAdd()  //RobotName  조회
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            #region Amkor K5 M3F_T3F
            //=============================================== AmkorK5 M3F_T3F =================================================//
            dtg_ErrorHistory.Rows.Clear();
            cboboxRobotName.Items.Clear();
            cboboxRobotName.Enabled = false;
            chkboxRobotName.Checked = false;
            txtSearchCount.Text = "";
            //=================================================================================================================//
            #endregion
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {

        }
    }


}

