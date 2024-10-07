using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ACSRobotGroupConfigModel
    {
        //==========================================ACS Robot Group 생성(설정)

        public int Id { get; set; }
        public string GroupUse { get; set; }              //Robot Group 사용 유/무
        public string GroupName { get; set; }             //Robot Group 이름
        public int DisplayFlag { get; set; }              //Robot Group 그리드에 표시하는 신호
        
        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"GroupUse={GroupUse,-5}, " +
                   $"GroupName={GroupName,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
