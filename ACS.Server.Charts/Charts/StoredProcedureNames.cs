using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class StoredProcedureNames
    {
        public static string GetJobHistoryAggr1 = "spGetJobHistoryAggr1";
        public static string GetJobHistoryAggr2 = "spGetJobHistoryAggr2";
        public static string GetJobHistoryAggr3 = "spGetJobHistoryAggr3";
        public static string GetJobHistoryAggr4 = "spGetJobHistoryAggr4";

        public static string GetJobHistoryAggr1_JobHistoryTransportValueColumnAdd = "spGetJobHistoryAggr1_JobHistoryTransportValueColumnAdd";
        public static string GetJobHistoryAggr2_JobHistoryTransportValueColumnAdd = "spGetJobHistoryAggr2_JobHistoryTransportValueColumnAdd";
        public static string GetJobHistoryAggr3_JobHistoryTransportValueColumnAdd = "spGetJobHistoryAggr3_JobHistoryTransportValueColumnAdd";
        public static string GetJobHistoryAggr4_JobHistoryTransportValueColumnAdd = "spGetJobHistoryAggr4_JobHistoryTransportValueColumnAdd";

        public static string GetErrorHistoryAggr1_JobHistoryTransportValueColumnAdd = "spGetErrorHistoryAggr1_JobHistoryTransportValueColumnAdd";
        public static string GetErrorHistoryAggr1 = "spGetErrorHistoryAggr1";
        public static string GetErrorHistoryAggr2 = "spGetErrorHistoryAggr2";

        public static string GetSummary1 = "spGetSummary1_총반송량_평균반송시간";
        public static string GetSummary2 = "spGetSummary2_시간평균반송량";
        public static string GetSummary3 = "spGetSummary3_월평균반송량";
        public static string GetSummary4 = "spGetSummary4_총에러수_평균반송시간";
        public static string GetSummary5 = "spGetSummary5_시간평균에러수";
        public static string GetSummary6 = "spGetSummary6_월별반송량";
    }
}
