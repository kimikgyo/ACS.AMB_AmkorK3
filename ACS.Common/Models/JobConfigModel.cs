using System;
using System.Collections.Generic;

namespace INA_ACS_Server
{
    //CallButton Mission 설정 변수
    public class JobConfigModel
    {
        public int Id { get; set; }
        public string ACSMissionGroup { get; set; } = "None";    //미션 그룹 설정 변수(Robot 그룹과 일치한 미션을 전송하기 위함)
        public string CallName { get; set; }
        public string JobConfigUse { get; set; }            //JobMission사용 미사용 설정
        public string MissionName { get; set; }         //CallButton 미션 설정 변수           // ===>  job 에서는 사용안함. 대신 GetJobMissionName()/SetJobMissionName() 사용.
        public string ErrorMissionName { get; set; }    //CallButton 에러미션 설정 변수        // ===>  job 에서는 사용안함.

        public const int JOB_MISSION_TOTAL_COUNT = 5;  // 고정개수!

        public string JobMissionName1 { get; set; } = "None"; //CallButton job 미션 설정 변수... 직접 사용하지않고 아래 Get/SetJobMissionName() 사용.
        public string JobMissionName2 { get; set; } = "None";
        public string JobMissionName3 { get; set; } = "None";
        public string JobMissionName4 { get; set; } = "None";
        public string JobMissionName5 { get; set; } = "None";

        public string jobCallSignal { get; set; } = "None";        //jobConfig 에서 설정된 jobCall신호

        public int DisplayFlag { get; set; }
       
        public bool ProductValue { get; set; } = false;  // Mission Post 시 자재 유무  
        public bool ProductActive { get; set; } = false;
        public string ElevatorModeValue { get; set; } = "None";
        public bool ElevatorModeActive { get; set; } = false;
        public bool TransportCountActive { get; set; } = false;      //리포트 챠트시 필요 [체크되어있는것만 차트에서 조회가 가능]

        public int JobPriority { get; set; } //우선순위
        public double ExecuteBattery { get; set; }          //job수행 가능 배터리
        public string SourceFloor { get; set; }
        public string DestFloor { get; set; }

        public string GetJobMissionName(int i)
        {
            switch (i)
            {
                case 0: return JobMissionName1;
                case 1: return JobMissionName2;
                case 2: return JobMissionName3;
                case 3: return JobMissionName4;
                case 4: return JobMissionName5;
            }
            throw new ArgumentOutOfRangeException("GetMissionName");
        }

        public void SetJobMissionName(int i, string newValue)
        {
            switch (i)
            {
                case 0: JobMissionName1 = newValue; return;
                case 1: JobMissionName2 = newValue; return;
                case 2: JobMissionName3 = newValue; return;
                case 3: JobMissionName4 = newValue; return;
                case 4: JobMissionName5 = newValue; return;
            }
            throw new ArgumentOutOfRangeException("SetMissionName");
        }


        public override string ToString()
        {
            return $"Id={Id}," +
                   $"ACSMissionGroup={ACSMissionGroup}, " +
                   $"CallName={CallName}, " +
                   $"JobConfig={JobConfigUse}, " +
                   $"JobMissionName1={JobMissionName1}," +
                   $"JobMissionName2={JobMissionName2}," +
                   $"JobMissionName3={JobMissionName3}," +
                   $"JobMissionName4={JobMissionName4}," +
                   $"JobMissionName5={JobMissionName5}," +
                   $"ProductValue={ProductValue}," +
                   $"ProductActive={ProductActive}," +
                   $"ElevatorModeValue={ElevatorModeValue}," +
                   $"ElevatorModeActive={ElevatorModeActive}," +
                   $"TransportCountActive={TransportCountActive}," +
                   $"DisplayFlag={DisplayFlag},";
        }


    }
}
