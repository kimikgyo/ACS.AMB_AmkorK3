using Dapper;
using INA_ACS_Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class UserNumberRepositoy
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<UserNumberModel> _UserNumberModels = new List<UserNumberModel>(); // cache data

        public UserNumberRepositoy(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var userNumbers = new List<UserNumberModel>();

            for (int i = 0; i < ConfigData.UserNumber_MaxNum; i++)
            {
                int userNumbersIndex = i + 1;
                var config = GetByWaitMissionConfigIndex_Ignore_CountFlag(userNumbersIndex);
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
                    config = new UserNumberModel
                    {
                        UserNumber = "0",
                        UserName = "",
                        UserPassword = "",
                        CallMissionAuthority = 0,
                        ElevatorAuthority = 0,
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                userNumbers.Add(config);
            }
            Update_DisplayFlags_Except_For(userNumbers);
            Load();

            UserNumberModel GetByWaitMissionConfigIndex_Ignore_CountFlag(int userNumbersIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<UserNumberModel>("SELECT * FROM UserNumber WHERE Id=@index",
                            param: new { index = userNumbersIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<UserNumberModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE UserNumber SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
        }

        private void Load()
        {
            _UserNumberModels.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var waitMissionConfig in con.Query<UserNumberModel>("SELECT * FROM UserNumber WHERE DisplayFlag=1"))
                {

                    _UserNumberModels.Add(waitMissionConfig);
                }
            }
        }

        //DB 추가하기
        public UserNumberModel Add(UserNumberModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO UserNumber
                                ([UserNumber]
                                ,[UserName]
                                ,[UserPassword]
                                ,[CallMissionAuthority]
                                ,[ElevatorAuthority]
                                ,[DisplayFlag])
                           VALUES
                                (@UserNumber
                                ,@UserName
                                ,@UserPassword
                                ,@CallMissionAuthority
                                ,@ElevatorAuthority
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<UserNumberModel> Find(Func<UserNumberModel, bool> predicate)
        {
            lock (this)
            {
                return _UserNumberModels.Where(predicate).ToList();
            }
        }

        public IList<UserNumberModel> GetAll() => _UserNumberModels;


        public List<UserNumberModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<UserNumberModel>("SELECT * FROM UserNumber").ToList();

                }
            }
        }

        //DB업데이트
        public void Update(UserNumberModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE UserNumber
                    SET 
                        UserNumber=@UserNumber, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }

        public void ContentUpdate(UserNumberModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE UserNumber
                    SET 
                        UserNumber=@UserNumber,
                        UserName=@UserName,
                        CallMissionAuthority=@CallMissionAuthority,
                        ElevatorAuthority=@ElevatorAuthority,
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(UserNumberModel model)
        {
            lock (this)
            {
                _UserNumberModels.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM UserNumber WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
