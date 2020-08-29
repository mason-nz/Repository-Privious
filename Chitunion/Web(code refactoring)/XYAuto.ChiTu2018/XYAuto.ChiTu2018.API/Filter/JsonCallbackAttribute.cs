using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace XYAuto.ChiTu2018.API.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonCallbackAttribute: ActionFilterAttribute
    {
        private const string CallbackQueryParameter = "jsonpcallback";

        /// <summary>
        /// 在调用操作方法之后发生。
        /// </summary>
        /// <param name="context">操作执行的上下文。</param>
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            string callback;

            if (IsJsonp(out callback))
            {
                var jsonBuilder = new StringBuilder(callback);
                jsonBuilder.AppendFormat("({0})", context.Response.Content.ReadAsStringAsync().Result);
                context.Response.Content = new StringContent(jsonBuilder.ToString());
            }

            base.OnActionExecuted(context);
        }

        private bool IsJsonp(out string callback)
        {
            callback = System.Web.HttpContext.Current.Request.QueryString[CallbackQueryParameter];
            return !string.IsNullOrEmpty(callback);
        }
    }
}