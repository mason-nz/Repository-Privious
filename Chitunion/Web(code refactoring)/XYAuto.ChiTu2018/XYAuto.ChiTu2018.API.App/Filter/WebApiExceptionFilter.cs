﻿using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.CTUtils.Email;

namespace XYAuto.ChiTu2018.API.App.Filter
{
    /// <summary>
    /// api异常管理
    /// </summary>
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly bool _isSetCrossSite = bool.Parse(ConfigurationManager.AppSettings["IsSetCrossSite"]);
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";
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
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("XYAuto.ChiTu2018.API——异常", context.Exception);            string trueName = "";//BLL.Util.GetLoginRealName();

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
                HttpContext.Current.Request.Form.ToString(), HttpContext.Current.Request.Browser.Type,
                HttpContext.Current.Request.Browser.Browser + "[" + HttpContext.Current.Request.Browser.Version + "]",
                HttpContext.Current.Request.Browser.Platform,
                HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString(),
                GetCookiesContent(HttpContext.Current.Request.Cookies),
                HttpContext.Current.Request.UrlReferrer == null ? string.Empty : HttpContext.Current.Request.UrlReferrer.ToString(),
                errorMsg, source, stackTrace,
                innerErrorMsg, innerSource, innerStackTrace
                );
            string subject = "XYAuto.ChiTu2018.API——报错通知";
            string[] userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail.Length > 0)
            {
                //EmailMain senderMail = new EmailMain();
                //senderMail.SendMailByTemplate("mailSysError", mailBody, subject, userEmail);
                EmailHelper.Instance.SendErrorMail(mailBody, subject, userEmail);
            }

            var result = new JsonResult
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Message = errorMsg,
                Result = null
            };

            context.Response = new HttpResponseMessage(HttpStatusCode.OK);
            if (_isSetCrossSite)
            {
                var requestReferrer = context.Request.Headers.Referrer;
                if (requestReferrer != null)
                {
                    var requestHost = $"{requestReferrer.Scheme}://{requestReferrer.Host}";
                    if (!context.Response.Headers.Contains(AccessControlAllowOrigin))
                    {
                        context.Response.Headers.Add(AccessControlAllowOrigin, requestHost);
                    }
                    if (!context.Response.Headers.Contains(AccessControlAllowCredentials))
                    {
                        context.Response.Headers.Add(AccessControlAllowCredentials, "true");
                    }
                }

            }
            context.Response.Content = new StringContent(JsonConvert.SerializeObject(result, Formatting.Indented));
            //base.OnException(context);
        }

        private string GetCookiesContent(HttpCookieCollection httpCookieCollection)
        {
            string str = string.Empty;
            for (int i = 0; i < httpCookieCollection.AllKeys.Length; i++)
            {
                var httpCookie = httpCookieCollection[httpCookieCollection.Keys[i]];
                if (httpCookie != null)
                    str += httpCookieCollection.Keys[i].ToString() + "=" +
                           httpCookie.Value + "&";
            }
            return str.TrimEnd('&');
        }
    }
}