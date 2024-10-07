using System.Collections.Generic;

namespace INA_ACS_Server
{
    public class JobHistoryChartConfigFilter
    {
        public List<string> RobotNames { get; set; } = new List<string>();
        public List<string> RobotAlias { get; set; } = new List<string>();
        public List<string> StartPos { get; set; } = new List<string>();
        public List<string> EndPos { get; set; } = new List<string>();
    }

}
