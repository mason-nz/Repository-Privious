using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Timers;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync
{
    public class MailReminderService
    {
        public readonly static MailReminderService Instance = new MailReminderService();
        private string[] MailReminder = (ConfigurationManager.AppSettings["ReceiveEmailReminder"] != null ? ConfigurationManager.AppSettings["ReceiveEmailReminder"].Split(';') : null);

        public void SendMailReminder(string subject,string mailBody)
        {
            if (MailReminder != null && MailReminder.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, MailReminder);
            }
        }
    }
}
