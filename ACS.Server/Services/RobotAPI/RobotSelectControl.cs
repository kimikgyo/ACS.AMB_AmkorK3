using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        public Robot workRobot = null; // 작업 로봇

        private bool bStartup = true; // 최초실행 체크 플래그

        private List<Robot> GetActiveRobotsOrderbyDescendingBattery()
        {

            return ActiveRobots().OrderByDescending(r => r.BatteryPercent).ToList();
        }
        private List<Robot> GetActiveRobotsOrderbyDescendingBattery(string acsRobotGroup)
        {

            return ActiveRobots().Where(r => r.ACSRobotGroup == acsRobotGroup).OrderByDescending(r => r.BatteryPercent).ToList();
        }

        // 설정창에서 활성(active) 체크된 로봇들만 리턴한다
        private List<Robot> ActiveRobots()
        {

            #region Fleet Varsion

            var activeRobotList = uow.Robots.GetAll().Where(r => r.ConnectState 
                                         && r.FleetState != FleetState.None            //MiR 전원이 켜져있을경우
                                         && r.FleetState != FleetState.unavailable    //MiR 전원이 켜져있을경우
                                         && !string.IsNullOrWhiteSpace(r.RobotName)    //로봇상태값과 이름이있을경우
                                         && !string.IsNullOrWhiteSpace(r.StateText)    //로봇상태값과 이름이있을경우
                                         && r.ACSRobotGroup != "None"                  //로봇이 그룹이 설정되어있는것
                                         && r.ACSRobotActive == true).ToList();        //로봇이 active 설정되어있는것

            #endregion

            #region Robot Varsion

            //var activeRobotList = uow.Robots.GetAll().Where(r => r.ConnectState
            //                                   && !string.IsNullOrWhiteSpace(r.RobotName)    //로봇상태값과 이름이있을경우
            //                                   && !string.IsNullOrWhiteSpace(r.StateText)    //로봇상태값과 이름이있을경우
            //                                   && r.ACSRobotGroup != "None"                  //로봇이 그룹이 설정되어있는것
            //                                   && r.ACSRobotActive == true).ToList();        //로봇이 active 설정되어있는것
            #endregion
            return activeRobotList;
        }

        // 작업 로봇을 리턴한다
        private List<Robot> GetWorkRobot()
        {
            // 1. active 로봇 목록 가져온다
            var targetRobots = ActiveRobots();

            // 2. 프로그램 시작시 작업로봇 선택한다
            if (bStartup)
            {
                workRobot = SelectStartupWorkRobot(targetRobots);
                bStartup = false;
            }

            // 3. 특정조건만족시 작업로봇을 변경한다
            if (targetRobots.Count == 2)
            {
                // 로봇이 2대이고, 모두 active상태이고, 모두 충전위치에 있으면, 배터리가 많은 로봇을 선택한다
                var newWorkRobot = SelectNewWorkRobot(targetRobots);
                if (newWorkRobot != null)
                {
                    workRobot = newWorkRobot;
                }
            }
            else
            {
                // 로봇이 2대가 아닌 경우,
                // ** (연결 끊어진) 로봇의 위치를 알 수 없어서, 로봇을 선택하기 모호하다..
                // ** 유저가 임의로 선택시, 주행 경로상에 2대가 동시에 위치할 수도 있다..
                // case1. 작업로봇에 null할당하는 경우     ==>  현재 작업로봇이 없으므로 더이상 작업(post)하지 않는다
                // case2. 작업로봇에 null할당하지 않는 경우 ==>  현재 작업로봇으로 계속 작업(post)한다
                //workRobot = null;
            }

            // 4. 선택된 로봇을 리턴한다
            if (workRobot != null)
                return new List<Robot>() { workRobot };
            else
                return new List<Robot>();
        }

        // 프로그램 시작시 작업로봇을 선택한다
        private Robot SelectStartupWorkRobot(List<Robot> targetRobots)
        {
            // job이 할당되어 있거나 충전포지션이 아닌 곳에 있는 로봇을 선택한다

            foreach (var r in targetRobots)
            {
                if (r.JobId > 0)
                    return r;
                else if (RobotIsInChargingPosition(r) == false)
                    return r;
            }
            return null;
        }

        // 새로운 작업로봇을 선택한다
        private Robot SelectNewWorkRobot(List<Robot> targetRobots)
        {
            // 모든 로봇이 충전포지션에 있고, 레디상태 또는 충전미션실행중이면
            // 배터리가 많은 로봇을 선택한다

            bool allRobotIsChargingPosition = targetRobots.All(r => RobotIsInChargingPosition(r));
            bool allRobotIsReadyOrCharging = targetRobots.All(r => RobotIsReadyOrCharging(r));

            if (allRobotIsChargingPosition && allRobotIsReadyOrCharging)
            {
                return targetRobots.OrderByDescending(r => r.BatteryPercent).First();
            }
            return null;
        }

        // 로봇이 레디상태이거나 충전미션실행중인가?
        private bool RobotIsReadyOrCharging(Robot robot)
        {
            string robotMissionName = uow.Missions.Find(m => m.ReturnID == robot.MissionQueueID).FirstOrDefault()?.MissionName ?? string.Empty;
            bool isChargingMission = GetAllChargingMissionNames().Contains(robotMissionName);

            return robot.StateText == "Ready" || (robot.StateText == "Executing" && isChargingMission);
        }

        // 로봇이 충전 포지션에 있나?
        private bool RobotIsInChargingPosition(Robot robot)
        {
            if (robot?.PositionZoneName != null)
            {
                return GetAllChargingPositionNames().Contains(robot.PositionZoneName);
            }
            return false;
        }

        // 로봇이 충전 트리거 포지션에 있나?
        private bool RobotIsInChargingTriggerPosition(Robot robot)
        {
            var configs = GetChargingConfigs(robot.RobotName);

            foreach (var config in configs)
            {
                string chargingTriggerZoneName = config?.PositionZone;

                if (RobotIsInPosArea(robot.RobotName, chargingTriggerZoneName))
                    return true;
            }
            return false;
        }
    }
}
