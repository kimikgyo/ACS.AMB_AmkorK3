using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ElevatorInfoModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string ACSMode { get; set; }
        public string ElevatorMode { get; set; }
        public string FloorIndex { get; set; }
        public string TransportMode { get; set; }
        public string UserNumber { get; set; }

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"Location={Location,-5}, " +
                   $"ACSMode={ACSMode,-5}, " +
                   $"ElevatorMode={ElevatorMode,-5}, " +
                   $"FloorIndex={FloorIndex,-5}, " +
                   $"TransportMode={TransportMode,-5}, " +
                   $"UserNumber={UserNumber,-5}";
        }
    }
}
