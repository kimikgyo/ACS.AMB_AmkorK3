using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public enum JobState
    {
        None = 0,
        JobInit,            // job 생성시
        JobWaitWmsQty,      // job 창고재고확인대기상태 (LIFT 자재미션에만 해당)
        JobWaitWmsOut,      // job 창고출고완료대기상태 (LIFT 자재미션에만 해당)
        JobStart,           // job control 조건에 따라 waiting 상태로 넘어간다.   (출고완료시, 출고무관하면 바로 jobinit->jobstart로 변경됨)
        JobWaitWmsIn,       // job 창고입고대기상태 (창고 컨베이어 랙 레디 신호 기다림) (LIFT 회수미션에만 해당)
        JobWaiting,         // job 첫미션이 선택시 (이상태일때 postMission으로 전송됨)
        JobExecuting,       // job 첫미션이 전송시
        JobDone,            // job 모든미션이 완료시
        JobAborted,         // job 에러시
        JobInvalid,         // job 무효시
    }

    // ** job queue만 사용 (mission queue는 사용안함, 삭제하지 않고 그대로 유지)
    //
    // 콜버튼에서 job 요청 (enqueue job reuest)
    // job queue process.
    // - dequeue job request
    // - add new job(with missions) to uow.jobs)
    // - job.missions 모두 생성한다
    // job scheduler. uow.jobs 에서 job 꺼내 처리
    // job scheduler. job 미션 1개씩 전송한다
    // job 상태 갱신 (job 내 참조하고 있는 미션들의 상태를 모두 갱신,  미션삭제는 하지 않음!)
    // 모든 미션이 완료되면 job 삭제한다 (job의 미션도 모두 삭제)

    public class Job
    {
        // job config
        public int Id { get; set; }
        public string Name { get; set; }

        public string CallName { get; set; }          // pop call name
        //public string CallButtonName { get; set; }          // pop call name
        public int PopServerId { get; set; }                // pop call server id
        public string PopCallReturnType { get; set; }       // pop call return type (N=투입,Y=회수)
        public int PopCallAngle { get; set; }      // pop call angle
        public int WmsId { get; set; }                      // pop call wms id
        public int PopCallState { get; set; }               // pop call 처리 상태 (0=미처리,1=처리완료)
        public DateTime PopCallTime { get; set; } = DateTime.Now;          // pop call time (pop에서 가져온 값이 null 일 경우 에러나서 미리 초기화)

        public string ACSJobGroup { get; set; }             // 미션 그룹 설정 변수(Robot 그룹과 일치한 미션을 전송하기 위함)
        public string JobCreateRobotName { get; set; }      //Job 생성시 지정 로봇이름
        public string RobotName { get; set; }
        public double ExecuteBattery { get; set; }                      //jobConfig에서 설정한 job가능 배터리
        public JobState JobState { get; set; }
        public string JobStateText { get; set; }
        public DateTime CallTime { get; set; } = DateTime.Now;          // pop call time (pop에서 가져온 값이 null 일 경우 에러나서 미리 초기화)
        public DateTime JobCreateTime { get; set; }

        public int[] MissionIds { get; set; } = new int[5];
        public List<Mission> Missions { get; set; }

        public int MissionTotalCount { get; set; }
        public int MissionSentCount { get; set; }

        public int WmsStepNo = 0; // 자재 재고 처리 스텝
        public int TransportCountValue { get; set; }                    //반송량 Count (자재반송수량이 다를수있기때문에 설정)
        public int JobPriority { get; set; }

        // 미션 리스트 설정 변수 =====================> DB에 억세스할때만 사용된다.

        // MissionIds[] <=> DB 필드 맵핑용

        public int MissionId1 { get; set; }      // repository 클래스에서만 사용한다!
        public int MissionId2 { get; set; }      // repository 클래스에서만 사용한다!
        public int MissionId3 { get; set; }      // repository 클래스에서만 사용한다!
        public int MissionId4 { get; set; }      // repository 클래스에서만 사용한다!
        public int MissionId5 { get; set; }      // repository 클래스에서만 사용한다!

        // 미션 리스트 설정 변수 =====================> DB에 억세스할때만 사용된다. 


        public override string ToString()
        {
            return $"id={Id,-5}, " +
                   $"ACSJobGroup = {ACSJobGroup,-5}, " +
                   $"call={CallName,-5}, " +
                   //$"call={CallButtonName,-5}, " +
                   $"popSvrId={PopServerId}, " +
                   $"RetuType={PopCallReturnType}, " +
                   $"Angle={PopCallAngle}, " +
                   $"WmsId={WmsId}, " +
                   $"robot={RobotName,-15}, " +
                   $"ExecuteBattery={ExecuteBattery,-15}, " +
                   $"state={JobState,-15}, " +
                   $"Sent/Total={MissionSentCount}/{MissionTotalCount}, " +
                   $"missionID={string.Join("/", MissionIds)}, ";
        }
    }
}
