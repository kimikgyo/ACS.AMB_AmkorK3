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
    public class ProductNameInfoRepository
    {
        //private readonly static ILog logger = LogManager.GetLogger("User");

        private readonly IDbConnection db;
        private readonly string connectionString = null;

        private readonly List<ProductNameInfoModel> _productNameInfoModel = new List<ProductNameInfoModel>(); // cache data


        public ProductNameInfoRepository(string connectionString)
        {
            this.connectionString = connectionString;
            //Load();
            Validate_DB_Items();
        }
        public void Validate_DB_Items()
        {

            var productNameInfoModel = new List<ProductNameInfoModel>();

            for (int i = 0; i < ConfigData.ProductNameInfo_MaxNum; i++)
            {
                int probuctNameInfoIndex = i + 1;
                var config = GetByEtnLampConfigIndex_Ignore_CountFlag(probuctNameInfoIndex);
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
                    config = new ProductNameInfoModel
                    {
                       ProductName = "None",
                       Regiser22Vlaue = 0,
                       RegisterNo = "0",
                       RegisterValue = 0,
                       DisplayFlag = 1
                    };
                    Add(config);
                }
                productNameInfoModel.Add(config);
            }
            Update_DisplayFlags_Except_For(productNameInfoModel);
            Load();

            ProductNameInfoModel GetByEtnLampConfigIndex_Ignore_CountFlag(int etnLampIndex)
            {
                lock (this)
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        return con.Query<ProductNameInfoModel>("SELECT * FROM ProductNameInfoModel WHERE Id=@index",
                            param: new { index = etnLampIndex }).FirstOrDefault();
                    }
                }
            }

            void Update_DisplayFlags_Except_For(List<ProductNameInfoModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE ProductNameInfoModel SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
        }
        private void Load()
        {
            _productNameInfoModel.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var productNameInfo in con.Query<ProductNameInfoModel>("SELECT * FROM ProductNameInfoModel WHERE DisplayFlag=1"))
                {

                    _productNameInfoModel.Add(productNameInfo);
                }
            }
        }
        //DB 추가하기
        public ProductNameInfoModel Add(ProductNameInfoModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ProductNameInfoModel
                                ([Regiser22Vlaue]
                                ,[RegisterNo]
                                ,[RegisterValue]
                                ,[ProductName]
                                ,[DisplayFlag])
                           VALUES
                                (@Regiser22Vlaue
                                ,@RegisterNo
                                ,@RegisterValue
                                ,@ProductName
                                ,@DisplayFlag);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                //logger.Info($"PositionAreaConfig Add   : {model}");
                return model;
            }
        }

        //DB찾기
        public List<ProductNameInfoModel> Find(Func<ProductNameInfoModel, bool> predicate)
        {
            lock (this)
            {
                return _productNameInfoModel.Where(predicate).ToList();
            }
        }

        //public List<ProductNameInfoModel> GetDisplayFlagtrueData()
        //{
        //    lock (this)
        //    {
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            return con.Query<ProductNameInfoModel>("SELECT * FROM ProductNameInfoModel WHERE DisplayFlag=1").ToList();

        //        }
        //    }
        //}


        public IList<ProductNameInfoModel> GetAll() => _productNameInfoModel;






        //DB업데이트
        public void Update(ProductNameInfoModel model)
        {
            lock (this)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    const string UPDATE_SQL = @"
                    UPDATE ProductNameInfoModel 
                    SET 
                        Regiser22Vlaue=@Regiser22Vlaue, 
                        RegisterNo=@RegisterNo, 
                        RegisterValue=@RegisterValue, 
                        ProductName=@ProductName, 
                        DisplayFlag=@DisplayFlag
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);

                }
            }
        }

        //DB삭제
        public void Remove(ProductNameInfoModel model)
        {
            lock (this)
            {
                _productNameInfoModel.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ProductNameInfoModel WHERE Name LIKE @Name",
                        param: new { id = model.Id });
                }
            }
        }
    }
}
