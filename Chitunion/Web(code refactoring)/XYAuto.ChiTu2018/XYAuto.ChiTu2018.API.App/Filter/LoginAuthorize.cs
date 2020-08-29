using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.App.Models;

namespace XYAuto.ChiTu2018.API.App.Filter
{
    /// <summary>
    /// 登录授权校验
    /// </summary>
    public class LoginAuthorize : AuthorizeAttribute
    {

        private string SYSID = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("ThisSysID", false);
        /// <summary>
        /// 是否需要验证IP（默认需要）
        /// </summary>
        public bool IsCheckIP = true;

        /// <summary>
        /// 是否需要验证登录（默认需要）
        /// </summary>
        public bool IsCheckLogin = true;

        /// <summary>
        /// 是否验证集中权限系统中，模块、功能点权限
        /// </summary>
        public string CheckModuleRight = string.Empty;

        //private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        //private const string originHeaderdefault = "http://client.wxtest.chitunion.com";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            int userid = 0;

            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(string.Format("[IsAuthorized]IsCheckIP={0},IsCheckLogin={1},CheckModuleRight={2},用户请求的地址={3},UrlRefer={4}",
                IsCheckIP, IsCheckLogin, CheckModuleRight,
                HttpContext.Current.Request.Url, HttpContext.Current.Request.UrlReferrer));
            bool flag = true;
            //int loginUserid = -1;
            //int category = -1;
            XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new ITSC.Chitunion2017.Common.LoginUser();

            if (IsCheckLogin)
            {
                var userAgent = HttpContext.Current.Request.UserAgent;
                ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                if (lu != null && lu.Category == 29002)
                {
                    flag = true;
                    //if (!string.IsNullOrWhiteSpace(userAgent) && ((userAgent.Contains("MicroMessenger") || userAgent.Contains("Windows Phone"))))
                    //{
                    //    flag = true;
                    //}
                    //else
                    //{
                    //    flag = false;
                    //}

                }
                else
                {
                    string openid = HttpContext.Current.Request["openid"];
                    //var user = LEWeiXinUserService.Instance.GetUnionAndUserId(openid);
                    //if (user!= null && user.UserID > 0)
                    //{
                    //    userid = user.UserID.GetValueOrDefault();
                    //    XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userid);
                    //    flag = true;
                    //}
                    //else
                    //{
                    //    flag = false;
                    //}
                }
                //flag = Chitunion2017.Common.UserInfo.IsLogin(out loginUserid, out category);
                actionContext.ActionArguments.Add("CheckLogin", flag);
            }
            //if (IsCheckIP)
            //{
            //    bool f2 = Common.Util.CheckIP();
            //    flag = f2 & flag;
            //    actionContext.ActionArguments.Add("CheckIP", f2);
            //}
            if (flag && !string.IsNullOrEmpty(CheckModuleRight))
            {
                bool f3 = ITSC.Chitunion2017.Common.UserInfo.CheckRight(CheckModuleRight, SYSID);
                flag = f3 & flag;
                actionContext.ActionArguments.Add("CheckModuleRight", CheckModuleRight);
            }
            return flag;
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            string logMsg = "[AuthorizedResult]用户验证失败";
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;
            var unAuthenResult = new JsonResult()
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = "身份验证失败",
                Result = false
            };

            if (actionContext.ActionArguments.ContainsKey("CheckLogin"))
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(logMsg + string.Format(",CheckLogin={0}", bool.Parse(actionContext.ActionArguments["CheckLogin"].ToString())));
            }
            if (actionContext.ActionArguments.ContainsKey("CheckIP"))
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(logMsg + string.Format(",CheckIP={0},用户IP={1},用户请求的地址={2},UrlRefer={3}",
                    bool.Parse(actionContext.ActionArguments["CheckIP"].ToString()),
                    HttpContext.Current.Request.UserHostAddress,
                    HttpContext.Current.Request.Url,
                    HttpContext.Current.Request.UrlReferrer));
            }
            if (actionContext.ActionArguments.ContainsKey("CheckModuleRight"))
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(logMsg + string.Format(",CheckModuleRight={0}", actionContext.ActionArguments["CheckModuleRight"].ToString()));
                statusCode = HttpStatusCode.OK;
                unAuthenResult = new JsonResult()
                {
                    Status = (int)HttpStatusCode.OK,
                    Message = "功能权限验证失败",
                    Result = false
                };
            }

            actionContext.Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(unAuthenResult)),
            };

            //if (!actionContext.Response.Headers.Contains(AccessControlAllowOrigin))
            //{
            //    actionContext.Response.Headers.Add(AccessControlAllowOrigin, originHeaderdefault);
            //}
            if (!actionContext.Response.Headers.Contains(AccessControlAllowCredentials))
            {
                actionContext.Response.Headers.Add(AccessControlAllowCredentials, "true");
            }
        }
    }
}