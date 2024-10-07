using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public partial class MainLoop
    {
        private bool flag = true;
        private void Email_Control()
        {
            try
            {
                Email email = new Email();
                email.smtpHostName = "smtp.naver.com";
                email.smtpPort = 465;
                email.smtpAuthUser = "hi2358@naver.com";
                email.smtpAuthPassword = "rhdwhsghltk4wkd@";
                email.smtpSendMailAccount = "hi2358@naver.com";

                string subjectText = "테스트 메일";
                string bodyText = "테스트 중 입니다.";
                string ToEmail = "";
                
                var EmailAddress = uow.UserEmailAddress.GetAll();
                int count = 0;
                foreach (var toemail in EmailAddress)
                {
                    if (toemail.EmailUse == "Use" && toemail.DisplayFlag == 1)
                    {
                        count++;

                        ToEmail += toemail.UserEmailAddress;
                        
                        if (EmailAddress.Count < count)
                            ToEmail += ";";
                    }
                }

                if (flag)
                {
                    email.SendMail(subjectText, bodyText, ToEmail);
                    flag = false;
                }
            }
            catch(Exception ex)
            {
                main.LogExceptionMessage(ex);
            }
        }
    }
}
