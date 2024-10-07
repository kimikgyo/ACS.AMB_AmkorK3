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
    public class ACSModeInfoRepository
    {

        private readonly IDbConnection db;
        private readonly string connectionString = null;
        //private readonly List<ACSModeInfoModel> _aCSModeInfoModels = new List<ACSModeInfoModel>(); // cache data


        public ACSModeInfoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //DB 불러오기

        //DB 추가하기
        public ACSModeInfoModel Add(ACSModeInfoModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ACSModeInfo
                                ([Location]
                                ,[ACSMode]
                                ,[ElevatorMode])
                           VALUES
                                (@Location
                                ,@ACSMode
                                ,@ElevatorMode);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                return model;
            }
        }

        //DB찾기
        public IEnumerable<ACSModeInfoModel> Find(Func<ACSModeInfoModel, bool> predicate)
        {
            lock (this)
            {

                using (var con = new SqlConnection(connectionString))
                {
                    var result = con.Query<ACSModeInfoModel>("SELECT * FROM ACSModeInfo");
                    return result.Where(predicate);
                }
            }
        }

        public IEnumerable<ACSModeInfoModel> GetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ACSModeInfoModel>("SELECT * FROM ACSModeInfo");
                }
            }
        }

        //DB업데이트
        public void Update(ACSModeInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ACSModeInfo 
                    SET 
                        Location=@Location, 
                        ACSMode=@ACSMode, 
                        ElevatorMode=@ElevatorMode 
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                }
            }
        }


        //DB삭제
        public void Remove(ACSModeInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ACSModeInfo WHERE Id=@id",
                        param: new { id = model.Id });
                }
            }
        }
    }
}

