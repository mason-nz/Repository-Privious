using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Web.Channels;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            BLL.Loger.Log4Net.Info("Application_Start");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {

                string errorMsg = Server.GetLastError().Message;
                string stackTrace = Server.GetLastError().StackTrace;

                //var s = Server.GetLastError();
                //BLL.Loger.Log4Net.Info("Application_Error ERROR:" + errorMsg);
                //BLL.Loger.Log4Net.Info("Application_Error StackTrace" + stackTrace );


                string source = Server.GetLastError().Source;
                var exception = Server.GetLastError().InnerException;
                string innerErrorMsg = string.Empty;
                string innerStackTrace = string.Empty;
                string innerSource = string.Empty;
                if (exception != null)
                {
                    innerErrorMsg = exception.Message;
                    innerStackTrace = exception.StackTrace;
                    innerSource = exception.Source;
                }
                string trueName = BLL.Util.GetLoginRealName();
                Server.ClearError();

                string mailBody = string.Format("当前登陆者：{0} <br/>当前页面：{1}<br/>当前页面Post参数：{8}<br/>浏览器内核（Type）：{9}<br/>浏览器Name+Version：{14}<br/>操作系统：{10}<br/>客户端IP：{11}<br/>客户端Cookies：{12}<br/>UrlReferrer：{13}<br/>错位信息：{2}<br/>错误Source：{3}<br/>错误StackTrace：{4}<br/>内部错位信息：{5}<br/>内部错误Source：{6}<br/>内部错误StackTrace：{7}",
                    trueName, Request.Url.ToString(), errorMsg, source, stackTrace,
                    innerErrorMsg, innerSource, innerStackTrace, Request.Form.ToString(),
                    Request.Browser.Type,
                    Request.Browser.Platform, Request.ServerVariables.Get("Remote_Addr").ToString(),
                    GetCookiesContent(Request.Cookies), Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString(),
                    Request.Browser.Browser + "[" + Request.Browser.Version + "]");

                //add by qizq 2015-1-7 加监控入库日志
                Entities.UserActionLog model = new UserActionLog();
                model.CreateTime = System.DateTime.Now;
                //model.CreateUserID = (int)userid;
                //系统
                model.OperUserType = 3;
                //异常
                model.LogInType = 3;
                model.LogInfo = mailBody;
                //BLL.UserActionLog.Instance.Insert(model);
                BulkInserUserActionThread.EnQueueActionLogs(model);
                string subject = "IM淘车通——报错通知";
                string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
                if (userEmail != null && userEmail.Length > 0)
                {
                    BLL.EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
                }

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("Application_Error 发送邮件失败:" + ex.Message);
            }

        }

        private string GetCookiesContent(HttpCookieCollection httpCookieCollection)
        {
            string str = string.Empty;
            for (int i = 0; i < httpCookieCollection.AllKeys.Length; i++)
            {
                str += httpCookieCollection.Keys[i].ToString() + "=" +
                     httpCookieCollection[httpCookieCollection.Keys[i]].Value + "&";
            }
            return str.TrimEnd('&');
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //DefaultChannelHandler.StateManager.Dispose();
            BLL.Loger.Log4Net.Info("Application_End");

        }
    }
}