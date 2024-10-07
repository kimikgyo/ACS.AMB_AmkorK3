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
    public class PositionAreaConfigRepository
    {
        //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<PositionAreaConfig> _PositionAreaConfig = new List<PositionAreaConfig>(); // cache data


        public PositionAreaConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var positionAreaConfigs = new List<PositionAreaConfig>();

            for (int i = 0; i < ConfigData.PosAreaData_MaxNum; i++)
            {
                int PosAreaIndex = i + 1;
                var config = GetByPosAreaIndex_Ignore_CountFlag(PosAreaIndex);
                if (config != null)  // DB에 있으면 flag 체크한다 (set UseFlag=1)
                {
                    if (config.DisplayFlag != 1)
                    {
                        config.DisplayFlag = 1;
                        Update(config);
                    }
                }
                else
                {
                    config = new PositionAreaConfig
                    {
                        ACSRobotGroup = "None",
                        PositionAreaUse = "Unuse",
                        PositionAreaFloorName = "None",
                        PositionAreaFloorMapId = "None",
                        PositionAreaName = "None",
                        PositionAreaX1 = "0",
                        PositionAreaX2 = "0",
                        PositionAreaX3 = "0",
                        PositionAreaX4 = "0",
                        PositionAreaY1 = "0",
                        PositionAreaY2 = "0",
                        PositionAreaY3 = "0",
                        PositionAreaY4 = "0",
                        PositionWaitTimeLog = false,
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                positionAreaConfigs.Add(config);
            }
            Update_DisplayFlags_Except_For(positionAreaConfigs);
            Load();


            PositionAreaConfig GetByPosAreaIndex_Ignore_CountFlag(int PosAreaIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<PositionAreaConfig>("SELECT * FROM PositionAreaConfig WHERE Id=@index",
                            param: new { index = PosAreaIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<PositionAreaConfig> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE PositionAreaConfig SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
        }
        //DB 불러오기
        //private void Load()
        //{
        //    _PositionAreaConfig.Clear();
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        foreach (var skyNetModels in con.Query<PositionAreaConfig>("SELECT * FROM PositionAreaConfig"))
        //        {

        //            _PositionAreaConfig.Add(skyNetModels);
        //        }
        //    }
        //}
        private void Load()
        {
            _PositionAreaConfig.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var positionAreaConfig in con.Query<PositionAreaConfig>("SELECT * FROM PositionAreaConfig WHERE DisplayFlag=1"))
                {

                    _PositionAreaConfig.Add(positionAreaConfig);
                }
            }
        }
        //DB 추가하기
        public PositionAreaConfig Add(PositionAreaConfig model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO PositionAreaConfig
                                ([ACSRobotGroup]
                                ,[PositionAreaUse]
                                ,[PositionAreaFloorName]
                                ,[PositionAreaFloorMapId]
                                ,[PositionAreaName]
                                ,[PositionAreaX1]
                                ,[PositionAreaX2]
                                ,[PositionAreaX3]
                                ,[PositionAreaX4]
                                ,[PositionAreaY1]
                                ,[PositionAreaY2]
                                ,[PositionAreaY3]
                                ,[PositionAreaY4]
                                ,[PositionWaitTimeLog]
                                ,[DisplayFlag])
                           VALUES
                                (@ACSRobotGroup
                                ,@PositionAreaUse
                                ,@PositionAreaFloorName
                                ,@PositionAreaFloorMapId
                                ,@PositionAreaName
                                ,@PositionAreaX1
                                ,@PositionAreaX2
                                ,@PositionAreaX3
                                ,@PositionAreaX4
                                ,@PositionAreaY1
                                ,@PositionAreaY2
                                ,@PositionAreaY3
                                ,@PositionAreaY4
                                ,@PositionWaitTimeLog
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<PositionAreaConfig> Find(Func<PositionAreaConfig, bool> predicate)
        {
            lock (this)
            {
                return _PositionAreaConfig.Where(predicate).ToList();
            }
        }


        //public List<PositionAreaConfig> GetAll()
        //{
        //    lock (this)
        //    {
        //        return _PositionAreaConfig.ToList();
        //    }
        //}

        //public List<PositionAreaConfig> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<PositionAreaConfig>("SELECT * FROM PositionAreaConfig WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}

        public IList<PositionAreaConfig> GetAll() => _PositionAreaConfig;

        public List<PositionAreaConfig> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<PositionAreaConfig>("SELECT * FROM PositionAreaConfig").ToList();

                }
            }
        }
        public List<PositionAreaConfig> PositionName(string PositionName)
        {
            lock (this)
            {
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                            && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaName == PositionName).ToList();
            }
        }


        public List<PositionAreaConfig> PositionArea(double mir_pos_x, double mir_pos_y)    // 위치값으로 찾기
        {

            lock (this)
            {
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                            && t.PositionAreaName != "None" && t.PositionAreaName != "none"
                            && double.Parse(t.PositionAreaX1) < mir_pos_x && double.Parse(t.PositionAreaX2) > mir_pos_x
                            && double.Parse(t.PositionAreaX3) > mir_pos_x && double.Parse(t.PositionAreaX4) < mir_pos_x
                            && double.Parse(t.PositionAreaY1) < mir_pos_y && double.Parse(t.PositionAreaY2) < mir_pos_y
                            && double.Parse(t.PositionAreaY3) > mir_pos_y && double.Parse(t.PositionAreaY4) > mir_pos_y).ToList();
            }
        }
        public List<PositionAreaConfig> ElevatorPositionArea(double mir_pos_x, double mir_pos_y)    // 위치값으로 찾기(엘리베이터 위치)
        {

            lock (this)
            {
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                            && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaName.Contains("Elevator")
                            && double.Parse(t.PositionAreaX1) < mir_pos_x && double.Parse(t.PositionAreaX2) > mir_pos_x
                            && double.Parse(t.PositionAreaX3) > mir_pos_x && double.Parse(t.PositionAreaX4) < mir_pos_x
                            && double.Parse(t.PositionAreaY1) < mir_pos_y && double.Parse(t.PositionAreaY2) < mir_pos_y
                            && double.Parse(t.PositionAreaY3) > mir_pos_y && double.Parse(t.PositionAreaY4) > mir_pos_y).ToList();
            }
        }
        public List<PositionAreaConfig> NotElevatorPositionArea(double mir_pos_x, double mir_pos_y)    //위치값으로 찾기(엘리베이터 위치 제외)
        {

            lock (this)
            {
                //return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                //            && t.PositionAreaName != "None" && t.PositionAreaName != "none" && (t.PositionAreaName.Contains("Elevator") == false)
                //            && double.Parse(t.PositionAreaX1) < mir_pos_x && double.Parse(t.PositionAreaX2) > mir_pos_x
                //            && double.Parse(t.PositionAreaX3) > mir_pos_x && double.Parse(t.PositionAreaX4) < mir_pos_x
                //            && double.Parse(t.PositionAreaY1) < mir_pos_y && double.Parse(t.PositionAreaY2) < mir_pos_y
                //            && double.Parse(t.PositionAreaY3) > mir_pos_y && double.Parse(t.PositionAreaY4) > mir_pos_y).ToList();

                // TEST
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                                && t.PositionAreaName != "None" && t.PositionAreaName != "none" && (t.PositionAreaName.Contains("Elevator") == false)).ToList();
            }
        }
        //============================================UpGrade 버젼
        public List<PositionAreaConfig> UpGrade_AllPositionArea(Robot robot)    //위치값으로 찾기(모든포지션)
        {
            lock (this)
            {
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                            && t.PositionAreaName != "None" && t.PositionAreaName != "none"
                            && t.PositionAreaFloorMapId == robot.MapID && t.ACSRobotGroup == robot.ACSRobotGroup
                            && double.Parse(t.PositionAreaX1) < robot.Position_X && double.Parse(t.PositionAreaX2) > robot.Position_X
                            && double.Parse(t.PositionAreaX3) > robot.Position_X && double.Parse(t.PositionAreaX4) < robot.Position_X
                            && double.Parse(t.PositionAreaY1) < robot.Position_Y && double.Parse(t.PositionAreaY2) < robot.Position_Y
                            && double.Parse(t.PositionAreaY3) > robot.Position_Y && double.Parse(t.PositionAreaY4) > robot.Position_Y).ToList();

                // TEST
                //return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                //                && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaFloorMapId == robot.MapID).ToList();
            }
        }       

        public List<PositionAreaConfig> UpGrade_GroupPOSArea(Robot robot, string GroupName)    //Position Group 으로 위치값으로 찾기(모든포지션)
        {
            lock (this)
            {
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                            && t.PositionAreaName != "None" && t.PositionAreaName != "none"
                            && t.PositionAreaFloorMapId == robot.MapID && t.ACSRobotGroup == GroupName
                            && double.Parse(t.PositionAreaX1) < robot.Position_X && double.Parse(t.PositionAreaX2) > robot.Position_X
                            && double.Parse(t.PositionAreaX3) > robot.Position_X && double.Parse(t.PositionAreaX4) < robot.Position_X
                            && double.Parse(t.PositionAreaY1) < robot.Position_Y && double.Parse(t.PositionAreaY2) < robot.Position_Y
                            && double.Parse(t.PositionAreaY3) > robot.Position_Y && double.Parse(t.PositionAreaY4) > robot.Position_Y).ToList();

                // TEST
                //return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                //                && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaFloorMapId == robot.MapID).ToList();
            }
        }
        public List<PositionAreaConfig> UpGrade_NotElevatorPositionArea(Robot robot)    //위치값으로 찾기(엘리베이터 위치 제외)
        {
            lock (this)
            {
                //return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                //&& t.PositionAreaName != "None" && t.PositionAreaName != "none" && (t.PositionAreaName.Contains("Elevator") == false)
                //&& t.PositionAreaFloorMapId == robot.MapID && t.ACSRobotGroup == robot.ACSRobotGroup
                //&& double.Parse(t.PositionAreaX1) < robot.Position_X && double.Parse(t.PositionAreaX2) > robot.Position_X
                //&& double.Parse(t.PositionAreaX3) > robot.Position_X && double.Parse(t.PositionAreaX4) < robot.Position_X
                //&& double.Parse(t.PositionAreaY1) < robot.Position_Y && double.Parse(t.PositionAreaY2) < robot.Position_Y
                //&& double.Parse(t.PositionAreaY3) > robot.Position_Y && double.Parse(t.PositionAreaY4) > robot.Position_Y).ToList();

                // TEST
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaFloorMapId == robot.MapID && (t.PositionAreaName.Contains("Elevator") == false)).ToList();
            }
        }
        public List<PositionAreaConfig> UpGrade_ElevatorPositionArea(Robot robot)    //위치값으로 찾기(엘리베이터 위치)
        {
            lock (this)
            {
                //return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                //            && t.PositionAreaName != "None" && t.PositionAreaName != "none" &&t.PositionAreaName.Contains("Elevator")
                //            && t.PositionAreaFloorMapId == robot.MapID && t.ACSRobotGroup == robot.ACSRobotGroup
                //            && double.Parse(t.PositionAreaX1) < robot.Position_X && double.Parse(t.PositionAreaX2) > robot.Position_X
                //            && double.Parse(t.PositionAreaX3) > robot.Position_X && double.Parse(t.PositionAreaX4) < robot.Position_X
                //            && double.Parse(t.PositionAreaY1) < robot.Position_Y && double.Parse(t.PositionAreaY2) < robot.Position_Y
                //            && double.Parse(t.PositionAreaY3) > robot.Position_Y && double.Parse(t.PositionAreaY4) > robot.Position_Y).ToList();

                // TEST
                return _PositionAreaConfig.Where(t => t.PositionAreaUse == "Use"
                                && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaFloorMapId == robot.MapID && t.PositionAreaName.Contains("Elevator")).ToList();
            }
        }
        //========================================================

        //DB업데이트
        public void Update(PositionAreaConfig model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE PositionAreaConfig 
                    SET 
                        ACSRobotGroup=@ACSRobotGroup, 
                        PositionAreaUse=@PositionAreaUse, 
                        PositionAreaFloorName=@PositionAreaFloorName, 
                        PositionAreaFloorMapId=@PositionAreaFloorMapId, 
                        PositionAreaName=@PositionAreaName, 
                        PositionAreaX1=@PositionAreaX1, 
                        PositionAreaX2=@PositionAreaX2,        
                        PositionAreaX3=@PositionAreaX3,
                        PositionAreaX4=@PositionAreaX4, 
                        PositionAreaY1=@PositionAreaY1,        
                        PositionAreaY2=@PositionAreaY2,
                        PositionAreaY3=@PositionAreaY3,
                        PositionAreaY4=@PositionAreaY4,
                        PositionWaitTimeLog=@PositionWaitTimeLog,
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(PositionAreaConfig model)
        {
            lock (this)
            {
                _PositionAreaConfig.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM PositionAreaConfig WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}

