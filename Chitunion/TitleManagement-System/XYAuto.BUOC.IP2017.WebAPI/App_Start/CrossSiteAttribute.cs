using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace XYAuto.BUOC.IP2017.WebAPI.App_Start
{
    public class CrossSiteAttribute : ActionFilterAttribute
    {
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string originHeaderdefault = "http://client.ip1.chitunion.com";
        private bool IsSetCrossSite = bool.Parse(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("IsSetCrossSite", false));
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (IsSetCrossSite && actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add(AccessControlAllowOrigin, originHeaderdefault);
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }

    public class CrossSiteDataAttribute : ActionFilterAttribute
    {
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private string originHeaderdefault = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("DataCrossDoaminUrl", false);
        private bool IsSetCrossSite = bool.Parse(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("IsSetCrossSite", false));
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (IsSetCrossSite && actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add(AccessControlAllowOrigin, originHeaderdefault);
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}