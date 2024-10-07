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
    public class ACSChargerCountConfigRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<ACSChargerCountConfigModel> _aCSChargerCountConfigModel = new List<ACSChargerCountConfigModel>(); // cache data


        public ACSChargerCountConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var aCSChargerCountConfig= new List<ACSChargerCountConfigModel>();

            for (int i = 0; i < ConfigData.ACSChargerCount_MaxNum; i++)
            {
                int aCSChargerCountConfigIndex = i + 1;
                var config = GetByACSChargerCountIndex_Ignore_CountFlag(aCSChargerCountConfigIndex);
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
                    config = new ACSChargerCountConfigModel
                    {
                        ChargerCountUse = "Unuse",
                        ChargerGroupName = "None",
                        ChargerCount = 0,
                        ChargerCountStatus = 0,
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                aCSChargerCountConfig.Add(config);
            }
            Update_DisplayFlags_Except_For(aCSChargerCountConfig);
            Load();

            ACSChargerCountConfigModel GetByACSChargerCountIndex_Ignore_CountFlag(int aCSChargerCountConfigIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<ACSChargerCountConfigModel>("SELECT * FROM ACSChargerCountConfig WHERE Id=@index",
                            param: new { index = aCSChargerCountConfigIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<ACSChargerCountConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE ACSChargerCountConfig SET DisplayFlag=0 WHERE Id NOT IN @ids",
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
            _aCSChargerCountConfigModel.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var aCSChargerCountConfigModel in con.Query<ACSChargerCountConfigModel>("SELECT * FROM ACSChargerCountConfig WHERE DisplayFlag=1"))
                {

                    _aCSChargerCountConfigModel.Add(aCSChargerCountConfigModel);
                }
            }
        }
        //DB 추가하기
        public ACSChargerCountConfigModel Add(ACSChargerCountConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ACSChargerCountConfig
                                ([ChargerCountUse]
                                ,[RobotGroupName]
                                ,[FloorName]
                                ,[FloorMapId]
                                ,[ChargerGroupName]
                                ,[ChargerCount]
                                ,[ChargerCountStatus]
                                ,[DisplayFlag])
                           VALUES
                                (@ChargerCountUse
                                ,@RobotGroupName
                                ,@FloorName
                                ,@FloorMapId
                                ,@ChargerGroupName
                                ,@ChargerCount
                                ,@ChargerCountStatus
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<ACSChargerCountConfigModel> Find(Func<ACSChargerCountConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _aCSChargerCountConfigModel.Where(predicate).ToList();
            }
        }
        public IList<ACSChargerCountConfigModel> GetAll() => _aCSChargerCountConfigModel;


        //public List<ACSRobotGroupConfigModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<ACSRobotGroupConfigModel>("SELECT * FROM ACSGroupConfigs WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}

        public List<ACSChargerCountConfigModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ACSChargerCountConfigModel>("SELECT * FROM ACSChargerCountConfig").ToList();

                }
            }
        }


        //DB업데이트
        public void Update(ACSChargerCountConfigModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ACSChargerCountConfig 
                    SET 
                        ChargerCountUse=@ChargerCountUse, 
                        RobotGroupName=@RobotGroupName, 
                        FloorName=@FloorName,
                        FloorMapId=@FloorMapId,
                        ChargerGroupName=@ChargerGroupName,
                        ChargerCount=@ChargerCount,
                        ChargerCountStatus=@ChargerCountStatus,
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(ACSChargerCountConfigModel model)
        {
            lock (this)
            {
                _aCSChargerCountConfigModel.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ACSChargerCountConfig WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
