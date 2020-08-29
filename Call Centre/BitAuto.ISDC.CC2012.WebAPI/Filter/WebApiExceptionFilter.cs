using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using Newtonsoft.Json;


namespace BitAuto.ISDC.CC2012.WebAPI.Filter
{
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var result = new CommonJsonResult();
            result.ErrorNumber = context.Exception.GetHashCode();
            result.ErrorMsg = context.Exception.Message;
            if (context.Exception.InnerException != null)
            {
                result.ErrorMsg = context.Exception.InnerException.Message;
            }
            Loger.Log4Net.Info(string.Format("系统内部异常，用户IP {0}, 用户请求的地址为 {1},UrlRefer为 {2}, 异常内容{3}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.UrlReferrer, result.ErrorMsg));
            context.Response = new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(result, Formatting.Indented)) };
        }
    }
}