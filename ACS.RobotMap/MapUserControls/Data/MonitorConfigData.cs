using INA_ACS_Server;
using System;
using System.Collections.Generic;

namespace ACS.RobotMap
{
    //internal static class ConfigData
    //{
    //    // 모니터링에서 사용
    //    public static IList<Robot> Robots = null;
    //    public static Dictionary<string, string> DisplayRobotNames;
    //    public static int MonitorPcList_MaxNum = 2;


    //    static ConfigData()
    //    {
    //        string tmp = AppConfiguration.GetAppConfig("RobotNames");
    //        ConfigData.DisplayRobotNames = Helpers.ConvertStringToDictionary(tmp) ?? new Dictionary<string, string>();
    //    }
    //}


    public class MonitorConfigData
    {
        // 모니터링에서 사용
        public IList<Robot> Robots = null;
        public Dictionary<string, string> DisplayRobotNames;
    }

}
