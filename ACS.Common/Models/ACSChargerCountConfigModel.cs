using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ACSChargerCountConfigModel
    {

        public int Id { get; set; }
        public string ChargerCountUse { get; set; }          //충전기 사용 유/무
        public string RobotGroupName { get; set; }           //충전기 사용할 그룹 이름
        public string FloorName { get; set; }                //충전기 위치 층이름
        public string FloorMapId { get; set; }               //충전기 위치 층Map ID
        public string ChargerGroupName { get; set; }         //충전기 Count대한 그룹(같은 Map상에 Count수량이 다르기때문에 ChargerGroup설정해주어야함)
        public int ChargerCount { get; set; }                //충전기 수량
        public int ChargerCountStatus { get; set; }          //Robot Group 충전기 수량 상태(충전미션 전송시 사용)
        public int DisplayFlag { get; set; }                 //충전기Count 그리드에 표시하는 신호

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"ChargerUse={ChargerCountUse,-5}, " +
                   $"RobotGroupName={RobotGroupName,-5}, " +
                   //$"FloorName={FloorName,-5}, " +
                   //$"FloorMapId={FloorMapId,-5}, " +
                   $"ChargerGroupName={ChargerGroupName,-5}, " +
                   $"ChargerCountStatus={ChargerCountStatus,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
