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
    public class WaitMissionConfigRepository
    { //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<WaitMissionConfigModel> _waitMissionConfig = new List<WaitMissionConfigModel>(); // cache data


        public WaitMissionConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var waitMissionConfig = new List<WaitMissionConfigModel>();

            for (int i = 0; i < ConfigData.WaitMission_MaxNum; i++)
            {
                int WaitMissionIndex = i + 1;
                var config = GetByWaitMissionConfigIndex_Ignore_CountFlag(WaitMissionIndex);
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
                    config = new WaitMissionConfigModel
                    {

                        PositionZone = "None",
                        WaitMissionUse = "Unuse",
                        WaitMissionName ="None",
                        EnableBattery = 0,
                        RobotName = "None",
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                waitMissionConfig.Add(config);
            }
            Update_DisplayFlags_Except_For(waitMissionConfig);
            Load();

            WaitMissionConfigModel GetByWaitMissionConfigIndex_Ignore_CountFlag(int WaitMissionIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<WaitMissionConfigModel>("SELECT * FROM WaitMissionConfigs WHERE Id=@index",
                            param: new { index = WaitMissionIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<WaitMissionConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE WaitMissionConfigs SET DisplayFlag=0 WHERE Id NOT IN @ids",
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
            _waitMissionConfig.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var waitMissionConfig in con.Query<WaitMissionConfigModel>("SELECT * FROM WaitMissionConfigs WHERE DisplayFlag=1"))
                {

                    _waitMissionConfig.Add(waitMissionConfig);
                }
            }
        }
        //DB 추가하기
        public WaitMissionConfigModel Add(WaitMissionConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO WaitMissionConfigs
                                ([PositionZone]
                                ,[WaitMissionUse]
                                ,[WaitMissionName]
                                ,[EnableBattery]
                                ,[ProductValue]
                                ,[ProductActive]
                                ,[RobotName]
                                ,[DisplayFlag])
                           VALUES
                                (@PositionZone
                                ,@WaitMissionUse
                                ,@WaitMissionName
                                ,@EnableBattery
                                ,@ProductValue
                                ,@ProductActive
                                ,@RobotName
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<WaitMissionConfigModel> Find(Func<WaitMissionConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _waitMissionConfig.Where(predicate).ToList();
            }
        }

        //public List<WaitMissionConfigModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<WaitMissionConfigModel>("SELECT * FROM WaitMissionConfigs WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}
        public IList<WaitMissionConfigModel> GetAll() => _waitMissionConfig;


        public List<WaitMissionConfigModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<WaitMissionConfigModel>("SELECT * FROM WaitMissionConfigs").ToList();

                }
            }
        }

        //public List<EtnLampConfigs> PositionName(string PositionName)
        //{
        //    lock (this)
        //    {
        //        return _etnLampConfigs.Where(t => t.PositionAreaUse == "Use"
        //                    && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaName == PositionName).ToList();
        //    }
        //}




        //DB업데이트
        public void Update(WaitMissionConfigModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WaitMissionConfigs
                    SET 
                        PositionZone=@PositionZone, 
                        WaitMissionUse=@WaitMissionUse, 
                        WaitMissionName=@WaitMissionName, 
                        EnableBattery=@EnableBattery, 
                        ProductValue=@ProductValue, 
                        ProductActive=@ProductActive,        
                        RobotName=@RobotName,        
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(WaitMissionConfigModel model)
        {
            lock (this)
            {
                _waitMissionConfig.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM WaitMissionConfigs WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
