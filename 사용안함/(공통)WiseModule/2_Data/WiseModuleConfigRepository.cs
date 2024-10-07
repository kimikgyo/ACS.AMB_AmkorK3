using Dapper;
using INA_ACS_Server.Models.AmkorK5_M3F_T3F;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server.Data.AmKorK5_M3F_T3F
{
    public class WiseModuleConfigRepository
    {
        //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<WiseModuleConfigModel> _wiseModuleConfigs = new List<WiseModuleConfigModel>(); // cache data


        public WiseModuleConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {

            var wiseModuleConfigs = new List<WiseModuleConfigModel>();

            for (int i = 0; i < ConfigData.WiseModule_MaxNum * 4; i++)
            {
                int wiseModuleIndex = i + 1;
                var config = GetByEtnLampConfigIndex_Ignore_CountFlag(wiseModuleIndex);
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
                    int WiseModuleNumber = i / 4;
                    for (int j = 0; j <= 3; j++)
                    {
                        config = new WiseModuleConfigModel
                        {
                            ModuleName = $"WiseModule#{WiseModuleNumber + 1}_Ch#{j}",
                            ModuleIpAddress = "0.0.0.0",
                            ModuleUse = "Unuse",
                            ModuleStatus = "DisConnect",
                            ModuleIn_Value = 0,
                            ModuleOut_Value = 0,
                            DisplayName = "None",
                            DisplayFlag = 1
                        };
                        Add(config);
                        wiseModuleConfigs.Add(config);
                    }

                }
                wiseModuleConfigs.Add(config);
            }
            Update_DisplayFlags_Except_For(wiseModuleConfigs);
            Load();

            WiseModuleConfigModel GetByEtnLampConfigIndex_Ignore_CountFlag(int wiseModuleIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<WiseModuleConfigModel>("SELECT * FROM WiseModules WHERE Id=@index",
                            param: new { index = wiseModuleIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<WiseModuleConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE WiseModules SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
        }

        private void Load()
        {
            _wiseModuleConfigs.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var wiseModuleConfigModel in con.Query<WiseModuleConfigModel>("SELECT * FROM WiseModules WHERE DisplayFlag=1"))
                {

                    _wiseModuleConfigs.Add(wiseModuleConfigModel);
                }
            }
        }
        //DB 추가하기
        public WiseModuleConfigModel Add(WiseModuleConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO WiseModules
                                ([ModuleUse]
                                ,[ModuleIpAddress]
                                ,[ModuleName]
                                ,[ModuleStatus]
                                ,[ModuleControlMode]
                                ,[ModuleIn_Value]
                                ,[ModuleOut_Value]
                                ,[DisplayName]
                                ,[DisplayFlag])
                           VALUES
                                (@ModuleUse
                                ,@ModuleIpAddress
                                ,@ModuleName
                                ,@ModuleStatus
                                ,@ModuleControlMode
                                ,@ModuleIn_Value
                                ,@ModuleOut_Value
                                ,@DisplayName
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<WiseModuleConfigModel> Find(Func<WiseModuleConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _wiseModuleConfigs.Where(predicate).ToList();
            }
        }

        public List<WiseModuleConfigModel> GetDisplayFlagtrueData()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<WiseModuleConfigModel>("SELECT * FROM WiseModules WHERE DisplayFlag=1").ToList();

                }
            }
        }


        public IList<WiseModuleConfigModel> GetAll() => _wiseModuleConfigs;

        //DB업데이트
        public void Update(WiseModuleConfigModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WiseModules 
                    SET 
                        ModuleUse=@ModuleUse, 
                        ModuleIpAddress=@ModuleIpAddress, 
                        ModuleName=@ModuleName, 
                        ModuleStatus=@ModuleStatus, 
                        ModuleControlMode=@ModuleControlMode, 
                        ModuleIn_Value=@ModuleIn_Value, 
                        ModuleOut_Value=@ModuleOut_Value, 
                        DisplayName=@DisplayName, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //Load();
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }

        }

        public void ModuleUpdate(WiseModuleConfigModel model)
        {
            lock (this)
            {
                string[] ConfigUpdateNameSplit = model.ModuleName.Split('_');
                string Name = $"%{ConfigUpdateNameSplit[0]}%";
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WiseModules 
                    SET 
                        ModuleStatus=@ModuleStatus,
                        ModuleIpAddress=@ModuleIpAddress
                    WHERE ModuleName LIKE @Name";

                    con.Execute(UPDATE_SQL, new { Name, model.ModuleStatus, model.ModuleIpAddress });
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }

            }
        }


        public void ControlModeUpdate(string controlValue)
        {
            lock (this)
            {

                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE WiseModules 
                    SET 
                        ModuleControlMode=@controlValue";
                    con.Execute(UPDATE_SQL, param: new { controlValue });
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }

        //DB삭제
        public void Remove(WiseModuleConfigModel model)
        {
            lock (this)
            {
                _wiseModuleConfigs.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM WiseModules WHERE ModuleName LIKE @ModuleName",
                        param: new { id = model.Id });
                    //Load();
                    //logger.Info($"PositionAreaConfig Remove: {model}");

                }
            }
        }
    }
}
