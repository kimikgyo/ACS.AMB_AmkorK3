using System;
using System.Collections.Generic;
using System.Linq;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        private void MissionControl()
        {
            var MissionsSpecifics = uow.MissionsSpecific.GetAll(0).FirstOrDefault(x => x.RobotName == null && x.CallState == "wait" && x.Cancel != "cancel" && 
                                                                                 (string.IsNullOrEmpty(x.Move_CallName) || x.Move_CallName == "RobotCall"));

            if (MissionsSpecifics != null)
            {
                //차량들 비교해서 가장 가까운 차량 찾기
                //string RobotName = CloseRobot(MissionsSpecifics.CallName);


                string RobotName = RobotClose(MissionsSpecifics.CallName);

                if (!string.IsNullOrEmpty(RobotName) && RobotName != "RobotCall")
                {

                    CallNameJobAdd(MissionsSpecifics.CallName, RobotName);

                    //JobCommand command = new JobCommand();
                    //command.Code = JobCommandCode.ADD;
                    //command.Extra1 = 0;
                    //command.Extra2 = null;
                    //command.Extra3 = 0;
                    //command.Extra4 = RobotName;
                    //command.Extra5 = 0;
                    //command.Text = MissionsSpecifics.CallName;

                    //JobCommandQueue.Enqueue(command);

                    var Robot = uow.Robots.GetAll().FirstOrDefault(x => x.RobotName == RobotName);

                    // MissionsSpecific missionsSpecific = new MissionsSpecific();
                    // missionsSpecific.No = MissionsSpecifics.No;
                    //missionsSpecific.RobotName = RobotName;
                    //missionsSpecific.RobotAlias = Robot.RobotAlias;
                    //missionsSpecific.CallState = Robot.StateText;

                    //uow.MissionsSpecific.Update(missionsSpecific);

                    MissionsSpecifics.RobotName = RobotName;
                    MissionsSpecifics.RobotAlias = Robot.RobotAlias;
                    MissionsSpecifics.CallState = Robot.StateText;
                    uow.MissionsSpecific.Update(MissionsSpecifics);


                }
                else if (RobotName == "RobotCall" && MissionsSpecifics.Move_CallName != "RobotCall")
                {
                    //다른 층에 있는 차량 부르는 로직
                    var Robots = uow.Robots.GetAll().Where(x => x.StateID == RobotState.Ready || (x.JobId == 0 && x.StateID == RobotState.Executing && x.MissionText.StartsWith("Charging")));
                    var Floors = uow.FloorMapIDConfigs.GetAll();
                    var Positions = uow.PositionAreaConfigs.GetAll();

                    //출발지, 목적지 위치 찾기
                    RobotName = DestFloorRobotNotFind(Robots, Floors, Positions);
                    if (!string.IsNullOrEmpty(RobotName))
                    {
                        var MapID = Robots.FirstOrDefault(x => x.RobotName == RobotName).MapID;
                        string SourceFloor = Floors.FirstOrDefault(x => x.MapID == MapID).FloorName;//차량이 있는 층
                        var DestFloor = Positions.FirstOrDefault(x => x.PositionAreaName == MissionsSpecifics.CallName.Split('_')[0]).PositionAreaFloorName;//가야 하는 목적지 층

                        var Jobconfig = uow.JobConfigs.GetAll().FirstOrDefault(x => x.CallName.Split('_')[0].Contains("E/VGO") && x.SourceFloor == SourceFloor && x.DestFloor == DestFloor);

                        //기존 데이터 수정
                        {
                            MissionsSpecific missionsSpecific = new MissionsSpecific();
                            missionsSpecific.No = MissionsSpecifics.No;
                            missionsSpecific.Move_CallName = Jobconfig.CallName;

                            uow.MissionsSpecific.MoveCallName_Update(missionsSpecific);
                        }

                        //다른 차량에 부르는 미션 추가(찾은 데이터로 DB 생성)
                        {
                            MissionsSpecific missionsSpecific = new MissionsSpecific();
                            missionsSpecific.CallName = Jobconfig.CallName;
                            missionsSpecific.CallState = "wait";
                            missionsSpecific.CallTime = DateTime.Now;
                            missionsSpecific.Priority = 0;
                            missionsSpecific.JobSection = "ACS";

                            uow.MissionsSpecific.Add(missionsSpecific);
                        }
                    }
                }
            }
            else
            {

                var Missions = uow.MissionsSpecific.GetAll(0).Where(x => x.Cancel == "cancel");

                foreach (var Mission in Missions)
                {
                    //MissionsSpecific missionsSpecific = new MissionsSpecific();
                    //missionsSpecific.No = Mission.No;

                    uow.MissionsSpecific.Remove(Mission);
                }
            }
        }

        #region 가까운 차량 알고리즘(좌표)
        private string RobotClose(string CallName)
        {
            var Robots = uow.Robots.GetAll().Where(x => x.StateID == RobotState.Ready || (x.JobId == 0 && x.StateID == RobotState.Executing && x.MissionText.StartsWith("Charging")));
            var Floors = uow.FloorMapIDConfigs.GetAll();
            var Positions = uow.PositionAreaConfigs.GetAll();

            if (Robots.Count() != 0)
            {
                //1. 목적지 범위 층 찾기
                var DestFloor = Positions.FirstOrDefault(x => x.PositionAreaName == CallName.Split('_')[0]);

                if (DestFloor != null)
                {
                    //2. 목적지 범위 층에 대기중인 차량 찾기
                    string RobotName = DestFloorRobotFind(Robots, DestFloor);

                    if (!string.IsNullOrEmpty(RobotName))
                        return RobotName;//찾은 로봇 리턴
                    else
                    {
                        //3. 목적지 범위 층에 대기중인 차량이 없을경우
                        return "RobotCall";
                        /*//안씀
                        RobotName = DestFloorRobotNotFind(Robots, Floors, Positions);
                        if (!string.IsNullOrEmpty(RobotName))
                            return RobotName;
                        */
                    }
                }
                else
                {
                    main.ACS_UI_Log("RobotClose() - 층을 찾을 수 없습니다.");
                    return "";
                }
            }

            return "";
        }

        private string DestFloorRobotFind(IEnumerable<Robot> Robots, PositionAreaConfig DestFloor)
        {
            Robots = Robots.Where(x => x.MapID == DestFloor.PositionAreaFloorMapId);

            string robotName = "";
            double BeforeCalc = Double.MaxValue;
            foreach (var robot in Robots)
            {
                double result_X = robot.Position_X - ((Convert.ToDouble(DestFloor.PositionAreaX1) + Convert.ToDouble(DestFloor.PositionAreaX3)) / 2);
                double result_Y = robot.Position_Y - ((Convert.ToDouble(DestFloor.PositionAreaY1) + Convert.ToDouble(DestFloor.PositionAreaY3)) / 2);
                double Calc = Math.Abs(result_X + result_Y);

                if (BeforeCalc > Calc)
                {
                    BeforeCalc = Calc;
                    robotName = robot.RobotName;
                }
            }

            if (string.IsNullOrEmpty(robotName))
                return "";//그 층에서 로봇을 찾지 못함
            else
                return robotName;// 찾은 로봇 리턴
        }

        private string DestFloorRobotNotFind(IEnumerable<Robot> Robots, IList<FloorMapIdConfigModel> Floors, IList<PositionAreaConfig> DestFloor)
        {
            //층별 대기중인 차량이 많은 곳 산출
            IEnumerable<Robot> FloorRobot = null;
            FloorMapIdConfigModel floor = null;
            int beforeRobotCount = -1;
            foreach (var Floor in Floors)
            {
                var robots = Robots.Where(x => x.MapID == Floor.MapID);

                if (beforeRobotCount < robots.Count())
                {
                    FloorRobot = robots;
                    floor = Floor;
                    beforeRobotCount = robots.Count();
                }
            }

            if (FloorRobot != null)
            {
                Robots = FloorRobot.Where(x => x.MapID == floor.MapID);

                PositionAreaConfig destfloor = DestFloor.FirstOrDefault(x => x.PositionAreaFloorName == floor.FloorName && x.PositionAreaName.Contains("E/VGO"));

                if (destfloor != null)
                {
                    string robotName = "";
                    double BeforeCalc = Double.MaxValue;
                    foreach (var robot in Robots)
                    {
                        double result_X = robot.Position_X - ((Convert.ToDouble(destfloor.PositionAreaX1) + Convert.ToDouble(destfloor.PositionAreaX3)) / 2);
                        double result_Y = robot.Position_Y - ((Convert.ToDouble(destfloor.PositionAreaY1) + Convert.ToDouble(destfloor.PositionAreaY3)) / 2);
                        double Calc = Math.Abs(result_X + result_Y);

                        if (BeforeCalc > Calc)
                        {
                            BeforeCalc = Calc;
                            robotName = robot.RobotName;
                        }
                    }

                    if (string.IsNullOrEmpty(robotName))
                        return "";//그 층에서 로봇을 찾지 못함
                    else
                        return robotName;// 찾은 로봇 리턴
                }
                else
                {
                    return "";
                }
            }

            return "";
        }
        #endregion

        #region 가까운 차량 알고리즘(범위+좌표)
        private string CloseRobot(string CallName)
        {
            var Robots = uow.Robots.GetAll().Where(x => x.StateID == RobotState.Ready);
            var Floors = uow.FloorMapIDConfigs.GetAll();
            var Positions = uow.PositionAreaConfigs.GetAll();

            if (Robots.Count() != 0)
            {
                //로봇 범위 정하기
                RobotsScope(Robots, Floors);

                //목적지 범위 정하기
                DestScope(Floors, Positions);

                //가까운 차량 찾기 알고리즘
                //1. 목적지 범위 층 찾기
                var DestFloor = Positions.FirstOrDefault(x => x.PositionAreaName == CallName.Split('_')[0]);

                if (DestFloor != null)
                {
                    //2. 목적지 범위 층에 대기중인 차량 찾기
                    var compareScopes = SetScope(Robots, Floors, DestFloor);

                    if (compareScopes.Count != 0)
                    {
                        //3. 목적지 범위 층에 대기중인 차량이 있는 경우 (경로탐색)
                        return PathExplore(compareScopes);
                    }
                    else
                    {
                        //3. 목적지 범위 층에 대기중인 차량이 없는 경우 (경로탐색)
                        var comparescopes = SetScope(Robots, Floors, Positions);
                        return PathExplore(comparescopes);
                    }
                }
                else
                {
                    main.ACS_UI_Log("CloseRobot() - 층을 찾을 수 없습니다.");
                    return "";
                }
            }
            else
            {
                //충전중 혹은 대기중인 차량 검색

                return "";
            }
        }

        private List<CompareScope> SetScope(IEnumerable<Robot> Robots, IList<FloorMapIdConfigModel> Floors, PositionAreaConfig DestFloor)
        {
            List<CompareScope> compareScopes = new List<CompareScope>();
            compareScopes.Clear();

            var Floor = Floors.FirstOrDefault(x => x.FloorName == DestFloor.PositionAreaFloorName);
            Robots = Robots.Where(x => x.MapID == Floor.MapID);

            if (Robots != null)
            {
                foreach (var robot in Robots)
                {
                    //3. 목적지 범위와 차량 범위를 가지고 오기
                    PathFloor floorKey = new PathFloor();
                    if (Floor.FloorName == "B1")
                        floorKey = PathFloor.지하1층;
                    else if (Floor.FloorName == "1F")
                        floorKey = PathFloor.지상1층;
                    else if (Floor.FloorName == "2F")
                        floorKey = PathFloor.지상2층;

                    //source, dest 비교할 값 구하기
                    CompareScope scope = new CompareScope();
                    scope.RobotName = robot.RobotName;
                    scope.PathFloor = floorKey;

                    //source 값 구하기
                    var RobotsScopes = RobotScopes.TryGetValue(floorKey, out List<RobotScope> Robotvalue);
                    if (RobotsScopes)
                    {
                        foreach (var robotscope in Robotvalue)
                        {
                            if (robotscope.RobotName == robot.RobotName)
                            {
                                scope.SourceScope = robotscope.ScopeName;
                                scope.Source_X = robot.Position_X;
                                scope.Source_Y = robot.Position_Y;
                                break;
                            }
                        }
                    }
                    else
                    {
                        main.ACS_UI_Log("SetScope() - 층을 찾을 수 없습니다.");
                    }

                    //dest 값 구하기
                    var DestesScopes = DestScopes.TryGetValue(floorKey, out List<DestScope> Destvalue);
                    if (DestesScopes)
                    {
                        foreach (var destscope in Destvalue)
                        {
                            if (destscope.PositionAreaName == DestFloor.PositionAreaName)
                            {
                                scope.DestScope = destscope.ScopeName;
                                scope.Dest_X = (Convert.ToDouble(DestFloor.PositionAreaX1) + Convert.ToDouble(DestFloor.PositionAreaX3)) / 2;
                                scope.Dest_Y = (Convert.ToDouble(DestFloor.PositionAreaY1) + Convert.ToDouble(DestFloor.PositionAreaY3)) / 2;
                                break;
                            }
                        }
                    }
                    else
                    {
                        main.ACS_UI_Log("SetScope() - 층을 찾을 수 없습니다.");
                    }

                    compareScopes.Add(scope);
                }

                return compareScopes;
            }
            else
            {
                main.ACS_UI_Log("SetScope() - 대기중인 차량이 없습니다.");
            }

            return compareScopes;
        }

        private List<CompareScope> SetScope(IEnumerable<Robot> Robots, IList<FloorMapIdConfigModel> Floors, IList<PositionAreaConfig> DestFloor)
        {
            List<CompareScope> compareScopes = new List<CompareScope>();
            compareScopes.Clear();

            //층별 대기중인 차량이 많은 곳 산출
            IEnumerable<Robot> FloorRobot = null;
            FloorMapIdConfigModel floor = null;
            int beforeRobotCount = -1;
            foreach (var Floor in Floors)
            {
                var robots = Robots.Where(x => x.MapID == Floor.MapID);

                if (beforeRobotCount < robots.Count())
                {
                    FloorRobot = robots;
                    floor = Floor;
                    beforeRobotCount = robots.Count();
                }
            }

            if (FloorRobot != null)
            {
                foreach (var robot in FloorRobot)
                {
                    //3. 목적지 범위와 차량 범위를 가지고 오기
                    PathFloor floorKey = new PathFloor();
                    if (floor.FloorName == "B1")
                        floorKey = PathFloor.지하1층;
                    else if (floor.FloorName == "1F")
                        floorKey = PathFloor.지상1층;
                    else if (floor.FloorName == "2F")
                        floorKey = PathFloor.지상2층;

                    //source, dest 비교할 값 구하기
                    CompareScope scope = new CompareScope();
                    scope.RobotName = robot.RobotName;
                    scope.PathFloor = floorKey;

                    //source 값 구하기
                    var RobotsScopes = RobotScopes.TryGetValue(floorKey, out List<RobotScope> Robotvalue);
                    if (RobotsScopes)
                    {
                        foreach (var robotscope in Robotvalue)
                        {
                            if (robotscope.RobotName == robot.RobotName)
                            {
                                scope.SourceScope = robotscope.ScopeName;
                                scope.Source_X = robot.Position_X;
                                scope.Source_Y = robot.Position_Y;
                                break;
                            }
                        }
                    }
                    else
                    {
                        main.ACS_UI_Log("SetScope() - 층을 찾을 수 없습니다.");
                    }

                    //dest 값 구하기
                    PositionAreaConfig destfloor = DestFloor.FirstOrDefault(x => x.PositionAreaFloorName == floor.FloorName && x.PositionAreaName.Contains("Elevator"));
                    var DestesScopes = DestScopes.TryGetValue(floorKey, out List<DestScope> Destvalue);
                    if (DestesScopes)
                    {
                        foreach (var destscope in Destvalue)
                        {
                            if (destscope.PositionAreaName == destfloor.PositionAreaName)
                            {
                                scope.DestScope = destscope.ScopeName;
                                scope.Dest_X = (Convert.ToDouble(destfloor.PositionAreaX1) + Convert.ToDouble(destfloor.PositionAreaX3)) / 2;
                                scope.Dest_Y = (Convert.ToDouble(destfloor.PositionAreaY1) + Convert.ToDouble(destfloor.PositionAreaY3)) / 2;
                                break;
                            }
                        }
                    }
                    else
                    {
                        main.ACS_UI_Log("SetScope() - 층을 찾을 수 없습니다.");
                    }

                    compareScopes.Add(scope);
                }

                return compareScopes;
            }
            else
            {
                main.ACS_UI_Log("SetScope() - 대기중인 차량이 없습니다");
            }

            return compareScopes;
        }

        private string PathExplore(List<CompareScope> compareScopes)
        {
            //가장 가까운 범위 차량들(같은 범위에 있는것 포함)
            List<ColseRobotList> CloseRobots = new List<ColseRobotList>();
            CloseRobots.Clear();

            //차량들 비교해서 가장 가까운 차량 찾기
            foreach (var robot in compareScopes)
            {
                var Pathresult = PathDatas.TryGetValue(robot.PathFloor, out List<Paths> Pathsvalue);

                if (Pathresult)
                {
                    if (!string.IsNullOrEmpty(robot.SourceScope) && !string.IsNullOrEmpty(robot.DestScope))
                    {
                        //로봇범위에서 출발 가능한 경로 산출
                        var SourcePaths = Pathsvalue.Where(x => x.path.StartsWith(robot.SourceScope));
                        //위에서 산출값에 목적지 찾기
                        var Path = SourcePaths.FirstOrDefault(x => x.path.EndsWith(robot.DestScope));

                        ColseRobotList list = new ColseRobotList();
                        list.RobotName = robot.RobotName;
                        list.path = Path.path;
                        list.pathCount = Path.path.Split('-').Count();
                        list.Source_X = robot.Source_X;
                        list.Source_Y = robot.Source_Y;
                        list.Dest_X = robot.Dest_X;
                        list.Dest_Y = robot.Dest_Y;

                        var PathCoordresult = PathCoordinateDatas.TryGetValue(robot.PathFloor, out List<PathCoordinate> PathCoorValue);
                        if (PathCoordresult)
                        {
                            if (Path.path.Split('-').Count() == 0 || Path.path.Split('-').Count() == 1)
                            {
                                list.Nearest_X = robot.Dest_X;
                                list.Nearest_Y = robot.Dest_Y;
                            }
                            else
                            {
                                var path = PathCoorValue.FirstOrDefault(x => x.ScopeName == Path.path.Split('-')[1]);

                                list.Nearest_X = robot.Dest_X;
                                list.Nearest_Y = robot.Dest_Y;

                                //list.Nearest_X = path.Dot_X;
                                //list.Nearest_Y = path.Dot_Y;
                            }
                        }
                        else
                        {
                            main.ACS_UI_Log("PathExplore() - 해당 층이 없습니다.");
                        }

                        CloseRobots.Add(list);
                    }
                    else
                    {
                        main.ACS_UI_Log("PathExplore() - 범위를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    main.ACS_UI_Log("PathExplore() - 해당 층이 없습니다.");
                }
            }

            //가장 가까운 범위 차량들 중 좌표계산으로 차량 하나 산출
            var MinCount = CloseRobots.Min(x => x.pathCount);
            var closerobots = CloseRobots.Where(x => x.pathCount == MinCount);

            if (closerobots.Count() == 0)
                main.ACS_UI_Log("PathExplore() - 차량이 존재하지 않습니다.");
            else if (closerobots.Count() == 1)
                return closerobots.FirstOrDefault(x => x.RobotName != "").RobotName;
            else
            {
                //closerobots 리스트 값 2개 이상
                string robotName = "";
                double BeforeCalc = Double.MaxValue;
                foreach (var closerobot in closerobots)
                {
                    //차량들 좌표값 비교
                    if (closerobot.pathCount == 1)
                    {
                        double result_X = closerobot.Source_X - closerobot.Dest_X;
                        double result_Y = closerobot.Source_Y - closerobot.Dest_Y;
                        double Calc = Math.Abs(result_X + result_Y);

                        if (BeforeCalc > Calc)
                        {
                            BeforeCalc = Calc;
                            robotName = closerobot.RobotName;
                        }
                    }
                    else
                    {
                        double result_X = closerobot.Source_X - closerobot.Nearest_X;
                        double result_Y = closerobot.Source_Y - closerobot.Nearest_Y;
                        double Calc = Math.Abs(result_X + result_Y);

                        if (BeforeCalc > Calc)
                        {
                            BeforeCalc = Calc;
                            robotName = closerobot.RobotName;
                        }
                    }
                }

                return robotName;
            }

            return "";
        }

        Dictionary<PathFloor, List<RobotScope>> RobotScopes = new Dictionary<PathFloor, List<RobotScope>>();
        List<RobotScope> robotScopes = new List<RobotScope>();
        /// <summary>
        /// 대기중인 차량 범위 지정
        /// </summary>
        /// <param name="Robots"></param>
        /// <param name="Floors"></param>
        private void RobotsScope(IEnumerable<Robot> Robots, IList<FloorMapIdConfigModel> Floors)
        {
            robotScopes.Clear();

            foreach (var Robot in Robots)
            {
                // 대기중인 로봇 범위 위치값 저장
                var Floor = Floors.FirstOrDefault(x => x.MapID == Robot.MapID);

                if (Floor.FloorName == "B1")
                {
                    //범위 비교하여 값 가져오기
                    var Datas = PathCoordinateDatas.FirstOrDefault(x => x.Key == PathFloor.지하1층);

                    if (Datas.Value != null)
                    {
                        string ScopeName = "";
                        foreach (var Data in Datas.Value)
                        {
                            if (Robot.Position_X >= Data.x1 &&
                                Robot.Position_X < Data.x3 &&
                                Robot.Position_Y >= Data.Y1 &&
                                Robot.Position_Y < Data.Y3)
                            {
                                ScopeName = Data.ScopeName;
                            }
                        }

                        RobotScope robotScope = new RobotScope();
                        robotScope.RobotName = Robot.RobotName;
                        robotScope.MapID = Floor.MapID;
                        robotScope.ScopeName = ScopeName;

                        robotScopes.Add(robotScope);
                        if (!RobotScopes.ContainsKey(PathFloor.지하1층))
                            RobotScopes.Add(PathFloor.지하1층, robotScopes);
                    }
                }
                else if (Floor.FloorName == "1F")
                {
                    //범위 비교하여 값 가져오기
                    var Datas = PathCoordinateDatas.FirstOrDefault(x => x.Key == PathFloor.지상1층);

                    if (Datas.Value != null)
                    {
                        string ScopeName = "";
                        foreach (var Data in Datas.Value)
                        {
                            if (Robot.Position_X >= Data.x1 &&
                                Robot.Position_X < Data.x3 &&
                                Robot.Position_Y >= Data.Y1 &&
                                Robot.Position_Y < Data.Y3)
                            {
                                ScopeName = Data.ScopeName;
                            }
                        }

                        RobotScope robotScope = new RobotScope();
                        robotScope.RobotName = Robot.RobotName;
                        robotScope.MapID = Floor.MapID;
                        robotScope.ScopeName = ScopeName;

                        robotScopes.Add(robotScope);
                        if (!RobotScopes.ContainsKey(PathFloor.지상1층))
                            RobotScopes.Add(PathFloor.지상1층, robotScopes);
                    }
                }
                else if (Floor.FloorName == "2F")
                {
                    //범위 비교하여 값 가져오기
                    var Datas = PathCoordinateDatas.FirstOrDefault(x => x.Key == PathFloor.지상2층);

                    if (Datas.Value != null)
                    {
                        string ScopeName = "";
                        foreach (var Data in Datas.Value)
                        {
                            if (Robot.Position_X >= Data.x1 &&
                                Robot.Position_X < Data.x3 &&
                                Robot.Position_Y >= Data.Y1 &&
                                Robot.Position_Y < Data.Y3)
                            {
                                ScopeName = Data.ScopeName;
                            }
                        }

                        RobotScope robotScope = new RobotScope();
                        robotScope.RobotName = Robot.RobotName;
                        robotScope.MapID = Floor.MapID;
                        robotScope.ScopeName = ScopeName;

                        robotScopes.Add(robotScope);
                        if (!RobotScopes.ContainsKey(PathFloor.지상2층))
                            RobotScopes.Add(PathFloor.지상2층, robotScopes);
                    }
                }
            }
        }

        Dictionary<PathFloor, List<DestScope>> DestScopes = new Dictionary<PathFloor, List<DestScope>>();
        List<DestScope> destScopes = new List<DestScope>();
        /// <summary>
        /// 각 목적지별 범위 지정 함수
        /// </summary>
        private void DestScope(IList<FloorMapIdConfigModel> Floors, IList<PositionAreaConfig> Positions)
        {
            DestScopes.Clear();
            destScopes.Clear();

            foreach (var Position in Positions)
            {
                var Floor = Floors.FirstOrDefault(x => x.MapID == Position.PositionAreaFloorMapId);

                if (Floor.FloorName == "B1")
                {
                    //범위 비교하여 값 가져오기
                    var Datas = PathCoordinateDatas.FirstOrDefault(x => x.Key == PathFloor.지하1층);

                    if (Datas.Value != null)
                    {
                        string ScopeName = "";
                        foreach (var Data in Datas.Value)
                        {
                            float x1 = float.Parse(Position.PositionAreaX1);
                            float x3 = float.Parse(Position.PositionAreaX3);
                            float y1 = float.Parse(Position.PositionAreaY1);
                            float y3 = float.Parse(Position.PositionAreaY3);

                            if (x1 >= Data.x1 &&
                                x3 < Data.x3 &&
                                y1 >= Data.Y1 &&
                                y3 < Data.Y3)
                            {
                                ScopeName = Data.ScopeName;
                            }
                        }

                        DestScope DestScope = new DestScope();
                        DestScope.PositionAreaName = Position.PositionAreaName;
                        DestScope.MapID = Floor.MapID;
                        DestScope.ScopeName = ScopeName;

                        destScopes.Add(DestScope);
                        if (!DestScopes.ContainsKey(PathFloor.지하1층))
                            DestScopes.Add(PathFloor.지하1층, destScopes);
                    }
                }
                else if (Floor.FloorName == "1F")
                {
                    //범위 비교하여 값 가져오기
                    var Datas = PathCoordinateDatas.FirstOrDefault(x => x.Key == PathFloor.지상1층);

                    if (Datas.Value != null)
                    {
                        string ScopeName = "";
                        foreach (var Data in Datas.Value)
                        {
                            float x1 = float.Parse(Position.PositionAreaX1);
                            float x3 = float.Parse(Position.PositionAreaX3);
                            float y1 = float.Parse(Position.PositionAreaY1);
                            float y3 = float.Parse(Position.PositionAreaY3);

                            if (x1 >= Data.x1 &&
                                x3 < Data.x3 &&
                                y1 >= Data.Y1 &&
                                y3 < Data.Y3)
                            {
                                ScopeName = Data.ScopeName;
                            }
                        }

                        DestScope DestScope = new DestScope();
                        DestScope.PositionAreaName = Position.PositionAreaName;
                        DestScope.MapID = Floor.MapID;
                        DestScope.ScopeName = ScopeName;

                        destScopes.Add(DestScope);
                        if (!DestScopes.ContainsKey(PathFloor.지상1층))
                            DestScopes.Add(PathFloor.지상1층, destScopes);
                    }
                }
                else if (Floor.FloorName == "2F")
                {
                    //범위 비교하여 값 가져오기
                    var Datas = PathCoordinateDatas.FirstOrDefault(x => x.Key == PathFloor.지상2층);

                    if (Datas.Value != null)
                    {
                        string ScopeName = "";
                        foreach (var Data in Datas.Value)
                        {
                            float x1 = float.Parse(Position.PositionAreaX1);
                            float x3 = float.Parse(Position.PositionAreaX3);
                            float y1 = float.Parse(Position.PositionAreaY1);
                            float y3 = float.Parse(Position.PositionAreaY3);

                            if (x1 >= Data.x1 &&
                                x3 < Data.x3 &&
                                y1 >= Data.Y1 &&
                                y3 < Data.Y3)
                            {
                                ScopeName = Data.ScopeName;
                            }
                        }

                        DestScope DestScope = new DestScope();
                        DestScope.PositionAreaName = Position.PositionAreaName;
                        DestScope.MapID = Floor.MapID;
                        DestScope.ScopeName = ScopeName;

                        destScopes.Add(DestScope);
                        if (!DestScopes.ContainsKey(PathFloor.지상2층))
                            DestScopes.Add(PathFloor.지상2층, destScopes);
                    }
                }
            }
        }

        Dictionary<PathFloor, List<Paths>> PathDatas = new Dictionary<PathFloor, List<Paths>>();
        /// <summary>
        /// 경로 데이터 함수
        /// 정해진 범위에서 범위 끼리 연결하는 데이터
        /// 1 - 2 - 3
        /// </summary>
        private void PathData()
        {
            List<Paths> paths_B1 = new List<Paths>();
            {
                Paths path = new Paths();
                path.path = "E";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2-3";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2-4";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-E";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2-3";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2-4";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-1";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-3";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-4";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-1-E";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-4";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2-1";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2-1-E";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-3";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2-1";
                paths_B1.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2-1-E";
                paths_B1.Add(path);
            }

            PathDatas.Add(PathFloor.지하1층, paths_B1);

            List<Paths> paths_1F = new List<Paths>();
            {
                Paths path = new Paths();
                path.path = "E";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2-3";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2-4";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-E";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2-3";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2-4";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-1";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-3";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-4";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-1-E";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-4";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2-1";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2-1-E";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-3";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2-1";
                paths_1F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2-1-E";
                paths_1F.Add(path);
            }

            PathDatas.Add(PathFloor.지상1층, paths_1F);

            List<Paths> paths_2F = new List<Paths>();
            {
                Paths path = new Paths();
                path.path = "E";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2-3";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "E-1-2-4";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-E";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2-3";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "1-2-4";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-1";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-3";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-4";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "2-1-E";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-4";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2-1";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "3-2-1-E";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-3";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2-1";
                paths_2F.Add(path);
            }
            {
                Paths path = new Paths();
                path.path = "4-2-1-E";
                paths_2F.Add(path);
            }

            PathDatas.Add(PathFloor.지상2층, paths_2F);
        }
        

        Dictionary<PathFloor, List<PathCoordinate>> PathCoordinateDatas = new Dictionary<PathFloor, List<PathCoordinate>>();
        /// <summary>
        /// 경로 데이터 좌표 함수
        /// 정해진 범위의 좌표값들
        /// </summary>
        private void PathCoordinateData()
        {
            List<PathCoordinate> Floor_B1_pathCoordinates = new List<PathCoordinate>();
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "E";
                pathCoordinate.x1 = (float)0.0;
                pathCoordinate.x3 = (float)10.0;
                pathCoordinate.Y1 = (float)25.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)5.0;
                pathCoordinate.Dot_Y = (float)27.5;
                Floor_B1_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "1";
                pathCoordinate.x1 = (float)11.0;
                pathCoordinate.x3 = (float)20.0;
                pathCoordinate.Y1 = (float)25.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)15.0;
                pathCoordinate.Dot_Y = (float)27.5;
                Floor_B1_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "2";
                pathCoordinate.x1 = (float)21.0;
                pathCoordinate.x3 = (float)25.0;
                pathCoordinate.Y1 = (float)20.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)23.5;
                pathCoordinate.Dot_Y = (float)25.0;
                Floor_B1_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "3";
                pathCoordinate.x1 = (float)11.0;
                pathCoordinate.x3 = (float)20.0;
                pathCoordinate.Y1 = (float)20.0;
                pathCoordinate.Y3 = (float)24.0;
                pathCoordinate.Dot_X = (float)15.5;
                pathCoordinate.Dot_Y = (float)22.0;
                Floor_B1_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "4";
                pathCoordinate.x1 = (float)21.0;
                pathCoordinate.x3 = (float)25.0;
                pathCoordinate.Y1 = (float)0.0;
                pathCoordinate.Y3 = (float)19.0;
                pathCoordinate.Dot_X = (float)23.0;
                pathCoordinate.Dot_Y = (float)9.5;
                Floor_B1_pathCoordinates.Add(pathCoordinate);
            }

            PathCoordinateDatas.Add(PathFloor.지하1층, Floor_B1_pathCoordinates);

            List<PathCoordinate> Floor_1F_pathCoordinates = new List<PathCoordinate>();
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "E";
                pathCoordinate.x1 = (float)0.0;
                pathCoordinate.x3 = (float)10.0;
                pathCoordinate.Y1 = (float)25.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)5.0;
                pathCoordinate.Dot_Y = (float)27.5;
                Floor_1F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "1";
                pathCoordinate.x1 = (float)11.0;
                pathCoordinate.x3 = (float)20.0;
                pathCoordinate.Y1 = (float)25.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)15.0;
                pathCoordinate.Dot_Y = (float)27.5;
                Floor_1F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "2";
                pathCoordinate.x1 = (float)21.0;
                pathCoordinate.x3 = (float)25.0;
                pathCoordinate.Y1 = (float)20.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)23.5;
                pathCoordinate.Dot_Y = (float)25.0;
                Floor_1F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "3";
                pathCoordinate.x1 = (float)11.0;
                pathCoordinate.x3 = (float)20.0;
                pathCoordinate.Y1 = (float)20.0;
                pathCoordinate.Y3 = (float)24.0;
                pathCoordinate.Dot_X = (float)15.5;
                pathCoordinate.Dot_Y = (float)22.0;
                Floor_1F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "4";
                pathCoordinate.x1 = (float)21.0;
                pathCoordinate.x3 = (float)25.0;
                pathCoordinate.Y1 = (float)0.0;
                pathCoordinate.Y3 = (float)19.0;
                pathCoordinate.Dot_X = (float)23.0;
                pathCoordinate.Dot_Y = (float)9.5;
                Floor_1F_pathCoordinates.Add(pathCoordinate);
            }

            PathCoordinateDatas.Add(PathFloor.지상1층, Floor_1F_pathCoordinates);

            List<PathCoordinate> Floor_2F_pathCoordinates = new List<PathCoordinate>();
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "E";
                pathCoordinate.x1 = (float)0.0;
                pathCoordinate.x3 = (float)10.0;
                pathCoordinate.Y1 = (float)25.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)5.0;
                pathCoordinate.Dot_Y = (float)27.5;
                Floor_2F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "1";
                pathCoordinate.x1 = (float)11.0;
                pathCoordinate.x3 = (float)20.0;
                pathCoordinate.Y1 = (float)25.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)15.0;
                pathCoordinate.Dot_Y = (float)27.5;
                Floor_2F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "2";
                pathCoordinate.x1 = (float)21.0;
                pathCoordinate.x3 = (float)25.0;
                pathCoordinate.Y1 = (float)20.0;
                pathCoordinate.Y3 = (float)30.0;
                pathCoordinate.Dot_X = (float)23.5;
                pathCoordinate.Dot_Y = (float)25.0;
                Floor_2F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "3";
                pathCoordinate.x1 = (float)11.0;
                pathCoordinate.x3 = (float)20.0;
                pathCoordinate.Y1 = (float)20.0;
                pathCoordinate.Y3 = (float)24.0;
                pathCoordinate.Dot_X = (float)15.5;
                pathCoordinate.Dot_Y = (float)22.0;
                Floor_2F_pathCoordinates.Add(pathCoordinate);
            }
            {
                PathCoordinate pathCoordinate = new PathCoordinate();
                pathCoordinate.ScopeName = "4";
                pathCoordinate.x1 = (float)21.0;
                pathCoordinate.x3 = (float)25.0;
                pathCoordinate.Y1 = (float)0.0;
                pathCoordinate.Y3 = (float)19.0;
                pathCoordinate.Dot_X = (float)23.0;
                pathCoordinate.Dot_Y = (float)9.5;
                Floor_2F_pathCoordinates.Add(pathCoordinate);
            }

            PathCoordinateDatas.Add(PathFloor.지상2층, Floor_2F_pathCoordinates);
        }
        #endregion
    }

    public enum PathFloor
    {
        지하1층,
        지상1층,
        지상2층
    }

    public class Paths
    {
        public string path { get; set; }
    }

    public class ColseRobotList
    {
        public string RobotName { get; set; }
        public string path { get; set; }
        public int pathCount { get; set; }
        public double Source_X { get; set; }
        public double Source_Y { get; set; }
        public double Dest_X { get; set; }
        public double Dest_Y { get; set; }
        public double Nearest_X { get; set; }
        public double Nearest_Y { get; set; }
    }

    public class PathCoordinate
    {
        public string ScopeName { get; set; }
        public float x1 { get; set; }
        public float x3 { get; set; }
        public float Y1 { get; set; }
        public float Y3 { get; set; }
        public float Dot_X { get; set; }
        public float Dot_Y { get; set; }
    }

    public class RobotScope
    {
        public string RobotName { get; set; }
        public string MapID { get; set; }
        public string ScopeName { get; set; }
    }

    public class DestScope
    {
        public string PositionAreaName { get; set; }
        public string MapID { get; set; }
        public string ScopeName { get; set; }
    }

    public class CompareScope
    {
        public string RobotName { get; set; }
        public string SourceScope { get; set; }
        public double Source_X { get; set; }
        public double Source_Y { get; set; }
        public string DestScope { get; set; }
        public double Dest_X { get; set; }
        public double Dest_Y { get; set; }
        public PathFloor PathFloor { get; set; }
    }
}