using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Linq;

namespace BitAuto.DSC.APPReport2016.BLL
{
    public class EmailMain
    {
        public void SendMail(string mailBody, string subject, string[] reciver)
        {
            SmtpClient client = new SmtpClient();
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = mailBody;
            message.IsBodyHtml = true;
            for (int i = 0; i < reciver.Length; i++)
            {
                message.To.Add(reciver[i]);
            }
            client.Send(message);
        }

        public void SendMailByTemplate(string templateName, string mailBody, string subject, string[] reciver)
        {
            BLL.Loger.Log4Net.Info("发送报错邮件-发送中。。。");
            //过滤为空的邮箱
            reciver = reciver.Where(x => x.Length > 0).ToArray();
            if (reciver.Length > 0)
            {
                BitAuto.YanFa.SysRightManager.Common.IsdcMail mail = new YanFa.SysRightManager.Common.IsdcMail();
                mail.Subject = subject;
                mail.TemplateName = templateName;
                mail.Parameters.Add("MailContent1", mailBody);
                for (int i = 0; i < reciver.Length; i++)
                {
                    mail.To.Add(reciver[i]);
                }
                try
                {
                    //mail.Send();
                    mail.SendAsync(EmailCompleted);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("发送邮件失败：", ex);
                }
            }
        }

        private void EmailCompleted(string message)
        {
            BLL.Loger.Log4Net.Info("异步发邮件——回调结果：" + message);
        }

        public void SendMailByEmployeeTemplate(string mailBody, string subject, string[] reciver)
        {
            SendMailByTemplate("mail630", mailBody, subject, reciver);
        }

        public void SendMailWithAttachments(string mailBody, string subject, string[] reciver, string fileName)
        {
            SmtpClient client = new SmtpClient();
            MailMessage message = new MailMessage();
            Attachment attach = new Attachment(fileName);
            message.Attachments.Add(attach);
            message.Subject = subject;
            message.Body = mailBody;
            message.IsBodyHtml = true;
            for (int i = 0; i < reciver.Length; i++)
            {
                message.To.Add(reciver[i]);
            }
            client.Send(message);
            //释放文件资源
            attach.Dispose();
        }
    }
}
