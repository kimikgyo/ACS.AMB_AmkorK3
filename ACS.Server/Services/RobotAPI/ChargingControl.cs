using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        //Charging 미션
        private void ChargingControl()
        {
            try
            {
                //(전체)진행중인 충전기 Count 수량을 확인한다.
                Upgrade_ChargerCountStatusUpdate();

                foreach (var robot in GetActiveRobotsOrderbyDescendingBattery())
                {
                    // running 미션 리스트
                    var runMissions = uow.Missions.GetAll()
                                                  .Where(m => m.ReturnID > 0)                                                           // 이미 전송한 미션 찾는다
                                                  .Where(m => new[] { m.RobotName, m.JobCreateRobotName }.Contains(robot.RobotName))    // 로봇네임이 일치하는 미션 찾는다
                                                  .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid").ToList();        // 완료되지 않은 미션 찾는다


                    // run미션중에서 충전미션을 찾는다
                    var runChargingMission = runMissions.Where(m => m.JobId == 0)
                                                        .Where(m => GetChargingConfigs(robot.RobotName, m.MissionName).Count() > 0).FirstOrDefault();

                    // 찾은 충전미션의 충전config를 찾는다
                    var runChargingConfig = GetChargingConfigs(runChargingMission?.RobotName, runChargingMission?.MissionName).FirstOrDefault();


                    // 충전 완료 삭제 (실행중인 미션이 있고 충전 미션일때)
                    bool c1 = runChargingConfig != null
                        && runMissions.Count > 0
                        && runChargingConfig.EndBattery <= robot.BatteryPercent;

                    // 충전 미션 전송 (실행중인 미션이 없을때)
                    bool c2 = robot.StateID == RobotState.Ready
                        && runChargingConfig == null
                        && runMissions.Count == 0;

                    if (c1)
                    {
                        //(전체)충전기 수량 확인
                        var chargerCount = GetChargerStatus(runChargingConfig.ChargerGroupName).FirstOrDefault();
                        ChangeMissionDelete(robot, chargerCount);
                    }

                    // 충전 미션 전송
                    else if (c2)
                    {
                        // 로봇의 충전config를 선택한다 (로봇의 네임/배터리/포지션 조건에 일치하는 충전config를 선택한다)
                        var selectedConfig = SelectChargingConfig(robot);

                        // 선택한 충전config의 설정값을 사용하여 충전미션을 보낸다
                        if (selectedConfig != null)
                        {
                            //충전기 수량 확인
                            var chargerCount = GetChargerStatus(selectedConfig.ChargerGroupName).FirstOrDefault(c => c.ChargerCountUse == "Use");
                            //스위칭 부분도 Send에서 한다
                            ChangeMssionSend(robot, selectedConfig, chargerCount);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

        //충전기 수량 동기화
        private void Upgrade_ChargerCountStatusUpdate()
        {
            // 충전config에 있는 모든 충전미션의 네임
            string[] allChargingMissionNames = GetAllChargingMissionNames();

            // 실행중인 스페셜미션을 찾는다
            var runSpecialMissions = uow.Missions.Find(m => m.JobId == 0 && m.ReturnID > 0 && m.MissionState != "Done"
                                          && GetActiveRobotsOrderbyDescendingBattery().Any(r => m.JobCreateRobotName == r.RobotName || m.RobotName == r.RobotName));

            // 스페셜미션중 충전미션만 찾는다
            var runChargingMissions = runSpecialMissions.Where(m => allChargingMissionNames.Contains(m.MissionName)).ToList();

            // 해당 충전미션의 충전설정을 찾는다
            var runChargingConfigs = (from c in uow.ChargeMissionConfigs.GetAll()
                                      from m in runChargingMissions
                                      where m.CallName == $"{c.PositionZone}_None"
                                      where m.MissionName == c.ChargeMissionName
                                      where m.RobotName == c.RobotName || m.JobCreateRobotName == c.RobotName
                                      select c).ToList();

            // 충전설정 중복항목 제거
            runChargingConfigs = runChargingConfigs.Distinct().ToList();

            //같은 충전 그룹을 찾아서 업데이트 한다
            foreach (var config in uow.ACSChargerCountConfigs.GetAll())
            {
                var chargerCount = runChargingConfigs.Where(x => x.ChargerGroupName == config.ChargerGroupName).Count();
                if (config.ChargerCountStatus != chargerCount)
                {
                    config.ChargerCountStatus = chargerCount;
                    uow.ACSChargerCountConfigs.Update(config);
                }
            }
        }

        //충전 미션 전송
        private void ChangeMssionSend(Robot robot, ChargeMissionConfigModel chargeConfig, ACSChargerCountConfigModel chargerCount)
        {
            if (chargerCount != null)
            {
                //충전기 설정수량보다 같거나 충전기 설정수량카운터보다 크면 충전을 시작해야하는 Robot보다 배터리가 큰 Robot 1대를 삭제후 전송한다
                if (chargerCount.ChargerCount <= chargerCount.ChargerCountStatus)
                {
                    // 스페셜 미션 List
                    var runMissions = uow.Missions.Find(m => m.JobId == 0 && m.ReturnID > 0 && m.MissionState != "Done");

                    // 스페셜 미션중 충전Mission을 검색한다
                    var runChargingMissions = uow.ChargeMissionConfigs.Find(r => r.ChargeMissionUse == "Use" && r.ChargerGroupName == chargerCount.ChargerGroupName
                                                && runMissions.Count(m => r.ChargeMissionName == m.MissionName && r.RobotName == m.RobotName) != 0).ToList();

                    // ActiveRobot 중 그룹이 같고 맵Id가 같지만 충전이 필요한 Robot 과 다른 Robot을 검색한다(배터리가 높은순으로 정렬한다)
                    var runChargingRobots = GetActiveRobotsOrderbyDescendingBattery(robot.ACSRobotGroup).Where(r => r.MapID == robot.MapID && r.RobotName != robot.RobotName).ToList();


                    // 지금 진행중인 충전Mission 에서 Robot 이름이 같고 스위칭 배터리보다 큰 Robot을 검색한다.
                    var deleteChargingRobot = runChargingRobots.Where(r =>
                                              runChargingMissions.Count(c => r.RobotName == c.RobotName && r.BatteryPercent > c.SwitchaingBattery) != 0).FirstOrDefault();



                    if (deleteChargingRobot != null)
                    {
                        ChangeMissionDelete(deleteChargingRobot, chargerCount);
                    }
                }

                //위에서 삭제후 카운터수량을 확인한뒤에 바로 전송하기위해서 else로 안하고 if 문으로 작업함.
                if (chargerCount.ChargerCount > chargerCount.ChargerCountStatus)
                {
                    if (DeleteMission(robot, null))
                    {
                        SendMission(robot, new Mission
                        {
                            ACSMissionGroup = robot.ACSRobotGroup,
                            CallName = $"{chargeConfig.PositionZone}_None",
                            MissionName = chargeConfig.ChargeMissionName
                        });

                        //충전기 상태 Count 를 +1 해준다.
                        chargerCount.ChargerCountStatus = chargerCount.ChargerCountStatus + 1;
                        uow.ACSChargerCountConfigs.Update(chargerCount);
                    }
                }
            }
            //충전기 카운터가 설정되지 않았을때 또는 Count수량이 있을시
            else
            {
                if (DeleteMission(robot, null))
                {
                    SendMission(robot, new Mission
                    {
                        ACSMissionGroup = robot.ACSRobotGroup,
                        CallName = $"{chargeConfig.PositionZone}_None",
                        MissionName = chargeConfig.ChargeMissionName
                    });

                }
            }
        }

        //충전 미션 삭제
        private void ChangeMissionDelete(Robot robot, ACSChargerCountConfigModel chargerCount)
        {
            if (chargerCount != null)
            {
                if (chargerCount.ChargerCountStatus > 0)
                {
                    chargerCount.ChargerCountStatus = chargerCount.ChargerCountStatus - 1;
                    uow.ACSChargerCountConfigs.Update(chargerCount);
                }
            }

            // 충전미션을 삭제한다
            if (DeleteMission(robot, null))
            {
                // 해당 mir에 이미 전송한 미션을 검색한다
                var runMission_Specials = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0)
                                                .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid")
                                                .Where(m => m.JobId == 0).ToList(); // 특수미션들

                foreach (var m in runMission_Specials)
                {
                    uow.Missions.Remove(m);
                }
            }

        }
    }
}

