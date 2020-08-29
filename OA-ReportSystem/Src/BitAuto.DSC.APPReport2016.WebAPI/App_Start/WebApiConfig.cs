using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BitAuto.DSC.APPReport2016.WebAPI.Filter;
using System.Web.Http.Dispatcher;

namespace BitAuto.DSC.APPReport2016.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //接口中异常处理
            config.Filters.Add(new WebApiExceptionFilter());

            //根据参数中的版本号，选择指定的Controller
            config.Services.Replace(typeof(IHttpControllerSelector), new VersionControllerSelector(config));

            //处理请求时，消息过滤器（目前暂时用不到）
            //config.MessageHandlers.Add(new MessageHandler());

            //去掉XML格式
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
