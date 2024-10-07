using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ACS.Common.DTO;
using ACS.RobotApi;

namespace INA_ACS_Server
{
    public enum FleetState
    {
        None = 0,
        unavailable = 1,
        version_mismatch = 2,
        sw_guid_mismatch = 3,
        emergency_stop = 4,
        error = 5,
        starting = 6,
        manual_control = 8,
        pause = 9,
        low_battery = 10,
        ready = 11,
        charging = 12,
        staging = 13,
        independent_mission = 14,
        executing_fleet_order = 15,
        unknown = 16,
        crashed = 17,
        synchronizing = 18,
        initial_synchronization = 19,
        evacuating = 20,
        evacuated = 21,
        thread_died = 22,
        Factory_reset_failed = 23,
        Factory_Resetting = 24,
        deactivated = 25,
    }

    public enum RobotState
    {
        None = 0,
        Starting = 1,
        ShuttingDown = 2,
        Ready = 3,
        Pause = 4,
        Executing = 5,
        Aborted = 6,
        Completed = 7,
        Docked = 8,
        Docking = 9,
        EmergencyStop = 10,
        ManualControl = 11,
        Error = 12,
    }

    public enum RobotType
    {
        LIFT = 0,
        HOOK = 1,
    }


    //MiR Status 저장관련 변수
    public class Robot
    {


        private IMirApi _api = null;
        public IMirApi Api
        {
            get => _api;
            set
            {
                if (_api != null) new InvalidOperationException("Api는 최초 한번만 할당 가능하다!");
                _api = value;
            }
        }


        public bool DataChanged = false;
        private int id;
        private int jobId;
        private string aCSRobotGroup;           //Robot ACSMissionGroup 를 나누어 미션을 전송하기 위한 변수 
        private bool aCSRobotActive;            // robot 사용 여부
        private int robotID;                    // robot id (fleet only)
        private string robotIp;                 // robot ip
        private string robotName;               // robot name
        private string robotAlias;              // robot alias
        private RobotState stateID;             // MiR의 상태(Text)
        private string stateText;               // MiR의 상태(ID)
        private string missionText;             // MiR의 Mission Text
        private int missionQueueID;             // MiR의 현재 Mission Queue ID
        private double batteryPercent;          // MiR의 Battery Percent(Text)
        private string mapID;                   // map id
        private double distanceToNextTarget;    // MiR의 다음 타겟까지의 거리
        private double position_Orientation;    // MiR의 Position R Value
        private double position_X;              // MiR의 Position X Value
        private double position_Y;              // MiR의 Position Y Value
        private string robotModel;              // MiR의 Model Name ex. "MiR100"
        private string errors;                  // MiR의 Error List
        private string product;                 // AmkorK5 사용 Line 자재 확인 
        private string door;                    // AmkorK5 사용 Line TopModule Door 상태
        private string positionZoneName;
        private int batteryTimeRemaining;       // MiR의 Battery 남은 시간
        private bool hookAvailable;             // Hook가 존재하나?
        private bool hookCartAttached;          // Hook에 카트 연결되어 있나?

        public RobotType RobotType = RobotType.LIFT;     // 0=LIFT타입, 1=HOOK타입
        private FleetState fleetState;             // fleet 상태
        private string fleetStateText;             // fleet 상태
        private bool connectState;                 // RobotConnect 상태

        public int Id
        {
            get => id;
            set
            {
                if (id != value) DataChanged = true;
                id = value;
            }
        }
        public int JobId
        {
            get => jobId;
            set
            {
                if (jobId != value) DataChanged = true;
                jobId = value;
            }
        }
        public string ACSRobotGroup
        {
            get => aCSRobotGroup;
            set
            {
                if (aCSRobotGroup != value) DataChanged = true;
                aCSRobotGroup = value;
            }
        }
        public bool ACSRobotActive
        {
            get => aCSRobotActive;
            set
            {
                if (aCSRobotActive != value) DataChanged = true;
                aCSRobotActive = value;
            }
        }


        // robot info
        public int RobotID
        {
            get => robotID;
            set
            {
                if (robotID != value) DataChanged = true;
                robotID = value;
            }
        }
        public string RobotIp
        {
            get => robotIp;
            set
            {
                if (robotIp != value) DataChanged = true;
                robotIp = value;
            }
        }


        // robot status
        public string RobotName
        {
            get => robotName;
            set
            {
                if (robotName != value) DataChanged = true;
                robotName = value;
            }
        }
        public string RobotAlias
        {
            get => robotAlias;
            set
            {
                if (robotAlias != value) DataChanged = true;
                robotAlias = value;
            }
        }
        public RobotState StateID
        {
            get => stateID;
            set
            {
                if (stateID != value) DataChanged = true;
                stateID = value;
            }
        }
        public string StateText
        {
            get => stateText;
            set
            {
                if (stateText != value) DataChanged = true;
                stateText = value;
            }
        }

        public bool ConnectState
        {
            get => connectState;
            set
            {
                if (connectState != value) DataChanged = true;
                connectState = value;
            }
        }
        public FleetState FleetState
        {
            //Fleet 버전
            get => fleetState;
            set
            {
                if (fleetState != value) DataChanged = true;
                fleetState = value;
            }
            //로봇 버전 
            //get => throw new NotImplementedException($"이 FleetState 속성은 Fleet Version일때 사용가능합니다!");
            //set => throw new NotImplementedException($"이 FleetState 속성은 Fleet Version일때 사용가능합니다!");
        }

        public string FleetStateText
        {
            //Fleet 버전
            get => fleetStateText;
            set
            {
                if (fleetStateText != value) DataChanged = true;
                fleetStateText = value;
            }

            //로봇 버전
            //get => throw new NotImplementedException($"이 FleetStateText 속성은 Fleet Version일때 사용가능합니다!");
            //set => throw new NotImplementedException($"이 FleetStateText 속성은 Fleet Version일때 사용가능합니다!");
        }

        public string MissionText
        {
            get => missionText;
            set
            {
                if (missionText != value) DataChanged = true;
                missionText = value;
            }
        }
        public int MissionQueueID
        {
            get => missionQueueID;
            set
            {
                if (missionQueueID != value) DataChanged = true;
                missionQueueID = value;
            }
        }
        public double BatteryPercent
        {
            get => batteryPercent;
            set
            {
                double roundedValue = Math.Round(value, 2);
                if (batteryPercent != roundedValue) DataChanged = true;
                batteryPercent = roundedValue;
            }
        }
        public string MapID
        {
            get => mapID;
            set
            {
                if (mapID != value) DataChanged = true;
                mapID = value;
            }
        }
        public double DistanceToNextTarget
        {
            get => distanceToNextTarget;
            set
            {
                double roundedValue = Math.Round(value, 2);
                if (distanceToNextTarget != roundedValue) DataChanged = true;
                distanceToNextTarget = roundedValue;
            }
        }
        public double Position_Orientation
        {
            get => position_Orientation;
            set
            {
                double roundedValue = Math.Round(value, 2);
                if (position_Orientation != roundedValue) DataChanged = true;
                position_Orientation = roundedValue;
            }
        }
        public double Position_X
        {
            get => position_X;
            set
            {
                double roundedValue = Math.Round(value, 2);
                if (position_X != roundedValue) DataChanged = true;
                position_X = roundedValue;
            }
        }
        public double Position_Y
        {
            get => position_Y;
            set
            {
                double roundedValue = Math.Round(value, 2);
                if (position_Y != roundedValue) DataChanged = true;
                position_Y = roundedValue;
            }
        }

        // 로봇 에러 정보 ==========BEGIN
        // - Errors
        // - ErrorsJson : robots 테이블 컬럼 추가 : alter table robots add ErrorsJson varchar(5000) null

        public RobotErrorResponse[] Errors; // 로봇 에러 리스트. 이 필드는 DB에 저장하지 않는다!

        public string ErrorsJson        // 로봇 에러 리스트 json.
        {
            get => errors;
            set
            {
                if (errors != value) DataChanged = true;
                errors = value;
            }
        }

        public string ErrorMailState = ""; // 로봇에러발생시 이메일전송진행상태 플래그 (mail-sent=전송했음, no-error=에러없음)

        public bool POSTimeOutFlag = false;
        public double POSTimeOutError_X = 0;
        public double POSTimeOutError_Y = 0;
        public DateTime POSTimeOutError_Time;

        // 로봇 에러 정보 ==========END

        public string RobotModel
        {
            get => robotModel;
            set
            {
                if (robotModel != value) DataChanged = true;
                robotModel = value;
            }
        }

        public string Product
        {
            get => product;
            set
            {
                if (product != value) DataChanged = true;
                product = value;
            }
        }
        public string Door
        {
            get => door;
            set
            {
                if (door != value) DataChanged = true;
                door = value;
            }
        }
        public string PositionZoneName
        {
            get => positionZoneName;
            set
            {
                if (positionZoneName != value) DataChanged = true;
                positionZoneName = value;
            }
        }
        public int BatteryTimeRemaining
        {
            get => batteryTimeRemaining;
            set
            {
                if (batteryTimeRemaining != value) DataChanged = true;
                batteryTimeRemaining = value;
            }
        }
        public bool HookAvailable
        {
            get => hookAvailable;
            set
            {
                if (hookAvailable != value) DataChanged = true;
                hookAvailable = value;
            }
        }
        public bool HookCartAttached
        {
            get => hookCartAttached;
            set
            {
                if (hookCartAttached != value) DataChanged = true;
                hookCartAttached = value;
            }
        }
        public string LastCallName { get; set; }    // 로봇이 수행한 마지막 콜을 기억해둔다



        // hook status cart
        public double HookStatusCartId { get; set; }
        public double HookStatusCartWidth { get; set; }
        public double HookStatusCartLength { get; set; }
        public double HookStatusCartHeight { get; set; }
        public double HookStatusCartOffsetLockedWheels { get; set; }



        // hook status
        public string Hook_BrakeStateText { get; set; }
        public int Hook_BrakeState { get; set; }
        public bool Hook_BrakeBraked { get; set; }

        public double Hook_ArmAngle { get; set; }

        public string Hook_GripperStateText { get; set; }
        public int Hook_GripperState { get; set; }
        public bool Hook_GripperClosed { get; set; }

        public string Hook_HeightStateText { get; set; }
        public int Hook_HeightState { get; set; }
        public double Hook_Height { get; set; }

        //아이세로미림 사용
        public double StartTargetDistance { get; set; }



        // fleet status
        //public FleetState Fleet_State;              // fleet 상태
        //public string Fleet_State_Text;             // fleet 상태


        // robot registers
        public RobotRegisters Registers = new RobotRegisters(); // 레지스터.  데이터 갱신은 MiR_Get_Register()함수 이용한다.
        public RoBotIoModules internalIoModule = new RoBotIoModules(); //Robot 내부 IoModule




        public override string ToString()
        {
            //return JsonConvert.SerializeObject(this);
            return $"id={Id}, RobotID={RobotID}, Name={RobotName}, JobID={JobId}" +
                $", stateID={StateID}" +
                //$", stateText={StateText}" +
                $", Group={ACSRobotGroup}" +
                $", MissionQueueID={missionQueueID,4}" +
                $", Battery={BatteryPercent}" +
                //$", DistanceToNextTarget={DistanceToNextTarget}" +
                $", Theta={Position_Orientation}, X={position_X}, Y={position_Y}" +
                $", MissionText={MissionText}" +
                "";
        }
    }

    public class RoBotIoModules
    {
        public string type = "internal";
        public string guid;
        public bool Get_InputValue1;    //자채 IO Module Get_InputValue1 = 자재 무(false) 유(true)
        public bool Get_InputValue2;    //자채 IO Module Get_InputValue2 = door 열림 (false) 닫힘 (true)
        public bool Get_InputValue3;
        public bool Get_InputValue4;
        public bool Get_OutputValue1;
        public bool Get_OutputValue2;
        public bool Get_OutputValue3;
        public bool Get_OutputValue4;
    }



    public class RobotRegisters
    {
        public double[] dMiR_Register_Value = new double[100];
    }
}
