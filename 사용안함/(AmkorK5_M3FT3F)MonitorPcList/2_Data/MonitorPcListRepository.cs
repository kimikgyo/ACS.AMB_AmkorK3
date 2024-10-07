using Dapper;
using INA_ACS_Server.Models.AmkorK5_M3F_T3F;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server.Data.AmKorK5_M3F_T3F
{
    public class MonitorPcListRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<MonitorPcListModel> _monitorPcListModel = new List<MonitorPcListModel>(); // cache data


        public MonitorPcListRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var monitorPcListModel = new List<MonitorPcListModel>();

            for (int i = 0; i < ConfigData.MonitorPcList_MaxNum; i++)
            {
                int monitorPcListIndex = i + 1;
                var config = GetByWaitMissionConfigIndex_Ignore_CountFlag(monitorPcListIndex);
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
                    config = new MonitorPcListModel
                    {

                        IpAddress = "0.0.0.0",
                        ZoneName = "",
                        BcrExist = false,
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                monitorPcListModel.Add(config);
            }
            Update_DisplayFlags_Except_For(monitorPcListModel);
            Load();

            MonitorPcListModel GetByWaitMissionConfigIndex_Ignore_CountFlag(int monitorPcListIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<MonitorPcListModel>("SELECT * FROM MonitorPcList WHERE Id=@index",
                            param: new { index = monitorPcListIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<MonitorPcListModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE MonitorPcList SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
        }
  
        private void Load()
        {
            _monitorPcListModel.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var waitMissionConfig in con.Query<MonitorPcListModel>("SELECT * FROM MonitorPcList WHERE DisplayFlag=1"))
                {

                    _monitorPcListModel.Add(waitMissionConfig);
                }
            }
        }
        //DB 추가하기
        public MonitorPcListModel Add(MonitorPcListModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO MonitorPcList
                                ([IpAddress]
                                ,[ZoneName]
                                ,[BcrExist]
                                ,[DisplayFlag])
                           VALUES
                                (@IpAddress
                                ,@ZoneName
                                ,@BcrExist
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<MonitorPcListModel> Find(Func<MonitorPcListModel, bool> predicate)
        {
            lock (this)
            {
                return _monitorPcListModel.Where(predicate).ToList();
            }
        }

        //public List<MonitorPcListModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<MonitorPcListModel>("SELECT * FROM MonitorPcList WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}
        public IList<MonitorPcListModel> GetAll() => _monitorPcListModel;

        public List<MonitorPcListModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<MonitorPcListModel>("SELECT * FROM MonitorPcList").ToList();

                }
            }
        }
        //DB업데이트
        public void Update(MonitorPcListModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE MonitorPcList
                    SET 
                        IpAddress=@IpAddress, 
                        ZoneName=@ZoneName, 
                        BcrExist=@BcrExist, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(MonitorPcListModel model)
        {
            lock (this)
            {
                _monitorPcListModel.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM MonitorPcList WHERE Id=@id",
                        param: new { id = model.Id });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
