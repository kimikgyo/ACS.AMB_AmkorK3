using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
   public class TabletZoneModel
    {
        public int SEQ { get; set; }                //사용하는 Zone Name Id
        public string ZONENAME { get; set; }        //사용하는 Zone Name
        public DateTime REGDATE { get; set; }       //사용하는 Zone Name 등록시간
        public int DisplayFlag { get; set; }

    }
}
