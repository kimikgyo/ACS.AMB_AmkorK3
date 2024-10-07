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
    public class ErrorCodeListRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<ErrorCodeListModel> _errorCodeLists = new List<ErrorCodeListModel>();

        public ErrorCodeListRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Load();
        }

        private void Load()
        {
            lock (this)
            {
                _errorCodeLists.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    foreach (var errorCodeListModel in con.Query<ErrorCodeListModel>("SELECT * FROM ErrorCodeList"))
                        _errorCodeLists.Add(errorCodeListModel);
                }
            }
        }

        public List<ErrorCodeListModel> GetAll()
        {
            lock (this)
            {
                return _errorCodeLists;
            }
        }

        public List<ErrorCodeListModel> Find(Func<ErrorCodeListModel, bool> predicate)
        {
            lock (this)
            {
                return _errorCodeLists.Where(predicate).ToList();
            }
        }

        public void Update(ErrorCodeListModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string query = @"
                    UPDATE ErrorCodeList 
                    SET 
                       ErrorCode=@ErrorCode,
                       ErrorMessage=@ErrorMessage, 
                       ErrorType=@ErrorType, 
                       Explanation=@Explanation,
                       MailSend=@MailSend
                    WHERE Id=@Id";

                con.Execute(query, param: model);
            }
        }

        public void Remove(ErrorCodeListModel model)
        {
            lock (this)
            {
                _errorCodeLists.Remove(model);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ErrorCodeList WHERE Id=@id",
                        param: new { id = model.Id });
                }
            }
        }

        public void Delete()
        {
            lock (this)
            {
                _errorCodeLists.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM ErrorCodeList");
                }
            }
        }
    }
}
