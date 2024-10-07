using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace INA_ACS_Server
{
    /*
     * 
    CREATE TABLE [dbo].[EquipmentCallNames]
    (
	    [GROUP_NAME] [varchar](50) NOT NULL,
	    [EQP_NAME]  [varchar](50) NOT NULL,
	    [INCH_TYPE] [varchar](50) NOT NULL,
	    [CALL_NAME] [varchar](50) NOT NULL,
    )
     * 
     */

    public class EquipmentCallInfo
    {
        public string GROUP_NAME { get; set; }
        public string EQP_NAME { get; set; }
        public string CALL_NAME { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this); ;
        }
    }



    public class EquipmentCallInfoRepo
    {
        private readonly static string connectionString = ConnectionStrings.DB1;

        public static List<EquipmentCallInfo> GetAll()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<EquipmentCallInfo>("SELECT * FROM EquipmentCallNames").ToList();
            }
        }

        public static EquipmentCallInfo GetByEqpName(string eqpName, string inchType)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.QueryFirstOrDefault<EquipmentCallInfo>("SELECT * FROM EquipmentCallNames WHERE EQP_NAME=@eqpName AND INCH_TYPE=@inchType",
                    param: new { eqpName, inchType });
            }
        }

        //public static string GetCallNameByEqpName(string eqpName)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        return con.QueryFirstOrDefault<string>("SELECT CALL_NAME FROM EquipmentCallNames WHERE EQP_NAME=@eqpName", param: new { eqpName });
        //    }
        //}

        public static string GetCallNameByEqpName(string eqpName, string inchType)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.QueryFirstOrDefault<string>("SELECT CALL_NAME FROM EquipmentCallNames WHERE EQP_NAME=@eqpName AND INCH_TYPE=@inchType",
                    param: new { eqpName, inchType });
            }
        }

        public static string GetEqpNameByCallName(string callName)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.QueryFirstOrDefault<string>("SELECT EQP_NAME FROM EquipmentCallNames WHERE CALL_NAME=@callName",
                    param: new { callName });
            }
        }

    }
}
