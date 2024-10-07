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
    public class WmsModelRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;


        
        
        public WmsModelRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public WmsModel Add(WmsModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO [dbo].[WMSModels]
                               ([LINE_CD]
                               ,[POST_CD]
                               ,[COMM_ANG]
                               ,[RETU_TYPE]
                               ,[OUT_Q]
                               ,[PART_CD]
                               ,[PART_NM]
                               ,[OUT_WH]
                               ,[OUT_POINT]
                               ,[WMS_IF_FLAG]
                               ,[CREATE_DT]
                               ,[MODIFY_DT])
                         VALUES
                               (@LINE_CD
                               ,@POST_CD
                               ,@COMM_ANG
                               ,@RETU_TYPE
                               ,@OUT_Q
                               ,@PART_CD
                               ,@PART_NM
                               ,@OUT_WH
                               ,@OUT_POINT
                               ,@WMS_IF_FLAG
                               ,@CREATE_DT
                               ,@MODIFY_DT);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                model.Id = id;
                return model;
            }
        }

        public List<WmsModel> GetAll()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<WmsModel>("SELECT * FROM WMSModels").ToList();
            }
        }

        public List<WmsModel> GetAll_With_Flag_N()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<WmsModel>("SELECT * FROM WMSModels WHERE WMS_IF_FLAG='N'").ToList();
            }
        }

        public WmsModel GetById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<WmsModel>("SELECT * FROM WMSModels WHERE Id=@id", 
                    param: new { id = id }).FirstOrDefault();
            }
        }

        public void Remove(WmsModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM WMSModels WHERE Id=@id",
                    param: new { id = model.Id });
            }
        }

        //public void RemoveById(int id)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM WMSModels WHERE Id=@id",
        //            param: new { id = id });
        //    }
        //}

        //public void RemoveByIds(params int[] ids)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM WMSModels WHERE Id IN @ids",
        //            param: new { ids = ids });
        //    }
        //}

        public void Update(WmsModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string query = @"
                    UPDATE WMSModels
                       SET LINE_CD     = @LINE_CD   
                          ,POST_CD     = @POST_CD   
                          ,COMM_ANG    = @COMM_ANG  
                          ,RETU_TYPE   = @RETU_TYPE 
                          ,OUT_Q       = @OUT_Q     
                          ,PART_CD     = @PART_CD   
                          ,PART_NM     = @PART_NM   
                          ,OUT_WH      = @OUT_WH    
                          ,OUT_POINT   = @OUT_POINT 
                          ,WMS_IF_FLAG = @WMS_IF_FLAG
                          ,CREATE_DT   = @CREATE_DT 
                          ,MODIFY_DT   = @MODIFY_DT 
                    WHERE Id=@Id";

                con.Execute(query, param: model);
            }
        }

        public void Update_WmsFlag(WmsModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("UPDATE WMSModels SET WMS_IF_Flag=@wmsflag WHERE Id=@id",
                    param: new { wmsflag = model.WMS_IF_FLAG, id = model.Id });
            }
        }

        //public void RemoveMultiple(List<WmsModel> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM WMSModels WHERE Id IN @ids",
        //            param: new { ids = pops.Select(p => p.Id) });
        //    }
        //}

        //public void UpdateMultiple_AcsFlag(List<WmsModel> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE WMSModels SET ACS_IF_Flag=@ACS_IF_Flag WHERE Id=@id",
        //            param: pops);
        //    }
        //}

        //public void UpdateMultiple_AcsFlag_To_YesOrNo(string acsflag, List<WmsModel> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE WMSModels SET ACS_IF_Flag=@acsflag WHERE Id IN @ids",
        //            param: new { acsflag = acsflag, ids = pops.Select(p => p.Id) });
        //    }
        //}

        //public IEnumerable<int> DeleteCanceledOrders()
        //{
        //    string query = @"DELETE FROM Orders
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
