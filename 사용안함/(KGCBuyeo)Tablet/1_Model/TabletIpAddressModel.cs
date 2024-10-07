using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class TabletIpAddressModel
    {
        public int Seq { get; set; }            //Tablet IpAddress Id
        public string IP { get; set; }   //Tablet IpAddress
        public string ZONENAME { get; set; }    //Tablet 해당되는 ZoneName
        public int DisplayFlag { get; set; }
    }
}
