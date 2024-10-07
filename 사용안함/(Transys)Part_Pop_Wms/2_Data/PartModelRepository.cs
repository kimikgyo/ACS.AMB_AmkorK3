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
    public class PartModelRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;


        
        
        public PartModelRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public PartModel Add(PartModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO PartModels
                               ([LINE_CD]
                               ,[POST_CD]
                               ,[COMM_PO]
                               ,[OUT_Q]
                               ,[COMM_ANG]
                               ,[PART_CD]
                               ,[PART_NM]
                               ,[NP_MODE]
                               ,[NP_OUT_Q]
                               ,[NP_PART_CD]
                               ,[NP_PART_NM])
                         VALUES
                               (@LINE_CD
                               ,@POST_CD
                               ,@COMM_PO
                               ,@OUT_Q
                               ,@COMM_ANG
                               ,@PART_CD
                               ,@PART_NM
                               ,@NP_MODE
                               ,@NP_OUT_Q
                               ,@NP_PART_CD
                               ,@NP_PART_NM);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                model.Id = id;
                return model;
            }
        }

        public List<PartModel> GetAll()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<PartModel>("SELECT * FROM PartModels").ToList();
            }
        }

        public PartModel GetById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<PartModel>("SELECT * FROM PartModels WHERE Id=@id", 
                    param: new { id = id }).FirstOrDefault();
            }
        }

        public void Remove(PartModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM PartModels WHERE Id=@id",
                    param: new { id = model.Id });
            }
        }

        //public void RemoveById(int id)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM PartModels WHERE Id=@id",
        //            param: new { id = id });
        //    }
        //}

        //public void RemoveByIds(params int[] ids)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM PartModels WHERE Id IN @ids",
        //            param: new { ids = ids });
        //    }
        //}

        public void Update(PartModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string query = @"
                    UPDATE PartModels
                    SET 
                        LINE_CD    = @LINE_CD
                       ,POST_CD    = @POST_CD
                       ,COMM_PO    = @COMM_PO
                       ,OUT_Q      = @OUT_Q
                       ,COMM_ANG   = @COMM_ANG
                       ,PART_CD    = @PART_CD
                       ,PART_NM    = @PART_NM
                       ,NP_MODE    = @NP_MODE
                       ,NP_OUT_Q   = @NP_OUT_Q
                       ,NP_PART_CD = @NP_PART_CD
                       ,NP_PART_NM = @NP_PART_NM
                    WHERE Id=@Id";

                con.Execute(query, param: model);
            }
        }

        //public void Update_AcsFlag(PartModel model)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE PartModels SET ACS_IF_Flag=@acsflag WHERE Id=@id",
        //            param: new { acsflag = model.ACS_IF_Flag, id = model.Id });
        //    }
        //}

        //public void RemoveMultiple(List<PartModel> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM PartModels WHERE Id IN @ids",
        //            param: new { ids = pops.Select(p => p.Id) });
        //    }
        //}

        //public void UpdateMultiple_AcsFlag(List<PartModel> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE PartModels SET ACS_IF_Flag=@ACS_IF_Flag WHERE Id=@id",
        //            param: pops);
        //    }
        //}

        //public void UpdateMultiple_AcsFlag_To_YesOrNo(string acsflag, List<PartModel> pops)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE PartModels SET ACS_IF_Flag=@acsflag WHERE Id IN @ids",
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
