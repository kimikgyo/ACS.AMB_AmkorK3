using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ErrorHistory
    {
        public int Id { get; set; }
        public DateTime ErrorCreateTime { get; set; }     // Error 발생 시간
        public string RobotName { get; set; }             // Error 발생 Robot 
        public int ErrorCode { get; set; }                // Error Code
        public string ErrorDescription { get; set; }      //
        public string ErrorModule { get; set; }           //
        //ErrorCodeList있는 내용을 가지고 오기 위함
        public string ErrorMessage { get; set; }   // 에러 메세지(영어)
        public string ErrorType { get; set; }      // 에러 타입
        public string Explanation { get; set; }    // 에러 설명(한글 번역 설명)
        public string POSMessage { get; set; }
    }
}
