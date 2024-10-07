using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class PopService
    {
        private readonly static ILog popLogger = LogManager.GetLogger("PopEvent");
        private readonly static ILog partLogger = LogManager.GetLogger("PartEvent");

        private readonly IUnitOfWork uow;

        public static readonly string[] LIFT_TYPES = new string[] { "MC310", "MC311", "MC350", "MC351" };
        public static readonly string[] HOOK_TYPES = new string[] { "MC110", "MC111", "MC112" };

        private static readonly string[] POP_DB1_LINES = new string[] { "MC110", "MC350" };
        private static readonly string[] POP_DB2_LINES = new string[] { "MC112", "MC311", "MC351" };
        private static readonly string[] POP_DB3_LINES = new string[] { "MC111" };
        private static readonly string[] POP_DB4_LINES = new string[] { "MC310" };


        //========================================================== test
        public static string call_group_1 = "";
        public static string call_group_2 = "";
        public static string call_group_3 = "";
        public static string call_group_4 = "";
        //========================================================== test


        public PopService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void Start()
        {
            SetupDailyTimer();

            Task.Run(() => Loop());
        }

        private void Loop()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            PopCall_RequestControl();

            while (true)
            {
                if (sw.ElapsedMilliseconds >= 2000) // 2초마다 처리
                {
                    PopCall_RequestControl();
                    sw.Reset();
                    sw.Start();
                }

                //PopCall_StatusControl_HOOK();
                //PopCall_StatusControl_LIFT();

                Thread.Sleep(1000);
            }
        }

        private void PopCall_RequestControl()
        {
            // POP CALL 항목들을 DB에서 가져와서, 처리안된 CALL 요청에 대해서 JOB 생성한다
            var allTypes = HOOK_TYPES.Concat(LIFT_TYPES).ToArray();

#if HOOK_ONLY
            var popCallInfos = GetPopCallsFromDB(HOOK_TYPES);
#elif LIFT_ONLY
            var popCallInfos = GetPopCallsFromDB(LIFT_TYPES);
#else
            var popCallInfos = GetPopCallsFromDB(allTypes);
#endif

            foreach (PopCallInfo popCallInfo in popCallInfos)
            {
                int popServerId = popCallInfo.PopServerId;
                PopModel popCall = popCallInfo.PopItem;

                string popCallName = $"{popCall.LINE_CD}_{popCall.POST_CD}"; // key
                string popCallType = popCall.RETU_TYPE;
                int popCallAngle = popCall.COMM_ANG;
                DateTime popCallTime = popCall.CREATE_DT;


                // ============= TEST ============== 아래에 해당하는 call만 받아들인다
                if (popCallName.StartsWith("MC1")
                || (PopService.call_group_1.Length > 0 && popCallName.StartsWith(PopService.call_group_1))
                || (PopService.call_group_2.Length > 0 && popCallName.StartsWith(PopService.call_group_2))
                || (PopService.call_group_3.Length > 0 && popCallName.StartsWith(PopService.call_group_3))
                || (PopService.call_group_4.Length > 0 && popCallName.StartsWith(PopService.call_group_4)))
                {
                    Console.WriteLine("CALL OK: " + popCallName); // call accept
                }
                else
                {
                    Console.WriteLine("CALL NG: " + popCallName); // call accept
                    continue; // call deny
                }
                // ============= TEST ==============


                var job = uow.Jobs.GetByCallName(popCallName);
                if (job == null)
                {
                    //JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = popCallName, Extra1 = popServerId, Extra2 = popCallType, Extra3 = popCallAngle, Extra4 = popCallTime });

                    Console.WriteLine(popCallInfo);
                    popLogger.Info($"PopCallReq: {popCallInfo.PopItem}    PopServerId:{popCallInfo.PopServerId}");
                }
            }
        }


        public PartModel GetPartInfo(PopModel popItem)
        {
            if (popItem == null) return null;

            if (popItem.COMM_PO == "Y")
            {
                return uow.PartDB.GetAll().Where(x => x.LINE_CD == popItem.LINE_CD
                                                   && x.POST_CD == popItem.POST_CD
                                                   && x.COMM_PO == popItem.COMM_PO
                                                   && x.COMM_ANG == popItem.COMM_ANG).SingleOrDefault();
            }
            else
            {
                return uow.PartDB.GetAll().Where(x => x.LINE_CD == popItem.LINE_CD
                                                   && x.POST_CD == popItem.POST_CD
                                                   && x.COMM_PO == popItem.COMM_PO).SingleOrDefault();
            }
        }
        public PartModel GetPartInfo2(Job job) // COMM_PO 필드대신 ANGLE>0 여부로 구분하여 처리
        {
            if (job == null) return null;

            var lineCD = job.CallName.Split('_')[0];
            var postCD = Convert.ToInt32(job.CallName.Split('_')[1]);

            if (job.PopCallAngle > 0)
            {
                return uow.PartDB.GetAll().Where(x => x.LINE_CD == lineCD
                                                   && x.POST_CD == postCD
                                                   && x.COMM_PO == "Y"
                                                   && x.COMM_ANG == job.PopCallAngle).SingleOrDefault();
            }
            else
            {
                return uow.PartDB.GetAll().Where(x => x.LINE_CD == lineCD
                                                   && x.POST_CD == postCD
                                                   && x.COMM_PO == "N"
                                                   ).SingleOrDefault();
            }
        }


        class PopCallInfo
        {
            public int PopServerId { get; set; }
            public PopModel PopItem { get; set; }

            public override string ToString()
            {
                return $"PopServerId:{PopServerId},    PopItem:{PopItem}";
            }
        }


        private List<PopCallInfo> GetPopCallsFromDB(string[] targetTypes)
        {
            // POP DB에서 아직 처리안된(FLAG=N) 항목들 가져온다
            var pop1Items = uow.PopDB1.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.LINE_CD)).Where(x => targetTypes.Contains(x.LINE_CD)).ToList();
            var pop2Items = uow.PopDB2.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.LINE_CD)).Where(x => targetTypes.Contains(x.LINE_CD)).ToList();
            var pop3Items = uow.PopDB3.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.LINE_CD)).Where(x => targetTypes.Contains(x.LINE_CD)).ToList();
            var pop4Items = uow.PopDB4.GetAll().Where(x => !string.IsNullOrWhiteSpace(x.LINE_CD)).Where(x => targetTypes.Contains(x.LINE_CD)).ToList();



            var popCallInfos = new List<PopCallInfo>();
            if (pop1Items != null) popCallInfos.AddRange(pop1Items.Select(item => new PopCallInfo { PopServerId = 1, PopItem = item }));
            if (pop2Items != null) popCallInfos.AddRange(pop2Items.Select(item => new PopCallInfo { PopServerId = 2, PopItem = item }));
            if (pop3Items != null) popCallInfos.AddRange(pop3Items.Select(item => new PopCallInfo { PopServerId = 3, PopItem = item }));
            if (pop4Items != null) popCallInfos.AddRange(pop4Items.Select(item => new PopCallInfo { PopServerId = 4, PopItem = item }));

            return popCallInfos.OrderBy(x => x.PopItem.CREATE_DT).ToList(); // pop call 시간순 정렬
        }


        public PopRepository SelectPopDB_ByNo(int popServerNo/*, string lineCode*/)
        {
            //PopRepository selectedPopDB = null;
            //const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=HYUNDAI_TRANSYS_Gyeongju;Integrated Security=True;Connect Timeout=30;";
            //using (var con = new SqlConnection(connectionString))
            //{
            //    var serverInfo = con.QueryFirst<(int Id, int PopServerNo)>("select Id, PopServerNo from PopServerInfos Where LineCD=@lineCD", new { lineCode });
            //    if (serverInfo.PopServerNo == 1) selectedPopDB = uow.PopDB1;
            //    if (serverInfo.PopServerNo == 2) selectedPopDB = uow.PopDB2;
            //    if (serverInfo.PopServerNo == 3) selectedPopDB = uow.PopDB3;
            //    if (serverInfo.PopServerNo == 4) selectedPopDB = uow.PopDB4;
            //}

            //PopRepository selectedPopDB = null;
            //PopServerInfo popServerInfo = popServerInfos.FirstOrDefault(x => x.LineCD == lineCode);
            //if (popServerInfo.PopServerNo == 1) selectedPopDB = uow.PopDB1;
            //if (popServerInfo.PopServerNo == 2) selectedPopDB = uow.PopDB2;
            //if (popServerInfo.PopServerNo == 3) selectedPopDB = uow.PopDB3;
            //if (popServerInfo.PopServerNo == 4) selectedPopDB = uow.PopDB4;

            PopRepository selectedPopDB = null;
            if (popServerNo == 1) selectedPopDB = uow.PopDB1;
            if (popServerNo == 2) selectedPopDB = uow.PopDB2;
            if (popServerNo == 3) selectedPopDB = uow.PopDB3;
            if (popServerNo == 4) selectedPopDB = uow.PopDB4;

            return selectedPopDB;
        }





        // =================================================

        System.Timers.Timer timer;

        // 매일 지정된 시간에 POP DB에서 오래된 데이터를 삭제한다
        void SetupDailyTimer()
        {
            DateTime nowTime = DateTime.Now;
            DateTime oneAmTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 6, 0, 0, 0); // 6시
            if (nowTime > oneAmTime)
                oneAmTime = oneAmTime.AddDays(1);

            double tickTime = (oneAmTime - nowTime).TotalMilliseconds;
            timer = new System.Timers.Timer(tickTime);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 타이머 중지
            timer.Stop();

            // 7일전 데이터 모두 삭제
            uow.PopDB1.DeleteOldData();
            uow.PopDB2.DeleteOldData();
            uow.PopDB3.DeleteOldData();
            uow.PopDB4.DeleteOldData();

            // 타이머 다시 설정
            SetupDailyTimer();
        }

        // =================================================
    }
}
