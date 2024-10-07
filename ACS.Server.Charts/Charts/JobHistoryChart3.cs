﻿using Dapper;
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
    public partial class JobHistoryChart3 : Form
    {
        private readonly string connectionString;
        private readonly ScottPlot.Styles.IStyle plotStyle = new MyPlotStyle();

        public JobHistoryChart3(string connectionString)
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

        public void DrawChart(DateTime searchDate1, DateTime searchDate2, JobHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                formsPlot1.Reset();
                Plot plt = formsPlot1.Plot;
                plt.Style(plotStyle);
                plt.Title("목적지별 반송량&평균반송시간");

                //if (filteredItems?.EndPos?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));
                    params1.Add("postNames", string.Join(",", filteredItems.EndPos));

                    var result = con.Query(StoredProcedureNames.GetJobHistoryAggr3_JobHistoryTransportValueColumnAdd
                                            , params1, commandType: CommandType.StoredProcedure);

                    if (result.Count() > 0)
                    {
                        // prepare chart data
                        double[] positions = Enumerable.Range(0, result.Count()).Select(x => (double)x).ToArray();
                        string[] labels = result.Select(x => (string)x.목적지).ToArray();
                        double[] values1 = result.Select(x => (double)(x.반송량 ?? 0)).ToArray();
                        double[] values2 = result.Select(x => (double)(x.평균반송시간 ?? 0)).ToArray();

                        // draw chart
                        var barPlot = plt.AddBar(values1, positions);
                        barPlot.Label = "반송량";
                        barPlot.ShowValuesAboveBars = true;

                        var linePlot = plt.AddSignal(values2, label: "평균반송시간");
                        linePlot.YAxisIndex = plt.YAxis2.AxisIndex;
                        linePlot.LineWidth = 2.0f;
                        linePlot.Smooth = true;

                        plt.XTicks(positions, labels);
                        plt.XAxis.TickLabelStyle(rotation: 45);
                        plt.XAxis.Grid(false);
                        plt.YAxis.Label("count(ea)");
                        plt.YAxis2.Label("평균반송시간(sec)");
                        plt.YAxis2.Ticks(true);
                        plt.SetAxisLimits(yMin: 0, yAxisIndex: 0);
                        plt.SetAxisLimits(yMin: 0, yAxisIndex: 1);

                        formsPlot1.Refresh();

                        // view chart data
                        dataGridView1.DataSource = Enumerable.Range(0, positions.Length)
                            .Select(n => new
                            {
                                목적지 = labels[n],
                                반송량 = values1[n],
                                평균반송시간 = ChartHelper.GetFormattedTime_Grid((int)values2[n]),
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