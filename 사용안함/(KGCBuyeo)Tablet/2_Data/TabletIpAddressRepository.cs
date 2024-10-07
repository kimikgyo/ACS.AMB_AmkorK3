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
    public class TabletIpAddressRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<TabletIpAddressModel> _tabletIpAddressModels = new List<TabletIpAddressModel>();


        public TabletIpAddressRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var tabletIpAddressModel = new List<TabletIpAddressModel>();

            for (int i = 0; i < ConfigData.TabletIpAddressConfigMaxNum; i++)
            {
                int tabletIpAddressModelIndex = i + 1;
                var config = GetByTabletIpAddressModelIndex_Ignore_CountFlag(tabletIpAddressModelIndex);
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
                    config = new TabletIpAddressModel
                    {
                        IP = "0.0.0.0",
                        ZONENAME = "",
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                tabletIpAddressModel.Add(config);
            }
            Update_DisplayFlags_Except_For(tabletIpAddressModel);
            Load();

            TabletIpAddressModel GetByTabletIpAddressModelIndex_Ignore_CountFlag(int tabletIpAddressModelIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<TabletIpAddressModel>("SELECT * FROM IP WHERE Seq=@index",
                            param: new { index = tabletIpAddressModelIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<TabletIpAddressModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE IP SET DisplayFlag=0 WHERE Seq NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Seq) });
                }
            }

        }
        private void Load()
        {
            _tabletIpAddressModels.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var aCSRobotGroupModel in con.Query<TabletIpAddressModel>("SELECT * FROM IP WHERE DisplayFlag=1"))
                {

                    _tabletIpAddressModels.Add(aCSRobotGroupModel);
                }
            }
        }
        //DB 추가하기
        public TabletIpAddressModel Add(TabletIpAddressModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO IP
                                ([IP]
                                ,[ZONENAME]
                                ,[DisplayFlag])
                           VALUES
                                (@IP
                                ,@ZONENAME
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Seq = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<TabletIpAddressModel> Find(Func<TabletIpAddressModel, bool> predicate)
        {
            lock (this)
            {
                return _tabletIpAddressModels.Where(predicate).ToList();
            }
        }
        public IList<TabletIpAddressModel> GetAll() => _tabletIpAddressModels;


        //public List<ACSRobotGroupConfigModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<ACSRobotGroupConfigModel>("SELECT * FROM ACSGroupConfigs WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}

        public List<TabletIpAddressModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<TabletIpAddressModel>("SELECT * FROM IP").ToList();

                }
            }
        }


        //DB업데이트
        public void Update(TabletIpAddressModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE IP 
                    SET 
                        IP=@IP, 
                        ZONENAME=@ZONENAME, 
                        DisplayFlag=@DisplayFlag
                    WHERE Seq=@Seq";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(TabletIpAddressModel model)
        {
            lock (this)
            {
                _tabletIpAddressModels.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ACSGroupConfigs WHERE Seq=@id",
                        param: new { id = model.Seq });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }

}

