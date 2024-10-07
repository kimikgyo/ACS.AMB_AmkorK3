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
    public class MissionRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("MissionEvent");

        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<Mission> _missions = new List<Mission>(); // cached data

        public bool NeedUpdateUI { get; set; }

        public MissionRepository(string connectionString, RobotRepository robots)
        {
            this.connectionString = connectionString;
            Load(robots);
        }

        // DB에서 모든 항목을 로드하여 _missions 에 캐싱해 둔다
        private void Load(RobotRepository robots)
        {
            _missions.Clear();

            // DB에서 읽어와서 캐싱해 둔다
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var mission in con.Query<Mission>("SELECT * FROM Missions"))
                {
                    // 미션의 해당 로봇정보를 가져와서, 미션정보에 기입해주고
                    //mission.Robot = robots.GetById(mission.RobotTableIndex); // RobotID = fleet 관리id, RobotTableID = DB 관리id
                    var robot = robots.GetByRobotName(mission.RobotName);
                    mission.Robot = robot;
                    mission.RobotID = robot?.RobotID ?? -1;

                    _missions.Add(mission);
                }

                ////$$$$$$$$$ // 읽어온 미션중 콜버튼정보가 없는 것을 필터링한다
                //var callButtons = con.Query<CallButton>("SELECT * FROM CallButtons").ToList();
                //var missionsToDelete = new List<Mission>();
                //foreach (var mission in con.Query<Mission>("SELECT * FROM Missions"))
                //{
                //    if (callButtons.FirstOrDefault(cb => cb.ButtonName == mission.CallButtonName) == null)
                //    {
                //        missionsToDelete.Add(mission);
                //        continue;
                //    }
                //}
                ////$$$$$$$$$ // 위에서 걸러진 미션들(콜버튼 정보 없는것들)을 삭제한다
                //foreach (var m in missionsToDelete)
                //{
                //    Remove(m);
                //}
            }

            NeedUpdateUI = true;
        }

        public Mission Add(Mission model)
        {
            lock (this)
            {
                _missions.Add(model);
                NeedUpdateUI = true;

                using (var con = new SqlConnection(connectionString))
                {
                    const string INSERT_SQL = @"
                    INSERT INTO Missions
                               ([JobId]
                               ,[ACSMissionGroup]
                               ,[CallName]
                               ,[CallButtonIndex]
                               ,[CallButtonName]
                               ,[MissionName]
                               ,[ErrorMissionName]
                               ,[MissionOrderTime]
                               ,[JobCreateRobotName]
                               ,[RobotName]
                               ,[RobotID]
                               ,[ReturnID]
                               ,[MissionState])
                           VALUES
                               (@JobId
                               ,@ACSMissionGroup
                               ,@CallName
                               ,@CallButtonIndex
                               ,@CallButtonName
                               ,@MissionName
                               ,@ErrorMissionName
                               ,@MissionOrderTime
                               ,@JobCreateRobotName
                               ,@RobotName
                               ,@RobotID
                               ,@ReturnID
                               ,@MissionState);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                    model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                    logger.Info($"Mission Add   : {model}");
                    return model;
                }
            }
        }

        public List<Mission> Find(Func<Mission, bool> predicate)
        {
            lock (this)
            {
                return _missions.Where(predicate).ToList();
            }
        }

        public List<Mission> GetAll()
        {
            lock (this)
            {
                return _missions.ToList();
                //using (var con = new SqlConnection(connectionString))
                //{
                //    return con.Query<Mission>("SELECT * FROM Missions").ToList();
                //}
            }
        }

        public Mission GetById(int id)
        {
            lock (this)
            {
                return _missions.SingleOrDefault(m => m.Id == id);
                //using (var con = new SqlConnection(connectionString))
                //{
                //    return con.Query<Mission>("SELECT * FROM Missions WHERE Id=@id",
                //        param: new { id = id }).FirstOrDefault();
                //}
            }
        }

        public Mission GetByCallButtonName(string aCSMissionName)
        {
            lock (this)
            {
                return _missions.FirstOrDefault(m => m.CallName == aCSMissionName);
                //using (var con = new SqlConnection(connectionString))
                //{
                //    return con.Query<Mission>("SELECT * FROM Missions WHERE CallButtonName=@callButtonName",
                //        param: new { callButtonName = callButtonName }).FirstOrDefault();
                //}
            }
        }


        public void Remove(Mission model)
        {
            lock (this)
            {
                _missions.Remove(model);
                NeedUpdateUI = true;

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Missions WHERE Id=@id",
                        param: new { id = model.Id });
                    logger.Info($"Mission Remove: {model}");
                }
            }
        }

        public void Update(Mission model)
        {
            lock (this)
            {
                NeedUpdateUI = true;

                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE Missions 
                    SET 
                        JobId=@JobId,
                        ACSMissionGroup=@ACSMissionGroup,
                        CallName=@CallName,
                        CallButtonIndex=@CallButtonIndex, 
                        CallButtonName=@CallButtonName, 
                        MissionName=@MissionName, 
                        ErrorMissionName=@ErrorMissionName, 
                        MissionOrderTime=@MissionOrderTime, 
                        JobCreateRobotName=@JobCreateRobotName,
                        RobotName=@RobotName, 
                        RobotID=@RobotID, 
                        ReturnID=@ReturnID, 
                        MissionState=@MissionState 
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    logger.Info($"Mission Update: {model}");
                }
            }
        }
        public void Delete()
        {
            lock (this)
            {
                _missions.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Missions");
                }
            }
        }
        //public void UpdateState(Mission model)
        //{
        //    Mission missionToUpdate = _missions.SingleOrDefault(m => m.Id == model.Id);
        //    missionToUpdate.MissionState = model.MissionState;
        //    NeedUpdateUI = true;

        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE Missions SET MissionState=@missionState WHERE Id=@id",
        //            param: new { missionState = model.MissionState, id = model.Id });
        //    }
        //}
    }
}
