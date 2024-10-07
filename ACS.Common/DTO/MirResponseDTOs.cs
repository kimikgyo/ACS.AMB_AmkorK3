using System;
using Newtonsoft.Json;

namespace ACS.Common.DTO
{
    public class MirMapSimpleResponse
    {
        public string guid;
        public string name;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class MirMapDetailResponse
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



    public class MirPositionSimpleResponse
    {
        public string name;
        public string guid;
        public string type_id;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class MirPositionDetailResponse
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



    public class GetHookStatusResponse
    {
        public bool available { get; set; }
        public double angle { get; set; }
        public GetHookBrakeResponse brake { get; set; }
        public GetHookGripperResponse gripper { get; set; }
        public GetHookHeightResponse height { get; set; }
    }
    public class GetHookBrakeResponse
    {
        public string state_string { get; set; }
        public int state { get; set; }
        public bool braked { get; set; }
    }
    public class GetHookGripperResponse
    {
        public string state_string { get; set; }
        public int state { get; set; }
        public bool closed { get; set; }
    }
    public class GetHookHeightResponse
    {
        public string state_string { get; set; }
        public int state { get; set; }
        public double height { get; set; }
    }



    public class RegisterResponse
    {
        public int id { get; set; }
        public string label { get; set; }
        public float value { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }



    public class MissionQueueRequest
    {
        public string mission_id;  // 필수항목
    }

    public class MissionQueueSimpleResponse
    {
        public int id;                  //Post Mission 시 MiR에서 반환된 ID 설정 변수
        public string state;
        public string url;
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class MissionQueueDetailResponse
    {
        /*
        MiR
            "priority": 0,
            "ordered": "2022-08-03T09:25:01",
            "description": "",
            "parameters": [],
            "state": "Executing",
            "started": "2022-08-03T09:25:01",
            "created_by_name": "Administrator",
            "mission": "/v2.0.0/missions/900a76ed-d0fe-11ec-b651-94c691a7389a",
            "actions": "/v2.0.0/mission_queue/723/actions",
            "fleet_schedule_guid": "",
            "mission_id": "900a76ed-d0fe-11ec-b651-94c691a7389a",
            "finished": null,
            "created_by": "/v2.0.0/users/mirconst-guid-0000-0005-users0000000",
            "created_by_id": "mirconst-guid-0000-0005-users0000000",
            "allowed_methods": ["PUT", "GET", "DELETE"],
            "message": "",
            "control_state": 0,
            "id": 723,
            "control_posid": "0"
        */
        public int id;                          // common  //Post Mission 시 MiR에서 반환된 ID 설정 변수
        public string state;                    // common
        public int priority;                    // common
        public string mission_id;               // common
        public DateTime? ordered;               // mir only
        public DateTime? finished;              // mir only
        public DateTime? started;               // mir only
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

}