using Dapper;
using INA_ACS_Server.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        private void AddRobotError()
        {
            foreach (var robot in GetActiveRobotsOrderbyDescendingBattery())
            {
                if (robot.Errors?.Length > 0)
                {
                    if (string.IsNullOrEmpty(robot.ErrorMailState))
                    {
                        robot.ErrorMailState = robot.Errors?.Length > 0 ? "add-error" : "no-error";
                        continue;
                    }
                    if (robot.Errors?.Length > 0)
                    {
                        if (robot.ErrorMailState != "add-error")
                            AddErrorHistory(robot);
                        robot.ErrorMailState = "add-error";
                    }
                }
                else
                {
                    robot.ErrorMailState = "no-error";

                }
            }
            void AddErrorHistory(Robot robot)
            {
                ErrorHistory ErrorLog = new ErrorHistory();
                // CALL 로그 (DB)
                using (var con = new SqlConnection(ConnectionStrings.DB1))
                {
                    try
                    {
                        if (robot.Errors != null)
                        {
                            for (int i = 0; i < robot.Errors.Length; i++)
                            {
                                //var ErrorListFind = uow.ErrorCodeList.Find(x => x.ErrorCode == robot.Errors[i].Code).FirstOrDefault();
                                ////에러 List 내용이 있을경우 에러List 있는 내용을 전부 히스토리에 넣어둔다
                                //if (ErrorListFind != null)
                                //{
                                //    ErrorLog = new ErrorHistory()
                                //    {
                                //        ErrorCreateTime = DateTime.Now,
                                //        RobotName = robot.RobotName,
                                //        ErrorCode = robot.Errors[i].Code,
                                //        ErrorDescription = robot.Errors[i].Description,
                                //        ErrorModule = robot.Errors[i].Module,
                                //        ErrorMessage = ErrorListFind.ErrorMessage,
                                //        ErrorType = ErrorListFind.ErrorType,
                                //        Explanation = ErrorListFind.Explanation,
                                //        POSMessage = ""
                                //    };


                                //}
                                ////에러 List 내용이 없을경우 Robot에 있는 에러 코드만 입력한다
                                //else
                                {
                                    ErrorLog = new ErrorHistory()
                                    {
                                        ErrorCreateTime = DateTime.Now,
                                        RobotName = robot.RobotName,
                                        ErrorCode = robot.Errors[i].Code,
                                        ErrorDescription = robot.Errors[i].Description,
                                        ErrorModule = robot.Errors[i].Module,
                                        POSMessage = ""
                                    };
                                }
                                ErrorHistoryDataBaseInset(ErrorLog);

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        main.LogExceptionMessage(ex);
                    }
                }
            }
            void ErrorHistoryDataBaseInset(ErrorHistory ErrorLog)
            {
                using (var con = new SqlConnection(ConnectionStrings.DB1))
                {
                    try
                    {
                        const string INSERT_SQL = @"
                            INSERT INTO ErrorHistory
                               ([ErrorCreateTime]
                               ,[RobotName]
                               ,[ErrorCode]
                               ,[ErrorDescription]
                               ,[ErrorModule]
                               ,[ErrorMessage]
                               ,[ErrorType]
                               ,[Explanation]
                               ,[POSMessage])
                           VALUES
                               (@ErrorCreateTime
                               ,@RobotName
                               ,@ErrorCode
                               ,@ErrorDescription
                               ,@ErrorModule
                               ,@ErrorMessage
                               ,@ErrorType
                               ,@Explanation
                               ,@POSMessage);
                                SELECT Cast(SCOPE_IDENTITY() As Int);";

                        int Id = con.ExecuteScalar<int>(INSERT_SQL, param: ErrorLog);
                        ErrorLog.Id = Id;
                    }
                    catch (Exception ex)
                    {
                        main.LogExceptionMessage(ex);
                    }
                }
            }
        }

        #region 메일 통지

        // 로봇별로 에러발생시 이메일 발송한다
        /*private void SendMailOnRobotError()
        {

            if (string.IsNullOrWhiteSpace(ConfigData.sErrorSolution_URL))
                return;

            foreach (var robot in ActiveRobots())
            {
                // 최초 이벤트 발생시에는 이전값이 유효하지 않으므로 이전값/현재값을 동일하게 설정한다
                if (string.IsNullOrEmpty(robot.ErrorMailState))
                {
                    robot.ErrorMailState = robot.Errors?.Length > 0 ? "mail-sent" : "no-error";
                    continue;
                }
                //로봇이 미션을 진행중일때
                var runMission = uow.Missions.Find(m => m.ReturnID > 0 && m.MissionState != "Done" && m.Robot == robot);
                // 충전 미션 제외
                var runChargeingMissions = runMission.Where(m =>
                 uow.ChargeMissionConfig.GetAll().Count(c => m.MissionName == c.ChargeMissionName) != 0).FirstOrDefault();

                //로봇이 한자리에서 일정시간 있을시 메일 및 손목워치 에러로그에 남기기(에러일때 또는 일정시간 한자리에 있을경우!!!)
                //1.미션이 진행중이고 진행중인 미션이 충전 미션이 아닐때!
                if (runMission.Count > 0 && runChargeingMissions == null)
                {
                    //과거 X,Y 값이 현재 X,Y값과 다르면 현재 X,Y 값을 적용시킨다
                    if (robot.POSTimeOutError_X != robot.Position_X || robot.POSTimeOutError_Y != robot.Position_Y)
                    {
                        robot.POSTimeOutError_X = robot.Position_X;
                        robot.POSTimeOutError_Y = robot.Position_Y;
                        robot.POSTimeOutError_Time = DateTime.Now;
                        robot.POSTimeOutFlag = false;
                    }
                    else
                    {
                        //과거 X,Y 값이 현재 X,Y 값 일치시 설정Time을 Add하여 현재 시간과 비교해서 Flag 신호를 true 시킨다
                        if (DateTime.Now > robot.POSTimeOutError_Time.AddMinutes(ConfigData.POSTimeOut) && robot.POSTimeOutFlag == false)
                        {
                            robot.POSTimeOutFlag = true;
                        }
                    }
                }

                //2.TimeOut 시간내에 x,y 좌표가 같으면 에러메세지 전송한다
                //3.TimeOut 시간내에 x.y 좌표가 다르면 다시 Time시간은 초기화한다
                //4.메세지생성은 포지션 으로 생성하여 전송한다. 포시션 이름이 없는경우 x.y좌표로 전송한다.

                // 로봇 에러 정보 있으면 메일 발송한다
                if (robot.Errors?.Length > 0 || robot.POSTimeOutFlag)
                {
                    if (robot.ErrorMailState != "mail-sent") // 이미 발송했으면 skip
                    {

                        //Position 에대한 Message
                        string posMessage = "";
                        var posName = uow.PositionAreaConfigs.UpGrade_GroupPOSArea(robot, ConfigData.ErrorPositionGroup).FirstOrDefault();
                        if (posName != null) posMessage = posName.PositionAreaName;
                        string PosSendMsg = $"Robot {posMessage} 위치 정차 중입니다. 확인 바랍니다.";

                        // mail subject 설정
                        string mailSubject = $"Robot Error Notification for {robot.RobotName} [{robot.RobotAlias}] at [{DateTime.Now:yyyy-MM-dd HH:mm:ss}]";

                        // mail body 설정
                        var ToEmailAddress = uow.UserEmailAddress.GetAll().Where(u => u.EmailUse == "Use" && (u.UserEmailAddress != "None" || u.UserEmailAddress != "none")).ToList();
                        //보낼 Email있을때
                        if (ToEmailAddress.Count > 0)
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine(@"<style> b { color: red; } </style>");
                            sb.AppendLine($"<br>안녕하세요");
                            sb.AppendLine($"<br>");
                            //sb.AppendLine($"<h4>{mailSubject}</h4>")
                            sb.AppendLine($"<br>{mailSubject}");
                            sb.AppendLine($"<br>{PosSendMsg}");
                            sb.AppendLine($"<br>");

                            //robot에 Error 코드가 존재할때 (한포지션에 오래 정차시에도 에러메일을 보내야함)
                            if (robot.Errors.Length > 0)
                            {
                                for (int i = 0; i < robot.Errors.Length; i++)
                                {

                                    sb.AppendLine($"<br><b> ■ Error Code : {robot.Errors[i].Code}</b>");
                                    sb.AppendLine($"<br> -Description : {robot.Errors[i].Description}");
                                    sb.AppendLine($"<br> -Module : {robot.Errors[i].Module}");
                                    //sb.AppendLine($"<br>Description : {GetErrorDescription(robot.Errors[i].Description)}");
                                    //ErrorCodeList에 있는 설명을 ErrorCode로 찾는다
                                    var ErrorListFind = uow.ErrorCodeList.Find(x => x.ErrorCode == robot.Errors[i].Code).FirstOrDefault();
                                    if (ErrorListFind != null)
                                    {
                                        sb.AppendLine($"<br> -ErrorMessage : {ErrorListFind.ErrorMessage}");
                                        sb.AppendLine($"<br> -ErrorType : {ErrorListFind.ErrorType}");
                                        sb.AppendLine($"<br> -Explanation : {ErrorListFind.Explanation}");
                                    }

                                    sb.AppendLine($"<br>");
                                }
                            }

                            //Robot 에 Error 코드가 존재하지 않을때 내부적으로 TimeOut 대한 에러 코드를 생성한다.
                            else
                            {
                                var ErrorListFind = uow.ErrorCodeList.Find(x => x.ErrorCode == 0).FirstOrDefault();
                                if (ErrorListFind != null)
                                {
                                    sb.AppendLine($"<br> -ErrorMessage : {ErrorListFind.ErrorMessage}");
                                    sb.AppendLine($"<br> -ErrorType : {ErrorListFind.ErrorType}");
                                    sb.AppendLine($"<br> -Explanation : {ErrorListFind.Explanation}");
                                }

                                sb.AppendLine($"<br>");
                            }
                            sb.AppendLine($"<br>{ConfigData.ErrorSolutionName1}");
                            sb.AppendLine($"<br>{ConfigData.sErrorSolution_URL1}");
                            sb.AppendLine($"<br>");
                            sb.AppendLine($"<br>{ConfigData.ErrorSolutionName2}");
                            sb.AppendLine($"<br>{ConfigData.sErrorSolution_URL2}");
                            sb.AppendLine($"<br>");

                            string mailBody = sb.ToString();

                            foreach (var UserEmail in ToEmailAddress)
                            {
                                // 메일발송은 오래 걸리므로 태스크 처리한다
                                Task.Run(() =>
                                {
                                    Helper.SendMail(mailSubject, mailBody, UserEmail.UserEmailAddress);
                                });
                            }
                        }

                        AddErrorHistory(robot, PosSendMsg);
                        robot.ErrorMailState = "mail-sent";
                    }
                }
                else
                {
                    robot.ErrorMailState = "no-error";
                }
            }
            return;


            string GetErrorDescription(string errorDescJson)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(errorDescJson))
                    {
                        // message
                        var messageJson = JObject.Parse(errorDescJson);
                        string message = messageJson["message"].ToString();

                        // message arguments
                        var arguments = messageJson["args"];
                        foreach (var item in arguments)
                        {
                            var argument = (JProperty)item;
                            string pattern = "%(" + argument.Name + ")";
                            message = message.Replace(pattern, argument.Value.ToString());
                        }

                        return message;
                    }
                }
                catch { }
                return string.Empty;
            }

            void AddErrorHistory(Robot robot, string PosSendMsg)
            {
                ErrorHistory ErrorLog = new ErrorHistory();
                // CALL 로그 (DB)
                using (var con = new SqlConnection(ConnectionStrings.DB1))
                {
                    try
                    {
                        if (robot.Errors != null)
                        {
                            for (int i = 0; i < robot.Errors.Length; i++)
                            {
                                //var ErrorListFind = uow.ErrorCodeList.Find(x => x.ErrorCode == robot.Errors[i].Code).FirstOrDefault();
                                ////에러 List 내용이 있을경우 에러List 있는 내용을 전부 히스토리에 넣어둔다
                                //if (ErrorListFind != null)
                                //{
                                //    ErrorLog = new ErrorHistory()
                                //    {
                                //        ErrorCreateTime = DateTime.Now,
                                //        RobotName = robot.RobotName,
                                //        ErrorCode = robot.Errors[i].Code,
                                //        ErrorDescription = robot.Errors[i].Description,
                                //        ErrorModule = robot.Errors[i].Module,
                                //        ErrorMessage = ErrorListFind.ErrorMessage,
                                //        ErrorType = ErrorListFind.ErrorType,
                                //        Explanation = ErrorListFind.Explanation,
                                //        POSMessage = PosSendMsg
                                //    };


                                //}
                                ////에러 List 내용이 없을경우 Robot에 있는 에러 코드만 입력한다
                                //else
                                {
                                    ErrorLog = new ErrorHistory()
                                    {
                                        ErrorCreateTime = DateTime.Now,
                                        RobotName = robot.RobotName,
                                        ErrorCode = robot.Errors[i].Code,
                                        ErrorDescription = robot.Errors[i].Description,
                                        ErrorModule = robot.Errors[i].Module,
                                        POSMessage = PosSendMsg
                                    };
                                }
                                ErrorHistoryDataBaseInset(ErrorLog);

                            }
                        }
                        //else
                        //{
                        //    //포지션 TimeOutError
                        //    var ErrorListFind = uow.ErrorCodeList.Find(x => x.ErrorCode == 0).FirstOrDefault();
                        //    ErrorLog = new ErrorHistory()
                        //    {
                        //        ErrorCreateTime = DateTime.Now,
                        //        RobotName = robot.RobotName,
                        //        ErrorMessage = ErrorListFind.ErrorMessage,
                        //        ErrorType = ErrorListFind.ErrorType,
                        //        Explanation = ErrorListFind.Explanation,
                        //        POSMessage = PosSendMsg
                        //    };
                        //    ErrorHistoryDataBaseInset(ErrorLog);
                        //}
                    }
                    catch (Exception ex)
                    {
                        LogExceptionMessage(ex);
                    }
                }
            }
            void ErrorHistoryDataBaseInset(ErrorHistory ErrorLog)
            {
                using (var con = new SqlConnection(ConnectionStrings.DB1))
                {
                    try
                    {
                        const string INSERT_SQL = @"
                            INSERT INTO ErrorHistory
                               ([ErrorCreateTime]
                               ,[RobotName]
                               ,[ErrorCode]
                               ,[ErrorDescription]
                               ,[ErrorModule]
                               ,[ErrorMessage]
                               ,[ErrorType]
                               ,[Explanation]
                               ,[POSMessage])
                           VALUES
                               (@ErrorCreateTime
                               ,@RobotName
                               ,@ErrorCode
                               ,@ErrorDescription
                               ,@ErrorModule
                               ,@ErrorMessage
                               ,@ErrorType
                               ,@Explanation
                               ,@POSMessage);
                                SELECT Cast(SCOPE_IDENTITY() As Int);";

                        int Id = con.ExecuteScalar<int>(INSERT_SQL, param: ErrorLog);
                        ErrorLog.Id = Id;
                    }
                    catch (Exception ex)
                    {
                        LogExceptionMessage(ex);
                    }
                }
            }

        }*/

        #endregion
    }
}
