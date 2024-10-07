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

        private readonly IDbConnection db;
        private readonly string connectionString;
        private readonly List<ElevatorStateModule> _elevatorStateModule = new List<ElevatorStateModule>(); // cache data


        public ElevatorStateRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
        }

        //DB 불러오기
        private void Load()
        {
            _elevatorStateModule.Clear();


            using (var con = new SqlConnection(connectionString))
            {
                foreach (var skyNetModels in con.Query<ElevatorStateModule>("SELECT * FROM ElevatorState"))
                {
                    _elevatorStateModule.Add(skyNetModels);
                }
            }
        }
        //DB 추가하기
        public ElevatorStateModule Add(ElevatorStateModule model)
        {
            _elevatorStateModule.Add(model);

            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ElevatorState
                                ([RobotName]
                                ,[MiRStateElevator]
                                ,[ElevatorState]
                                ,[ElevatorMissionName])
                           VALUES
                                (@RobotName
                                ,@MiRStateElevator
                                ,@ElevatorState
                                ,@ElevatorMissionName);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                ElevatorEventlogger.Info($"ElevatorState Robot Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<ElevatorStateModule> Find(Func<ElevatorStateModule, bool> predicate)
        {
            lock (this)
            {
                return _elevatorStateModule.Where(predicate).ToList();
            }
        }

        public IList<ElevatorStateModule> GetAll() => _elevatorStateModule;


        //DB업데이트
        public void Update(ElevatorStateModule model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ElevatorState 
                    SET 
                        RobotName=@RobotName, 
                        MiRStateElevator=@MiRStateElevator, 
                        ElevatorState=@ElevatorState,
                        ElevatorMissionName=@ElevatorMissionName
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                    ElevatorEventlogger.Info($"ElevatorState Update: {model}");

                }
            }
        }


        //DB삭제
        public void Remove(ElevatorStateModule model)
        {
            lock (this)
            {
                _elevatorStateModule.Remove(model);    // Skynet_Robot_DataSend Data 자체 제거한다

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ElevatorState WHERE Id=@id",
                        param: new { id = model.Id });
                    ElevatorEventlogger.Info($"ElevatorState Remove: {model}");
                }
            }
        }
        public void Delete()
        {
            lock (this)
            {
                _elevatorStateModule.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ElevatorState");
                }
            }
        }
    }
}
