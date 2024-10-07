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
    public class EquipmentOrderRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("DBCallEvent");

        private readonly IDbConnection db;
        private readonly string connectionString = null;




        public EquipmentOrderRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public EquipmentOrder Add(EquipmentOrder model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO [dbo].[EquipmentOrders]
                               ([Id]
                               ,[EQP_NAME]
                               ,[COMMAND]
                               ,[INCH_TYPE]
                               ,[IF_FLAG]
                               ,[CREATE_DT]
                               ,[MODIFY_DT])
                         VALUES
                               (@Id
                               ,@EQP_NAME
                               ,@COMMAND
                               ,@INCH_TYPE
                               ,@IF_FLAG
                               ,@CREATE_DT
                               ,@MODIFY_DT);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                //int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //model.Id = id;
                //return model;

                // ID 필드가 자동증가값이 아니므로 직접 할당...
                int maxId = con.QueryFirstOrDefault<int>("SELECT MAX(Id) FROM EquipmentOrders");
                model.Id = maxId + 1;
                con.ExecuteScalar<int>(INSERT_SQL, param: model);
                return model;

            }
        }

        public List<EquipmentOrder> GetAll()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<EquipmentOrder>("SELECT * FROM EquipmentOrders").ToList();
            }
        }

        public List<EquipmentOrder> GetAll_With_Flag_N()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<EquipmentOrder>("SELECT * FROM EquipmentOrders WHERE IF_FLAG='N'").ToList();
            }
        }

        public EquipmentOrder GetById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                return con.Query<EquipmentOrder>("SELECT * FROM EquipmentOrders WHERE Id=@id",
                    param: new { id = id }).FirstOrDefault();
            }
        }

        public void Remove(EquipmentOrder model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM EquipmentOrders WHERE Id=@id",
                    param: new { id = model.Id });
            }
        }

        //public void RemoveById(int id)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM EquipmentOrders WHERE Id=@id",
        //            param: new { id = id });
        //    }
        //}

        //public void RemoveByIds(params int[] ids)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("DELETE FROM EquipmentOrders WHERE Id IN @ids",
        //            param: new { ids = ids });
        //    }
        //}

        public void Update(EquipmentOrder model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string query = @"
                    UPDATE EquipmentOrders
                    SET  EQP_NAME   = @EQP_NAME
                        ,COMMAND    = @COMMAND
                        ,INCH_TYPE  = @INCH_TYPE
                        ,IF_FLAG    = @IF_FLAG
                        ,CREATE_DT  = @CREATE_DT
                        ,MODIFY_DT  = @MODIFY_DT
                    WHERE Id=@Id";

                try
                {
                    con.Execute(query, param: model);
                    logger.Info($"DBCall_Update: {model}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    logger.Info(ex.Message);
                }
            }
        }

        public void UpdateFlag(EquipmentOrder model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Execute("UPDATE EquipmentOrders SET IF_FLAG=@if_flag WHERE Id=@id", param: new { if_flag = model.IF_FLAG, id = model.Id });
                    logger.Info($"DBCall_UpdateFlag: {model}");
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
                string query = $@"DELETE FROM EquipmentOrders WHERE CREATE_DT < Convert(varchar(14),DATEADD(DAY, -7, GETDATE()),23)";

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
