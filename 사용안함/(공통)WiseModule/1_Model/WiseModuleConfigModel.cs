using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server.Models.AmkorK5_M3F_T3F
{
    public class WiseModuleConfigModel
    {
        //================================================= WiseModule 설정 및 생성(DataBase연동)
        public int Id { get; set; }                         
        public string ModuleUse { get; set; }               //WiseModule 을 (사용 / 미사용)
        public string ModuleIpAddress { get; set; }         //WiseModule Ip 설정
        public string ModuleName { get; set; }              //WiseModule 이름
        public string ModuleStatus { get; set; }            //WiseModule 상태    (연결 / 미연결)
        public string ModuleControlMode { get; set; }       //WiseModule Control (자동 / 수동)
        public int ModuleIn_Value { get; set; }             //WiseModule In신호
        public int ModuleOut_Value { get; set; }            //WiseModule Out신호
        public string DisplayName { get; set; }             //WiseModule 화면에 표시하기위한 이름
        public int DisplayFlag { get; set; }                //WiseModule 그리드에 표시하기 위한 신호


        public ServiceData serviceData = new ServiceData();                 
        public class ServiceData
        {
            //========================================================== 모듈을 제어하기위한 신호 데이터 신호데이터는 DB연동안함!(모듈상태 및 On/Off시 갱신)
            public int WriteOutputSignalFlag { get; set; }               //모듈 OutPut 전달 신호
            public int WriteOutputSignalValue { get; set; }              //모듈 OutPut 전달 신호 값
            public DateTime WriteOutputSignalTime { get; set; }          //모듈 OutPut 전달 신호 시간
        }


        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"ModuleUse={ModuleUse,-5}, " +
                   $"ModuleIpAddress={ModuleIpAddress,-5}, " +
                   $"ModuleName={ModuleName,-5}, " +
                   $"ModuleStatus={ModuleStatus,-5}, " +
                   $"ModuleIn_Value={ModuleIn_Value,-5}, " +
                   $"ModuleOut_Vlaue={ModuleOut_Value,-5}, " +
                   $"DisplayName={DisplayName,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}, ";

        }


    }
}
