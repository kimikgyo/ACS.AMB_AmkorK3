using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class JobLog
    {
        public int Id { get; set; }
        // job info
        public string CallName { get; set; }
        public string LineName { get; set; }
        public string PostName { get; set; }
        public string RobotAlias { get; set; }
        public string RobotName { get; set; }
        public string JobState { get; set; }
        public DateTime CallTime { get; set; }
        public DateTime JobCreateTime { get; set; }
        public DateTime JobFinishTime { get; set; }
        public string JobElapsedTime { get; set; }   //경과시간
        public string MissionNames { get; set; }
        public string MissionStates { get; set; }
        public int TransportCountValue { get; set; } //자재 반송량
        // job result
        public int? ResultCD { get; set; }

        //미사용
        public string CallType { get; set; }
        public string PartCD { get; internal set; }
        public string PartNM { get; internal set; }
        public int? PartOutQ { get; internal set; }
        public int? PartOutP { get; internal set; }
        public int WmsId { get; set; }

    }
}
