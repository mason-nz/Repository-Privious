using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;
using XYAuto.POBU.Chitunion2018.AdminWebAPI.Common;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI
{
    public class CrossSiteAttribute : ActionFilterAttribute
    {
        private readonly string crossKey = "__action_cross__";
        private readonly string crossValue = "__action_cross_value__";
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private static readonly string OriginHeaderdefault = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("AccessAllowOrigin", false);
        private bool IsSetCrossSite = bool.Parse(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("IsSetCrossSite", false));

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (IsSetCrossSite)
            {
                var requestReferrer = actionContext.Request.Headers.Referrer;
                if (requestReferrer != null)
                {
                    var requestHost = $"{requestReferrer.Scheme}://{requestReferrer.Host}";
                    var supportCredentials = VerifyAccessAllowOrigin(requestHost);
                    actionContext.Request.Properties[crossKey] = supportCredentials;
                    actionContext.Request.Properties[crossValue] = requestHost;
                    if (!supportCredentials)
                    {
                        var result = new JsonResult
                        {
                            Status = (int)HttpStatusCode.InternalServerError,
                            Message = $"not allow request",
                            Result = null
                        };
                        actionContext.Response = new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.BadGateway,
                            Content = new StringContent(JsonConvert.SerializeObject(result, Formatting.Indented))
                        };
                        return;
                    }
                    //actionContext.Response.Headers.Add(AccessControlAllowOrigin, requestHost);
                    //actionContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                }
            }
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (IsSetCrossSite && actionExecutedContext.Response != null)
            {
                if (actionExecutedContext.Request.Properties.ContainsKey(crossKey))
                {
                    var requestReferrer = actionExecutedContext.Request.Headers.Referrer;
                    if (requestReferrer != null)
                    {
                        var requestHost = actionExecutedContext.Request.Properties[crossValue];
                        if (requestHost != null)
                        {
                            actionExecutedContext.Response.Headers.Add(AccessControlAllowOrigin, requestHost.ToString());
                            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                        }
                    }
                }

            }
            base.OnActionExecuted(actionExecutedContext);
        }

        public static bool VerifyAccessAllowOrigin(string requestHost)
        {
            var supportCredentials = false;
            OriginHeaderdefault.Split(',').ForEach(t =>
            {
                if (t.Equals(requestHost))
                {
                    supportCredentials = true;
                }
            });
            return supportCredentials;
        }
    }
}