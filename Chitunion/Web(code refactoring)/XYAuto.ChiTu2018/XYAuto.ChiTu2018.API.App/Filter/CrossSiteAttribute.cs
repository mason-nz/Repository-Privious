using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.App.Models;

namespace XYAuto.ChiTu2018.API.App.Filter
{
    /// <summary>
    /// 跨域条件设置
    /// </summary>
    public class CrossSiteAttribute : ActionFilterAttribute
    {
        private readonly string crossKey = "__action_cross__";
        private readonly string crossValue = "__action_cross_value__";
        private readonly string _accessControlAllowOrigin = "Access-Control-Allow-Origin";
        private readonly string _originHeaderdefault = ConfigurationManager.AppSettings["AccessAllowOrigin"];
        private readonly bool _isSetCrossSite = bool.Parse(ConfigurationManager.AppSettings["IsSetCrossSite"]);

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_isSetCrossSite)
            {
                var requestReferrer = actionContext.Request.Headers.Referrer;
                if (requestReferrer != null)
                {
                    var requestHost = $"{requestReferrer.Scheme}://{requestReferrer.Host}{(requestReferrer.Port != 80 ? (":" + requestReferrer.Port) : "")}";
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

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (_isSetCrossSite && actionExecutedContext.Response != null)
            {
                if (actionExecutedContext.Request.Properties.ContainsKey(crossKey))
                {
                    var requestReferrer = actionExecutedContext.Request.Headers.Referrer;
                    if (requestReferrer != null)
                    {
                        var requestHost = actionExecutedContext.Request.Properties[crossValue];
                        if (requestHost != null)
                        {
                            actionExecutedContext.Response.Headers.Add(_accessControlAllowOrigin, requestHost.ToString());
                            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                        }
                    }
                }

            }
            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// 校验是否在允许跨域的白名单中
        /// </summary>
        /// <param name="requestHost"></param>
        /// <returns></returns>
        public bool VerifyAccessAllowOrigin(string requestHost)
        {
            var supportCredentials = false;
            foreach (var t in _originHeaderdefault.Split(','))
            {
                if (t.Equals(requestHost))
                {
                    supportCredentials = true;
                }
            }

            return supportCredentials;
        }
    }
}