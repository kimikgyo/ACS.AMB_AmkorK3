using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {
        private readonly static ILog TabletLogger = LogManager.GetLogger("TabletEvent");

        private enum KGCBuyeo_PLCEntryNo
        {
            //PLC Entry 레지스터 번호
            차류포장 = 30,
            액상병입 = 31,
            캔포장실 = 32,
            기타작업장 = 33,
        }

        private enum KGCBuyeo_WiseResetNo
        {
            //PLC WiseModule 레지스터 번호
            차류포장 = 34,
            액상병입 = 35,
            캔포장실 = 36,
            기타작업장 = 37,
        }

        private void KGCBuyeo_JobStartControl_Call()                                              //<==========미션 상태 Start 변경
        {

            //=============================================== 미션 상태 Start =================================================//
            //1.Init으로 등록되어 있는 job 을 Start로 상태를 변경한다.
            //=================================================================================================================//
            try
            {
                var jobs = uow.Jobs.Find(x => (x.ACSJobGroup != "None") && x.JobState == JobState.JobInit);
                foreach (var job in jobs)
                {
                    // ===============================================
                    // job 시작 ~~~~~~~~~~~~

                    job.JobState = JobState.JobStart;
                    uow.Jobs.Update(job);
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void KGCBuyeo_PositionAreaUpdate()                                                //<========== [Robot위치 및 자재정보]
        {
            try
            {
                //PositionArea Updata
                foreach (var robot in uow.Robots.GetAll())
                {
                    if ((robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                        || (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText)))
                    //|| !robot.ACSRobotActive || robot.ACSRobotGroup == "None")
                    {
                        //Position이름을 초기화
                        robot_POSAreaNameUpdate(robot);
                        //자재 및 Door 초기화
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
                LogExceptionMessage(ex);
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


                if (Active)
                {

                    if (!MiR_Get_Register(robot, 1)) return;
                    if (robot.Registers.dMiR_Register_Value1 > 0) Product = "제품있음";
                    else Product = "";
                }
                else Product = "";

                //지금 현재 값과 다르면 DB Update
                if (robot.Product != Product)
                {
                    robot.Product = Product;
                    uow.Robots.Update(robot);
                }
            }
        }

        private void KGCBuyeo_TabletMissionStatusUpdate(int TabletDbId, int jobId, string status) //<========== [Tablet 상태값 변경]
        {
            try
            {
                var TabletMission = uow.TabletMissionStatus.GetById(TabletDbId);
                if (TabletMission != null)
                {
                    if (TabletMission.JobId == 0 || TabletMission.CALLFLAG != status)
                    {
                        TabletMission.JobId = jobId;
                        TabletMission.CALLFLAG = status;
                        uow.TabletMissionStatus.Update(TabletMission);
                    }

                    //완료된 데이터는 삭제한다
                    if (TabletMission.CALLFLAG == "JobDone")
                    {
                        foreach (var tabletJobDone in uow.TabletMissionStatus.GetbyCallName(TabletMission.CALLNAME).ToList())
                        {
                            uow.TabletMissionStatus.Remove(tabletJobDone);

                            TabletLogger.Info($"JobDone, TabletId : {tabletJobDone.SEQ}, JobId : {tabletJobDone.JobId}, Name : {tabletJobDone.CALLNAME},  CallFlag : {tabletJobDone.CALLFLAG}, CancelFlag : {tabletJobDone.CANCELFLAG}");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void KGCBuyeo_RegisterSync()                                                      //========== [레지스터 Sync]
        {
            try
            {
                //Robot이 설정한 위치에 있으면 위치에 있는 로봇과 같은 Group 으로 설정된 Robot에게 전부 레지스터를 전송한다.

                //레지스터 싱크 설정 Use 이고 그룹이 None 아닌 상태인 항목만 레지스터를 공유한다
                var RegisterSyncs = uow.RobotRegisterSync.Find(r => r.RegisterSyncUse == "Use" && r.ACSRobotGroup != "None" && r.PositionGroup != "None" && r.PositionName != "None" && r.RegisterNo > 0).ToList();

                //2.레지스터 싱크 활성화가 되어있는것
                foreach (var RegisterSync in RegisterSyncs)
                {
                    bool RegisterSyncFlag = false;

                    //3.싱크 활성화 되어있는 목록중에 Robot그룹이 일치한것을 Robot을 검색한다.
                    var GroupRobot = ActiveRobots().Where(a => a.ACSRobotGroup == RegisterSync.ACSRobotGroup).ToList();

                    foreach (var robot in GroupRobot)
                    {
                        //RegiaterSyncGroup 포지션 일치하는 로봇을 찾는다.
                        var PositionRobot = uow.PositionAreaConfigs.UpGrade_GroupPOSArea(robot, RegisterSync.PositionGroup).FirstOrDefault();
                        if (PositionRobot != null)
                        {
                            if (PositionRobot.PositionAreaName == RegisterSync.PositionName)
                            {
                                RegisterSyncFlag = true;
                                break;
                            }

                        }
                    }
                    if (RegisterSyncFlag)
                    {
                        foreach (var robot in GroupRobot)
                        {
                            MiR_Put_Register(robot, RegisterSync.RegisterNo, RegisterSync.RegisterValue);
                        }
                    }
                    else
                    {
                        foreach (var robot in GroupRobot)
                        {
                            MiR_Put_Register(robot, RegisterSync.RegisterNo, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void KGCBuyeo_PositionPutRegister()                                               //<========== 사용안함 [위치 있는 Robot PutRegister]
        {
            //해당 포지션에 위치한 Robot 한테만 Register를 전송한다.

            var PositionPutRegisterList = uow.RobotPositionPutRegister.Find(r => r.PutRegisterUse == "Use" && r.PositionGroup != "None" && r.PositionName != "None" && r.RegisterNo > 0).ToList();

            //1.Position Put Register 활성화가 되어있는것
            foreach (var positionPutRegister in PositionPutRegisterList)
            {

                foreach (var robot in ActiveRobots().ToList())
                {
                    //Position Put Register 포지션 일치하는 로봇을 찾는다.
                    var PositionRobot = uow.PositionAreaConfigs.UpGrade_GroupPOSArea(robot, positionPutRegister.PositionGroup).FirstOrDefault();
                    if (PositionRobot != null)
                    {
                        if (PositionRobot.PositionAreaName == positionPutRegister.PositionName)
                        {
                            MiR_Put_Register(robot, positionPutRegister.RegisterNo, positionPutRegister.RegisterValue);

                        }
                        else
                        {
                            MiR_Put_Register(robot, positionPutRegister.RegisterNo, 0);
                        }

                    }
                    else
                    {
                        MiR_Put_Register(robot, positionPutRegister.RegisterNo, 0);
                    }

                }
            }
        }

        private void KGCBuyeo_AutoWaitingControl(Robot WaitRobot = null)                          //<========== [자동 Waitting 미션]
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
                    foreach (var robot in ActiveRobots())
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
                LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====

            void Waiting_Mission(bool methodCall, Robot robot)
            {
                WaitMissionConfigModel waitMissionSelect = null;

                // 해당 mir에 이미 전송한 미션을 검색한다 (대기 및 충전 보내는 미션을 확인)
                var runMission = uow.Missions.Find(m => (m.Robot == robot || m.JobCreateRobotName == robot.RobotName) && m.ReturnID > 0 && m.MissionState != "Done").FirstOrDefault();

                //현재Robot 있는 층수를 확인한다 
                var robotFloorName = uow.FloorMapIDConfig.Find(n => n.MapID == robot.MapID).FirstOrDefault();

                //Robot 이름으로 설정된 End Position이 다른것
                var waitingEndPosition = uow.WaitMissionConfig.Find(x => x.WaitMissionUse == "Use" && x.RobotName == robot.RobotName).FirstOrDefault(p => p.PositionZone == robot.PositionZoneName);

                //정상적으로 스레드로 WaitMission함수탔을경우
                bool c1 = methodCall == false
                    && robot.StateID == RobotState.Ready
                    && runMission == null
                    && waitingEndPosition == null
                    && robotFloorName != null;
                //&& string.IsNullOrEmpty(robot.PositionZoneName);

                //다른곳에서 함수를 호출하였을때
                bool c2 = methodCall == true
                     && runMission == null
                     && waitingEndPosition == null
                     && robotFloorName != null;

                if (c1 || c2)
                {

                    var waitingConfigs = WaitingConfigSelect(robot, robotFloorName.FloorName);

                    if (waitingConfigs.Count > 0)
                    {
                        //자재 활성화가 사용 / 자재를 사용 / 현재 Robot 자재 있음
                        waitMissionSelect = waitingConfigs.Where(w => w.ProductActive == true && w.ProductValue == !string.IsNullOrEmpty(robot.Product)).FirstOrDefault();

                        if (waitMissionSelect == null)
                        {
                            //자재 활성화가 미사용 / 자재를 미사용 / 현재 Robot 자재 없음
                            waitMissionSelect = waitingConfigs.Where(w => w.ProductActive == false).FirstOrDefault();
                        }
                    }
                }

                if (waitMissionSelect != null)
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
                            CallName = $"None_{waitMissionSelect.PositionZone}",
                            MissionName = waitMissionSelect.WaitMissionName
                        });
                    }
                }
            }

            List<WaitMissionConfigModel> WaitingConfigSelect(Robot robot, string robotFloorName)
            {
                //WaitConfig 에서 로봇정보로 조회한다.(층수를 비교하여 현재층수에서 갈수있는 Waitting 미션을 조회한다.)
                var waitingmissionConfigs = uow.WaitMissionConfig.Find(f => f.WaitMissionUse == "Use" && !string.IsNullOrEmpty(robotFloorName) && f.PositionZone.StartsWith(robotFloorName)
                                                                            && f.RobotName == robot.RobotName && f.EnableBattery <= robot.BatteryPercent);

                return waitingmissionConfigs;
            }
        }

        private void KGCBuyeo_AutoChargingControl()                                               //<========== [자동 Charging 미션]
        {
            try
            {
                //진행중인 충전기 Count 수량을 확인한다.
                Upgrade_ChargerCountStatusUpdate();

                foreach (var robot in ActiveRobots())
                {
                    ChargeMissionConfigModel ChargingMissionSelect = null;

                    //충전 미션이 실행중인지 확인하기 위하여 List 형태로 Return 받는다.
                    var runMission = uow.Missions.Find(m => (m.Robot == robot || m.JobCreateRobotName == robot.RobotName) && m.ReturnID > 0 && m.MissionState != "Done");

                    //스페셜 미션중 충전중인 미션을 조회한다
                    var runchargingmission = uow.ChargeMissionConfig.Find(x => x.RobotName == robot.RobotName)
                                                                     .Where(x => runMission.Count(r => $"{x.PositionZone}_None" == r.CallName
                                                                             && x.ChargeMissionName == r.MissionName) != 0).FirstOrDefault();

                    // 충전 완료 삭제 (실행중인 미션이 있고 충전 미션일때)
                    bool c1 = runchargingmission != null
                        && runMission.Count > 0
                        && runchargingmission.EndBattery <= robot.BatteryPercent;

                    // 충전 미션 전송 (실행중인 미션이 없을때)
                    bool c2 = robot.StateID == RobotState.Ready
                        && runchargingmission == null
                        && runMission.Count == 0;

                    if (c1)
                    {
                        //충전기 수량 확인
                        var chargerCount = uow.ACSChargerCountConfig.Find(c => c.ChargerCountUse == "Use"
                                                                      && c.ChargerGroupName == runchargingmission.ChargerGroupName).FirstOrDefault();
                        ChangeMissionDelete(robot, chargerCount);

                    }

                    // 충전 미션 전송
                    else if (c2)
                    {

                        //robot 위치에 맞는 충전 미션을 확인한다.
                        var chargeMissionConfig = uow.ChargeMissionConfig.Find(c => !string.IsNullOrEmpty(robot.PositionZoneName) && c.StartBattery >= robot.BatteryPercent
                                                                        && c.ChargeMissionUse == "Use" && c.PositionZone == robot.PositionZoneName && c.RobotName == robot.RobotName);

                        //robot 모든 조건이 일치하고 충전 그룹도 일치한 그룹을 설정한다.
                        if (chargeMissionConfig.Count > 0)
                        {
                            //자재 활성화가 사용 / 자재를 사용 / 현재 Robot 자재 있음
                            ChargingMissionSelect = chargeMissionConfig.Where(w => w.ProductActive == true && w.ProductValue == !string.IsNullOrEmpty(robot.Product)).FirstOrDefault();

                            if (ChargingMissionSelect == null)
                            {
                                //자재 활성화가 미사용 / 자재를 미사용 / 현재 Robot 자재 없음
                                ChargingMissionSelect = chargeMissionConfig.Where(w => w.ProductActive == false).FirstOrDefault();
                            }
                        }
                    }
                    if (ChargingMissionSelect != null)
                    {
                        //충전기 수량 확인
                        var chargerCount = uow.ACSChargerCountConfig.Find(c => c.ChargerCountUse == "Use"/* && c.RobotGroupName == robot.ACSRobotGroup && c.FloorMapId == robot.MapID*/
                                                                      && c.ChargerGroupName == ChargingMissionSelect.ChargerGroupName).FirstOrDefault();
                        //스위칭 부분도 Send에서 한다
                        ChangeMssionSend(robot, ChargingMissionSelect, chargerCount);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====

            // ========== 진행중인 충전기 Count 수량을 확인한다.
            void Upgrade_ChargerCountStatusUpdate()                               //<========== [충전기 수량 동기화]
            {

                //스페셜 미션을 조회한다
                var Robot = uow.Missions.Find(x => x.JobId == 0 && x.ReturnID > 0 && x.MissionState != "Done"
                                              && ActiveRobots().Count(a => x.JobCreateRobotName == a.RobotName || x.RobotName == a.RobotName) != 0).ToList();

                //스페셜 미션중 충전중인 미션을 조회한다
                var Charger = uow.ChargeMissionConfig.Find(x => Robot.Count(r => $"{x.PositionZone}_None" == r.CallName
                                                        && x.ChargeMissionName == r.MissionName
                                                        && (x.RobotName == r.JobCreateRobotName || x.RobotName == r.RobotName)) != 0).ToList();

                //같은 충전 그룹을 찾아서 업데이트 한다
                foreach (var config in uow.ACSChargerCountConfig.GetAll())
                {
                    var ChargerStatusCount = Charger.Where(c => c.ChargerGroupName == config.ChargerGroupName).Count();
                    if (config.ChargerCountStatus != ChargerStatusCount)
                    {
                        config.ChargerCountStatus = ChargerStatusCount;
                        uow.ACSChargerCountConfig.Update(config);
                    }
                }

            }

            //충전 미션 전송
            void ChangeMssionSend(Robot robot, ChargeMissionConfigModel chargeMission, ACSChargerCountConfigModel chargerCount)
            {
                if (chargerCount != null)
                {
                    //충전기 설정수량보다 같거나 충전기 설정수량카운터보다 크면 충전을 시작해야하는 Robot보다 배터리가 큰 Robot 1대를 삭제후 전송한다
                    if (chargerCount.ChargerCount <= chargerCount.ChargerCountStatus)
                    {
                        // 스페셜 미션을 진행중인 List 를 확인한다 
                        var runmission = uow.Missions.Find(m => m.JobId == 0 && m.ReturnID > 0 && m.MissionState != "Done");

                        // 스페셜 미션중 충전Mission을 검색한다
                        var runChargeingMissions = uow.ChargeMissionConfig.Find(r => r.ChargeMissionUse == "Use" && r.ChargerGroupName == chargerCount.ChargerGroupName
                                                    && runmission.Count(m => r.ChargeMissionName == m.MissionName && r.RobotName == m.RobotName) != 0).ToList();

                        // ActiveRobot 중 그룹이 같고 맵Id가 같지만 충전이 필요한 Robot 과 다른 Robot을 검색한다(배터리가 높은순으로 정렬한다)
                        var runChargeingRobots = ActiveRobots().Where(r => r.ACSRobotGroup == robot.ACSRobotGroup
                                                      && r.MapID == robot.MapID && r.RobotName != robot.RobotName).OrderByDescending(x => x.BatteryPercent).ToList();

                        // 지금 진행중인 충전Mission 에서 Robot 이름이 같고 스위칭 배터리보다 큰 Robot을 검색한다.
                        var DeleteChargeingRobot = runChargeingRobots.Where(r =>
                                                  runChargeingMissions.Count(c => r.RobotName == c.RobotName && r.BatteryPercent > c.SwitchaingBattery) != 0).FirstOrDefault();



                        if (DeleteChargeingRobot != null)
                        {
                            ChangeMissionDelete(DeleteChargeingRobot, chargerCount);
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
                                CallName = $"{chargeMission.PositionZone}_None",
                                MissionName = chargeMission.ChargeMissionName
                            });

                            //충전기 상태 Count 를 +1 해준다.
                            chargerCount.ChargerCountStatus = chargerCount.ChargerCountStatus + 1;
                            uow.ACSChargerCountConfig.Update(chargerCount);
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
                            CallName = $"{chargeMission.PositionZone}_None",
                            MissionName = chargeMission.ChargeMissionName
                        });

                    }
                }
            }

            //충전 미션 삭제
            void ChangeMissionDelete(Robot robot, ACSChargerCountConfigModel chargerCount)
            {
                if (chargerCount != null)
                {
                    if (chargerCount.ChargerCountStatus > 0)
                    {
                        chargerCount.ChargerCountStatus = chargerCount.ChargerCountStatus - 1;
                        uow.ACSChargerCountConfig.Update(chargerCount);
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
                    //Upgrade_AmkorK5_AutoWaiting_Mission(robot);
                }

            }
        }

        private void KGCBuyeo_Job_PostMission()                                                   //<========== [미션 전송]
        {
            try
            {
                Job job = null;
                Job jobCreateRobot = null;  //Robot 선택하여 미션 전송

                foreach (var robot in ActiveRobots().OrderByDescending(x=>x.BatteryPercent).ToList())
                {

                    var runMissions = uow.Missions.Find(m => (m.Robot == robot || m.JobCreateRobotName == robot.RobotName) && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
                                                  .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid").ToList(); // 완료 미션은 제외한다

                    var runMission_Job = runMissions.Where(m => m.JobId != 0).SingleOrDefault(); // 일반미션

                    //충전 Config 에서 일치하는 값이 있는지 확인 runMission_Specials 과 일치하는 값이 있는지 확인
                    var runCharging_Specials = runMissions.Where(r => r.JobId == 0 && uow.ChargeMissionConfig.GetAll()
                                                          .Count(c => r.MissionName == c.ChargeMissionName && robot.RobotName == c.RobotName) != 0).ToList();
                    //대기 Config 에서 일치하는 값이 있는지 확인
                    var runWaiting_Specials = runMissions.Where(r => r.JobId == 0 && uow.WaitMissionConfig.GetAll()
                                                         .Count(c => r.MissionName == c.WaitMissionName && robot.RobotName == c.RobotName) != 0).ToList();

                    var RobotResetMission = runMissions.FirstOrDefault(r => r.MissionName == ConfigData.RobotResetMissionName && r.RobotName == robot.RobotName);

                    //충전 및 대기 Config 일치하는 값을 확인한 List를 합침
                    var runMission_Specials = runCharging_Specials.Concat(runWaiting_Specials).ToList();

                    // (조건1) 이 로봇이 실행중인 미션이 없을때
                    bool c1 = robot.ACSRobotGroup != "None"
                           && robot.StateID == RobotState.Ready
                           && runMission_Job == null
                           && RobotResetMission == null;

                    // (조건2) 이 로봇이 특수미션을 실행중일때 (자동충전미션 / 자동대기위치미션)
                    bool c2 = robot.StateID == RobotState.Executing
                         && runMission_Specials.Count() > 0
                         && RobotResetMission == null;

                    if (c1 || c2)
                    {
                        // ==== select job ====

                        // 이 로봇에 job이 할당되어 있으면 수행하던 executing job을 선택하고,
                        // 아니면 다음 waiting job을 찾는다

                        // get executing job
                        if (robot.JobId != 0)
                        {
                            //로봇 지정
                            jobCreateRobot = uow.Jobs.Find(j => j.Id == robot.JobId && j.JobCreateRobotName == robot.RobotName).SingleOrDefault(); // executing job
                                                                                                                                                   //if (jobCreateRobot == null) EventLogger.Info($"jobCreateRobot {robot.JobId} not found!");

                            //로봇지정이 아닌 스케쥴
                            job = uow.Jobs.Find(j => j.Id == robot.JobId && string.IsNullOrWhiteSpace(j.JobCreateRobotName)).SingleOrDefault(); // executing job
                                                                                                                                                //if (job == null) EventLogger.Info($"job {robot.JobId} not found!");

                            if (job == null && jobCreateRobot == null) EventLogger.Info($"job {robot.JobId} not found!");
                        }
                        // get next waiting job
                        else
                        {
                            if (!MiR_Get_Register(robot, 1)) return; //MiR 자재 유무 판단

                            //로봇 지정
                            jobCreateRobot = uow.Jobs.Find(j =>
                            {
                                bool flag = true;
                                flag &= j.JobState == JobState.JobWaiting;          //waiting job
                                flag &= j.MissionSentCount == 1;                    //first mission
                                flag &= j.ACSJobGroup == robot.ACSRobotGroup;       //robot 그룹과 일치하는 미션을 전송하기 위함
                                flag &= j.JobCreateRobotName == robot.RobotName;    //job 생성시로봇과 현재로봇을비교 (Amkor K5 M3F_T3F층간이송)
                                                                                    //flag &= j.ExecuteBattery == robot.BatteryPercent; //jobConfig 설정된 배터리 용량(Robot지정경우 jobAdd할때 배터리용량을 확인후 job생성)
                                return flag;
                            })
                                            .FirstOrDefault();

                            //로봇지정이 아닌 스케쥴
                            job = uow.Jobs.Find(j =>
                            {
                                bool flag = true;
                                flag &= j.JobState == JobState.JobWaiting;          // waiting job
                                flag &= j.MissionSentCount == 1;                    // first mission
                                flag &= j.ACSJobGroup == robot.ACSRobotGroup;       // robot 그룹과 일치하는 미션을 전송하기 위함
                                flag &= string.IsNullOrEmpty(j.JobCreateRobotName); // 이 경우는 JobCreateRobotName 사용하지 않는다!
                                flag &= j.ExecuteBattery <= robot.BatteryPercent;   //jobConfig 설정된 배터리 용량
                                flag &= robot.Registers.dMiR_Register_Value1 == 0;  //자재가 없는경우
                                return flag;
                            })
                                            .FirstOrDefault();

                        }

                        // ==== jobCreateRobot select mission ====
                        //로봇 지정
                        if (jobCreateRobot != null)
                        {
                            IEnumerable<Mission> missions = uow.Missions.GetAll();
                            missions = missions.Where(m => m.ACSMissionGroup == robot.ACSRobotGroup).ToList();
                            missions = missions.Where(m => m.JobId == jobCreateRobot.Id).ToList();                          // 해당 job의 미션들
                            missions = missions.Where(m => m.ReturnID == 0).ToList();                            // 아직 전송하지 않은 미션들
                            missions = missions.Where(m => m.MissionState == "Waiting").ToList();                // 전송대기상태인 미션들........Pending 상태인 것 제외?
                            missions = missions.Where(m => !string.IsNullOrWhiteSpace(m.CallName) && !string.IsNullOrWhiteSpace(m.MissionName) && m.MissionName != "None").ToList(); // 콜버튼네임,미션네임 유효인가?
                            var CreateRobotnextMission = missions.FirstOrDefault(m => m.JobCreateRobotName == robot.RobotName);

                            // ==== send mission ====

                            if (CreateRobotnextMission != null) //지정한 Robot 이 있을경우
                            {
                                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                // job에 지정된 로봇과 일치하는지 확인한다 (첫미션은 로봇 지정안됨)
                                // 로봇을 지정하여 보내야함 (Amkor K5 M3F_T3F층간이송)
                                bool isFirstMission = CreateRobotnextMission.Id == jobCreateRobot.Missions[0].Id;
                                bool isRobotMatched = isFirstMission ? CreateRobotnextMission.Robot == null : CreateRobotnextMission.Robot == robot;
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
                                    foreach (var m in runMission_Specials)
                                    {
                                        uow.Missions.Remove(m);
                                    }
                                    if (SendMission(robot, CreateRobotnextMission))
                                    {
                                        // ===== job 상태 갱신
                                        jobCreateRobot.RobotName = robot.RobotName;        // job로봇 지정

                                        if (CreateRobotnextMission.Id == jobCreateRobot.MissionIds[0]) // job상태는 첫미션전송시에만 변경
                                        {
                                            jobCreateRobot.JobState = JobState.JobExecuting;   // job상태 변경(JobExecuting)
                                        }

                                        uow.Jobs.Update(jobCreateRobot);

                                        // ===== robot 상태 갱신
                                        robot.JobId = jobCreateRobot.Id;                   // 로봇에 job id 지정
                                        uow.Robots.Update(robot);

                                    }
                                }
                            }
                        }

                        // ==== select mission ====
                        //로봇지정이 아닌 스케쥴
                        else if (job != null)
                        {
                            IEnumerable<Mission> missions = uow.Missions.GetAll();
                            missions = missions.Where(m => m.ACSMissionGroup == robot.ACSRobotGroup).ToList();
                            missions = missions.Where(m => m.JobId == job.Id).ToList();                          // 해당 job의 미션들
                            missions = missions.Where(m => m.ReturnID == 0).ToList();                            // 아직 전송하지 않은 미션들
                            missions = missions.Where(m => m.MissionState == "Waiting").ToList();                // 전송대기상태인 미션들........Pending 상태인 것 제외?
                            missions = missions.Where(m => !string.IsNullOrWhiteSpace(m.CallName) && !string.IsNullOrWhiteSpace(m.MissionName) && m.MissionName != "None").ToList(); // 콜버튼네임,미션네임 유효인가?
                            var nextMission = missions.FirstOrDefault();

                            // ==== send mission ====

                            if (nextMission != null)      //지정 Robot 이 없을경우
                            {
                                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                // job에 지정된 로봇과 일치하는지 확인한다 (첫미션은 로봇 지정안됨)
                                // 로봇을 지정하여 보내야함 (Amkor K5 M3F_T3F층간이송)
                                bool isFirstMission = nextMission.Id == job.Missions[0].Id;
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
                                    foreach (var m in runMission_Specials)
                                    {
                                        uow.Missions.Remove(m);
                                    }

                                    if (SendMission(robot, nextMission))
                                    {

                                        // ===== job 상태 갱신
                                        job.RobotName = robot.RobotName;        // job로봇 지정

                                        if (nextMission.Id == job.MissionIds[0]) // job상태는 첫미션전송시에만 변경
                                        {
                                            job.JobState = JobState.JobExecuting;   // job상태 변경(JobExecuting)
                                        }

                                        uow.Jobs.Update(job);
                                        KGCBuyeo_TabletMissionStatusUpdate(job.PopServerId, job.Id, job.JobStateText);

                                        // ===== robot 상태 갱신
                                        robot.JobId = job.Id;                   // 로봇에 job id 지정
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
                LogExceptionMessage(ex);
            }
        }

        private void KGCBuyeo_PLCTrafficControl()                                                 //<========== [PLC 트레픽 제어(진입 및 진입완료)]
        {

            //1.Robot 진입대기위치(PLC 진입 불가능 상태)
            //2.ACS->PLC DT12002 Robot 진입신호(1Word = 1) / DT12005 Robot Pick 신호(1Word = 1) 변경전송
            //3.PLC 자재없는경우 알람(PLC 진입 불가능 상태)
            //4.PLC->ACS 자재가 있는경우 DT11002(Robot 진입가능(1Word = 1) 변경
            //5.ACS 진입 위치 도착시 DT12002및 DT12005 0으로 초기화

            //1.Robot 진입대기위치(PLC 진입 불가능 상태)
            //2.ACS->PLC DT12002 Robot 진입신호(1Word = 1) / DT12006 Robot Place 신호(1Word = 1) 변경전송
            //3.PLC 자재 있는 경우 알람(PLC 진입 불가능 상태)
            //4.PLC->ACS 자재가 없는 경우 DT11002(Robot 진입가능(1Word = 1) 변경
            //5.ACS 진입 위치 도착시 DT12002및 DT12006 0으로 초기화
            //6.PLC DT12002 또는 DT12006 신호가 0이면 0으로 초기화 하여도 가능할것으로 보임

            try
            {
                var AutoControlPLCList = uow.PlcConfigs.Find(x => x.ControlMode == "AutoControl" && x.PlcModuleUse == "Use").ToList();


                if (AutoControlPLCList.Count > 0)
                {
                    var ConnectAutoControlPLCList = AutoControlPLCList.Where(p => p.Connect == true).ToList();
                    if (ConnectAutoControlPLCList.Count > 0)
                    {
                        //설비 작업 완료 초기화
                        PLCComplet(ConnectAutoControlPLCList);
                        //PLC관련 Robot데이터 Reset
                        PLCRobotDataReset(ConnectAutoControlPLCList);
                        //로봇 Mission 입고 또는 출고 PLC 전송
                        RobotJobModeSandPLC(ConnectAutoControlPLCList);
                        //로봇 진입 신호
                        RobotPLCStandby(ConnectAutoControlPLCList);
                        //로봇 진입완료 위치
                        RobotPLCEntryComplet(ConnectAutoControlPLCList);
                        //WiseModule Reset
                        WiseModuleReset(ConnectAutoControlPLCList);
                    }

                    var DisConnectAutoControlPLCList = AutoControlPLCList.Where(p => p.Connect == false).ToList();
                    if (DisConnectAutoControlPLCList.Count > 0)
                    {
                        foreach (var DisConnectAutoControl in DisConnectAutoControlPLCList)
                        {
                            PLCDataReset(DisConnectAutoControl);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====

            //설비 작업 완료 초기화
            void PLCComplet(List<PlcConfig> AutoControlPLCs)
            {
                //설비 작업 완료 초기화
                foreach (var pLCJobComplet in AutoControlPLCs)
                {
                    //PLC -> ACS [설비 작업 완료 초기화 신호]
                    if (pLCJobComplet.serviceReadData.DT11004 == 1)
                    {
                        //ACS -> PLC [설비 작업 완료 초기화 확인신호]
                        pLCJobComplet.serviceWriteData.DT12004 = 1;
                    }
                    //ACS -> PLC [설비 작업 완료 신호가 아닐경우]
                    else pLCJobComplet.serviceWriteData.DT12004 = 0;
                }
            }

            void PLCRobotDataReset(List<PlcConfig> AutoControlPLCs)
            {
                //PLC 모듈에 로봇이 있는것을 검색한다
                var PlcRobotDataResetList = AutoControlPLCs.Where(x => x.Robot != null).ToList();

                foreach (var PlcRobotDataReset in PlcRobotDataResetList)
                {
                    bool activeRobotPLCReset = false;

                    //로봇이 비정상적일때 리셋을시킨다
                    var NotActiveRobotPLCReset = uow.Robots.GetAll().Where(r => r.RobotName == PlcRobotDataReset.Robot.RobotName
                                                && (r.Fleet_State == FleetState.None           //MiR 전원이 켜져있지않은것
                                                || r.Fleet_State == FleetState.unavailable    //MiR 전원이 켜져있지않은것
                                                || string.IsNullOrWhiteSpace(r.RobotName)    //로봇상태값과 이름이없을경우
                                                || string.IsNullOrWhiteSpace(r.StateText)    //로봇상태값과 이름이없을경우
                                                || r.ACSRobotGroup == "None"                  //로봇이 그룹이 설정되어있지않은것
                                                || r.ACSRobotActive == false)).FirstOrDefault();        //로봇이 active 설정되어있지않은것
                    //jobId가 없을경우 Reset을 한다.
                    var ActiveRobotPLCReset = ActiveRobots().Where(r => r.RobotName == PlcRobotDataReset.Robot.RobotName).FirstOrDefault();

                    if (ActiveRobotPLCReset != null)
                    {
                        if (ActiveRobotPLCReset.JobId == 0) activeRobotPLCReset = true;
                        else
                        {
                            var jobName = uow.Jobs.GetById(ActiveRobotPLCReset.JobId);
                            if (jobName != null && (jobName.CallName.StartsWith(PlcRobotDataReset.PlcModuleName) == false
                                && jobName.CallName.EndsWith(PlcRobotDataReset.PlcModuleName) == false))
                            {
                                activeRobotPLCReset = true;
                            }
                        }

                    }

                    if (NotActiveRobotPLCReset != null || activeRobotPLCReset) PLCDataReset(PlcRobotDataReset);

                    ////로봇중에 현재 실행하고 있는 미션중에 모듈이름이 Call이름과 다른것은 다른 미션이 들어가고 로봇은 초기화되지않은것으로 판단하여 로봇데이터 삭제한다.()
                    //var plcMissionRobotList = uow.Missions.Find(x => (x.RobotName == PlcRobotDataReset.Robot.RobotName || x.JobCreateRobotName == PlcRobotDataReset.Robot.RobotName)
                    //                                    && x.CallName.Contains(PlcRobotDataReset.PlcModuleName) == false).FirstOrDefault();


                }
            }

            //로봇 Mission 입고 또는 출고 PLC 전송
            void RobotJobModeSandPLC(List<PlcConfig> AutoControlPLCs)
            {
                //Pick 또는 Place PLC로 전송

                //통신이 정상적이고 로봇이 없고 진입불가능 PLC를 찾는다
                var PlcStandbyList = AutoControlPLCs.Where(x => x.serviceReadData.DT11002 == 0 && x.Robot == null).ToList();
                //job진행중이고 진입대기위치에 잇는 로봇을찾는다
                var plcStandbyRobotList = ActiveRobots().Where(r => r.JobId > 0
                                           && PlcStandbyList.Count(p => r.PositionZoneName == $"{r.ACSRobotGroup}{p.PlcModuleName}진입대기위치") != 0).ToList();


                foreach (var plcStandbyRobot in plcStandbyRobotList)
                {
                    //진입 대기위치를 지운다
                    string PLCStandbyPosition = plcStandbyRobot.PositionZoneName.Replace("진입대기위치", "");

                    PLCStandbyPosition = PLCStandbyPosition.Replace($"{plcStandbyRobot.ACSRobotGroup}", "");
                    //ModuleName 일치한것을 찾는다
                    var PlcModuleStandby = PlcStandbyList.FirstOrDefault(p => p.PlcModuleName == PLCStandbyPosition);

                    if (PlcModuleStandby != null)
                    {
                        var runjob = uow.Jobs.GetById(plcStandbyRobot.JobId);
                        if (runjob != null)
                        {
                            //Robot Pick경우 Pick신호
                            if (runjob.CallName.StartsWith(PlcModuleStandby.PlcModuleName))
                            {
                                //Robot 준비되어 진입신호
                                PlcModuleStandby.serviceWriteData.DT12002 = 1;
                                PlcModuleStandby.serviceWriteData.DT12005 = 1;
                            }

                            //Robot Place경우 Place 신호 
                            else if (runjob.CallName.EndsWith(PlcModuleStandby.PlcModuleName))
                            {
                                PlcModuleStandby.serviceWriteData.DT12002 = 1;
                                PlcModuleStandby.serviceWriteData.DT12006 = 1;
                            }
                        }
                    }
                }

            }

            //로봇 진입신호
            void RobotPLCStandby(List<PlcConfig> AutoControlPLCs)
            {
                //통신이 정상적이고 로봇이 없고 가능 PLC를 찾는다
                //Robot 레지스터가 1번Send로 변경이 안되는경우가 있어서 Robot==null 조건을 막아둠

                var PlcStandbyList = AutoControlPLCs.Where(x => x.serviceReadData.DT11001 != 1 && x.serviceReadData.DT11002 == 1 /*&& x.Robot == null*/).ToList();

                //job진행중이고 진입대기위치에 잇는 로봇을찾는다 (PLC 설정된 Robot이 없거나 또는 PLC설정된로봇과 진입대기위치에 있는 로봇이 일치할때)
                var plcStandbyRobotList = ActiveRobots().Where(r => r.JobId > 0
                                           && PlcStandbyList.Count(p => r.PositionZoneName == $"{r.ACSRobotGroup}{p.PlcModuleName}진입대기위치"
                                           && (p.Robot == null || p.Robot.RobotName == r.RobotName)) != 0).ToList();


                foreach (var plcStandbyRobot in plcStandbyRobotList)
                {
                    int RegisterNo = 0;
                    string Mode = "PlcStandby";

                    //어떠한 job을 실행중인지 찾는다
                    var JobListFind = uow.Jobs.GetById(plcStandbyRobot.JobId);
                    if (JobListFind != null)
                    {

                        //PLC IN 모드시 와이즈 모듈킨후 상태값1로변경 0일때진입
                        var PLCINMode = PlcStandbyList.FirstOrDefault(x => x.serviceReadData.DT11001 == 0 && JobListFind.CallName.EndsWith(x.PlcModuleName));

                        if (PLCINMode != null)
                        {
                            var plcJobRunRobotList = ActiveRobots().Where(r => r.RobotName != plcStandbyRobot.RobotName
                                            && r.PositionZoneName == $"{r.ACSRobotGroup}{PLCINMode.PlcModuleName}진입위치").FirstOrDefault();
                            //해당모듈 진입 위치에 로봇이 없을때 전송한다
                            if (plcJobRunRobotList == null)
                            {
                                if (PLCINMode.PlcModuleName == KGCBuyeo_PLCEntryNo.차류포장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.차류포장);
                                else if (PLCINMode.PlcModuleName == KGCBuyeo_PLCEntryNo.액상병입.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.액상병입);
                                else if (PLCINMode.PlcModuleName == KGCBuyeo_PLCEntryNo.캔포장실.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.캔포장실);
                                else if (PLCINMode.PlcModuleName == KGCBuyeo_PLCEntryNo.기타작업장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.기타작업장);
                                if (RegisterNo > 0) SendFlagPLCAndRobot(PLCINMode, plcStandbyRobot, RegisterNo, Mode);
                            }
                        }

                        //PLC OUT 모드시 신호 포크 올린후 상태값변경 2일때진입
                        var PLCOutMode = PlcStandbyList.FirstOrDefault(x => x.serviceReadData.DT11001 == 2 && JobListFind.CallName.StartsWith(x.PlcModuleName));

                        if (PLCOutMode != null)
                        {
                            var plcJobRunRobotList = ActiveRobots().Where(r => r.RobotName != plcStandbyRobot.RobotName
                                           && r.PositionZoneName == $"{r.ACSRobotGroup}{PLCOutMode.PlcModuleName}진입위치").FirstOrDefault();
                            //해당모듈 진입 위치에 로봇이 없을때 전송한다
                            if (plcJobRunRobotList == null)
                            {
                                if (PLCOutMode.PlcModuleName == KGCBuyeo_PLCEntryNo.차류포장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.차류포장);
                                else if (PLCOutMode.PlcModuleName == KGCBuyeo_PLCEntryNo.액상병입.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.액상병입);
                                else if (PLCOutMode.PlcModuleName == KGCBuyeo_PLCEntryNo.캔포장실.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.캔포장실);
                                else if (PLCOutMode.PlcModuleName == KGCBuyeo_PLCEntryNo.기타작업장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.기타작업장);
                                if (RegisterNo > 0) SendFlagPLCAndRobot(PLCOutMode, plcStandbyRobot, RegisterNo, Mode);
                            }
                        }

                    }
                }
            }

            //로봇 진입 완료 신호
            void RobotPLCEntryComplet(List<PlcConfig> AutoControlPLCs)
            {
                //통신이 정상적이고 진입 로봇이 있는 PLC를 찾는다
                var plcEntryList = AutoControlPLCs.Where(x => x.Robot != null).ToList();

                //진입 로봇이 있는PLC 중 로봇이름이 일치하고 진입위치에 있는 Robot을 선택한다.
                var plcJobRunRobotList = ActiveRobots().Where(r =>
                                         plcEntryList.Count(p => (r.RobotName == p.Robot.RobotName || r.RobotName == p.Robot.RobotName)
                                                              && r.PositionZoneName == $"{r.ACSRobotGroup}{p.PlcModuleName}진입위치") != 0).ToList();

                foreach (var plcJobRunRobot in plcJobRunRobotList)
                {
                    int RegisterNo = 0;
                    string Mode = "PlcEntry";

                    string PlcEntryPosition = plcJobRunRobot.PositionZoneName.Replace("진입위치", "");
                    PlcEntryPosition = PlcEntryPosition.Replace($"{plcJobRunRobot.ACSRobotGroup}", "");
                    var PlcEntryFind = plcEntryList.FirstOrDefault(p => p.PlcModuleName == PlcEntryPosition);

                    if (PlcEntryFind != null)
                    {
                        if (PlcEntryFind.PlcModuleName == KGCBuyeo_PLCEntryNo.차류포장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.차류포장);
                        else if (PlcEntryFind.PlcModuleName == KGCBuyeo_PLCEntryNo.액상병입.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.액상병입);
                        else if (PlcEntryFind.PlcModuleName == KGCBuyeo_PLCEntryNo.캔포장실.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.캔포장실);
                        else if (PlcEntryFind.PlcModuleName == KGCBuyeo_PLCEntryNo.기타작업장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_PLCEntryNo.기타작업장);
                        if (RegisterNo > 0) SendFlagPLCAndRobot(PlcEntryFind, plcJobRunRobot, RegisterNo, Mode);
                    }
                }
            }

            //WiseModuleReset 
            void WiseModuleReset(List<PlcConfig> AutoControlPLCs)                                                   //<========== [PLC WiseModuleReset 신호 및 Register변경]
            {
                //작업자가 아닌 Robot이 WiseModule Reset신호를 보낸경우

                foreach (var AutoControlPLC in AutoControlPLCs)
                {
                    if (AutoControlPLC.OperatorWiseModuleResetFlag)
                    {
                        //작업자가 WiseModule Reset신호를 Click한경우
                        //Reset신호 전송
                        if (AutoControlPLC.serviceReadData.DT11003 == 0) AutoControlPLC.serviceWriteData.DT12003 = 1;
                        //초기화
                        else
                        {
                            PLCDataReset(AutoControlPLC);
                            AutoControlPLC.serviceWriteData.DT12003 = 0;
                            AutoControlPLC.OperatorWiseModuleResetFlag = false;
                        }
                    }
                    else
                    {
                        //작업자가 아닌 Robot이 Reset신호를 보내는경우
                        var plcrunjobrobot = ActiveRobots().Where(a => a.JobId > 0 && a.PositionZoneName.EndsWith("진입위치") &&
                                              uow.Jobs.GetAll().Count(j => j.Id == a.JobId) != 0).ToList();

                        foreach (var robot in plcrunjobrobot)
                        {
                            int RegisterNo = 0;
                            string Mode = "WiseModuleReset";

                            var PLCEntryPosition = robot.PositionZoneName.Replace("진입위치", "");
                            PLCEntryPosition = PLCEntryPosition.Replace($"{robot.ACSRobotGroup}", "");

                            if (AutoControlPLC.PlcModuleName == PLCEntryPosition)
                            {
                                if (AutoControlPLC.PlcModuleName == KGCBuyeo_WiseResetNo.차류포장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_WiseResetNo.차류포장);
                                else if (AutoControlPLC.PlcModuleName == KGCBuyeo_WiseResetNo.액상병입.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_WiseResetNo.액상병입);
                                else if (AutoControlPLC.PlcModuleName == KGCBuyeo_WiseResetNo.캔포장실.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_WiseResetNo.캔포장실);
                                else if (AutoControlPLC.PlcModuleName == KGCBuyeo_WiseResetNo.기타작업장.ToString()) RegisterNo = Convert.ToInt32(KGCBuyeo_WiseResetNo.기타작업장);
                                if (RegisterNo > 0) SendFlagPLCAndRobot(AutoControlPLC, robot, RegisterNo, Mode);
                            }
                        }
                    }
                }


            }

            //레지스터 및 PLC 신호 전송 (진입 신호 및 진입 완료)
            void SendFlagPLCAndRobot(PlcConfig plcConfig, Robot robot, int RegisterNo, string Mode)
            {
                if (!MiR_Get_Register(robot, RegisterNo)) return;

                if (RegisterNo == 30) //차류포장
                {
                    if (Mode == "PlcStandby")
                    {
                        if (robot.Registers.dMiR_Register_Value30 == 0)
                        {
                            MiR_Put_Register(robot, RegisterNo, 1);
                            plcConfig.Robot = robot;
                        }

                    }
                    else if (Mode == "PlcEntry")
                    {
                        if (robot.Registers.dMiR_Register_Value30 == 1) MiR_Put_Register(robot, RegisterNo, 0);
                        else PLCDataReset(plcConfig);
                    }
                }
                else if (RegisterNo == 31)  //액상병입
                {
                    if (Mode == "PlcStandby")
                    {
                        if (robot.Registers.dMiR_Register_Value31 == 0)
                        {
                            MiR_Put_Register(robot, RegisterNo, 1);
                            plcConfig.Robot = robot;
                        }

                    }
                    else if (Mode == "PlcEntry")
                    {
                        if (robot.Registers.dMiR_Register_Value31 == 1) MiR_Put_Register(robot, RegisterNo, 0);
                        else PLCDataReset(plcConfig);
                    }
                }
                else if (RegisterNo == 32) //캔포장실
                {
                    if (Mode == "PlcStandby")
                    {
                        if (robot.Registers.dMiR_Register_Value32 == 0)
                        {
                            MiR_Put_Register(robot, RegisterNo, 1);
                            plcConfig.Robot = robot;
                        }

                    }
                    else if (Mode == "PlcEntry")
                    {
                        if (robot.Registers.dMiR_Register_Value32 == 1) MiR_Put_Register(robot, RegisterNo, 0);
                        else PLCDataReset(plcConfig);
                    }
                }
                else if (RegisterNo == 33)  //기타작업장
                {
                    if (Mode == "PlcStandby")
                    {
                        if (robot.Registers.dMiR_Register_Value33 == 0)
                        {
                            MiR_Put_Register(robot, RegisterNo, 1);
                            plcConfig.Robot = robot;
                        }


                    }
                    else if (Mode == "PlcEntry")
                    {
                        if (robot.Registers.dMiR_Register_Value33 == 1) MiR_Put_Register(robot, RegisterNo, 0);
                        else PLCDataReset(plcConfig);
                    }
                }
                //레지스터 및 PLC 신호 전송 (WiseModule Reset)
                else if (RegisterNo == 34)
                {
                    if (Mode == "WiseModuleReset")
                    {
                        //MiR->ACS Reset신호 받았을경우
                        if (robot.Registers.dMiR_Register_Value34 == 1)
                        {
                            //MiR->ACS->PLC Reset신호 전송
                            if (plcConfig.serviceWriteData.DT12003 == 0) plcConfig.serviceWriteData.DT12003 = 1;
                            //PLC->ACS->MiR Reset완료 신호시 Register 0으로 변경
                            else if (plcConfig.serviceReadData.DT11003 == 1) MiR_Put_Register(robot, RegisterNo, 0);

                        }
                        //MiR->ACS->PLC Register = 0 으로 변경되었으면 Reset신호 초기화
                        else if (plcConfig.serviceReadData.DT11003 == 1 && robot.Registers.dMiR_Register_Value34 == 0) plcConfig.serviceWriteData.DT12003 = 0;
                    }
                }
                else if (RegisterNo == 35)
                {
                    if (Mode == "WiseModuleReset")
                    {
                        //MiR->ACS Reset신호 받았을경우
                        if (robot.Registers.dMiR_Register_Value35 == 1)
                        {
                            //MiR->ACS->PLC Reset신호 전송
                            if (plcConfig.serviceWriteData.DT12003 == 0) plcConfig.serviceWriteData.DT12003 = 1;
                            //PLC->ACS->MiR Reset완료 신호시 Register 0으로 변경
                            else if (plcConfig.serviceReadData.DT11003 == 1) MiR_Put_Register(robot, RegisterNo, 0);

                        }
                        //MiR->ACS->PLC Register = 0 으로 변경되었으면 Reset신호 초기화
                        else if (plcConfig.serviceReadData.DT11003 == 1 && robot.Registers.dMiR_Register_Value35 == 0) plcConfig.serviceWriteData.DT12003 = 0;
                    }
                }
                else if (RegisterNo == 36)
                {
                    if (Mode == "WiseModuleReset")
                    {
                        //MiR->ACS Reset신호 받았을경우
                        if (robot.Registers.dMiR_Register_Value36 == 1)
                        {
                            //MiR->ACS->PLC Reset신호 전송
                            if (plcConfig.serviceWriteData.DT12003 == 0) plcConfig.serviceWriteData.DT12003 = 1;
                            //PLC->ACS->MiR Reset완료 신호시 Register 0으로 변경
                            else if (plcConfig.serviceReadData.DT11003 == 1) MiR_Put_Register(robot, RegisterNo, 0);

                        }
                        //MiR->ACS->PLC Register = 0 으로 변경되었으면 Reset신호 초기화
                        else if (plcConfig.serviceReadData.DT11003 == 1 && robot.Registers.dMiR_Register_Value36 == 0) plcConfig.serviceWriteData.DT12003 = 0;
                    }
                }
                else if (RegisterNo == 37)
                {
                    if (Mode == "WiseModuleReset")
                    {
                        //MiR->ACS Reset신호 받았을경우
                        if (robot.Registers.dMiR_Register_Value37 == 1)
                        {
                            //MiR->ACS->PLC Reset신호 전송
                            if (plcConfig.serviceWriteData.DT12003 == 0) plcConfig.serviceWriteData.DT12003 = 1;
                            //PLC->ACS->MiR Reset완료 신호시 Register 0으로 변경
                            else if (plcConfig.serviceReadData.DT11003 == 1) MiR_Put_Register(robot, RegisterNo, 0);

                        }
                        //MiR->ACS->PLC Register = 0 으로 변경되었으면 Reset신호 초기화
                        else if (plcConfig.serviceReadData.DT11003 == 1 && robot.Registers.dMiR_Register_Value37 == 0) plcConfig.serviceWriteData.DT12003 = 0;
                    }
                }

            }
        }

        private void KGCBuyeo_JobReset()                                                          //<========== [Reset 제어]
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigData.jobResetRobotNo) && !string.IsNullOrEmpty(ConfigData.RobotResetMissionName)
                 && !string.IsNullOrEmpty(ConfigData.RobotResetMissionName))
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
                                var Tabletrunjob = uow.TabletMissionStatus.GetByjobId(job.Id);
                                if (Tabletrunjob != null)
                                {
                                    uow.TabletMissionStatus.Remove(Tabletrunjob);
                                    Console.WriteLine($"JobReset[OK], TabletId : {Tabletrunjob.SEQ}, JobId : {Tabletrunjob.JobId}, Name : {Tabletrunjob.CALLNAME},  CallFlag : {Tabletrunjob.CALLFLAG}, CancelFlag : {Tabletrunjob.CANCELFLAG}");
                                    TabletLogger.Info($"JobReset[OK], TabletId : {Tabletrunjob.SEQ}, JobId : {Tabletrunjob.JobId}, Name : {Tabletrunjob.CALLNAME},  CallFlag : {Tabletrunjob.CALLFLAG}, CancelFlag : {Tabletrunjob.CANCELFLAG}");
                                }

                                var pLCDataReset = uow.PlcConfigs.Find(x => x.Robot != null && x.Robot.RobotName == robot.RobotName).FirstOrDefault();
                                if (pLCDataReset != null) PLCDataReset(pLCDataReset);

                                Job_DeleteJobData(job, 3);
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
                LogExceptionMessage(ex);
            }
        }

        private void PLCDataReset(PlcConfig plcData, Robot robot = null)
        {
            plcData.Robot = null;
            plcData.serviceWriteData.DT12002 = 0;
            plcData.serviceWriteData.DT12005 = 0;
            plcData.serviceWriteData.DT12006 = 0;
        }


        private void KGCBuyeo_PLCTrafficReigsterReset()                                           //<========== [PLC 트레픽 관련 레지스터 초기화]
        {
            foreach (var robot in ActiveRobots().Where(x => x.PositionZoneName.EndsWith("진입대기위치") == false && x.PositionZoneName.EndsWith("진입위치") == false).ToList())
            {

                MiR_Put_Register(robot, 30, 0);
                MiR_Put_Register(robot, 31, 0);
                MiR_Put_Register(robot, 32, 0);
                MiR_Put_Register(robot, 33, 0);
                MiR_Put_Register(robot, 34, 0);
                MiR_Put_Register(robot, 35, 0);
                MiR_Put_Register(robot, 36, 0);
                MiR_Put_Register(robot, 37, 0);
            }
        }

    }
}

