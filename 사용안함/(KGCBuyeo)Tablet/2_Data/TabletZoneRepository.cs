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
    public class TabletZoneRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<TabletZoneModel> _tabletZoneModels = new List<TabletZoneModel>();

        public TabletZoneRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var tabletZoneModel = new List<TabletZoneModel>();

            for (int i = 0; i < ConfigData.TabletZoneConfigMaxNum; i++)
            {
                int tabletZoneModelIndex = i + 1;
                var config = GetByTabletZoneModelIndexndex_Ignore_CountFlag(tabletZoneModelIndex);
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
                    config = new TabletZoneModel
                    {
                        ZONENAME = "",
                        REGDATE = DateTime.Now,
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                tabletZoneModel.Add(config);
            }
            Update_DisplayFlags_Except_For(tabletZoneModel);
            Load();

            TabletZoneModel GetByTabletZoneModelIndexndex_Ignore_CountFlag(int tabletZoneModelIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<TabletZoneModel>("SELECT * FROM ZONETABLE WHERE SEQ=@index",
                            param: new { index = tabletZoneModelIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<TabletZoneModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE ZONETABLE SET DisplayFlag=0 WHERE SEQ NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.SEQ) });
                }
            }

        }
        private void Load()
        {
            _tabletZoneModels.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var tabletZoneModel in con.Query<TabletZoneModel>("SELECT * FROM ZONETABLE WHERE DisplayFlag=1"))
                {

                    _tabletZoneModels.Add(tabletZoneModel);
                }
            }
        }
        //DB 추가하기
        public TabletZoneModel Add(TabletZoneModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ZONETABLE
                                ([ZONENAME]
                                ,[REGDATE]
                                ,[DisplayFlag])
                           VALUES
                                (@ZONENAME
                                ,@REGDATE
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.SEQ = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<TabletZoneModel> Find(Func<TabletZoneModel, bool> predicate)
        {
            lock (this)
            {
                return _tabletZoneModels.Where(predicate).ToList();
            }
        }
        public IList<TabletZoneModel> GetAll() => _tabletZoneModels;


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

        public List<TabletZoneModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<TabletZoneModel>("SELECT * FROM ZONETABLE").ToList();

                }
            }
        }


        //DB업데이트
        public void Update(TabletZoneModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ZONETABLE 
                    SET 
                        ZONENAME=@ZONENAME, 
                        REGDATE=@REGDATE, 
                        DisplayFlag=@DisplayFlag
                    WHERE SEQ=@SEQ";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(TabletZoneModel model)
        {
            lock (this)
            {
                _tabletZoneModels.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ZONETABLE WHERE SEQ=@id",
                        param: new { id = model.SEQ });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
