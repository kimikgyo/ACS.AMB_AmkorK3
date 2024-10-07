using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ProductNameInfoModel
    {
        //============================================자재 이름 관련 (설정)
        public int Id { get; set; }                     
        //public string RegisterNo22 { get; set; }    
        public int Regiser22Vlaue { get; set; }       //자재 레지스터 번호
        public string RegisterNo { get; set; }        //자재 레지스터 번호
        public int RegisterValue { get; set; }        //자재 레지스터 값
        public string ProductName { get; set; }       //자재 이름
        public int DisplayFlag { get; set; }          //그리드 표시 관련 신호

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"Regiser22Vlaue={Regiser22Vlaue,-5}, " +
                   $"RegisterNo={RegisterNo,-5}, " +
                   $"RegisterValue={RegisterValue,-5}, " +
                   $"ProductName={ProductName,-5}, " +
                   $"DisplayFlag={DisplayFlag,-5}";
        }
    }
}
