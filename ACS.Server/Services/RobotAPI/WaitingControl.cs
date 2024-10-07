using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        private void WaitingControl(Robot WaitRobot = null)                  //<========== [자동 Waitting 미션]
        {

            //=============================================== 자동 대기 미션 =================================================//
            //1. Robot이 기본 조건 이고 레디 상태
            //2. Robot이 PositionAreaConfigs 설정된 포지션이 아닌 다른 포지션에 있을경우
            //3. 자재 경우 Waiting 포지션엔 자재가 있을시 수행해도 상관없으므로 자재 유무를 config에서 설정한다.
            //=================================================================================================================//


            try
            {
                //다른함수에서 Wait 위치로 보내지 않을시
                if (WaitRobot == null)
                {
                    foreach (var robot in GetActiveRobotsOrderbyDescendingBattery())
                    {
                        Waiting_Mission(false, robot);
                    }
                }
                //다른 함수 내에서 Wait미션을 전송할시
                else
                {
                    Waiting_Mission(true, WaitRobot);
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====

            //Waitting 보낼수있는 기본 조건 확인
            void Waiting_Mission(bool methodCall, Robot robot)
            {
                // running 미션 리스트
                var runMissions = uow.Missions.GetAll()
                                              .Where(m => m.ReturnID > 0)                                                           // 이미 전송한 미션 찾는다
                                              .Where(m => new[] { m.RobotName, m.JobCreateRobotName }.Contains(robot.RobotName))    // 로봇네임이 일치하는 미션 찾는다
                                              .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid").ToList();        // 완료되지 않은 미션 찾는다

                //Robot 이름으로 설정된 End Position이 다른것
                //var waitingEndPosition = GetWaitingConfigs(robot?.RobotName);

                //정상적으로 스레드로 WaitMission함수탔을경우
                bool c1 = methodCall == false
                    && robot.StateID == RobotState.Ready
                    && runMissions.Count == 0;

                //다른곳에서 함수를 호출하였을때
                bool c2 = methodCall == true
                     && runMissions.Count == 0;

                if (c1 || c2)
                {

                    var selectedConfig = SelectWaitingConfig(robot);
                    if (selectedConfig != null)
                    {
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

                            SendMission(robot, new Mission
                            {
                                ACSMissionGroup = robot.ACSRobotGroup,
                                CallName = $"None_{selectedConfig.PositionZone}",
                                MissionName = selectedConfig.WaitMissionName
                            });
                        }
                    }


                }
            }

        }
    }
}
