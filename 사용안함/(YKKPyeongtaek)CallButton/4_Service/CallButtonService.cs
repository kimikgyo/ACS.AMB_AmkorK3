using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class CallButtonService
    {
        private readonly ILog logger;
        private readonly IUnitOfWork uow;


        public CallButtonService(IUnitOfWork uow)
        {
            this.logger = LogManager.GetLogger("Event");
            this.uow = uow;

            // 콜버튼 이벤트 핸들러 추가
            foreach (var callButton in uow.CallButtons.GetAll())
            {
                callButton.MissionCallEvent += MissionCallRequest;
                callButton.MissionCallCancelEvent += MissionCancelRequest;
            }
        }

        private void MissionCallRequest(object sender, EventArgs e) // 미션 추가 요청
        {
            var callButton = (CallButton)sender;
            var callButtonName = callButton.ButtonName;

            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = callButtonName });
        }

        private void MissionCancelRequest(object sender, EventArgs e) // 미션 삭제 요청
        {
            var callButton = (CallButton)sender;
            var callButtonName = callButton.ButtonName;

            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = callButtonName });
        }

        public void Start()
        {
            Task.Run(() => Loop());
        }

        private async void Loop()
        {
            while (true)
            {
                foreach (CallButton btn in uow.CallButtons.GetAll())
                {
                    //await Task.Delay(500); // <=========================== 노드간 통신 간격
                    await Task.Delay(1); // <=========================== 노드간 통신 간격

                    // 콜버튼 데이타 송수신 처리
                    bool recv_good = await btn.SendRecvAsync();

                    btn.AccessElapsedTime = (DateTime.Now - btn.LastAccessTime).TotalSeconds;

                    if (recv_good)
                    {
                        btn.LastAccessTime = DateTime.Now;
                    }

                    //Console.WriteLine(btn.AccessElapsedTime);

                    // 버튼상태 갱신 (경과시간을 초과하면 타임아웃처리한다)
                    //if (recv_good && btn.AccessElapsedTime <= 10.0)  // <============================== 콜버튼 타임아웃 시간
                    if (recv_good)  // <============================== 콜버튼 타임아웃 시간
                        btn.ConnectionState = CallButtonConnectionState.Connect;
                    else
                        btn.ConnectionState = CallButtonConnectionState.Disconnect;


                    //uow.CallButtons.Update(btn);

                }

               // await Task.Delay(1); // <=========================== 루프 통신 딜레이
            }
        }
    }
}
