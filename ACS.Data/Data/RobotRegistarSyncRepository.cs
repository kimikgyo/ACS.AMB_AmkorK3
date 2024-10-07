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
    public class RobotRegisterSyncRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<RobotRegisterSyncModel> _robotRegisterSyncModel = new List<RobotRegisterSyncModel>(); // cache data


        public RobotRegisterSyncRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var robotRegisterSyncModel = new List<RobotRegisterSyncModel>();

            for (int i = 0; i < ConfigData.RobotRegistarSync_MaxNum; i++)
            {
                int robotRegisterSyncModelIndex = i + 1;
                var config = GetByRobotRegisterSyncIndex_Ignore_CountFlag(robotRegisterSyncModelIndex);
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
                    config = new RobotRegisterSyncModel
                    {
                       RegisterSyncUse = "Unuse",
                       PositionGroup = "None",
                       PositionName = "None",
                       ACSRobotGroup="None",
                       RegisterNo = 0,
                       RegisterValue = 0,
                       DisplayFlag =1
                    };
                    Add(config);
                }
                robotRegisterSyncModel.Add(config);
            }
            Update_DisplayFlags_Except_For(robotRegisterSyncModel);
            Load();

            RobotRegisterSyncModel GetByRobotRegisterSyncIndex_Ignore_CountFlag(int robotRegisterSyncModelIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<RobotRegisterSyncModel>("SELECT * FROM RobotRegisterSync WHERE Id=@index",
                            param: new { index = robotRegisterSyncModelIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<RobotRegisterSyncModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE RobotRegisterSync SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }

        }
        private void Load()
        {
            _robotRegisterSyncModel.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var robotRegisterSyncModel in con.Query<RobotRegisterSyncModel>("SELECT * FROM RobotRegisterSync WHERE DisplayFlag=1"))
                {

                    _robotRegisterSyncModel.Add(robotRegisterSyncModel);
                }
            }
        }
        //DB 추가하기
        public RobotRegisterSyncModel Add(RobotRegisterSyncModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO RobotRegisterSync
                                ([RegisterSyncUse]
                                ,[PositionGroup]
                                ,[PositionName]
                                ,[ACSRobotGroup]
                                ,[RegisterNo]
                                ,[RegisterValue]
                                ,[DisplayFlag])
                           VALUES
                                (@RegisterSyncUse
                                ,@PositionGroup
                                ,@PositionName
                                ,@ACSRobotGroup
                                ,@RegisterNo
                                ,@RegisterValue
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<RobotRegisterSyncModel> Find(Func<RobotRegisterSyncModel, bool> predicate)
        {
            lock (this)
            {
                return _robotRegisterSyncModel.Where(predicate).ToList();
            }
        }

        public IList<RobotRegisterSyncModel> GetAll() => _robotRegisterSyncModel;

        public List<RobotRegisterSyncModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<RobotRegisterSyncModel>("SELECT * FROM RobotRegisterSync").ToList();

                }
            }
        }


        //DB업데이트
        public void Update(RobotRegisterSyncModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE RobotRegisterSync 
                    SET 
                        RegisterSyncUse=@RegisterSyncUse, 
                        PositionGroup=@PositionGroup,
                        PositionName=@PositionName,
                        ACSRobotGroup=@ACSRobotGroup, 
                        RegisterNo=@RegisterNo,
                        RegisterValue=@RegisterValue,
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(RobotRegisterSyncModel model)
        {
            lock (this)
            {
                _robotRegisterSyncModel.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM RobotRegisterSync WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
