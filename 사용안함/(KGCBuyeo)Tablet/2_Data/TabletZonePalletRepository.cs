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
    public class TabletZonePalletRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<TabletZonePalletModel> _tabletZonePalletModels = new List<TabletZonePalletModel>();

        public TabletZonePalletRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {
            var tabletZonePalletModel = new List<TabletZonePalletModel>();

            for (int i = 0; i < ConfigData.TabletZonePalletConfigMaxNum; i++)
            {
                int tabletZonePalletModelIndex = i + 1;
                var config = GetByTabletZonePalletModeIndex_Ignore_CountFlag(tabletZonePalletModelIndex);
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
                    config = new TabletZonePalletModel
                    {
                        ZONENAME = "",
                        PALLETNO = 0,
                        REGDATE = DateTime.Now,
                        DisplayFlag = 1
                    };
                    Add(config);
                }
                tabletZonePalletModel.Add(config);
            }
            Update_DisplayFlags_Except_For(tabletZonePalletModel);
            Load();

            TabletZonePalletModel GetByTabletZonePalletModeIndex_Ignore_CountFlag(int tabletIpAddressModelIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<TabletZonePalletModel>("SELECT * FROM PALLETTABLE WHERE SEQ=@index",
                            param: new { index = tabletIpAddressModelIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<TabletZonePalletModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE PALLETTABLE SET DisplayFlag=0 WHERE SEQ NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.SEQ) });
                }
            }

        }
        private void Load()
        {
            _tabletZonePalletModels.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var tabletZonePalletModel in con.Query<TabletZonePalletModel>("SELECT * FROM PALLETTABLE WHERE DisplayFlag=1"))
                {

                    _tabletZonePalletModels.Add(tabletZonePalletModel);
                }
            }
        }
        //DB 추가하기
        public TabletZonePalletModel Add(TabletZonePalletModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO PALLETTABLE
                                ([ZONENAME]
                                ,[PALLETNO]
                                ,[REGDATE]
                                ,[DisplayFlag])
                           VALUES
                                (@ZONENAME
                                ,@PALLETNO
                                ,@REGDATE
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.SEQ = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<TabletZonePalletModel> Find(Func<TabletZonePalletModel, bool> predicate)
        {
            lock (this)
            {
                return _tabletZonePalletModels.Where(predicate).ToList();
            }
        }
        public IList<TabletZonePalletModel> GetAll() => _tabletZonePalletModels;


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

        public List<TabletZonePalletModel> DBGetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<TabletZonePalletModel>("SELECT * FROM PALLETTABLE").ToList();

                }
            }
        }


        //DB업데이트
        public void Update(TabletZonePalletModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE PALLETTABLE 
                    SET 
                        ZONENAME=@ZONENAME, 
                        PALLETNO=@PALLETNO, 
                        REGDATE=@REGDATE, 
                        DisplayFlag=@DisplayFlag
                    WHERE SEQ=@SEQ";

                    con.Execute(UPDATE_SQL, param: model);
                    //logger.Info($"PositionAreaConfig Update: {model}");
                }
            }
        }


        //DB삭제
        public void Remove(TabletZonePalletModel model)
        {
            lock (this)
            {
                _tabletZonePalletModels.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM PALLETTABLE WHERE SEQ=@id",
                        param: new { id = model.SEQ });
                    //logger.Info($"PositionAreaConfig Remove: {model}");
                }
            }
        }
    }
}
