using System.Web.Http;
using XYAuto.ChiTu2018.PublicApi.Filter;

namespace XYAuto.ChiTu2018.PublicApi
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

            //根据参数中的版本号，选择指定的Controller，自定义Route 此方法不生效，得重写
            //config.Services.Replace(typeof(IHttpControllerSelector), new VersionControllerSelector(config));

            config.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());

            //去掉XML格式
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
