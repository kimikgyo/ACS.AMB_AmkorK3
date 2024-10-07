using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        #region Charge 

        private readonly string[] CHARGE_POSITION_NAMES = new string[] { "Charger1", "Charger2" };

        private string[] GetAllChargingPositionNames()
        {
            return CHARGE_POSITION_NAMES;
        }

        private List<PositionAreaConfig> GetAllChargingPositionConfigs()
        {
            return uow.PositionAreaConfigs.Find(x => GetAllChargingPositionNames().Contains(x.PositionAreaName));
        }


        private string[] GetAllChargingMissionNames()
        {
            return uow.ChargeMissionConfigs.Find(x => !string.IsNullOrEmpty(x.ChargeMissionName))
                                          .Select(x => x.ChargeMissionName).ToArray();
        }
        private List<ChargeMissionConfigModel> GetChargingConfigs(string robotName)
        {
            return uow.ChargeMissionConfigs.Find(config => config.ChargeMissionUse == "Use" && config.RobotName == robotName);
        }


        private List<ChargeMissionConfigModel> GetChargingConfigs(string robotName, string chargingMissionName)
        {
            return uow.ChargeMissionConfigs.Find(config => config.ChargeMissionUse == "Use" && config.RobotName == robotName && config.ChargeMissionName == chargingMissionName);
        }

        private List<ChargeMissionConfigModel> GetChargingConfigs(string robotName, double robotBatteryPercent)
        {
            return uow.ChargeMissionConfigs.Find(config =>
                           config.ChargeMissionUse == "Use"
                        && config.StartBattery >= robotBatteryPercent
                        && config.RobotName == robotName);
        }

        private List<ChargeMissionConfigModel> GetChargingConfigs(string robotName, double robotBatteryPercent, string robotPositionZoneName)
        {
            return uow.ChargeMissionConfigs.Find(config =>
                        !string.IsNullOrEmpty(robotPositionZoneName)
                        && config.ChargeMissionUse == "Use"
                        && config.StartBattery >= robotBatteryPercent
                        && config.PositionZone == robotPositionZoneName
                        && config.RobotName == robotName);
        }


        // 해당 로봇의 충전config중에서 조건에 맞는 충전config를 선택한다
        ChargeMissionConfigModel SelectChargingConfig(Robot robot)
        {
            // 조건에 맞는 충전config들을 가져온다 (로봇 네임 체크)
            //var chargingConfigs = GetChargingConfigs(robot.RobotName);

            // 조건에 맞는 충전config들을 가져온다 (로봇 네임/배터리 체크)
            //var chargingConfigs = GetChargingConfigs(robot.RobotName, robot.BatteryPercent);

            // 조건에 맞는 충전config들을 가져온다 (로봇 네임/배터리/포지션 체크)
            var chargingConfigs = GetChargingConfigs(robot.RobotName, robot.BatteryPercent, robot.PositionZoneName);

            #region 다른 로봇이 위치한 포지션영역과 관련된 충전config는 제외한다

            ////다른 robot 설정된 Position 위치 있고 미션이 수행중이 아닌로봇확인한다.
            //var otherRobots = uow.Robots.GetAll().Where(r => r.ACSRobotGroup == robot.ACSRobotGroup && r.RobotName != robot.RobotName
            //                                           && r.ConnectState
            //                                           && !string.IsNullOrWhiteSpace(r.RobotName) && !string.IsNullOrWhiteSpace(r.StateText)
            //                                           && !string.IsNullOrEmpty(robot.PositionZoneName)).ToList();

            //// 다른 로봇들이 위치한 포지션영역과 관련된 충전config는 제외한다
            //var otherRobotPositionZones = otherRobots.Select(r => r.PositionZoneName).ToList();
            //chargingConfigs = chargingConfigs.Where(cfg => otherRobotPositionZones.Contains(cfg.PositionZone) == false).ToList();

            #endregion


            // 충전config 중에서 1개를 선택한다 ( robot 모든 조건이 일치하고 충전 그룹도 일치한 그룹을 설정한다. )
            if (chargingConfigs.Count > 0)
            {
                ChargeMissionConfigModel selectedConfig = null;

                // 자재 있을때 config
                if (!string.IsNullOrEmpty(robot.Product))
                {
                    selectedConfig = chargingConfigs.FirstOrDefault(cfg => cfg.ProductActive == true && cfg.ProductValue == true);
                }
                else
                {
                    selectedConfig = chargingConfigs.FirstOrDefault(cfg => cfg.ProductActive == true && cfg.ProductValue == false);
                }

                // 자재 없을때 config (만일, 자재 있을때의 config가 없으면 여기를 탄다)
                if (selectedConfig == null)
                {
                    selectedConfig = chargingConfigs.FirstOrDefault(cfg => cfg.ProductActive == false);
                }

                return selectedConfig;
            }

            return null;
        }

        private List<ACSChargerCountConfigModel> GetChargerStatus(string chargerGroup)
        {
            return uow.ACSChargerCountConfigs.Find(config => config.ChargerGroupName == chargerGroup);
        }
        #endregion

        #region Wait 

        WaitMissionConfigModel SelectWaitingConfig(Robot robot)
        {
            // 조건에 맞는 대기위치config들을 가져온다 (로봇 네임 체크)
            //var waitingConfigs = GetWaitingConfigs(robot.RobotName);

            // 조건에 맞는 대기위치config들을 가져온다 (로봇 네임/포지션 체크)
            //var waitingConfigs = GetWaitingConfigs(robot.RobotName, robot.PositionZoneName);

            // 조건에 맞는 대기위치config들을 가져온다 (로봇 네임/배터리/포지션 체크)
            var waitingConfigs = GetWaitingConfigs(robot.RobotName, robot.BatteryPercent, robot.PositionZoneName);

            #region 다른 로봇이 위치한 포지션영역과 관련된 대기위치config는 제외한다

            //WaitConfig 에서 로봇정보로 조회한다.(층위치(floor)는 비교하지 않고, 갈수있는 Waitting 미션을 조회한다.)
            //현재 수행중인 미션
            var runmission = uow.Missions.Find(m => m.ACSMissionGroup == robot.ACSRobotGroup && m.MissionState != "Done");

            //현재 Job"_"배열로 나누어서 마지막 값만 가지고온다(목적지)[같은 WaitPosition으로 가는것방지]
            var missionEndPosition = runmission.Select(s => s.CallName.Split('_').LastOrDefault()).ToList();

            var robotPosition = uow.Robots.GetAll().Where(r => r.ACSRobotGroup == robot.ACSRobotGroup && r.RobotName != robot.RobotName
                                                             && r.ConnectState
                                                             && !string.IsNullOrWhiteSpace(r.RobotName)
                                                             && !string.IsNullOrWhiteSpace(r.StateText)).Select(r => r.PositionZoneName).ToList();
            var notOverlapPosition = new List<string>();
            notOverlapPosition.AddRange(missionEndPosition);
            notOverlapPosition.AddRange(robotPosition);

            waitingConfigs = waitingConfigs.Where(cfg => notOverlapPosition.Contains(cfg.PositionZone) == false).ToList();

            #endregion


            // 충전config 중에서 1개를 선택한다 ( robot 모든 조건이 일치하고 충전 그룹도 일치한 그룹을 설정한다. )
            if (waitingConfigs.Count > 0)
            {
                WaitMissionConfigModel selectedConfig = null;

                // 자재 있을때 config
                if (!string.IsNullOrEmpty(robot.Product))
                {
                    selectedConfig = waitingConfigs.FirstOrDefault(cfg => cfg.ProductActive == true && cfg.ProductValue == true);
                }
                else
                {
                    selectedConfig = waitingConfigs.FirstOrDefault(cfg => cfg.ProductActive == true && cfg.ProductValue == false);
                }

                // 자재 없을때 config (만일, 자재 있을때의 config가 없으면 여기를 탄다)
                if (selectedConfig == null)
                {
                    selectedConfig = waitingConfigs.FirstOrDefault(cfg => cfg.ProductActive == false);
                }

                return selectedConfig;
            }

            return null;
        }


        private List<WaitMissionConfigModel> GetWaitingConfigs(string robotName)
        {
            return uow.WaitMissionConfigs.Find(config => config.WaitMissionUse == "Use" && config.RobotName == robotName);
        }


        private List<WaitMissionConfigModel> GetWaitingConfigs(string robotName, string waitingMissionName)
        {
            return uow.WaitMissionConfigs.Find(config => config.WaitMissionUse == "Use" && config.RobotName == robotName && config.WaitMissionName == waitingMissionName);
        }

        private List<WaitMissionConfigModel> GetWaitingConfigs(string robotName, double robotBatteryPercent, string robotPositionZoneName)
        {
            return uow.WaitMissionConfigs.Find(config =>
                        config.WaitMissionUse == "Use"
                        && config.EnableBattery <= robotBatteryPercent
                        && config.PositionZone != robotPositionZoneName
                        && config.RobotName == robotName);
        }

        #endregion

    }
}
