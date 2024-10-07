using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace INA_ACS_Server
{
    public abstract class JobControl_Base
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly static ILog WorkLogger = LogManager.GetLogger("Work"); //물동량 관련 Log
        private readonly static ILog PutRegisterLogger = LogManager.GetLogger("PutRegister");
        private readonly static ILog GetRegisterLogger = LogManager.GetLogger("GetRegister");
        private MainForm main;
        private IUnitOfWork uow;


        public JobControl_Base(MainForm main, IUnitOfWork uow)
        {
            this.main = main;
            this.uow = uow;
        }

        //상속자 같이 사용하기 위하여 [Virtual pritected]사용
        //virtual 에서 외부 공유하기위하여 Public로변경하여 [Virtual Public]로 사용가능
        //상속받은 Class에서 변경하려면 [override] 사용 하여 변경할수있음


        #region JobControl_Base 외부 사용

        virtual public void UpdateMissionState()
        {
            var missions = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState != "Done"
                                             && m.Robot.ConnectState
                                             //&& m.Robot.Fleet_State != FleetState.None && m.Robot.Fleet_State != FleetState.unavailable
                                             && !string.IsNullOrWhiteSpace(m.Robot.RobotName) && !string.IsNullOrWhiteSpace(m.Robot.StateText)).ToList();


            foreach (var m in missions)
            {

                var oldMissionState = m.MissionState;
                var oldRobotID = m.RobotID;

                var result = m.Robot.Api.GetMissionQueueByIdAsync(m.ReturnID).Result;
                if (result != null)
                {
                    string newMissionState = result.state;
                    int newRobotID = -1; // fleet only

                    if (newMissionState == "Aborted") Upgrade_Error_Reset_Charge_Waiting_PostMission(m.Robot, m);

                    if ((oldMissionState != newMissionState) || (oldRobotID != newRobotID))
                    {
                        m.MissionState = newMissionState;
                        m.RobotID = newRobotID; // fleet only.  미션전송시 미지정한 경우, 미션상태가 Executing일때 유효한 값이 들어온다
                        uow.Missions.Update(m);

                        MissionsSpecific missionsSpecific = new MissionsSpecific();
                        missionsSpecific.RobotName = m.RobotName;
                        missionsSpecific.CallState = newMissionState;

                        uow.MissionsSpecific.CallState_Update(missionsSpecific);

                        Console.WriteLine("mission 상태 변경: id={0}, {1} => {2}", m.Id, oldMissionState, m.MissionState);
                    }
                }
            }
        }

        virtual public void JobHandler()
        {
            foreach (var job in uow.Jobs.Find(j => j.MissionTotalCount > 0))
            {
                if (job.Missions.Count != job.MissionTotalCount) // 미션총개수와 실제미션수와 일치하지 않는 경우는 제외!
                {
                    EventLogger.Info($"{job.Name} MissionTotalCount and Missions.Count is NOT same!");
                    continue;
                }

                // ========== job 상태 갱신
                Job_UpdateJobState(job);

                Mission sentMission = null; // job의 이전에 보낸 (처리중인) 미션
                Mission nextMission = null; // job의 다음 보낼 미션

                // ========== 미션 Waiting 설정
                if (job.JobState >= JobState.JobStart) // ==> 함수 PopCall_StatusControl_LIFT_Sub() 에 의해 JobStart로 전환된다
                {
                    // ========== 미션(sent/nextMission) 선택
                    {
                        if (job.MissionSentCount == 0)
                        {
                            // 일반적인 1번째 미션
                            {
                                sentMission = null;
                                nextMission = job.Missions[0];
                            }
                        }
                        else if (job.MissionSentCount < job.MissionTotalCount)      // 2~n-1번째 미션
                        {
                            sentMission = job.Missions[job.MissionSentCount - 1];
                            nextMission = job.Missions[job.MissionSentCount];
                        }
                        else if (job.MissionSentCount == job.MissionTotalCount)     // n번째(마지막) 미션
                        {
                            sentMission = job.Missions[job.MissionSentCount - 1];
                            nextMission = null;
                        }
                        else // 비정상
                        {
                            continue;
                        }
                    }

                    // ========== 미션(nextMission)의 상태를 전송대기(Waiting) 설정 (이전미션 완료시)
                    if ((job.MissionSentCount == 0 && sentMission == null) ||                   // - 첫번째 미션은 바로 전송대기로,
                        (job.MissionSentCount > 0 && sentMission.MissionState == "Done"))       // - 나머지 미션은 이전 미션이 완료(Done)되면 전송대기로 설정.
                    {
                        if (nextMission != null && nextMission.MissionState == "Init")
                        {

                            //상태갱신
                            if (job.MissionSentCount == 0) // 첫미션
                            {
                                nextMission.Robot = null;
                                nextMission.RobotID = 0;
                                nextMission.RobotName = "";
                                job.JobState = JobState.JobWaiting; // 첫미션일때 Job 상태를 JobWaiting 으로 변경
                            }
                            else // 나머지미션
                            {
                                nextMission.Robot = uow.Robots.GetByRobotName(job.RobotName);
                                nextMission.RobotID = 0;
                                nextMission.RobotName = job.RobotName;
                            }
                            nextMission.MissionState = "Waiting";

                            // 전송 미션 카운팅!
                            job.MissionSentCount++;

                            // db update
                            uow.Missions.Update(nextMission);
                            uow.Jobs.Update(job);
                        }
                        else if (nextMission != null && nextMission.MissionState == "Waiting")
                        {
                            // 미션이 전송대기중이다
                        }
                        else if (nextMission == null)
                        {
                            // 미션이 더이상 없다
                        }
                    }
                }
                // ========== job 상태 갱신         // =====> 위로 이동시킴
                //Job_UpdateJobState(job);


                // ========== job 완료/에러 처리
                Job_JobAbortedHandler(job, sentMission);    // job 에러 처리 - 로봇에러가 클리어되면(레디상태로 변경되면) 미션 재전송
                Job_JobInvalidHandler(job);                 // job 무효 처리 - 데이터 삭제
                Job_JobDoneHandler(job);                    // job 완료 처리 - 데이터 삭제
            }

            //로컬함수

            // job aborted 처리
            void Job_JobAbortedHandler(Job job, Mission abortedMission)
            {
                if (job != null && job.JobState == JobState.JobAborted)
                {
                    var robot = uow.Robots.GetByRobotName(job.RobotName);
                    if (robot != null)
                    {
                        // (조건1) 미션을 수행하던 로봇이 레디상태가 되었나?
                        bool c1 = robot.ACSRobotActive == true
                               && robot.StateID == RobotState.Ready;

                        if (c1)
                        {
                            // mir queue 먼저 비워주고, (aborted) 미션 재전송한다
                            if (DeleteMission(robot, null))
                            {
                                SendMission(robot, abortedMission);

                                Console.WriteLine("Job_JobAbortedHandler 호출됨");
                                Thread.Sleep(2000);


                            }
                        }
                    }
                }
            }

            // job invalid 처리 (카운팅없이 완료처리)
            void Job_JobInvalidHandler(Job job)
            {
                if (job != null && job.JobState == JobState.JobInvalid)
                {
                    var robot = uow.Robots.GetByRobotName(job.RobotName);
                    if (robot != null)
                    {
                        // mir queue 비워주고, job 데이터 삭제한다
                        if (DeleteMission(robot, null))
                        {
                            DeleteJobData(job, 4);
                        }
                    }
                }
            }

            // job done 처리 (카운팅후 완료처리)
            void Job_JobDoneHandler(Job job)
            {
                if (job != null && job.JobState == JobState.JobDone)
                {
                    var robot = uow.Robots.GetByRobotName(job.RobotName);
                    if (robot != null)
                    {
                        // mir queue 비워주고, job 데이터 삭제한다
                        if (DeleteMission(robot, null))
                        {
                            // CALL 완료 로그
                            WorkLogger.Info(
                                $"{job.CallName}," +                  //콜네임
                                $"{job.RobotName}," +                       //수행로봇
                                $"{job.JobState}," +                        //완료상태(DONE/ABORTED)
                                $"{job.JobCreateTime:yyyy-MM-dd HH:mm:ss}," + //콜시간(CALL)
                                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");     //완료시간(FINISH)

                            DeleteJobData(job, 1);

                            // 로봇이 수행한 마지막 콜을 기억해둔다
                            robot.LastCallName = job.CallName;

                            {
                                var Missions = uow.MissionsSpecific.GetAll(0).Where(x => x.RobotName != null && x.CallState == "Done");

                                foreach (var temp in Missions)
                                {
                                    MissionsSpecific missionsSpecific = new MissionsSpecific();
                                    missionsSpecific.No = temp.No;

                                    uow.MissionsSpecific.Remove(missionsSpecific);
                                }
                            }

                            //만약 다른층에서 부른 미션이 들어갈 경우
                            //그 미션은 끝나고 연결된 미션 수정 필요
                            {
                                var Missions = uow.MissionsSpecific.GetAll(0).Where(x => string.IsNullOrEmpty(x.RobotName) && x.CallState == "wait" && x.Move_CallName == job.CallName);

                                foreach (var temp in Missions)
                                {
                                    MissionsSpecific missionsSpecific = new MissionsSpecific();
                                    missionsSpecific.No = temp.No;
                                    missionsSpecific.Move_CallName = "RobotCall";

                                    uow.MissionsSpecific.MoveCallName_Update(missionsSpecific);
                                }
                            }
                        }
                    }
                }
            }
        }

        virtual public void Process_JobCommandQueue()
        {
            while (JobCommandQueue.TryDequeue(out var cmd))
            {
                var commandCode = cmd.Code;
                // pop call 정보
                var CallName = cmd.Text;                 // call name
                var CallServerId = cmd.Extra1;           // DB Call DB에있는 ID 를 가지고 온다 
                var popCallType = cmd.Extra2;            // pop call return type
                var popCallAngle = cmd.Extra3;           // pop call angle
                var JobCreateRobotName = cmd.Extra4;     // Robot Name 지정 (Amkor K5 M3F_T3F층간이송)
                var JobPriority = cmd.Extra5;            // 우선순위
                // old
                ////var existingJob = uow.Jobs.GetByCallButtonName(popCallName); // 해당 콜버튼의 기존 job
                //var existingJob = uow.Jobs.GetByCreateRobot(JobCreateRobotName); // Robot으로 수행중인 job검색

                //new
                Job existingJob = null;

                if (!string.IsNullOrWhiteSpace(JobCreateRobotName)) // m3f
                {
                    existingJob = uow.Jobs.GetByCreateRobot(JobCreateRobotName); // m3f  는 job생성시 로봇네임이 지정된다! Robot으로 수행중인 job검색
                }
                else // amt
                {
                    existingJob = uow.Jobs.GetByCallName(CallName);  // amt 는 CallName으로 수행중인 job검색
                    //existingJob = uow.Jobs.GetByInterfaceServerId(CallServerId); //PLC 중복방지ID 로 인하여 수행중인 JOb을 판단한다
                }

                //var existingJob = uow.Jobs.GetByCallName(CallName);


                //======== 같은 job 이름은 같지만 다른로봇이 job을 수행하기위함 필요없으면 삭제해야함 (Amkor K5 M3F_T3F층간이송) Test
                //var CreateRobotexistingJob = uow.Jobs.Find(x => x.JobCreateRobotName == JobCreateRobotName).SingleOrDefault(); // job 실행중인 로봇(Test)

                string message = null;

                switch (commandCode)
                {
                    case JobCommandCode.ADD: // 해당 콜버튼의 job이 없으면 추가한다
                        if (existingJob == null)
                        {
                            //Operator Call
                            var jobCfg = uow.JobConfigs.GetByCallName(CallName); // 현재.. job config 는 mission config 참조해서 설정한다

                            if (jobCfg != null && (main.ACSMode == ElevatorAGVMode.MiRControlMode || !main.Floors.Contains(jobCfg.DestFloor)))
                            {
                                Job newJob = null;
                                using (var trxScope = new TransactionScope())
                                {
                                    // job 생성
                                    newJob = new Job
                                    {
                                        Name = $"Job_{jobCfg.CallName}_{DateTime.Now:HHmmss}",
                                        ACSJobGroup = jobCfg.ACSMissionGroup,
                                        //======== pop info
                                        CallName = jobCfg.CallName,                 //call name (= pop key)
                                        PopServerId = CallServerId,                  //pop server id // Convert.ToInt32(jobCfg.CallButtonIpAddress)
                                        PopCallReturnType = popCallType,            //pop call return type
                                        PopCallAngle = popCallAngle,                //pop call angle
                                        WmsId = 0,
                                        //======== RobotSelect
                                        JobCreateRobotName = JobCreateRobotName,    //Job 생성시 로봇지정해서 전송해야함 (Amkor K5 M3F_T3F층간이송)
                                        //========
                                        RobotName = "",                             // robot info.  로봇정보는 첫미션전송시 설정
                                        ExecuteBattery = jobCfg.ExecuteBattery,     //jobConfig 설정된 배터리 데이터
                                        JobState = JobState.JobInit,                //JobState.JobStart,
                                        JobCreateTime = DateTime.Now,
                                        Missions = new List<Mission>(),
                                    };

                                    // job 목록에 추가
                                    uow.Jobs.Add(newJob);

                                    // job 미션 생성
                                    for (int i = 0; i < JobConfigModel.JOB_MISSION_TOTAL_COUNT; i++)
                                    {
                                        string subMissionName = jobCfg.GetJobMissionName(i);  // config 에서 n번째 미션 선택한다

                                        if (!string.IsNullOrWhiteSpace(subMissionName) && subMissionName.ToUpper() != "NONE")
                                        {
                                            var newMission = new Mission()
                                            {
                                                // 미션 기본 설정
                                                JobId = newJob.Id,
                                                ACSMissionGroup = jobCfg.ACSMissionGroup,
                                                CallName = jobCfg.CallName,
                                                MissionName = subMissionName,
                                                ErrorMissionName = null,            // 사용x
                                                MissionOrderTime = DateTime.Now,
                                                // 로봇정보는 미션전송시 설정
                                                Robot = null,
                                                RobotID = -1,       // fleet only
                                                JobCreateRobotName = JobCreateRobotName, //Job 생성시 로봇지정해서 전송해야함 (Amkor K5 M3F_T3F층간이송)
                                                RobotName = "",
                                                // 리턴ID는 미션전송후 리턴값으로 설정
                                                ReturnID = 0,
                                                MissionState = "Init",
                                            };

                                            newJob.Missions.Add(newMission);
                                            uow.Missions.Add(newMission);

                                            newJob.MissionIds[i] = newMission.Id; // 미션 id list 저장한다!
                                            newJob.MissionTotalCount++; // 총 미션 개수
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    // job 미션 정보 갱신
                                    uow.Jobs.Update(newJob);
                                    //Tablet 상태정보 갱신
                                    trxScope.Complete();
                                }


                                // log message
                                message = $"{">> JOB.ADD",-20} [--] {newJob}";
                                foreach (var m in newJob.Missions)
                                {
                                    message += Environment.NewLine;
                                    message += $"{"  >> MISSION ADD",-20} [--] {m}";
                                }

                                //// 해당 콜버튼의 미션상태를 갱신해 준다
                                //var callButton = uow.CallButtons.GetByName(newJob.CallName);
                                //if (callButton != null) callButton.MissionStateText = "waiting"; //미션상태를 갱신하기위해서 소문자로 변경함.
                            }
                            else
                            {
                                message = $"Process_JobQueue(ADD): ignore.  job config not found.  CallName \'{CallName}\'";
                            }
                        }
                        else
                        {
                            message = $"Process_JobQueue(ADD): ignore.  job already exist.     callbutton \'{CallName}\'";
                        }

                        break;

                    case JobCommandCode.REMOVE: // 해당 콜버튼의 job이 있으면 삭제한다
                        if (existingJob != null)
                        {
                            // mir queue 삭제
                            var robot = uow.Robots.GetByRobotName(existingJob.RobotName);
                            if (robot != null && robot.ConnectState) DeleteMission(robot, null);

                            // 해당 job 삭제
                            DeleteJobData(existingJob, 2);

                            ElevatorStateModule elevatorState = new ElevatorStateModule();
                            elevatorState.RobotName = robot.RobotName;
                            uow.ElevatorState.Remove(elevatorState);

                            // log message
                            message = $"{">> JOB.REMOVE",-20} [--] {existingJob}";

                        }
                        else // job 없는데 cancel 요청이 올 경우..  cancel 요청만 마무리 해준다
                        {
                            //DBCall_Done_Without_Job(CallName);
                        }
                        break;

                    case JobCommandCode.REMOVE_BY_ACS: // 해당 콜버튼의 job이 있으면 삭제한다
                        if (existingJob != null)
                        {
                            // mir queue 삭제
                            var robot = uow.Robots.GetByRobotName(existingJob.RobotName);
                            if (robot != null && robot.ConnectState) DeleteMission(robot, null);

                            // 해당 job 삭제
                            DeleteJobData(existingJob);

                            // log message
                            message = $"{">> JOB.REMOVE_BY_ACS",-20} [--] {existingJob}";
                        }
                        else // job 없는데 cancel 요청이 올 경우..  cancel 요청만 마무리 해준다
                        {
                            //DBCall_Done_Without_Job(CallName, byACS: true);
                        }
                        break;

                    case JobCommandCode.REMOVE_ALL: // 해당 콜버튼의 job이 있으면 삭제한다
                        foreach (var x in uow.Jobs.GetAll())
                        {
                            // mir queue 삭제
                            var robot = uow.Robots.GetByRobotName(x.RobotName);
                            if (robot != null && robot.ConnectState) DeleteMission(robot, null);

                            // 해당 job 삭제
                            DeleteJobData(x);

                            // log message
                            message = $"{">> JOB.REMOVE.ALL",-20} [--] {x}";
                        }
                        break;
                }

                // log
                if (message != null)
                {
                    Console.WriteLine(message);
                    EventLogger.Info(message);
                    main.ACS_UI_Log(message);
                }
            }
        }

        virtual public void SpecialMissionDone_Handler()
        {
            foreach (var m in uow.Missions.Find(m => m.ReturnID > 0 && m.JobId == 0 && m.MissionState == "Done").ToList())
            {
                var ChargeConfig = uow.ChargeMissionConfigs.Find(c => c.ChargeMissionName == m.MissionName).FirstOrDefault();
                var WaitingConfig = uow.WaitMissionConfigs.Find(w => w.WaitMissionName == m.MissionName).FirstOrDefault();
                string RobotResetMission = $"None_{ConfigData.RobotResetMissionName}";

                if (ChargeConfig != null || WaitingConfig != null || (m.CallName == RobotResetMission))
                {
                    DeleteMission(m.Robot, m);
                }
            }
        }

        virtual public bool MiR_Get_Register(Robot robot, int registerNo)
        {
            int retry = 0;
            while (retry++ < 3)
            {
                var getResult = robot.Api.GetRegisterById(registerNo);
                if (getResult != null)
                {
                    MiR_ReST_ParsingRegister_ID(robot, getResult.id, (int)getResult.value);

                    string message = $"{nameof(MiR_Get_Register),-10} [OK] {robot.RobotName} = Register_No {registerNo} = {getResult.value}";
                    GetRegisterLogger.Info(message);
                    main.ACS_UI_Log(message);
                    return true;
                }
            }
            return false;
        }

        virtual public bool MiR_Put_Register(Robot robot, int registerNo, int registerValue)
        {
            var putResult = robot.Api.PutRegisterById(registerNo, registerValue);
            if (putResult != null)
            {
                MiR_ReST_ParsingRegister_ID(robot, putResult.id, (int)putResult.value);

                string message = $"{nameof(MiR_Put_Register),-10} [OK] {robot.RobotName} = Register_No {registerNo} = {putResult.value}";
                PutRegisterLogger.Info(message);
                //main.ACS_UI_Log(message);
                return true;
            }
            return false;
        }

        #endregion


        #region JobControl_Base 내부사용

        virtual protected bool SendMission(Robot robot, Mission newMission)//<==========미션 전송[JobControl 내부사용]
        {
            string missionGuid = "";
            //========= Test 진행중
            if (robot == null || newMission == null) return false;

            if (newMission.MissionName == null)
                missionGuid = FindMissionGuid(newMission.ErrorMissionName);
            else
                missionGuid = FindMissionGuid(newMission.MissionName);

            if (missionGuid == null)
            {
                string msg = $"SendMission: mission \'{newMission.MissionName ?? newMission.ErrorMissionName }\' guid not found!";
                Console.WriteLine(msg);
                EventLogger.Info(msg);
                main.ACS_UI_Log(msg);
                return false;
            }

            string message = null;
            int retry = 0;

            // 타겟 로봇정보 지정
            newMission.Robot = robot;
            newMission.RobotID = 0; // fleet only.
            newMission.RobotName = robot.RobotName;

            while (retry++ < 2)
            {
                // send mir data & update mission data
                var postResult = robot.Api.PostMissionQueueAsync(new { mission_id = missionGuid }).Result;
                if (postResult != null)
                {
                    newMission.ReturnID = postResult.id; // 전송에 대한 응답메시지로 받은 ReturnID 저장
                    newMission.MissionState = postResult.state;

                    //if (!string.IsNullOrEmpty(newMission.MissionName) && !string.IsNullOrEmpty(newMission.CallName)) // 콜버튼미션일때
                    if (newMission.JobId > 0) //jobId로 스페셜미션 아니면 기존미션을 판단한다
                    {
                        uow.Missions.Update(newMission);
                    }
                    else // 특수미션일때 및 에러 미션
                    {
                        uow.Missions.Add(newMission);
                    }

                    // log success
                    message = $"{nameof(SendMission),-20} [OK] {newMission}";
                    Console.WriteLine(message);
                    EventLogger.Info(message);
                    main.ACS_UI_Log(message);

                    return true;
                }
            }

            // log fail
            message = $"{nameof(SendMission),-20} [NG] robot={robot.RobotName}";
            Console.WriteLine(message);
            EventLogger.Info(message);
            main.ACS_UI_Log(message);

            return false;
        }

        virtual protected bool DeleteMission(Robot robot, Mission mission)//<===========미션 삭제[JobControl 내부사용]
        {
            if (robot == null) return false;

            string message = null;
            int retry = 0;

            while (retry++ < 2)
            {
                // delete mir data , mission data
                if (robot.Api.DeleteMissionQueueAsync().Result)
                {
                    //if (mission != null) uow.Missions.Remove(mission);
                    if (mission != null)
                    {
                        if (mission.JobId == 0)
                        {
                            uow.Missions.Remove(mission);
                        }
                        else
                        {
                            EventLogger.Info("DeleteMission() Job Mission 삭제 시! 하면 안된다!");
                        }
                    }

                    // log success
                    message = (mission != null) ? $"{nameof(DeleteMission),-20} [OK] {mission}"
                                                : $"{nameof(DeleteMission),-20} [OK] robot={robot.RobotName}";
                    Console.WriteLine(message);
                    EventLogger.Info(message);
                    main.ACS_UI_Log(message);

                    return true;
                }
            }

            // log fail
            message = $"{nameof(DeleteMission),-20} [NG] robot={robot.RobotName}";
            Console.WriteLine(message);
            EventLogger.Info(message);
            main.ACS_UI_Log(message);

            return false;
        }

        virtual protected void DeleteJobData(Job job, int jobResultCode = 0)
        {
            //[기본]
            //jobResultCode = 0 완료가 안될시 0(REMOVE_BY_ACS삭제)
            //jobResultCode = 1 완료 
            //jobResultCode = 2 취소 
            //jobResultCode = 3 리셋 
            //jobResultCode = 4 Invalid

            //[차트표기할 job]
            //jobResultCode = 10 완료가 안될시 0(REMOVE_BY_ACS삭제)
            //jobResultCode = 11 완료 
            //jobResultCode = 12 취소 
            //jobResultCode = 13 리셋 
            //jobResultCode = 14 Invalid


            if (job == null) return;

            //운반량 차트 표기를 활성화조건 true라면 운반량 차트 표기 구분하기위하여 Code변경한다.
            var transprotCountActive = uow.JobConfigs.Find(j => j.CallName == job.CallName && j.TransportCountActive == true).FirstOrDefault();
            if (transprotCountActive != null)
            {
                if (jobResultCode == 0) jobResultCode = 10;
                else if (jobResultCode == 1) jobResultCode = 11;
                else if (jobResultCode == 2) jobResultCode = 12;
                else if (jobResultCode == 3) jobResultCode = 13;
                else if (jobResultCode == 4) jobResultCode = 14;
            }
            if ((jobResultCode == 0) || (jobResultCode == 10) || (jobResultCode == 2) || (jobResultCode == 12) || (jobResultCode == 3)
                || (jobResultCode == 13) || (jobResultCode == 4) || (jobResultCode == 14)) job.TransportCountValue = 0;

            AddJobLog(job, jobResultCode);

            using (var trxScope = new TransactionScope())
            {
                // job missions 삭제
                foreach (var m in job.Missions)
                {
                    uow.Missions.Remove(m);
                }

                // job 삭제
                uow.Jobs.Remove(job);

                // robot job 할당 삭제
                var robot = uow.Robots.GetByRobotName(job.RobotName);
                if (robot != null)
                {
                    robot.JobId = 0;
                    uow.Robots.Update(robot);
                }
                else
                {
                    Console.WriteLine("Job_DeleteJobData(): robot is null !");
                    EventLogger.Info("Job_DeleteJobData(): robot is null !");
                }

                //Commits the transaction
                trxScope.Complete();
            }

            // log
            string message = $"{"Job_DeleteJobData",-20} [OK] {job}";
            Console.WriteLine(message);
            EventLogger.Info(message);
            main.ACS_UI_Log(message);
        }

        virtual protected void Job_UpdateJobState(Job job)
        {
            if (job != null)
            {
                var allMissions = job.Missions;

                JobState currentState = job.JobState;
                JobState newState;

                if (allMissions.All(x => x.MissionState == "Done"))
                {
                    newState = JobState.JobDone; // 모든미션 완료이고 POP완료처리 되었을때
                }
                else if (allMissions.Any(x => x.MissionState == "Invalid"))
                {
                    newState = JobState.JobInvalid;
                }
                else if (allMissions.Any(x => x.MissionState == "Aborted"))
                {
                    newState = JobState.JobAborted;
                }
                else if (allMissions.Any(x => x.MissionState == "Sending" || x.MissionState == "Pending" || x.MissionState == "Executing"))
                {
                    newState = JobState.JobExecuting;
                }
                else
                {
                    newState = currentState; // 상태 유지
                }


                // 상태 변경시 db 업데이트
                if (currentState != newState)
                {
                    job.JobState = newState;
                    uow.Jobs.Update(job);
                    //Tablet 상태 변경 갱신
                    Console.WriteLine("job 상태 변경: id={0}, {1} => {2}", job.Id, currentState, job.JobState);
                }
            }
        }

        virtual protected void AddJobLog(Job job, int jobResultCode)
        {
            string linecd = "";
            string postcd = "";
            string elapsedTime = "";
            if (job.CallName.Contains('_'))
            {
                linecd = job.CallName.Split('_')[0];
                postcd = job.CallName.Split('_')[1];
            }
            TimeSpan elapsed = DateTime.Now - job.JobCreateTime;
            int elapsedTimeDay = elapsed.Days;
            int elapsedTimeHour = elapsed.Hours;
            int elapsedTimeMinute = elapsed.Minutes;
            int elapsedTimeSecond = elapsed.Seconds;
            if (elapsedTimeDay != 0) elapsedTime += elapsedTimeDay + "일";
            if (elapsedTimeHour != 0) elapsedTime += elapsedTimeHour + "시";
            if (elapsedTimeMinute != 0) elapsedTime += elapsedTimeMinute + "분";
            if (elapsedTimeSecond != 0) elapsedTime += elapsedTimeSecond + "초";
            // CALL 로그 (FILE)
            try
            {


                string logTxt = "";

                logTxt += $"{jobResultCode}";                               // job result

                logTxt += $",{job.CallName}";                          // 콜네임

                logTxt += $",{linecd}";  // 라인네임
                logTxt += $",{postcd}";   // 포지션네임

                logTxt += $",{job.RobotName}";                              // 수행로봇
                logTxt += $",{job.JobState}";                               // job상태(DONE/EXECUTING/ABORTED/INVALID)

                logTxt += $",{job.JobCreateTime:yyyy-MM-dd HH:mm:ss}";        // 콜시간
                logTxt += $",{job.JobCreateTime:yyyy-MM-dd HH:mm:ss}";      // job생성시간
                logTxt += $",{DateTime.Now:yyyy-MM-dd HH:mm:ss}";           // job완료시간

                logTxt += $",{elapsedTime}";           // 경과시간

                //logTxt += $",{job.WmsId}";

                logTxt += $",{string.Join("/", job.Missions.Select(m => m.MissionName))}";
                logTxt += $",{string.Join("/", job.Missions.Select(m => m.MissionState))}";

                logTxt += $",{job.TransportCountValue}";
                LogManager.GetLogger("AllWork").Info(logTxt);

            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }

            // JobHistory (DB)
            using (var con = new SqlConnection(ConnectionStrings.DB1))
            {
                try
                {
                    var robot = uow.Robots.Find(r => r.RobotName == job.RobotName).FirstOrDefault();
                    var jobLog = new JobLog()
                    {
                        CallName = job.CallName,
                        LineName = $"{linecd}",
                        PostName = $"{postcd}",
                        RobotAlias = robot != null ? robot.RobotAlias : "",
                        RobotName = job.RobotName,
                        JobState = job.JobStateText,
                        CallTime = job.JobCreateTime,
                        JobCreateTime = job.JobCreateTime,
                        JobFinishTime = DateTime.Now,
                        JobElapsedTime = $"{elapsedTime}",
                        MissionNames = $"{string.Join("/", job.Missions.Select(m => m.MissionName))}",
                        MissionStates = $"{string.Join("/", job.Missions.Select(m => m.MissionState))}",
                        ResultCD = jobResultCode,

                        //CallType = job.PopCallReturnType,
                        //LineName = $"{GetLineNameByCallName(job.CallButtonName)}",
                        //PosName = $"{GetPosNameByCallName(job.CallButtonName)}",
                        //PartCD = $"{popSvc.GetPartInfo2(job)?.PART_CD}",
                        //PartNM = $"{popSvc.GetPartInfo2(job)?.PART_NM}",
                        //PartOutQ = uow.WmsDB.GetById(job.WmsId)?.OUT_Q,
                        //PartOutP = uow.WmsDB.GetById(job.WmsId)?.OUT_POINT,
                        //WmsId = job.WmsId,

                    };
                    uow.JobLogs.Add(jobLog);
                }
                catch (Exception ex)
                {
                    main.LogExceptionMessage(ex);
                }
            }
        }

        virtual protected void Upgrade_Error_Reset_Charge_Waiting_PostMission(Robot robot, Mission runMission)
        {
            try
            {
                var waitmission = uow.WaitMissionConfigs.Find(x => x.WaitMissionName == runMission.MissionName && x.RobotName == robot.RobotName).FirstOrDefault();
                var chargemission = uow.ChargeMissionConfigs.Find(c => c.ChargeMissionName == runMission.MissionName && c.RobotName == robot.RobotName).FirstOrDefault();
                string RobotResetMission = $"None_{ConfigData.RobotResetMissionName}";

                // (조건1) 미션을 수행하던 로봇이 레디상태가 되었나? // 미션 전송 사용 (Active == true) 미사용(Active = false)
                bool c1 = robot.ACSRobotActive == true
                       && robot.StateID == RobotState.Ready;

                // (조건2) 미션이 특수미션인가?
                bool c2 = waitmission != null || chargemission != null || (runMission.CallName == RobotResetMission);

                //충전 , 대기위치 ,Reset 미션 부분    
                if (c1 && c2)
                {
                    if (DeleteMission(robot, runMission))   // 미션삭제
                    {
                        SendMission(robot, runMission);     // 미션추가.   위 삭제와 한쌍으로 맞춰야 한다. 그렇지 않으면 특수미션이 2개가 되는 상황 발생한다..
                    }
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        virtual protected string FindMissionGuid(string sMissionName)//<============Guid 검색
        {
            //해당 Mission Name으로 Guid를 검색하는 루틴
            var missions = main.GetMissions.SingleOrDefault(m => m.name == sMissionName);
            return missions?.guid;
        }

        virtual protected void MiR_ReST_ParsingRegister_ID(Robot robot, int RegisterNo, int registerValue)
        {

            robot.Registers.dMiR_Register_Value[RegisterNo] = registerValue;

            //default: throw new Exception($"{nameof(MiR_ReST_ParsingRegister_ID)} : parsing register not found");
        }
        #endregion
    }
}
