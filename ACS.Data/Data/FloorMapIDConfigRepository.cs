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
   public class FloorMapIDConfigRepository
    {
        //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<FloorMapIdConfigModel> _floorMapIDConfigModel = new List<FloorMapIdConfigModel>(); // cache data


        public FloorMapIDConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {

            var floorMapIDConfigModel = new List<FloorMapIdConfigModel>();

            for (int i = 0; i < ConfigData.FloorMapID_MaxNum; i++)
            {
                int floorMapIDConfigIndex = i + 1;
                var config = GetByEtnLampConfigIndex_Ignore_CountFlag(floorMapIDConfigIndex);
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
                    config = new FloorMapIdConfigModel
                    {
                        FloorIndex = "None",
                        FloorName = "None",
                        MapID = "None",
                        MapImageData = "None",
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                floorMapIDConfigModel.Add(config);
            }
            Update_DisplayFlags_Except_For(floorMapIDConfigModel);
            Load();
            
            FloorMapIdConfigModel GetByEtnLampConfigIndex_Ignore_CountFlag(int floorMapIDConfigIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<FloorMapIdConfigModel>("SELECT * FROM FloorMapIDConfigs WHERE Id=@index",
                            param: new { index = floorMapIDConfigIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<FloorMapIdConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE FloorMapIDConfigs SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
        }
        private void Load()
        {
            _floorMapIDConfigModel.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var floorMapIDConfigs in con.Query<FloorMapIdConfigModel>("SELECT * FROM FloorMapIDConfigs WHERE DisplayFlag=1"))
                {

                    _floorMapIDConfigModel.Add(floorMapIDConfigs);
                }
            }
        }
        //DB 추가하기
        public FloorMapIdConfigModel Add(FloorMapIdConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO FloorMapIDConfigs
                                ([FloorIndex]
                                ,[FloorName]
                                ,[MapID]
                                ,[MapImageData]
                                ,[DisplayFlag])
                           VALUES
                                (@FloorIndex
                                ,@FloorName
                                ,@MapID
                                ,@MapImageData
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                return model;
            }
        }

        //DB찾기
        public List<FloorMapIdConfigModel> Find(Func<FloorMapIdConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _floorMapIDConfigModel.Where(predicate).ToList();
            }
        }

        //public List<FloorMapIdConfigModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<FloorMapIdConfigModel>("SELECT * FROM FloorMapIDConfigs WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}

        public IList<FloorMapIdConfigModel> GetAll() => _floorMapIDConfigModel;

        //DB업데이트
        public void Update(FloorMapIdConfigModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE FloorMapIDConfigs 
                    SET 
                        FloorIndex=@FloorIndex, 
                        FloorName=@FloorName, 
                        MapID=@MapID, 
                        MapImageData=@MapImageData, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                }
            }
        }

        //DB삭제
        public void Remove(FloorMapIdConfigModel model)
        {
            lock (this)
            {
                _floorMapIDConfigModel.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM FloorMapIDConfigs WHERE Name LIKE @Name",
                        param: new { id = model.Id });
                }
            }
        }
    }
}
