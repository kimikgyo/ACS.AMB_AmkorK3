using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class RobotRegisterSyncModel
    {
        public int Id { get; set; }
        public string RegisterSyncUse { get; set; }                //레지스터 싱크 사용 유무
        public string PositionGroup { get; set; }                  //레지스터 싱크 사용 포지션 그룹
        public string PositionName { get; set; }                   //레지스터 싱크 사용 위치
        public string ACSRobotGroup { get; set; }                  //레지스터 싱크 사용 그룹
        public int RegisterNo { get; set; }                        //레지스터 번호
        public int RegisterValue { get; set; }                     //레지스터 공유 값
        public int DisplayFlag { get; set; }                       //레지스터 싱크 그리드에 표기하기위한 신호

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"RegisterSyncUse={RegisterSyncUse,-5}, " +
                   $"RegisterSyncUse={PositionGroup,-5}, " +
                   $"PositionName={PositionName,-5}, " +
                   $"ACSRobotGroup={ACSRobotGroup,-5}, " +
                   $"RegisterNo={RegisterNo,-5}, " +
                   $"RegisterValue={RegisterValue,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
