using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.BLL
{
    public class EmailHelper
    {
        #region Instance
        public static readonly EmailHelper Instance = new EmailHelper();
        #endregion

        #region Contructor
        protected EmailHelper()
        { }
        #endregion

        #region 发送邮件
        private void SendMailByTemplate(string templateName, string[] mailBody, string subject, string[] reciver)
        {
            if (reciver.Length > 0 && mailBody.Length > 0)
            {
                BLL.Loger.Log4Net.Info("发邮件开始：");
                try
                {
                    BitAuto.YanFa.SysRightManager.Common.IsdcMail mail = new YanFa.SysRightManager.Common.IsdcMail();
                    mail.Subject = subject;
                    mail.TemplateName = templateName;
                    for (int i = 0; i < mailBody.Length; i++)
                    {
                        mail.Parameters.Add("MailContent" + (i + 1), mailBody[i]);
                    }
                    for (int i = 0; i < reciver.Length; i++)
                    {
                        mail.To.Add(reciver[i]);
                        BLL.Loger.Log4Net.Info("发送邮件给谁:" + reciver[i]);
                    }
                    mail.Send();
                    //写入发送日志
                    //...
                }
                catch (Exception e)
                {
                    BLL.Loger.Log4Net.Error("发邮件失败: ", e);
                }
                BLL.Loger.Log4Net.Info("发邮件结束！");
            }
        }
        #endregion

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailBody1">邮件内容1</param>
        /// <param name="mailBody2">邮件内容2</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        public void SendMail(string mailBody1, string mailBody2, string subject, string[] reciver)
        {
            string[] body = new string[] { mailBody1, mailBody2 };
            SendMailByTemplate("mail630", body, subject, reciver);
        }

        /// <summary>
        /// 工单模块-发送邮件 add lxw 13.9.12
        /// </summary>
        /// <param name="mailBody1">邮件内容1</param>
        /// <param name="mailBody2">邮件内容2</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        public void SendMailByWorkOrder(string userName, string title, string desc, string lastOperTime, string url, string subject, string[] reciver)
        {
            string[] body = new string[] { userName, title, desc, lastOperTime, url };
            SendMailByTemplate("mailWorkOrder", body, subject, reciver);
        }

        /// <summary>
        /// 发送报错邮件
        /// </summary>
        /// <param name="mailBody1">邮件内容</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        public void SendErrorMail(string mailBody1, string subject, string[] reciver)
        {
            BLL.Loger.Log4Net.Info(mailBody1);
            string[] body = new string[] { mailBody1 };
            SendMailByTemplate("mailSysError", body, subject, reciver);
        }

        /// <summary>
        /// 易集客CC项目生成时发邮件
        /// </summary>
        /// <param name="mailBody1">邮件内容</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        public void SendCreateProjectMail(string mailBody1, string subject, string[] reciver)
        {
            string[] body = new string[] { mailBody1 };
            SendMailByTemplate("mailSysError", body, subject, reciver);
        }

        /// <summary>
        /// 在线留言邮件发送
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="subject"></param>
        /// <param name="reciver"></param>
        public void SendUserMessageMail(string userName, string title, string desc,  string subject, string[] reciver)
        {
            string[] body = new string[] { userName, title, desc };
            SendMailByTemplate("UserMessage", body, subject, reciver);
        }
    }
}
