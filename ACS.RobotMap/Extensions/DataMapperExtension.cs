using ACS.Common.DTO;
using ACS.RobotMap;
using INA_ACS_Server;

internal static class DataMapperExtension
{
    static readonly object lockObject = new object();



    public static void MapToFleetMap(this FleetMapDetailResponse obj, FleetMap map)
    {
        lock (lockObject)
        {
            map.Name = obj.name;
            map.Guid = obj.guid;
            map.OriginX = obj.origin_x;
            map.OriginY = obj.origin_y;
            map.OriginTheta = obj.origin_theta;
            map.Resolution = obj.resolution;
            map.map= obj.map;
            map.base_map = obj.base_map;
            map.Image = obj.Image;
        }
    }

    public static void MapToFleetMap(this MirMapDetailResponse obj, FleetMap map)
    {
        lock (lockObject)
        {
            map.Name = obj.name;
            map.Guid = obj.guid;
            map.OriginX = obj.origin_x;
            map.OriginY = obj.origin_y;
            map.OriginTheta = obj.origin_theta;
            map.Resolution = obj.resolution;
            map.map = obj.map;
            map.base_map = obj.base_map;
            map.Image = obj.Image;
        }
    }



    public static void MapToFleetRobot(this RobotStatusResponse obj, FleetRobot robot)
    {
        lock (lockObject)
        {
            // fleet status
            //robot.RobotID = -1;
            //robot.RobotIP = string.Empty;
            //robot.RobotModel = string.Empty;
            //robot.FleetState = FleetState.None;
            //robot.FleetStateText = string.Empty;
            // robot status
            robot.RobotName = obj.robot_name;
            robot.StateID = (RobotState)obj.state_id;
            robot.StateText = obj.state_text;
            robot.MissionText = obj.mission_text;
            robot.MissionQueueID = (obj.mission_queue_id ?? default).ToString();
            robot.BatteryPercent = obj.battery_percentage;
            robot.BatteryTimeRemaining = obj.battery_time_remaining;
            robot.Position_Orientation = obj.position.orientation;
            robot.PosX = obj.position.x;
            robot.PosY = obj.position.y;
            robot.MapID = obj.map_id;
            robot.DistanceToNextTarget = obj.distance_to_next_target;
        }
    }

    public static void MapToFleetRobot(this FleetRobotInfoResponse obj, FleetRobot robot)
    {
        // fleet status
        robot.RobotID = obj.id;
        robot.RobotIP = obj.ip;
        robot.RobotModel = obj.robot_model;
        robot.FleetState = (FleetState)obj.fleet_state;
        robot.FleetStateText = obj.fleet_state_text;
        // robot status
        robot.RobotName = obj.status.robot_name;
        robot.StateID = (RobotState)obj.status.state_id;
        robot.StateText = obj.status.state_text;
        robot.MissionText = obj.status.mission_text;
        robot.MissionQueueID = (obj.status.mission_queue_id ?? default).ToString();
        robot.BatteryPercent = obj.status.battery_percentage;
        robot.BatteryTimeRemaining = obj.status.battery_time_remaining;
        robot.Position_Orientation = obj.status.position.orientation;
        robot.PosX = obj.status.position.x;
        robot.PosY = obj.status.position.y;
        robot.MapID = obj.status.map_id;
        robot.DistanceToNextTarget = obj.status.distance_to_next_target;
    }

}
