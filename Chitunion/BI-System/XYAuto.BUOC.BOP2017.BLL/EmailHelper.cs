using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.BUOC.BOP2017.BLL
{
    public class EmailHelper
    {
        #region Instance

        public static readonly EmailHelper Instance = new EmailHelper();

        #endregion Instance

        #region Contructor

        protected EmailHelper()
        { }

        #endregion Contructor

        #region 发送邮件

        private void SendMailByTemplate(string templateName, string[] mailBody, string subject, string[] reciver)
        {
            if (reciver.Length > 0 && mailBody.Length > 0)
            {
                Loger.Log4Net.Info("发邮件开始：");
                try
                {
                    XYAuto.YanFa.SysRightManager.Common.IsdcMail mail = new YanFa.SysRightManager.Common.IsdcMail();
                    mail.Subject = subject;
                    mail.TemplateName = templateName;
                    for (int i = 0; i < mailBody.Length; i++)
                    {
                        mail.Parameters.Add("MailContent" + (i + 1), mailBody[i]);
                    }
                    for (int i = 0; i < reciver.Length; i++)
                    {
                        if (string.IsNullOrEmpty(reciver[i]))
                        {
                            continue;
                        }
                        mail.To.Add(reciver[i]);
                        Loger.Log4Net.Info("发送邮件给谁:" + reciver[i]);
                    }
                    mail.Send();
                    //写入发送日志
                    //...
                }
                catch (Exception e)
                {
                    Loger.Log4Net.Error("发邮件失败: ", e);
                }
                Loger.Log4Net.Info("发邮件结束！");
            }
        }

        #endregion 发送邮件

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
        /// 发送邮件
        /// </summary>
        /// <param name="mailBody1">邮件内容1</param>
        /// <param name="mailBody2">邮件内容2</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        public void SendMail(string subject, string[] reciver, string[] body, string Template)
        {
            SendMailByTemplate(Template, body, subject, reciver);
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
        /// 发送带附件的邮件（模板使用mailSysError）
        /// </summary>
        /// <param name="mailBody1">邮件内容</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        /// <param name="copyto">抄送人Email</param>
        /// <param name="attachPath">附件路径</param>
        public void SendCCReportDataMail(string mailBody1, string subject, string[] reciver, string[] copyto, string attachPath)
        {
            string[] body = new string[] { mailBody1 };
            SendMailByTemplateAndAttach("mailSysError", body, subject, reciver, copyto, attachPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="mailBody">邮件内容</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="reciver">接收者Email</param>
        /// <param name="copyto">抄送人Email</param>
        /// <param name="attachPath">附件路径</param>
        private void SendMailByTemplateAndAttach(string templateName, string[] mailBody, string subject, string[] reciver, string[] copyto, string attachPath)
        {
            if (reciver.Length > 0 && mailBody.Length > 0)
            {
                Loger.Log4Net.Info("发邮件开始：");
                try
                {
                    XYAuto.YanFa.SysRightManager.Common.IsdcMail mail = new XYAuto.YanFa.SysRightManager.Common.IsdcMail();
                    mail.Subject = subject;
                    mail.Attachments.Add(AddAttachments(attachPath));
                    mail.TemplateName = templateName;
                    for (int i = 0; i < mailBody.Length; i++)
                    {
                        mail.Parameters.Add("MailContent" + (i + 1), mailBody[i]);
                    }
                    for (int i = 0; i < reciver.Length; i++)
                    {
                        if (string.IsNullOrEmpty(reciver[i]))
                        {
                            continue;
                        }
                        mail.To.Add(reciver[i]);
                        Loger.Log4Net.Info("发送邮件给谁:" + reciver[i]);
                    }
                    if (copyto != null)
                    {
                        for (int i = 0; i < copyto.Length; i++)
                        {
                            if (string.IsNullOrEmpty(copyto[i]))
                            {
                                continue;
                            }
                            mail.CC.Add(copyto[i]);
                            Loger.Log4Net.Info("发送邮件给谁:" + copyto[i]);
                        }
                    }
                    mail.Send();
                    //写入发送日志
                    //...
                }
                catch (Exception e)
                {
                    Loger.Log4Net.Error("发邮件失败: ", e);
                }
                Loger.Log4Net.Info("发邮件结束！");
            }
        }

        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="attachmentsPath">附件的路径集合，以分号分隔</param>
        public Attachment AddAttachments(string attachmentsPath)
        {
            try
            {
                string[] path = attachmentsPath.Split(';'); //以什么符号分隔可以自定义
                Attachment data = new Attachment(path[0], MediaTypeNames.Application.Octet); ;
                return data;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("获取附件路径发生异常：" + ex);
                return null;
            }
        }
    }
}