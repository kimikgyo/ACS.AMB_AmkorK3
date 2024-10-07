using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class PositionAreaConfig
    {
        //=============================================================//PositionArea(설정)
                                                                       
        public int Id { get; set; }                                    
        public string ACSRobotGroup { get; set; }                      //Position Area 대한 그룹
        public string PositionAreaUse { get; set; }                    //Position Area 사용 / 미사용
        public string PositionAreaFloorName { get; set; }              //Position Area 층이름
        public string PositionAreaFloorMapId { get; set; }             //Position Area 층에대한 Map Id
        public string PositionAreaName { get; set; }                   //Position Area 이름
        public string PositionAreaX1 { get; set; }                     //Position Area X1
        public string PositionAreaX2 { get; set; }                     //Position Area X2
        public string PositionAreaX3 { get; set; }                     //Position Area X3
        public string PositionAreaX4 { get; set; }                     //Position Area X4
        public string PositionAreaY1 { get; set; }                     //Position Area Y1
        public string PositionAreaY2 { get; set; }                     //Position Area Y2
        public string PositionAreaY3 { get; set; }                     //Position Area Y3
        public string PositionAreaY4 { get; set; }                     //Position Area Y4
        public bool PositionWaitTimeLog { get; set; }                  //Position 대기 시간을 Log 기록 [체크되어 있는 항목만]

        public int DisplayFlag { get; set; }                           //Position Area 그리드에 표기하기 위한 신호

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"ACSRobotGroup={ACSRobotGroup,-5}, " +
                   $"PositionAreaUse={PositionAreaUse,-5}, " +
                   $"PositionAreaName={PositionAreaName,-5}, " +
                   $"PositionAreaFloorName={PositionAreaFloorName,-5}, " +
                   $"PositionAreaFloorMapId={PositionAreaFloorMapId,-5}, " +
                   $"PositionAreaX1={PositionAreaX1,-5}, " +
                   $"PositionAreaX2={PositionAreaX2,-5}, " +
                   $"PositionAreaX3={PositionAreaX3,-5}, " +
                   $"PositionAreaX4={PositionAreaX4,-5}, " +
                   $"PositionAreaY1={PositionAreaY1,-5}, " +
                   $"PositionAreaY2={PositionAreaY2,-5}, " +
                   $"PositionAreaY3={PositionAreaY3,-5}, " +
                   $"PositionAreaY4={PositionAreaY4,-5}, " +
                   $"PositionWaitTimeLog={PositionWaitTimeLog,-5}, " +
                   $"DisplatFlag={DisplayFlag,-5}";
        }

    }
}
