using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ErrorCodeListModel
    {
        public int Id { get; set; }
        public int ErrorCode { get; set; }         // 에러 코드   
        public string ErrorMessage { get; set; }   // 에러 메세지(영어)
        public string ErrorType { get; set; }      // 에러 타입
        public string Explanation { get; set; }    // 에러 설명(한글 번역 설명)
        public bool MailSend { get; set; } = false;  // 체크 되어있는 항목만 메일전송한다
    }
}
