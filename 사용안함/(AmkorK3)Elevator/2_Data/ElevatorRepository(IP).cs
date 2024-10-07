using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp9
{
    public class ElevatorRepository
    {
        private readonly string connectionString = null;

        public ElevatorRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<ElevatorModel> Load()
        {
            using (var con = new SqlConnection(connectionString))
            {
                return (List<ElevatorModel>) con.Query<ElevatorModel>("SELECT * FROM ElevatorInfo");
            }
        }

        public ElevatorModel Add(ElevatorModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO ElevatorInfo
                                ([IP]
                                ,[Port]
                                ,[Alias])
                           VALUES
                                (@IP
                                ,@Port
                                ,@Alias);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                model.No = con.ExecuteScalar<int>(INSERT_SQL, param: model);

                return model;
            }
        }

        public void Remove(ElevatorModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM ElevatorInfo WHERE IP=@IP AND Port=@Port",
                    param: new { IP = model.IP, Port = model.Port });
            }
        }
    }
}
