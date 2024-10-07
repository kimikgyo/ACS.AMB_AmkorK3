using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        public void RegisterSync()                                              //========== [레지스터 Sync]
        {
            try
            {
                //Robot이 설정한 위치에 있으면 위치에 있는 로봇과 같은 Group 으로 설정된 Robot에게 전부 레지스터를 전송한다.

                //레지스터 싱크 설정 Use 이고 그룹이 None 아닌 상태인 항목만 레지스터를 공유한다
                var RegisterSyncs = uow.RobotRegisterSyncs.Find(r => r.RegisterSyncUse == "Use" && r.ACSRobotGroup != "None" && r.PositionGroup != "None" && r.PositionName != "None" && r.RegisterNo > 0).ToList();

                //2.레지스터 싱크 활성화가 되어있는것
                foreach (var RegisterSync in RegisterSyncs)
                {
                    bool RegisterSyncFlag = false;

                    //3.싱크 활성화 되어있는 목록중에 Robot그룹이 일치한것을 Robot을 검색한다.
                    var GroupRobot = GetActiveRobotsOrderbyDescendingBattery(RegisterSync.ACSRobotGroup);

                    foreach (var robot in GroupRobot)
                    {
                        //RegiaterSyncGroup 포지션 일치하는 로봇을 찾는다.
                        var PositionRobot = uow.PositionAreaConfigs.UpGrade_GroupPOSArea(robot, RegisterSync.PositionGroup).FirstOrDefault();
                        if (PositionRobot != null)
                        {
                            //방향성으로 Reg Sync를 진행한다(Robot Turn 을 못하는 Area 있음!!)
                            if(PositionRobot.PositionAreaName == RegisterSync.PositionName)
                            {
                                if(RegisterSync.PositionName.EndsWith("Left") && robot.Position_Orientation < 0) RegisterSyncFlag = true;
                                else if (RegisterSync.PositionName.EndsWith("Right") && robot.Position_Orientation > 0) RegisterSyncFlag = true;
                                else RegisterSyncFlag = true;
                                break;
                            }
                        }
                    }
                    if (RegisterSyncFlag)
                    {
                        foreach (var robot in GroupRobot)
                        {
                            MiR_Put_Register(robot, RegisterSync.RegisterNo, RegisterSync.RegisterValue);
                        }
                    }
                    else
                    {
                        foreach (var robot in GroupRobot)
                        {
                            MiR_Put_Register(robot, RegisterSync.RegisterNo, 0);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }

    }
}
