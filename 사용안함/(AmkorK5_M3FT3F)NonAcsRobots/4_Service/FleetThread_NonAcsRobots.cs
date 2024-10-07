using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class NonAcsMission
    {
        public int Id { get; set; }
        public Robot Robot { get; set; }
        public string RobotName { get; set; }
        public string MissionName { get; set; }
        //
        public int ReturnID { get; set; }
        public string MissionState { get; set; }
        public DateTime? MissionOrderTime { get; set; }
        public DateTime? MissionStartTime { get; internal set; }
        public DateTime? MissionFinishTime { get; internal set; }
        //
        public override string ToString()
        {
            //return JsonConvert.SerializeObject(this);
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
            sb.Append(",id=" + Id);
            sb.Append(",State=" + MissionState);
            sb.Append(",mission_id=" + ReturnID);
            sb.Append(",mission_name=" + MissionName);
            sb.Append(",ordered=" + MissionOrderTime?.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append(",started=" + MissionStartTime?.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.Append(",finished=" + MissionFinishTime?.ToString("yyyy-MM-dd HH:mm:ss"));
            return sb.ToString();
        }
    }

    public partial class FleetThread
    {

        // 나중에 uow.XXMissions 로 대체하기
        private readonly List<NonAcsMission> uow_NonAcsMissions = new List<NonAcsMission>();

        private void Job_UpdateMissionState_NotAcsMissions()
        {
            //아래 로직은 사용안함
            return;


            // ===== add new mission
            foreach (var robot in uow.Robots.Find(x => x.ACSRobotActive == false && x.ACSRobotGroup == "None"))
            {
                if (robot.MissionQueueID > 0 && !uow_NonAcsMissions.Any(m => m.ReturnID == robot.MissionQueueID && m.MissionState != "Done"))
                {
                    var newMission = new NonAcsMission()
                    {
                        ReturnID = robot.MissionQueueID,
                        MissionName = "",
                        MissionState = "Pending",
                        MissionOrderTime = default,
                        Robot = robot,
                        RobotName = robot.RobotName,
                    };
                    uow_NonAcsMissions.Add(newMission); //uow.Missions.Add(newMission);
                }
            }


            // ===== update mission state
            var runningMissions = uow_NonAcsMissions.Where(m => m.ReturnID > 0 && m.MissionState != "Done"
                                                             && m.Robot.Fleet_State != FleetState.None && m.Robot.Fleet_State != FleetState.unavailable
                                                             && !string.IsNullOrWhiteSpace(m.Robot.RobotName) && !string.IsNullOrWhiteSpace(m.Robot.StateText)).ToList();
            foreach (var m in runningMissions)
            {
                string oldMissionState = m.MissionState;

                if (MiR_ReST_Send("GET_MISSION_QUEUE_ID", m.Robot, m.ReturnID.ToString(), "") == 1) // get mission-state by mission-return-id
                {
                    int returnId = main.MissionScheduler.id;
                    string newMissionState = main.MissionScheduler.state;
                    string missionId = main.MissionScheduler.mission_id;
                    DateTime? missionOrderTime = main.MissionScheduler.ordered;
                    DateTime? missionStartTime = main.MissionScheduler.started;
                    DateTime? missionFinishTime = main.MissionScheduler.finished;

                    if (oldMissionState != newMissionState)
                    {
                        //m.ReturnID = returnId;
                        m.MissionState = newMissionState;
                        m.MissionName = FindMissionNameByGuid(missionId); // string.IsNullOrEmpty(m.MissionName) ? FindMissionNameByGuid(missionId) : m.MissionName;
                        m.MissionOrderTime = missionOrderTime;// ?? default;
                        m.MissionStartTime = missionStartTime;// ?? default;
                        m.MissionFinishTime = missionFinishTime;// ?? default;

                        //uow_NonAcsMissions.Update(m);
                        //uow.Missions.Update(m);

                        //Console.WriteLine("mission 상태 변경: id={0}, {1} => {2}", m.Id, oldMissionState, newMissionState);
                        TestLogMissionQueue(m);
                    }
                }
            }


            // ===== delete finished mission       // Aborted 포함시켜야 하나??
            var finishedMissions = runningMissions.Where(x => x.MissionState == "Done").ToList();
            foreach (var m in finishedMissions)
            {
                AddLog(m);
                uow_NonAcsMissions.Remove(m); //uow.Missions.Remove(m);
            }
            return;


            //===========================
            // local functions

            string FindMissionNameByGuid(string guid) => main.GetMissions.FirstOrDefault(m => m.guid == guid)?.name;

            void TestLogMissionQueue(NonAcsMission m)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
                    sb.Append(' ');
                    sb.Append(m.ToString());
                    sb.AppendLine();
                    string response = sb.ToString();
                    string logFile = $@"/Log/MissionQueueLog-{m.RobotName}.txt";
                    System.IO.File.AppendAllText(logFile, response);
                }
                catch (Exception ex)
                {
                    string logFile = $@"/Log/MissionQueueLog-{m.RobotName}.txt";
                    System.IO.File.AppendAllText(logFile, ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }

            void AddLog(NonAcsMission m)
            {

                string s = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {m}";
                System.IO.File.AppendAllText(@"/Log/no_acs_missions.txt", s + "\r\n");
                Console.WriteLine(s);
            }
        }








        public static void TaskCancelSample()
        {
            var cts = new CancellationTokenSource();
            var t = Task.Run(() => Sum(cts.Token, 10000), cts.Token);

            // 작업이 성공했을 경우 실행  
            t.ContinueWith(task =>
            {
                Console.Write("The sum is: " + task.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            // 작업에 예외가 발생했을 경우 실행  
            t.ContinueWith(task =>
            {
                foreach (var e in task.Exception.InnerExceptions)
                {
                    if (e is OperationCanceledException) Console.WriteLine("Sum was canceled");
                    else Console.WriteLine("Sum threw: " + task.Exception);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            // 작업이 취소 되었을 경우 실행  
            t.ContinueWith(task =>
            {
                Console.Write("Sum was canceld");
            }, TaskContinuationOptions.OnlyOnCanceled);

            //cts.Cancel();

            t.Wait();
            Thread.Sleep(100);
        }

        private static int Sum(CancellationToken ct, int n)
        {
            int sum = 0;
            for (; n > 0; n--)
            {
                // ct가 참조하는 CancellationTokenSource 객체의 Cancel 메서드가 호출된 경우  
                // OperationCancelException을 던진다.  
                ct.ThrowIfCancellationRequested();

                checked { sum += n; }
            }

            return sum;
        }

    }
}
