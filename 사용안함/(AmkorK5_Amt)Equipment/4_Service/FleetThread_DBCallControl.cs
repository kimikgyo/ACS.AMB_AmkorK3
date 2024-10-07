using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {
        private readonly static ILog logger = LogManager.GetLogger("DBCallEvent");


        private void DBCall_RequestControl()
        {
#if (!DBCALL)
            return;
#endif

            // CALL/CANCEL 항목들을 DB에서 가져와서 처리한다

            List<EquipmentOrder> newOrders = DBCall_GetOrdersFromDB();

            foreach (var order in newOrders)
            {
                EquipmentCallInfo eqpInfo = EquipmentCallInfoRepo.GetByEqpName(order.EQP_NAME, order.INCH_TYPE);
                var callName = eqpInfo.CALL_NAME;
                var groupName = eqpInfo.GROUP_NAME;

                if (order.COMMAND == "CALL")
                {
                    var job = uow.Jobs.GetByCallName(callName);
                    if (job == null)
                    {
                        //// 로봇 미지정
                        //JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = callName });

                        // 로봇 지정 (이 방식은 job queue 에서 로봇당 job 1개만 받아들인다. 미리 queue에 쌓을 수 없다)
                        var targetRobot = uow.Robots.GetAll().FirstOrDefault(x => x.ACSRobotGroup == groupName);
                        if (!IsJobExist(targetRobot)) // job이 없을때만 추가 시도.  (큐에 계속 추가하려고 시도하지 않도록)
                        {
                            JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.ADD, Text = callName, Extra4 = targetRobot?.RobotName ?? "" });
                        }

                        Console.WriteLine(order);
                        logger.Info($"DBCall_Call_Request: {order}");
                    }
                }
                else if (order.COMMAND == "CANCEL")
                {
                    var job = uow.Jobs.GetByCallName(callName);
                    //if (job != null)
                    {
                        JobCommandQueue.Enqueue(new JobCommand { Code = JobCommandCode.REMOVE, Text = callName });

                        Console.WriteLine(order);
                        logger.Info($"DBCall_Cancel_Request: {order}");
                    }
                }
            }
            return;


            // ===== 로컬 함수 =====

            bool IsJobExist(Robot robot) // 이 로봇에 할당된 job이 있나?
            {
                var foundJobs = uow.Jobs.Find(j => j.ACSJobGroup == robot.ACSRobotGroup
                                                && j.JobCreateRobotName == robot.RobotName);
                return foundJobs.Count > 0;
            }

        }


        private List<EquipmentOrder> DBCall_GetOrdersFromDB()
        {
            // DB에서 아직 처리안된(FLAG=N) 항목들 가져온다
            return uow.DBCalls.GetAll_With_Flag_N()
                      .Where(x => !string.IsNullOrWhiteSpace(x.EQP_NAME))
                      .OrderBy(x => x.CREATE_DT).ToList(); // order call 시간순 정렬
        }


        // pop 완료 처리
        private void DBCall_Done(Job job, bool byACS = false)
        {
            if (job.PopCallState == 0)
            {
                // 해당 job에 설정된 정보를 이용하여 popcall 요청을 가져온다

                var eqpName = EquipmentCallInfoRepo.GetEqpNameByCallName(job.CallName);
                var dbCalls = uow.DBCalls.GetAll_With_Flag_N().Where(x => x.EQP_NAME == eqpName);

                // 해당 job의 call 완료 처리
                foreach (var x in dbCalls)
                {
                    x.IF_FLAG = byACS ? "C" : "Y";
                    uow.DBCalls.UpdateFlag(x);
                }

                // update job state
                job.PopCallState = 1;
                uow.Jobs.Update(job);
            }
        }


        // pop 완료 처리 (job 이 없을때의 db call 완료처리)
        private void DBCall_Done_Without_Job(string callName, bool byACS = false)
        {
            // 해당 job에 설정된 정보를 이용하여 popcall 요청을 가져온다

            var eqpName = EquipmentCallInfoRepo.GetEqpNameByCallName(callName);
            var dbCalls = uow.DBCalls.GetAll_With_Flag_N().Where(x => x.EQP_NAME == eqpName);

            // 해당 job의 모든 call 완료 처리
            foreach (var x in dbCalls)
            {
                x.IF_FLAG = byACS ? "C" : "Y";
                uow.DBCalls.UpdateFlag(x);
            }

        }

    }
}
