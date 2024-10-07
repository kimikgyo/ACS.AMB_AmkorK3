using System;
using System.Linq;
using System.Collections.Concurrent;

namespace INA_ACS_Server
{
    public enum JobCommandCode
    {
        ADD,
        REMOVE,
        REMOVE_BY_ACS, // 유저가 ACS화면에서 직접 job cancel 한 경우, EQP CALL IF_FLAG 를 "Y"가 아닌 "C" 로 완료시키기 위해 추가.
        REMOVE_ALL,
    }


    public class JobCommand
    {
        public JobCommandCode Code;
        public string Text;
        public int Extra1;
        public string Extra2;
        public int Extra3;
        public string Extra4; //(Amkor K5 M3F_T3F층간이송)
        public int Extra5; //우선순위
    }


    public static class JobCommandQueue
    {
        private static readonly ConcurrentQueue<JobCommand> _queue = new ConcurrentQueue<JobCommand>();


        public static void Enqueue(JobCommand item)
        {
            //미션 및 Queue 를 실행한부분을 순차적으로 추가시킨다
            _queue.Enqueue(item);
        }

        public static bool TryDequeue(out JobCommand item)
        {
            //실행하면 순차적으로 하나씩 Return한다
            return _queue.TryDequeue(out item);
        }
    }
}
