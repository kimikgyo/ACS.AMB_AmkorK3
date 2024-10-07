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
    public class ChargeMissionConfigRepository
    { //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<ChargeMissionConfigModel> _chargeMissionConfig = new List<ChargeMissionConfigModel>(); // cache data


        public ChargeMissionConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {

            var chargeMissionConfig = new List<ChargeMissionConfigModel>();

            for (int i = 0; i < ConfigData.ChargeMission_MaxNum; i++)
            {
                int ChargeMissionIndex = i + 1;
                var config = GetByChargeMissionConfigIndex_Ignore_CountFlag(ChargeMissionIndex);
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
                    config = new ChargeMissionConfigModel
                    {
                        ChargerGroupName = "None",
                        PositionZone = "None",
                        ChargeMissionUse = "Unuse",
                        ChargeMissionName = "None",
                        StartBattery = 0,
                        SwitchaingBattery = 0,
                        EndBattery = 0,
                        RobotName = "None",
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                chargeMissionConfig.Add(config);
               
            }
            Update_DisplayFlags_Except_For(chargeMissionConfig);
            Load();

            ChargeMissionConfigModel GetByChargeMissionConfigIndex_Ignore_CountFlag(int chargeMissionIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<ChargeMissionConfigModel>("SELECT * FROM ChargeMissionConfigs WHERE Id=@index",
                            param: new { index = chargeMissionIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<ChargeMissionConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE ChargeMissionConfigs SET DisplayFlag=0 WHERE Id NOT IN @ids",
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
            _chargeMissionConfig.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var chargeMissionConfig in con.Query<ChargeMissionConfigModel>("SELECT * FROM ChargeMissionConfigs WHERE DisplayFlag=1"))
                {

                    _chargeMissionConfig.Add(chargeMissionConfig);
                }
            }
        }
        //DB 추가하기
        public ChargeMissionConfigModel Add(ChargeMissionConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ChargeMissionConfigs
                                ([ChargerGroupName]
                                ,[PositionZone]
                                ,[ChargeMissionUse]
                                ,[ChargeMissionName]
                                ,[StartBattery]
                                ,[SwitchaingBattery]
                                ,[EndBattery]
                                ,[ProductValue]
                                ,[ProductActive]
                                ,[RobotName]
                                ,[DisplayFlag])
                           VALUES
                                (@ChargerGroupName
                                ,@PositionZone
                                ,@ChargeMissionUse
                                ,@ChargeMissionName
                                ,@StartBattery
                                ,@SwitchaingBattery
                                ,@EndBattery
                                ,@ProductValue
                                ,@ProductActive
                                ,@RobotName
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                Load();
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<ChargeMissionConfigModel> Find(Func<ChargeMissionConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _chargeMissionConfig.Where(predicate).ToList();
            }
        }

        //public List<ChargeMissionConfigModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<ChargeMissionConfigModel>("SELECT * FROM ChargeMissionConfigs WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}
        public IList<ChargeMissionConfigModel> GetAll() => _chargeMissionConfig;

        public List<ChargeMissionConfigModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ChargeMissionConfigModel>("SELECT * FROM ChargeMissionConfigs").ToList();

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
        public void Update(ChargeMissionConfigModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ChargeMissionConfigs 
                    SET 
                        ChargerGroupName=@ChargerGroupName, 
                        PositionZone=@PositionZone, 
                        ChargeMissionUse=@ChargeMissionUse, 
                        ChargeMissionName=@ChargeMissionName, 
                        StartBattery=@StartBattery, 
                        SwitchaingBattery=@SwitchaingBattery, 
                        EndBattery=@EndBattery, 
                        ProductValue=@ProductValue, 
                        ProductActive=@ProductActive,        
                        RobotName=@RobotName,        
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    Load();
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(ChargeMissionConfigModel model)
        {
            lock (this)
            {
                _chargeMissionConfig.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ChargeMissionConfigs WHERE Id=@id",
                        param: new { id = model.Id });
                    Load();
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
