using log4net;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class Email
    {
        public string smtpHostName = "";
        public int smtpPort = 0;
        public string smtpAuthUser = "";
        public string smtpAuthPassword = "";
        public string smtpSendMailAccount = "";

        private readonly static ILog EventLogger = LogManager.GetLogger("Event");
        private readonly static object lockObjct = new object();

        public void SendMail(string subjectText, string bodyText, string ToEmail)
        {
            lock (lockObjct)
            {
                try
                {
                    var to = ToEmail;

                    using (var client = new SmtpClient(new ProtocolLogger("smtp.log"))) // for file logging .. thread NOT safety !
                    using (var message = new MimeMessage())
                    {
                        message.From.Add(MailboxAddress.Parse(smtpSendMailAccount));

                        string[] emails = to.Split(';');

                        foreach (var toemail in emails)
                        {
                            message.To.Add(MailboxAddress.Parse(toemail));
                        }                  
                        
                        message.Subject = subjectText;
                        message.Body = new TextPart(TextFormat.Html) { Text = bodyText };

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
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    EventLogger.Info(ex.Message);
                }
            }
        }

        private static void Client_MessageSent(object sender, MessageSentEventArgs e)
        {
            EventLogger.Info($"email sent to {e.Message.To[0]}");
        }
    }
}
