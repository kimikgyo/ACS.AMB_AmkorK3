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
        /// Robot 위치 및 자재정보 변경
        /// </summary>
        private void PositionAreaUpdate()
        {
            try
            {
                //PositionArea Updata
                foreach (var robot in uow.Robots.GetAll())
                {
                    //자재 및 Door 초기화
                    if (!robot.ConnectState || string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                    {
                        robot_POSAreaNameUpdate(robot);
                        Upgrade_ProductState(robot);
                    }
                    else
                    {
                        Upgrade_ProductState(robot, true);
                        var positionAreaName = uow.PositionAreaConfigs.UpGrade_AllPositionArea(robot).FirstOrDefault();
                        if (positionAreaName == null) robot_POSAreaNameUpdate(robot);
                        else robot_POSAreaNameUpdate(robot, positionAreaName.PositionAreaName);
                    }

                }
            }

            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====
            //Robot Position Update
            void robot_POSAreaNameUpdate(Robot robot, string pOSZoneName = "")
            {
                if (robot.PositionZoneName != pOSZoneName)
                {
                    robot.PositionZoneName = pOSZoneName;
                    uow.Robots.Update(robot);
                }
            }

            void Upgrade_ProductState(Robot robot, bool Active = false)    //<========== [자재 내용Updata 및 Door 상태 Update]  
            {
                string Product = "";
                string Door = "";

                if (Active)
                {
                    if (!MiR_Get_Register(robot, 1)) return;
                    if (!MiR_Get_Register(robot, 2)) return;

                    if (robot.Registers.dMiR_Register_Value[1] > 0) Product = $"Product_{robot.Registers.dMiR_Register_Value[1]}";
                    //if (robot.Registers.dMiR_Register_Value[16] > 0) Product += $"Right_{robot.Registers.dMiR_Register_Value[16]}";

                    if (robot.Registers.dMiR_Register_Value[2] > 0) Door = $"Door열림";
                    else Door = $"Door닫힘";
                }

                else Product = "";

                //지금 현재 값과 다르면 DB Update
                if (robot.Product != Product)
                {
                    robot.Product = Product;
                    uow.Robots.Update(robot);
                }

                if (robot.Door != Door)
                {
                    robot.Door = Door;
                    uow.Robots.Update(robot);
                }
            }
        }

        /// <summary>
        /// Reset 미션 전송 제어
        /// </summary>
        private void JobReset()
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigData.jobResetRobotNo) && !string.IsNullOrEmpty(ConfigData.RobotResetMissionName))
                {
                    Robot robot = uow.Robots.GetByRobotName(ConfigData.jobResetRobotName);

                    if (robot != null)
                    {
                        // mir mission queue 비운다
                        if (DeleteMission(robot, null))
                        {
                            // 이 로봇에 보낸 특수미션들
                            var runMission_Specials = uow.Missions
                                                .Find(m => m.Robot == robot && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
                                                .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid") // 완료 미션은 제외한다
                                                .Where(m => m.JobId == 0) // 특수미션들
                                                .ToList();

                            foreach (var m in runMission_Specials)
                            {
                                uow.Missions.Remove(m);
                            }

                            // 해당 robot에 연관된 job 삭제
                            var job = uow.Jobs.GetById(robot.JobId);
                            if (job != null)
                            {
                                DeleteJobData(job, 3);
                            }

                            //리셋 미션 전송한다
                            SendMission(robot, new Mission
                            {
                                ACSMissionGroup = robot.ACSRobotGroup,
                                CallName = $"None_{ConfigData.RobotResetMissionName}",
                                MissionName = ConfigData.RobotResetMissionName
                            });
                        }
                    }

                    ConfigData.jobResetRobotNo = string.Empty;    // 다음 리셋 요청을 받을 수 있도록 하기 위해 클리어...
                    ConfigData.jobResetRobotName = string.Empty;
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }
        /// <summary>
        /// CallName[출발지_목적지] JobADD
        /// </summary>
        /// <param name="CallName"></param> 출발지_목적지
        /// <param name="robotName"></param> Robot 지정 경우 Robot 이름
        private void CallNameJobAdd(string CallName, string robotName = null)
        {
            var jobConfig = FindJobConfig_By_CallName(CallName);
            if (jobConfig != null)
            {
                var job = FindJob_By_CallName(jobConfig.CallName);
                if (job == null)
                {
                    JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = jobConfig.CallName, Extra4 = robotName, Extra5 = jobConfig.JobPriority });
                }
            }
        }


        /// <summary>
        /// 목적지 명칭으로 JobAdd
        /// </summary>
        /// <param name="name"></param> 목적지 이름
        /// <param name="robotName"></param> Robot 지정경우 Robot 이름
        private void EndZoneJobAdd(string EndZonename, string robotName = null)
        {
            string floorName = uow.FloorMapIDConfigs.GetAll().Select(f => f.FloorName).FirstOrDefault();
            if (floorName != null)
            {
                string displayname = $"{floorName}{EndZonename}";
                var jobConfig = FindJobConfig_By_EndPosName(displayname);
                if (jobConfig != null)
                {
                    var job = FindJob_By_CallName(jobConfig.CallName);
                    if (job == null)
                    {
                        JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = jobConfig.CallName, Extra4 = robotName, Extra5 = jobConfig.JobPriority });
                    }
                }
            }
        }

        /// <summary>
        /// 특정 Robot 특정Position 위치 있는지 확인
        /// </summary>
        /// <param name="robotName"></param>
        /// <param name="posName"></param>
        /// <returns></returns>
        private bool RobotIsInPosArea(string robotName, string posName)
        {
            var posArea = uow.PositionAreaConfigs.Find(p => p.PositionAreaName == posName).FirstOrDefault();

            var robot = uow.Robots.GetByRobotName(robotName);

            if (posArea == null || robot == null)
                return false;

            double pos_x = robot.Position_X;
            double pos_y = robot.Position_Y;

            return double.Parse(posArea.PositionAreaX1) < pos_x && double.Parse(posArea.PositionAreaX2) > pos_x
                && double.Parse(posArea.PositionAreaX3) > pos_x && double.Parse(posArea.PositionAreaX4) < pos_x
                && double.Parse(posArea.PositionAreaY1) < pos_y && double.Parse(posArea.PositionAreaY2) < pos_y
                && double.Parse(posArea.PositionAreaY3) > pos_y && double.Parse(posArea.PositionAreaY4) > pos_y;
        }

        /// <summary>
        /// CallName 으로 등록되어 있는 Job 찾기
        /// </summary>
        /// <param name="CallName"></param>
        /// <returns></returns>
        private Job FindJob_By_CallName(string CallName)
        {
            return uow.Jobs.Find(j => j.CallName == CallName).FirstOrDefault();
        }

        /// <summary>
        /// RobotName 으로 등록되어 있는 Job 찾기
        /// </summary>
        /// <param name="RobotName"></param>
        /// <returns></returns>
        private Job FindJob_By_RobotName(string RobotName)
        {
            return uow.Jobs.Find(j => j.RobotName == RobotName || j.JobCreateRobotName == RobotName).FirstOrDefault();
        }

        /// <summary>
        /// 출발지 포지션 이름으로 등록되어 있는 Job 찾기 
        /// </summary>
        /// <param name="startPosName"></param>
        /// <returns></returns>
        private Job FindJob_By_StartPosName(string startPosName)
        {
            return uow.Jobs.Find(j => j.CallName.StartsWith(startPosName)).FirstOrDefault();
        }

        /// <summary>
        /// 목적지 포지션 이름으로 등록되어 있는 Job 찾기
        /// </summary>
        /// <param name="endPosName"></param>
        /// <returns></returns>
        private Job FindJob_By_EndPosName(string endPosName)
        {
            return uow.Jobs.Find(j => j.CallName.EndsWith(endPosName)).FirstOrDefault();
        }

        /// <summary>
        /// 출발지 포지션 이름으로 설정되어 있는 JobConfig 찾기
        /// </summary>
        /// <param name="startPosName"></param>
        /// <returns></returns>
        private JobConfigModel FindJobConfig_By_StartPosName(string startPosName)
        {
            return uow.JobConfigs.Find(j => j.JobConfigUse == "Use" && j.CallName.StartsWith(startPosName)).FirstOrDefault();
        }

        /// <summary>
        /// 목적지 포지션 이름으로 설정되어 있는 JobConfig 찾기
        /// </summary>
        /// <param name="endPosName"></param>
        /// <returns></returns>
        private JobConfigModel FindJobConfig_By_EndPosName(string endPosName)
        {
            return uow.JobConfigs.Find(j => j.JobConfigUse == "Use" && j.CallName.EndsWith(endPosName)).FirstOrDefault();
        }

        /// <summary>
        /// CallName 으로 설정되어 있는 JobConfig 찾기 
        /// </summary>
        /// <param name="CallName"></param>
        /// <returns></returns>
        private JobConfigModel FindJobConfig_By_CallName(string CallName)
        {
            return uow.JobConfigs.Find(j => j.JobConfigUse == "Use" && j.CallName.EndsWith(CallName)).FirstOrDefault();
        }

        private bool JobMissionIsDone(Job job, int missionNo)
        {
            Mission m = job.Missions[missionNo]; // n번 미션
            return m?.MissionState == "Done";
        }

        private bool JobMissionIsDone(Job job, string missionNamePart)
        {
            Mission m = job.Missions.FirstOrDefault(x => x.MissionName.Contains(missionNamePart)); // 주어진 미션네임 일부를 포함하는 미션 찾는다
            return m?.MissionState == "Done";
        }


    }
}
