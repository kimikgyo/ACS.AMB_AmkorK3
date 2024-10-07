using System;

namespace INA_ACS_Server
{
    public class MissionsSpecific
    {
        public int No { get; set; }
        public string RobotAlias { get; set; }
        public string RobotName { get; set; }
        public string CallName { get; set; }
        public string CallState { get; set; }
        public DateTime CallTime { get; set; }
        public int Priority { get; set; }
        public string JobSection { get; set; }
        public string Cancel { get; set; }
        public string Move_CallName { get; set; }

        public override string ToString()
        {
            return $"No={No}, " +
                   $"RobotName={RobotName}, " +
                   $"CallName={CallName}, " +
                   $"CallState={CallState}, " +
                   $"CallTime={CallTime}, " +
                   $"Priority={Priority} ";
        }
    }
}
