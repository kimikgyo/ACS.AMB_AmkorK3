using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ChargeMissionConfigModel
    {
        //=================================================ChargeMission 설정 및 생성(설정)
        public int Id { get; set; }                        //Charge Mission Id 고정     
        public string ChargerGroupName { get; set; }       //충전기 사용 Count가 다르므로 Group따로 설정 
        public string PositionZone { get; set; }           //충전 미션 시작 위치
        public string ChargeMissionUse { get; set; }       //충전 미션 을 (사용 / 미사용)
        public string ChargeMissionName { get; set; }      //충전 미션 이름
        public double StartBattery { get; set; }           //충전 미션 시작 배터리 %
        public double SwitchaingBattery { get; set; }      //충전 진행 중 다른 Robot 과 충전을 바꿀수있는 배터리
        public double EndBattery { get; set; }             //충전 미션 끝나는 배터리 %
        public bool ProductValue { get; set; } = false;    //충전 미션 시작시 자재 확인  유 (true) 무 (false)
        public bool ProductActive { get; set; } = false;   //충전 미션 시작시 자재 사용 유무 
        public string RobotName { get; set; }              //충전 미션 설정 Robot
        public int DisplayFlag { get; set; }               //충전 미션 그리드 표시 신호

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"ChargerGroupName={ChargerGroupName,-5}, " +
                   $"PositionZone={PositionZone,-5}, " +
                   $"ChargeMissionUse={ChargeMissionUse,-5}, " +
                   $"WaitMissionName={ChargeMissionName,-5}, " +
                   $"EnableBattery={StartBattery,-5}, " +
                   $"EnableBattery={SwitchaingBattery,-5}, " +
                   $"EnableBattery={EndBattery,-5}, " +
                   $"ProductValue={ProductValue,-5}, " +
                   $"ProductActive={ProductActive,-5}, " +
                   $"RobotName={RobotName,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
