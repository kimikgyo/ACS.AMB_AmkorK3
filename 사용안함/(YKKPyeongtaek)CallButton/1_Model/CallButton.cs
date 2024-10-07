using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using log4net;
using System.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace INA_ACS_Server
{
    /// 응답 메시지 포맷 : [STX]01RD00[ETX]
    ///                        |  ||
    ///                        |  |+--> Button2 (cancel)
    ///                        |  +---> Button1 (call)
    ///                        +------> node no


    public enum CallButtonConnectionState
    {
        None,
        Disconnect,
        Connect,
    }

    public class CallButton
    {
        enum LampState
        {
            OFF,
            ON,
            BLINK,
            RESET,
        }

        public ILog logger;
        private int button1_recvState;          // 아두이노 to PC (버튼상태)
        private int button2_recvState;
        private LampState button1_sendState;    // PC to 아두이노 (램프상태)
        private LampState button2_sendState;

        // 기본정보
        public int Id { get; set; }
        public int ButtonIndex { get; set; }
        public string ButtonName { get; set; }
        public string IpAddress { get; set; }
        public int MissionCount { get; set; }
        // 상태정보
        //public string MissionStateText { get; set; } = "";
        private string _missionStateText = "";
        public string MissionStateText
        {
            get { return _missionStateText; }
            set { /*Console.WriteLine($"CallButton {ButtonName} MissionStateText changed {_missionStateText} => {value}");*/ _missionStateText = value; }
        }
        public CallButtonConnectionState ConnectionState { get; set; } = CallButtonConnectionState.Disconnect;
        public DateTime LastAccessTime { get; set; } = DateTime.Now;    // 최근 통신 성공 시간
        public double AccessElapsedTime { get; set; } = 0.0d;           // 최근 통신 성공 이후 경과시간(초)

        public event EventHandler MissionCallEvent;
        public event EventHandler MissionCallCancelEvent;


        public CallButton()
        {
            //this.logger = LogManager.GetLogger($"CallButton{ButtonIndex}"); // 여기서 제대로 설정안되서 버튼생성부분으로 이동. (ButtonIndex가 생성자 실행후 설정되므로.)
        }

        public override string ToString()
        {
            return $"{nameof(CallButton)} : {ButtonIndex}, {ButtonName}, {ConnectionState}";
        }





        // 콜버튼에 보낼 데이터 생성
        private byte[] MakeSendingData()
        {
            string sendMsg = string.Format("{0:00}WD{1}{2}", ButtonIndex, (int)button1_sendState, (int)button2_sendState);

            byte[] tempBuff = Encoding.ASCII.GetBytes(sendMsg);
            byte[] sendData = null;
            using (var ms = new MemoryStream())
            {
                ms.WriteByte(0x02);
                ms.Write(tempBuff, 0, tempBuff.Length);
                ms.WriteByte(0x03);
                sendData = ms.ToArray();
            }
            return sendData;
        }

        // 콜버튼에 데이터 송수신 처리
        public async Task<bool> SendRecvAsync()
        {
            Log("ip = " + IpAddress);

            byte[] sendData = MakeSendingData();

            try
            {
                using (var client = new TcpClient())
                {
                    var cancelTask = Task.Delay(100); // <=========================== 연결타임아웃 시간
                    var connectTask = client.ConnectAsync(IpAddress, 5555);

                    //double await so if cancelTask throws exception, this throws it
                    await await Task.WhenAny(connectTask, cancelTask);
                    if (cancelTask.IsCompleted)
                    {
                        //throw new TimeoutException("connection time out");
                        Log("SendRecvAsync() : connection time out");
                        return false;
                    }

                    using (var stream = client.GetStream())
                    {
                        String response = String.Empty;
                        byte[] recvBuff = new Byte[256];
                        int recvLength = 0;

                        stream.ReadTimeout = 1000; // <=========================== 수신타임아웃 시간

                        // send message
                        stream.Write(sendData, 0, sendData.Length);
                        string message = Encoding.ASCII.GetString(sendData);
                        Console.WriteLine($"Sent : {message}");
                        Log($"Sent: {message}");

                        // recv response
                        while ((recvLength = stream.Read(recvBuff, 0, recvBuff.Length)) != 0)
                        {
                            response += Encoding.ASCII.GetString(recvBuff, 0, recvLength);

                            if (response.IndexOf(Constants.ETX) != -1) // ETX 수신시 루프 탈출
                                break;
                        }
                        Console.WriteLine($"Recv : {response}");
                        Log($"Recv: {response}");

                        if (response.Length > 0)
                            return RecvDataHandler(response);
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is IOException)) Debug.Print(ex.StackTrace); // IOException(여기서는 수신타임아웃일 경우)은 디버그메시지출력x
                Log(string.Format("\nMessage ---\n{0}", ex.InnerException?.Message ?? ex.Message));
                Log(string.Format("\nStackTrace ---\n{0}", ex.StackTrace));
            }
            return false;
        }

        // 수신된 데이터에 따른 처리
        private bool RecvDataHandler(string response)
        {
            //// 응답 메시지 유효성 체크
            if (response.Length != 8) return false; // 길이 체크
            if (response[0] != Constants.STX) return false;                          // STX
            if (response.Substring(1, 2) != $"{ButtonIndex:00}") return false;   // 01 (node no)
            if (response.Substring(3, 2) != "RD") return false;             // RD
            int value1 = response[5] - '0';                                 // button1 상태
            int value2 = response[6] - '0';                                 // button2 상태
            if (response[7] != Constants.ETX) return false;                          // ETX
            if (value1 < 0 || value1 > 9) return false; // 유효숫자인가?
            if (value2 < 0 || value2 > 9) return false; // 유효숫자인가?

            // 상태 정보  ====================
            int button1_recvState_new = value1; // Button1 (CALL)
            int button2_recvState_new = value2; // Button2 (CALL-CANCEL)


            // 버튼1 처리 ====================
            if (button1_recvState_new == 1 && button1_recvState == 0) // MISSION CALL 요청
            {
                if (string.IsNullOrEmpty(MissionStateText) == true || MissionStateText == "Waiting" || MissionStateText == "deleted")
                {
                    Console.WriteLine(MissionStateText);
                    Log2("MISSION CALL REQUEST!");
                    MissionCallEvent?.Invoke(this, null);
                    button1_sendState = LampState.OFF;    // 미션대기중   (램프 점멸)
                }
            }
            else if ((button1_recvState_new == 1 && button1_recvState == 1) || string.IsNullOrEmpty(MissionStateText) == false)
            //CallButton 꺼졋다가 켜졌을시 램프 상태 값을받기위한 조건 추가
            {
                // 미션처리순서:
                // 콜버튼클릭 => 미션생성,Waiting => 미션전송,Sending => Pending => Executing => Done => 미션완료,삭제

                switch (MissionStateText)
                {
                    case "Waiting": button1_sendState = LampState.BLINK; break;     // 미션대기중   (램프 점멸)
                    case "Sending": button1_sendState = LampState.BLINK; break;     // 미션대기중   (램프 점멸)
                    case "Pending": button1_sendState = LampState.ON; break;        // 미션실행중   (램프 점등)
                    case "Executing": button1_sendState = LampState.ON; break;      // 미션실행중   (램프 점등)
                    case "Done": // 미션실행완료 (램프 소등. 아두이노 시퀀스 초기화)
                        button1_sendState = LampState.RESET; //미션 완료 모든 램프 상태 초기화
                        button2_sendState = LampState.RESET; //미션 완료 모든 램프 상태 초기화
                        MissionStateText = "";
                        break;
                    // 미션수행중 에러시 : 아래는 에러미션을 전송하기전 상태이므로 CALL 요청 받지 않도록 ON으로 설정
                    case "Aborted": button1_sendState = LampState.ON; break;        // 미션실행중   (램프 점등)
                    case "Invalid": button1_sendState = LampState.ON; break;        // 미션실행중   (램프 점등)
                    default:
                        Console.WriteLine($"콜버튼 {ButtonName} : {MissionStateText}"); break;
                }

            }
            else
            {
                button1_sendState = LampState.OFF; // init
            }

            // 버튼2 처리 ====================
            if (button2_recvState_new == 1 && button2_recvState == 0) // MISSION CALL-CANCEL 요청
            {
                if (string.IsNullOrEmpty(MissionStateText) == false)
                {
                    Console.WriteLine(MissionStateText);
                    Log2("MISSION CALL-CANCEL REQUEST!");
                    MissionCallCancelEvent?.Invoke(this, null);
                    button2_sendState = LampState.RESET; // 미션콜 취소 완료 램프 상태 초기화
                    MissionStateText = "deleted";
                }
            }
            else
            {
                button2_sendState = LampState.OFF; // init
            }

            // 버튼 상태 저장  ====================
            button1_recvState = button1_recvState_new;
            button2_recvState = button2_recvState_new;
            //if (button1_sendState == LampState.OFF || button1_sendState == LampState.RESET) button1_recvState = button1_recvState_new;
            //if (button2_sendState == LampState.OFF || button2_sendState == LampState.RESET) button2_recvState = button2_recvState_new;
            return true;
        }

        private void Log(string comment)
        {
            logger.InfoFormat("{0:00}, {1}", ButtonIndex, comment);
        }

        private void Log2(string comment)
        {
            logger.InfoFormat("{0:00}, {1}", ButtonIndex, new string('*', 30) + comment);
        }
    }
}
