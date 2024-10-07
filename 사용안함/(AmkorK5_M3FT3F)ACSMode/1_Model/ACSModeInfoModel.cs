using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ACSModeInfoModel
    {
        //==========================================모드 확인
        public int Id { get; set; }
        public string Location { get; set; }        //모드 변경할 위치 (예 : Elevator 또는 예: WiseTowerLamp)
        public string ACSMode { get; set; }         //ACS 모드
        public string ElevatorMode { get; set; }    //Elevator 모드
    }
}
