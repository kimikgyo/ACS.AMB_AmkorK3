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
    public class JobConfigRepository
    {
        private readonly IDbConnection db;
        private readonly string connectionString = null;
        private readonly List<JobConfigModel> _jobConfigs = new List<JobConfigModel>(); // cached data


        public JobConfigRepository(string connectionString)
        {
            this.connectionString = connectionString;
            Validate_DB_Items();
        }

        private void Load()
        {
            _jobConfigs.Clear();
            using (var con = new SqlConnection(connectionString))
            {
                foreach (var missionConfigs in con.Query<JobConfigModel>("SELECT * FROM JobConfigs WHERE DisplayFlag=1"))
                    _jobConfigs.Add(missionConfigs);
            }
        }

        // DB 항목 개수를 CallButton 개수에 맞게 추가/갱신해 준다
        public void Validate_DB_Items()
        {
            var usingConfigs = new List<JobConfigModel>();

            //for (int i = 0; i < ConfigData.CallButton_MaxNum; i++)
            for (int i = 0; i < ConfigData.JobConfig_MaxNum; i++)
            {
                int jobConfigIndex = i + 1;
                var config = GetByCallButtonIndex_Ignore_UseFlag(jobConfigIndex);
                if (config != null) // DB에 있으면 flag 체크한다 (set UseFlag=1)
                {
                    if (config.DisplayFlag != 1)
                    {
                        config.DisplayFlag = 1;
                        Update(config);
                    }
                }
                else // DB에 없으면 추가한다 (set UseFlag=1)
                {
                    //Setting 변경시에 미션전송이 되는 증상발견 Use 부분 추가!!
                    config = new JobConfigModel
                    {
                        ACSMissionGroup = "None",
                        CallName = "None_None",
                        JobConfigUse = "Unuse",
                        ExecuteBattery = 0,
                        ProductValue = false,
                        ProductActive = false,
                        ElevatorModeActive = false,
                        TransportCountActive = false,
                        MissionName = "",
                        ErrorMissionName = "",
                        DisplayFlag = 1,
                        SourceFloor = "Node",
                        DestFloor = "None",
                    };
                    Add(config);
                }

                // 사용할 항목들 임시 리스트에 저장한다
                usingConfigs.Add(config);
            }

            // 위 항목들 외 나머지는 모두 UseFlag=0로 설정한다
            Update_UseFlags_Except_For(usingConfigs);
            Load();


            // =============== local functions ===============
            JobConfigModel GetByCallButtonIndex_Ignore_UseFlag(int missionConfigIndex)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    return con.Query<JobConfigModel>("SELECT * FROM JobConfigs WHERE Id=@index",
                        param: new { index = missionConfigIndex }).FirstOrDefault();
                }
            }
            void Update_UseFlags_Except_For(List<JobConfigModel> someConfigs)
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Execute("UPDATE JobConfigs SET DisplayFlag=0 WHERE Id NOT IN @ids",
                        param: new { ids = someConfigs.Select(c => c.Id) });
                }
            }
            // =============== local functions ===============
        }

        public JobConfigModel Add(JobConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                _jobConfigs.Add(model);

                const string INSERT_SQL = @"
                    INSERT INTO JobConfigs
                               ([ACSMissionGroup]
                               ,[CallName]
                               ,[JobConfigUse]
                               ,[JobMissionName1]
                               ,[JobMissionName2]
                               ,[JobMissionName3]
                               ,[JobMissionName4]
                               ,[JobMissionName5]
                               ,[ExecuteBattery]
                               ,[jobCallSignal]
                               ,[JobPriority]
                               ,[ProductActive]
                               ,[ProductValue]
                               ,[ElevatorModeActive]
                               ,[ElevatorModeValue]
                               ,[TransportCountActive]
                               ,[DisplayFlag]
                               ,[MissionName]
                               ,[ErrorMissionName]
                               ,[DestFloor])
                           VALUES
                               (@ACSMissionGroup
                               ,@CallName
                               ,@JobConfigUse
                               ,@JobMissionName1
                               ,@JobMissionName2
                               ,@JobMissionName3
                               ,@JobMissionName4
                               ,@JobMissionName5
                               ,@ExecuteBattery
                               ,@jobCallSignal
                               ,@JobPriority
                               ,@ProductActive
                               ,@ProductValue
                               ,@ElevatorModeActive
                               ,@ElevatorModeValue
                               ,@TransportCountActive
                               ,@DisplayFlag
                               ,@MissionName
                               ,@ErrorMissionName
                               ,@DestFloor);
                    SELECT Cast(SCOPE_IDENTITY() As Int);";

                int id = con.ExecuteScalar<int>(INSERT_SQL, param: model);
                model.Id = id;
                return model;
            }
        }

        public List<JobConfigModel> GetAll()
        {
            return _jobConfigs.ToList();
            //using (var con = new SqlConnection(connectionString))
            //{
            //    return con.Query<JobConfigModel>("SELECT * FROM JobConfigs WHERE UseFlag=1").ToList();
            //}
        }

        public JobConfigModel GetById(int id)
        {
            return _jobConfigs.SingleOrDefault(x => x.Id == id);
            //using (var con = new SqlConnection(connectionString))
            //{
            //    return con.Query<JobConfigModel>("SELECT * FROM JobConfigs WHERE Id=@id AND UseFlag=1",
            //        param: new { id = id }).FirstOrDefault();
            //}
        }

        //public MissionConfig GetByCallButtonIndex(int callButtonIndex)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        return con.Query<MissionConfig>("SELECT * FROM MissionConfigs WHERE CallButtonIndex=@index AND UseFlag=1",
        //            param: new { index = callButtonIndex }).FirstOrDefault();
        //    }
        //}

        //public JobConfigModel GetByCallRegistar(string RegistarNo, string RegistarValue)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        return con.Query<JobConfigModel>("SELECT * FROM JobConfigs WHERE RegistarNo=@No AND RegistarNo=@No AND UseFlag=1",
        //            param: new { No = RegistarNo, Value = RegistarValue }).FirstOrDefault();
        //    }
        //}

        public JobConfigModel GetByCallName(string aCSMissionName)
        {
            return _jobConfigs.FirstOrDefault(x => x.CallName == aCSMissionName);
            //using (var con = new SqlConnection(connectionString))
            //{
            //    return con.Query<JobConfigModel>("SELECT * FROM JobConfigs WHERE CallName=@name AND UseFlag=1",
            //        param: new { name = aCSMissionName }).FirstOrDefault();
            //}
        }

        public void Remove(JobConfigModel model)
        {
            _jobConfigs.Remove(model);

            using (var con = new SqlConnection(connectionString))
            {
                con.Execute("DELETE FROM JobConfigs WHERE Id=@id AND DisplayFlag=1",
                    param: new { id = model.Id });
            }
        }

        public void Update(JobConfigModel model)
        {
            using (var con = new SqlConnection(connectionString))
            {
                const string query = @"
                    UPDATE JobConfigs 
                    SET 
                       ACSMissionGroup=@ACSMissionGroup,
                       CallName=@CallName, 
                       JobConfigUse=@JobConfigUse, 
                       JobMissionName1=@JobMissionName1, 
                       JobMissionName2=@JobMissionName2, 
                       JobMissionName3=@JobMissionName3, 
                       JobMissionName4=@JobMissionName4, 
                       JobMissionName5=@JobMissionName5,
                       ExecuteBattery=@ExecuteBattery,
                       jobCallSignal=@jobCallSignal,
                       JobPriority=@JobPriority,
                       ProductValue=@ProductValue,
                       ProductActive=@ProductActive,
                       ElevatorModeValue=@ElevatorModeValue,
                       ElevatorModeActive=@ElevatorModeActive,
                       TransportCountActive=@TransportCountActive,
                       DisplayFlag=@DisplayFlag, 
                       MissionName=@MissionName, 
                       ErrorMissionName=@ErrorMissionName,
                       SourceFloor=@SourceFloor,
                       DestFloor=@DestFloor
                    WHERE Id=@Id";

                con.Execute(query, param: model);
            }
        }
        public List<JobConfigModel> Find(Func<JobConfigModel, bool> predicate)
        {
            lock (this)
            {
                return _jobConfigs.Where(predicate).ToList();
            }
        }
        public JobConfigModel ElevatorMiRUnContorlMissionSelect(Robot robot, string startPosition, string endPosition)      //출발지와 목적지로 Mission 찾기(자재 유무 확인안함)
        {
            lock (this)
            {
                return _jobConfigs.FirstOrDefault(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.CallName != "None_None" && m.CallName != "none_none"
                                                      && m.CallName.StartsWith(startPosition) && m.CallName.EndsWith(endPosition));
            }
        }

        public JobConfigModel AutoMissionSelect(Robot robot, string startPosition, string endPosition)      //출발지와 목적지로 Mission 찾기(자재 유무 확인)
        {
            lock (this)
            {
                return _jobConfigs.FirstOrDefault(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.CallName != "None_None" && m.CallName != "none_none"
                                                       && m.CallName.StartsWith(startPosition) && m.CallName.EndsWith(endPosition)
                                                       && m.ProductValue == robot.internalIoModule.Get_InputValue1);
            }
        }


        public JobConfigModel RegisterMissionSelect(Robot robot, string startPosition)     //출발지와 레지스터로 Mission 찾기
        {
            lock (this)
            {
                return _jobConfigs.FirstOrDefault(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.CallName != "None_None" && m.CallName != "none_none"
                                                      && m.CallName.StartsWith(startPosition)
                                                      && m.jobCallSignal == robot.Registers.dMiR_Register_Value[20].ToString() && m.ProductValue == robot.internalIoModule.Get_InputValue1);
            }
        }
        public JobConfigModel T3FMissionSelect(Robot robot, string startPosition, string endPosition) //(T3F 층에 가는 미션)
        {
            lock (this)
            {

                return _jobConfigs.FirstOrDefault(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.CallName != "None_None" && m.CallName != "none_none"
                                                     && m.CallName.StartsWith(startPosition) && m.CallName.Contains("T3F") && (m.CallName.EndsWith(endPosition) == false)
                                                     && m.ProductValue == robot.internalIoModule.Get_InputValue1);

            }
        }
        public JobConfigModel AMTMissionSelect(Robot robot, string startPosition, string EndPosition)     //출발지와 레지스터로 Mission 찾기
        {
            lock (this)
            {
                return _jobConfigs.FirstOrDefault(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.CallName != null
                                                      && m.CallName.StartsWith(startPosition)
                                                      && m.jobCallSignal == robot.Registers.dMiR_Register_Value[20].ToString());
            }
        }
        public JobConfigModel AMTMissionSelect_Without_Register(Robot robot, string startPosition, string EndPosition)     //출발지와 목적지로 Mission 찾기
        {
            lock (this)
            {
                return _jobConfigs.FirstOrDefault(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.CallName != null
                                                      && m.CallName.StartsWith(startPosition) && m.CallName.EndsWith(EndPosition));
            }
        }

        //public void Update_MissionName(MissionConfig model)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        con.Execute("UPDATE MissionConfigs SET MissionName=@missionName WHERE Id=@id AND UseFlag=1",
        //            param: new { missionName = model.MissionName, id = model.Id });
        //    }
        //}

    }
}
