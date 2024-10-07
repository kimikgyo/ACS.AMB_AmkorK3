using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        private void Upgrade_Elevator_Traffic_Control()
        {
            try
            {
                foreach (var robot in ActiveRobots().Where(r => r.ACSRobotGroup != "AMTWEST" && r.ACSRobotGroup != "AMTEAST").ToList())
                {
                    var Elevator_runmission = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState == "Executing" && m.RobotName == robot.RobotName
                                                 && (m.MissionName.Contains("E/V"))).FirstOrDefault();

                    if (Elevator_runmission != null)
                    {
                        bool elevatorStart = robot.PositionZoneName.Contains("E/VGO");
                        bool elevatorEnter = true;// robot.PositionZoneName.StartsWith("E/V");
                        bool elevatorEnd = true;// robot.PositionZoneName.Contains("ElevatorEnd");

                        var mirStatusElevator = uow.ElevatorState.Load().FirstOrDefault();

                        if (mirStatusElevator == null)
                        {
                            if (elevatorStart)
                            {
                                var jobconfig = uow.JobConfigs.GetAll().FirstOrDefault(x => x.CallName == Elevator_runmission.CallName);

                                ElevatorStateModule stateModule = new ElevatorStateModule();
                                stateModule.RobotAlias = robot.RobotAlias;
                                stateModule.RobotName = robot.RobotName;
                                stateModule.ElevatorRobotState = "Elevator_CALL";
                                stateModule.ElevatorMissionName = Elevator_runmission.MissionName;
                                stateModule.ElevatorSourceFloor = jobconfig.SourceFloor = (string.IsNullOrEmpty(jobconfig.SourceFloor)) ? "0" : jobconfig.SourceFloor;
                                stateModule.ElevatorDestFloor = jobconfig.DestFloor = (string.IsNullOrEmpty(jobconfig.DestFloor)) ? "0" : jobconfig.DestFloor;

                                uow.ElevatorState.Add(stateModule);
                            }
                        }
                        else
                        {
                            int Floor = 0;

                            switch (mirStatusElevator.ElevatorRobotState)
                            {
                                case "Elevator_CALL":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;

                                    if (mirStatusElevator.ElevatorSourceFloor == "B1")
                                        Floor = 0;
                                    else
                                        Floor = Convert.ToInt32(mirStatusElevator.ElevatorSourceFloor);

                                    if (main.ElevatorFloor != mirStatusElevator.ElevatorSourceFloor + "F" && main.ElevatorState == "문열림")
                                    {
                                        main.rS485.FlagWrite = 2;
                                        main.rS485.FlagData = true;
                                    }

                                    if (main.ElevatorFloor != mirStatusElevator.ElevatorSourceFloor + "F" && main.ElevatorState == "문닫힘")
                                    {
                                        main.rS485.Floor = Floor;
                                        main.rS485.FlagWrite = 3;
                                        main.rS485.FlagData = true;
                                    }
                                    
                                    if (main.ElevatorFloor == mirStatusElevator.ElevatorSourceFloor + "F")
                                    {
                                        ElevatorStateModule stateModule = new ElevatorStateModule();
                                        stateModule.Id = mirStatusElevator.Id;
                                        stateModule.ElevatorRobotState = "Elevator_Start";

                                        uow.ElevatorState.ElevatorRobotUpdate(stateModule);
                                    }

                                    break;

                                case "Elevator_Start":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;
                                    if (!MiR_Get_Register(robot, 84)) break;

                                    if (main.ElevatorFloor == mirStatusElevator.ElevatorSourceFloor + "F" && robot.PositionZoneName.EndsWith("E/VGO"))
                                    {
                                        main.rS485.FlagWrite = 1;
                                        main.rS485.FlagData = true;
                                    }

                                    if (main.ElevatorFloor == mirStatusElevator.ElevatorSourceFloor + "F" && main.ElevatorState == "문열림" &&
                                        robot.PositionZoneName.EndsWith("E/VGO"))
                                    {
                                        MiR_Put_Register(robot, 30, 1); // [MiR -> Elevator 진입]

                                        main.rS485.FlagWrite = 1;
                                        main.rS485.FlagData = true;
                                    }

                                    if (main.ElevatorFloor == mirStatusElevator.ElevatorSourceFloor + "F" && main.ElevatorState == "문열림" &&
                                        robot.PositionZoneName.EndsWith("E/V"))
                                    {
                                        ElevatorStateModule stateModule = new ElevatorStateModule();
                                        stateModule.Id = mirStatusElevator.Id;
                                        stateModule.ElevatorRobotState = "Elevator_Enter";

                                        uow.ElevatorState.ElevatorRobotUpdate(stateModule);
                                    }

                                    break;

                                case "Elevator_Enter":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;
                                    if (!MiR_Get_Register(robot, 84)) break;

                                    if (elevatorEnter && robot.Registers.dMiR_Register_Value[84] == 1 && main.ElevatorState == "문열림")
                                    {
                                        main.rS485.FlagWrite = 2;
                                        main.rS485.FlagData = true;
                                    }

                                    if (elevatorEnter && robot.Registers.dMiR_Register_Value[84] == 1 && main.ElevatorState == "문닫힘")
                                    {
                                        MiR_Put_Register(robot, 30, 2); // [진입 후 180도 턴]

                                        ElevatorStateModule stateModule = new ElevatorStateModule();
                                        stateModule.Id = mirStatusElevator.Id;
                                        stateModule.ElevatorRobotState = "Elevator_Move";

                                        uow.ElevatorState.ElevatorRobotUpdate(stateModule);
                                    }

                                    break;

                                case "Elevator_Move":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;

                                    var Job = uow.Jobs.GetAll().FirstOrDefault(x => x.Id == Elevator_runmission.JobId);
                                    if (Job != null)
                                    {
                                        uow.JobConfigs.Validate_DB_Items();
                                        var DestFloor = uow.JobConfigs.GetAll().FirstOrDefault(x => x.CallName == Job.CallName).DestFloor;

                                        if (main.ElevatorFloor != DestFloor + "F" && main.ElevatorState == "문열림")
                                        {
                                            main.rS485.FlagWrite = 2;
                                            main.rS485.FlagData = true;
                                        }

                                        if (main.ElevatorFloor != DestFloor + "F" && main.ElevatorState == "문닫힘")
                                        {
                                            if (DestFloor == "B1")
                                                Floor = 0;
                                            else
                                                Floor = Convert.ToInt32(DestFloor);

                                            main.rS485.Floor = Floor;
                                            main.rS485.FlagWrite = 3;
                                            main.rS485.FlagData = true;
                                        }

                                        if (main.ElevatorFloor == DestFloor + "F")
                                        {
                                            ElevatorStateModule stateModule = new ElevatorStateModule();
                                            stateModule.Id = mirStatusElevator.Id;
                                            stateModule.ElevatorRobotState = "Elevator_Complete";

                                            uow.ElevatorState.ElevatorRobotUpdate(stateModule);
                                        }
                                    }

                                    break;

                                case "Elevator_Complete":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;
                                    if (!MiR_Get_Register(robot, 84)) break;

                                    //설정한 목적지 층 가져오기
                                    var Jobs = uow.Jobs.GetAll().FirstOrDefault(x => x.Id == Elevator_runmission.JobId);
                                    if (Jobs != null)
                                    {
                                        uow.JobConfigs.Validate_DB_Items();
                                        var DestFloor = uow.JobConfigs.GetAll().FirstOrDefault(x => x.CallName == Jobs.CallName).DestFloor;

                                        //엘리베이터 목적지 층 도착
                                        if (elevatorEnter && main.ElevatorFloor == DestFloor + "F" && main.ElevatorState == "문닫힘")
                                        {
                                            main.rS485.FlagWrite = 1;
                                            main.rS485.FlagData = true;
                                        }

                                        if (robot.Registers.dMiR_Register_Value[84] == 1 && main.ElevatorFloor == DestFloor + "F" && main.ElevatorState == "문열림" &&
                                        robot.PositionZoneName.EndsWith("E/V"))
                                        {
                                            MiR_Put_Register(robot, 30, 3); // [Elevator -> MiR 진출신호]

                                            main.rS485.FlagWrite = 1;
                                            main.rS485.FlagData = true;
                                        }

                                        if (main.ElevatorFloor == DestFloor + "F" && main.ElevatorState == "문열림" && !robot.PositionZoneName.EndsWith("E/V"))
                                        {
                                            ElevatorStateModule stateModule = new ElevatorStateModule();
                                            stateModule.Id = mirStatusElevator.Id;
                                            stateModule.ElevatorRobotState = "Elevator_Leave";

                                            uow.ElevatorState.ElevatorRobotUpdate(stateModule);
                                        }
                                    }

                                    break;

                                case "Elevator_Leave":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;
                                    if (!MiR_Get_Register(robot, 84)) break;

                                    if (elevatorEnd && main.ElevatorFloor == mirStatusElevator.ElevatorDestFloor + "F" && robot.Registers.dMiR_Register_Value[84] == 2)
                                    {
                                        MiR_Put_Register(robot, 30, 4); // [Elevator -> MiR 진출완료]

                                        ElevatorStateModule stateModule = new ElevatorStateModule();
                                        stateModule.Id = mirStatusElevator.Id;
                                        stateModule.ElevatorRobotState = "DB_Delete";

                                        uow.ElevatorState.ElevatorRobotUpdate(stateModule);

                                        main.rS485.FlagWrite = 2;
                                        main.rS485.FlagData = true;
                                    }

                                    break;

                                case "DB_Delete":
                                    if (mirStatusElevator.RobotName != robot.RobotName) break;

                                    //엘리베이터 미션이 없을경우 삭제한다.
                                    var Elevator = uow.ElevatorState.Load().FirstOrDefault(m => m.RobotName == robot.RobotName);
                                    if (Elevator != null)
                                    {
                                        uow.ElevatorState.Remove(Elevator);
                                        MiR_Put_Register(robot, 30, 0); // [초기화]
                                    }
                                    
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }
    }
}
