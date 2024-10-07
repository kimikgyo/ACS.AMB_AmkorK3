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
    public class PopRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("PopEvent");

        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly string tableName;



        public PopRepository(string connectionString, string tableName)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
        }

        public PopModel Add(PopModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string INSERT_SQL = $@"
                    INSERT INTO {tableName}
                               ([LINE_CD]
                               ,[POST_CD]
                               ,[COMM_PO]
                               ,[COMM_ANG]
                               ,[RETU_TYPE]
                               ,[POST_STA]
                               ,[ACS_IF_FLAG]
                               ,[CREATE_DT])
                           VALUES
                               (@LINE_CD
                               ,@POST_CD
                               ,@COMM_PO
                               ,@COMM_ANG
                               ,@RETU_TYPE
                               ,@POST_STA
                               ,@ACS_IF_FLAG
                               ,@CREATE_DT);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                logger.Info($"Pop Add   : {model}");
                model.Id = id;
                return model;
            }
        }

        public List<PopModel> GetAll()
        {
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    //var result = con.Query<PopModel>($"SELECT * FROM {tableName}").ToList();
                    var result = con.Query<PopModel>($"SELECT * FROM {tableName} WHERE ACS_IF_FLAG='N'").ToList();  // 불필요한 데이터가 많을 수 있으므로 flag = 'N' 만 읽어온다
                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                    return new List<PopModel>();
                }
            }
        }

        public PopModel GetById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<PopModel>($"SELECT * FROM {tableName} WHERE Id=@id",
                    param: new { id = id }).FirstOrDefault();
            }
        }

        public PopModel GetByPopKey(string popKey)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string[] queryParameters = popKey.Split('_');

                try
                {
                    return con.Query<PopModel>($"SELECT TOP 1 * FROM {tableName} WHERE LINE_CD=@lineCD AND POST_CD=@postCD AND ACS_IF_Flag=@ACS_IF_Flag",
                        param: new { lineCD = queryParameters[0], postCD = queryParameters[1], ACS_IF_Flag = "N" }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                    return null;
                }
            }
        }

        public void Remove(PopModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute($"DELETE FROM {tableName} WHERE Id=@id",
                    param: new { id = model.Id });
                logger.Info($"Pop Remove: {model}");
            }
        }

        //public void RemoveById(int id)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute($"DELETE FROM {tableName} WHERE Id=@id",
        //            param: new { id = id });
        //    }
        //}

        //public void RemoveByIds(params int[] ids)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute($"DELETE FROM {tableName} WHERE Id IN @ids",
        //            param: new { ids = ids });
        //    }
        //}

        public void Update(PopModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                string query = $@"
                    UPDATE {tableName} 
                    SET 
                        Line_Cd=@Line_Cd, 
                        Post_Cd=@Post_Cd, 
                        COMM_PO=@COMM_PO, 
                        COMM_ANG=@COMM_ANG, 
                        RETU_TYPE=@RETU_TYPE, 
                        POST_STA=@POST_STA, 
                        ACS_IF_Flag=@ACS_IF_Flag, 
                        CREATE_DT=@CREATE_DT 
                    WHERE Id=@Id";

                try
                {
                    con.Execute(query, param: model);
                    logger.Info($"Pop Update: {model}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                }
            }
        }

        public void Update_AcsFlag(PopModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    //con.Execute($"UPDATE {tableName} SET ACS_IF_Flag=@acsflag WHERE Id=@id", param: new { acsflag = model.ACS_IF_Flag, id = model.Id });
                    //logger.Info($"Pop Update_AcsFlag: {model}");

                    //con.Execute($"UPDATE {tableName} SET ACS_IF_Flag=@acsflag WHERE LINE_CD=@lineCD AND POST_CD=@postCD AND CREATE_DT=@createDT",
                    //    param: new { acsflag = model.ACS_IF_Flag, lineCD = model.LINE_CD, postCD = model.POST_CD, createDT = model.CREATE_DT });

                    con.Execute($"UPDATE {tableName} SET ACS_IF_Flag=@acsflag WHERE LINE_CD=@lineCD AND POST_CD=@postCD",
                        param: new { acsflag = model.ACS_IF_Flag, lineCD = model.LINE_CD, postCD = model.POST_CD });

                    //logger.Info($"Pop Update_AcsFlag: {model}");
                    logger.Info($"PopAcsFlag: {model}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                }
            }
        }

        public void DeleteOldData()
        {
            using (var con = new SqlConnection(connectionString))
            {
                // 7일전 데이터 모두 삭제
                string query = $@"DELETE FROM {tableName} WHERE CREATE_DT < Convert(varchar(14),DATEADD(DAY, -7, GETDATE()),23)";

                try
                {
                    con.Execute(query);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                }
            }
        }


        //public void RemoveMultiple(List<PopItem> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute($"DELETE FROM {tableName} WHERE Id IN @ids",
        //            param: new { ids = pops.Select(p => p.Id) });
        //    }
        //}

        //public void UpdateMultiple_AcsFlag(List<PopItem> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute($"UPDATE {tableName} SET ACS_IF_Flag=@ACS_IF_Flag WHERE Id=@id",
        //            param: pops);
        //    }
        //}

        //public void UpdateMultiple_AcsFlag_To_YesOrNo(string acsflag, List<PopItem> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute($"UPDATE {tableName} SET ACS_IF_Flag=@acsflag WHERE Id IN @ids",
        //            param: new { acsflag = acsflag, ids = pops.Select(p => p.Id) });
        //    }
        //}

        //public IEnumerable<int> DeleteCanceledOrders()
        //{
        //    string query = $@"DELETE FROM Orders
        //                    OUTPUT DELETED.OrderId
        //                    WHERE [Status]='Canceled'";

        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        var deletedOrderIds = con.Query<int>(query);
        //        return deletedOrderIds;
        //    }
        //}
    }
}
