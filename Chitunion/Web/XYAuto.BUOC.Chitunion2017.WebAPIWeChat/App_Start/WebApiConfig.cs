using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
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
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API 路由
            config.MapHttpAttributeRoutes();
            //接口中异常处理
            config.Filters.Add(new WebApiExceptionFilter());

            config.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());
            
            //config.Filters.Add(new CrossSiteAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
