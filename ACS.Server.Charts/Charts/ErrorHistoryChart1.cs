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
    public partial class ErrorHistoryChart1 : Form
    {
        private readonly string connectionString;
        private readonly ScottPlot.Styles.IStyle plotStyle = new MyPlotStyle();

        public ErrorHistoryChart1(string connectionString)
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
                plt.Title("일별 에러발생건수&반송량");

                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var results = con.QueryMultiple(StoredProcedureNames.GetErrorHistoryAggr1_JobHistoryTransportValueColumnAdd, params1, commandType: CommandType.StoredProcedure);
                    var result1 = results.Read();
                    var result2 = results.Read();

                    if (result1.Count() > 0 && result2.Count() > 0)
                    {
                        // prepare chart data
                        double[] positions = Enumerable.Range(0, result1.Count()).Select(x => (double)x).ToArray();
                        string[] labels = result1.Select(x => (string)x.DATE).ToArray();
                        double[] values1 = result1.Select(x => (double)x.TOTAL).ToArray();
                        double[] values2 = result1.Select(x => (double)x.OFFICE).ToArray();
                        double[] values3 = result1.Select(x => (double)x.NIGHT).ToArray();
                        double[] values4 = result2.Select(x => (double)x.반송량).ToArray();

                        // draw chart (전체에러수 막대그래프 그리고 그 위에 OFFCE에러수 막대그래프 곂쳐서 그린다)
                        var barPlot1 = plt.AddBar(values1, color: Color.DarkSlateBlue); // 전체 에러수
                        barPlot1.Label = "night";
                        barPlot1.ShowValuesAboveBars = true;

                        var barPlot2 = plt.AddBar(values2, color: Color.DarkOrange); // OFFICE 에러수
                        barPlot2.Label = "office";
                        barPlot2.ShowValuesAboveBars = false;

                        var linePlot = plt.AddSignal(values4, color: Color.ForestGreen, label: "반송량");
                        linePlot.YAxisIndex = plt.YAxis2.AxisIndex;
                        linePlot.LineWidth = 2.0f;
                        linePlot.Smooth = true;

                        plt.XTicks(positions, labels);
                        plt.XAxis.TickLabelStyle(rotation: 45);
                        plt.XAxis.Grid(false);
                        plt.YAxis.Label("에러개수(ea)");
                        plt.YAxis2.Label("반송량(ea)");
                        plt.YAxis2.Ticks(true);
                        plt.SetAxisLimits(yMin: 0, yAxisIndex: 0);
                        plt.SetAxisLimits(yMin: 0, yAxisIndex: 1);
                        plt.Legend(location: Alignment.UpperRight);

                        formsPlot1.Refresh();

                        // 그리드 데이터 설정
                        dataGridView1.DataSource = Enumerable.Range(0, positions.Length)
                            .Select(n => new
                            {
                                DATE = labels[n],
                                TOTAL = values1[n],
                                OFFICE = values2[n],
                                NIGHT = values3[n],
                                반송량 = values4[n],
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
