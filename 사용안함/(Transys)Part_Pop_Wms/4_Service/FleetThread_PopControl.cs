using Dapper;
using INA_ACS_Server.Views.Popups;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace INA_ACS_Server
{
    public partial class FleetThread
    {
        private readonly static ILog partLogger = LogManager.GetLogger("PartEvent");

        //========================================================== 출고수량변경 팝업 처리 관련

        public bool QtyEdit_Popup_Processing = false;   // 출고수량변경 팝업 처리중 플래그
        public bool QtyEdit_UpdateButton = false;       // 팝업창 업데이트버튼누를때
        public bool QtyEdit_CancelButton = false;       // 팝업창 취소버튼누를때

        public string QtyEdit_CallName;     // 콜네임
        public string QtyEdit_ItemCode;     // 품번
        public string QtyEdit_ItemName;     // 품번
        public int QtyEdit_ItemOutQ;        // 출고요청수량
        public int QtyEdit_ItemTotalQ;      // 재고수량

        public int QtyEdit_ChangedValue = 0; // 출고수량 입력값

        //==========================================================


        private void PopCall_JobStartControl_HOOK()
        {
            var jobs = uow.Jobs.Find(x => x.CallName.StartsWith("MC1") && x.JobState == JobState.JobInit);
            foreach (var job in jobs)
            {
                // ===============================================
                if (job.PopCallReturnType == "N")         // 자재배달미션 (모든미션 완료후 pop완료처리)
                {
                    // job 시작 ~~~~~~~~~~~~
                    if (job.JobState == JobState.JobInit)
                    {
                        job.JobState = JobState.JobStart;
                        uow.Jobs.Update(job);
                    }
                }
                // ===============================================
                else if (job.PopCallReturnType == "Y") // 공박스회수미션 (공박스 픽업후 pop완료처리)
                {
                    // job 시작 ~~~~~~~~~~~~
                    if (job.JobState == JobState.JobInit)
                    {
                        job.JobState = JobState.JobStart;
                        uow.Jobs.Update(job);
                    }
                }
            }
        }

        private void PopCall_JobStartControl_LIFT()
        {
            var jobs = uow.Jobs.Find(x => x.CallName.StartsWith("MC3") && (x.JobState < JobState.JobStart));

            foreach (var job in jobs)
            {
                // ===============================================
                if (job.PopCallReturnType == "N") // 자재배달미션 (모든미션 완료후 pop완료처리)
                {
                    // 창고에 출고요청한다 ~~~~~

                    if (job.JobState == JobState.JobInit)
                    {
                        job.JobState = JobState.JobWaitWmsQty;
                        uow.Jobs.Update(job);
                    }
                    else if (job.JobState == JobState.JobWaitWmsQty)
                    {
                        if (job.WmsId == 0)
                        {
                            var popServerId = job.PopServerId;
                            var popCallName = job.CallName;
                            var PopCallType = job.PopCallReturnType;
                            var PopCallAngle = job.PopCallAngle;
                            var popCallLineCD = job.CallName.Split('_')[0];
                            bool result = int.TryParse(job.CallName.Split('_')[1], out int popCallPostCD);
                            if (result == false || popCallPostCD == 0)
                            {
                                continue; // postCD가 0보다 클때만 처리한다
                            }


                            #region 출고요청전 재고 확인

                            // 자재정보를 가져온다
                            var partItem = popSvc.GetPartInfo2(job);
                            if (partItem == null)
                            {
                                // 자재정보가 없을때.
                                partLogger.Info($"no part info! : popcallname={job.CallName} popcallangle={job.PopCallAngle} popcallreturntype={job.PopCallReturnType}");
                                continue;
                            }

                            var itemCode = partItem.PART_CD; // 품번
                            var itemName = partItem.PART_NM; // 품명
                            int itemOutQ = partItem.OUT_Q; // 출고요청수량
                            int itemTotalQty = 0;


                            // 재고 수량 확인 시작
                            if (job.WmsStepNo == 0) // 재고 확인 시작
                            {
                                // 해당 자재의 출고가능한 재고수량을 읽어온다
                                itemTotalQty = GetWmsDB_ItemStockByItemCode(itemCode); // 재고수량
                                if (itemTotalQty < 0)
                                {
                                    if (itemTotalQty == -1)
                                    {
                                       string msg = $"{job.CallName}, {GetPosNameByCallName(job.CallName)}, 품번 {itemCode} ({itemName}), 재고가 없습니다!";
                                        popLogger.Info(msg);
                                        main.PopCallErrorMessageQueue.Enqueue(msg.Replace(",", "\n"));

                                        //// pop 완료 처리
                                        //PopCall_Done(job);

                                        //// job 데이터 삭제
                                        //Job_DeleteJobData(job);

                                        job.WmsStepNo = 1; // 재고 부족
                                        continue;
                                    }
                                    else if (itemTotalQty == -2)
                                    {
                                        string msg = $"{job.CallName}, {GetPosNameByCallName(job.CallName)}, 품번 {itemCode} ({itemName}), 재고 DB 연결 에러!";
                                        popLogger.Info(msg);
                                        main.PopCallErrorMessageQueue.Enqueue(msg.Replace(",", "\n"));

                                        //// pop 완료 처리
                                        //PopCall_Done(job);

                                        //// job 데이터 삭제
                                        //Job_DeleteJobData(job);

                                        job.WmsStepNo = 2; // 재고 DB 연결 에러
                                        continue;
                                    }
                                }

                                // 해당 자재의 출고수량과 재고수량 비교한다
                                if (itemOutQ <= itemTotalQty)
                                {
                                    job.WmsStepNo = 88; // 출고수량유지
                                }
                                else // 재고 부족시 수량변경 팝업창 띄운다
                                {
                                    // - 출고수량변경 UI는 한번에 한개씩만 처리한다
                                    // - 유저가 UI창을 닫아야, 다음 항목이 처리된다.

                                    if (!this.QtyEdit_Popup_Processing)
                                    {
                                        // 출고수량변경 팝업 처리시작
                                        this.QtyEdit_Popup_Processing = true;
                                        this.QtyEdit_CallName = job.CallName; // 콜네임
                                        this.QtyEdit_ItemCode = itemCode;           // 품번
                                        this.QtyEdit_ItemName = itemName;           // 품명
                                        this.QtyEdit_ItemOutQ = itemOutQ;           // 출고요청수량
                                        this.QtyEdit_ItemTotalQ = itemTotalQty;     // 재고수량

                                        job.WmsStepNo = 3; // 팝업 처리 시작

                                        // 팝업처리 (비동기)
                                        this.main.BeginInvoke(new Action(() =>
                                        {
                                            var itemQtyEditForm = new ItemQtyEditPopupForm(this.QtyEdit_CallName, this.QtyEdit_ItemCode, this.QtyEdit_ItemName, this.QtyEdit_ItemTotalQ, this.QtyEdit_ItemOutQ);

                                            if (itemQtyEditForm.ShowDialog(this.main) == System.Windows.Forms.DialogResult.OK)
                                            {
                                                this.QtyEdit_ChangedValue = itemQtyEditForm.ReturnValue; // 입력값
                                                this.QtyEdit_UpdateButton = true;
                                            }
                                            else
                                            {
                                                this.QtyEdit_CancelButton = true;
                                            }

                                            job.WmsStepNo = 4; // 팝업 처리 완료
                                        }));
                                    }
                                }
                            }

                            if (job.WmsStepNo == 4) // 팝업 처리 완료
                            {
                                // qty edit 폼 입력값 수신시
                                if (this.QtyEdit_UpdateButton)
                                {
                                    // 플래그 설정
                                    this.QtyEdit_Popup_Processing = false;
                                    this.QtyEdit_UpdateButton = false;
                                    this.QtyEdit_CancelButton = false;
                                    popLogger.Info($"출고수량 변경완료.  품번:{itemCode}  재고수량:{itemTotalQty}  출고수량변경전:{itemOutQ}  출고수량변경후:{this.QtyEdit_ChangedValue}");

                                    job.WmsStepNo = 99; // 출고수량변경
                                }
                                // qty edit 폼 취소시
                                else if (this.QtyEdit_CancelButton)
                                {
                                    // 플래그 설정
                                    this.QtyEdit_Popup_Processing = false;
                                    this.QtyEdit_UpdateButton = false;
                                    this.QtyEdit_CancelButton = false;
                                    popLogger.Info($"출고수량 변경취소.  품번:{itemCode}  재고수량:{itemTotalQty}  출고수량변경전:{itemOutQ}  출고수량변경후:{this.QtyEdit_ChangedValue}");

                                    // pop 완료 처리
                                    PopCall_Done(job);

                                    // job 데이터 삭제
                                    Job_DeleteJobData(job);

                                    // 이 job은 삭제했으므로 더이상 처리하지 않고 패스한다
                                    continue;
                                }
                            }

                            #endregion


                            #region 출고요청 추가

                            if (job.WmsStepNo == 88 || job.WmsStepNo == 99) // 88=출고수량유지, 99=출고수량변경
                            {
                                // 출고요청 생성..................자재2(NP_xxxx) 정의되어 있으면 출고요청을 2번 처리??
                                var wmsItem = new WmsModel()
                                {
                                    LINE_CD = popCallLineCD ?? "",
                                    POST_CD = popCallPostCD,
                                    COMM_ANG = PopCallAngle,
                                    RETU_TYPE = PopCallType,
                                    OUT_Q = partItem.OUT_Q,
                                    PART_CD = partItem.PART_CD,
                                    PART_NM = partItem.PART_NM,
                                    OUT_WH = PopService.LIFT_TYPES.Contains(partItem.LINE_CD) ? "Y" : "N",
                                    OUT_POINT = 0,
                                    WMS_IF_FLAG = "N",
                                    CREATE_DT = DateTime.Now,
                                    MODIFY_DT = DateTime.Now,
                                };

                                // 출고수량을 팝업창에서 입력된 값으로 변경한다
                                if (job.WmsStepNo == 99) // 출고수량변경시
                                {
                                    wmsItem.OUT_Q = this.QtyEdit_ChangedValue;
                                }

                                // 창고DB에 출고요청 추가
                                uow.WmsDB.Add(wmsItem);

                                // job에 출고요청 id 저장
                                job.WmsId = wmsItem.Id;
                                job.JobState = JobState.JobWaitWmsOut;
                                uow.Jobs.Update(job);
                            }

                            #endregion

                        }
                    }

                    // 창고에서 출고완료될때까지 기다린 후, job Start 한다 ~~~~~

                    else if (job.JobState == JobState.JobWaitWmsOut)
                    {
                        var wmsItem = uow.WmsDB.GetById(job.WmsId);
                        if (wmsItem != null && wmsItem.WMS_IF_FLAG == "Y") // 출고완료되었나?
                        {
                            // ====================================
                            // 촐고포지션에 따른 job 미션네임 변경!!
                            // ====================================

                            if (wmsItem.OUT_POINT >= 1 && wmsItem.OUT_POINT <= 3) // 출고포지션(출고랙) 번호
                            {
                                // 변경 적용할 job misison name 설정
                                for (int i = 0; i < job.Missions.Count; i++)
                                {
                                    if (job.Missions.Count > 0 && job.Missions[i].MissionName == "L_Pick_S_WMS_??") // 미션명이 L_Pick_S_WMS_?? 일때만 적용!!
                                    {
                                        Mission pickMission = job.Missions[i];

                                        // change mission name
                                        string newMissionName = pickMission.MissionName.Replace("_??", $"_{wmsItem.OUT_POINT:00}");
                                        pickMission.MissionName = newMissionName;
                                        uow.Missions.Update(pickMission);

                                        break;
                                    }
                                }

                                #region 다른구현테스트
                                //// 변경 적용할 job config name 설정
                                //if (job.Missions.Count > 0 && job.Missions[0].MissionName.EndsWith("_??"))
                                //{
                                //    string selectedJobConfigName = null;
                                //    selectedJobConfigName = job.CallButtonName.Split('_')[0];
                                //    selectedJobConfigName += "_";
                                //    selectedJobConfigName += wmsItem.OUT_POINT.ToString();

                                //    // 변경 적용할 job config 가져와서 적용한다
                                //    var selectedJobConfig = uow.MissionConfigs.GetByCallButtonName(selectedJobConfigName);
                                //    if (selectedJobConfig != null)
                                //    {
                                //        for (int i = 0; i < job.Missions.Count; i++)
                                //        {
                                //            Mission mission = job.Missions[i];

                                //            string subMissionName = selectedJobConfig.GetJobMissionName(i);  // config 에서 n번째 미션 선택한다

                                //            if (string.IsNullOrWhiteSpace(subMissionName) || subMissionName.ToUpper() == "NONE") break;

                                //            mission.MissionName = subMissionName;
                                //            uow.Missions.Update(mission);
                                //        }
                                //    }
                                //}
                                #endregion


                                // job 시작 (자재출고후 job 시작) ~~~~~~~~~~~~
                                if (job.JobState == JobState.JobWaitWmsOut)
                                {
                                    job.JobState = JobState.JobStart;
                                    uow.Jobs.Update(job);
                                }

                            }
                        }
                    }
                }
                // ===============================================
                else if (job.PopCallReturnType == "Y") // 공박스회수미션 (공박스 픽업후 pop완료처리)
                {
                    // job 시작 ~~~~~~~~~~~~
                    if (job.JobState == JobState.JobInit)
                    {
                        job.JobState = JobState.JobStart;
                        uow.Jobs.Update(job);
                    }
                }
            }
        }


        // pop 완료 처리
        private void PopCall_Done(Job job)
        {
            if (job.PopCallState == 0)
            {
                // 해당 job에 설정된 정보를 이용하여 popcall 요청을 가져온다
                var popServerId = job.PopServerId;
                var popCallName = job.CallName;
                var popDB = popSvc.SelectPopDB_ByNo(popServerId);
                var popCall = popDB?.GetByPopKey(popCallName);

                // 해당 job의 popcall 완료 처리
                if (popCall != null)
                {
                    popCall.ACS_IF_Flag = "Y";
                    popDB.Update_AcsFlag(popCall);
                }

                // update job state
                job.PopCallState = 1;
                uow.Jobs.Update(job);
            }
        }


        // 창고 아이템 재고 수량 쿼리
        public int GetWmsDB_ItemStockByItemCode(string itemCode)
        {
            const string connectionString = @"Data Source=10.214.252.191,1433;Network Library=DBMSSOCN;Initial Catalog = HOST_IMPEXP; User ID = inatech; Password=inatech1234;";
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var stockInfo = con.Query<(string CreateTime, string itemCode, int itemStock)>(@"
                                            SELECT TOP 1 GIA_DATAORAS1, GIA_ARTICOLO, GIA_GIAC 
                                            FROM EXP_GIACENZE 
                                            WHERE GIA_ARTICOLO = @itemCode
                                            ", new { itemCode })
                                       .FirstOrDefault();

                    // 디버그 출력
                    //var isValidDateTime = DateTime.TryParse(stockInfo.CreateTime, out DateTime dt);
                    //if (isValidDateTime) Console.WriteLine("날짜 {0}", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                    //Console.WriteLine("생성시간 {0}", stockInfo.CreateTime);
                    //Console.WriteLine("자재품번 {0}", stockInfo.itemCode);
                    //Console.WriteLine("자재수량 {0}", stockInfo.itemStock);

                    if (stockInfo.itemCode != null)
                        return stockInfo.itemStock;
                    else
                        return -1;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    EventLogger.Info(ex.Message);
                    return -2;
                }

            }
        }

    }
}
