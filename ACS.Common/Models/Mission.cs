using Newtonsoft.Json;
using System;

namespace INA_ACS_Server
{
    /// * Mission 상태 전이
    ///                                             |  => 여기부터는 로봇에서 수신되는 미션 상태 (GetMissionQueue API)
    ///                                             |
    /// (미션콜) => Init    => Waiting   => Sending  |  => Pending   => Executing   => Done / Aborted => (미션삭제)
    ///                                             |
    ///            미션생성    전송대기       전송중       전송완료/실행대기   실행중         완료     에러

    public class Mission
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        // 미션 생성시 설정
        public string ACSMissionGroup { get; set; }   //미션 그룹 설정 변수(Robot 그룹과 일치한 미션을 전송하기 위함)
        public string CallName { get; set; }   //미션 그룹 설정 변수(Robot 그룹과 일치한 미션을 전송하기 위함)
        public int CallButtonIndex { get; set; }
        public string CallButtonName { get; set; }
        public string MissionName { get; set; }
        public string ErrorMissionName { get; set; }
        public DateTime MissionOrderTime { get; set; } = DateTime.Now;
        // 미션 전송시 설정
        public Robot Robot { get; set; }
        public string JobCreateRobotName { get; set; }  //Job 생성시 로봇지정 이름
        public string RobotName { get; set; }
        public int RobotID { get; set; }                    // fleet only
        // 미션 전송후 응답데이터로 설정
        public int ReturnID { get; set; }                   // 미션상태 조회시 필요
        // 미션 상태 변경될때 갱신
        public string MissionState { get; set; }

        public override string ToString()
        {
            return $"id={Id,-5}, " +
                    $"jobid={JobId,-5}, " +
                    $"ACSMissionGroup={ACSMissionGroup,-5}, " +
                    $"CallName={CallName,-5}, " +
                    $"call={CallButtonName,-8}, " +
                    $"JobCreateRobotName={JobCreateRobotName,-15}, " +
                    $"robot={RobotName,-15}, " +
                    $"state={MissionState,-10}, " +
                    $"returnid={ReturnID,-5}, " +
                    $"mission={MissionName}";
        }
    }
}
