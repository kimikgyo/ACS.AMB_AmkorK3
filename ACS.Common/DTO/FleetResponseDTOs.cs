using System;
using Newtonsoft.Json;

namespace ACS.Common.DTO
{
    public class FleetMapSimpleResponse
    {
        public string guid;
        public string name;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class FleetMapDetailResponse
    {
        public string name;
        public string guid;
        public double origin_x;
        public double origin_y;
        public double origin_theta;
        public double resolution;
        public string map;
        public string metadata;
        public string base_map;
        public System.Drawing.Image Image = null;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }



    public class FleetPositionSimpleResponse
    {
        public string name;
        public string guid;
        public string type_id;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class FleetPositionDetailResponse
    {
        public string name;
        public string guid;
        public string type_id;
        public string map_id;
        public double pos_x;
        public double pos_y;
        public double orientation;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }



    public class MissionResponse
    {
        /*
            "guid": "47ffbd03-cabb-11ec-8a38-94c691a7389a",
            "name": "MyMission",
        */
        public string guid;
        public string name;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }



    public class FleetRobotInfoResponse
    {
        public int id { get; set; }
        public string ip { get; set; }
        public int fleet_state { get; set; }
        public string fleet_state_text { get; set; }
        public string robot_model { get; set; }
        public RobotStatusResponse status { get; set; }
    }
    public class RobotStatusResponse
    {
        public string robot_name { get; set; }
        public int state_id { get; set; }
        public string state_text { get; set; }
        public string mission_text { get; set; }
        public int? mission_queue_id { get; set; }
        public double battery_percentage { get; set; }
        public double distance_to_next_target { get; set; }
        public string map_id { get; set; }
        public RobotErrorResponse[] errors { get; set; }
        public RobotPositionResponse position { get; set; }
        public string errorsJson { get; set; }
        public int battery_time_remaining { get; set; }
        public HookStatusResponse hook_status { get; set; }
        public HookDataResponse hook_data { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class RobotErrorResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("module")]
        public string Module { get; set; }
    }
    public class RobotPositionResponse
    {
        public double x { get; set; }
        public double y { get; set; }
        public double orientation { get; set; }
    }
    public class HookStatusResponse
    {
        public bool available { get; set; }
        public bool trolley_attached { get; set; }  // cart 끌고 갈때 true.    fleet only.
        public bool cart_attached { get; set; }     // cart 끌고 갈때 true.    mir only.
        public HookCartInfoResponse trolley { get; set; }   // fleet only.
        public HookCartInfoResponse cart { get; set; }      // mir only.
    }
    public class HookCartInfoResponse
    {
        public double id { get; set; }
        public double width { get; set; }
        public double length { get; set; }
        public double height { get; set; }
        public double offset_locked_wheels { get; set; }
    }
    public class HookDataResponse
    {
        public double length { get; set; }
        public double height { get; set; }
        public int brake_state { get; set; }
        public int gripper_state { get; set; }
        public int height_state { get; set; }
    }



    public class SettingsResponse
    {
        /*
            "name": "Minimum battery percentage for release",
            "default": "60",
            "value": "60",
            "id": 2085
        */
        public string name;
        public string @default;
        public string value;
        public int id;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }



    public class MissionSchedulerRequest
    {
        public string mission_id;   // mission guid
        public int robot_id;        // 0 = any robot
        //public string earliest_start_time; // 필드값이 null 이면 안된다!
        public int priority;
        public bool high_priority;
        public string description;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class MissionSchedulerSimpleResponse
    {
        public int id;                  //Post Mission 시 MiR에서 반환된 ID 설정 변수
        public string state;
        public string url;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class MissionSchedulerDetailResponse
    {
        /*
        Fleet
            "id": 542
            "state": "Pending",
            "order_time": "2022-05-31T17:44:24",
            "finish_time": "1970-01-01T00:00:00",
            "earliest_start_time": "2022-05-31T17:44:24",
            "start_time": "1970-01-01T00:00:00",
            "mission_id": "47ffbd03-cabb-11ec-8a38-94c691a7389a",
            "mission_name": "YKK_2",
            "priority": 0,
            "high_priority": true,
            "robot_id": 14,
        */
        public int id;                          // common  //Post Mission 시 MiR에서 반환된 ID 설정 변수
        public string state;                    // common
        public int priority;                    // common
        public string mission_id;               // common
        public int robot_id;                    // fleet only  //Post Mission 시 사용된 Robot ID 설정 변수
        public string mission_name;             // fleet only
        public bool high_priority;              // fleet only
        public DateTime? order_time;            // fleet only
        public DateTime? finish_time;           // fleet only
        public DateTime? earliest_start_time;   // fleet only
        public DateTime? start_time;            // fleet only
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

}