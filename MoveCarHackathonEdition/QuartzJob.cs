using System;
using MoveCarHackathonEdition.Send_Mail_and_SMS;
using Quartz;

namespace MoveCarHackathonEdition
{
    class QuartzJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                MoveCarsMain moveCarsMain = new MoveCarsMain();
                SendEmailandSms sendEmailandSms = new SendEmailandSms();

                moveCarsMain.EmailSmsEvent += sendEmailandSms.OnSendtoEmployee;
                moveCarsMain.EmailSmsEventNoBlocked += sendEmailandSms.OnSendtoEmployeeNoBlocked;
                moveCarsMain.EmailSmsEventNoBlocker += sendEmailandSms.OnSendtoEmployeeNoBlocker;
                moveCarsMain.Execute();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
