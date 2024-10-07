using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
   public class FleetPositionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string TypeID { get; set; }
        public string MapID { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double Orientation { get; set; }
    }
}
