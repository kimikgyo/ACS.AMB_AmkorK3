using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class RegisterInfoModel
    {
        //=================================================Register 설명
        public int Id { get; set; }
        public string ACSRobotGroup { get; set; }
        public int RegisterNumber { get; set; }
        public int RegisterValue { get; set; }
        public string RegisterInfoMessge { get; set; }
        public int DisplayFlag { get; set; }
    }
}
