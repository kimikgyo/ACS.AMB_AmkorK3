using Dapper;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class ErrorHistoryChart2 : Form
    {
        private readonly string connectionString;
        private readonly ScottPlot.Styles.IStyle plotStyle = new MyPlotStyle();

        public ErrorHistoryChart2(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;

            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Padding = new Padding(0);
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            formsPlot1.BorderStyle = BorderStyle.None;
            formsPlot1.Margin = new Padding(0);

            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.Margin = new Padding(0);
            dataGridView1.BackgroundColor = Color.WhiteSmoke;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DoubleBuffered(true);
        }

        public void SetPlotConfig_PanZoomRightClick(bool active)
        {
            formsPlot1.Configuration.Pan = active;
            formsPlot1.Configuration.Zoom = active;
            formsPlot1.Configuration.MiddleClickAutoAxis = active;
            if (!active) formsPlot1.RightClicked -= formsPlot1.DefaultRightClickEvent;
        }

        public void DrawChart(DateTime searchDate1, DateTime searchDate2, ErrorHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                formsPlot1.Reset();
                Plot plt = formsPlot1.Plot;
                plt.Style(plotStyle);
                //plt.Title("에러 종류별 발생 비율");

                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var result = con.Query(StoredProcedureNames.GetErrorHistoryAggr2, params1, commandType: CommandType.StoredProcedure);

                    if (result.Count() > 0)
                    {
                        // prepare chart data
                        double[] values = result.Select(x => (double)x.에러개수).ToArray();
                        string[] labels1 = result.Select(x => (string)(x.ErrorCode?.ToString() ?? "")).ToArray();
                        string[] labels2 = result.Select(x => (string)(x.ErrorText?.ToString() ?? "-")).ToArray();

                        double totalValue = values.Sum();

                        string[] percents = Enumerable.Range(0, values.Length)
                                .Select(n => $"{values[n] / totalValue * 100:N1}%").ToArray();

                        string[] sliceLabels = Enumerable.Range(0, values.Length)
                                .Select(n => $"E-{labels1[n]}\n({percents[n]})").ToArray();

                        string[] legendLabels = labels2
                                .Select(x => x.Length > 40 ? x.Substring(0, 40) + "..." : x).ToArray();

                        // draw chart
                        var pie = plt.AddPie(values);
                        pie.DonutSize = .4;
                        pie.DonutLabel = "에러\n발생 비율";
                        pie.CenterFont.Size = 20;
                        pie.SliceFont.Size = 16;
                        pie.SliceLabels = sliceLabels;
                        //pie.ShowLabels = true;
                        //pie.ShowValues = true;
                        pie.ShowPercentages = true;
                        pie.LegendLabels = legendLabels;

                        plt.SetAxisLimits(xMin: -0.5, xMax: 1.0, yMin: -1.1, yMax: 1.1); // 범례와 겹치지 않도록 차트를 좌측으로 이동시킨다
                        plt.Legend();

                        formsPlot1.Refresh();

                        // 데이터 그리드 설정
                        dataGridView1.DataSource = Enumerable.Range(0, values.Length)
                            .Select(n => new
                            {
                                에러코드 = labels1[n],
                                에러메시지 = labels2[n],
                                //에러개수 = values[n],
                                에러비율 = percents[n],
                            }).ToList();

                        // 그리드 컬럼 정렬
                        ChartHelper.AlignGridColumns(dataGridView1);
                        return;
                    }
                }

                formsPlot1.Refresh();
                dataGridView1.DataSource = null;
            }
        }
    }
}
