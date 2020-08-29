using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_2015.BLL;

namespace BitAtuo.DSC.IM_2015.UserMessageMail
{
    partial class UserMessageService : ServiceBase
    {
        public UserMessageService()
        {
            InitializeComponent();
        }

        //定时器
        Timer timer = new Timer();

        int RegularTime = 60;

        protected override void OnStart(string[] args)
        {
            string value = ConfigurationUtil.GetAppSettingValue("RegularTime", false);
            if (!string.IsNullOrEmpty(value))
            {
                RegularTime = int.Parse(value);
            }
            timer.Interval = RegularTime * 60 * 1000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                string[] userEmail = ConfigurationUtil.GetAppSettingValue("MailTo", false).Split(';');
                int totalCount = 0;
                var dt = UserMessage.Instance.GetUserMessage(new BitAuto.DSC.IM_2015.Entities.QueryUserMessage() { Status = 1 }, "CreateTime desc", 1, 100000, out totalCount);
                string subject = "在线留言提醒";
                if (userEmail != null && userEmail.Length > 0 && dt.Rows.Count != 0)
                {
                    EmailHelper.Instance.SendUserMessageMail("", "在线留言提醒", "你有" + dt.Rows.Count + "条留言待处理", subject, userEmail);
                }
                timer.Start();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("Application_Error 发送邮件失败:" + ex.Message);
            }

        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
