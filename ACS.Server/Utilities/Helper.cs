using log4net;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace INA_ACS_Server.Utilities
{
    public static class Helper
    {
        private readonly static ILog EventLogger = LogManager.GetLogger("Event"); //Function 실행관련 Log
        private readonly static object lockObjct = new object();

        //여러개 메일주소로 변경요청함
        public static void SendMail(string subjectText, string bodyText, string ToEmail)
        {
            lock (lockObjct)
            {

                // 메일 설정
                var smtpHostName = "10.101.10.6"; //"smtp.office365.com"; "smtp.gmail.com";
                var smtpPort = 25; //587;
                var smtpAuthUser = ""; //"ATKAutomationAdmin@amkor.co.kr";
                var smtpAuthPassword = ""; //"amkor123!!!";
                var from = "ATKAutomationAdmin@amkor.co.kr";
                var to = ToEmail; //"ATKAutomationAdmin@amkor.co.kr";  //"ByungKwon.Son@amkor.co.kr";

                //var to = "igkim@inatech.co.kr"; //"ATKAutomationAdmin@amkor.co.kr";  //"ByungKwon.Son@amkor.co.kr";

                //var to = ConfigData.sErrorSolution_URL; //"ATKAutomationAdmin@amkor.co.kr";  //"ByungKwon.Son@amkor.co.kr";

                try
                {
                    using (var client = new SmtpClient(new ProtocolLogger("smtp.log"))) // for file logging .. thread NOT safety !
                    using (var message = new MimeMessage())
                    {
                        message.From.Add(MailboxAddress.Parse(from));
                        message.To.Add(MailboxAddress.Parse(to));
                        message.Subject = subjectText;
                        message.Body = new TextPart(TextFormat.Html) { Text = bodyText };

                        //파일 전송(Test안됨)[URL Send 로 변경]
                        //var builder = new BodyBuilder();
                        //builder.Attachments.Add(@"C:\Users\N0440\Desktop\BackUp\MiR&Fleet 통신 메뉴얼\Fleet\010419_MiR_Fleet_2_ReST_API_v_2_0_ver3.pdf");
                        //message.Body = builder.ToMessageBody();

                        client.MessageSent += Client_MessageSent;

                        client.Connect(smtpHostName, smtpPort, SecureSocketOptions.Auto);

                        if (string.IsNullOrEmpty(smtpAuthUser) == false)
                        {
                            client.Authenticate(smtpAuthUser, smtpAuthPassword);
                        }

                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    EventLogger.Info(ex.Message);
                }
            }

        }


        //하나의 메일
        //public static void SendMail(string subjectText, string bodyText)
        //{
        //    lock (lockObjct)
        //    {

        //        // 메일 설정
        //        var smtpHostName = "10.101.10.6"; //"smtp.office365.com"; "smtp.gmail.com";
        //        var smtpPort = 25; //587;
        //        var smtpAuthUser = ""; //"ATKAutomationAdmin@amkor.co.kr";
        //        var smtpAuthPassword = ""; //"amkor123!!!";
        //        var from = "ATKAutomationAdmin@amkor.co.kr";
        //        var to = "igkim@inatech.co.kr"; //"ATKAutomationAdmin@amkor.co.kr";  //"ByungKwon.Son@amkor.co.kr";

        //        //var to = ConfigData.sErrorSolution_URL; //"ATKAutomationAdmin@amkor.co.kr";  //"ByungKwon.Son@amkor.co.kr";

        //        try
        //        {
        //            using (var client = new SmtpClient(new ProtocolLogger("smtp.log"))) // for file logging .. thread NOT safety !
        //            using (var message = new MimeMessage())
        //            {
        //                message.From.Add(MailboxAddress.Parse(from));
        //                message.To.Add(MailboxAddress.Parse(to));
        //                message.Subject = subjectText;
        //                message.Body = new TextPart(TextFormat.Html) { Text = bodyText };

        //                client.MessageSent += Client_MessageSent;

        //                client.Connect(smtpHostName, smtpPort, SecureSocketOptions.Auto);

        //                if (string.IsNullOrEmpty(smtpAuthUser) == false)
        //                {
        //                    client.Authenticate(smtpAuthUser, smtpAuthPassword);
        //                }

        //                client.Send(message);
        //                client.Disconnect(true);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            EventLogger.Info(ex.Message);
        //        }
        //    }

        //}

        private static void Client_MessageSent(object sender, MessageSentEventArgs e)
        {
            EventLogger.Info($"email sent to {e.Message.To[0]}");
        }
    }
}
