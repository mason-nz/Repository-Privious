using System;
using System.Linq;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace XYAuto.ChiTu2018.API.Filter
{
    /// <summary>
    /// auth:lixiong
    /// desc:用于记录api接口详细
    /// </summary>
    public class ApiLogAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private readonly string logKey = "requestEndLog";
        private readonly string timingKey = "__action_duration__";
        private readonly string requestArguments = "__ActionArguments__";

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (SkipLogging(actionContext))//是否该类标记为NoLog
            {
                return;
            }
            actionContext.Request.Properties[requestArguments] =
                Newtonsoft.Json.JsonConvert.SerializeObject(actionContext.ActionArguments);
            //记录进入请求的时间

            actionContext.Request.Properties[timingKey] = DateTime.Now;
            actionContext.Request.Properties[logKey] = DateTime.Now.ToBinary();
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.Request.Properties.ContainsKey(logKey))
            {
                return;
            }
            LogExcuteManage(actionExecutedContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public void LogExcuteManage(HttpActionExecutedContext actionExecutedContext)
        {
            //请求参数
            var requestResult = actionExecutedContext.Request.Properties[requestArguments];

            //获取response响应的执行时间
            var stopWatch = (DateTime)actionExecutedContext.Request.Properties[timingKey];
            var excuteTime = (DateTime.Now - stopWatch).TotalMilliseconds;

            //获取response响应的结果
            var executeResult = string.Empty;
            if (actionExecutedContext.Response != null)
            {
                executeResult = actionExecutedContext.Response.Content != null ? actionExecutedContext.Response.Content.ReadAsStringAsync().Result : string.Empty;
            }
            var requetLog = string.Format("{1}请求的url地址：{0}{1}客户端IP:{5}{1}执行的时间:{2}ms{1}请求的参数:{3}{1}返回的结果:{4}{1}",
                actionExecutedContext.Request.RequestUri,
                System.Environment.NewLine, excuteTime, requestResult, executeResult,
                XYAuto.CTUtils.Html.RequestHelper.GetIpAddress());
            var log = requetLog;

            ThreadPool.QueueUserWorkItem(t => XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(log), null);
            //var getWebApiConfig = WebConfigManage.GetWebApiConfig();
            //if (excuteTime <= getWebApiConfig.AppSettingExcuteNoticWarnTime) return;
            //{
            //    requetLog = string.Format("{0}api执行时间警告:超出了设定的时间:{1}ms{0}", System.Environment.NewLine, getWebApiConfig.AppSettingExcuteNoticWarnTime) + requetLog;
            //    ThreadPool.QueueUserWorkItem(t => _logger.Warn(requetLog), null);
            //}
        }

        private static bool SkipLogging(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>().Any() ||
                    actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<NoLogAttribute>().Any();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class NoLogAttribute : Attribute
    {
    }
}