using System;
using System.Text;

namespace ACS.RobotMap
{
    public class FleetPosition
    {
        public string Name;
        public string Guid;
        public string TypeID;
        public string MapID;
        public double PosX;
        public double PosY;
        public double Orientation;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("\ttype_id    = {0}\n", TypeID);
            sb.AppendFormat("\tpos_x      = {0}\n", PosX);
            sb.AppendFormat("\tpos_y      = {0}\n", PosY);
            sb.AppendFormat("\tOrientation= {0}\n", Orientation);
            sb.AppendLine();
            return sb.ToString();
        }
    }
}