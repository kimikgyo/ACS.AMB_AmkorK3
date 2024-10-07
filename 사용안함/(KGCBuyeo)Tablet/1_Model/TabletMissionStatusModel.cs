using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class TabletMissionStatusModel
    {
        public int SEQ { get; set; }            // Id
        public int JobId { get; set; }
        public string CALLNAME { get; set; }    // 미션 Call Name
        public string MESSIONSEQ { get; set; }  // Tablet 기존UI 와 겹치지 않게 하기위하여 Tablet Program에서 변경요청함
        public string LOCATION { get; set; }    // 
        public string CALLFLAG { get; set; }        // 상태값(Wait)    
        public string CANCELFLAG { get; set; }      // 상태값(Cancel)    
        public DateTime REGDATE { get; set; }   // 미션등록시 타임
    }
}
