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
    public class JobLogRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        public JobLogRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(JobLog jobLog)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
               INSERT INTO JobHistory
                            ([CallName]
                            ,[RobotAlias]
                            ,[RobotName]
                            ,[LineName]
                            ,[PostName]
                            ,[JobState]
                            ,[CallTime]
                            ,[JobCreateTime]
                            ,[JobFinishTime]
                            ,[JobElapsedTime]
                            ,[MissionNames]
                            ,[MissionStates]
                            ,[TransportCountValue]
                            ,[ResultCD])
                       VALUES 
                            (@CallName
                            ,@RobotAlias
                            ,@RobotName
                            ,@LineName
                            ,@PostName
                            ,@JobState
                            ,@CallTime
                            ,@JobCreateTime
                            ,@JobFinishTime
                            ,@JobElapsedTime
                            ,@MissionNames
                            ,@MissionStates
                            ,@TransportCountValue
                            ,@ResultCD);
                     SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: jobLog);
                jobLog.Id = id;
                //return jobLog;
            }    
        }

        //Query 적용 부분을 그대로 Return 하기위해 IEnumerable<dynamic>를 사용
        public IEnumerable<dynamic> GetJobLogBindingSource(string SELECT_SQL)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL).ToList();
            }

        }

        public IEnumerable<dynamic> GetJobLogBindingSource_AnynymousType(string SELECT_SQL, object queryParams)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL, queryParams).ToList();
            }
        }

        public IEnumerable<dynamic> GetJobLogBindingSource_Count_From_To(string SELECT_SQL, object queryParams)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL, queryParams).ToList();
            }
        }
    }
}
