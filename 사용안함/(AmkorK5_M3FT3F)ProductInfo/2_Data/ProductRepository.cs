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
    public class ProductRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;

        public ProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ProductModel Add(ProductModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string INSERT_SQL = @"
                    INSERT INTO Products (
                                 [CreateTime]
                                ,[Barcode]
                                ,[RobotName]
                                ,[ProductName]
                                ,[Qty]
                                ,[Info1]
                                ,[Info2]
                                ,[Info3]
                                ,[Info4]
                    ) VALUES (
                                @CreateTime
                               ,@Barcode
                               ,@RobotName
                               ,@ProductName
                               ,@Qty
                               ,@Info1
                               ,@Info2
                               ,@Info3
                               ,@Info4
                    );
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                    int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                    model.Id = id;
                    return model;
                }
            }
        }

        public List<ProductModel> GetAll()
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ProductModel>("SELECT * FROM Products").ToList();
                }
            }
        }

        //DB찾기
        public IEnumerable<ProductModel> Find(Func<ProductModel, bool> predicate)
        {
            lock (this)
            {

                using (var con = new SqlConnection(connectionString))
                {
                    var result = con.Query<ProductModel>("SELECT * FROM Products");
                    return result.Where(predicate);
                }
            }
        }
        public ProductModel GetById(int id)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ProductModel>("SELECT * FROM Products WHERE Id=@id",
                        param: new { id = id }).FirstOrDefault();
                }
            }
        }

        public List<ProductModel> Get10ProductsByRobotName(string robotName)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<ProductModel>("SELECT TOP 10 * FROM Products WHERE RobotName=@robotName",
                        param: new { robotName = robotName }).ToList();
                }
            }
        }

        public void Remove(ProductModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Products WHERE Id=@id",
                        param: new { id = model.Id });
                }
            }
        }

        public void Update(ProductModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string query = @"
                    UPDATE Products
                    SET 
                        CreateTime  = @CreateTime  ,
                        Barcode     = @Barcode     ,
                        RobotName   = @RobotName   ,
                        ProductName = @ProductName ,
                        Qty         = @Qty         ,
                        Info1       = @Info1       ,
                        Info2       = @Info2       ,
                        Info3       = @Info3       ,
                        Info4       = @Info4      
                    WHERE Id=@Id";

                    con.Execute(query, param: model);
                }
            }
        }

    }
}
