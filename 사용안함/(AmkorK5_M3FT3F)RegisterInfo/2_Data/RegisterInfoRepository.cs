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
    public class RegisterInfoRepository
    { //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<RegisterInfoModel> _registerInfoModle = new List<RegisterInfoModel>(); // cache data


        public RegisterInfoRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var registerInfoModle = new List<RegisterInfoModel>();

            for (int i = 0; i < ConfigData.RegisterInfo_MaxNum; i++)
            {
                int RegisterInfoIndex = i + 1;
                var config = GetByWaitMissionConfigIndex_Ignore_CountFlag(RegisterInfoIndex);
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
                    config = new RegisterInfoModel
                    {
                        
                        ACSRobotGroup = "None",
                        RegisterNumber = 0,
                        RegisterValue = 0,
                        RegisterInfoMessge = "None",
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                registerInfoModle.Add(config);
            }
            Update_DisplayFlags_Except_For(registerInfoModle);
            Load();

            RegisterInfoModel GetByWaitMissionConfigIndex_Ignore_CountFlag(int RegisterInfoIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<RegisterInfoModel>("SELECT * FROM RegisterInfoModle WHERE Id=@index",
                            param: new { index = RegisterInfoIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<RegisterInfoModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE RegisterInfoModle SET DisplayFlag=0 WHERE Id NOT IN @ids",
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
            _registerInfoModle.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var waitMissionConfig in con.Query<RegisterInfoModel>("SELECT * FROM RegisterInfoModle WHERE DisplayFlag=1"))
                {

                    _registerInfoModle.Add(waitMissionConfig);
                }
            }
        }
        //DB 추가하기
        public RegisterInfoModel Add(RegisterInfoModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO RegisterInfoModle
                                ([ACSRobotGroup]
                                ,[RegisterNumber]
                                ,[RegisterValue]
                                ,[RegisterInfoMessge]                         
                                ,[DisplayFlag])
                           VALUES
                                (@ACSRobotGroup
                                ,@RegisterNumber
                                ,@RegisterValue 
                                ,@RegisterInfoMessge
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<RegisterInfoModel> Find(Func<RegisterInfoModel, bool> predicate)
        {
            lock (this)
            {
                return _registerInfoModle.Where(predicate).ToList();
            }
        }

        public List<RegisterInfoModel> GetDisplayFlagtrueData()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<RegisterInfoModel>("SELECT * FROM RegisterInfoModle WHERE DisplayFlag=1").ToList();

                }
            }
        }
        public List<RegisterInfoModel> GetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<RegisterInfoModel>("SELECT * FROM RegisterInfoModle").ToList();

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
        public void Update(RegisterInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE RegisterInfoModle 
                    SET 
                        ACSRobotGroup=@ACSRobotGroup, 
                        RegisterNumber=@RegisterNumber, 
                        RegisterValue=@RegisterValue, 
                        RegisterInfoMessge=@RegisterInfoMessge, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(RegisterInfoModel model)
        {
            lock (this)
            {
                _registerInfoModle.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM RegisterInfoModle WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
