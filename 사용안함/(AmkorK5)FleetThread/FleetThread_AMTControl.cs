using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        private void AMT_JobStartControl_Call()          //<==========미션 상태 Start 변경
        {
            //=============================================== 미션 상태 Start =================================================//
            //1.Init으로 등록되어 있는 job 을 Start로 상태를 변경한다.
            //=================================================================================================================//

            var jobs = uow.Jobs.Find(x => x.ACSJobGroup == "AMT_WEST" || x.ACSJobGroup == "AMT_EAST");
            foreach (var job in jobs)
            {
                // ===============================================
                // job 시작 ~~~~~~~~~~~~
                if (job.JobState == JobState.JobInit)
                {
                    job.JobState = JobState.JobStart;
                    uow.Jobs.Update(job);
                }
            }
        }


        private void AMT_JobControl()
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

            bool twr1_connected = false;
            bool twr2_connected = false;
            bool twr3_connected = false;
            bool twr4_connected = false;
            bool twr5_connected = false;

            int twr1_input_ready = 0;
            int twr2_input_ready = 0;
            int twr3_input_ready = 0;
            int twr4_input_ready = 0;
            int twr5_input_ready = 0;

            int twr1_out_ready = 0;
            int twr2_out_ready = 0;
            int twr3_out_ready = 0;
            int twr4_out_ready = 0;
            int twr5_out_ready = 0;

            bool east_buffer_connected = false;
            bool west_buffer_connected = false;

            int east_supply1_sensor = 0;
            int east_supply2_sensor = 0;
            int east_buffer1_sensor = 0;
            int east_buffer2_sensor = 0;

            int west_supply1_sensor = 0;
            int west_supply2_sensor = 0;
            int west_buffer1_sensor = 0;
            int west_buffer2_sensor = 0;
            int west_buffer3_sensor = 0;
            int west_buffer4_sensor = 0;

            //bool twr1_connected = wiseModuleData[3].ConnectionState;
            //bool twr2_connected = wiseModuleData[4].ConnectionState;
            //bool twr3_connected = wiseModuleData[5].ConnectionState;
            //bool twr4_connected = wiseModuleData[0].ConnectionState;
            //bool twr5_connected = wiseModuleData[1].ConnectionState;

            //int twr1_input_ready = wiseModuleData[3].DigitalIn[0];        // WEST TWR 1
            //int twr2_input_ready = wiseModuleData[4].DigitalIn[0];        // WEST TWR 2
            //int twr3_input_ready = wiseModuleData[5].DigitalIn[0];        // WEST TWR 3
            //int twr4_input_ready = wiseModuleData[0].DigitalIn[0];        // EAST TWR 4
            //int twr5_input_ready = wiseModuleData[1].DigitalIn[0];        // EAST TWR 5

            //int twr1_out_ready = wiseModuleData[3].DigitalIn[1];
            //int twr2_out_ready = wiseModuleData[4].DigitalIn[1];
            //int twr3_out_ready = wiseModuleData[5].DigitalIn[1];
            //int twr4_out_ready = wiseModuleData[0].DigitalIn[1];
            //int twr5_out_ready = wiseModuleData[1].DigitalIn[1];

            //bool east_buffer_connected = wiseModuleData[2].ConnectionState;
            //bool west_buffer_connected = wiseModuleData[6].ConnectionState && wiseModuleData[7].ConnectionState;

            //int east_supply1_sensor = wiseModuleData[2].DigitalIn[0];   // EAST SUPPLY 7
            //int east_supply2_sensor = wiseModuleData[2].DigitalIn[1];   // EAST SUPPLY 13
            //int east_buffer1_sensor = wiseModuleData[2].DigitalIn[2];   // EAST BUFFER
            //int east_buffer2_sensor = wiseModuleData[2].DigitalIn[3];   // EAST BUFFER

            //int west_supply1_sensor = wiseModuleData[6].DigitalIn[0];   // WEST SUPPLY 7
            //int west_supply2_sensor = wiseModuleData[6].DigitalIn[1];   // WEST SUPPLY 13
            //int west_buffer1_sensor = wiseModuleData[6].DigitalIn[2];   // WEST BUFFER
            //int west_buffer2_sensor = wiseModuleData[6].DigitalIn[3];   // WEST BUFFER
            //int west_buffer3_sensor = wiseModuleData[7].DigitalIn[0];   // WEST BUFFER
            //int west_buffer4_sensor = wiseModuleData[7].DigitalIn[1];   // WEST BUFFER

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

            #endregion


            try
            {

                // RHS 회수미션 우선 처리한다
                DBCall_RequestControl();
                Process_JobCommandQueue();


                var amtRobots = uow.Robots.GetAll().Where(x => x.ACSRobotGroup == "AMT_WEST" || x.ACSRobotGroup == "AMT_EAST").ToList();

                foreach (var robot in amtRobots)
                {
                    //로봇 전원이 꺼져있으면 skip
                    if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
                        continue;
                    //로봇네임,로봇상태정보가 없다면 skip
                    if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
                        continue;
                    //로봇이 active 가 아니면 skip
                    if (!robot.ACSRobotActive)
                        continue;
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
                    if (robot.ACSRobotGroup == "AMT_EAST") callNamePrefix = "AMTEast";
                    else if (robot.ACSRobotGroup == "AMT_WEST") callNamePrefix = "AMTWest";


                    // 레지스터 값 읽기
                    if (!MiR_Get_Register(robot, 38)) continue; // 레지스터38 SUPPLY REQUEST  7 INCH
                    if (!MiR_Get_Register(robot, 39)) continue; // 레지스터39 SUPPLY REQUEST 13 INCH


                    // ===== 0. RHS 회수 요청부터 위에서 먼저 처리한다


                    // ===== 1. 공급 CANCEL 처리 (로봇이 공급영역안에 있을때만 cancel 처리한다)
                    if (GetAMTSupplyArea(robot) != null) // cancel 가능한 영역인가?
                    {
                        string cancelCallName = $"{callNamePrefix}_JobCancel";

                        if (IsCancelJobExist(robot))
                        //if (uow.Jobs.GetByCallButtonName(cancelCallName) == null) // cancel job이 없는가?
                        {
                            // cancel 요청 있으면 처리
                            if (!MiR_Get_Register(robot, 21)) continue; // 레지스터21 SUPPLY CANCEL
                            if (robot.Registers.dMiR_Register_Value21 == 1)
                            {
                                // 현재 영역의 job 을 모두 찾아서 삭제한다
                                var jobsToDelete = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup)
                                                           .Where(j => j.CallName.StartsWith(callNamePrefix)).ToList();

                                foreach (var j in jobsToDelete)
                                    JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = j.CallName });

                                // cancel job 추가한다
                                JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = cancelCallName });

                                // queue 처리되도록 이하는 skip
                                continue;
                            }
                        }
                    }


                    // ===== 2. 공급 요청 처리 (7inch)
                    if (robot.Registers.dMiR_Register_Value38 == 1) // 7inch supply
                    {
                        // check job
                        if (!IsJobExist(robot))
                        {
                            // 투입가능 릴타워 확인
                            var loadReadyTower = GetLoadReadyTower(robot.ACSRobotGroup);
                            if (loadReadyTower != null)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"{callNamePrefix}_Buffer7";
                                string endPosName = loadReadyTower.EQP_NAME;
                                AddJobToQueue(robot, startPosName, endPosName);
                                continue; // queue 처리되도록 이하는 skip
                            }
                        }
                    }


                    // ===== 3. 공급 요청 처리 (13inch)
                    if (robot.Registers.dMiR_Register_Value39 == 1) // 13inch supply
                    {
                        // check job
                        if (!IsJobExist(robot))
                        {
                            // 투입가능 릴타워 확인
                            var loadReadyTower = GetLoadReadyTower(robot.ACSRobotGroup);
                            if (loadReadyTower != null)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"{callNamePrefix}_Buffer13";
                                string endPosName = loadReadyTower.EQP_NAME;
                                AddJobToQueue(robot, startPosName, endPosName);
                                continue; // queue 처리되도록 이하는 skip
                            }
                        }
                    }


                    // ===== 4. 버퍼 자동이송 처리 (공급포지션이 비어있을때, 버퍼포지션에서 공급포지션으로 빈 카트를 이송시킨다)
                    if (!IsJobExist(robot))
                    {
                        if (robot.ACSRobotGroup == "AMT_EAST")
                        {
                            // EAST 7인치 버퍼 자동이송 처리
                            if (east_supply1_sensor == 0 && east_buffer1_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"AMTEast_Buffer7";
                                string endPosName = "User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // EAST 13인치 버퍼 자동이송 처리
                            else if (east_supply2_sensor == 0 && east_buffer2_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"AMTEast_Buffer13";
                                string endPosName = "User13";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                        }
                        else if (robot.ACSRobotGroup == "AMT_WEST")
                        {
                            // 이부분은 WEST 셋업전까지 사용안함
                            return;

                            // WEST 7인치 버퍼 #1 자동이송 처리
                            if (west_supply1_sensor == 0 && west_buffer1_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"AMTWest_Buffer7A";
                                string endPosName = "User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // WEST 7인치 버퍼 #2 자동이송 처리
                            else if (west_supply1_sensor == 0 && west_buffer2_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"AMTWest_Buffer7B";
                                string endPosName = "User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // WEST 7인치 버퍼 #3 자동이송 처리
                            else if (west_supply2_sensor == 0 && west_buffer3_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"AMTWest_Buffer7C";
                                string endPosName = "User7";
                                AddJobToQueue(robot, startPosName, endPosName);
                            }
                            // WEST 13인치 버퍼 #1 자동이송 처리
                            else if (west_supply2_sensor == 0 && west_buffer4_sensor == 1)
                            {
                                // 출발지와 목적지로 mission config 찾아서 job 등록한다
                                string startPosName = $"AMTWest_Buffer13";
                                string endPosName = "User13";
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
                var missionConfig = uow.JobConfigs.AMTMissionSelect_Without_Register(robot, startPosName, endPosName);
                if (missionConfig != null)
                {
                    if (uow.Jobs.GetByCallName(missionConfig.CallName) == null) // 중복 추가 방지
                        JobCommandQueueAdd(missionConfig, robot);
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
                return uow.PositionAreaConfigs.NotElevatorPositionArea(robot.Position_X, robot.Position_Y)
                                                        .FirstOrDefault(m => m.PositionAreaName == "AMTWestSupply" || m.PositionAreaName == "AMTEastSupply");
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


        private void AMT_jobCancelControl(Robot robot)    //<==========JobCancel 제어
        {

            // 사용하지 않는다!
            return;

            // =========== Area 구간에서만 Job 취소가 가능하다 ==========

            //try
            {
                double mir_pos_x = robot.Position_X;
                double mir_pos_y = robot.Position_Y;

                var PositionArea = uow.PositionAreaConfigs.NotElevatorPositionArea(mir_pos_x, mir_pos_y)
                                                        .FirstOrDefault(m => m.PositionAreaName == "AMTWestSupply" || m.PositionAreaName == "AMTEastSupply");

                var JobCancelmission = uow.JobConfigs.Find(m => m.JobConfigUse == "Use" && m.ACSMissionGroup == robot.ACSRobotGroup
                                                           && (m.CallName.Contains("Cancel") || m.CallName.Contains("cancel"))).FirstOrDefault();

                if (PositionArea != null && JobCancelmission != null)
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


        //        private void AMT_AutoWaiting_Mission()
        //        {
        //#if AMT_TEST
        //            return;
        //#endif
        //            try
        //            {
        //                double waitingPosX1 = 38.8;
        //                double waitingPosY1 = 89.1;
        //                double waitingPosX2 = 40.4;
        //                double waitingPosY2 = 90.8;

        //                foreach (var robot in uow.Robots.GetAll().Where(r => r.ACSRobotGroup == "AMT_WEST" || r.ACSRobotGroup == "AMT_EAST"))
        //                {

        //                    string waitingMissionName = "";
        //                    if (robot.ACSRobotGroup == "AMT_WEST") waitingMissionName = ConfigData.sAutoWaitingMissionAMTWest;
        //                    else if (robot.ACSRobotGroup == "AMT_EAST") waitingMissionName = ConfigData.sAutoWaitingMissionAMTEast;
        //                    else continue;

        //                    if (ConfigData.sAutoWaitingUse == "Use" && waitingMissionName != "" && waitingMissionName != "None")
        //                    {
        //                        //MiR 전원이 꺼져있을경우
        //                        if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
        //                            continue;
        //                        //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
        //                        if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
        //                            continue;


        //                        // 이 로봇에 보낸 미션들
        //                        var runMission = uow.Missions
        //                                            .Find(m => m.Robot == robot && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
        //                                            .FirstOrDefault();//.SingleOrDefault();

        //                        // 이 로봇에 보낸 특수미션들
        //                        var runMission_Specials = uow.Missions
        //                                            .Find(m => m.Robot == robot && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
        //                                            .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid") // 완료 미션은 제외한다
        //                                            .Where(m => m.JobId == 0) // 특수미션들
        //                                            .ToList();

        //                        var mir_pos_x = robot.Position_X;
        //                        var mir_pos_y = robot.Position_Y;

        //                        // mir상태가 Ready 이고 실행중인 미션이 없고 mir 배터리가 미션가능 배터리보다 크거나 같아야 한다.
        //                        bool c1 = robot.ACSRobotActive == true //미션 전송 사용(Active == true) 미사용(Active = false)
        //                               && robot.StateID == RobotState.Ready
        //                               && robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeMissionEnablePercent)
        //                               && runMission == null;

        //                        // ConfigData 설정된 자동대기위치 좌표값안에 mir 이 없는경우에만 미션을 전달할수있다. (좌표값안에 있으면 대기위치에 있는경우이기때문에 미션을 보낼필요없음)
        //                        bool c2 = mir_pos_x > waitingPosX1 && mir_pos_x < waitingPosX2
        //                               && mir_pos_y > waitingPosY1 && mir_pos_y < waitingPosY2;


        //                        if (c1 && !c2)
        //                        {
        //                            //MiR_Get_Register(robot, 20);//작업자 Call
        //                            //if (robot.Registers.dMiR_Register_Value20 == 0)
        //                            {
        //                                if (DeleteMission(robot, null))
        //                                {
        //                                    // 이 로봇과 관련된 특수미션이 있는 경우 모두 지워준다 (job미션은 제외하고)
        //                                    foreach (var m in runMission_Specials)
        //                                    {
        //                                        uow.Missions.Remove(m);
        //                                    }

        //                                    SendMission(robot, new Mission { MissionName = waitingMissionName });
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LogExceptionMessage(ex);
        //            }
        //        }


        //        private void AMT_AutoCharging_Mission()
        //        {
        //#if AMT_TEST
        //            return;
        //#endif
        //            try
        //            {
        //                foreach (var robot in uow.Robots.GetAll().Where(r => r.ACSRobotGroup == "AMT_WEST" || r.ACSRobotGroup == "AMT_EAST"))
        //                {

        //                    string ChargingMissionName = "";
        //                    string waitingMissionName = "";
        //                    if (robot.ACSRobotGroup == "AMT_WEST")
        //                    {
        //                        ChargingMissionName = ConfigData.sAutoChargeMissionAMTWest;
        //                        waitingMissionName = ConfigData.sAutoWaitingMissionAMTWest;
        //                    }
        //                    else if (robot.ACSRobotGroup == "AMT_EAST")
        //                    {
        //                        ChargingMissionName = ConfigData.sAutoChargeMissionAMTEast;
        //                        waitingMissionName = ConfigData.sAutoWaitingMissionAMTEast;
        //                    }
        //                    else continue;


        //                    //if (ConfigData.sAutoChargeUse == "Use" && ChargingMissionName != "" && ChargingMissionName != "None")
        //                    //{
        //                    //    //MiR 전원이 꺼져있을경우
        //                    //    if (robot.Fleet_State == FleetState.None || robot.Fleet_State == FleetState.unavailable)
        //                    //        continue;
        //                    //    //로봇그룹이 설정이 안되어 있거나 로봇상태값과 이름이없는경우
        //                    //    if (string.IsNullOrWhiteSpace(robot.RobotName) || string.IsNullOrWhiteSpace(robot.StateText))
        //                    //        continue;


        //                    //    var runMission = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0 && m.MissionState != "Done").SingleOrDefault(); // 해당 mir에 이미 전송한 미션을 검색한다


        //                    //    // (조건1) mir이 충전 미션 진행중이고 mir배터리가 충전 완료 배터리와 크거나 같을경우 
        //                    //    bool c1 = (robot.BatteryPercent >= double.Parse(ConfigData.sAutoChargeEndPercent))
        //                    //            && runMission != null
        //                    //            && runMission.ReturnID > 0
        //                    //            && runMission.MissionName == ChargingMissionName
        //                    //            && runMission.MissionState == "Executing";

        //                    //    bool c2 = robot.ACSRobotActive == true //미션 전송 사용(Active == true) 미사용(Active = false)
        //                    //           && robot.BatteryPercent <= double.Parse(ConfigData.sAutoChargeStartPercent)
        //                    //           && robot.StateID == RobotState.Ready
        //                    //           && runMission == null;


        //                    //    // 충전이 완료되었으면 충전미션을 삭제한다
        //                    //    if (c1)
        //                    //    {
        //                    //        // 충전미션을 삭제한다
        //                    //        DeleteMission(robot, null);

        //                    //        // 이 로봇과 관련된 특수미션이 있는 경우 모두 지워준다 (job미션은 제외하고)
        //                    //        var runMission_Specials = uow.Missions.Find(m => m.Robot == robot && m.ReturnID > 0) // 해당 mir에 이미 전송한 미션을 검색한다
        //                    //                                      .Where(m => m.MissionState != "Done" && m.MissionState != "Invalid")
        //                    //                                      .Where(m => m.JobId == 0); // 특수미션들

        //                    //        foreach (var m in runMission_Specials)
        //                    //        {
        //                    //            uow.Missions.Remove(m);
        //                    //        }

        //                    //        // 대기위치로 보낸다
        //                    //        SendMission(robot, new Mission { MissionName = waitingMissionName });

        //                    //    }
        //                    //    // 충전이 필요하면 충전미션을 실행한다
        //                    //    else if (c2)
        //                    //    {
        //                    //        //MiR_Get_Register(robot, 20);
        //                    //        //if (robot.Registers.dMiR_Register_Value20 == 0) // 0=자재없음 && TopModuleAuto = 1(Auto)
        //                    //        {
        //                    //            if (DeleteMission(robot, null))
        //                    //                SendMission(robot, new Mission { MissionName = ChargingMissionName });
        //                    //        }
        //                    //    }
        //                    //}

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LogExceptionMessage(ex);
        //            }
        //        }

    }
}





// ===== ===== ===== ===== ===== ===== ===== ===== 
// *que
//    supply : register(1), sensor(1) => add
//    supply : register(1), sensor(0) => ignore
//    return : db (notfound_in_joblist) => add
// ===== ===== ===== ===== ===== ===== ===== ===== 
// *
////var returnCalls = DBCall_GetOrdersFromDB()
////var supplyCall1 = (robot.Registers.dMiR_Register_Value38 == 1)
////var supplyCall2 = (robot.Registers.dMiR_Register_Value39 == 1)
// ===== ===== ===== ===== ===== ===== ===== ===== 
// *sche
//var returnCalls = uow.Jobs.Where(x=>x.Type == ReturnType)
//var supplyCalls = uow.Jobs.Where(x=>x.Type == SupplyType)
//
//if returnCalls.Count>=2 and supplyCalls.Count>0
//    process_returnCall
//
//else if returnCalls.Count>0 and supplyCalls.Count>0
//    if robot is upperZone
//        process_returnCall
//    else
//        process_supplyCall
//
//else if returnCalls.Count>0
//    process_returnCall
//
//else if supplyCalls.Count>0
//    process_supplyCall
// ===== ===== ===== ===== ===== ===== ===== ===== 
