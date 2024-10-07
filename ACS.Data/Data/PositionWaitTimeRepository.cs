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
   public class PositionWaitTimeRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<PositionWaitTimeModel> _positionWaitTimeModels = new List<PositionWaitTimeModel>();
        public PositionWaitTimeRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<PositionWaitTimeModel> GetAll()
        {
            lock (this)
            {
                _positionWaitTimeModels.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    foreach (var positionWaitTimeModel in con.Query<PositionWaitTimeModel>("SELECT * FROM PositionWaitTime"))
                    {
                        _positionWaitTimeModels.Add(positionWaitTimeModel);
                    }
                }
                return _positionWaitTimeModels;
            }
        }

        public void Add(PositionWaitTimeModel positionWaitTimeModel)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
               INSERT INTO PositionWaitTime
                            ([RobotName]
                            ,[RobotAlias]
                            ,[PositionName]
                            ,[CreateTime]
                            ,[FinishTime]
                            ,[ElapsedTime]
                            ,[Mailsend])
                       VALUES 
                            (@RobotName
                            ,@RobotAlias
                            ,@PositionName
                            ,@CreateTime
                            ,@FinishTime
                            ,@ElapsedTime
                            ,@Mailsend);
                     SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: positionWaitTimeModel);
                positionWaitTimeModel.Id = id;
                //return jobLog;
            }
        }


        public void Update(PositionWaitTimeModel positionWaitTimeModel)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE PositionWaitTime 
                    SET 
                        RobotName=@RobotName,
                        RobotAlias=@RobotAlias, 
                        PositionName=@PositionName, 
                        CreateTime=@CreateTime,
                        FinishTime=@FinishTime,
                        ElapsedTime=@ElapsedTime,
                        Mailsend=@Mailsend
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: positionWaitTimeModel);


                }
            }
        }
        public void Remove(PositionWaitTimeModel model)
        {
            lock (this)
            {
                _positionWaitTimeModels.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM PositionWaitTime WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
