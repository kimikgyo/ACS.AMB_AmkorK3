using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
   public class SkyNetModel
    {
        public int Id { get; set; }                     //Database Key
        public string SkyNetMode { get; set; }          //SkyNet Database 상태 비교 하기위한 SkyNetMode
        public string Linecode { get; set; }            //SkyNet Database 상태 비교 하기위한 LineCode
        public string Processcode { get; set; }         //SkyNet Database 상태 비교 하기위한 Processcode
        public string RobotName { get; set; }           //SkyNet Database 상태 비교 하기위한 RobotName
        public string RobotState { get; set; }          //SkyNet Database 상태 비교 하기위한 RobotState

        public string MissionName { get; set; }

        public string MissionState { get; set; }

        public override string ToString()
        {
            return $"id={Id,-5}, " +
                   $"SkyNetMode = {SkyNetMode,-5}, " +
                   $"Linecode={Linecode,-5}, " +
                   $"Processcode = {Processcode,-10}, " +
                   $"RobotName={RobotName,-10}, " +
                   $"RobotState={RobotState,-15} " +
                   $"MissionName={MissionName,-10}" +
                   $"MissionState ={ MissionState,-10}";

        }
    }
}
