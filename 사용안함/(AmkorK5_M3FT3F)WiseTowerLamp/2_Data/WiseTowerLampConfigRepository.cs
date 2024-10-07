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
    public class WiseTowerLampConfigRepository
    {
        //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<WiseTowerLampConfigModel> _wiseTowerLampConfigs = new List<WiseTowerLampConfigModel>(); // cache data


        public WiseTowerLampConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {

            var wiseTowerLampConfigs = new List<WiseTowerLampConfigModel>();

            for (int i = 0; i < ConfigData.WiseTowerLamp_MaxNum * 4; i++)
            {
                int wiseTowerLampIndex = i + 1;
                var config = GetByEtnLampConfigIndex_Ignore_CountFlag(wiseTowerLampIndex);
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
                    int TowerLampModuleNumber = i / 4;
                    for (int j = 0; j <= 3; j++)
                    {
                        config = new WiseTowerLampConfigModel
                        {
                            NameSetting = $"TowerLampModule#{TowerLampModuleNumber + 1}_Ch#{j}",
                            PositionZoneSetting = "None",
                            TowerLampUseSetting = "Unuse",
                            IpAddressSetting = "0.0.0.0",
                            OperationtimeSetting = 0,
                            productName = "None",
                            DisplayFlag = 1
                        };
                        Add(config);
                        wiseTowerLampConfigs.Add(config);
                    }

                }
                wiseTowerLampConfigs.Add(config);
            }
            Update_DisplayFlags_Except_For(wiseTowerLampConfigs);
            Load();

            WiseTowerLampConfigModel GetByEtnLampConfigIndex_Ignore_CountFlag(int etnLampIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<WiseTowerLampConfigModel>("SELECT * FROM WiseTowerLampConfigs WHERE Id=@index",
                            param: new { index = etnLampIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<WiseTowerLampConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE WiseTowerLampConfigs SET DisplayFlag=0 WHERE Id NOT IN @ids",
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
            _wiseTowerLampConfigs.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var positionAreaConfig in con.Query<WiseTowerLampConfigModel>("SELECT * FROM WiseTowerLampConfigs WHERE DisplayFlag=1"))
                {

                    _wiseTowerLampConfigs.Add(positionAreaConfig);
                }
            }
        }
        //DB 추가하기
        public WiseTowerLampConfigModel Add(WiseTowerLampConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO WiseTowerLampConfigs
                                ([NameSetting]
                                ,[PositionZoneSetting]
                                ,[ControlSetting]
                                ,[TowerLampUseSetting]
                                ,[IpAddressSetting]
                                ,[DisplayNameSetting]
                                ,[OperationtimeSetting]
                                ,[ProductValueSetting]
                                ,[ProductActiveSetting]
                                ,[ProductName]
                                ,[DisplayFlag])
                           VALUES
                                (@NameSetting
                                ,@PositionZoneSetting
                                ,@ControlSetting
                                ,@TowerLampUseSetting
                                ,@IpAddressSetting
                                ,@DisplayNameSetting
                                ,@OperationtimeSetting
                                ,@ProductValueSetting
                                ,@ProductActiveSetting
                                ,@ProductName
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<WiseTowerLampConfigModel> Find(Func<WiseTowerLampConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _wiseTowerLampConfigs.Where(predicate).ToList();
            }
        }

        public List<WiseTowerLampConfigModel> GetDisplayFlagtrueData()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<WiseTowerLampConfigModel>("SELECT * FROM WiseTowerLampConfigs WHERE DisplayFlag=1").ToList();

                }
            }
        }


        public IList<WiseTowerLampConfigModel> GetAll() => _wiseTowerLampConfigs;
        //public List<WiseTowerLampConfigs> GetAll()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<WiseTowerLampConfigs>("SELECT * FROM WiseTowerLampConfigs").ToList();

        //        }
        //    }
        //}


        //public List<EtnLampConfigs> PositionName(string PositionName)
        //{
        //    lock (this)
        //    {
        //        return _etnLampConfigs.Where(t => t.PositionAreaUse == "Use"
        //                    && t.PositionAreaName != "None" && t.PositionAreaName != "none" && t.PositionAreaName == PositionName).ToList();
        //    }
        //}

        //public void ServiceDataUpdate(WiseTowerLampConfigs model)
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            const string UPDATE_SQL = @"
        //            UPDATE WiseTowerLampConfigs 
        //            SET 
        //                Status=@Status,        
        //                WriteOutputOnSignalTime=@WriteOutputOnSignalTime,        
        //                WriteOutputOffSignalTime=@WriteOutputOffSignalTime,        
        //                TowerLampOffTimerCompletSignal=@TowerLampOffTimerCompletSignal,        
        //                ProductValueSetting=@ProductValueSetting, 
        //                ProductActiveSetting=@ProductActiveSetting,        
        //                WriteOutputSignalFlag=@WriteOutputSignalFlag,        
        //                WriteOutputSignalValue=@WriteOutputSignalValue,        
        //                RobotName=@RobotName                    
        //            WHERE Id=@Id";

        //            con.Execute(UPDATE_SQL, param: model);
        //            //Load();
        //            //logger.Info($"PositionAreaConfig Update: {model}");
        //        }
        //    }
        //}





        //DB업데이트
        public void Update(WiseTowerLampConfigModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WiseTowerLampConfigs 
                    SET 
                        NameSetting=@NameSetting, 
                        PositionZoneSetting=@PositionZoneSetting, 
                        ControlSetting=@ControlSetting, 
                        TowerLampUseSetting=@TowerLampUseSetting, 
                        IpAddressSetting=@IpAddressSetting, 
                        DisplayNameSetting=@DisplayNameSetting, 
                        ProductValueSetting=@ProductValueSetting, 
                        ProductActiveSetting=@ProductActiveSetting,       
                        OperationtimeSetting=@OperationtimeSetting,
                        ProductName=@ProductName,                    
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //Load();
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }

        public void IpAddressUpdate(WiseTowerLampConfigModel model)
        {
            lock (this)
            {
                string[] ConfigUpdateNameSplit = model.NameSetting.Split('_');
                string Name = $"%{ConfigUpdateNameSplit[0]}%";
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WiseTowerLampConfigs 
                    SET 
                        IpAddressSetting=@IpAddressSetting 
                    WHERE NameSetting LIKE @Name";

                    con.Execute(UPDATE_SQL, new { Name , model.IpAddressSetting });
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }
        //public void ModuleOutVlaueUpdate(int NodeNo, int OutPutchannel, int OutValue)
        //{
        //    lock (this)
        //    {
        //        string Name = "";
        //        Name = $"TowerLampModule#{NodeNo}_Ch#{OutPutchannel}";

        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            const string query = @"
        //            UPDATE WiseTowerLampConfigs 
        //            SET 
        //               Module_OutValue=@OutValue
        //            WHERE NameSetting=@Name";

        //            var date = con.Execute(query, param: new { Name, OutValue });
        //            //Load();
        //        }
        //    }
        //}

        //public void ModuleInVlaueUpdate(int NodeNo, int InPutchannel, int InValue)
        //{
        //    lock (this)
        //    {
        //        string Name = "";
        //        Name = $"TowerLampModule#{NodeNo}_Ch#{InPutchannel}";

        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            const string query = @"
        //            UPDATE WiseTowerLampConfigs 
        //            SET 
        //               Module_InValue=@InValue
        //            WHERE NameSetting=@Name";

        //            var date = con.Execute(query, param: new { Name, InValue });
        //            //Load();
        //        }
        //    }
        //}

        public void ControlModeUpdate(string controlValue)
        {
            lock (this)
            {

                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WiseTowerLampConfigs 
                    SET 
                        ControlSetting=@controlValue";
                    con.Execute(UPDATE_SQL, param: new { controlValue });
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }

        //DB삭제
        public void Remove(WiseTowerLampConfigModel model)
        {
            lock (this)
            {
                _wiseTowerLampConfigs.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM WiseTowerLampConfigs WHERE Name LIKE @Name",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
