using ACS.Common.DTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace INA_ACS_Server
{
    public partial class MainForm
    {
        //========================================================== Log Viewer 팝업 관련
        public UiLogMessageQueue<string> AcsLogMessageQueue = new UiLogMessageQueue<string>();


        //Fleet 연결 확인 설정 변수
        public bool bFleetConnected = false;

        //==========================================================


        //Fleet에서 읽은 자동충전 미션시작 배터리 양
        public string sFleetGetMissionStaet_battery = string.Empty;


        //읽어온 MissionGroup 정보
        public IList<MissionResponse> GetMissionGroups = new List<MissionResponse>(); // =========> 사용x


        //읽어온 Mission 정보
        public IList<MissionResponse> GetMissions = new List<MissionResponse>();


        //읽어온 Mission Schedule 정보 (mir에 의해 실행되는 Mission을 관리하는 객체)
        public IList<MissionSchedulerSimpleResponse> MissionSchedulers = new List<MissionSchedulerSimpleResponse>(); // the list of missions queued in the mission scheduler
        public MissionSchedulerDetailResponse MissionScheduler; // Retrieve the details about the mission scheduler with the specified id

        // job test flag
        public bool jobStepFlag = false;


        //==========================================================


        //Call Button관련 변수
        public int TCP_Connect_Client { get; set; } = 0;                    // =========> 사용x


        //========================================================== POPCALL 팝업 관련
        public ConcurrentQueue<string> SkyNetAlarmMessageQueue = new ConcurrentQueue<string>();


        //========================================================== POPCALL 팝업 관련
        public PopCallMessageQueue<string> PopCallErrorMessageQueue = new PopCallMessageQueue<string>();


        
        //========================================================== WiseModule 출고랙 관련
        public int[] Wise_OutputSignal_Write_Flag = new int[4];
        public string[] sWise_OutputSignal_Write_Ch = new string[4];
        public string[] sWise_OutputSignal_Write_Value = new string[4];


    }
}
