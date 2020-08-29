using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Filter;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //配置返回的时间类型数据格式
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                }
            );
            //添加JsonP过滤器
            GlobalConfiguration.Configuration.Filters.Add(new JsonCallbackAttribute());

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
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
