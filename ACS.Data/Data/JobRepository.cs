using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class JobRepository
    {
        private readonly static ILog logger = LogManager.GetLogger("JobEvent");

        private readonly IDbConnection db;
        private readonly string connectionString;
        private readonly List<Job> _jobs = new List<Job>(); // cached data

        public bool NeedUpdateUI { get; set; }

        public JobRepository(string connectionString, MissionRepository missions)
        {
            this.connectionString = connectionString;
            Load(missions);
        }

        // DB에서 모든 항목을 로드하여 _jobs 에 캐싱해 둔다
        private void Load(MissionRepository missions)
        {
            _jobs.Clear();

            // DB에서 읽어와서 캐싱해 둔다
            using (var con = new SqlConnection(connectionString))
            {
                //IEnumerable<Mission> missions = con.Query<Mission>("SELECT * FROM Missions");

                // 각 job에 대해 missionid/mission 을 수동으로 맵핑해준다
                foreach (var job in con.Query<Job>("SELECT * FROM Jobs"))
                {
                    // mission id 맵핑
                    job.MissionIds[0] = job.MissionId1;
                    job.MissionIds[1] = job.MissionId2;
                    job.MissionIds[2] = job.MissionId3;
                    job.MissionIds[3] = job.MissionId4;
                    job.MissionIds[4] = job.MissionId5;

                    // job 에 해당하는 mission을 job.Missions에 추가
                    job.Missions = new List<Mission>();
                    foreach (int missionId in job.MissionIds.ToList().TakeWhile(x => x != 0)) // 맵핑도중 mission id == 0 인 경우 처리 중단한다
                    {
                        //var mission = missions.GetAll().SingleOrDefault(m => m.Id == missionId);
                        var mission = missions.GetById(missionId);
                        if (mission != null)
                        {
                            job.Missions.Add(mission);
                        }
                    }

                    _jobs.Add(job);
                }
            }

            NeedUpdateUI = true;
        }

        public Job Add(Job model)
        {
            lock (this)
            {
                model.MissionId1 = model.MissionIds[0];
                model.MissionId2 = model.MissionIds[1];
                model.MissionId3 = model.MissionIds[2];
                model.MissionId4 = model.MissionIds[3];
                model.MissionId5 = model.MissionIds[4];

                model.JobStateText = model.JobState.ToString();

                _jobs.Add(model);
                NeedUpdateUI = true;

                using (var con = new SqlConnection(connectionString))
                {
                    const string INSERT_SQL = @"
                    INSERT INTO Jobs
                               ([Name]
                               ,[CallName]
                               ,[PopServerId]
                               ,[PopCallReturnType]
                               ,[PopCallAngle]
                               ,[WmsId]
                               ,[PopCallState]
                               ,[ACSJobGroup]
                               ,[JobCreateRobotName]
                               ,[RobotName]
                               ,[ExecuteBattery]
                               ,[JobState]
                               ,[JobStateText]
                               ,[JobCreateTime]
                               ,[MissionTotalCount]
                               ,[MissionSentCount]
                               ,[MissionId1]
                               ,[MissionId2]
                               ,[MissionId3]
                               ,[MissionId4]
                               ,[MissionId5])

                           VALUES
                               (@Name
                               ,@CallName
                               ,@PopServerId
                               ,@PopCallReturnType
                               ,@PopCallAngle
                               ,@WmsId
                               ,@PopCallState
                               ,@ACSJobGroup
                               ,@JobCreateRobotName
                               ,@RobotName
                               ,@ExecuteBattery
                               ,@JobState
                               ,@JobStateText
                               ,@JobCreateTime
                               ,@MissionTotalCount
                               ,@MissionSentCount
                               ,@MissionId1
                               ,@MissionId2
                               ,@MissionId3
                               ,@MissionId4
                               ,@MissionId5);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                    model.Id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                    logger.Info($"Job Add   : {model}");
                    return model;
                }
            }
        }

        public List<Job> Find(Func<Job, bool> predicate)
        {
            lock (this)
            {
                return _jobs.Where(predicate).ToList();
            }
        }

        public List<Job> GetAll()
        {
            lock (this)
            {
                return _jobs.ToList();
            }
        }

        public Job GetById(int id)
        {
            lock (this)
            {
                return _jobs.SingleOrDefault(x => x.Id == id);
            }
        }

        public Job GetByCreateRobot(string RobotName)
        {
            lock (this)
            {
                return _jobs.SingleOrDefault(x => x.JobCreateRobotName == RobotName || x.RobotName == RobotName);
            }
        }
        public Job GetByCallName(string callName)
        {
            lock (this)
            {
                return _jobs.FirstOrDefault(x => x.CallName == callName);
            }
        }

        public void Remove(Job model)
        {
            lock (this)
            {
                model.Missions.Clear(); // job.Missions 에서 모든 항목을 제거한다
                _jobs.Remove(model);    // job 자체 제거한다
                NeedUpdateUI = true;

                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Jobs WHERE Id=@id", 
                        param: new { id = model.Id });
                    logger.Info($"Job Remove: {model}");
                }
            }
        }

        public void Update(Job model)
        {
            lock (this)
            {
                NeedUpdateUI = true;

                using (var con = new SqlConnection(connectionString))
                {
                    model.MissionId1 = model.MissionIds[0];
                    model.MissionId2 = model.MissionIds[1];
                    model.MissionId3 = model.MissionIds[2];
                    model.MissionId4 = model.MissionIds[3];
                    model.MissionId5 = model.MissionIds[4];

                    model.JobStateText = model.JobState.ToString();

                    const string UPDATE_SQL = @"
                     UPDATE Jobs 
                    SET 
                        Name=@Name, 
                        CallName=@CallName, 
                        PopServerId=@PopServerId, 
                        PopCallReturnType=@PopCallReturnType, 
                        PopCallAngle=@PopCallAngle, 
                        WmsId=@WmsId, 
                        PopCallState=@PopCallState, 
                        ACSJobGroup=@ACSJobGroup, 
                        JobCreateRobotName=@JobCreateRobotName,
                        RobotName=@RobotName, 
                        ExecuteBattery=@ExecuteBattery, 
                        JobState=@JobState, 
                        JobStateText=@JobStateText, 
                        JobCreateTime=@JobCreateTime, 
                        MissionTotalCount=@MissionTotalCount, 
                        MissionSentCount=@MissionSentCount, 
                        MissionId1=@MissionId1, 
                        MissionId2=@MissionId2, 
                        MissionId3=@MissionId3, 
                        MissionId4=@MissionId4, 
                        MissionId5=@MissionId5
                    WHERE Id=@Id";

                    con.Execute(UPDATE_SQL, param: model);
                    logger.Info($"Job Update: {model}");
                }
            }
        }
        public void Delete()
        {
            lock (this)
            {
                _jobs.Clear();
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("DELETE FROM Jobs");
                }
            }
        }
        //public void UpdateState(Job model)
        //{
        //    Job JobToUpdate = _jobs.SingleOrDefault(m => m.Id == model.Id);
        //    Job.JobState = model.JobState;
        //    NeedUpdateUI = true;

        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE Jobs SET JobState=@JobState WHERE Id=@id",
        //            param: new { JobState = model.JobState, id = model.Id });
        //    }
        //}
    }
}
