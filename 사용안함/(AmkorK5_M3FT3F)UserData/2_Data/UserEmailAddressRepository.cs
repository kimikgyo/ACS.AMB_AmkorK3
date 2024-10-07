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
    public class UserEmailAddressRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<UserEmailAddressModel> _userEmailAddress = new List<UserEmailAddressModel>(); // cache data


        public UserEmailAddressRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var userEmailAddressModel = new List<UserEmailAddressModel>();

            for (int i = 0; i < ConfigData.UserEmail_MaxNum; i++)
            {
                int userEmailAddressIndex = i + 1;
                var config = GetByWaitMissionConfigIndex_Ignore_CountFlag(userEmailAddressIndex);
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
                    config = new UserEmailAddressModel
                    {

                        EmailUse = "Unuse",
                        UserEmailAddress = "None",
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                userEmailAddressModel.Add(config);
            }
            Update_DisplayFlags_Except_For(userEmailAddressModel);
            Load();

            UserEmailAddressModel GetByWaitMissionConfigIndex_Ignore_CountFlag(int WaitMissionIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<UserEmailAddressModel>("SELECT * FROM UserEmailAddress WHERE Id=@index",
                            param: new { index = WaitMissionIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<UserEmailAddressModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE UserEmailAddress SET DisplayFlag=0 WHERE Id NOT IN @ids",
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
            _userEmailAddress.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var userEmailAddress in con.Query<UserEmailAddressModel>("SELECT * FROM UserEmailAddress WHERE DisplayFlag=1"))
                {

                    _userEmailAddress.Add(userEmailAddress);
                }
            }
        }
        //DB 추가하기
        public UserEmailAddressModel Add(UserEmailAddressModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO UserEmailAddress
                                ([EmailUse]
                                ,[UserEmailAddress]
                                ,[DisplayFlag])
                           VALUES
                                (@EmailUse
                                ,@UserEmailAddress
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<UserEmailAddressModel> Find(Func<UserEmailAddressModel, bool> predicate)
        {
            lock (this)
            {
                return _userEmailAddress.Where(predicate).ToList();
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
        public IList<UserEmailAddressModel> GetAll() => _userEmailAddress;


        public List<UserEmailAddressModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<UserEmailAddressModel>("SELECT * FROM UserEmailAddress").ToList();

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
        public void Update(UserEmailAddressModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE UserEmailAddress
                    SET 
                        EmailUse=@EmailUse, 
                        UserEmailAddress=@UserEmailAddress, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(UserEmailAddressModel model)
        {
            lock (this)
            {
                _userEmailAddress.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM UserEmailAddress WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
