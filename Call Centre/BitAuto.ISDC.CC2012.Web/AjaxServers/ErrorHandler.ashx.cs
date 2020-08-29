using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// ErrorHandler 的摘要说明
    /// </summary>
    public class ErrorHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性定义
        public string RequestAction
        {
            get { return BLL.Util.GetCurrentRequestStr("Action");}
        }
        public string RequestErrorMsg
        {
            get { return BLL.Util.GetCurrentRequestStr("ErrorMsg"); }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            if (RequestAction.ToLower() == "sendemailforrecorderror")
            {
                SendEmailForRecordError();
            }
        }

        public void SendEmailForRecordError()
        {
            string trueName = BLL.Util.GetLoginRealName();
            string mailBody = string.Format("录音错误：当前登陆者：{0},错误信息：{1} ",trueName,RequestErrorMsg);
            string subject = "新呼叫中心系统——报错通知";
            string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}