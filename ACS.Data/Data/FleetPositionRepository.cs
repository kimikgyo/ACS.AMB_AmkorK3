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
    public class FleetPositionRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<FleetPositionModel> _fleetPositionModels = new List<FleetPositionModel>(); // cache data


        public FleetPositionRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
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
            _fleetPositionModels.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var aCSChargerCountConfigModel in con.Query<FleetPositionModel>("SELECT * FROM FleetPosition"))
                {

                    _fleetPositionModels.Add(aCSChargerCountConfigModel);
                }
            }
        }
        //DB 추가하기
        public FleetPositionModel Add(FleetPositionModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO FleetPosition
                                ([Name]
                                ,[Guid]
                                ,[TypeID]
                                ,[MapID]
                                ,[PosX]
                                ,[PosY]
                                ,[Orientation])
                           VALUES
                                (@Name
                                ,@Guid
                                ,@TypeID
                                ,@MapID
                                ,@PosX
                                ,@PosY
                                ,@Orientation);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<FleetPositionModel> Find(Func<FleetPositionModel, bool> predicate)
        {
            lock (this)
            {
                return _fleetPositionModels.Where(predicate).ToList();
            }
        }
        public IList<FleetPositionModel> GetAll() => _fleetPositionModels;


        public List<FleetPositionModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<FleetPositionModel>("SELECT * FROM FleetPosition").ToList();

                }
            }
        }


        //DB업데이트
        public void Update(FleetPositionModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE FleetPosition 
                    SET 
                        Name=@Name, 
                        Guid=@Guid, 
                        TypeID=@TypeID,
                        FloorMapId=@FloorMapId,
                        MapID=@MapID,
                        PosX=@PosX,
                        PosY=@PosY,
                        Orientation=@Orientation
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(FleetPositionModel model)
        {
            lock (this)
            {
                _fleetPositionModels.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM FleetPosition WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
        public void AllRemove()
        {
            lock(this)
            {
                _fleetPositionModels.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FleetPosition");
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
