using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class WiseTowerLampConfigModel
    {
        //========================================================= WiseTowerLamp 설정 및 생성(DataBase연동)
        public int Id { get; set; }
        public string NameSetting { get; set; }                    //Module 이름
        public string PositionZoneSetting { get; set; }            //Module TowerLamp 신호를 주기위한 포지션
        public string ControlSetting { get; set; }                 //Module 자동 Or 수동
        public string TowerLampUseSetting { get; set; }            //Module 사용 미사용
        public string IpAddressSetting { get; set; }               //Module 모듈 Ip
        public string DisplayNameSetting { get; set; }             //Module 화면에 표시할 모듈이름
        public int OperationtimeSetting { get; set; }              //Module 진행 설정 시간
        public bool ProductValueSetting { get; set; } = false;     //Module 자재 유 Or 무
        public bool ProductActiveSetting { get; set; } = false;    //Module 자재 유무 사용 Or 미사용
        public string productName { get; set; }                    //자재 설정 이름
        public int DisplayFlag { get; set; }                       //Module 설정 그리드에 표기하기위한 신호




        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"Name={NameSetting,-5}, " +
                   $"PositionZone={PositionZoneSetting,-5}, " +
                   $"Control={ControlSetting,-5}, " +
                   $"TowerLampUse={TowerLampUseSetting,-5}, " +
                   $"IpAddress={IpAddressSetting,-5}, " +
                   $"DisplayName={DisplayNameSetting,-5}, " +
                   $"OpraOperationtimetiontime={OperationtimeSetting,-5}, " +
                   $"ProductValue={ProductValueSetting,-5}, " +
                   $"ProductActive={ProductActiveSetting,-5}, " +
                   $"productName={productName,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}, ";

        }

        public ServiceData serviceData = new ServiceData();                 //모듈을 제어하기위한 신호 데이터 신호데이터는 DB연동안함!(모듈상태 및 On/Off시 갱신)
        public class ServiceData
        {
            public string Status { get; set; }                               //모듈 상태 (Connect Or DisConnect)
            public bool TowerLampOffTimerCompletSignal { get; set; }         //Off Timer 완료 신호
            public int Module_InValue { get; set; }                          //모듈 InPut
            public int Module_OutValue { get; set; }                         //모듈 OutPut
            public int WriteOutputSignalFlag { get; set; }                   //모듈 OutPut 전달 신호
            public int WriteOutputSignalValue { get; set; }                  //모듈 OutPut 전달 신호 값
            public DateTime WriteOutputOnSignalTime { get; set; }            //모듈 OutPut 전달 신호 시간
            public DateTime WriteOutputOffSignalTime { get; set; }           //모듈 OutPut 전달 신호 시간

            public List<string> RobotNames { get; set; } = new List<string>();
            public List<string> TowerLampOffTimerCompletSignals { get; set; } = new List<string>();
        }




    }

}
