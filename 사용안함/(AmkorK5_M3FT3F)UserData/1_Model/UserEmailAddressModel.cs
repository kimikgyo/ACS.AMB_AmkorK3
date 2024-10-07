using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class UserEmailAddressModel
    {
        //사용자 E-Mail등록 
        public int Id { get; set; }
        public string EmailUse { get; set; }            //EMail사용 미사용
        public string UserEmailAddress { get; set; }    //사용자 E-Mail
        public int DisplayFlag { get; set; }            //UI 표기하기위한 변수
    }
}
