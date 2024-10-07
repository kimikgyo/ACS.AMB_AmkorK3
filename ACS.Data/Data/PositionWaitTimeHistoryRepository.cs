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
   public class PositionWaitTimeHistoryRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        public PositionWaitTimeHistoryRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<PositionWaitTimeHistory> GetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string SELECT_SQL = @"Select *from PositionWaitTimeHistory";
                    return con.Query<PositionWaitTimeHistory>(SELECT_SQL).ToList();
                }
            }

        }

        public void Add(PositionWaitTimeHistory positionWaitTimeHistory )
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
               INSERT INTO PositionWaitTimeHistory
                            ([RobotName]
                            ,[RobotAlias]
                            ,[PositionName]
                            ,[CreateTime]
                            ,[FinishTime]
                            ,[ElapsedTime])
                       VALUES 
                            (@RobotName
                            ,@RobotAlias
                            ,@PositionName
                            ,@CreateTime
                            ,@FinishTime
                            ,@ElapsedTime);
                     SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: positionWaitTimeHistory);
                positionWaitTimeHistory.Id = id;
                //return jobLog;
            }
        }

      
        public void Update(PositionWaitTimeHistory model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ElevatorState 
                    SET 
                        RobotName=@RobotName,
                        RobotAlias=@RobotAlias, 
                        PositionName=@PositionName, 
                        CreateTime=@CreateTime,
                        FinishTime=@FinishTime,
                        ElapsedTime=@ElapsedTime
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);


                }
            }
        }

        //Query 적용 부분을 그대로 Return 하기위해 IEnumerable<dynamic>를 사용
        public IEnumerable<dynamic> GetPositionWaitTimeHistoryBindingSource(string SELECT_SQL)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL).ToList();
            }

        }

        public IEnumerable<dynamic> GetPositionWaitTimeHistoryBindingSource_AnynymousType(string SELECT_SQL, object queryParams)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL, queryParams).ToList();
            }
        }

        public IEnumerable<dynamic> GetPositionWaitTimeHistoryBindingSource_Count_From_To(string SELECT_SQL, object queryParams)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query(SELECT_SQL, queryParams).ToList();
            }
        }
    }
}
