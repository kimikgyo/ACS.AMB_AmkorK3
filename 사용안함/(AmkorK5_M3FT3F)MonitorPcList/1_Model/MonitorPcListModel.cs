using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server.Models.AmkorK5_M3F_T3F
{
    public class MonitorPcListModel
    {
        //============================================Robot Line 층 관련 정보(설정)
        public int Id { get; set; }
        public string IpAddress { get; set; }         //Monitoring PC Ip
        public string ZoneName { get; set; }          //Monitoring PC 구역 이름
        public bool BcrExist { get; set; }            //바코드 사용 유무
        public int DisplayFlag { get; set; }          //그리드 표시 관련 신호


        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"IpAddress={IpAddress,-5}, " +
                   $"ZoneName={ZoneName,-5}, " +
                   $"BcrExist={BcrExist,-5}" +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
