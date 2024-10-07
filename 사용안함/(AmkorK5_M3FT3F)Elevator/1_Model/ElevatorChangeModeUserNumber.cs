using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ElevatorChangeModeUserNumber
    {
        //Elevator Mode 변경할수있는 사번 입력
        public int Id { get; set; }                 
        public string UserNumber { get; set; }      //사번

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"UserNumber {UserNumber}, ";
        }
    }
}
