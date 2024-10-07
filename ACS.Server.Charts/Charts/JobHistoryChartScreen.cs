using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class JobHistoryChartScreen : Form
    {
        private readonly string connectionString;
        //private readonly Form mainForm;
        //private readonly IUnitOfWork uow;

        private JobHistoryChartConfigFilter filteredItems = new JobHistoryChartConfigFilter();
        private JobHistoryChart1 chart1;
        private JobHistoryChart2 chart2;
        private JobHistoryChart3 chart3;
        private JobHistoryChart4 chart4;


        public JobHistoryChartScreen(string connectionString/*, Form mainForm, IUnitOfWork uow*/)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            //this.mainForm = mainForm;
            //this.uow = uow;
            this.Font = SystemFonts.MessageBoxFont;

            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing) // 사용자가 ALT-F4 누르거나 x 버튼 눌러서 창을 닫으려 할때
                    e.Cancel = true;
            };

            this.Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // 차트 초기화
            InitCharts();

            // 날짜픽커 포맷 설정
            dateTimePicker1.CustomFormat = "yyyy-MM-dd (dddd)";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd (dddd)";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Value = DateTime.Now.Date.AddDays(0); // 오늘
            dateTimePicker2.Value = DateTime.Now.Date.AddDays(0);

            btnSearch_Click(null, null);
        }

        private void InitCharts()
        {
            // 판넬 기본 설정
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Padding = new Padding(0);
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // 판넬 col,row 설정
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f / tableLayoutPanel1.ColumnCount));
            }
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            // 차트 생성
            chart1 = new JobHistoryChart1(connectionString) { TopLevel = false, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, Visible = true };
            chart2 = new JobHistoryChart2(connectionString) { TopLevel = false, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, Visible = true };
            chart3 = new JobHistoryChart3(connectionString) { TopLevel = false, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, Visible = true };
            chart4 = new JobHistoryChart4(connectionString) { TopLevel = false, Dock = DockStyle.Fill, FormBorderStyle = FormBorderStyle.None, Visible = true };
            // 차트 줌/이동 비활성
            chart1.SetPlotConfig_PanZoomRightClick(false);
            chart2.SetPlotConfig_PanZoomRightClick(false);
            chart3.SetPlotConfig_PanZoomRightClick(false);
            chart4.SetPlotConfig_PanZoomRightClick(false);
            // 차트 배치
            tableLayoutPanel1.Controls.Add(chart1, column: 0, row: 0);
            tableLayoutPanel1.Controls.Add(chart2, column: 1, row: 0);
            tableLayoutPanel1.Controls.Add(chart3, column: 2, row: 0);
            tableLayoutPanel1.Controls.Add(chart4, column: 3, row: 0);
        }

        public void Init()
        {
            //
        }

        private void Display(DateTime dt1, DateTime dt2)
        {
            var result1 = ChartHelper.GetSummary1(dt1, dt2, filteredItems);
            var result2 = ChartHelper.GetSummary2(dt1, dt2, filteredItems);

            lblTotalTrans.Text = result1?["총반송량"]?.ToString() ?? default;
            lblElapsedTimeMax.Text = ChartHelper.GetFormattedTime_Summary(result1?["반송시간MAX"]?.ToString());
            lblElapsedTimeMin.Text = ChartHelper.GetFormattedTime_Summary(result1?["반송시간MIN"]?.ToString());
            lblElapsedTimeAvg.Text = ChartHelper.GetFormattedTime_Summary(result1?["반송시간AVG"]?.ToString());
            lblElapsedTimeStdev.Text = result1?["반송시간STDEVP"] is double tmpValue ? ChartHelper.GetFormattedTime_Summary((int)tmpValue) : "";
            lblAvgTransHour.Text = result2?["시간평균반송량"]?.ToString() ?? default;

            //var result3 = ChartHelper.GetJobSummary3(dt1, dt2, filteredItems);
            //if (result3 != null)
            //{
            //    lblAvgTransYearly.Text = default;
            //    lblAvgTransMonthly.Text = default;
            //    lblAvgTransWeekly.Text = default;
            //    lblAvgTransDaily.Text = default;
            //}

            // 차트,그리드 표시
            chart1.DrawChart(dt1, dt2, filteredItems);
            chart2.DrawChart(dt1, dt2, filteredItems);
            chart3.DrawChart(dt1, dt2, filteredItems);
            chart4.DrawChart(dt1, dt2, filteredItems);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var fromDate = dateTimePicker1.Value;
            var toDate = dateTimePicker2.Value.AddDays(1);

            if (toDate > fromDate.AddYears(1))
            {
                MessageBox.Show("한번에 조회할 수 있는 범위는 1년까지 입니다!");
                return;
            }

            try
            {
                btnSearch.Enabled = false;
                Display(fromDate, toDate);
            }
            finally
            {
                btnSearch.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (dtg_JobHistory.Rows.Count == 0) return;
            //mainForm.SaveAsDataGridviewToCSV(dtg_JobHistory);
            //mainForm.UserLog("Report Screen", " BackUp Click ");
        }


        private void btnRange1_Click(object sender, EventArgs e)    // yesterday 
        {
            var today = DateTime.Today;
            dateTimePicker1.Value = today.Date.AddDays(-1);
            dateTimePicker2.Value = today.Date.AddDays(-1);
        }

        private void btnRange2_Click(object sender, EventArgs e)    // today 
        {
            var today = DateTime.Today;
            dateTimePicker1.Value = today.Date.AddDays(0);
            dateTimePicker2.Value = today.Date.AddDays(0);
        }

        private void btnRange3_Click(object sender, EventArgs e)    // week 
        {
            var today = DateTime.Today;
            dateTimePicker1.Value = today.AddDays(-6);
            dateTimePicker2.Value = today.AddDays(0);
        }


        private void btnConfig_Click(object sender, EventArgs e)
        {
            var allItems = GetAllItems();
            var cfgForm = new JobHistoryChartConfigForm(allItems, filteredItems);
            if (cfgForm.ShowDialog(this) == DialogResult.OK)
            {
                filteredItems = cfgForm.GetSelectedItems();
                btnSearch_Click(null, null);
            }
        }

        //private JobHistoryChartConfigFilter GetAllItems()
        //{
        //    var validConfigs = uow.JobConfigs.GetAll()
        //        .Where(x => !string.IsNullOrWhiteSpace(x.ACSMissionGroup) && x.ACSMissionGroup != "None")
        //        .Where(x => !string.IsNullOrWhiteSpace(x.CallName) && x.CallName.Contains("_"))
        //        .Select(x => x.CallName)
        //        .Distinct()
        //        .ToList();

        //    var fromPosNames = validConfigs.Select(x => x.Split('_')[0]).Distinct().ToList();
        //    var toPosNames = validConfigs.Select(x => x.Split('_')[1]).Distinct().ToList();

        //    var robotInfos = uow.Robots.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.RobotName)).Select(x => new { x.RobotName, x.RobotAlias }).OrderBy(x => x.RobotName).ToList();
        //    var robotNames = robotInfos.Select(x => x.RobotName).ToList();
        //    var robotAlias = robotInfos.Select(x => x.RobotAlias).ToList();

        //    return new JobHistoryChartConfigFilter
        //    {
        //        RobotNames = robotNames,
        //        RobotAlias = robotAlias,
        //        StartPos = fromPosNames,
        //        EndPos = toPosNames
        //    };
        //}


        private Func<JobHistoryChartConfigFilter> _GetAllItemFunctor;
        private JobHistoryChartConfigFilter GetAllItems() => _GetAllItemFunctor();

        public void SetCallback_GetConfigFilter(Func<JobHistoryChartConfigFilter> func) => _GetAllItemFunctor = func;

    }
}
