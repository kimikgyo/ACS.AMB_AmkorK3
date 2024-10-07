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
    public class SkyNetRepository
    {

        private readonly static ILog logger = LogManager.GetLogger("SkyNetEvent");

        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<SkyNetModel> _skyNetModels = new List<SkyNetModel>(); // cache data


        public SkyNetRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
        }

        //DB 불러오기
        private void Load()
        {
            _skyNetModels.Clear();


            using (var con = new SqlConnection(connectionString))
            {
                foreach (var skyNetModels in con.Query<SkyNetModel>("SELECT * FROM Skynet_RobotData"))
                {

                    _skyNetModels.Add(skyNetModels);
                }
            }
        }
        //DB 추가하기
        public SkyNetModel Add(SkyNetModel model)
        {
            _skyNetModels.Add(model);

            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO Skynet_RobotData
                                ([SkyNetMode]
                                ,[Linecode]
                                ,[Processcode]
                                ,[RobotName]
                                ,[RobotState]
                                ,[MissionName]
                                ,[MissionState])
                           VALUES
                                (@SkyNetMode
                                ,@Linecode
                                ,@Processcode
                                ,@RobotName
                                ,@RobotState
                                ,@MissionName
                                ,@MissionState);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                logger.Info($"SkyNetModel Robot Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<SkyNetModel> Find(Func<SkyNetModel, bool> predicate)
        {
            lock (this)
            {
                return _skyNetModels.Where(predicate).ToList();
            }
        }

        public List<SkyNetModel> GetAll()
        {
            lock (this)
            {
                return _skyNetModels.ToList();
            }
        }
        //public SkyNetModel GetByCallButtonName(string callButtonName)
        //{
        //    lock (this)
        //    {
        //        return _skyNetModels.FirstOrDefault(x => x.CallButtonName == callButtonName);
        //    }
        //}

        //DB업데이트
        public void Update(SkyNetModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE Skynet_RobotData 
                    SET 
                        SkyNetMode=@SkyNetMode, 
                        Linecode=@Linecode, 
                        Processcode=@Processcode, 
                        RobotName=@RobotName, 
                        RobotState=@RobotState,        
                        MissionName=@MissionName,
                        MissionState=@MissionState
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                    logger.Info($"SkyNetModel Update: {model}");
                    
                }
            }
        }


        //DB삭제
        public void Remove(SkyNetModel model)
        {
            lock (this)
            {
                _skyNetModels.Remove(model);    // Skynet_Robot_DataSend Data 자체 제거한다

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Skynet_RobotData WHERE Id=@id",
                        param: new { id = model.Id });
                    logger.Info($"SkyNetModel Remove: {model}");
                }
            }
        }
    }

}
