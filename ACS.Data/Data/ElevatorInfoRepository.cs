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
    public class ElevatorInfoRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        public ElevatorInfoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ElevatorInfoModel Add(ElevatorInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string INSERT_SQL = @"
                    INSERT INTO ElevatorInfo 
                                ([Location]
                                ,[ACSMode]
                                ,[ElevatorMode]
                                ,[floorIndex]
                                ,[TransportMode]
                                ,[UserNumber])
                            VALUES 
                                
                               (@Location
                               ,@ACSMode
                               ,@ElevatorMode
                               ,@floorIndex
                               ,@TransportMode
                               ,@UserNumber);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                    int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                    model.Id = id;
                    return model;
                }
            }
        }

        public List<ElevatorInfoModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ElevatorInfoModel>("SELECT * FROM ElevatorInfo").ToList();
                }
            }
        }

        //DB찾기
        public IEnumerable<ElevatorInfoModel> Find(Func<ElevatorInfoModel, bool> predicate)
        {
            lock (this)
            {

                using (var con = new SqlConnection(connectionString))
                {
                    var result = con.Query<ElevatorInfoModel>("SELECT * FROM ElevatorInfo");
                    return result.Where(predicate);
                }
            }
        }

        public ElevatorInfoModel GetById(int id)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ElevatorInfoModel>("SELECT * FROM ElevatorInfo WHERE Id=@id",
                        param: new { id = id }).FirstOrDefault();
                }
            }
        }

        public void Remove(ElevatorInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ElevatorInfo WHERE Id=@id",
                        param: new { id = model.Id });
                }
            }
        }

        public void Update(ElevatorInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE ElevatorInfo
                    SET 
                        Location      = @Location    ,
                        ACSMode       = @ACSMode     ,
                        ElevatorMode  = @ElevatorMode,
                        floorIndex    = @floorIndex  ,
                        TransportMode    = @TransportMode  ,
                        UserNumber = @UserNumber       
                      
                    WHERE Id=@Id";

                    con.Execute(query, param: model);
                }
            }
        }

        public void ACSModeUpdate(ElevatorInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE ElevatorInfo
                    SET 
                        ACSMode  = @ACSMode
                    WHERE Location=@Location";

                    con.Execute(query, param: model);
                }
            }
        }

        public void ElevatorModeUpdate(ElevatorInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE ElevatorInfo
                    SET 
                        ElevatorMode  = @ElevatorMode
                    WHERE Location=@Location";

                    con.Execute(query, param: model);
                }
            }
        }

        public void TransportModeUpdate(ElevatorInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE ElevatorInfo
                    SET 
                        TransportMode = @TransportMode
                    WHERE floorIndex=@floorIndex";

                    con.Execute(query, param: model);
                }
            }
        }

    }
}
