using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Routing;
using XYAuto.ChiTu2018.Service.App;
using XYAuto.ChiTu2018.Service.App.AppInfo;
using XYAuto.ChiTu2018.Service.App.AutoMapperConfig;

namespace XYAuto.ChiTu2018.API.App
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            MapperConfig.Configure();
            
            OptionBootStarp boot = new OptionBootStarp(OptionBootStarpManage.GetAssemblies(LoadAssembType.ApiOrService));
            boot.Initialize();
        }
    }
}
