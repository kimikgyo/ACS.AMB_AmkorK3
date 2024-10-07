using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ElevatorStateRepository
    {
        private readonly ILog ElevatorEventlogger = LogManager.GetLogger("ElevatorEvent");
        private readonly string connectionString = null;

        public ElevatorStateRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<ElevatorStateModule> Load()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return (List<ElevatorStateModule>)con.Query<ElevatorStateModule>("SELECT * FROM ElevatorState");
            }
        }

        public ElevatorStateModule Add(ElevatorStateModule model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ElevatorState
                                ([RobotAlias]
                                ,[RobotName]
                                ,[ElevatorRobotState]
                                ,[ElevatorMissionName]
                                ,[ElevatorSourceFloor]
                                ,[ElevatorDestFloor])
                           VALUES
                                (@RobotAlias
                                ,@RobotName
                                ,@ElevatorRobotState
                                ,@ElevatorMissionName
                                ,@ElevatorSourceFloor
                                ,@ElevatorDestFloor);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);

                return model;
            }
        }

        public void Remove(ElevatorStateModule model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM ElevatorState WHERE RobotName=@RobotName",
                    param: new { Id = model.Id });
            }
        }

        public void ElevatorUpdate(ElevatorStateModule model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ElevatorState 
                    SET 
                        ElevatorState=@ElevatorState,
                        ElevatorFloor=@ElevatorFloor
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                    ElevatorEventlogger.Info($"ElevatorState Update: {model}");

                }
            }
        }

        public void ElevatorRobotUpdate(ElevatorStateModule model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ElevatorState 
                    SET 
                        ElevatorRobotState=@ElevatorRobotState
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                    ElevatorEventlogger.Info($"ElevatorState Update: {model}");

                }
            }
        }
    }
}
