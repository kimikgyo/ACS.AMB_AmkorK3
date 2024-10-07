using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {

        //================================== Amkor K5 M3F_AMT Contorl ===============================================//
        //1.공급방법 / 1호기 2호기 3호기 MiR 1대가 컨트롤(M3FAMTWest) / 4호기 5호기 MiR 1대가 컨트롤(M3FAMTEast)
        //- 초기 시작은 충전기(대기) 위치 -> Reel Pick위치 -> ReelTower도킹
        //- 공급하는 Reel 7인치 13인치 2개 포지션 구별 하여 출발한다
        //- 지정된 위치에서 작업자가 MiR 인터페이스 창에서 Register로 Call요청한다(7인치 13인치)
        //- 작업자 Call시 ReelTower 상태를 받아서 상태가 자재를 처리할수있는 상태 회수차가 없는 상태이면 해당 목적지에 전송
        //- 1)미션구성 미션전송 -> Waitting 포지션 구성 ->와이즈 신호 확인후 도킹
        //  [공급하는 호기를 구별하기 어려움 와이즈 신호로 도킹을 하기때문에]
        //- 2)미션구성 미션전송 -> 해당 Reel호기 레지스터 입력 및 미션을 각자 따로구성 -> 해당Reel호기 도킹
        //  [공급하는 호기를 구별할수 있음]
        //===========================================================================================================//

        private void Upgrade_AMT_JobStartControl_Call()          //<==========미션 상태 Start 변경 (M3F_T3F 같이사용 여부파악하여 삭제예정)
        {

            //=============================================== 미션 상태 Start =================================================//
            //1.Init으로 등록되어 있는 job 을 Start로 상태를 변경한다.
            //=================================================================================================================//

            var jobs = uow.Jobs.Find(x => (x.ACSJobGroup != "None") && (x.ACSJobGroup == "AMT_WEST") && (x.ACSJobGroup == "AMT_EAST") && x.JobState == JobState.JobInit);
            foreach (var job in jobs)
            {
                // ===============================================
                // job 시작 ~~~~~~~~~~~~

                job.JobState = JobState.JobStart;
                uow.Jobs.Update(job);
            }
        }

        private void Upgrade_AMT_JobControl()                    //<========== [Mission추가] 
        {
            //================================== Amkor K5 M3FAMTLoad_JobControl Program 구성===============================================//
            //1.ReelTower 공급 가능 호기를 조회한다        
            //2.해당 Robot에 위치를 확인한다 / 작업자Call 위치 : (M3FAMTWestMain / M3FAMTEastMain)
            //3.ReelTower MissionConfig에서 ReelTower이름을 End조건으로 검색한다.
            //4.출발지가 다르기 때문에 그룹은 (M3FAMT로 통일해도 상관없음)
            //6.레지스터 확인 20번에 Value 1은 7인치 20번에 Value 2 13인치(종류 확인)
            //7.MissionConfig 조회된 미션을 전송한다.(레지스터 출발지 목적지)조회
            //=============================================================================================================================//
            //================================== Amkor K5 M3FAMTLoad_JobControl Program 구성===============================================//
            //1.ReelTower 공급 가능 호기를 조회한다        
            //2.해당 Robot에 위치를 확인한다 / 작업자Call 위치 : (M3FAMTWestMain / M3FAMTEastMain)
            //3.레지스터 확인 20번에 Value 1은 7인치 20번에 Value 2 13인치(종류 확인)
            //=============================================================================================================================//


            // ========== 공급 처리 (REGISTER) ==========
            //
            // 1. 유저가 자재를 적재한 후 대시보드에서 공급시작 버튼을 클릭하면
            // 2. 레지스터 20번에 특정값이 들어 오고 (7인치=1, 13인치=2)
            // 3. ACS는 투입 가능한 릴타워로 가는 공급미션을 전송한다
            //    * WEST/EAST
            //      1 =  7인치 TWR 공급 버튼
            //      2 = 13인치 TWR 공급 버튼
            //
            // ========== 공급 CANCEL 처리 (REGISTER) ========== (공급영역내에서만 cancel가능)
            //
            // 1. 유저가 태블릿 대쉬보드 UI에서 'CANCEL' button click
            // 2. 해당 로봇의 register 21 = 1 로 설정됨
            // 3. acs는 이미 등록된 job 삭제후 cancelJob 등록/실행
            // 4. cancelJob 완료시 해당 로봇의 register 21 = 0 으로 리셋됨 (by robot)


            #region wise module sensor mapping

            // ========== Upgrade 구현
            //var AutoControlwiseModuleFind = uow.WiseModuleConfig.Find(w => w.ModuleUse == "Use" && w.ModuleStatus == "Connect" && w.ModuleControlMode == "AutoControl");
            var AutoControlwiseModuleFind = uow.WiseModuleConfig.Find(w => w.ModuleUse == "Use" && w.ModuleStatus == "Connect");

            bool twr1_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{4}"))?.ModuleStatus == "Connect";
            bool twr2_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{5}"))?.ModuleStatus == "Connect";
            bool twr3_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{6}"))?.ModuleStatus == "Connect";
            bool twr4_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{1}"))?.ModuleStatus == "Connect";
            bool twr5_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{2}"))?.ModuleStatus == "Connect";

            int twr1_input_ready = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName == $"WiseModule#{4}_Ch#{0}")?.ModuleIn_Value ?? 0;        // WEST TWR 1
            int twr2_input_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{5}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();        // WEST TWR 2
            int twr3_input_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{6}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();        // WEST TWR 3
            int twr4_input_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{1}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();        // EAST TWR 4
            int twr5_input_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{2}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();        // EAST TWR 5

            int twr1_out_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{4}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();
            int twr2_out_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{5}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();
            int twr3_out_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{6}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();
            int twr4_out_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{1}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();
            int twr5_out_ready = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{2}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();

            bool east_buffer_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{3}"))?.ModuleStatus == "Connect";
            bool west_buffer_connected = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{7}"))?.ModuleStatus == "Connect"
                                      && AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName.StartsWith($"WiseModule#{8}"))?.ModuleStatus == "Connect";

            int east_supply1_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{3}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // EAST SUPPLY 7
            int east_supply2_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{3}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // EAST SUPPLY 13
            int east_buffer1_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{3}_Ch#{2}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // EAST BUFFER
            int east_buffer2_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{3}_Ch#{3}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // EAST BUFFER

            int west_supply1_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{7}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // WEST SUPPLY 7
            int west_supply2_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{7}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // WEST SUPPLY 13
            int west_buffer1_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{7}_Ch#{2}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // WEST BUFFER
            int west_buffer2_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{7}_Ch#{3}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // WEST BUFFER
            int west_buffer3_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{8}_Ch#{0}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // WEST BUFFER
            int west_buffer4_sensor = AutoControlwiseModuleFind.Where(w => w.ModuleName == $"WiseModule#{8}_Ch#{1}").Select(s => s.ModuleIn_Value).FirstOrDefault();   // WEST BUFFER


            // ========== DEBUG wise info display
            //var sb = new StringBuilder();
            //for (int i = 0; i < ConfigData.wiseModuleMax; i++)
            //{
            //    sb.Append($"wiseModuleInput[{i}] = ");
            //    for (int j = 0; j < 4; j++) sb.Append(wiseModuleData[i].DigitalIn[j] + " ");
            //    if (i == 0) sb.Append("tower1");
            //    if (i == 1) sb.Append("tower2");
            //    if (i == 2) sb.Append("buffer");
            //    sb.AppendLine();
            //}
            //Console.WriteLine(sb.ToString());

            var sb = new StringBuilder();
            for (int i = 0; i < ConfigData.WiseModule_MaxNum; i++)
            {
                sb.Append($"wiseModuleInput[{i}] = ");
                for (int j = 0; j < 4; j++)
                {
                    var wz = AutoControlwiseModuleFind.FirstOrDefault(w => w.ModuleName == $"WiseModule#{i + 1}_Ch#{j}");
                    if (wz != null)
                        sb.Append(wz.ModuleIn_Value + " ");
                    else
                        sb.Append("." + " ");
                }
                if (i == 0) sb.Append("tower1");
                if (i == 1) sb.Append("tower2");
                if (i == 2) sb.Append("buffer");
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());

            #endregion


            try
            {

                // RHS 회수미션 우선 처리한다
                DBCall_RequestControl();
                Process_JobCommandQueue();



                foreach (var robot in ActiveRobots().Where(x => x.ACSRobotGroup == "AMTWEST" || x.ACSRobotGroup == "AMTEAST").ToList())
                {
                    //cancel job이 있는 경우 이 로봇은 스킵
                    if (IsCancelJobExist(robot))
                        continue;


                    #region 로봇에 job이 할당되어 있나 없나에 따라 처리 (테스트하지 않음)
                    ////로봇에 할당된 job을 찾는다
                    ////var runMission_Job = uow.Jobs.GetById(robot.JobId);               //로봇에 할당된 jobid로 job을 찾는다. (로봇에 전송한 job만 확인가능하다)
                    //var runMission_Job = uow.Jobs.GetByCreateRobot(robot.RobotName);  //job 생성시에 지정한 로봇네임으로 job을 찾는다. (로봇 전송여부는 무관하다)

                    ////로봇에 job이 할당되지 않은 경우
                    //if (runMission_Job == null)
                    //{
                    //    // 로봇이 현재 어느 포지션 영역에 있는지 확인한다 (포지션 영역은 DB에 정의된다)
                    //    var robotPositionArea = CheckRobotPositionArea(robot);
                    //    if (robotPositionArea == null)
                    //        continue; //로봇이 영역밖이면 skip

                    //    // 로봇의 현재 포지션 영역 이름 (JOB CONFIG 이름의 앞부분)
                    //    robotPosAreaName = robotPositionArea.PositionAreaName;

                    //    // 투입 가능한 ReelTower 있는지 확인
                    //    var loadReadyReelTower = EquipmentStatus.GetEqpPortStatus().FirstOrDefault(x => x.PORT_ACCESS == 1 && x.PORT_STATUS == 0); // 릴타워 투입가능조건
                    //    if (loadReadyReelTower == null)
                    //        continue;

                    //    // 7인치 공급 처리
                    //    if (robot.Registers.dMiR_Register_Value20 == 1)
                    //    {
                    //        string startPos = $"{robotPosAreaName}7";
                    //        string endPos = loadReadyReelTower.EQP_NAME;

                    //        var missionConfig = uow.MissionConfigs.AMTMissionSelect(robot, startPos, endPos);
                    //        if (missionConfig != null)
                    //        {
                    //            if (uow.Jobs.GetByCallButtonName(missionConfig.CallButtonName) == null) // 중복 추가 방지
                    //                JobCommandQueueAdd(missionConfig, robot);
                    //        }
                    //    }
                    //    // 13인치 공급 처리
                    //    else if (robot.Registers.dMiR_Register_Value20 == 2)
                    //    {
                    //        string startPos = $"{robotPosAreaName}13";
                    //        string endPos = loadReadyReelTower.EQP_NAME;

                    //        var missionConfig = uow.MissionConfigs.AMTMissionSelect(robot, startPos, endPos);
                    //        if (missionConfig != null)
                    //        {
                    //            if (uow.Jobs.GetByCallButtonName(missionConfig.CallButtonName) == null) // 중복 추가 방지
                    //                JobCommandQueueAdd(missionConfig, robot);
                    //        }
                    //    }
                    //}
                    ////로봇에 job이 할당되어 있는 경우, cancel요청 확인(register체크)하여 처리한다
                    //else
                    //{
                    //    if (runMission_Job.CallButtonName.ToUpper().Contains("JOBCANCEL") == false) //cancel 요청이 이미 실행중인 경우는 제외
                    //        AMT_jobCancelControl(robot);
                    //}
                    #endregion



                    // 로봇그룹별 콜네임 앞부분
                    string callNamePrefix = "";
                    string Linefloor = "";

                    //================================== Upgrade===============================================//
                    //1.Position Config 에서 X1,X2,X3,X4 Y1,Y2,Y3,Y4 Position설정후 위치값은 0으로 Setting 한다
                    //2.Position Config 에서 저장될시 앞에 {층 + Name}으로 저장이 된다.
                    //3.JobConfig 에서 출발지 와 목적지 설정한다
                    //4. "_"는 CallName에서 출발지와 목적지 나누기 위하여 사용되기때문에 "_"사용하지않는다.
                    //=========================================================================================//
                    
                    //로봇 MapId를 이용하여 층을 찾는다
                    var LineFloor = uow.FloorMapIDConfig.Find(f => f.MapID == robot.MapID).FirstOrDefault();
                    if(LineFloor != null) Linefloor = LineFloor.FloorName;

                    if (robot.ACSRobotGroup == "AMTEAST") callNamePrefix = $"{Linefloor}AMTEast";
                    else if (robot.ACSRobotGroup == "AMTWEST") callNamePrefix = $"{Linefloor}AMTWest";
                    //// 레지스터 값 읽기
                    //if (!MiR_Get_Register(robot, 38)) continue; // 레지스터38 SUPPLY REQUEST  7 INCH
                    //if (!MiR_Get_Register(robot, 39)) continue; // 레지스터39 SUPPLY REQUEST 13 INCH

                    // 레지스터 값 읽기(2023.06.08레지스터 수정)
                    if (!MiR_Get_Register(robot, 32)) continue; // 레지스터32 SUPPLY REQUEST  7 INCH
                    if (!MiR_Get_Register(robot, 33)) continue; // 레지스터33 SUPPLY REQUEST 13 INCH

                    // ===== 0. RHS 회수 요청부터 위에서 먼저 처리한다


                    // ===== 1. 공급 CANCEL 처리 (로봇이 공급영역안에 있을때만 cancel 처리한다)
                    if (GetAMTSupplyArea(robot) != null) // cancel 가능한 영역인가?
                    {
                        // ===== Robot Position Updata
                        robot.PositionZoneName = GetAMTSupplyArea(robot).PositionAreaName;
                        uow.Robots.Update(robot);

                        //string cancelCallName = $"{callNamePrefix}JobCancel";

                        if (IsCancelJobExist(robot))
                        //if (uow.Jobs.GetByCallButtonName(cancelCallName) == null) // cancel job이 없는가?
                        {
                            // cancel 요청 있으면 처리
                            if (!MiR_Get_Register(robot, 21)) continue; // 레지스터21 SUPPLY CANCEL


                            //2023.06.08 수정
                            //- 첫번째 미션인것을 확인해서 jobCancel 시키는 구현(그룹에 일치하는 Cancel 미션을 검색한다.)
                            var JobCancelmission = uow.JobConfigs.Find(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup && m.jobCancelSignal == Convert.ToString(robot.Registers.dMiR_Register_Value21)
                                                                       && (m.CallName.Contains("Cancel") || m.CallName.Contains("cancel"))).FirstOrDefault();

                             if(JobCancelmission !=null)
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


                            ////기존
                            //if (robot.Registers.dMiR_Register_Value21 == 1)
                            //{
                            //    // 현재 영역의 job 을 모두 찾아서 삭제한다
                            //    var jobsToDelete = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup)
                            //                               .Where(j => !string.IsNullOrEmpty(callNamePrefix) && j.CallName.StartsWith(callNamePrefix)).ToList();

                            //    foreach (var j in jobsToDelete)
                            //        JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = j.CallName });

                            //    // cancel job 추가한다
                            //    JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = cancelCallName });

                            //    // queue 처리되도록 이하는 skip
                            //    continue;
                            //}
                        }
                    }


                    // ===== 2. 공급 요청 처리 (7inch)
                    if (robot.Registers.dMiR_Register_Value32 == 1) // 7inch supply
                    {
                        // check job
                        if (!IsJobExist(robot))
                        {
                            // 투입가능 릴타워 확인
                            var loadReadyTower = GetLoadReadyTower(robot.ACSRobotGroup);
                            if (loadReadyTower != null)
                            {
                                string startPosName = $"{callNamePrefix}Buffer7";
                                string endPosName = $"{Linefloor}{loadReadyTower.EQP_NAME}";
                                AddJobToQueue(robot, startPosName, endPosName);
                                continue; // queue 처리되도록 이하는 skip
                            }
                        }
                    }


                    // ===== 3. 공급 요청 처리 (13inch)
                    if (robot.Registers.dMiR_Register_Value33 == 1) // 13inch supply
                    {
                        // check job
                        if (!IsJobExist(robot))
                        {
                            // 투입가능 릴타워 확인
                            var loadReadyTower = GetLoadReadyTower(robot.ACSRobotGroup);
                            if (loadReadyTower != null)
                            {
                                string startPosName = $"{callNamePrefix}Buffer13";
                                string endPosName = $"{Linefloor}{loadReadyTower.EQP_NAME}";
                                AddJobToQueue(robot, startPosName, endPosName);
                                continue; // queue 처리되도록 이하는 skip
                            }
                        }
                    }


                    // ===== 4. 버퍼 자동이송 처리 (공급포지션이 비어있을때, 버퍼포지션에서 공급포지션으로 빈 카트를 이송시킨다)
                    if (!IsJobExist(robot))
                    {
                        if (robot.ACSRobotGroup == "AMTEAST")
                        {
                            // EAST 7인치 버퍼 자동이송 처리
                            if (east_supply1_sensor == 0 && east_buffer1_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                //string startPosName = $"AMTEast_Buffer7";
                                //string endPosName = "User7";
                                string startPosName = $"{Linefloor}AMTEastBuffer7";
                                string endPosName = $"{Linefloor}User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // EAST 13인치 버퍼 자동이송 처리
                            else if (east_supply2_sensor == 0 && east_buffer2_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                //string startPosName = $"AMTEast_Buffer13";
                                //string endPosName = "User13"; 
                                string startPosName = $"{Linefloor}AMTEastBuffer13";
                                string endPosName = $"{Linefloor}User13";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                        }
                        else if (robot.ACSRobotGroup == "AMTWEST")
                        {
                            // 이부분은 WEST 셋업전까지 사용안함
                            return;

                            // WEST 7인치 버퍼 #1 자동이송 처리
                            if (west_supply1_sensor == 0 && west_buffer1_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                //string startPosName = $"AMTWest_Buffer7A";
                                //string endPosName = "User7";
                                string startPosName = $"{Linefloor}AMTWestBuffer7A";
                                string endPosName = $"{Linefloor}User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // WEST 7인치 버퍼 #2 자동이송 처리
                            else if (west_supply1_sensor == 0 && west_buffer2_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                //string startPosName = $"AMTWest_Buffer7B";
                                //string endPosName = "User7";
                                string startPosName = $"{Linefloor}AMTWestBuffer7B";
                                string endPosName = $"{Linefloor}User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // WEST 7인치 버퍼 #3 자동이송 처리
                            else if (west_supply2_sensor == 0 && west_buffer3_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                //string startPosName = $"AMTWest_Buffer7C";
                                //string endPosName = "User7";
                                string startPosName = $"{Linefloor}AMTWestBuffer7C";
                                string endPosName = $"{Linefloor}User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // WEST 13인치 버퍼 #1 자동이송 처리
                            else if (west_supply2_sensor == 0 && west_buffer4_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                //string startPosName = $"AMTWest_Buffer13";
                                //string endPosName = "User13";
                                string startPosName = $"{Linefloor}AMTWest_Buffer13";
                                string endPosName = $"{Linefloor}User13";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }

            return;


            // ===== 로컬 함수 =====


            bool AddJobToQueue(Robot robot, string startPosName, string endPosName)
            {
                var jobConfig = uow.JobConfigs.AMTMissionSelect_Without_Register(robot, startPosName, endPosName);
                if (jobConfig != null)
                {
                    if (uow.Jobs.GetByCallName(jobConfig.CallName) == null) // 중복 추가 방지
                        JobCommandQueueAdd(jobConfig, robot);
                }
                return true;
            }

            EquipmentStatus GetLoadReadyTower(string groupName)
            {
                EquipmentStatus loadReadyTower = null;
                if (false)
                {
                    // 투입할 릴타워 확인 (각 릴타워의 DB 상태정보 확인해서 비어있는 릴타워 선택)
                    loadReadyTower = GetTowers(groupName).FirstOrDefault(x => x.PORT_ACCESS == 1 && x.PORT_STATUS == 0); // 릴타워 투입가능조건
                }
                else
                {
                    // 투입할 릴타워 확인 (각 릴타워 와이즈 센서상태 확인해서 비어있는 릴타워 선택)
                    foreach (var twr in GetTowers(groupName))
                    {
                        if (twr.EQP_NAME == "TWR1" && twr1_connected && twr1_input_ready == 1) { loadReadyTower = twr; break; }
                        else if (twr.EQP_NAME == "TWR2" && twr2_connected && twr2_input_ready == 1) { loadReadyTower = twr; break; }
                        else if (twr.EQP_NAME == "TWR3" && twr3_connected && twr3_input_ready == 1) { loadReadyTower = twr; break; }
                        else if (twr.EQP_NAME == "TWR4" && twr4_connected && twr4_input_ready == 1) { loadReadyTower = twr; break; }
                        else if (twr.EQP_NAME == "TWR5" && twr5_connected && twr5_input_ready == 1) { loadReadyTower = twr; break; }
                    }
                }
                return loadReadyTower;
            }

            PositionAreaConfig GetAMTSupplyArea(Robot robot)
            {
                return uow.PositionAreaConfigs.UpGrade_AllPositionArea(robot)
                                                        .FirstOrDefault(m => m.PositionAreaName == "M3FAMTWestSupply" || m.PositionAreaName == "M3FAMTEastSupply");
            }

            bool IsCancelJobExist(Robot robot) // 이 로봇에 cancel job이 있나?
            {
                // job 리스트에서 특정 그룹의 cancel job 을 찾는다
                var foundJobs = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup && j.CallName != null
                                                && j.CallName.ToUpper().Contains("JOBCANCEL"));
                return foundJobs.Count > 0;
            }

            bool IsJobExist(Robot robot) // 이 로봇에 할당된 job이 있나?
            {
                var foundJobs = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup
                                                && j.JobCreateRobotName == robot.RobotName);
                return foundJobs.Count > 0;
            }

            IList<EquipmentStatus> GetTowers(string groupName)
            {
                // 특정 그룹의 tower name list
                List<string> towerNames = EquipmentCallInfoRepo.GetAll().Where(x => x.GROUP_NAME == groupName).Select(x => x.EQP_NAME).Distinct().ToList();
                // tower info list
                List<EquipmentStatus> towers = EquipmentStatus.GetEqpPortStatus().Where(x => towerNames.Contains(x.EQP_NAME)).ToList();
                return towers;
            }

        }

        private void Upgrade_AMT_jobCancelControl(Robot robot)   //<========== [JobCancel 제어] 
        {

            // 사용하지 않는다!
            return;

            // =========== Area 구간에서만 Job 취소가 가능하다 ==========

            //try
            {
                double mir_pos_x = robot.Position_X;
                double mir_pos_y = robot.Position_Y;

                //로봇 위치 PositionArea 에서 M3FAMTWestSupply or M3FAMTEastSupply 위치에 있는지 확인한다.
                //var PositionArea = uow.PositionAreaConfigs.UpGrade_AllPositionArea(robot)
                //                                        .FirstOrDefault(m => m.PositionAreaName == "M3FAMTWestSupply" || m.PositionAreaName == "M3FAMTEastSupply");

                bool PositionArea = robot.PositionZoneName == "M3FAMTWestSupply" || robot.PositionZoneName == "M3FAMTEastSupply";

                //jobConfig 설정에서 그룹이 일치한Cancel 미션이 있는지 확인한다.
                var JobCancelmission = uow.JobConfigs.Find(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup
                                                           && (m.CallName.Contains("Cancel") || m.CallName.Contains("cancel"))).FirstOrDefault();

                //if (PositionArea != null && JobCancelmission != null)
                if (PositionArea && JobCancelmission != null)
                {
                    MiR_Get_Register(robot, 21); //Cancel Mission 확인

                    if (robot.Registers.dMiR_Register_Value21 == 1)
                    {
                        // mir mission queue 비운다
                        if (DeleteMission(robot, null))
                        {
                            // 해당 robot에 연관된 job 삭제
                            var job = uow.Jobs.GetById(robot.JobId);
                            if (job != null)
                            {
                                Job_DeleteJobData(job);
                                JobCommandQueueAdd(JobCancelmission, robot);
                            }
                        }
                    }
                }
            }
            //catch (Exception ex)
            //{
            //    LogExceptionMessage(ex);
            //}
        }

        private void Upgrade_AMT_AutoWaiting_Mission()           //<========== [자동 대기 처리] (M3F_T3F 같이사용 여부파악하여 삭제예정) 
        {
#if AMT_TEST
            return;
#endif
            try
            {
                double waitingPosX1 = 38.8;
                double waitingPosY1 = 89.1;
                double waitingPosX2 = 40.4;
                double waitingPosY2 = 90.8;

                foreach (var robot in ActiveRobots().Where(r => r.ACSRobotGroup == "AMTWEST" || r.ACSRobotGroup == "AMTEAST").ToList())
                {

                    // 이 로봇에 보낸 미션들
                    var runMission = uow.Missions
                                        .Find(m => m.Robot == robot && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
                                        .FirstOrDefault();//.SingleOrDefault();

                    // 이 로봇에 보낸 특수미션들
                    var runMission_Specials = uow.Missions
                                        .Find(m => m.Robot == robot && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
                                        .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid") // 완료 미션은 제외한다
                                        .Where(m => m.JobId == 0) // 특수미션들
                                        .ToList();

                    //로봇 위치 Area를 확인한다.
                    //var robotPosition = uow.PositionAreaConfigs.UpGrade_AllPositionArea(robot).FirstOrDefault();

                    //로봇 Wait미션이 설정되어있는지 확인한다
                    //var WaitMission = uow.WaitMissionConfig.Find(w => robotPosition == null && w.WaitMissionUse == "Use"
                    //                                            && w.RobotName == robot.RobotName && w.EnableBattery >= robot.BatteryPercent).FirstOrDefault();

                    var WaitMission = uow.WaitMissionConfig.Find(w => w.WaitMissionUse == "Use"
                                                                && w.RobotName == robot.RobotName && w.EnableBattery >= robot.BatteryPercent).FirstOrDefault();

                    bool c1 = robot.StateID == RobotState.Ready
                             && runMission == null
                             && WaitMission != null
                             && string.IsNullOrEmpty(robot.PositionZoneName);
                    if (c1)
                    {
                        if (DeleteMission(robot, null))
                        {
                            // 이 로봇과 관련된 특수미션이 있는 경우 모두 지워준다 (job미션은 제외하고)
                            foreach (var m in runMission_Specials)
                            {
                                uow.Missions.Remove(m);
                            }
                            SendMission(robot, new Mission
                            {
                                ACSMissionGroup = robot.ACSRobotGroup,
                                CallName = $"None_{WaitMission.PositionZone}",
                                MissionName = WaitMission.WaitMissionName
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage(ex);
            }
        }

        private void Upgrade_AMT_AutoCharging_Mission()          //<========== [자동 대기 처리] (M3F_T3F 같이사용 여부파악하여 삭제예정) 
        {
            //=============================================== 자동 충전 미션 =================================================//

            //1. Robot이 기본 조건 이고 레디 상태
            //2. Robot이 PositionAreaConfigs 설정된 포지션에 있어야함
            //3. 설정된 포지션과 시작 포지션Zone일치한 충전 미션을 전송함.
            //4. WaitingConfig 에서 목적지 별로 Robot을 setting해야함.

            //충전기 카운터 수량이 없을시
            //5.충전중인 Robot중 배터리 용량이 큰 Robot 먼저 삭제
            //6.해당 Robot 미션을 전송한다

            //충전기 카운터 수량이 있을시 
            //5.바로 미션을 전송한다.
            //=================================================================================================================//

#if AMT_TEST
            return;
#endif

            //try
            {
                foreach (var robot in ActiveRobots().Where(r => r.ACSRobotGroup == "AMTWEST" || r.ACSRobotGroup == "AMTEAST").ToList())
                {
                    ChargeMissionConfigModel ChargingMissionSelect = null;

                    //충전기 수량 확인
                    var chargerCount = uow.ACSChargerCountConfig.Find(c => c.ChargerCountUse == "Use"/*&& c.RobotGroupName == robot.ACSRobotGroup && c.FloorMapId == robot.MapID*/).FirstOrDefault();


                    // 해당 Robot에 이미 전송한 미션을 검색한다 (대기 및 충전 보내는 미션을 확인)
                    var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0 && m.MissionState != "Done").FirstOrDefault();

                    //충전 미션이 실행중인지 확인한다.
                    var runchargingmission = uow.ChargeMissionConfig.Find(c => c.ChargeMissionUse == "Use" && c.RobotName == robot.RobotName && c.EndBattery <= robot.BatteryPercent).FirstOrDefault();

                    // 충전 완료 삭제
                    bool c1 = runMission != null
                        && runchargingmission != null
                        && chargerCount != null
                        && runchargingmission.ChargeMissionName == runMission.MissionName;

                    // 충전 미션 전송
                    bool c2 = robot.StateID == RobotState.Ready
                        && runMission == null
                        && runchargingmission == null
                        && chargerCount != null;

                    if (c1)
                    {
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
                        ChangeMssionSend(robot, chargerCount, ChargingMissionSelect);
                    }
                }
            }
            //catch (Exception ex)
            {
                //LogExceptionMessage(ex);
            }

            // ===== 로컬 함수 =====
            //충전 미션 삭제
            void ChangeMissionDelete(Robot robot, ACSChargerCountConfigModel chargerCount)
            {
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

                    if (chargerCount.ChargerCountStatus > 0)
                    {
                        chargerCount.ChargerCountStatus = chargerCount.ChargerCountStatus - 1;
                        uow.ACSChargerCountConfig.Update(chargerCount);
                    }
                }

            }

            //충전 미션 전송
            void ChangeMssionSend(Robot robot, ACSChargerCountConfigModel chargerCount, ChargeMissionConfigModel chargeMission)
            {
                //충전기 설정수량보다 같거나 충전기 설정수량카운터보다 크면 충전을 시작해야하는 Robot보다 배터리가 큰 Robot 1대를 삭제후 전송한다
                if (chargerCount.ChargerCount <= chargerCount.ChargerCountStatus)
                {
                    // 충전 미션을 진행중인 List 를 확인한다  ChargeingConfig List를와 비교한다
                    var runChargeingMissions = uow.Missions.Find(m => m.JobId == 0 && m.ReturnID > 0 && m.MissionState != "Done"
                                              && uow.ChargeMissionConfig.GetAll().Count(c => m.MissionName == c.ChargeMissionName && m.RobotName == c.RobotName) != 0).ToList();

                    //충전 미션진행중인 로봇중에 충전이 필요한 Robot 그룹,MapId 일치이고 로봇이름은다른것중에 배터리가 가장높은것
                    var runChargeingRobots = ActiveRobots().Where(r => r.ACSRobotGroup == robot.ACSRobotGroup
                                                  && r.MapID == robot.MapID && r.RobotName != robot.RobotName
                                                  && runChargeingMissions.Count(c => r.RobotName == c.RobotName) != 0).OrderBy(x => x.BatteryPercent > robot.BatteryPercent).FirstOrDefault();

                    if (runChargeingRobots != null)
                    {
                        ChangeMissionDelete(runChargeingRobots, chargerCount);
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
        }
    }
}
