using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using XY.Framework;
using XY.Framework.Messaging.Bus.Client;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Handler;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MediaMapperConfig.Configure();//配置autpMapper

            var receiver = ServiceBase.GetSingletonInstance<MessageBusReceiver>();
            receiver.Register<KrPayAsyncNoteHandler>();
        }
    }
}
