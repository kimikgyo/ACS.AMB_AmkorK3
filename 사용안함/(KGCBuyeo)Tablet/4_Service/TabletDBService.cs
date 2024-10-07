using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class TabletDBService
    {
        private readonly static ILog TabletLogger = LogManager.GetLogger("TabletEvent");
        private readonly IUnitOfWork uow;

        public TabletDBService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public void Start()
        {
            Task.Run(() => Loop());
        }

        private void Loop()
        {
            Stopwatch tabletDbSw = new Stopwatch();
            tabletDbSw.Start();
            TabletCall_RequestControl();
            while (true)
            {
                try
                {
                    if (tabletDbSw.ElapsedMilliseconds >= 2000) // 2초마다 처리
                    {
                        TabletCall_RequestControl();
                        tabletDbSw.Reset();
                        tabletDbSw.Start();
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    LogExceptionMessage(ex);
                }
            }
        }

        private void TabletCall_RequestControl()
        {
            //1.Tablet 전체 데이터를 가지고온다
            var tabletMissionStatusLists = uow.TabletMissionStatus.GetAll().ToList();

            //2.중복데이터 삭제 
            //전체 데이터중 JobId가 있는 data List 찾는다.
            var JobIdExistLists = tabletMissionStatusLists.Where(j => j.JobId > 0).ToList();

            //전체 데이터중 JobId가 있는 데이터와 일치한 List를 찾는다
            var JobIdExistOverlapLists = tabletMissionStatusLists.Where(l =>
                                            JobIdExistLists.Count(t => l.SEQ != t.SEQ && l.JobId != t.JobId && l.CALLNAME == t.CALLNAME) != 0).ToList();

            //전체 데이터중 JobId가 있는 데이터와 일치한 List중 jobId 없는 List 는 삭제한다
            foreach (var JobIdExistOverlapList in JobIdExistOverlapLists)
            {
                uow.TabletMissionStatus.Remove(JobIdExistOverlapList);
                TabletLogger.Info($"OverLampData, TabletId : {JobIdExistOverlapList.SEQ}, JobId : {JobIdExistOverlapList.JobId}, Name : {JobIdExistOverlapList.CALLNAME},  CallFlag : {JobIdExistOverlapList.CALLFLAG}, CancelFlag : {JobIdExistOverlapList.CANCELFLAG}");
            }

            var overlapDataExcept = tabletMissionStatusLists.Except(JobIdExistOverlapLists).ToList();
            if (overlapDataExcept.Count > 0)
            {
                AddjobSchedule(overlapDataExcept);
                DeletejobSchedule(overlapDataExcept);
            }

            //2.JobId 없는경우 Job 스케줄 등록
            void AddjobSchedule(List<TabletMissionStatusModel> tabletMissionStatusList)
            {
                //Tablet 전체 데이터중 CallFlag ==Wait 이고 Cancle 데이터가 없는것을 가지고온다
                foreach (var TabletDataCall in tabletMissionStatusList.Where(x => x.JobId == 0 && x.CALLFLAG == "wait" && string.IsNullOrEmpty(x.CANCELFLAG)).ToList())
                {
                    var overlapChecking = JobIdExistLists.FirstOrDefault(x => x.CALLNAME == TabletDataCall.CALLNAME);

                    if (overlapChecking == null)
                    {
                        //JobConfig 에서 Talbet데이터 CallName 있는지 확인한다
                        var jobCfg = uow.JobConfigs.GetByCallName(TabletDataCall.CALLNAME);

                        //JobConfig에 있고 JobConfig 해당데이터가 Use 인것을 확인한다
                        if (jobCfg != null && jobCfg.JobConfigUse == "Use" && jobCfg.jobCallSignal != "None")
                        {
                            //Job Add 하는 함수 (스케줄에 등록할 CallName , Tablet DB등록되어 있는 ID)
                            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = TabletDataCall.CALLNAME, Extra1 = TabletDataCall.SEQ });
                            TabletLogger.Info($"TabletCall[OK], TabletId : {TabletDataCall.SEQ}, JobId : {TabletDataCall.JobId}, Name : {TabletDataCall.CALLNAME},  CallFlag : {TabletDataCall.CALLFLAG}, CancelFlag : {TabletDataCall.CANCELFLAG}");
                        }
                    }
                    else
                    {
                        TabletLogger.Info($"TabletCall[NG(OverLapDate Exist)], TabletId : {TabletDataCall.SEQ}, JobId : {TabletDataCall.JobId}, Name : {TabletDataCall.CALLNAME},  CallFlag : {TabletDataCall.CALLFLAG}, CancelFlag : {TabletDataCall.CANCELFLAG}");
                    }
                }
            }
            //3.job스케줄 삭제
            void DeletejobSchedule(List<TabletMissionStatusModel> tabletMissionStatusList)
            {
                //Tablet 전체 데이터중 Cancel 신호가 있는것을 가지고온다.
                foreach (var TabletDataCancel in tabletMissionStatusList.Where(x => !string.IsNullOrEmpty(x.CANCELFLAG)).ToList())
                {
                    //4.JobId 있는 경우 조건 확인후 삭제
                    if (TabletDataCancel.JobId > 0)
                    {
                        if (TabletDataCancel.CALLFLAG == JobState.JobWaiting.ToString())
                        {
                            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = TabletDataCancel.CALLNAME, Extra1 = TabletDataCancel.SEQ });
                            foreach (var tabletDeleteData in uow.TabletMissionStatus.GetbyCallName(TabletDataCancel.CALLNAME).ToList())
                            {
                                uow.TabletMissionStatus.Remove(tabletDeleteData);
                                TabletLogger.Info($"TabletCancel[OK], TabletId : {tabletDeleteData.SEQ}, JobId : {tabletDeleteData.JobId}, Name : {tabletDeleteData.CALLNAME},  CallFlag : {tabletDeleteData.CALLFLAG}, CancelFlag : {tabletDeleteData.CANCELFLAG}");
                            }
                        }
                        else
                        {
                            TabletLogger.Info($"TabletCancel[NG(JobState Init different)], TabletId : {TabletDataCancel.SEQ}, JobId : {TabletDataCancel.JobId}, Name : {TabletDataCancel.CALLNAME},  CallFlag : {TabletDataCancel.CALLFLAG}, CancelFlag : {TabletDataCancel.CANCELFLAG}");
                            continue;
                        }
                    }
                    //5.JobId 없는데 Cancel 들어오는경우 삭제
                    else
                    {
                        foreach (var tabletDeleteData in overlapDataExcept.Where(t => t.CALLNAME == TabletDataCancel.CALLNAME).ToList())
                        {
                            uow.TabletMissionStatus.Remove(tabletDeleteData);
                            TabletLogger.Info($"TabletCancel[OK], TabletId : {TabletDataCancel.SEQ}, JobId : {TabletDataCancel.JobId}, Name : {TabletDataCancel.CALLNAME},  CallFlag : {TabletDataCancel.CALLFLAG}, CancelFlag : {TabletDataCancel.CANCELFLAG}");
                        }
                    }
                }
            }
        }

        private void LogExceptionMessage(Exception ex)
        {
            string message = ex.InnerException?.Message ?? ex.Message;
            Debug.WriteLine(message);
            TabletLogger.Info(message + "\n[StackTrace]\n" + ex.StackTrace);
        }
    }
}
