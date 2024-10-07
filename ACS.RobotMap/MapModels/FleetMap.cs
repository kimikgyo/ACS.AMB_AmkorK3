using System.Collections.Generic;
using System.Text;

namespace ACS.RobotMap
{
    public class FleetMap
    {
        public string Name;
        public string Guid;
        public double OriginX;
        public double OriginY;
        public double OriginTheta;
        public double Resolution;
        public string map;
        public string base_map;
        public System.Drawing.Image Image = null;
        public List<FleetPosition> Positions = new List<FleetPosition>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("name         = {0}\n", Name);
            sb.AppendFormat("guid         = {0}\n", Guid);
            sb.AppendFormat("origin_x     = {0}\n", OriginX);
            sb.AppendFormat("origin_y     = {0}\n", OriginY);
            sb.AppendFormat("origin_theta = {0}\n", OriginTheta);
            sb.AppendFormat("positions    = {0}\n", Positions.Count);
            return sb.ToString();
        }
    }
}