using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
   public class ErrorHistoryRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        public ErrorHistoryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void Add(ErrorHistory errorHistory)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
               INSERT INTO ErrorHistory
                            ([ErrorCreateTime]
                            ,[RobotName]
                            ,[ErrorCode]
                            ,[ErrorDescription]
                            ,[ErrorModule]
                            ,[ErrorMessage]
                            ,[ErrorType]
                            ,[Explanation]
                            ,[POSMessage])
                       VALUES 
                            (@ErrorCreateTime
                            ,@RobotName
                            ,@ErrorCode
                            ,@ErrorDescription
                            ,@ErrorModule
                            ,@ErrorMessage
                            ,@ErrorType
                            ,@Explanation
                            ,@POSMessage);
                     SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: errorHistory);
                errorHistory.Id = id;
                //return jobLog;
            }
        }

        //Query 적용 부분을 그대로 Return 하기위해 IEnumerable<dynamic>를 사용
        public IEnumerable<dynamic> GetErrorHistoryBindingSource(string SELECT_SQL)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL).ToList();
            }

        }

        public IEnumerable<dynamic> GetErrorHistoryBindingSource_AnynymousType(string SELECT_SQL, object queryParams)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL, queryParams).ToList();
            }
        }

        public IEnumerable<dynamic> GetErrorHistoryBindingSource_Count_From_To(string SELECT_SQL, object queryParams)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL, queryParams).ToList();
            }
        }
    }
}
