using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class TabletZonePalletModel
    {
        public int SEQ { get; set; }            //Tablet Pallet Id
        public string ZONENAME { get; set; }    //Tablet Pallet Zone 이름 설정
        public int PALLETNO { get; set; }       //Tablet Pallet Zone사용하는 Pallet번호
        public DateTime REGDATE { get; set; }   //Tablet Pallet Zone 사용하는 Pallet 등록 시간
        public int DisplayFlag { get; set; }

    }
}
