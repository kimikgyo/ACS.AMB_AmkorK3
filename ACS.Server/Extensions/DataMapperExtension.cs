using ACS.Common.DTO;
using INA_ACS_Server;
using Newtonsoft.Json;

internal static class DataMapperExtension
{
    static readonly object lockObject = new object();



    public static void MapToRobot(this FleetRobotInfoResponse obj, Robot robot)
    {
        lock (lockObject)
        {
            // fleet status
            robot.RobotID = obj.id;
            //robot.RobotIp = obj.ip; // =========> IP는 프로그램 시작시 config file 읽어서 설정하고, 이후 설정창에서 필요시 변경하므로 이 라인은 무시.
            robot.RobotModel = obj.robot_model;
            robot.FleetState = (FleetState)obj.fleet_state;
            robot.FleetStateText = obj.fleet_state_text;

            // robot status
            robot.RobotName = obj.status.robot_name;
            robot.StateID = (RobotState)obj.status.state_id;
            robot.StateText = obj.status.state_text;
            robot.MissionText = obj.status.mission_text;
            robot.MissionQueueID = obj.status.mission_queue_id ?? default;
            robot.BatteryPercent = obj.status.battery_percentage;
            robot.BatteryTimeRemaining = obj.status.battery_time_remaining;
            robot.Position_Orientation = obj.status.position.orientation;
            robot.Position_X = obj.status.position.x;
            robot.Position_Y = obj.status.position.y;
            robot.MapID = obj.status.map_id;
            robot.DistanceToNextTarget = obj.status.distance_to_next_target;
            robot.Errors = obj.status.errors;
            robot.ErrorsJson = JsonConvert.SerializeObject(obj.status.errors); // extra

            // hook status
            Map_HookStatus(obj.status);
        }

        void Map_HookStatus(RobotStatusResponse obj2)
        {
            // hook status
            if (obj2.hook_status != null)
            {
                robot.HookAvailable = obj2.hook_status.available;
                robot.HookCartAttached = obj2.hook_status.trolley_attached;
                // cart info
                if (obj2.hook_status.trolley != null)
                {
                    robot.HookStatusCartId = obj2.hook_status.trolley.id;
                    robot.HookStatusCartWidth = obj2.hook_status.trolley.width;
                    robot.HookStatusCartLength = obj2.hook_status.trolley.length;
                    robot.HookStatusCartHeight = obj2.hook_status.trolley.height;
                    robot.HookStatusCartOffsetLockedWheels = obj2.hook_status.trolley.offset_locked_wheels;
                }
            }
            // hook data
            if (obj2.hook_data != null)
            {
                robot.Hook_Height = obj2.hook_data.height;
                robot.Hook_HeightState = obj2.hook_data.height_state;
                robot.Hook_BrakeState = obj2.hook_data.brake_state;
                robot.Hook_GripperState = obj2.hook_data.gripper_state;
            }
        }
    }

    public static void MapToRobot(this RobotStatusResponse obj, Robot robot)
    {
        lock (lockObject)
        {
            // robot status
            robot.RobotName = obj.robot_name;
            robot.StateID = (RobotState)obj.state_id;
            robot.StateText = obj.state_text;
            robot.MissionText = obj.mission_text;
            robot.MissionQueueID = obj.mission_queue_id ?? default;
            robot.BatteryPercent = obj.battery_percentage;
            robot.BatteryTimeRemaining = obj.battery_time_remaining;
            robot.Position_Orientation = obj.position.orientation;
            robot.Position_X = obj.position.x;
            robot.Position_Y = obj.position.y;
            robot.MapID = obj.map_id;
            robot.DistanceToNextTarget = obj.distance_to_next_target;
            robot.Errors = obj.errors;
            robot.ErrorsJson = JsonConvert.SerializeObject(obj.errors); // extra
            robot.BatteryTimeRemaining = obj.battery_time_remaining;

            // hook status
            Map_HookStatus(obj);
        }

        void Map_HookStatus(RobotStatusResponse obj2)
        {
            // hook status
            if (obj2.hook_status != null)
            {
                robot.HookAvailable = obj2.hook_status.available;
                robot.HookCartAttached = obj2.hook_status.cart_attached;
                // cart info
                if (obj2.hook_status.cart != null)
                {
                    robot.HookStatusCartId = obj2.hook_status.cart.id;
                    robot.HookStatusCartWidth = obj2.hook_status.cart.width;
                    robot.HookStatusCartLength = obj2.hook_status.cart.length;
                    robot.HookStatusCartHeight = obj2.hook_status.cart.height;
                    robot.HookStatusCartOffsetLockedWheels = obj2.hook_status.cart.offset_locked_wheels;
                }
            }
            // hook data
            if (obj2.hook_data != null)
            {
                robot.Hook_Height = obj2.hook_data.height;
                robot.Hook_HeightState = obj2.hook_data.height_state;
                robot.Hook_BrakeState = obj2.hook_data.brake_state;
                robot.Hook_GripperState = obj2.hook_data.gripper_state;
            }
        }
    }

    public static void MapToRobot(this GetHookStatusResponse obj, Robot robot)
    {
        lock (lockObject)
        {
            robot.HookAvailable = obj.available;

            robot.Hook_ArmAngle = obj.angle;

            robot.Hook_BrakeStateText = obj.brake.state_string;
            robot.Hook_BrakeState = obj.brake.state;
            robot.Hook_BrakeBraked = obj.brake.braked;

            robot.Hook_GripperStateText = obj.gripper.state_string;
            robot.Hook_GripperState = obj.gripper.state;
            robot.Hook_GripperClosed = obj.gripper.closed;

            robot.Hook_HeightStateText = obj.height.state_string;
            robot.Hook_HeightState = obj.height.state;
            robot.Hook_Height = obj.height.height;
        }
    }

}
