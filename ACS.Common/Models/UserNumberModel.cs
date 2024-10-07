using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server.Models
{
   public class UserNumberModel
    {
        //============================================
        public int Id { get; set; }                 
        public string UserNumber { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int CallMissionAuthority { get; set; }
        public int ElevatorAuthority { get; set; }
        public int DisplayFlag { get; set; }
    }
}
