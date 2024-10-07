using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace ACS.RobotMap
{
    /// <summary>
    /// 로봇 네임 데이터 (real name, nick name) 클래스
    /// </summary>
    [DebuggerDisplay("{Id}, {RobotName}, {RobotAlias}, {Display}")]
    internal class RobotNameAlias
    {
        public int Id { get; set; }
        public string RobotName { get; set; }
        public string RobotAlias { get; set; }



        private readonly static ILog EventLogger = LogManager.GetLogger("Event");
        private readonly static string connectionString = ConnectionStrings.DB1; //System.Configuration.ConfigurationManager.ConnectionStrings["Connection1"].ConnectionString;


        public static List<RobotNameAlias> GetAll()
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    var result = con.Query<RobotNameAlias>("SELECT Id,RobotName,RobotAlias FROM Robots").ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Info("RobotNameAlias/GetAll() Fail = " + ex);
                return null;
            }
        }
    }

}
