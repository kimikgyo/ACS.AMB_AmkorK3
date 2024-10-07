using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace INA_ACS_Server
{
    public class RobotRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("RobotEvent");

        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<Robot> _robots = new List<Robot>(); // cache data

        // 현재 로봇정보는 SystemConfig화면에서 Active,Group 설정할때만 저장된다. 프로그램 기동시에는 읽기만 한다.
        public RobotRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
        }

        private void Load()
        {
            _robots.Clear();

            // DB에 저장된 데이터를 읽어와서 최대개수만큼만 캐싱해 둔다
            int i = 0;
            foreach (var robot in GetAll_Direct())
            {
                if (i < ConfigData.MiR_MaxNum)
                {
                    _robots.Add(robot);
                    i++;
                }
            }

            // DB에 저장된 데이터가 최대개수보다 적은 경우
            // DB 초기화된후 새로운 데이터로 변경할경우 ID강제로 부여
            while (i++ < ConfigData.MiR_MaxNum)
            {
                Add(new Robot()
                {
                    JobId = 0,
                    ACSRobotGroup = "",
                    ACSRobotActive = false,
                    //RobotID = 0,
                    RobotID = i,
                    RobotName = "",
                    RobotAlias = "",
                    RobotIp = "192.168.0.1",
                    StateID = RobotState.None,
                    StateText = "",
                    MissionText = "",
                    MissionQueueID = 0,
                    MapID = "",
                    BatteryPercent = 0.0f,
                    DistanceToNextTarget = 0.0f,
                    Position_Orientation = 0.0f,
                    Position_X = 0.0f,
                    Position_Y = 0.0f,
                    ErrorsJson = "",
                    RobotModel = "",
                    Product = "",
                    Door = "",
                });
            }

            // ----- local functions
            List<Robot> GetAll_Direct()
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<Robot>("SELECT * FROM Robots").ToList();
                }
            }
            // ----- local functions
        }

        public Robot Add(Robot model)
        {
            _robots.Add(model);
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO Robots
                                ([JobId]
                                ,[ACSRobotGroup]
                                ,[ACSRobotActive]
                                ,[Fleet_State]
                                ,[Fleet_State_Text]
                                ,[RobotID]
                                ,[RobotIp]
                                ,[RobotName]
                                ,[RobotAlias]
                                ,[StateID]
                                ,[StateText]
                                ,[MissionText]
                                ,[MissionQueueID]
                                ,[BatteryPercent]
                                ,[MapID]
                                ,[DistanceToNextTarget]
                                ,[Position_Orientation]
                                ,[Position_X]
                                ,[Position_Y]
                                ,[ErrorsJson]
                                ,[RobotModel]
                                ,[Product]
                                ,[Door]
                                ,[PositionZoneName])
                           VALUES
                                (@JobId
                                ,@ACSRobotGroup
                                ,@ACSRobotActive
                                ,@FleetState
                                ,@FleetStateText
                                ,@RobotID
                                ,@RobotIp
                                ,@RobotName
                                ,@RobotAlias
                                ,@StateID
                                ,@StateText
                                ,@MissionText
                                ,@MissionQueueID
                                ,@BatteryPercent
                                ,@MapID
                                ,@DistanceToNextTarget
                                ,@Position_Orientation
                                ,@Position_X
                                ,@Position_Y
                                ,@ErrorsJson
                                ,@RobotModel
                                ,@Product
                                ,@Door
                                ,@PositionZoneName);

                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                logger.Info($"Robot Add   : {model}");
                return model;
            }

            // Robot 버전
            //using (var con = new SqlConnection(connectionString))
            //{
            //    const string INSERT_SQL = @"
            //        INSERT INTO Robots
            //                    ([JobId]
            //                    ,[ACSRobotGroup]
            //                    ,[ACSRobotActive]
            //                    ,[RobotID]
            //                    ,[RobotIp]
            //                    ,[RobotName]
            //                    ,[RobotAlias]
            //                    ,[StateID]
            //                    ,[StateText]
            //                    ,[MissionText]
            //                    ,[MissionQueueID]
            //                    ,[BatteryPercent]
            //                    ,[MapID]
            //                    ,[DistanceToNextTarget]
            //                    ,[Position_Orientation]
            //                    ,[Position_X]
            //                    ,[Position_Y]
            //                    ,[ErrorsJson]
            //                    ,[RobotModel]
            //                    ,[Product]
            //                    ,[Door]
            //                    ,[PositionZoneName])
            //               VALUES
            //                    (@JobId
            //                    ,@ACSRobotGroup
            //                    ,@ACSRobotActive
            //                    ,@RobotID
            //                    ,@RobotIp
            //                    ,@RobotName
            //                    ,@RobotAlias
            //                    ,@StateID
            //                    ,@StateText
            //                    ,@MissionText
            //                    ,@MissionQueueID
            //                    ,@BatteryPercent
            //                    ,@MapID
            //                    ,@DistanceToNextTarget
            //                    ,@Position_Orientation
            //                    ,@Position_X
            //                    ,@Position_Y
            //                    ,@ErrorsJson
            //                    ,@RobotModel
            //                    ,@Product
            //                    ,@Door
            //                    ,@PositionZoneName);

            //        SELECT Cast(SCOPE_IDENTITY() As Int);";

            //    model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
            //    logger.Info($"Robot Add   : {model}");
            //    return model;
            //}
        }

        public Robot this[int index] => _robots[index];

        public int GetCount() => _robots.Count();

        public IList<Robot> GetAll() => _robots;

        public Robot GetById(int id) => _robots.SingleOrDefault(r => r.Id == id);

        //public Robot GetByRobotID(int robotID) => _robots.SingleOrDefault(r => r.RobotID == robotID);

        public Robot GetByRobotName(string robotName) => _robots.FirstOrDefault(r => r.RobotName == robotName && !string.IsNullOrEmpty(r.RobotName));

        //public int GetIndexByRobotID(int robotID) => _robots.FindIndex(r => r.RobotID == robotID);

        //public int GetIndexByRobotName(string robotName) => _robots.FindIndex(r => r.RobotName == robotName);

        //public int GetIndexOf(Robot robot) => _robots.FindIndex(r => r == robot);

        public List<Robot> Find(Func<Robot, bool> predicate)
        {
            lock (this)
            {
                return _robots.Where(predicate).ToList();
            }
        }


        public void Remove(Robot model)
        {
            _robots.Remove(model);

            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM Robots WHERE Id=@id",
                    param: new { id = model.Id });
                logger.Info($"Robot Remove: {model}");
            }
        }

        public void Update(Robot model)
        {
            lock (this)
            {
                if (!model.DataChanged) return;

                //Fleet 버전 
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE Robots 
                    SET 
                        JobId=@JobId,
                        ACSRobotGroup=@ACSRobotGroup,
                        ACSRobotActive=@ACSRobotActive,
                        Fleet_State=@FleetState,
                        Fleet_State_Text=@FleetStateText,
                        RobotID=@RobotID,
                        RobotIp=@RobotIp,
                        RobotName=@RobotName,
                        RobotAlias=@RobotAlias,
                        StateID=@StateID,
                        StateText=@StateText,
                        MissionText=@MissionText,
                        MissionQueueID=@MissionQueueID,
                        BatteryPercent=@BatteryPercent,
                        MapID=@MapID,
                        DistanceToNextTarget=@DistanceToNextTarget,
                        Position_Orientation=@Position_Orientation,
                        Position_X=@Position_X,
                        Position_Y=@Position_Y,
                        ErrorsJson=@ErrorsJson,
                        RobotModel=@RobotModel,
                        Product=@Product,
                        Door=@Door,
                        PositionZoneName=@PositionZoneName
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    logger.Info($"Robot Update: {model}");

                    model.DataChanged = false;
                }

                //Robot 버전
                //using (var con = new SqlConnection(connectionString))
                //{
                //    const string UPDATE_SQL = @"
                //    UPDATE Robots 
                //    SET 
                //        JobId=@JobId,
                //        ACSRobotGroup=@ACSRobotGroup,
                //        ACSRobotActive=@ACSRobotActive,
                //        RobotID=@RobotID,
                //        RobotIp=@RobotIp,
                //        RobotName=@RobotName,
                //        RobotAlias=@RobotAlias,
                //        StateID=@StateID,
                //        StateText=@StateText,
                //        MissionText=@MissionText,
                //        MissionQueueID=@MissionQueueID,
                //        BatteryPercent=@BatteryPercent,
                //        MapID=@MapID,
                //        DistanceToNextTarget=@DistanceToNextTarget,
                //        Position_Orientation=@Position_Orientation,
                //        Position_X=@Position_X,
                //        Position_Y=@Position_Y,
                //        ErrorsJson=@ErrorsJson,
                //        RobotModel=@RobotModel,
                //        Product=@Product,
                //        Door=@Door,
                //        PositionZoneName=@PositionZoneName
                //    WHERE Id=@Id";

                //    con.Execute(UPDATE_SQL, param: model);
                //    logger.Info($"Robot Update: {model}");

                //    model.DataChanged = false;
                //}
            }
        }

        //Robot Ip 업데이트만 따로 구성함.다른 정보 업데이트와 같이 할수없음!!
        public void UpdateIpAddress(int id, string IpAddress)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string UPDATE_SQL = @"
                    UPDATE Robots 
                    SET 
                        RobotIp=@RobotIp
                    WHERE Id=@id";

                con.Execute(UPDATE_SQL, param: new { id = id, RobotIp = IpAddress });
                //logger.Info($"Robot IpAddress Update: Id = {id} / RobotIp = {IpAddress} ");
            }
        }

        public string FindIpAddress(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string SelectDATE_SQL = @"
                    select RobotIp
                    From 
                        Robots
                    WHERE Id=@id";

                string returnIp = con.ExecuteScalar(SelectDATE_SQL, param: new { id = id }).ToString();
                logger.Info($"Robot IpAddress select: Id = {id} / IpAddress = {returnIp}");
                return returnIp;
            }
        }

        public void Delete()
        {
            lock (this)
            {
                _robots.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Robots");
                }
            }
        }

    }
}
