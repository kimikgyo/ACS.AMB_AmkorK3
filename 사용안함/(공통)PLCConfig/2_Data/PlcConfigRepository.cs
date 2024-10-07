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
    public class PlcConfigRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<PlcConfig> _plcConfig = new List<PlcConfig>(); // cached data

        public PlcConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Validate_DB_Items();
        }

        public void Validate_DB_Items()
        {
            lock (this)
            {
                var plcConfig = new List<PlcConfig>();

                for (int i = 0; i < ConfigData.PlcModule_MaxNum; i++)
                {
                    int plcConfigIndex = i + 1;
                    var config = GetByPosAreaIndex_Ignore_CountFlag(plcConfigIndex);
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
                        config = new PlcConfig
                        {
                            ControlMode = null,
                            PlcModuleUse = null,
                            PlcIpAddress = null,
                            PortNumber = 0,
                            PlcModuleName = null,
                            PlcMapType = "1WORD",
                            ReadFirstMapAddress = null,
                            ReadSecondMapAddress = null,
                            WriteFirstMapAddress = null,
                            WriteSecondMapAddress = null,
                            CallNotOverlapCount = 0,
                            DisplayFlag = 1
                        };
                        Add(config);
                    }
                    plcConfig.Add(config);
                }
                Update_DisplayFlags_Except_For(plcConfig);
                Load();

                PlcConfig GetByPosAreaIndex_Ignore_CountFlag(int plcConfigIndex)
                {
                    lock (this)
                    {
                        using (var con = new SqlConnection(connectionString))
                        {
                            return con.Query<PlcConfig>("SELECT * FROM PlcConfigs WHERE Id=@index",
                                param: new { index = plcConfigIndex }).FirstOrDefault();
                        }
                    }
                }

                void Update_DisplayFlags_Except_For(List<PlcConfig> someConfigs)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        con.Execute("UPDATE PlcConfigs SET DisplayFlag=0 WHERE Id NOT IN @ids",
                            param: new { ids = someConfigs.Select(c => c.Id) });
                    }
                }
            }
        }

        private void Load()
        {
            lock (this)
            {
                _plcConfig.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    foreach (var plcConfig in con.Query<PlcConfig>("SELECT * FROM PlcConfigs WHERE DisplayFlag=1"))
                    {

                        _plcConfig.Add(plcConfig);
                    }
                }
            }
        }

        //추가 하기
        public PlcConfig Add(PlcConfig model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    _plcConfig.Add(model);
                    const string INSERT_SQL = @"
                    INSERT INTO PlcConfigs
                               ([ControlMode]
                               ,[PlcModuleUse]
                               ,[PlcIpAddress]
                               ,[PortNumber]
                               ,[PlcModuleName]
                               ,[PlcMapType]
                               ,[ReadFirstMapAddress]
                               ,[ReadSecondMapAddress]
                               ,[WriteFirstMapAddress]
                               ,[WriteSecondMapAddress]
                               ,[CallNotOverlapCount]
                               ,[DisplayFlag])
                           VALUES
                               (@ControlMode
                               ,@PlcModuleUse
                               ,@PlcIpAddress
                               ,@PortNumber
                               ,@PlcModuleName
                               ,@PlcMapType
                               ,@ReadFirstMapAddress
                               ,@ReadSecondMapAddress
                               ,@WriteFirstMapAddress
                               ,@WriteSecondMapAddress
                               ,@CallNotOverlapCount
                               ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                    model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                    return model;
                }
            }
        }

        public IList<PlcConfig> GetAll()
        {
            lock (this)
            {
                return _plcConfig;
            }
        }

        ////데이터 불러오기
        //public List<PlcConfig> GetAll()
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        return con.Query<PlcConfig>("SELECT * FROM PlcConfigs").ToList();

        //    }
        //}

        public List<PlcConfig> Find(Func<PlcConfig, bool> predicate)
        {
            lock (this)
            {
                return _plcConfig.Where(predicate).ToList();
            }
        }

        //데이터 삭제
        public void Remove(PlcConfig model)
        {
            lock (this)
            {
                _plcConfig.Remove(model);
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM PlcConfigs WHERE Id=@id",
                        param: new { id = model.Id });
                }
            }
        }
        //데이터 찾기
        public PlcConfig GetByPlcMapName(string PlcMapName)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<PlcConfig>("SELECT * FROM PlcConfigs WHERE PlcMapName=@PlcMapName",
                        param: new { name = PlcMapName }).FirstOrDefault();
                }
            }
        }
        //데이터 업데이트
        public void Update(PlcConfig model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE PlcConfigs 
                    SET 
                        ControlMode=@ControlMode,
                        PlcModuleUse=@PlcModuleUse,
                        PlcIpAddress=@PlcIpAddress,
                        PortNumber=@PortNumber,
                        PlcModuleName=@PlcModuleName,
                        PlcMapType=@PlcMapType,
                        ReadFirstMapAddress=@ReadFirstMapAddress,
                        ReadSecondMapAddress=@ReadSecondMapAddress,
                        WriteFirstMapAddress=@WriteFirstMapAddress,
                        WriteSecondMapAddress=@WriteSecondMapAddress,
                        CallNotOverlapCount=@CallNotOverlapCount,
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";
                    con.Execute(query, param: model);
                }
            }
        }

        public void ControlModeUpdate(string controlValue)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE PlcConfigs 
                    SET 
                        ControlMode=@controlValue";
                    con.Execute(query, param: new { controlValue });
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }
    }
}
