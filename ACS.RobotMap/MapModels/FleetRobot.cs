using INA_ACS_Server;
using System.Text;

namespace ACS.RobotMap
{
    public class FleetRobot
    {
        // fleet state
        public int RobotID;
        public string RobotIP;
        public string RobotModel;
        public FleetState FleetState;
        public string FleetStateText;
        // robot state
        public string RobotName;                       //MiR의 Model Name 불러오는 변수
        public string MapID;
        public double PosX;                            //MiR의 Position X Value 불러오는 변수
        public double PosY;                            //MiR의 Position Y Value 불러오는 변수
        public double Position_Orientation;                     //MiR의 Position R Value 불러오는 변수
        public double BatteryPercent;                  //MiR의 Battery Percent(Text) 불러오는 변수
        public double BatteryTimeRemaining;                  //MiR의 Battery Percent(Text) 불러오는 변수
        public string MissionQueueID;                  //MiR의 현재 Mission Queue ID 불러오는 변수
        public string MissionText;                     //MiR의 Mission Text 불러오는 변수
        public RobotState StateID;                         //MiR의 상태(ID) 불러오는 변수
        public string StateText;                       //MiR의 상태(Text) 불러오는 변수
        public double DistanceToNextTarget;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("robot_id               : {0}\n", RobotID);
            sb.AppendFormat("robot_name             : {0}\n", RobotName);
            sb.AppendFormat("map_id                 : {0}\n", MapID);
            sb.AppendFormat("position_x             : {0}\n", PosX);
            sb.AppendFormat("position_y             : {0}\n", PosY);
            sb.AppendFormat("position_orientation   : {0}\n", Position_Orientation);
            return sb.ToString();
        }
    }
}