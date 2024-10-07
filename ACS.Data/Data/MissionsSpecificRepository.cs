using Dapper;
using log4net;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace INA_ACS_Server
{
    public class MissionsSpecificRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("Missions_Specific");
        private readonly string connectionString = null;

        public MissionsSpecificRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MissionsSpecific Add(MissionsSpecific model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string INSERT_SQL = @"
                    INSERT INTO Missions_Specific
                               ([CallName]
                               ,[CallState]
                               ,[CallTime]
                               ,[JobSection]
                               ,[Priority])
                           VALUES
                               (@CallName
                               ,@CallState
                               ,@CallTime
                               ,@JobSection
                               ,@Priority);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                con.ExecuteScalar<int>(INSERT_SQL, param: model);
                logger.Info($"MissionSpecific Add   : {model}");
                return model;
            }
        }

        public void Update(MissionsSpecific model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string UPDATE_SQL = @"
                    UPDATE Missions_Specific 
                    SET 
                        RobotName=@RobotName,
                        RobotAlias=@RobotAlias,
                        CallState=@CallState
                    WHERE No=@No";

                con.Execute(UPDATE_SQL, param: model);
                logger.Info($"MissionsSpecific Update: {model}");
            }
        }

        public void CallState_Update(MissionsSpecific model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string UPDATE_SQL = @"
                    UPDATE Missions_Specific 
                    SET 
                        CallState=@CallState
                    WHERE RobotName=@RobotName";

                con.Execute(UPDATE_SQL, param: model);
                logger.Info($"MissionsSpecific Update: {model}");
            }
        }

        public void MoveCallName_Update(MissionsSpecific model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string UPDATE_SQL = @"
                    UPDATE Missions_Specific 
                    SET 
                        Move_CallName=@Move_CallName
                    WHERE No=@No";

                con.Execute(UPDATE_SQL, param: model);
                logger.Info($"MissionsSpecific Update: {model}");
            }
        }

        public IEnumerable<MissionsSpecific> GetAll(int i)
        {
            using (var con = new SqlConnection(connectionString))
            {
                if (i == 0)
                    return con.Query<MissionsSpecific>("SELECT * FROM Missions_Specific order by Priority DESC, CallTime ASC");
                else if (i == 1)
                    return con.Query<MissionsSpecific>("SELECT * FROM Missions_Specific order by CallTime ASC");
                else if (i == 2)
                    return con.Query<MissionsSpecific>("SELECT * FROM Missions_Specific order by CallName ASC, CallTime ASC");
                else
                    return con.Query<MissionsSpecific>("SELECT * FROM Missions_Specific order by Priority DESC, CallTime ASC");
            }
        }

        public void Remove(MissionsSpecific model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM Missions_Specific WHERE No=@No", param: new { No = model.No });
                logger.Info($"Missions_Specific Remove: {model}");
            }
        }
    }
}
