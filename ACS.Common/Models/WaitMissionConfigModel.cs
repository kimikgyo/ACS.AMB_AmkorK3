using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
   public class WaitMissionConfigModel
    {
        //=================================================Wait Mission 설정 및 생성(설정)
        public int Id { get; set; }                        //Wait Mission Id 고정     
        public string PositionZone { get; set; }           //Wait 미션 End 위치
        public string WaitMissionUse { get; set; }         //Wait 미션 을 (사용 / 미사용)
        public string WaitMissionName { get; set; }        //Wait 미션 이름
        public double EnableBattery { get; set; }          //Wait 미션 시작 배터리 %
        public bool ProductValue { get; set; } = false;    //Wait 미션 시작시 자재 확인  유 (true) 무 (false)
        public bool ProductActive { get; set; } = false;   //Wait 미션 시작시 자재 사용 유무 
        public string RobotName { get; set; }              //Wait 미션 설정 Robot
        public int DisplayFlag { get; set; }               //Wait 미션 그리드 표시 신호
                                                           
        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"PositionZone={PositionZone,-5}, " +
                   $"WaitMissionUse={WaitMissionUse,-5}, " +
                   $"WaitMissionName={WaitMissionName,-5}, " +
                   $"EnableBattery={EnableBattery,-5}, " +
                   $"ProductValue={ProductValue,-5}, " +
                   $"ProductActive={ProductActive,-5}, " +
                   $"RobotName={RobotName,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
