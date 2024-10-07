using INA_ACS_Server.Models.AmkorK5_M3F_T3F;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {
        private void Upgrade_AmkorK5_JobStartControl_Call()                           //<==========미션 상태 Start 변경
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

        private void Upgrade_PositionAreaUpdate()                                     //<========== [Robot위치 및 자재정보]
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
                        Upgrade_ProductAndDoorState(robot);
                    }
                    else
                    {
                        Upgrade_ProductAndDoorState(robot, true);
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

            void Upgrade_ProductAndDoorState(Robot robot, bool Active = false)    //<========== [자재 내용Updata 및 Door 상태 Update]  
            {
                string Product = "";
                string Door = "";

                //DoorTime 설정된 그룹만 적용한다
                if (Active && robot.ACSRobotGroup == ConfigData.DoorOpenRobotStatusChangeGroup)
                {

                    MiR_Get_Register(robot, 22);    //자재 유/무
                    MiR_Get_Register(robot, 23);    //Door


                    if (robot.Registers.dMiR_Register_Value22 > 0)
                    {
                        Product = "자재있음";
                        TowerLampRegModeProductInset(robot);

                        //자재가 있을경우 자재 목록 초기화
                        MiR_Get_Register(robot, 25);
                        if (robot.Registers.dMiR_Register_Value25 > 0)
                        {
                            TowerLampProductDelete(robot);
                        }
                    }
                    else
                    {
                        Product = "";
                        TowerLampProductDelete(robot);
                    }

                    if (robot.Registers.dMiR_Register_Value23 > 0)
                    {
                        Door = "Door열림";
                    }
                    else Door = "Door닫힘";

                }
                else
                {
                    //초기화
                    Door = "";
                    Product = "";
                }

                //지금 현재 값과 다르면 DB Update
                if (robot.Product != Product || robot.Door != Door)
                {
                    robot.Door = Door;
                    robot.Product = Product;
                    uow.Robots.Update(robot);

                }

                // ===== 로컬 함수 =====
                void TowerLampRegModeProductInset(Robot robot1)
                {
                    //Registar 모드 일때 동작한다
                    var TowerLampMode = uow.ACSModeInfo.Find(a => a.Location == "WiseTowerLamp" && a.ACSMode == "RegistarMode").FirstOrDefault();
                    if (TowerLampMode != null)
                    {

                        //22번 레지스터 번호와 일치하고 32번 33번 34번 35번 셋팅 되어있는 List찾는다
                        var ProductInfo = uow.ProductNameInfos.Find(p => p.Regiser22Vlaue == Convert.ToInt32(robot1.Registers.dMiR_Register_Value22)
                                                                    && (p.RegisterNo == "Register26" || p.RegisterNo == "Register27" || p.RegisterNo == "Register28" || p.RegisterNo == "Register29"));
                        if (ProductInfo.Count > 0)
                        {
                            MiR_Get_Register(robot1, 26);
                            MiR_Get_Register(robot1, 27);
                            MiR_Get_Register(robot1, 28);
                            MiR_Get_Register(robot1, 29);

                            foreach (var ProductUpdata in ProductInfo)
                            {
                                if (ProductUpdata.RegisterNo == "Register26")
                                {
                                    //로봇데이터와 자재명이 일치하는 데이터가 없으면 추가한다
                                    if (ProductUpdata.RegisterValue == Convert.ToInt32(robot.Registers.dMiR_Register_Value26)) ProductInset(robot1, ProductUpdata);
                                    else ProductDelete(robot1, ProductUpdata);
                                }

                                if (ProductUpdata.RegisterNo == "Register27")
                                {
                                    //로봇데이터와 자재명이 일치하는 데이터가 없으면 추가한다
                                    if (ProductUpdata.RegisterValue == Convert.ToInt32(robot.Registers.dMiR_Register_Value27)) ProductInset(robot1, ProductUpdata);
                                    else ProductDelete(robot1, ProductUpdata);
                                }
                                if (ProductUpdata.RegisterNo == "Register28")
                                {
                                    if (ProductUpdata.RegisterValue == Convert.ToInt32(robot.Registers.dMiR_Register_Value28)) ProductInset(robot1, ProductUpdata);
                                    else ProductDelete(robot1, ProductUpdata);
                                }
                                if (ProductUpdata.RegisterNo == "Register29")
                                {
                                    //로봇데이터와 자재명이 일치하는 데이터가 없으면 추가한다
                                    if ( ProductUpdata.RegisterValue == Convert.ToInt32(robot.Registers.dMiR_Register_Value29)) ProductInset(robot1, ProductUpdata);
                                    else ProductDelete(robot1, ProductUpdata);
                                }
                            }
                        }
                    }
                }
                // ===== 로컬 함수 =====
                void ProductInset(Robot insetrobot, ProductNameInfoModel insetPorduct)
                {
                    var productInsetdata = uow.Products.Find(p => p.RobotName == insetrobot.RobotName && p.ProductName == insetPorduct.ProductName).FirstOrDefault();
                    if (productInsetdata == null)
                    {
                        var ProductData = new ProductModel
                        {
                            CreateTime = DateTime.Now,
                            RobotName = insetrobot.RobotName,
                            ProductName = insetPorduct.ProductName
                        };
                        uow.Products.Add(ProductData);

                    }
                }

                //Robot Reset 경우 자재 데이터가 계속 살아있어서 자재 레지스터가 0인경우 자재 데이터 삭제 
                void ProductDelete(Robot ProductDeleterobot, ProductNameInfoModel DeletePorduct)
                {
                    var productDeletedata = uow.Products.Find(p => p.RobotName == ProductDeleterobot.RobotName && p.ProductName == DeletePorduct.ProductName).FirstOrDefault();
                    if (productDeletedata != null) uow.Products.Remove(productDeletedata);

                }
                void TowerLampProductDelete(Robot robot1)
                {
                    //로봇 이름과 같은 데이터는 전부 삭제한다(레지스터 모드 및 바코드모드 동일로 Test일단진행 추후 변경가능)
                    var productDeleteDatas = uow.Products.Find(p => p.RobotName == robot1.RobotName).ToList();
                    foreach (var productDelete in productDeleteDatas)
                    {
                        uow.Products.Remove(productDelete);
                    }

                    var TowerLampMode = uow.ACSModeInfo.Find(a => a.Location == "WiseTowerLamp" && a.ACSMode == "RegistarMode").FirstOrDefault();
                    //Registar 모드 일때 동작한다
                    if (TowerLampMode != null)
                    {
                        MiR_Put_Register(robot1, 25, 0);
                        MiR_Put_Register(robot1, 26, 0);
                        MiR_Put_Register(robot1, 27, 0);
                        MiR_Put_Register(robot1, 28, 0);
                        MiR_Put_Register(robot1, 29, 0);

                    }

                }
            }

        }

        private void Upgrade_M3FT3F_JobStartControl_Call()                            //<========== [미션 상태 Start 변경]  (AMT 같이사용 여부파악하여 삭제예정)
        {
            //=============================================== 미션 상태 Start =================================================//
            //1.Init으로 등록되어 있는 job 을 Start로 상태를 변경한다.
            //=================================================================================================================//

            var jobs = uow.Jobs.Find(x => (x.ACSJobGroup != "None") && (x.ACSJobGroup != "AMTWEST") && (x.ACSJobGroup != "AMTEAST") && x.JobState == JobState.JobInit);

            foreach (var job in jobs)
            {
                // job 시작 ~~~~~~~~~~~~
                job.JobState = JobState.JobStart;
                uow.Jobs.Update(job);
            }
        }

        private void Upgrade_M3FT3FJobControl()                                       //<========== [Mission추가]
        {
            //=============================================== 미션 추가 방법 =================================================//
            //1.엘리베이터가 아닌 위치에 MiR 이있을경우 포지션이름을 RobotDataBase에 UpDate한다.
            //2.자재 경우 같은 포지션내에 있는 자재를 업데이트한다
            //3.출발지 설정한 포지션에서 Maint Tablet(MiR 인터페이스창에 Registar 변경 Button을 설정)하여 작업자가 RegiStar 변경시 설정 미션을 추가한다
            //4.Call레지스터경우 20 Value 값으로 지정한다
            //5.현재 포지션에서 다른 로봇과 같은 목적지의 Call은 보내지않고 다른 목적지 확인후 다른목적지가 있으면 미션을 전송한다
            //=================================================================================================================//

            try
            {
                //AMTWEST AMTEST 와 다른것
                foreach (var robot in ActiveRobots().Where(r => r.ACSRobotGroup != "AMTWEST" && r.ACSRobotGroup != "AMTEAST").ToList())
                {

                    //cancel job이 있는 경우 이 로봇은 스킵
                    if (IsCancelJobExist(robot))
                        continue;

                    //========================= job 관련

                    // check job(jobCancel확인)
                    if (IsJobExist(robot)) Upgrade_M3FT3F_jobCancelControl(robot);

                    else
                    {
                        //job ADD
                        var JobAdd = Upgrade_M3FT3F_JobSelectAddContorl(robot);
                        if (JobAdd != null) AddJobToQueue(robot, JobAdd);
                    }
                    //===============================================================

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====
            //job ADD
            bool AddJobToQueue(Robot robot, JobConfigModel jobConfig)
            {
                if (uow.Jobs.GetByCallName(jobConfig.CallName) == null && uow.Jobs.GetByCreateRobot(robot.RobotName) == null) // 중복 추가 방지
                {
                    JobCommandQueueAdd(jobConfig, robot);
                }
                return true;
            }



            // 이 로봇에 cancel job이 있나?
            bool IsCancelJobExist(Robot robot)
            {
                // job 리스트에서 특정 그룹의 cancel job 을 찾는다
                var foundJobs = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup && j.CallName != null
                                                 && (j.CallName.Contains("Cancel") || j.CallName.Contains("cancel")));
                return foundJobs.Count > 0;
            }

            // 이 로봇에 할당된 job이 있나?
            bool IsJobExist(Robot robot)
            {
                var foundJobs = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup
                                                && j.JobCreateRobotName == robot.RobotName);
                return foundJobs.Count > 0;
            }

            JobConfigModel Upgrade_M3FT3F_JobSelectAddContorl(Robot robot)   //<========== 미션선택 
            {
                JobConfigModel JobConfigSelect = null;
                //try
                {

                    //======================================================================
                    //AutoJob 경우 Config에서 레지스터 20번에 0으로 하면 AutoJob으로 설정됨.
                    //robot 자재가 있을경우 true (!string.IsNullOrEmpty(robot.Product))조건

                    //======================================================================
                    var runMissions = uow.Missions.Find(m => (m.Robot == robot || m.JobCreateRobotName == robot.RobotName) && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
                                            .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid").ToList(); // 완료 미션은 제외한다

                    var runMission_Job = runMissions.Where(m => m.JobId != 0).SingleOrDefault(); // 일반미션

                    //충전 Config 에서 일치하는 값이 있는지 확인 runMission_Specials 과 일치하는 값이 있는지 확인
                    var runCharging_Specials = runMissions.Where(r => r.JobId == 0 && uow.ChargeMissionConfig.GetAll()
                                                          .Count(c => r.MissionName == c.ChargeMissionName && robot.RobotName == c.RobotName) != 0).ToList();
                    //대기 Config 에서 일치하는 값이 있는지 확인
                    var runWaiting_Specials = runMissions.Where(r => r.JobId == 0 && uow.WaitMissionConfig.GetAll()
                                                         .Count(c => r.MissionName == c.WaitMissionName && robot.RobotName == c.RobotName) != 0).ToList();

                    //충전 및 대기 Config 일치하는 값을 확인한 List를 합침
                    var runMission_Specials = runCharging_Specials.Concat(runWaiting_Specials).ToList();

                    // (조건1) 이 로봇이 실행중인 미션이 없을때
                    bool c1 = robot.ACSRobotGroup != "None"
                           && robot.StateID == RobotState.Ready
                           && runMission_Job == null;

                    // (조건2) 이 로봇이 특수미션을 실행중일때 (자동충전미션 / 자동대기위치미션)
                    bool c2 = robot.StateID == RobotState.Executing
                         && runMission_Specials.Count() > 0;

                    //bool c1 = robot.JobId == 0
                    //        && !string.IsNullOrWhiteSpace(robot.PositionZoneName);
                    if (c1 || c2)
                    {
                        MiR_Get_Register(robot, 20);
                        MiR_Get_Register(robot, 32);
                        MiR_Get_Register(robot, 33);

                        var JobConfigs = JobConfigExcept();
                        if (JobConfigs.Count > 0)
                        {
                            //엘리베이터 모드 조회 한다.
                            var elevatorMode = uow.ACSModeInfo.Find(m => m.Location == "Elevator").FirstOrDefault();

                            if (elevatorMode != null)
                            {
                                //자재 활성화가 사용 / 자재를 사용 / 현재 Robot 자재 있음 / 엘리베이터가 사용 / 엘리베이터 모드와 현재 엘리베이터 모드가 일치한것.
                                JobConfigSelect = JobConfigs.Where(m => m.ProductActive == true && m.ProductValue == !string.IsNullOrEmpty(robot.Product)
                                                                     && m.ElevatorModeActive == true && elevatorMode.ElevatorMode == m.ElevatorModeValue).FirstOrDefault();
                                if (JobConfigSelect == null)
                                {
                                    //자재 활성화가 사용 / 자재를 사용안함 / 현재 Robot 자재 없음 / 엘리베이터가 사용 / 엘리베이터 모드와 현재 엘리베이터 모드가 일치한것.
                                    JobConfigSelect = JobConfigs.Where(m => m.ProductActive == true && m.ProductValue == !string.IsNullOrEmpty(robot.Product)
                                                                         && m.ElevatorModeActive == true && elevatorMode.ElevatorMode == m.ElevatorModeValue).FirstOrDefault();
                                    if (JobConfigSelect == null)
                                    {
                                        //자재 활성화가 미사용 / 엘리베이터가 사용 / 엘리베이터 모드와 현재 엘리베이터 모드가 일치한것.
                                        JobConfigSelect = JobConfigs.Where(m => m.ProductActive == false
                                                                             && m.ElevatorModeActive == true && elevatorMode.ElevatorMode == m.ElevatorModeValue).FirstOrDefault();
                                        if (JobConfigSelect == null)
                                        {
                                            //자재 활성화가 사용 / 자재를 사용 / 현재 Robot 자재 있음 / 엘리베이터가 미사용
                                            JobConfigSelect = JobConfigs.Where(m => m.ProductActive == true && m.ProductValue == !string.IsNullOrEmpty(robot.Product)
                                                                                 && m.ElevatorModeActive == false).FirstOrDefault();
                                            if (JobConfigSelect == null)
                                            {
                                                //자재 활성화가 사용 / 자재를 사용안함 / 현재 Robot 자재 없음 / 엘리베이터가 미사용
                                                JobConfigSelect = JobConfigs.Where(m => m.ProductActive == true && m.ProductValue == !string.IsNullOrEmpty(robot.Product)
                                                                                && m.ElevatorModeActive == false).FirstOrDefault();
                                                if (JobConfigSelect == null)
                                                {
                                                    //자재 미사용 / 엘리베이터 미사용
                                                    JobConfigSelect = JobConfigs.Where(m => m.ProductActive == false && m.ElevatorModeActive == false).FirstOrDefault();

                                                }
                                            }
                                        }

                                    }

                                }
                            }
                            else
                            {
                                //자재 활성화가 사용 / 자재를 사용 / 현재 Robot 자재 있음 / 엘리베이터가 미사용
                                JobConfigSelect = JobConfigs.Where(m => m.ProductActive == true && m.ProductValue == !string.IsNullOrEmpty(robot.Product)
                                                                     && m.ElevatorModeActive == false).FirstOrDefault();

                                if (JobConfigSelect == null)
                                {
                                    //자재 활성화가 사용 / 자재를 사용안함 / 현재 Robot 자재 없음 / 엘리베이터가 미사용
                                    JobConfigSelect = JobConfigs.Where(m => m.ProductActive == true && m.ProductValue == !string.IsNullOrEmpty(robot.Product)
                                                                         && m.ElevatorModeActive == false).FirstOrDefault();

                                }
                            }
                        }
                    }
                    return JobConfigSelect;
                }
                //catch (Exception ex)
                {
                    //LogExceptionMessage(ex);
                    return JobConfigSelect;
                }

                // ===== 로컬 함수 =====
                List<JobConfigModel> JobConfigExcept()
                {

                    //JobConfig 에서 그룹 일치 현재위치 일치 레지스터 일치한 jobConfig List 를 조회한다
                    var JobConfigs = uow.JobConfigs.Find(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && !string.IsNullOrEmpty(robot.PositionZoneName) && m.CallName.StartsWith(robot.PositionZoneName)
                                                    && m.jobCallSignal == Convert.ToString(robot.Registers.dMiR_Register_Value20)
                                                    && m.POSjobCallSignal_Reg32 == Convert.ToString(robot.Registers.dMiR_Register_Value32) && m.POSjobCallSignal_Reg33 == Convert.ToString(robot.Registers.dMiR_Register_Value33)
                                                    && m.ExecuteBattery <= robot.BatteryPercent);

                    if (JobConfigs.Count > 0)
                    {

                        //현재 수행중인 미션
                        var runmission = uow.Missions.Find(m => m.ACSMissionGroup == robot.ACSRobotGroup && m.MissionState != "Done").ToList();

                        //현재 수행중인미션"_"배열로 나누어서 마지막 값만 가지고온다(목적지)
                        var runmissionEndPosition = runmission.Select(s => s.CallName.Split('_').LastOrDefault()).ToList();

                        foreach (var positionArea in runmissionEndPosition)
                        {
                            //목적지와 같은 목적지가 있는지 확인
                            var jobConfigsToDelete = JobConfigs.Where(j => j.CallName.EndsWith(positionArea)).ToList();
                            //같은 목적지에 대한 List 와 다른 Config List를 만든다
                            JobConfigs = JobConfigs.Except(jobConfigsToDelete).ToList();
                        }

                        var DifferentRobots = uow.Robots.GetAll().Where(r => r.ACSRobotGroup == robot.ACSRobotGroup && r.RobotName != robot.RobotName
                                                                        && r.Fleet_State != FleetState.None && r.Fleet_State != FleetState.unavailable
                                                                        && !string.IsNullOrWhiteSpace(r.RobotName) && !string.IsNullOrWhiteSpace(r.StateText)).ToList();

                        //현재 robot 있는위치로 미션을 전송하지 않는다
                        foreach (var DifferentRobot in DifferentRobots)
                        {
                            //다른 robot 설정된 Position 위치 있고 미션이 수행중이 아닌로봇확인한다.
                            if (!string.IsNullOrEmpty(DifferentRobot.PositionZoneName))
                            {
                                //다른 robot 위치로 jobConfig에서 CallName 목적지로 설정되어있는 목록을 확인한다.
                                var jobConfigsToDelete = JobConfigs.Where(j => j.CallName.EndsWith(DifferentRobot.PositionZoneName)).ToList();
                                JobConfigs = JobConfigs.Except(jobConfigsToDelete).ToList();
                            }
                        }

                    }
                    return JobConfigs;

                }
            }

        }

        private void Upgrade_AmkorK5_AutoWaiting_Mission(Robot WaitRobot = null)      //<========== [자동대기]
        {
            //=============================================== 자동 대기 미션 =================================================//
            //1. Robot이 기본 조건 이고 레디 상태
            //2. Robot이 PositionAreaConfigs 설정된 포지션이 아닌 다른 포지션에 있을경우
            //3. 다른 Robot 과 목적지가 같을경우 다른 목적지의 미션을 전송한다.
            //4. WaitingConfig 에서 목적지 별로 Robot을 setting해야함.
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
                    && robotFloorName != null
                    && string.IsNullOrEmpty(robot.PositionZoneName);

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
                            //자재 활성화가 사용 / 자재를 미사용 / 현재 Robot 자재 없음
                            waitMissionSelect = waitingConfigs.Where(w => w.ProductActive == true && w.ProductValue == !string.IsNullOrEmpty(robot.Product)).FirstOrDefault();

                            if (waitMissionSelect == null)
                            {
                                //자재 활성화가 미사용 / 자재를 미사용 / 현재 Robot 자재 없음
                                waitMissionSelect = waitingConfigs.Where(w => w.ProductActive == false).FirstOrDefault();
                            }
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

                //현재 수행중인 미션
                var runmission = uow.Missions.Find(m => m.ACSMissionGroup == robot.ACSRobotGroup && m.MissionState != "Done");

                //현재 수행중인미션"_"배열로 나누어서 마지막 값만 가지고온다(목적지)
                var runmissionEndPosition = runmission.Select(s => s.CallName.Split('_').LastOrDefault()).ToList();

                foreach (var positionArea in runmissionEndPosition)
                {
                    //목적지와 같은 목적지가 있는지 확인
                    var waitingConfigsToDelete = waitingmissionConfigs.Where(j => j.PositionZone.EndsWith(positionArea)).ToList();
                    //같은 목적지에 대한 List 와 다른 Config List를 만든다
                    waitingmissionConfigs = waitingmissionConfigs.Except(waitingConfigsToDelete).ToList();
                }

                if (waitingmissionConfigs.Count > 0)
                {
                    var DifferentRobots = uow.Robots.GetAll().Where(r => r.ACSRobotGroup == robot.ACSRobotGroup && r.RobotName != robot.RobotName
                                                                     && r.Fleet_State != FleetState.None && r.Fleet_State != FleetState.unavailable
                                                                     && !string.IsNullOrWhiteSpace(r.RobotName) && !string.IsNullOrWhiteSpace(r.StateText)).ToList();

                    //로봇 그룹이 일치하고 지금 현재 선택되어있는 로봇과 다른 로봇을 조회한다
                    foreach (var DifferentRobot in DifferentRobots)
                    {
                        //다른 robot 설정된 Position 위치 있고 미션이 수행중이 아닌로봇확인한다.
                        if (!string.IsNullOrEmpty(DifferentRobot.PositionZoneName))
                        {
                            //다른 robot 위치로 jobConfig에서 CallName 목적지로 설정되어있는 목록을 확인한다.
                            var jobConfigsToDelete = waitingmissionConfigs.Where(j => j.PositionZone.EndsWith(DifferentRobot.PositionZoneName)).ToList();
                            waitingmissionConfigs = waitingmissionConfigs.Except(jobConfigsToDelete).ToList();
                        }
                    }

                }
                return waitingmissionConfigs;
            }
        }

        private void Upgrade_AmkorK5_AutoCharging_Mission()                           //<========== [자동충전] 
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

                    //// 해당 Robot에 이미 전송한 미션을 검색한다
                    //var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0 && m.MissionState != "Done").FirstOrDefault();

                    ////충전 미션이 실행중인지 확인한다.
                    //var runchargingmission = uow.ChargeMissionConfig.Find(c => c.ChargeMissionUse == "Use" && c.RobotName == robot.RobotName).FirstOrDefault();

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
                        ////robot 위치를 확인한다
                        //var robotPositionArea = uow.PositionAreaConfigs.UpGrade_AllPositionArea(robot).FirstOrDefault();

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
                                //자재 활성화가 사용 / 자재를 미사용 / 현재 Robot 자재 없음
                                ChargingMissionSelect = chargeMissionConfig.Where(w => w.ProductActive == true && w.ProductValue == !string.IsNullOrEmpty(robot.Product)).FirstOrDefault();

                                if (ChargingMissionSelect == null)
                                {
                                    //자재 활성화가 미사용 / 자재를 미사용 / 현재 Robot 자재 없음
                                    ChargingMissionSelect = chargeMissionConfig.Where(w => w.ProductActive == false).FirstOrDefault();
                                }
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
                //충전기 카운터가 설정되지 않았을때
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

        private void Upgrade_Elevator_Traffic_Control()                               //<========== [Elevator 제어]
        {
            //Robot 이 미션 진행중이고 
            //Robot 에서 수행하고 있는 미션이름이 엘리베이터 관련된 이름이고
            //Robot 엘리베이터 위치에 있는
            //Elevator Registar 30 Value 6 = M3F층
            //Elevator Registar 30 Value 3 = T3F층

            //Elevator Registar 31 Value 1 = Start Position -> Elevator Position [ACS 변경(이동요청)] 
            //Elevator Registar 31 Value 2 = Elevator Position [MiR 변경(도착)]
            //Elevator Registar 31 Value 3 = Elevator Position -> MiR 상태변경  [ACS 변경(상태변경 요청)]
            //Elevator Registar 31 Value 4 = Elevator Position -> End Position  [ACS변경(이동요청)]
            //Elevator Registar 31 Value 5 = Elevator Position -> End Position  [ACS변경(배출완료)]
            //Elevator Registar 31 Value 0 = End Position  [MiR 자채 변경]
            try
            {
                ////AGV모드 일때실행한다
                //var ElevatorAGVMode = uow.ACSModeInfo.Find(a => a.Location == "Elevator" && a.ElevatorMode == "AGVMode").FirstOrDefault();
                //if (ElevatorAGVMode != null)
                //{
                foreach (var robot in ActiveRobots().Where(r => r.ACSRobotGroup != "AMTWEST" && r.ACSRobotGroup != "AMTEAST").ToList())
                {

                    //Mission 이름이 Elevator_Up일때(M3F->T3F)
                    var Elevator_runmission = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState == "Executing" && m.RobotName == robot.RobotName
                                                 && (m.MissionName.Contains("Elevator_Up") || m.MissionName.Contains("Elevator_Down"))).FirstOrDefault();

                    if (Elevator_runmission != null)
                    {
                        //var ElevatorPositionArea = uow.PositionAreaConfigs.UpGrade_ElevatorPositionArea(robot).FirstOrDefault();

                        if (Elevator_runmission.MissionName.Contains("Elevator_Up")) // 미션 이름이 Elevator_Up 일경우
                        {
                            var mirStatusElevator = uow.ElevatorState.Find(m => m.ElevatorMissionName == Elevator_runmission.MissionName && m.RobotName == robot.RobotName).FirstOrDefault();

                            if (mirStatusElevator == null)  // 등록된 상태가 없을때
                            {
                                //if (ElevatorPositionArea != null && ElevatorPositionArea.PositionAreaName == ConfigData.T3F_ElevatorStartPOS)
                                if (robot.PositionZoneName == ConfigData.T3F_ElevatorStartPOS)
                                {
                                    var newElevatorStateModelsAdd = new ElevatorStateModule
                                    {
                                        RobotName = robot.RobotName,
                                        ElevatorState = "CallStartFloor",
                                        MiRStateElevator = "",
                                        ElevatorMissionName = Elevator_runmission.MissionName,
                                    };
                                    uow.ElevatorState.Add(newElevatorStateModelsAdd);
                                }
                            }

                            else
                            {
                                switch (mirStatusElevator.MiRStateElevator)
                                {
                                    case "MiRStateElevatorLoaderStart":

                                        if (!MiR_Get_Register(robot, 31)) break;

                                        //if (ElevatorPositionArea != null && ElevatorPositionArea.PositionAreaName == ConfigData.T3F_ElevatorEnterPOS
                                        if (robot.PositionZoneName == ConfigData.T3F_ElevatorEnterPOS
                                            && robot.Registers.dMiR_Register_Value31 == 2) // [MiR -> Elevator 진입완료]
                                        {
                                            //완료 되면 
                                            MiR_Put_Register(robot, 31, 3);
                                            mirStatusElevator.MiRStateElevator = "MiRStateElevatorLoaderComplet";
                                            uow.ElevatorState.Update(mirStatusElevator);
                                        }
                                        else if (robot.Registers.dMiR_Register_Value31 == 0) MiR_Put_Register(robot, 31, 1); // [MiR -> Elevator 진입]
                                        break;

                                    case "MiRStateElevatorUnLoaderStart":

                                        //if (ElevatorPositionArea != null)
                                        {
                                            if (!MiR_Get_Register(robot, 31)) break;

                                            //if (ElevatorPositionArea.PositionAreaName == ConfigData.M3F_ElevatorEnterPOS
                                            if (robot.PositionZoneName == ConfigData.M3F_ElevatorEnterPOS
                                                && robot.Registers.dMiR_Register_Value31 == 3 && robot.StateText == "Pause")
                                            {
                                                //[MiR -> Elevator 진출]
                                                MiR_Put_Register(robot, 31, 4);    //MiR_Elevator 상태 레지스터 쓰기
                                                RobotState robotStateChange = RobotState.Ready;
                                                int robotStateId = (int)robotStateChange;
                                                MiR_ReST_Send("PUT_STATUS", robot, robotStateId.ToString(), "");//레디 상태로 변경
                                            }
                                            //else if (ElevatorPositionArea.PositionAreaName == ConfigData.M3F_ElevatorEndPOS && robot.Registers.dMiR_Register_Value31 == 4)
                                            else if (robot.PositionZoneName == ConfigData.M3F_ElevatorEndPOS && robot.Registers.dMiR_Register_Value31 == 4)

                                            {
                                                //[MiR -> Elevator 진출완료]
                                                MiR_Put_Register(robot, 31, 5); //Elevator 진출 완료
                                                mirStatusElevator.MiRStateElevator = "MiRStateElevatorUnLoaderComplet";
                                                uow.ElevatorState.Update(mirStatusElevator);
                                            }

                                            else if (mirStatusElevator.ElevatorState == "CallEndDoorOpen" && robot.Registers.dMiR_Register_Value31 == 0)
                                            {
                                                mirStatusElevator.MiRStateElevator = "MiRStateElevatorUnLoaderComplet";
                                                uow.ElevatorState.Update(mirStatusElevator);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else if (Elevator_runmission.MissionName.Contains("Elevator_Down"))
                        {
                            var mirStatusElevator = uow.ElevatorState.Find(m => m.ElevatorMissionName == Elevator_runmission.MissionName && m.RobotName == robot.RobotName).FirstOrDefault();

                            if (mirStatusElevator == null)  // 등록된 상태가 없을때
                            {
                                //[목적지가 Up 일때] M3F Elevator Start Position (Start 대기위치)

                                //if (ElevatorPositionArea != null && ElevatorPositionArea.PositionAreaName == ConfigData.M3F_ElevatorStartPOS)
                                if (robot.PositionZoneName == ConfigData.M3F_ElevatorStartPOS)
                                {
                                    var newElevatorStateModelsAdd = new ElevatorStateModule
                                    {
                                        RobotName = robot.RobotName,
                                        ElevatorState = "CallStartFloor",
                                        MiRStateElevator = "",
                                        ElevatorMissionName = Elevator_runmission.MissionName,
                                    };
                                    uow.ElevatorState.Add(newElevatorStateModelsAdd);
                                }
                            }
                            else
                            {
                                switch (mirStatusElevator.MiRStateElevator)
                                {
                                    case "MiRStateElevatorLoaderStart":
                                        //[목적지가 Up 일때] M3F Elevator Position

                                        if (!MiR_Get_Register(robot, 31)) break;

                                        //if (ElevatorPositionArea != null && ElevatorPositionArea.PositionAreaName == ConfigData.M3F_ElevatorEnterPOS
                                        if (robot.PositionZoneName == ConfigData.M3F_ElevatorEnterPOS
                                            && robot.Registers.dMiR_Register_Value31 == 2)
                                        {
                                            // [MiR -> Elevator 진입완료]
                                            MiR_Put_Register(robot, 31, 3);
                                            mirStatusElevator.MiRStateElevator = "MiRStateElevatorLoaderComplet";
                                            uow.ElevatorState.Update(mirStatusElevator);
                                        }
                                        else if (robot.Registers.dMiR_Register_Value31 == 0)
                                        {
                                            //[MiR -> Elevator 진입]
                                            MiR_Put_Register(robot, 31, 1);
                                        }
                                        break;

                                    case "MiRStateElevatorUnLoaderStart":

                                        //if (ElevatorPositionArea != null)
                                        {
                                            if (!MiR_Get_Register(robot, 31)) break;

                                            //if (ElevatorPositionArea.PositionAreaName == ConfigData.T3F_ElevatorEnterPOS
                                            if (robot.PositionZoneName == ConfigData.T3F_ElevatorEnterPOS
                                                && robot.Registers.dMiR_Register_Value31 == 3 && robot.StateText == "Pause")
                                            {
                                                //[MiR -> Elevator 진출]
                                                MiR_Put_Register(robot, 31, 4);
                                                RobotState robotStateChange = RobotState.Ready;
                                                int robotStateId = (int)robotStateChange;
                                                MiR_ReST_Send("PUT_STATUS", robot, robotStateId.ToString(), "");//레디 상태로 변경
                                            }
                                            //[목적지가 Up 일때] M3F Elevator End Position
                                            //else if (ElevatorPositionArea.PositionAreaName == ConfigData.T3F_ElevatorEndPOS && robot.Registers.dMiR_Register_Value31 == 4)
                                            else if (robot.PositionZoneName == ConfigData.T3F_ElevatorEndPOS && robot.Registers.dMiR_Register_Value31 == 4)

                                            {
                                                MiR_Put_Register(robot, 31, 5); //Elevator 진출 완료
                                                mirStatusElevator.MiRStateElevator = "MiRStateElevatorUnLoaderComplet";
                                                uow.ElevatorState.Update(mirStatusElevator);
                                            }

                                            else if (mirStatusElevator.ElevatorState == "CallEndDoorOpen" && robot.Registers.dMiR_Register_Value31 == 0)
                                            {
                                                mirStatusElevator.MiRStateElevator = "MiRStateElevatorUnLoaderComplet";
                                                uow.ElevatorState.Update(mirStatusElevator);
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                        else continue;
                    }
                    else
                    //원래 End포지션에서 데이터를 변경하지만 타이밍상 다음 미션을 받았을때 데이터가 그냥 변경되지않고 넘어가버림 다음으로 넘어가면 엘리베이터 완료신호로 다시 전달함
                    {
                        //var mirStatusElevator = uow.ElevatorState.Find(m => m.MiRStateElevator == "MiRStateElevatorUnLoaderStart" && m.RobotName == robot.RobotName
                        //                                                && m.ElevatorState == "CallEndDoorOpen").FirstOrDefault();
                        //if (mirStatusElevator != null)
                        //{
                        //    mirStatusElevator.MiRStateElevator = "MiRStateElevatorUnLoaderComplet";
                        //    uow.ElevatorState.Update(mirStatusElevator);
                        //}

                        //엘리베이터 미션을 가지고 있지않은데 엘리베이터 상태가 있다면 삭제한다
                        var mirStatusElevator = uow.ElevatorState.Find(m => m.RobotName == robot.RobotName).FirstOrDefault();
                        if (mirStatusElevator != null)
                        {
                            uow.ElevatorState.Remove(mirStatusElevator);
                        }
                    }

                }
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }

        }

        private void Upgrade_M3FT3F_jobCancelControl(Robot robot)                     //<========== [JobCancel 제어]
        {

            // =========== 엘리베이터 기준으로 첫번째 미션만 Job 취소가 가능하다 ==========
            try
            {
                MiR_Get_Register(robot, 21);

                //- 첫번째 미션인것을 확인해서 jobCancel 시키는 구현(그룹에 일치하는 Cancel 미션을 검색한다.)
                var JobCancelmission = uow.JobConfigs.Find(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.jobCancelSignal == Convert.ToString(robot.Registers.dMiR_Register_Value21)
                                                           && (m.CallName.Contains("Cancel") || m.CallName.Contains("cancel"))).FirstOrDefault();

                if (JobCancelmission != null /*&& !string.IsNullOrEmpty(robot.PositionZoneName)*/)
                {

                    var runMissions = uow.Missions.Find(m => (m.Robot == robot || m.JobCreateRobotName == robot.RobotName) && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
                                            .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid").ToList(); // 완료 미션은 제외한다

                    var runMission_Job = runMissions.Where(m => m.JobId != 0).SingleOrDefault(); // 일반미션

                    if (runMission_Job != null) //일반 미션 삭제
                    {
                        //엘리베이터 미션이 아닌것만 삭제가능
                        if (runMission_Job.MissionName != "Elevator_Up" && runMission_Job.MissionName != "Elevator_Down")
                        {

                            //지금 로봇이 하고있는 미션이 Cancel미션과다른것인지 확인한다
                            var OperationCalljobCancel = uow.Jobs.Find(m => (m.RobotName == robot.RobotName || m.JobCreateRobotName == robot.RobotName)
                                                                   && (m.CallName.Contains("Cancel") || m.CallName.Contains("cancel")) == false
                                                                   && m.JobStateText != "JobDone").FirstOrDefault();

                            if (OperationCalljobCancel != null)
                            {
                                JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = OperationCalljobCancel.CallName });
                                JobCommandQueueAdd(JobCancelmission, robot);
                            }
                        }
                        else
                        {
                            //엘리베이터 미션진행중 JObCancel있을경우 레지스터 21번에 0으로 초기화한다.
                            MiR_Put_Register(robot, 21, 0);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }

        }

        bool Upgrade_WiseTowerLampReset = false;
        private void Upgrade_New_M3FT3F_WiseTowerLamp_Contorl()                           //<========== [Tower Lamp제어]
        {
            //=============================================== 미션 상태 Start =================================================//
            //1.WiseTowerLamp 위치에 Robot이오면 Robot에 들어있는  자재를 확인한다
            //2.로봇에 자재와 WiseTowerLamp 있는 설정되있는 자재를 확인하여 설정값과 같은 WiseModule 을 킨다
            //3.같은 자재가 있을시 마지막으로 들어온 것으로 Time을 조정한다
            //=================================================================================================================//

            try
            {
                //foreach (var TestTowerLamp in uow.WiseTowerLampConfigs.GetAll())
                //{
                //    if (TestTowerLamp.serviceData.RobotNames.Count > 0 && TestTowerLamp.serviceData.TowerLampOffTimerCompletSignals.Count > 0)
                //    {
                //        foreach (var item in TestTowerLamp.serviceData.RobotNames)
                //        {
                //            var Index = TestTowerLamp.serviceData.RobotNames.FindIndex(x => x.Equals(item));
                //            Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / RobotName = {item}");
                //            Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / TowerLampOffTimerCompletSignals ={TestTowerLamp.serviceData.TowerLampOffTimerCompletSignals[Index]}");

                //        }

                //    }
                //    else if (TestTowerLamp.serviceData.RobotNames.Count > 0 && TestTowerLamp.serviceData.TowerLampOffTimerCompletSignals.Count == 0)
                //    {
                //        foreach (var item in TestTowerLamp.serviceData.RobotNames)
                //        {
                //            Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / RobotName = {item}");
                //            Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / TowerLampOffTimerCompletSignals = Null");
                //        }
                //    }
                //    else if (TestTowerLamp.serviceData.RobotNames.Count == 0 && TestTowerLamp.serviceData.TowerLampOffTimerCompletSignals.Count > 0)
                //    {
                //        foreach (var item in TestTowerLamp.serviceData.TowerLampOffTimerCompletSignals)
                //        {
                //            Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / RobotName = Null");
                //            Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / TowerLampOffTimerCompletSignals = {item}");
                //        }
                //    }
                //    else
                //    {
                //        Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / RobotName == Null");
                //        Console.WriteLine($"Time = {DateTime.Now} / WiseTowerLampName = { TestTowerLamp.NameSetting} / TowerLampOffTimerCompletSignals = Null");

                //    }
                //}

                //WiseTowerLamp가 Auto 모드일때 사용한다
                var WiseTowerLampMode = uow.WiseTowerLampConfigs.Find(m => m.ControlSetting == "AutoControl").FirstOrDefault();
                if (WiseTowerLampMode != null)
                {
                    //1번의 OFF신호로 WiseModule가 꺼지지않은 증상이 있기때문에 아래 함수추가
                    WiseTowerLampOffCheck();


                    foreach (var robot in ActiveRobots().Where(r => r.ACSRobotGroup != "AMTWEST" && r.ACSRobotGroup != "AMTEAST").ToList())
                    {

                        //초기화 전부 wiseModule를 0으로 만든다
                        if (Upgrade_WiseTowerLampReset == false) Upgrade_WiseTowerLampReset = WiseTowerLampInit();
                        if (Upgrade_WiseTowerLampReset)
                        {

                            var WiseTowerLampPosition = uow.PositionAreaConfigs.UpGrade_GroupPOSArea(robot, ConfigData.WiseTowerLamp_ModuleGroup).FirstOrDefault();

                            if (WiseTowerLampPosition != null)
                            {
                                //로봇 이름과 일치 하는 것이 있는지(ON이될때 로봇Name등록됨)
                                var TowerLampOffFind = uow.WiseTowerLampConfigs.Find(m => m.TowerLampUseSetting == "Use" && m.serviceData.Status == "Connect"
                                                        && m.serviceData.RobotNames.Contains(robot.RobotName));

                                // Lamp Off
                                if (TowerLampOffFind.Count > 0)
                                {
                                    //WiseTowerLampPosition에 있는 Robot을 확인한다

                                    foreach (var TowerLampOffSelect in TowerLampOffFind)
                                    {
                                        //자재 사용
                                        if (TowerLampOffSelect.ProductActiveSetting == true)
                                        {
                                            //로봇정보와 일치하는 데이터를 찾는다
                                            //var Productdatas = uow.Products.Find(p => p.RobotName == robot.RobotName && p.ProductName == TowerLampOffSelect.productName).FirstOrDefault();

                                            //포지션이 다르거나 Robot 유무 확인 데이터가 없는경우(Robot자재List확인하지 않는다)
                                            //if (TowerLampOffSelect.PositionZoneSetting != WiseTowerLampPosition.PositionAreaName || Productdatas == null )
                                            //포지션이 다르거나 Robot 자재가 전부 없을경우(자재데이터를 입력하지않고 내려보내는 경우가 발생될수있음)
                                            if (TowerLampOffSelect.PositionZoneSetting != WiseTowerLampPosition.PositionAreaName || string.IsNullOrEmpty(robot.Product))
                                            {
                                                //로봇으로 Index조회하여 아래 조건을 만든다
                                                var robotdataIndex = TowerLampOffSelect.serviceData.RobotNames.FindIndex(x => x.Equals(robot.RobotName));
                                                if (robotdataIndex != -1)
                                                {

                                                    if (TowerLampOffSelect.serviceData.TowerLampOffTimerCompletSignals[robotdataIndex] == "TimerOff")
                                                    {
                                                        TowerLampOff(TowerLampOffSelect, "Delete", robot.RobotName);
                                                    }
                                                    else
                                                    {
                                                        if (TowerLampOffSelect.serviceData.Module_OutValue == 1)
                                                        {
                                                            TowerLampOff(TowerLampOffSelect, "TimerOff", robot.RobotName);
                                                        }
                                                    }
                                                }
                                            }
                                            else continue;

                                        }
                                        else  //자재 미사용
                                        {
                                            if (TowerLampOffSelect.PositionZoneSetting != WiseTowerLampPosition.PositionAreaName)
                                            {
                                                var robotdataIndex = TowerLampOffSelect.serviceData.RobotNames.FindIndex(x => x.Equals(robot.RobotName));
                                                if (robotdataIndex != -1)
                                                {
                                                    //if (TowerLampOffSelect.serviceData.TowerLampOffTimerCompletSignals[robotdataIndex] == false && TowerLampOffSelect.serviceData.Module_OutValue == 0) continue;
                                                    //else
                                                    {
                                                        //Wise Module 한번에 꺼지지않은 증상발견 보완
                                                        if (TowerLampOffSelect.serviceData.Module_OutValue == 1) TowerLampOff(TowerLampOffSelect, "TimerOff", robot.RobotName);
                                                        else TowerLampOff(TowerLampOffSelect, "Delete", robot.RobotName);
                                                    }
                                                }
                                            }
                                            else continue;
                                        }
                                    }
                                }

                                //Lamp On
                                var TowerLampOnFind = uow.WiseTowerLampConfigs.Find(m => m.PositionZoneSetting == WiseTowerLampPosition.PositionAreaName && m.TowerLampUseSetting == "Use" && m.serviceData.Status == "Connect");
                                if (TowerLampOnFind.Count > 0)
                                {
                                    foreach (var TowerLampOnSelect in TowerLampOnFind)
                                    {
                                        //자재 상태 신호를 확인했을경우
                                        if (TowerLampOnSelect.ProductActiveSetting == true && !string.IsNullOrEmpty(robot.Product))
                                        {
                                            //Robot 에 자재가 데이터가 있는경우
                                            //var Productdatas = uow.Products.Find(p => p.RobotName == robot.RobotName && p.ProductName == TowerLampOnSelect.productName).FirstOrDefault();
                                            //if (Productdatas != null)

                                            //Robot 자재 있음 데이터가 있는경우
                                            var ProductRobotdatas = uow.Products.Find(p => p.RobotName == robot.RobotName).ToList();
                                            if (ProductRobotdatas.Count > 0)
                                            {
                                                var productData = ProductRobotdatas.FirstOrDefault(x => x.ProductName == TowerLampOnSelect.productName);
                                                if (productData != null)
                                                {
                                                    //TowerLamp 설정 데이터와 일치 데이터가 있는경우
                                                    var robotdataIndex = TowerLampOnSelect.serviceData.RobotNames.FindIndex(x => x.Equals(robot.RobotName));
                                                    if (robotdataIndex != -1)
                                                    {
                                                        //램프가 켜져있을경우(Offtimer후 Timer Off)
                                                        if (TowerLampOnSelect.serviceData.TowerLampOffTimerCompletSignals[robotdataIndex] == "TimerOn" && TowerLampOnSelect.serviceData.Module_OutValue == 1
                                                            && TowerLampOnSelect.serviceData.WriteOutputOffSignalTime < DateTime.Now) TowerLampOff(TowerLampOnSelect, "TimerOff", robot.RobotName);
                                                    }
                                                    else
                                                    {
                                                        //새로운 타워램프 ON (robot을 ADD한다)
                                                        TowerLampOn(TowerLampOnSelect, robot.RobotName);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                //Robot 자재 있음 데이터가 없는경우 TowerLamp On한다

                                                var robotdataIndex = TowerLampOnSelect.serviceData.RobotNames.FindIndex(x => x.Equals(robot.RobotName));
                                                if (robotdataIndex != -1)
                                                {
                                                    //램프가 켜져있을경우(Offtimer후 Timer Off)
                                                    if (TowerLampOnSelect.serviceData.TowerLampOffTimerCompletSignals[robotdataIndex] == "TimerOn" && TowerLampOnSelect.serviceData.Module_OutValue == 1
                                                        && TowerLampOnSelect.serviceData.WriteOutputOffSignalTime < DateTime.Now) TowerLampOff(TowerLampOnSelect, "TimerOff", robot.RobotName);
                                                }
                                                else
                                                {
                                                    //새로운 타워램프 ON (robot을 ADD한다)
                                                    TowerLampOn(TowerLampOnSelect, robot.RobotName);
                                                }
                                            }
                                        }
                                        //자재 여부 (무)
                                        else if (TowerLampOnSelect.ProductActiveSetting == false && string.IsNullOrEmpty(robot.Product))
                                        {
                                            var robotdataIndex = TowerLampOnSelect.serviceData.RobotNames.FindIndex(x => x.Equals(robot.RobotName));
                                            if (robotdataIndex != -1)
                                            {
                                                //램프가 켜져있을경우(Offtimer후 Timer Off)
                                                if (TowerLampOnSelect.serviceData.TowerLampOffTimerCompletSignals[robotdataIndex] == "TimerOn" && TowerLampOnSelect.serviceData.Module_OutValue == 1
                                                    && TowerLampOnSelect.serviceData.WriteOutputOffSignalTime < DateTime.Now) TowerLampOff(TowerLampOnSelect, "TimerOff", robot.RobotName);
                                            }
                                            else
                                            {
                                                //새로운 타워램프 ON (robot을 ADD한다)
                                                TowerLampOn(TowerLampOnSelect, robot.RobotName);
                                            }

                                        }
                                        else continue;
                                    }
                                }
                            }
                            else
                            {
                                //선택 위치가 
                                var TowerLampOffFind = uow.WiseTowerLampConfigs.Find(m => m.TowerLampUseSetting == "Use" && m.serviceData.Status == "Connect" && m.serviceData.RobotNames.Contains(robot.RobotName));
                                // Lamp Off
                                if (TowerLampOffFind.Count > 0)
                                {
                                    foreach (var TowerLampOffSelect in TowerLampOffFind)
                                    {
                                        //로봇으로 Index조회하여 아래 조건을 만든다
                                        var robotdataIndex = TowerLampOffSelect.serviceData.RobotNames.FindIndex(x => x.Equals(robot.RobotName));
                                        if (robotdataIndex != -1)
                                        {

                                            if (TowerLampOffSelect.serviceData.TowerLampOffTimerCompletSignals[robotdataIndex] == "TimerOff")
                                            {
                                                TowerLampOff(TowerLampOffSelect, "Delete", robot.RobotName);
                                            }
                                            else
                                            {
                                                if (TowerLampOffSelect.serviceData.Module_OutValue == 1)
                                                {
                                                    TowerLampOff(TowerLampOffSelect, "TimerOff", robot.RobotName);
                                                }

                                                else //타워램프가 꺼져있는데 데이터가 ON으로 남아있는경우 초기화한다
                                                {
                                                    TowerLampOff(TowerLampOffSelect, "Delete", robot.RobotName);
                                                }
                                            }
                                        }

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
            // =============== local functions ===============

            bool WiseTowerLampInit()
            {
                var TowerLampDataResetFind = uow.WiseTowerLampConfigs.Find(m => m.TowerLampUseSetting == "Use" && m.serviceData.Status == "Connect"
                                                                       && (m.serviceData.RobotNames.Count > 0 || m.serviceData.Module_OutValue == 1 || m.serviceData.TowerLampOffTimerCompletSignals.Count > 0));

                if (TowerLampDataResetFind.Count > 0)
                {
                    foreach (var towerLampDateResetSelect in TowerLampDataResetFind)
                    {
                        TowerLampOff(towerLampDateResetSelect, "Reset");

                    }
                    return false;
                }
                else return true;
            }

            void WiseTowerLampOffCheck()
            {
                //1번의 신호로 안꺼지는 경우가 있기때문에 Chack루틴
                //로봇정보가 없는데 켜잇는경우
                var WiseTowerLampOffChecks = uow.WiseTowerLampConfigs.Find(m => m.TowerLampUseSetting == "Use" && m.serviceData.Status == "Connect"
                                                                     && (m.serviceData.RobotNames.Count == 0 && m.serviceData.Module_OutValue == 1 && m.serviceData.TowerLampOffTimerCompletSignals.Count == 0));
                if (WiseTowerLampOffChecks.Count > 0)
                {
                    foreach (var wiseTowerLampOffCheck in WiseTowerLampOffChecks)
                    {
                        TowerLampOff(wiseTowerLampOffCheck, "WiseOffCheck");
                    }
                }
            }


            void TowerLampOn(WiseTowerLampConfigModel towerLampOn, string robotName)
            {


                //기존 Robot들의 상태값을 변경하기 위함
                var TowerLampRobots = towerLampOn.serviceData.RobotNames.Where(x => x.Equals(robotName) == false).ToList();
                if (TowerLampRobots.Count > 0)
                {
                    foreach (var TowerLampRobot in TowerLampRobots)
                    {
                        //기존Robot 상태값을 변경하기위하여 Robot이름으로 Index조회하여 변경한다
                        var TowerLampRobotIndex = towerLampOn.serviceData.RobotNames.FindIndex(x => x.Equals(TowerLampRobot));
                        var TowerLampSignal = towerLampOn.serviceData.TowerLampOffTimerCompletSignals[TowerLampRobotIndex] = "TimerOff";
                    }
                    // 현재 들어온 값을 ADD한다 최종적인 값을 타임적용한다.
                    towerLampOn.serviceData.WriteOutputSignalFlag = 1;
                    towerLampOn.serviceData.WriteOutputSignalValue = 1;
                    towerLampOn.serviceData.WriteOutputOnSignalTime = DateTime.Now;
                    towerLampOn.serviceData.WriteOutputOffSignalTime = towerLampOn.serviceData.WriteOutputOnSignalTime.AddSeconds(towerLampOn.OperationtimeSetting);
                    towerLampOn.serviceData.TowerLampOffTimerCompletSignals.Add("TimerOn");
                    towerLampOn.serviceData.RobotNames.Add(robotName);
                }
                else
                {
                    // 기존 Robot이 없다면 현재 들어온 값을 ADD한다 최종적인 값을 타임적용한다.
                    towerLampOn.serviceData.WriteOutputSignalFlag = 1;
                    towerLampOn.serviceData.WriteOutputSignalValue = 1;
                    towerLampOn.serviceData.WriteOutputOnSignalTime = DateTime.Now;
                    towerLampOn.serviceData.WriteOutputOffSignalTime = towerLampOn.serviceData.WriteOutputOnSignalTime.AddSeconds(towerLampOn.OperationtimeSetting);
                    towerLampOn.serviceData.TowerLampOffTimerCompletSignals.Add("TimerOn");
                    towerLampOn.serviceData.RobotNames.Add(robotName);
                }
            }

            void TowerLampOff(WiseTowerLampConfigModel towerLampOff, string TimerSignal, string RobotName = "")
            {
                switch (TimerSignal)
                {
                    case "Reset":
                        towerLampOff.serviceData.RobotNames.Clear();
                        towerLampOff.serviceData.TowerLampOffTimerCompletSignals.Clear();
                        towerLampOff.serviceData.WriteOutputSignalFlag = 1;
                        towerLampOff.serviceData.WriteOutputSignalValue = 0;
                        towerLampOff.serviceData.WriteOutputOffSignalTime = DateTime.Now;
                        break;

                    //Timer로 끄는경우
                    case "TimerOff":
                        //Robot정보가 있는지 확인한다
                        var TimeOverRobotIndex = towerLampOff.serviceData.RobotNames.FindIndex(x => x.Equals(RobotName));
                        if (TimeOverRobotIndex != -1)
                        {
                            //해당 로봇정보에 TowerLampTimer 신호가 ON이면 Off신호로 변경해준다
                            if (towerLampOff.serviceData.TowerLampOffTimerCompletSignals[TimeOverRobotIndex] == "TimerOn")
                            {
                                towerLampOff.serviceData.RobotNames[TimeOverRobotIndex] = RobotName;
                                towerLampOff.serviceData.TowerLampOffTimerCompletSignals[TimeOverRobotIndex] = TimerSignal;
                            }
                            //Timer 시간이 완료 되지 않은 Robot을 검색한다 
                            var TowerLampRunRobot = towerLampOff.serviceData.TowerLampOffTimerCompletSignals.FirstOrDefault(x => x.Equals("TimerOn"));
                            // 모두가 타이머가 OFF 신호일때 해당 TowerLamp OFF한다 
                            if (TowerLampRunRobot == null)
                            {
                                towerLampOff.serviceData.WriteOutputSignalFlag = 1;
                                towerLampOff.serviceData.WriteOutputSignalValue = 0;
                                towerLampOff.serviceData.WriteOutputOffSignalTime = DateTime.Now;
                            }
                        }
                        break;

                    case "Delete":
                        var DeleteRobotIndex = towerLampOff.serviceData.RobotNames.FindIndex(x => x.Equals(RobotName));
                        if (DeleteRobotIndex != -1)
                        {
                            //로봇정보 대한 Index 내용을 삭제한다
                            towerLampOff.serviceData.RobotNames.RemoveAt(DeleteRobotIndex);
                            towerLampOff.serviceData.TowerLampOffTimerCompletSignals.RemoveAt(DeleteRobotIndex);

                            //Timer 시간이 완료 되지 않은 Robot을 검색한다 
                            var TowerLampRunRobot = towerLampOff.serviceData.TowerLampOffTimerCompletSignals.FirstOrDefault(x => x.Equals("TimerOn"));
                            // 모두가 타이머가 OFF 신호일때 해당 TowerLamp OFF한다 
                            if (TowerLampRunRobot == null)
                            {
                                towerLampOff.serviceData.WriteOutputSignalFlag = 1;
                                towerLampOff.serviceData.WriteOutputSignalValue = 0;
                                towerLampOff.serviceData.WriteOutputOffSignalTime = DateTime.Now;
                            }

                        }
                        break;
                    case "WiseOffCheck":
                        {
                            towerLampOff.serviceData.WriteOutputSignalFlag = 1;
                            towerLampOff.serviceData.WriteOutputSignalValue = 0;
                            towerLampOff.serviceData.WriteOutputOffSignalTime = DateTime.Now;
                        }
                        break;
                }
            }

            //==================================================

        }

        private void Upgrad_AmkorK5_RegisterSync()

        {
            try
            {
                //=============================================== RegisterSync 방법 =================================================//
                //1.Register Sync Use 된 그룹과 로봇에 설정된 그룹이 일치한 Robot에게 설정된 레지스터번호 에 0으로 전달한다.
                //2.Register Sync 설정된 위치에 있으면 설정된 레지스터번호와 레지스터 값을 다른로봇에게 변경해준다.
                //=================================================================================================================//

                //Elevator 상태값을 MiR 레지스터로 전송하기 위함

                if (ConfigData.ElevatorModeSendRobotUse == "Use")
                {
                    var ElevatorMode = uow.ACSModeInfo.Find(a => a.Location == "Elevator" && a.ElevatorMode == "AGVMode").FirstOrDefault();
                    if (ElevatorMode != null)
                    {
                        //AGV모드 경우
                        foreach (var ElevatorModeSendRobot in ActiveRobots().Where(a => a.ACSRobotGroup == ConfigData.ElevatorModeSendRobotGroup))
                        {
                            MiR_Put_Register(ElevatorModeSendRobot, 34, ConfigData.ElevatorModeSendRobotAGVModeSignal);
                        }
                    }
                    //AGV모드 아닐경우
                    else
                    {
                        foreach (var ElevatorModeSendRobot in ActiveRobots().Where(a => a.ACSRobotGroup == ConfigData.ElevatorModeSendRobotGroup))
                        {
                            MiR_Put_Register(ElevatorModeSendRobot, 34, ConfigData.ElevatorModeSendRobotNotAGVModeSignal);
                        }
                    }

                }

                //Door 관련 딜레이 Time Put레지스터만 한다
                if (ConfigData.DoorOpenRobotStatusChangeTimeUse == "Use")
                {
                    foreach (var DoorOpenRobotStatusChangeTimeSync in ActiveRobots().Where(a => a.ACSRobotGroup == ConfigData.DoorOpenRobotStatusChangeGroup).ToList())
                    {
                        MiR_Put_Register(DoorOpenRobotStatusChangeTimeSync, 40, ConfigData.DoorOpenRobotStatusChangeTime);
                    }
                }

                //레지스터 싱크 설정 Use 이고 그룹이 None 아닌 상태인 항목만 레지스터를 공유한다
                var RegisterSyncs = uow.RobotRegisterSync.Find(r => r.RegisterSyncUse == "Use" && r.ACSRobotGroup != "None");

                //var 중복되는값찾기 = uow.RobotRegisterSync.GetAll().GroupBy(x => x.RegisterNo).Where(w=>w.).ToList();

                //레지스터 싱크 활성화가 되어있는것
                foreach (var RegisterSync in RegisterSyncs)
                {
                    //싱크 활성화되어있는 것중에 Robot 그룹이 일치한 Robot을 검색한다 
                    var GroupRobot = ActiveRobots().Where(a => a.ACSRobotGroup == RegisterSync.ACSRobotGroup).ToList();
                    // 검색한 로봇중 로봇포지션과 설정포지션이 일치한 로봇을 검색한다
                    var PositionRobot = GroupRobot.Where(c => c.PositionZoneName == RegisterSync.PositionName).FirstOrDefault();

                    //일치 로봇이 있다면
                    if (PositionRobot != null)
                    {
                        //그룹에 로봇을 전부 설정 레지스터로 변경한다
                        foreach (var robot in GroupRobot)
                        {
                            MiR_Put_Register(robot, RegisterSync.RegisterNo, RegisterSync.RegisterValue);
                        }
                    }
                    //일치하는 로봇이 없다면
                    else
                    {
                        //그룹에 로봇을 전부 레지스터 값을 0으로 변경한다
                        foreach (var robot in GroupRobot)
                        {
                            MiR_Put_Register(robot, RegisterSync.RegisterNo, 0);
                        }
                    }

                    //Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void JobCommandQueueAdd(JobConfigModel jobConfig, Robot robot)        //<==========미션Add [Mission추가완료]
        {
            try
            {
                JobCommandQueue.Enqueue(new JobCommand
                {
                    Code = JobCommandCode.ADD,
                    Text = jobConfig.CallName,
                    Extra4 = robot?.RobotName        //job 생성시 로봇지정

                });
                //Job 초기 등록시 Mission등록되는 순서가 늦어서 mission목적지를 부르지 못하는 증상발견 추가하였음
                Process_JobCommandQueue();
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void JobCommandQueueDelete(Job runjob)        //<==========미션Add [Mission추가완료]
        {
            try
            {
                JobCommandQueue.Enqueue(new JobCommand
                {
                    Code = JobCommandCode.REMOVE,
                    Text = runjob.CallName,
                });
                //Job 초기 등록시 Mission등록되는 순서가 늦어서 mission목적지를 부르지 못하는 증상발견 추가하였음
                Process_JobCommandQueue();
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void DoorOpenRobotStatusChangeTime()
        {
            try
            {
                //Robot Door Open 및 Close 상태에 따라서 정지 및 Ready를 변경하지만 Time은 레지스터 변경은 ACS에서 한다
                foreach (var robot in ActiveRobots().Where(w => w.ACSRobotGroup == ConfigData.DoorOpenRobotStatusChangeGroup).ToList())
                {
                    //설정된 그룹과 일치하고 40번 레지스터가 timer와 다른경우
                    if (robot.Registers.dMiR_Register_Value40 != ConfigData.DoorOpenRobotStatusChangeTime)
                    {
                        //레지스터가 변경되었는지 확인차 다시 레지스터를 Get한다
                        MiR_Get_Register(robot, 40);

                        //새로 읽어온 레지스터가 설정된time과 다르면 40번레지스터를 설정 Timer로 전송한다
                        if (robot.Registers.dMiR_Register_Value40 != ConfigData.DoorOpenRobotStatusChangeTime)
                        {
                            MiR_Put_Register(robot, 40, ConfigData.DoorOpenRobotStatusChangeTime);
                        }
                        else continue;

                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        bool notAGVMode = false;
        DateTime elevatorStateChangeTime;
        private void ElevatorStateChangeDeleteJob()
        {
            if (ConfigData.ElevatorModeSendRobotUse == "Use")
            {
                var ElevatorMode = uow.ACSModeInfo.Find(a => a.Location == "Elevator" && a.ElevatorMode == "NotAGVMode").FirstOrDefault();
                //Elevator NotAGVMode 상태인경우
                if (ElevatorMode != null)
                {

                    if (notAGVMode)
                    {
                        //지금 현재 시간이 경과시간보다 크거나 같으면
                        if (DateTime.Now >= elevatorStateChangeTime)
                        {
                            foreach (var robot in ActiveRobots().Where(w => w.ACSRobotGroup == ConfigData.ElevatorModeSendRobotGroup))
                            {
                                var ElevatorjobConfig = uow.JobConfigs.Find(j => j.ACSMissionGroup == robot.ACSRobotGroup && j.ElevatorModeActive == true && j.ElevatorModeValue == "AGVMode").ToList();

                                var runjob = uow.Jobs.Find(f => (f.JobCreateRobotName == robot.RobotName || f.RobotName == robot.RobotName) && f.JobStateText != "Done"
                                                           && ElevatorjobConfig.Count(c => f.CallName == c.CallName) != 0).FirstOrDefault();

                                ////job이 있으면 job 삭제 후 Wait미션 전송(출발지 M3F 이고 도착지 T3F || 출발지가 T3F 이고 도착지가 M3F 이고 완료가 되지않은 job)
                                //var runjobs = uow.Jobs.Find(j => ((j.CallName.Split('_')[0].StartsWith("M3F") && j.CallName.Split('_')[1].StartsWith("T3F"))
                                //                            || (j.CallName.Split('_')[0].StartsWith("T3F") && j.CallName.Split('_')[1].StartsWith("M3F")))
                                //                            && (j.RobotName == robot.RobotName || j.JobCreateRobotName == robot.RobotName)
                                //                            && j.JobStateText != "Done").FirstOrDefault();
                                if (runjob != null)
                                {
                                    //robot 엘리베이터 탑승 상태 
                                    var elevatorStatus = uow.ElevatorState.Find(e => e.RobotName == runjob.RobotName || e.RobotName == runjob.JobCreateRobotName).FirstOrDefault();
                                    //robot이 없는경우
                                    if (elevatorStatus == null)
                                    {
                                        Job_DeleteJobData(runjob);
                                        Upgrade_AmkorK5_AutoWaiting_Mission(robot);
                                    }
                                    //robot이 있는경우
                                    else
                                    {
                                        //robot 탑승 상태를 주지 않은경우
                                        if (string.IsNullOrEmpty(elevatorStatus.MiRStateElevator))
                                        {
                                            //해당 Robot대한 상태를삭제
                                            uow.ElevatorState.Remove(elevatorStatus);
                                            Job_DeleteJobData(runjob);
                                            Upgrade_AmkorK5_AutoWaiting_Mission(robot);
                                        }
                                        //robot 탑승 상태를 주었을경우
                                        else continue;
                                    }
                                }
                                //층간이송 job이없는경우
                                else
                                {
                                    if (robot.StateID == RobotState.Ready)
                                    {
                                        //Robot이 Ready상태인데 층간이송관련된 위치에 있을시 Waiting포지션으로 보낸다
                                        var ElevatorjobConfig1 = uow.JobConfigs.Find(j => j.ACSMissionGroup == robot.ACSRobotGroup && j.ElevatorModeActive == true && j.ElevatorModeValue == "AGVMode"
                                                                                    && j.CallName.EndsWith(robot.PositionZoneName)).FirstOrDefault();
                                        if (ElevatorjobConfig1 != null)
                                        {
                                            Upgrade_AmkorK5_AutoWaiting_Mission(robot);
                                        }
                                        else continue;
                                    }


                                }
                            }
                        }
                    }
                    else
                    {
                        //NotAGVMode 처음 시작신호 Time ADD
                        notAGVMode = true;
                        elevatorStateChangeTime = DateTime.Now.AddMinutes(ConfigData.ElevatorModeSendRobotJobDeleteTime);
                    }

                }
                // AGV 모드 경우
                else notAGVMode = false;
            }
        }

        private void MiRReset()
        {
            if (ConfigData.RobotResetUse == "Use")
            {
                foreach (var robot in ActiveRobots().Where(a => a.ACSRobotGroup == ConfigData.RobotResetGroupName).ToList())
                {
                    var runResetMission = uow.Missions.Find(m => (m.JobCreateRobotName == robot.RobotName || m.RobotName == robot.RobotName)
                                                            && m.ReturnID > 0 && m.CallName == $"None_{ConfigData.RobotResetMissionName}"
                                                            && m.MissionState != "Done" && m.MissionState != "Invalid").FirstOrDefault();
                    //리셋미션이 진행중인것은 제외한다
                    if (runResetMission == null)
                    {
                        if (!MiR_Get_Register(robot, ConfigData.RobotResetRegisterNo)) continue;

                        if (robot.Registers.dMiR_Register_Value98 == ConfigData.RobotResetRegisterValue)
                        {
                            //엘리베이터 삭제한다
                            var runElevator = uow.ElevatorState.Find(e => e.RobotName == robot.RobotName).FirstOrDefault();
                            if (runElevator != null) uow.ElevatorState.Remove(runElevator);

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
                                    Job_DeleteJobData(job, 3);
                                    Process_JobCommandQueue();

                                }

                                // 리셋 미션 전송한다
                                SendMission(robot, new Mission
                                {
                                    ACSMissionGroup = robot.ACSRobotGroup,
                                    CallName = $"None_{ConfigData.RobotResetMissionName}",
                                    MissionName = ConfigData.RobotResetMissionName
                                });
                            }


                        }
                    }
                }
            }
        }

    }
}
