using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BitAuto.ISDC.CC2012.WebAPI.Filter;

namespace BitAuto.ISDC.CC2012.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new WebApiExceptionFilter());


            config.Routes.MapHttpRoute(
               name: "DefaultActionApi",
               routeTemplate: "api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional }
              );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "HollyApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
