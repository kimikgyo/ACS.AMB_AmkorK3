using System;
using System.Collections.Generic;

namespace ACS.RobotMap
{
    public class MapReadDto
    {
        public FleetMap Map { get; set; }
        public List<FleetRobot> RobotInfo { get; set; }
        public DateTime CreatedTime { get; set; } // 이 데이터 개체를 생성한 시간
    }
}
