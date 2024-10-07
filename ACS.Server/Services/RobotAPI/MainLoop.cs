using ACS.RobotApi;
using Dapper;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INA_ACS_Server
{
    public partial class MainLoop : JobControl_Base
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly static ILog UserLogger = LogManager.GetLogger("User"); //버튼 및 화면조작관련 Log
        private readonly static ILog WorkLogger = LogManager.GetLogger("Work"); //물동량 관련 Log
        private readonly static ILog TimeoutLogger = LogManager.GetLogger("Timeout");
        private readonly static ILog WiseTowerLampLogger = LogManager.GetLogger("WiseTowerLamp");
        private readonly static ILog ScanTimeLogger = LogManager.GetLogger("ScanTime");


        private readonly IFleetApi _fleetApi;
        private readonly MainForm main;
        private readonly IUnitOfWork uow;

        //private readonly PlcService plcSvc;
        //private readonly PopService popSvc;
        //private readonly WiseModuleService wiseModuleSvc;
        //private readonly WiseTowerLampService wiseTowerLampSvc;

        static object dataReadLock = new object();
        Stopwatch GetMissionstopwatch = new Stopwatch();

        public MainLoop(MainForm main, IUnitOfWork uow,
                            IFleetApi fleetApi) : base(main, uow)

        {
            this.main = main;
            this.uow = uow;
            this._fleetApi = fleetApi;

            //지정Class만 사용하고 싶을때
            //Misumi_Inchon misumi_Inchon = (Misumi_Inchon)jobControl;
            //ms_ver.AicellomilimIkSan_JobStartControl_Call

        }


        public void Start()
        {
            // main loop
            Task.Run(() => Loop());

            //register sync
            //Task.Run(() => PutRegisterLoop());
        }

        protected async Task PutRegisterLoop()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    stopwatch.Restart();
                    RegisterSync();
                    //LogScanTime($"PutRegisterCompleteTime = {stopwatch.Elapsed.TotalMilliseconds:0.000} ms");
                    LogScanTime($"PutRegisterCompleteTime = {stopwatch.Elapsed.TotalSeconds:0.000} sec");
                    await Task.Delay(100); //API 접속시 접속이 끈키는 증상이 발견되어 Delay 줌!!!!
                }
                catch (Exception ex)
                {
                    main.LogExceptionMessage(ex);
                }

            }
        }


        // 미션 스케줄링 처리 루프
        protected async Task Loop()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (true)
            {
                try
                {
                    stopwatch.Restart();

                    MissionControl();
                    // ============ get data
                    var getDataOK = await GetData();    //전체 Robot 데이터
                    if (getDataOK)
                    {
                        // ============ update db
                        foreach (var robot in uow.Robots.GetAll())
                        {
                            uow.Robots.Update(robot);
                        }

                        //// ========== Robot Position 값을 Upadata한다.
                        PositionAreaUpdate();

                        Process_JobCommandQueue();
                        // ========== JOB START 제어
                        JobStartControl();
                        // ========== 미션 전송 처리
                        PostJobControl();
                        // ========== 모든 미션의 상태를 갱신한다
                        UpdateMissionState();
                        // ========== 모든 정상적인 job 에 대해... 미션을 순차적으로 전송대기(Waiting)상태로 변경해 준다
                        JobHandler();
                        // ========== 특수 미션 처리
                        WaitingControl();
                        ChargingControl();
                        SpecialMissionDone_Handler();
                        JobReset();
                        // ========== Robot Error
                        AddRobotError();
                        // ========== Elevator 미션 처리
                        Upgrade_Elevator_Traffic_Control();
                        // ========== Email 보내기
                        //Email_Control();
                    }

                    // ============ check comm state
                    bool preState = main.bFleetConnected;
                    bool newState = getDataOK;
                    if (preState == false && newState == true)
                    {
                        EventLogger.Info("Fleet Connect");
                    }
                    main.bFleetConnected = newState; // update comm state

                    //============ delay
                    //LogScanTime($"FleetThreadCompleteTime = {stopwatch.Elapsed.TotalMilliseconds:0.000} ms");
                    LogScanTime($"FleetThreadCompleteTime = {stopwatch.Elapsed.TotalSeconds:0.000} sec");
                    await Task.Delay(500);
                }
                catch (Exception ex)
                {
                    main.LogExceptionMessage(ex);
                }
            }
        }


        private async Task<bool> GetData()
        {
            bool commOk = true; // 통신상태 (아래 일부에서 에러발생시 더이상 진행하지 않고 false로 리턴한다)


            //**********[FleetVersion]**********
            //사내 Fleet이 없기때문에 현재상태에선 Test 불가능!

            //Get Robot Id읽기
            //get fleet robot ids[FleetVersion]
            if (commOk)
                commOk = await GetFleetRobotIds_And_AssignToRobotsAsync();
            //Get Robot Status
            await GetFleetRobotStatus();

            //Get Mission 
            commOk = await GetFleetMissionAsync();

            //**********************************

            //**********[RobotVersion]**********

            //Get Robot Status
            //commOk = await GetRobotStatusAsync(); // 로봇상태정보 읽는다 (1개의 로봇이 접속이 되더라도 true 로 반환한다.)

            ////Get Robot Mission
            //if (GetMissionstopwatch.ElapsedMilliseconds == 0 || GetMissionstopwatch.ElapsedMilliseconds > 30000)  //30초간격으로 Mission을 읽어온다
            //{
            //    GetMissionstopwatch.Restart();
            //    await GetMissionsAsync();
            //}

            //**********************************

            return commOk;
        }

        #region Fleet Version GET API DATA
        // fleet에서 로봇 id 리스트를 가져와서, 각 로봇에 할당한다.
        private async Task<bool> GetFleetRobotIds_And_AssignToRobotsAsync()
        {
            // 1. robot id list 가져온다
            var robotIds = await _fleetApi.GetRobotIdsAsync();
            if (robotIds != null)
            {
                // 2. 가져온 robot id list에서 mir 최대 개수만큼만 선택한다
                // - fleet reboot 하면 순서 바뀌는 경우도 있어서, robot id 순으로 정렬한다
                robotIds = robotIds.OrderBy(x => x).Take(uow.Robots.GetCount()).ToList();

                // 3. 선택한 robot id list를 각 로봇에 할당한다
                for (int i = 0; i < robotIds.Count; i++)
                {
                    uow.Robots[i].RobotID = robotIds[i];
                }
                return true;
            }
            return false;
        }

        private async Task<bool> GetFleetMissionAsync()
        {
            var getMission = await _fleetApi.GetMissionsAsync();

            if (getMission != null)
            {
                main.GetMissions = getMission;
                return true;
            }
            else return false;
        }

        private async Task GetFleetRobotStatus()
        {
            foreach (var robot in uow.Robots.GetAll().ToList())
            {
                bool beforConnectState = robot.ConnectState;
                var robotStatus = await _fleetApi.GetRobotByIdAsync(robot.RobotID);

                lock (dataReadLock)
                {
                    if (robotStatus != null)
                    {
                        robotStatus.MapToRobot(robot);

                        if (robot.FleetState == FleetState.None || robot.FleetState == FleetState.unavailable) robot.ConnectState = false;
                        else robot.ConnectState = true;
                    }
                    else
                    {
                        robot.ConnectState = false;
                    }

                    // Robot 상태값 DB 갱신 업데이트
                    //uow.Robots.Update(robot);
                }

            }
        }
        #endregion

        #region Robot Version GET API DATA
        // 각 로봇의 상태정보를 가져온다
        private async Task<bool> GetRobotStatusAsync()
        {
            using (var semaphore = new SemaphoreSlim(initialCount: ConfigData.MiR_MaxNum)) // 2 = 동시에 진입가능한 스레드 수[KIG 수정]
            {
                var tasks = uow.Robots.GetAll().Select(async robot =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        return await DoWorkAsync(robot); // 각 태스크가 실제 작업하는 부분
                    }
                    finally { semaphore.Release(); }
                });

                // 모든 태스크가 완료될때까지 기다린다
                bool[] resultList = await Task.WhenAll(tasks);

                // 성공여부 반환한다
                return resultList.Any(x => x == true); // result가 한개라도 true이면 true
            }

            async Task<bool> DoWorkAsync(Robot robot)
            {
                bool beforConnectState = robot.ConnectState;
                var robotStatus = await robot.Api.GetStatusAsync();

                lock (dataReadLock)
                {
                    // Robot 데이터 복사
                    if (robotStatus != null)
                    {
                        robotStatus.MapToRobot(robot);
                        robot.ConnectState = true;
                    }
                    else
                    {
                        robot.ConnectState = false;
                    }

                    // Robot 상태값 DB 갱신 업데이트
                    //uow.Robots.Update(robot);

                    // Robot 연결상태 반환
                    return robot.ConnectState;
                }
            }
        }

        // 미션 리스트를 가져온다
        private async Task<bool> GetMissionsAsync()
        {
            bool returnValue = false;

            //KIG 수정 로봇별로 전체 미션 읽어오기
            foreach (var robot in GetActiveRobotsOrderbyDescendingBattery())
            {
                var getmissionList = await robot.Api.GetMissionsAsync();
                if (getmissionList != null)
                {
                    foreach (var getmission in getmissionList)
                    {
                        var getmissionFind = main.GetMissions.FirstOrDefault(x => x.guid == getmission.guid);
                        if (getmissionFind == null)
                        {
                            main.GetMissions.Add(getmission);

                        }
                        returnValue = true;
                    }
                }
            }
            return returnValue;
        }
        #endregion

        private void LogScanTime(string msg)
        {
            ScanTimeLogger.Info(msg);
            Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff} {1}", DateTime.Now, msg);  //Console.WriteLine(msg);
        }


        #region 미사용코드

#if NOT_USED_CODE

        #region 미션 상태 처리

        //// 콜버튼 미션 상태 갱신
        //private void UpdateCallButtonMissionState()
        //{
        //    // 콜버튼 연관 미션에 대해서...
        //    var missions = uow.Missions.Find(m => !string.IsNullOrEmpty(m.ACSMissionName)).ToList();
        //    foreach (var m in missions)
        //    {
        //        // 해당 미션의 콜버튼을 찾아서 미션상태를 갱신해 준다
        //        var callButton = uow.CallButtons.GetByName(m.ACSMissionName);
        //        if (callButton != null)
        //        {
        //            if (callButton.MissionStateText != m.MissionState)
        //            {
        //                callButton.MissionStateText = m.MissionState;
        //                uow.CallButtons.Update(callButton);
        //            }
        //        }
        //    }
        //}

        // 미션의 상태 갱신, 그 상태에 따른 처리
        private void UpdateRunMissionState()
        {
            try
            {
                // 전송한 미션을 모두 찾는다
                var runMissions = uow.Missions.Find(m => m.ReturnID > 0).ToList();

                // 전송한 미션들의 상태를 갱신한다
                foreach (var runMission in runMissions)
                {
                    string newMissionState = "";
                    int newRobotID = 0;

                    //int robotIndex = uow.Robots.GetIndexOf(runMission.Robot);
                    //if (robotIndex < 0)
                    //{
                    //    Debug.WriteLine($"UpdateRunMissionState.  robot not found~~~~~~~~~~{runMission.Robot}");
                    //    continue;
                    //}

                    // ========== 미션상태 갱신 ==========
                    //MiR 전원이 꺼져있을경우
                    if (runMission.Robot.Fleet_State == FleetState.None || runMission.Robot.Fleet_State == FleetState.unavailable)
                        continue;
                    //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                    if (string.IsNullOrWhiteSpace(runMission.Robot.RobotName) || string.IsNullOrWhiteSpace(runMission.Robot.StateText))
                        continue;

                    if (MiR_ReST_Send("GET_MISSION_QUEUE_ID", runMission.Robot, runMission.ReturnID.ToString(), "") == 1) // get mission-state by mission-return-id
                        {
                            newMissionState = main.MissionScheduler.state;
                            newRobotID = main.MissionScheduler.robot_id;

                            if ((runMission.MissionState != newMissionState) || (runMission.RobotID != newRobotID))
                            {
                                string debugMessage = string.Format("STATE CHANGE : {0}, {1}, {2}, {3} ~~~~~> {4} ",
                                                                    runMission.RobotID, runMission.ACSMissionName, runMission.MissionName, runMission.MissionState, newMissionState);
                                Debug.WriteLine(debugMessage);

                                runMission.MissionState = newMissionState;
                                runMission.RobotID = newRobotID; // fleet only.  미션전송시 미지정한 경우, 미션상태가 Executing일때 유효한 값이 들어온다

                                uow.Missions.Update(runMission);

                                // log
                                string message = $"{nameof(UpdateRunMissionState),-20}[--] {runMission}";
                                Console.WriteLine(message);
                                EventLogger.Info(message);
                                main.ACS_UI_Log(message);
                            }
                        }

                    // ========== 미션상태에 따른 처리 ==========

                    switch (runMission.MissionState)
                    {
                        case "Pending":
                        case "Executing":
                            break;

                        case "Done":
                            //// 리셋미션 삭제
                            //if (runMission.MissionName == ConfigData.sReset_MiR_Mission || runMission.MissionName == ConfigData.sAutoWaitingMission || runMission.MissionName == ConfigData.sAutoChargeMission)
                            //{
                            //    DeleteMission(runMission.Robot, runMission);
                            //}
                            //// 일반미션 삭제
                            //else
                            //{
                            //  if (DeleteMission(runMission.Robot, runMission))
                            //  {
                            //      // 콜버튼 미션카운터 갱신
                            //      var callButton = uow.CallButtons.GetByName(runMission.ACSMissionName);
                            //      if (callButton != null)
                            //      {
                            //          callButton.MissionCount++;
                            //          callButton.MissionStateText = "Done";
                            //          uow.CallButtons.Update(callButton); //"done"상태를 datebase에 업데이트
                            //          WorkLogger.Info($"{callButton.ButtonName},{callButton.MissionCount}");
                            //      }
                            //    }
                            //}

                            DeleteMission(runMission.Robot, runMission);
                            break;

                        case "Aborted":
                            Error_Reset_Charge_Waiting_PostMission(runMission.Robot, runMission);                  // 미션수행중 에러발생시 대체미션처리
                            break;
                        case "Invalid":
                            DeleteMission(runMission.Robot, runMission);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        #endregion


        #region 미션 전송

        private void PostMission()
        {
            // mir 상태 체크후 새로운 미션을 전송한다
            // - MiR 상태가 Ready 이고 배터리 정보가 미션가능상태이고 returnID, MissionName이 없어야 된다
            // - 또는 MiR 상태가 Executing 이고 배터리 정보가 미션가능상태일때 scheduler에 있는 미션이 충전미션, 대기미션일 때는 새로운 미션 전송가능하다 
            //
            // MiR에서 읽은 Register_1 자재 유무를 파악한다. Value 값이 0인 MiR을 조회한다. (Value = 0 자재없음 , Value = 1 자재있음)
            // 위 조건에 맞는 MiR을 선택한다.
            //
            // CallButton 에서 전송요청 미션이 있는지 확인한다.
            // CallButton 미션이 있으면 첫번째 미션을 선택한다.
            //
            // 선택한 MiR 기존에 가지고 있었던 미션정보를 초기화 한다.(Return id 존재하여 계속 물어볼경우 해당 Return id 대한 상태정보가 들어옴)
            // 선택한 MiR 에 미션을 전송한다. 

            //try
            {
                const int Product_REGISTER_NO = 20;         //자재유무 확인 VALUE = 1(자재 유) VALUE = 0(자재 무)
                const int TopModuleAuto_REGISTER_NO = 11;  //Top모듈 콘베어 Auto일시에만 미션진행 추가 진행 해야함 VALUE = 1(AUTO) VALUE = 0(ERROR)

                // 각 mir에 대해..
                foreach (var robot in uow.Robots.GetAll())
                {
                    //로봇 전원이 꺼져있는경우
                    if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                        continue;
                    //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                    if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                        continue;

                    var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다


                    // (조건1) 실행중인 미션이 없을때 //미션 전송 사용(Active == true) 미사용(Active = false)
                    bool c1 = robot.ACSRobotActive == true
                            && robot.StateID == RobotState.Ready
                            && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent)
                            && runMission == null;

                    // (조건2) 실행중인 미션이 있을때 (자동충전미션 / 자동대기위치미션)
                    bool c2 = robot.StateID == RobotState.Executing
                            && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent)
                            && runMission != null
                            && (runMission.MissionName == ConfigData.sAutoChargeMission || runMission.MissionName == ConfigData.sAutoWaitingMission);


                    if (c1 || c2)
                    {
                        // 전송할 미션을 가져온다
                        Mission nextMission = GetNextMission(robot);
                        if (nextMission != null)
                        {
                            if (MiR_Get_Register(robot, Product_REGISTER_NO))
                            {
                                if (MiR_Get_Register(robot, TopModuleAuto_REGISTER_NO))
                                {
                                    // 해당 mir에 자재가 없다면 미션을 전송한다
                                    if (robot.Registers.dMiR_Register_Value20 == 0 && robot.Registers.dMiR_Register_Value11 == 1) // 0=자재없음 && TopModuleAuto = 1(Auto)
                                    {
                                        if (DeleteMission(robot, runMission))
                                            SendMission(robot, nextMission);
                                    }
                                }
                            }
                        }
                    }
                }


        #region ========= 로봇기준이 아닌 미션기준으로 처리한 구현 =========
                //// 대기중인 미션에 대해...
                //IEnumerable<Mission> waitMissions = uow.Missions.Find(m => m.ReturnID == 0).ToList();
                //foreach (var waitMission in waitMissions)
                //{
                //    // 새로운 미션을 수행가능한 로봇을 찾는다
                //    var robot = GetInputReadyRobot(targetRobots: uow.Robots.GetAll());

                //    //// 미션을 수행가능한 로봇이 없으면 더이상 할 일이 없으므로 바로 루프 종료
                //    //if (robot == null) break;

                //    //  찾은 로봇에 새로운 미션을 보낸다
                //    if (robot != null)
                //    {
                //        var oldMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다
                //        var newMission = waitMission;

                //        MiR_Get_Register(robot, REGISTER_NO);

                //        // 해당 mir에 자재가 없다면 미션을 전송한다
                //        if (robot.Registers.dMiR_Register_Value20 == 0) // 0=자재없음
                //        {
                //            if (DeleteMission(robot, oldMission))
                //                SendMission(robot, newMission);
                //        }
                //    }
                //}
        #endregion

            }
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.StackTrace);
            //    EventLogger.Info(ex.StackTrace);
            //    main.ACS_UI_Log(ex.InnerException?.Message ?? ex.Message);
            //}
        }


        // 인수로 전달된 로봇리스트에서 새로운 미션이 가능한 로붓을 찾는다
        private Robot GetInputReadyRobot(IList<Robot> targetRobots)
        {
            // 1. 레디상태인 로봇을 1대 찾는다
            var readyRobot = targetRobots.FirstOrDefault(r => r.StateID == RobotState.Ready && r.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent));

            // 2. 레디상태인 로봇이 없으면 특수미션을 수행중인 로봇을 1대 찾는다
            if (readyRobot == null)
            {
                readyRobot = targetRobots.Where(robot => robot.StateID == RobotState.Executing && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent))
                                         .Where(robot =>
                                         {
                                             var robotMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).FirstOrDefault();
                                             //var robotMission = uow.Missions.GetByReturnId(robot.MissionQueueID);

                                             // 해당 로봇의 미션이 특수미션인가?
                                             return robotMission.MissionName == ConfigData.sAutoChargeMission || robotMission.MissionName == ConfigData.sAutoWaitingMission;
                                         })
                                         .FirstOrDefault();
            }

            return readyRobot;
        }


        private bool CheckRobotReadyState(Robot robot)
        {
            var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다

            // (조건1) 실행중인 미션이 없을때 //미션 전송 사용(Active == true) 미사용(Active = false)
            bool c1 = robot.ACSRobotActive == true
                    && robot.StateID == RobotState.Ready
                    && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent)
                    && runMission == null;

            // (조건2) 실행중인 미션이 있을때 (자동충전미션 / 자동대기위치미션)
            bool c2 = robot.StateID == RobotState.Executing
                    && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent)
                    && runMission != null
                    && (runMission.MissionName == ConfigData.sAutoChargeMission || runMission.MissionName == ConfigData.sAutoWaitingMission);



            return robot != null && robot.StateID == RobotState.Ready && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent);
        }


        // 새로 전송할 미션을 찾는다
        private Mission GetNextMission(Robot robot)
        {
            // 아직 전송하지 않은 미션중에서 첫번째 것.
            return uow.Missions.Find(m =>
                                {
                                    bool flag = true;
                                    flag &= m.ACSMissionGroup == robot.ACSRobotGroup;
                                    flag &= m.ReturnID == 0;
                                    flag &= !string.IsNullOrWhiteSpace(m.ACSMissionName);
                                    flag &= !string.IsNullOrWhiteSpace(m.MissionName) && m.MissionName != "None";
                                    flag &= !string.IsNullOrWhiteSpace(m.MissionState) && m.MissionState == "Waiting";
                                    flag &= (m.Robot == null || (m.Robot != null && m.Robot == robot));   // 로봇이 할당된 경우 동일로봇인지 체크
                                    return flag;
                                })
                                .FirstOrDefault();
        }

        #endregion


        #region 미션 에러 복구



        #endregion


        #region 특수 미션 처리

        private void Fleet_AutoCharging_Mission()
        {
            try
            {
                const int Product_REGISTER_NO = 20;          //자재유무 확인 VALUE = 1(자재 유) VALUE = 0(자재 무)
                                                             // ConfigData 자동충전 사용 유무 와 ConfigData에 충전미션이 설정되어있는지 확인한다.
                if (ConfigData.sAutoChargeUse == "Use" && ConfigData.sAutoChargeMission.Length > 0 && (ConfigData.sAutoChargeMission != "None"))
                {
                    foreach (var robot in uow.Robots.GetAll())
                    {
                        if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                            continue;
                        //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                        if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                            continue;

                        var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다


                        // (조건1) mir이 충전 미션 진행중이고 mir배터리가 충전 완료 배터리와 크거나 같을경우 
                        bool c1 = (robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeEndPercent))
                                && runMission != null
                                && runMission.ReturnID > 0
                                && runMission.MissionName == ConfigData.sAutoChargeMission
                                && runMission.MissionState == "Executing";

                        // (조건2) 자재가 없고(mir Register_1_Value = 0), mir 배터리가 충전시작 배터리와 작거나 같고 mir상태가 Ready이고 진행중이 미션이 없을경우
                        bool c2 = robot.ACSRobotActive == true //미션 전송 사용(Active == true) 미사용(Active = false)
                               && robot.BatteryPercent <= double.Parse(ConfigData.sAutoChargeStartPercent)
                                //&& main.dMiR_Register_Value20[iMiR_No] == 0  // 아래if문 내부에서 레지스터값 다시 읽은후 검사
                                && robot.StateID == RobotState.Ready
                                && runMission == null;


                        // 충전이 완료되었으면 충전미션을 삭제한다
                        if (c1)
                        {
                            DeleteMission(robot, null);
                        }
                        // 충전이 필요하면 충전미션을 실행한다
                        else if (c2)
                        {
                            MiR_Get_Register(robot, Product_REGISTER_NO);
                            if (robot.Registers.dMiR_Register_Value20 == 0) // 0=자재없음 && TopModuleAuto = 1(Auto)
                            {
                                if (DeleteMission(robot, null))
                                    SendMission(robot, new Mission { MissionName = ConfigData.sAutoChargeMission });
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

        private void Fleet_AutoWaiting_Mission()
        {
            try
            {
                const int Product_REGISTER_NO = 20;          //자재유무 확인 VALUE = 1(자재 유) VALUE = 0(자재 무)
                const int TopModuleAuto_REGISTER_NO = 11;    //Top모듈 콘베어 Auto일시에만 미션진행 추가 진행 해야함 VALUE = 1(AUTO) VALUE = 0(ERROR)

                // ConfigData 자동대기위치 사용 유무 와 ConfigData에 자동대기위치 미션이 설정되어있는지 확인한다.
                if (ConfigData.sAutoWaitingUse == "Use" && ConfigData.sAutoWaitingMission.Length > 0 && (ConfigData.sAutoWaitingMission != "None"))
                {
                    foreach (var robot in uow.Robots.GetAll())
                    {
                        if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                            continue;
                        //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                        if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                            continue;

                        var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다


                        var mir_pos_x = robot.Position_X;
                        var mir_pos_y = robot.Position_Y;

                        // mir상태가 Ready 이고 실행중인 미션이 없고 mir 배터리가 미션가능 배터리보다 크거나 같아야 한다.
                        bool c1 = robot.ACSRobotActive == true //미션 전송 사용(Active == true) 미사용(Active = false)
                               && robot.StateID == RobotState.Ready
                               && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent)
                               && runMission == null;

                        // ConfigData 설정된 자동대기위치 좌표값안에 mir 이 없는경우에만 미션을 전달할수있다. (좌표값안에 있으면 대기위치에 있는경우이기때문에 미션을 보낼필요없음)
                        bool c2 = mir_pos_x > ConfigData.dAutoWaiting_Position_X1 && mir_pos_x < ConfigData.dAutoWaiting_Position_X2
                               && mir_pos_x < ConfigData.dAutoWaiting_Position_X3 && mir_pos_x > ConfigData.dAutoWaiting_Position_X4
                               && mir_pos_y > ConfigData.dAutoWaiting_Position_Y1 && mir_pos_y > ConfigData.dAutoWaiting_Position_Y2
                               && mir_pos_y < ConfigData.dAutoWaiting_Position_Y3 && mir_pos_y < ConfigData.dAutoWaiting_Position_Y4;


                        if (c1 && !c2)
                        {
                            // Register_1 Value 값이 0인 MiR을 조회한다.
                            // 조건에 맞는 mir을 선택합니다
                            // 선택한 MiR 기존에 가지고 있었던 미션정보를 초기화 한다.
                            // 자동대기 위치 Mission 을 해당 mir에게 미션을 전송합니다.

                            MiR_Get_Register(robot, Product_REGISTER_NO);
                            MiR_Get_Register(robot, TopModuleAuto_REGISTER_NO);

                            // 자재가 없으면(레지스터값=0) 자동대기위치미션 실행한다
                            if (robot.Registers.dMiR_Register_Value20 == 0 && robot.Registers.dMiR_Register_Value11 == 1) // 0=자재없음 && TopModuleAuto = 1(Auto)
                            {
                                if (DeleteMission(robot, null))
                                    SendMission(robot, new Mission { MissionName = ConfigData.sAutoWaitingMission });
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

        private void MiR_Reset()
        {
            //ConfigData 선택된 MiR번호와 ResetMission 이 있을경우
            //ConfigData MiR 기존에 가지고 있었던 미션정보를 초기화 한다.
            //ConfigData 선택된 mir 을 Reset 미션을 전송합니다.
            try
            {
                if (ConfigData.sReset_MiR_No.Length > 0 && ConfigData.sReset_MiR_Mission.Length > 0)
                {
                    Robot robot = uow.Robots.GetByRobotName(ConfigData.sReset_MiR_Name);
                    if (robot != null) // mir found
                    {
                        ///////////////////////////
                        //Debug.Assert(false, "복수 미션 삭제 테스트-b");
                        //var robotMissions = uow.Missions.Find(m => m.Robot == robot).ToList(); // 해당 mir의 미션을 검색한다 (전송 여부 무관)
                        //DeleteMissions(robot, robotMissions);
                        ///////////////////////////

                        var robotMission = uow.Missions.Find(m => m.Robot == robot).SingleOrDefault(); // 해당 mir의 미션을 검색한다 (전송 여부 무관)

                        if (DeleteMission(robot, robotMission))
                        {
                            if (robotMission != null)
                            //CallButton 미션 진행하고 있는경우
                            {
                                //var callButton = uow.CallButtons.GetByName(robotMission.ACSMissionName);
                                //if (callButton != null)
                                //{
                                //    callButton.MissionStateText = "Done";
                                //    uow.CallButtons.Update(callButton); //"done"상태를 datebase에 업데이트
                                //}
                                SendMission(robot, new Mission { MissionName = ConfigData.sReset_MiR_Mission });
                            }
                            else //CallButton 미션을 진행 하지않을경우 자채적으로 Reset할경우
                            {
                                SendMission(robot, new Mission { MissionName = ConfigData.sReset_MiR_Mission });
                            }
                        }
                    }

                    ConfigData.sReset_MiR_No = string.Empty;    // 다음 리셋 요청을 받을 수 있도록 하기 위해 클리어...
                    ConfigData.sReset_MiR_Name = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        #endregion


        #region traffic 관리

        private void Position_Traffic_Control()
        {
            //mir 위치값경우 +50mm -50mm오차발생함(개선해야함)
            // ** 미션시작시 mir에서 처음 Traffic 대기위치 포지션 레지스터는 변경함 이후 레지스터는 ACS에서 변경해주어야함
            // Register_10 Value값은 ( 0=P0<목적지> / 1=P1<대기위치1번> / 2=P2<대기위치2번>) 미션시작시 앞에 Register_10 Value 값과 겹치지않아야 합니다.
            // mir Traffic 대기위치 이동은 2->1->0 번호순이며 3번 대기위치에 있어야 Register_10 Value 를 조회하여 Register를 변경해줍니다.
            // 2번에 mir위치해있으며 1번위치 0번위치에 mir이 없을경우 바로 0번으로 Register_10 Value값을 변경해줍니다.
            // 2번에 mir 위치해있고 1번위치에 mir이 있으면 1번위치에 있는 mir Register_10 Value값을 0으로 번경후 2번에 위치한 mir을 Register_10Value 값을 1로 변경하여 1번위치로 이동시킵니다.
            // Register_10을 읽고 쓴는 이유는 이동중 포지션위치값이 겹치거나 포지션위치값을 못읽는경우가 생길수 있기때문에 Register_10을 위치값으로하여 읽습니다.

            // mir 상태가 Executing 이고 Return id가 있고 mir 미션 이름이 sReset_MiR_Mission ,sAutoChargeMission ,sAutoWaitingMission 미션과 달라야 합니다.
            // ConfigData 설정된 sTraffic_Position_Area_Use[3] 이 사용 이여야합니다 (각 포지션마다 사용 미사용 설정)
            // mir Register_30 Value값이 2이고 ConfigData포지션값 안에 mir이 위치해있는지 확인합니다.
            // P2 위치에 있는 mir을 선택합니다
            // ConfigData 설정된 sTraffic_Position_Area_Use[2] 이 사용 이여야합니다 (각 포지션마다 사용 미사용 설정)
            // mir Register_30 Value값이 1 이거나  (mir Register_10 Value값이 1 이고 ConfigData포지션값 안에 mir이 위치해있는지 확인합니다.)
            // P1 위치에 있는 mir 선택합니다. 
            // ConfigData 설정된 sTraffic_Position_Area_Use[1] 이 사용 이여야합니다 (각 포지션마다 사용 미사용 설정)
            // mir Register_30 Value값이 0 이거나  (mir Register_10 Value값이 0 이고 ConfigData포지션값 안에 mir이 위치해있는지 확인합니다.)
            // P0 위치에 있는 mir 선택합니다.

            // P2 위치에 MIR이 있고 P1위치 와 P0위치에 MIR 이없으면 P2위치한 MIR Register_30 Value 값을 0으로 변경합니다.
            // P2 위치에 MIR이 있고 P1위치에 MIR이없고 P0위치에 MIR 이 있으면 Register_30 Value 값을 1로 변경합니다.
            // P2 위치에 MIR이 있고 P1위치에 MIR이 있고 P0위치에 MIR이없으면 P1위치에 있는 MIR Register_30 Value값을 0으로 변경후 P2위치에 있는 MiR Register_10 Value 값을 1로 변경합니다.
            // P2 위치에 MiR이 없고 P1위치에 MiR이 있으면 P1위치에 있는 MiR Register_30 Value 값을 0으로 변경합니다.

            // P3 위치에 MiR이 있으면 빈box 가 준비가 되어있으면 준비가 되어있다고 레지스터를 전송하여 빈box를 가지러 가게합니다.
            // p3 위치에 MiR 이 있으면 컨베이어 PLC 신호를 읽어서 빈box가 준비가 되어있으면 레지스터 21번에 1번으로 변경하고 컨베이어 MiR 에게 신호를 주었다고 다시 컨베이어PLC 써줍니다.
            // 빈 box buffer 3개가지 가능하며 PLC Address 는 각buffer 따로 잡아주어야함

            //미션 시작시 MiR에서 처음 Traffic 대기위치 포지션 레지스터는 변경함 이후 레지스터는 ACS 에서 변경해주어야함
            const int Product_REGISTER_NO = 20;         //자재유무 확인 VALUE = 1(자재 유) VALUE = 0(자재 무)
            const int TopModuleAuto_REGISTER_NO = 11;  //Top모듈 콘베어 Auto일시에만 미션진행 추가 진행 해야함 VALUE = 1(AUTO) VALUE = 0(ERROR)

            const int GetEmpty_REGISTER_NO = 21;
            const int GetEmpty_REGISTER_VALUE = 1;

            const int FullBoxPosition_REGISTER_NO = 30;
            const int FullBoxPosition_REGISTER_VALUE_P0 = 0;
            const int FullBoxPosition_REGISTER_VALUE_P1 = 1;

            Robot p0_robot = null;
            Robot p1_robot = null;
            Robot p2_robot = null;
            Robot p3_robot = null;

            try
            {
                foreach (var robot in uow.Robots.GetAll())
                {
                    if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                        continue;
                    //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
                    if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                        continue;

                    var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0).SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다

                    if (robot == null || runMission == null) continue;


                    if (robot.StateID == RobotState.Executing && runMission.ReturnID > 0
                     && ((runMission.MissionName != null
                     && runMission.MissionName != ConfigData.sReset_MiR_Mission
                     && runMission.MissionName != ConfigData.sAutoChargeMission
                     && runMission.MissionName != ConfigData.sAutoWaitingMission)
                     || (runMission.MissionName == null
                     && runMission.ErrorMissionName.Length > 0
                     && runMission.ErrorMissionName != ConfigData.sReset_MiR_Mission
                     && runMission.ErrorMissionName != ConfigData.sAutoChargeMission
                     && runMission.ErrorMissionName != ConfigData.sAutoWaitingMission)))
                    {
                        if (ConfigData.sTraffic_Position_Area_Use[4] == "Use") // Traffic Position Area4 Setting Empty box Load 
                        {
                            //1.MiR이 FullBox 위치에 있는지 확인
                            //2.빈box Buffer_1 자재가 있는지 확인(Buffer 는 3개까지 있음)
                            //3.MiR 빈Box Get 신호를 보냈는지 확인(Register 21 ==0)신호를 보내지않음
                            //4.MiR 빈box Get 신호를 보내지 않았아면 (Register 21 ==1)신호를 보냄
                            //5.PLC MiR에게 빈box Get 신호를 보냈다고 알림

                            if (ConfigData.dTraffic_Position_Area_X1[4] < robot.Position_X && ConfigData.dTraffic_Position_Area_X2[4] > robot.Position_X
                             && ConfigData.dTraffic_Position_Area_X3[4] > robot.Position_X && ConfigData.dTraffic_Position_Area_X4[4] < robot.Position_X
                             && ConfigData.dTraffic_Position_Area_Y1[4] < robot.Position_Y && ConfigData.dTraffic_Position_Area_Y2[4] < robot.Position_Y
                             && ConfigData.dTraffic_Position_Area_Y3[4] > robot.Position_Y && ConfigData.dTraffic_Position_Area_Y4[4] > robot.Position_Y)
                            {
                                if (EmptyboxBufferConveyer_1StandBy == true)
                                {
                                    if (MiR_Get_Register(robot, GetEmpty_REGISTER_NO))
                                    {
                                        if (robot.Registers.dMiR_Register_Value21 == 0)
                                        {
                                            p3_robot = robot;
                                        }
                                    }
                                }
                                else if (EmptyboxBufferConveyer_2StandBy == true)
                                {
                                    if (MiR_Get_Register(robot, GetEmpty_REGISTER_NO))
                                    {
                                        if (robot.Registers.dMiR_Register_Value21 == 0)
                                        {
                                            p3_robot = robot;
                                        }
                                    }
                                }
                                else if (EmptyboxBufferConveyer_3StandBy == true)
                                {
                                    if (MiR_Get_Register(robot, GetEmpty_REGISTER_NO))
                                    {
                                        if (robot.Registers.dMiR_Register_Value21 == 0)
                                        {
                                            p3_robot = robot;
                                        }
                                    }
                                }
                            }

                        }

                        if (ConfigData.sTraffic_Position_Area_Use[3] == "Use") //Traffic Position Area3 Setting Full Box 컨베이어 MiR 밀어내기
                        {
                            if (MiR_Get_Register(robot, FullBoxPosition_REGISTER_NO))
                            {
                                if (robot.Registers.dMiR_Register_Value30 == 2
                                 && ConfigData.dTraffic_Position_Area_X1[3] < robot.Position_X && ConfigData.dTraffic_Position_Area_X2[3] > robot.Position_X
                                 && ConfigData.dTraffic_Position_Area_X3[3] > robot.Position_X && ConfigData.dTraffic_Position_Area_X4[3] < robot.Position_X
                                 && ConfigData.dTraffic_Position_Area_Y1[3] < robot.Position_Y && ConfigData.dTraffic_Position_Area_Y2[3] < robot.Position_Y
                                 && ConfigData.dTraffic_Position_Area_Y3[3] > robot.Position_Y && ConfigData.dTraffic_Position_Area_Y4[3] > robot.Position_Y)
                                {
                                    if (MiR_Get_Register(robot, Product_REGISTER_NO))
                                    {
                                        if (MiR_Get_Register(robot, TopModuleAuto_REGISTER_NO))
                                        {
                                            if (robot.Registers.dMiR_Register_Value20 == 1 && robot.Registers.dMiR_Register_Value11 == 1) // 1=자재있음 && TopModuleAuto = 1(Auto)

                                            {
                                                p2_robot = robot;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (ConfigData.sTraffic_Position_Area_Use[2] == "Use") //Traffic Position Area2 Setting Full Box 컨베이어 MiR 밀어내기
                        {
                            if (MiR_Get_Register(robot, FullBoxPosition_REGISTER_NO))
                            {
                                if (robot.Registers.dMiR_Register_Value30 == 1
                                 || (robot.Registers.dMiR_Register_Value30 == 1 && ConfigData.dTraffic_Position_Area_X1[2] < robot.Position_X && ConfigData.dTraffic_Position_Area_X2[2] > robot.Position_X
                                 && ConfigData.dTraffic_Position_Area_X3[2] > robot.Position_X && ConfigData.dTraffic_Position_Area_X4[2] < robot.Position_X
                                 && ConfigData.dTraffic_Position_Area_Y1[2] < robot.Position_Y && ConfigData.dTraffic_Position_Area_Y2[2] < robot.Position_Y
                                 && ConfigData.dTraffic_Position_Area_Y3[2] > robot.Position_Y && ConfigData.dTraffic_Position_Area_Y4[2] > robot.Position_Y))
                                {
                                    if (MiR_Get_Register(robot, Product_REGISTER_NO))
                                    {
                                        if (MiR_Get_Register(robot, TopModuleAuto_REGISTER_NO))
                                        {
                                            if (robot.Registers.dMiR_Register_Value20 == 1 && robot.Registers.dMiR_Register_Value11 == 1) // 1=자재있음 && TopModuleAuto = 1(Auto)

                                            {
                                                p1_robot = robot;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        if (ConfigData.sTraffic_Position_Area_Use[1] == "Use") //Traffic Position Area3 Setting Full Box 컨베이어 MiR 밀어내기(MiR FullBox 도킹위치)
                        {
                            if (MiR_Get_Register(robot, FullBoxPosition_REGISTER_NO))
                            {
                                if (robot.Registers.dMiR_Register_Value30 == 0 || (robot.Registers.dMiR_Register_Value30 == 0
                                 && ConfigData.dTraffic_Position_Area_X1[1] < robot.Position_X && ConfigData.dTraffic_Position_Area_X2[1] > robot.Position_X
                                 && ConfigData.dTraffic_Position_Area_X3[1] > robot.Position_X && ConfigData.dTraffic_Position_Area_X4[1] < robot.Position_X
                                 && ConfigData.dTraffic_Position_Area_Y1[1] < robot.Position_Y && ConfigData.dTraffic_Position_Area_Y2[1] < robot.Position_Y
                                 && ConfigData.dTraffic_Position_Area_Y3[1] > robot.Position_Y && ConfigData.dTraffic_Position_Area_Y4[1] > robot.Position_Y))
                                {

                                    if (MiR_Get_Register(robot, Product_REGISTER_NO))
                                    {
                                        if (MiR_Get_Register(robot, TopModuleAuto_REGISTER_NO))
                                        {
                                            if (robot.Registers.dMiR_Register_Value20 == 1 && robot.Registers.dMiR_Register_Value11 == 1) // 1=자재있음 && TopModuleAuto = 1(Auto)

                                            {
                                                p0_robot = robot;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (p3_robot != null)
                {
                    MiR_Put_Register(p3_robot, GetEmpty_REGISTER_NO, GetEmpty_REGISTER_VALUE); // Get Empty Box Load 신호

                    //if(콘베어 PLC 에게 Load신호를 주었다고 다시 신호를 보내주어야함)
                    if (EmptyboxBufferConveyer_1StandBy == true)
                    {
                        DataSend?.Invoke(this, new PlcDataSendEventArgs("EmptyboxBuffer_1OrderByMiR", "1"));
                        EmptyboxBufferConveyer_1StandBy = false;
                    }
                    else if (EmptyboxBufferConveyer_2StandBy == true)
                    {
                        DataSend?.Invoke(this, new PlcDataSendEventArgs("EmptyboxBuffer_2OrderByMiR", "1"));
                        EmptyboxBufferConveyer_2StandBy = false;
                    }
                    else
                    {
                        DataSend?.Invoke(this, new PlcDataSendEventArgs("EmptyboxBuffer_3OrderByMiR", "1"));
                        EmptyboxBufferConveyer_3StandBy = false;
                    }

                }
                // 로봇위치 P2=1, P1=0, P0=0
                if (p2_robot != null && p1_robot == null && p0_robot == null)
                {
                    MiR_Put_Register(p2_robot, FullBoxPosition_REGISTER_NO, FullBoxPosition_REGISTER_VALUE_P0); // 로봇이동 P2 => P0
                }
                // 로봇위치 P2=1, P1=0, P0=1
                if (p2_robot != null && p1_robot == null && p0_robot != null)
                {
                    MiR_Put_Register(p2_robot, FullBoxPosition_REGISTER_NO, FullBoxPosition_REGISTER_VALUE_P1); // 로봇이동 P2 => P1
                }
                // 로봇위치 P2=1, P1=1, P0=0
                if (p2_robot != null && p1_robot != null && p0_robot == null)
                {
                    MiR_Put_Register(p1_robot, FullBoxPosition_REGISTER_NO, FullBoxPosition_REGISTER_VALUE_P0); // 로봇이동 P1 => P0
                    MiR_Put_Register(p2_robot, FullBoxPosition_REGISTER_NO, FullBoxPosition_REGISTER_VALUE_P1); // 로봇이동 P2 => P1
                }
                // 로봇위치 P2=0, P1=1, P0=0
                if (p2_robot == null && p1_robot != null && p0_robot == null)
                {
                    MiR_Put_Register(p1_robot, FullBoxPosition_REGISTER_NO, FullBoxPosition_REGISTER_VALUE_P0); // 로봇이동 P1 => P0
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        #endregion


        #region 서브 함수 (상태 / GUID / 미션삭제 / 미션전송)
        /* Interfacs 로변경
        private string FindMissionGuid(string sMissionName)//<============Guid 검색
        {
            //해당 Mission Name으로 Guid를 검색하는 루틴
            var missions = main.GetMissions.SingleOrDefault(m => m.name == sMissionName);
            return missions?.guid;
        }

        private bool SendMission(Robot robot, Mission newMission)//<==========미션 전송
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

        private bool DeleteMission(Robot robot, Mission mission)//<===========미션 삭제
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
        */

        #endregion


        #region 레지스터 관련 함수
        /* Interface 로 변경처리
        private bool MiR_Get_Register(Robot robot, int registerNo)
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

        private bool MiR_Put_Register(Robot robot, int registerNo, int registerValue)
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

        private void MiR_ReST_ParsingRegister_ID(Robot robot, int RegisterNo, int registerValue)
        {
            switch (RegisterNo)
            {
                // MiR 자재 유무 판단 [KGCBuyeo]
                case 1: robot.Registers.dMiR_Register_Value1 = registerValue; break;
                case 2: robot.Registers.dMiR_Register_Value2 = registerValue; break;
                case 3: robot.Registers.dMiR_Register_Value3 = registerValue; break;
                case 4: robot.Registers.dMiR_Register_Value4 = registerValue; break;
                case 5: robot.Registers.dMiR_Register_Value5 = registerValue; break;
                case 6: robot.Registers.dMiR_Register_Value6 = registerValue; break;
                case 7: robot.Registers.dMiR_Register_Value7 = registerValue; break;
                case 8: robot.Registers.dMiR_Register_Value8 = registerValue; break;
                case 9: robot.Registers.dMiR_Register_Value9 = registerValue; break;
                case 10: robot.Registers.dMiR_Register_Value10 = registerValue; break;
                case 11: robot.Registers.dMiR_Register_Value11 = registerValue; break;// MiR Top Module 정상유무 (Value = 1 정상신호 / Value = 0 비정상신호)[YKK]
                case 12: robot.Registers.dMiR_Register_Value12 = registerValue; break;// MiR 콘베어 자재 유무 (Value = 1 자재있음 / Value = 0 자재없음)[YKK][현대트랜시스]
                case 13: robot.Registers.dMiR_Register_Value13 = registerValue; break;
                case 14: robot.Registers.dMiR_Register_Value14 = registerValue; break;
                case 15: robot.Registers.dMiR_Register_Value15 = registerValue; break;
                case 16: robot.Registers.dMiR_Register_Value16 = registerValue; break;
                case 17: robot.Registers.dMiR_Register_Value17 = registerValue; break;
                case 18: robot.Registers.dMiR_Register_Value18 = registerValue; break;
                case 19: robot.Registers.dMiR_Register_Value19 = registerValue; break;
                case 20: robot.Registers.dMiR_Register_Value20 = registerValue; break;
                case 21: robot.Registers.dMiR_Register_Value21 = registerValue; break;
                case 22: robot.Registers.dMiR_Register_Value22 = registerValue; break;// 자재 신호[Value = 0(자재 없음) / Value = 1(자재 있음)] [Amkor K5] DoorTime 설정된 그룹만 적용한다
                case 23: robot.Registers.dMiR_Register_Value23 = registerValue; break;// Door 신호[Value = 0(Door 닫힘) / Value = 1(Door 열림)] [Amkor K5] DoorTime 설정된 그룹만 적용한다
                case 24: robot.Registers.dMiR_Register_Value24 = registerValue; break;//PLC ResetButton 사용중
                case 25: robot.Registers.dMiR_Register_Value25 = registerValue; break;// WiseTowerLamp RegisterMode 사용레지스터 [Amkor K5] DoorTime 설정된 그룹만 적용한다 (자재이름 클리어)
                case 26: robot.Registers.dMiR_Register_Value26 = registerValue; break;// WiseTowerLamp RegisterMode 사용레지스터 [Amkor K5] DoorTime 설정된 그룹만 적용한다 (자재이름)
                case 27: robot.Registers.dMiR_Register_Value27 = registerValue; break;// WiseTowerLamp RegisterMode 사용레지스터 [Amkor K5] DoorTime 설정된 그룹만 적용한다 (자재이름)
                case 28: robot.Registers.dMiR_Register_Value28 = registerValue; break;// WiseTowerLamp RegisterMode 사용레지스터 [Amkor K5] DoorTime 설정된 그룹만 적용한다 (자재이름)
                case 29: robot.Registers.dMiR_Register_Value29 = registerValue; break;// WiseTowerLamp RegisterMode 사용레지스터 [Amkor K5] DoorTime 설정된 그룹만 적용한다 (자재이름)
                case 30: robot.Registers.dMiR_Register_Value30 = registerValue; break;//==========Elevator 관련 Register ==========
                case 31: robot.Registers.dMiR_Register_Value31 = registerValue; break;// Elevator 이동 Traffic[Amkor K5]
                case 32: robot.Registers.dMiR_Register_Value32 = registerValue; break;//M3F 트레픽 관련 POS 레지스터 싱크
                case 33: robot.Registers.dMiR_Register_Value33 = registerValue; break;//T3F 트레픽 관련 POS 레지스터 싱크
                case 34: robot.Registers.dMiR_Register_Value34 = registerValue; break;
                case 35: robot.Registers.dMiR_Register_Value35 = registerValue; break;
                case 36: robot.Registers.dMiR_Register_Value36 = registerValue; break;
                case 37: robot.Registers.dMiR_Register_Value37 = registerValue; break;
                case 38: robot.Registers.dMiR_Register_Value38 = registerValue; break;// AMT SUPPLY REQUEST 7 INCH 자재 확인[Amkor K5]
                case 39: robot.Registers.dMiR_Register_Value39 = registerValue; break;// AMT SUPPLY REQUEST 13 INCH 자재 확인[Amkor K5]
                case 40: robot.Registers.dMiR_Register_Value40 = registerValue; break;//Robot Door Open 상태값변경 [Amkor K5] DoorTime 설정된 그룹만 적용한다
                case 41: robot.Registers.dMiR_Register_Sync41 = registerValue; break;
                case 42: robot.Registers.dMiR_Register_Sync42 = registerValue; break;
                case 43: robot.Registers.dMiR_Register_Sync43 = registerValue; break;
                case 44: robot.Registers.dMiR_Register_Sync44 = registerValue; break;
                case 45: robot.Registers.dMiR_Register_Sync45 = registerValue; break;
                case 46: robot.Registers.dMiR_Register_Sync46 = registerValue; break;
                case 47: robot.Registers.dMiR_Register_Sync47 = registerValue; break;
                case 48: robot.Registers.dMiR_Register_Sync48 = registerValue; break;
                case 49: robot.Registers.dMiR_Register_Sync49 = registerValue; break;
                case 50: robot.Registers.dMiR_Register_Sync50 = registerValue; break;

                case 98: robot.Registers.dMiR_Register_Value98 = registerValue; break;

                default: throw new Exception($"{nameof(MiR_ReST_ParsingRegister_ID)} : parsing register not found");
            }
        }
        */
        #endregion

#endif

        #endregion


    }

}
