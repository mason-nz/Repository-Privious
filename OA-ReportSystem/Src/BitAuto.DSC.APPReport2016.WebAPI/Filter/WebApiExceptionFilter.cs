    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using BitAuto.DSC.OASysRightManager2016.Common.Common;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace BitAuto.DSC.APPReport2016.WebAPI.Filter
{
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            string errorMsg = context.Exception.Message;
            string stackTrace = context.Exception.StackTrace;
            string source = context.Exception.Source;
            var exception = context.Exception.InnerException;
            string innerErrorMsg = string.Empty;
            string innerStackTrace = string.Empty;
            string innerSource = string.Empty;
            if (exception != null)
            {
                innerErrorMsg = exception.Message;
                innerStackTrace = exception.StackTrace;
                innerSource = exception.Source;
            }
            Common.Loger.Log4Net.Error("HR_WebApi——异常", context.Exception);
            string trueName = BLL.Util.GetLoginRealName();

            string mailBody = string.Format(@"
当前登陆者：{0} <br/>
当前页面：{1}<br/>
当前页面Post参数：{2}<br/>
浏览器内核（Type）：{3}<br/>
浏览器Name+Version：{4}<br/>
操作系统：{5}<br/>
客户端IP：{6}<br/>
客户端Cookies：{7}<br/>
UrlReferrer：{8}<br/>
错位信息：{9}<br/>
错误Source：{10}<br/>
错误StackTrace：{11}<br/>
内部错位信息：{12}<br/>
内部错误Source：{13}<br/>
内部错误StackTrace：{14}",
                trueName, context.Request.RequestUri.ToString(),
                System.Web.HttpContext.Current.Request.Form.ToString(), System.Web.HttpContext.Current.Request.Browser.Type,
                System.Web.HttpContext.Current.Request.Browser.Browser + "[" + System.Web.HttpContext.Current.Request.Browser.Version + "]",
                System.Web.HttpContext.Current.Request.Browser.Platform,
                System.Web.HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString(),
                GetCookiesContent(System.Web.HttpContext.Current.Request.Cookies),
                System.Web.HttpContext.Current.Request.UrlReferrer == null ? string.Empty : System.Web.HttpContext.Current.Request.UrlReferrer.ToString(),
                errorMsg, source, stackTrace,
                innerErrorMsg, innerSource, innerStackTrace
                );
            string subject = "HR_WebApi——报错通知";
            string[] userEmail = BLL.Util.Reciver;
            if (userEmail != null && userEmail.Length > 0)
            {
                BLL.EmailMain senderMail = new BLL.EmailMain();
                senderMail.SendMailByTemplate("mailSysError", mailBody, subject, userEmail);
            }

            var result = new JsonResult();
            result.Status = (int)HttpStatusCode.InternalServerError;
            result.Message = errorMsg;
            result.Result = null;

            context.Response = new HttpResponseMessage(HttpStatusCode.OK);
            context.Response.Content = new StringContent(JsonConvert.SerializeObject(result, Formatting.Indented));

            //base.OnException(context);
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
    }
}