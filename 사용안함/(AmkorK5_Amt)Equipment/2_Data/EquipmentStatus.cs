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
    CREATE TABLE [dbo].[EquipmentStatus]
    (
	    --[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	    [Id] [int] NOT NULL PRIMARY KEY,
	    [EQP_NAME] [varchar](50) NOT NULL,
	    [EQP_MODE] [int] NOT NULL,
	    [PORT_ACCESS] [int] NOT NULL,
	    [PORT_STATUS] [int] NOT NULL,
	    [MODIFY_DT] [datetime] NOT NULL DEFAULT getdate()
    )
     * 
     */


    public class EquipmentStatus
    {
        public int Id { get; set; }
        public string EQP_NAME { get; set; }        // RHS1, RHS2, ...
        public int EQP_MODE { get; set; }           // 0=down, 1=idle, 2=run
        public int PORT_ACCESS { get; set; }        // 0=manual, 1=auto
        public int PORT_STATUS { get; set; }        // 0=empty, 1=exist
        public DateTime MODIFY_DT { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this); ;
        }




        //private static readonly string connectionString = ConnectionStrings.DB1;
        private static readonly string connectionString = ConnectionStrings.DB2; // 원격


        public static EquipmentStatus GetEqpPortStatusByEqpName(string eqp_name)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<EquipmentStatus>("SELECT * FROM EquipmentStatus WHERE EQP_NAME=@eqp_name", param: new { eqp_name = eqp_name }).FirstOrDefault();
            }
        }

        public static List<EquipmentStatus> GetEqpPortStatus()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<EquipmentStatus>("SELECT * FROM EquipmentStatus").ToList();
            }
        }


    }
}
