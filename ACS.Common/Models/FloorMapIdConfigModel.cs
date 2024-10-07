using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
   public class FloorMapIdConfigModel
    {
        //============================================Robot Line 층 관련 정보(설정)
        public int Id { get; set; }                 
        public string FloorIndex { get; set; }         //층설정 (2024.05.14 수정)
        public string FloorName { get; set; }       //층이름설정
        public string MapID { get; set; }           //MapId 설정(RobotMapId를 확인후 설정한다)
        public string MapImageData { get; set; }    //Map Image Data
        public int DisplayFlag { get; set; }        //그리드 표시 관련 신호

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"Floor={FloorName,-5}, " +
                   $"MapID={MapID,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
