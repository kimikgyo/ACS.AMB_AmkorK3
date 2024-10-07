using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public enum ElevatorAGVMode
    {
        MiRControlMode = 0,
        MiRUnControlMode = 1
    }

    public class ElevatorStateModule
    {
        //========================================================= Elevator Mission 정보
        public int Id { get; set; }
        public string RobotAlias { get; set; }
        public string RobotName { get; set; }                       //Elevator 사용중인 Robot 이름
        public string ElevatorRobotState { get; set; }                   //Elevator 사용중인 Robot 상태
        public string ElevatorMissionName { get; set; }             //Robot 목적지 확인하기 위함
        public string ElevatorSourceFloor { get; set; }                      //Robot 출발 층
        public string ElevatorDestFloor { get; set; }                       //Robot 도착 층
    }
}
