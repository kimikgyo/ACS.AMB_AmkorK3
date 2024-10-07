using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        /// <summary>
        /// 스케줄러 대기
        /// </summary>
        private void JobStartControl()                                      //<==========미션 상태 Start 변경
        {
            #region 미션상태 init => start (변경전)
            ////=============================================== 미션 상태 Start =================================================//
            ////1.Init으로 등록되어 있는 job 을 Start로 상태를 변경한다.
            ////=================================================================================================================//
            //try
            //{
            //    var jobs = uow.Jobs.Find(x => (x.ACSJobGroup != "None") && x.JobState == JobState.JobInit);
            //    foreach (var job in jobs)
            //    {
            //        job.JobState = JobState.JobStart;
            //        uow.Jobs.Update(job);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogExceptionMessage(ex);
            //}
            #endregion

            #region 미션상태 init => start (변경후)
            //=============================================== 미션 상태 Start =================================================//
            //1.Init으로 등록되어 있는 job 중에서, Job 1개를 Init => Start 상태로 변경한다
            //=================================================================================================================//

            int activeJobCount = uow.Jobs.Find(x => x.JobState == JobState.JobInit).Count();
            if (activeJobCount != 0) // 대기중이거나 진행중인 Job이 없을 때
            {
                try
                {
                    var jobs = uow.Jobs.Find(x => x.ACSJobGroup != "None" && x.JobState == JobState.JobInit);

                    //jobs = jobs.OrderByDescending(x => x.JobCreateTime).ToList(); // job 생성시간순으로 정렬
                    jobs = jobs.OrderBy(x => x.JobPriority).ToList(); // job 우선순위로 정렬[오름차순으로 정렬 0->1->2->3]



                    foreach (var robot in GetActiveRobotsOrderbyDescendingBattery())
                    {
                        Job selectedJob = null;
                        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 상태 변경할 잡 선택

                        // - case1. 순서대로 job 1개를 선택한다
                        //selectedJob = jobs.FirstOrDefault();

                        //우선순위 1.컨베이어 다음미션은 스케줄에 들어오는 순서대로 진행
                        if (robot.Registers.dMiR_Register_Value[15] > 0 && robot.Registers.dMiR_Register_Value[16] > 0) selectedJob = jobs.FirstOrDefault(j => j.JobPriority == 1);
                        //자재가 없을경우 첫번째 미션을 찾는다.
                        else selectedJob = jobs.FirstOrDefault();

                        if (selectedJob != null)
                        {
                            selectedJob.JobState = JobState.JobStart;
                            uow.Jobs.Update(selectedJob);
                        }

                    }

                }
                catch (Exception ex)
                {
                    main.LogExceptionMessage(ex);
                }
            }
            #endregion
        }
        
        /// <summary>
        /// 미션 전송
        /// </summary>
        private void PostJobControl()                                           //<========== [미션 전송]
        {
            try
            {
                Job selectedJob = null;
                foreach (var robot in GetActiveRobotsOrderbyDescendingBattery())
                {

                    // running 미션 리스트
                    var runMissions = uow.Missions.GetAll()
                                                  .Where(m => m.ReturnID > 0)                                                           // 이미 전송한 미션 찾는다
                                                  .Where(m => new[] { m.RobotName, m.JobCreateRobotName }.Contains(robot.RobotName))    // 로봇네임이 일치하는 미션 찾는다
                                                  .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid").ToList();        // 완료되지 않은 미션 찾는다


                    // run미션중에서 일반Job미션 찾는다
                    var runMission_Job = runMissions.SingleOrDefault(m => m.JobId != 0); // 일반미션

                    // run미션중에서 충전미션을 찾는다
                    var runChargingMissions = runMissions.Where(m => m.JobId == 0)
                                                         .Where(m => GetChargingConfigs(robot.RobotName, m.MissionName).Count() > 0).ToList();
                    // run미션중에서 대기미션을 찾는다
                    var runWaitingMissions = runMissions.Where(m => m.JobId == 0)
                                                        .Where(m => GetWaitingConfigs(robot.RobotName, m.MissionName).Count() > 0).ToList();

                    // 리셋미션 찾는다
                    var RobotResetMission = runMissions.FirstOrDefault(r => r.MissionName == ConfigData.RobotResetMissionName && r.RobotName == robot.RobotName);

                    // 특수 미션 리스트 생성 (위 충전,대기미션 리스트를 합친 리스트)
                    var runSpecialMissions = new List<Mission>();
                    runSpecialMissions.AddRange(runChargingMissions);
                    runSpecialMissions.AddRange(runWaitingMissions);


                    // (조건1) 이 로봇이 실행중인 미션이 없을때
                    bool c1 = robot.ACSRobotGroup != "None"
                           && robot.StateID == RobotState.Ready
                           && runMission_Job == null
                           && RobotResetMission == null;

                    // (조건2) 이 로봇이 특수미션을 실행중일때 (자동충전미션 / 자동대기위치미션)
                    bool c2 = robot.StateID == RobotState.Executing
                         && runSpecialMissions.Count() > 0
                         && RobotResetMission == null;

                    if (c1 || c2)
                    {
                        // ==== select job ====

                        // 이 로봇에 job이 할당되어 있으면 수행하던 executing job을 선택하고,
                        // 아니면 다음 waiting job을 찾는다

                        // get executing job
                        if (robot.JobId != 0)
                        {
                            selectedJob = uow.Jobs.GetById(robot.JobId); // executing job
                            if (selectedJob == null) EventLogger.Info($"job {robot.JobId} not found!");
                        }
                        // get next waiting job
                        else
                        {

                            //// 선택가능한(대기중인) job들만 필터링한다
                            //var waitingJobs = uow.Jobs.Find(j =>
                            //{
                            //    bool flag = true;
                            //    flag &= j.JobState == JobState.JobWaiting;          // waiting job
                            //    flag &= j.MissionSentCount == 1;                    // first mission
                            //    flag &= j.ACSJobGroup == robot.ACSRobotGroup;       // robot 그룹과 일치하는 미션을 전송하기 위함
                            //    flag &= string.IsNullOrEmpty(j.JobCreateRobotName); // 이 경우는 JobCreateRobotName 사용하지 않는다!
                            //    flag &= j.ExecuteBattery <= robot.BatteryPercent;   // jobConfig 설정된 배터리 용량
                            //    return flag;
                            //});

                            var waitingJobs = uow.Jobs.Find(j =>
                            {
                                bool flag = true;
                                flag &= j.JobState == JobState.JobWaiting;          // waiting job
                                flag &= j.MissionSentCount == 1;                    // first mission
                                flag &= j.ACSJobGroup == robot.ACSRobotGroup;       // robot 그룹과 일치하는 미션을 전송하기 위함
                                flag &= j.JobCreateRobotName == robot.RobotName;    //job 생성시로봇과 현재로봇을비교 (Amkor K5 M3F_T3F층간이송)
                                                                                    //flag &= j.ExecuteBattery == robot.BatteryPercent;    //jobConfig 설정된 배터리 용량(Robot지정경우 jobAdd할때 배터리용량을 확인후 job생성)
                                return flag;
                            }).FirstOrDefault();

                            // 순서대로 job 1개를 선택한다
                            selectedJob = waitingJobs;
                        }

                        // ==== select mission ====
                        //로봇지정이 아닌 스케쥴
                        if (selectedJob != null)
                        {
                            IEnumerable<Mission> missions = uow.Missions.GetAll();
                            missions = missions.Where(m => m.ACSMissionGroup == robot.ACSRobotGroup).ToList();
                            missions = missions.Where(m => m.JobId == selectedJob.Id).ToList();                          // 해당 job의 미션들
                            missions = missions.Where(m => m.ReturnID == 0).ToList();                            // 아직 전송하지 않은 미션들
                            missions = missions.Where(m => m.MissionState == "Waiting").ToList();                // 전송대기상태인 미션들........Pending 상태인 것 제외?
                            missions = missions.Where(m => !string.IsNullOrWhiteSpace(m.CallName) && !string.IsNullOrWhiteSpace(m.MissionName) && m.MissionName != "None").ToList(); // 콜버튼네임,미션네임 유효인가?
                            var nextMission = missions.FirstOrDefault(m => m.JobCreateRobotName == robot.RobotName);

                            // ==== send mission ====

                            if (nextMission != null)      //지정 Robot 이 없을경우
                            {
                                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                // job에 지정된 로봇과 일치하는지 확인한다 (첫미션은 로봇 지정안됨)
                                // 로봇을 지정하여 보내야함 (Amkor K5 M3F_T3F층간이송)
                                bool isFirstMission = nextMission.Id == selectedJob.Missions[0].Id;
                                bool isRobotMatched = isFirstMission ? nextMission.Robot == null : nextMission.Robot == robot;
                                if (isRobotMatched == false)
                                {
                                    EventLogger.Info($" (nextMissionRobot and targetRobot) robot {robot.Id} not same!");
                                    continue; // 로봇 불일치!!
                                }
                                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                                // mir queue 비워주고, 새로운 미션을 전송한다
                                if (DeleteMission(robot, null))
                                {
                                    // 이 로봇과 관련된 특수미션이 있는 경우 모두 지워준다 (job미션은 제외하고)
                                    foreach (var m in runSpecialMissions)
                                    {
                                        uow.Missions.Remove(m);
                                    }

                                    if (SendMission(robot, nextMission))
                                    {

                                        // ===== job 상태 갱신
                                        selectedJob.RobotName = robot.RobotName;        // job로봇 지정

                                        if (nextMission.Id == selectedJob.MissionIds[0]) // job상태는 첫미션전송시에만 변경
                                        {
                                            selectedJob.JobState = JobState.JobExecuting;   // job상태 변경(JobExecuting)
                                        }

                                        uow.Jobs.Update(selectedJob);

                                        // ===== robot 상태 갱신
                                        robot.JobId = selectedJob.Id;                   // 로봇에 job id 지정
                                        uow.Robots.Update(robot);

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

    }
}
