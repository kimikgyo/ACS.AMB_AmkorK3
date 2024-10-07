using Dapper;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public static class ChartHelper
    {
        public static string connectionString;


        // 총반송량, 평균반송시간
        public static IDictionary<string, object> GetSummary1(DateTime searchDate1, DateTime searchDate2, JobHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var result = con.QueryFirstOrDefault(StoredProcedureNames.GetSummary1, params1, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        return (IDictionary<string, object>)result;
                    }
                }
                return null;
            }
        }


        // 시간평균반송량
        public static IDictionary<string, object> GetSummary2(DateTime searchDate1, DateTime searchDate2, JobHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var result = con.QueryFirstOrDefault(StoredProcedureNames.GetSummary2, params1, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        return (IDictionary<string, object>)result;
                    }
                }
                return null;
            }
        }


        // 월평균반송량
        public static IDictionary<string, object> GetSummary3(DateTime searchDate1, DateTime searchDate2, JobHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var result = con.QueryFirstOrDefault(StoredProcedureNames.GetSummary3, params1, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        return (IDictionary<string, object>)result;
                    }
                }
                return null;
            }
        }


        // 총에러수, 평균반송시간
        public static IDictionary<string, object> GetSummary4(DateTime searchDate1, DateTime searchDate2, ErrorHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var result = con.QueryFirstOrDefault(StoredProcedureNames.GetSummary4, params1, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        return (IDictionary<string, object>)result;
                    }
                }
                return null;
            }
        }


        // 시간평균에러수
        public static IDictionary<string, object> GetSummary5(DateTime searchDate1, DateTime searchDate2, ErrorHistoryChartConfigFilter filteredItems)
        {
            using (var con = new SqlConnection(connectionString))
            {
                //if (filteredItems?.RobotNames?.Count > 0)
                {
                    // query by stored procedure
                    var params1 = new DynamicParameters();
                    params1.Add("searchDate1", searchDate1);
                    params1.Add("searchDate2", searchDate2);
                    params1.Add("robotNames", string.Join(",", filteredItems.RobotNames));

                    var result = con.QueryFirstOrDefault(StoredProcedureNames.GetSummary5, params1, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        return (IDictionary<string, object>)result;
                    }
                }
                return null;
            }
        }


        // 월별 반송량 (not used)
        public static object DrawChart5(FormsPlot formsPlot, DateTime searchDate1, DateTime searchDate2)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string sql = @"
--월별 반송량
SELECT concat(DATEPART(MONTH, JobFinishTime), '월') [Month], COUNT(*) [반송량]
FROM [JobHistory]
WHERE 
    ResultCD = 11  
    AND (JobFinishTime >= @searchDate1 AND JobFinishTime < @searchDate2)
GROUP BY DATEPART(MONTH, JobFinishTime)
ORDER BY DATEPART(MONTH, JobFinishTime)
";

                formsPlot.Reset();
                Plot plt = formsPlot.Plot;
                plt.Title("월별 반송량");

                {
                    var result = con.Query(sql, new { searchDate1, searchDate2 });
                    if (result.Count() > 0)
                    {
                        double[] positions = Enumerable.Range(0, result.Count()).Select(x => (double)x).ToArray();
                        string[] labels = result.Select(x => (string)x.Month).ToArray();
                        double[] values = result.Select(x => (double)(x.반송량 ?? 0)).ToArray();

                        var barPlot = plt.AddBar(values, positions);
                        barPlot.Label = "반송량";
                        barPlot.ShowValuesAboveBars = true;

                        plt.XTicks(positions, labels);
                        plt.XAxis.Grid(false);
                        plt.YAxis.Label("count(ea)");
                        plt.SetAxisLimits(yMin: 0);

                        formsPlot.Refresh();

                        // return chart data
                        return result.Select((x, n) => new
                        {
                            //No = n + 1,
                            Month = labels[n],
                            반송량 = values[n],
                        }).ToList();
                    }
                }

                formsPlot.Refresh();
                return null;
            }
        }



        // 소요시간 포맷
        public static string GetFormattedTime_Summary(string strTotalSec)
        {
            if (int.TryParse(strTotalSec, out int intTotalSec))
                return GetFormattedTime_Summary(intTotalSec);
            else
                return "";
        }

        public static string GetFormattedTime_Summary(int totalSec)
        {
            //int day = (totalSec / 86400);
            //int hou = (totalSec - totalSec / 86400 * 86400) / 3600;
            //int min = (totalSec - totalSec / 3600 * 3600) / 60;
            //int sec = (totalSec - totalSec / 60 * 60) / 1;

            TimeSpan intervals = new TimeSpan(0, 0, totalSec);
            int day = intervals.Days;
            int hou = intervals.Hours;
            int min = intervals.Minutes;
            int sec = intervals.Seconds;

            var sb = new StringBuilder();

            if (day > 0)
                sb.Append($"{day}d {hou}h {min}m {sec}s");
            else if (hou > 0)
                sb.Append($"{hou}h {min}m {sec}s");
            else if (min > 0)
                sb.Append($"{min}m {sec}s");
            else
                sb.Append($"{sec}s");

            return sb.ToString();
        }

        public static string GetFormattedTime_Grid(int totalSec)
        {
            //return $"{totalSec}";
            string formattedtotalSec = GetFormattedTime_Summary(totalSec);
            //return $"{totalSec:N0} ({formattedtotalSec})";
            return $"{formattedtotalSec} ({totalSec:N0})";
        }



        // 그리드 컬럼 정렬
        public static void AlignGridColumns(DataGridView dgv)
        {
            string[] columnNames = new[] { "반송량", "평균반송시간" };

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (columnNames.Contains(col.Name))
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                else
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }
    }
}
