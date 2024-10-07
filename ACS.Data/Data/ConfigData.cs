using System;
using System.Collections.Generic;

namespace INA_ACS_Server
{
    public static class ConfigData
    {
        //MiR 개수 설정
#if HOOK_ONLY
        public const int MiR_MaxNum = 2;
#elif LIFT_ONLY
        public const int MiR_MaxNum = 2;
#else
        //===============================AmkorK5 5월 Set-Up시 사용
        public static int MiR_MaxNum;
        //=========================================================

#endif


        public static int JobConfig_MaxNum;
        public static int ACSRobotGroup_MaxNum;
        public static int WaitMission_MaxNum;
        public static int ChargeMission_MaxNum;
        public static int PosAreaData_MaxNum;
        public static int ACSChargerCount_MaxNum;
        public static int RobotRegistarSync_MaxNum;
        public static int FloorMapID_MaxNum;

        //Amkor K3 추가
        public static int UserEmail_MaxNum;
        public static int UserNumber_MaxNum;
        public static string sErrorSolution_URL1 = string.Empty;
        public static string sErrorSolution_URL2 = string.Empty;
        public static string ErrorSolutionName1 = string.Empty;
        public static string ErrorSolutionName2 = string.Empty;
        public static IList<FleetPositionModel> FleetPositions = null;
        public static string UserNumber = null;   //LogIn 정보 {사번}
        public static string UserName = null;     //LogIn 정보 {사용자이름}
        public static string StartZone = null;    //User가 선택한 StartZone              
        public static int UserElevatorAuthority;  //Elevator {사용권한}
        public static int UserCallAuthority;      //CallSystem {사용권한}


        //User관련 설정
        public static string sAccessLevel = string.Empty;
        public static string sMaint_Password = string.Empty;
        public static string sEngineer_Password = string.Empty;
        public static string sOperator_TrafficSetting = string.Empty;
        public static string sOperator_MissionSetting = string.Empty;
        public static string sOperator_User = string.Empty;
        public static string sMaint_TrafficSetting = string.Empty;
        public static string sMaint_MissionSetting = string.Empty;
        public static string sMaint_User = string.Empty;

        //Fleet IP 설정 관련 변수 
        public static string sFleet_IP_Address_SV = string.Empty;                                    //Fleet IP 설정 설정 변수
        public static string sFleet_ResponseTime = string.Empty;                                     //Fleet 통신 응답시간 설정 변수   

        //Robot IP 설정 관련 변수
        public static string[] RobotIPAddress;      //Robot IP 설정 설정 변수




        public static string RobotResetMissionName = string.Empty;
        public static string jobResetRobotNo = string.Empty;                                           //Reset MiR 번호 설정 변수  
        public static string jobResetRobotName = string.Empty;                                         //Reset MiR 이름 설정 변수  



        static ConfigData()
        {
            //Load(); // 파싱에러 식별이 용이하도록 Program.Main() 에서 호출하도록 변경!
        }

        // 이 클래스가 생성되는 순간에 위의 필드 ConfigData.XXX 데이타를 한번에 모두 로드한다.
        public static void Load()
        {
            try
            {
                ConfigData.MiR_MaxNum = int.Parse(AppConfiguration.GetAppConfig("MiR_MaxNum"));


                //==========ROBOT IP 설정값 읽기
                ConfigData.RobotIPAddress = new string[ConfigData.MiR_MaxNum];

                for (int i = 0; i < ConfigData.MiR_MaxNum; i++)
                {
                    ConfigData.RobotIPAddress[i] = AppConfiguration.GetAppConfig($"RobotIPAddress{i + 1}");
                }
                //==========
                ConfigData.JobConfig_MaxNum = int.Parse(AppConfiguration.GetAppConfig("JobConfig_MaxNum"));
                ConfigData.FloorMapID_MaxNum = int.Parse(AppConfiguration.GetAppConfig("FloorMapID_MaxNum"));
                ConfigData.ACSRobotGroup_MaxNum = int.Parse(AppConfiguration.GetAppConfig("ACSRobotGroup_MaxNum"));
                ConfigData.WaitMission_MaxNum = int.Parse(AppConfiguration.GetAppConfig("WaitMission_MaxNum"));
                ConfigData.ChargeMission_MaxNum = int.Parse(AppConfiguration.GetAppConfig("ChargeMission_MaxNum"));
                ConfigData.PosAreaData_MaxNum = int.Parse(AppConfiguration.GetAppConfig("PosAreaData_MaxNum"));

                ConfigData.ACSChargerCount_MaxNum = int.Parse(AppConfiguration.GetAppConfig("ACSChargerCount_MaxNum"));
                ConfigData.RobotRegistarSync_MaxNum = int.Parse(AppConfiguration.GetAppConfig("RobotRegistarSync_MaxNum"));
                ConfigData.RobotResetMissionName = AppConfiguration.GetAppConfig("RobotResetMissionName");
                //=========================================================

                //초기에 필요한 변수 설정값을 읽는 부분
                AppConfiguration.ConfigDataSetting("sAccessLevel", "Operator"); // 프로그램 구동시 "Operator" 로 설정한다
                ConfigData.sAccessLevel = AppConfiguration.GetAppConfig("sAccessLevel");

                ConfigData.sMaint_Password = AppConfiguration.GetAppConfig("sMaint_Password");
                ConfigData.sEngineer_Password = AppConfiguration.GetAppConfig("sEngineer_Password");
                ConfigData.sOperator_MissionSetting = AppConfiguration.GetAppConfig("sOperator_MissionSetting");
                ConfigData.sOperator_TrafficSetting = AppConfiguration.GetAppConfig("sOperator_TrafficSetting");
                ConfigData.sOperator_User = AppConfiguration.GetAppConfig("sOperator_User");
                ConfigData.sMaint_MissionSetting = AppConfiguration.GetAppConfig("sMaint_MissionSetting");
                ConfigData.sMaint_TrafficSetting = AppConfiguration.GetAppConfig("sMaint_TrafficSetting");
                ConfigData.sMaint_User = AppConfiguration.GetAppConfig("sMaint_User");

                //Fleet 기본 Setting 값 불러오는 부분
                ConfigData.sFleet_ResponseTime = AppConfiguration.GetAppConfig("sFleet_ResponseTime");
                ConfigData.sFleet_IP_Address_SV = AppConfiguration.GetAppConfig("sFleet_IP_Address_SV");

                //Amkor K3에서 추가
                ConfigData.UserEmail_MaxNum = int.Parse(AppConfiguration.GetAppConfig("UserEmail_MaxNum"));
                ConfigData.UserNumber_MaxNum = int.Parse(AppConfiguration.GetAppConfig("UserNumber_MaxNum"));
                ConfigData.sErrorSolution_URL1 = AppConfiguration.GetAppConfig("sErrorSolution_URL1");
                ConfigData.sErrorSolution_URL2 = AppConfiguration.GetAppConfig("sErrorSolution_URL2");
                ConfigData.ErrorSolutionName1 = AppConfiguration.GetAppConfig("ErrorSolutionName1");
                ConfigData.ErrorSolutionName2 = AppConfiguration.GetAppConfig("ErrorSolutionName2");

            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Config File Load Error!");
                throw;
            }


        }

        // 데이터 저장부분은 SystenScreen 에서 개별적으로 처리된다...
        private static void Save()
        {
            throw new NotImplementedException();
            //..... string newValue = "...";
            // .... AppConfiguration.ConfigDataSetting("XXX", newValue);
        }
    }
}
