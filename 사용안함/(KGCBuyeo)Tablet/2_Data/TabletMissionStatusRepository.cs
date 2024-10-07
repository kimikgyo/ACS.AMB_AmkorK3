using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class TabletMissionStatusRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("TabletEvent");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        public TabletMissionStatusRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public TabletMissionStatusModel Add(TabletMissionStatusModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string INSERT_SQL = $@"
                    INSERT INTO MESSIONTABLE
                               ([JobId]
                               ,[CALLNAME]
                               ,[MESSIONSEQ]
                               ,[LOCATION]
                               ,[CALLFLAG]
                               ,[CANCELFLAG]
                               ,[REGDATE])
                           VALUES
                               (@JobId
                               ,@CALLNAME
                               ,@MESSIONSEQ
                               ,@LOCATION
                               ,@CALLFLAG
                               ,@CANCELFLAG
                               ,@REGDATE);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                logger.Info($"Tablet Add   : {model.CALLNAME}, TabletId : {model.SEQ}");
                model.SEQ = id;
                return model;
            }
        }

        public List<TabletMissionStatusModel> GetAll()
        {
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var result = con.Query<TabletMissionStatusModel>($"SELECT * FROM MESSIONTABLE ").ToList();  
                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                    return new List<TabletMissionStatusModel>();
                }
            }
        }



        public List<TabletMissionStatusModel> GetAllCallFlag()
        {
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var result = con.Query<TabletMissionStatusModel>($"SELECT * FROM MESSIONTABLE WHERE CALLFLAG='wait'").ToList();  // 불필요한 데이터가 많을 수 있으므로 flag = 'N' 만 읽어온다
                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                    return new List<TabletMissionStatusModel>();
                }
            }
        }

        //CallName으로 해당 Seq를찾는다
        public List<TabletMissionStatusModel> GetbyCallName(string CallName)
        {
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var result = con.Query<TabletMissionStatusModel>($"SELECT * FROM MESSIONTABLE WHERE CALLNAME=@CALLNAME",
                                                                   param: new { CALLNAME = CallName }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                    return new List<TabletMissionStatusModel>();
                }
            }
        }

        public TabletMissionStatusModel GetByjobId(int jobId)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<TabletMissionStatusModel>($"SELECT * FROM MESSIONTABLE WHERE JobId=@JobId",
                    param: new { JobId = jobId }).FirstOrDefault();
            }
        }
        public TabletMissionStatusModel GetById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<TabletMissionStatusModel>($"SELECT * FROM MESSIONTABLE WHERE SEQ=@SEQ",
                    param: new { SEQ = id }).FirstOrDefault();
            }
        }

        public void Remove(TabletMissionStatusModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute($"DELETE FROM MESSIONTABLE WHERE SEQ=@SEQ",
                    param: new { SEQ = model.SEQ });
                logger.Info($"Tablet Remove: {model.CALLNAME}, TabletId : {model.SEQ} ");
            }
        }

        public void Update(TabletMissionStatusModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = $@"
                    UPDATE MESSIONTABLE
                    SET 
                        JobId=@JobId, 
                        CALLNAME=@CALLNAME, 
                        MESSIONSEQ=@MESSIONSEQ, 
                        LOCATION=@LOCATION, 
                        CALLFLAG=@CALLFLAG, 
                        CANCELFLAG=@CANCELFLAG, 
                        REGDATE=@REGDATE 
                    WHERE SEQ=@SEQ";

                try
                {
                    con.Execute(query, param: model);
                    logger.Info($"Tablet Update, TabletId : {model.SEQ}, JobId : {model.JobId} " +
                                $"Name : {model.CALLNAME}, CallFlag : {model.CALLFLAG}, CancelFlag : {model.CANCELFLAG}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                }
            }
        }
    }
}
