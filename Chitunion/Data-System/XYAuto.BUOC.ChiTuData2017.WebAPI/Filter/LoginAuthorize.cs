using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Filter
{
    public class LoginAuthorize : AuthorizeAttribute
    {
        private string SYSID = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ThisSysID", true);

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

        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string originHeaderdefault = "http://data1.chitunion.com";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            Loger.Log4Net.Info(string.Format("[IsAuthorized]IsCheckIP={0},IsCheckLogin={1},CheckModuleRight={2},用户请求的地址={3},UrlRefer={4}",
                IsCheckIP, IsCheckLogin, CheckModuleRight,
                HttpContext.Current.Request.Url, HttpContext.Current.Request.UrlReferrer));
            bool flag = true;
            //int loginUserid = -1;
            //int category = -1;
            ITSC.Chitunion2017.Common.LoginUser lu = new ITSC.Chitunion2017.Common.LoginUser();

            if (IsCheckLogin)
            {
                flag = ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                //flag = Chitunion2017.Common.UserInfo.IsLogin(out loginUserid, out category);
                actionContext.ActionArguments.Add("CheckLogin", flag);
            }
            if (IsCheckIP)
            {
                bool f2 = Common.Util.CheckIP();
                flag = f2 & flag;
                actionContext.ActionArguments.Add("CheckIP", f2);
            }
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
                Loger.Log4Net.Info(logMsg + string.Format(",CheckLogin={0}", bool.Parse(actionContext.ActionArguments["CheckLogin"].ToString())));
            }
            if (actionContext.ActionArguments.ContainsKey("CheckIP"))
            {
                Loger.Log4Net.Info(logMsg + string.Format(",CheckIP={0},用户IP={1},用户请求的地址={2},UrlRefer={3}",
                    bool.Parse(actionContext.ActionArguments["CheckIP"].ToString()),
                    HttpContext.Current.Request.UserHostAddress,
                    HttpContext.Current.Request.Url,
                    HttpContext.Current.Request.UrlReferrer));
            }
            if (actionContext.ActionArguments.ContainsKey("CheckModuleRight"))
            {
                Loger.Log4Net.Info(logMsg + string.Format(",CheckModuleRight={0}", actionContext.ActionArguments["CheckModuleRight"].ToString()));
                statusCode = HttpStatusCode.OK;
                unAuthenResult = new JsonResult()
                {
                    Status = (int)HttpStatusCode.OK,
                    Message = "您没有该功能权限",
                    Result = false
                };
            }

            actionContext.Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(unAuthenResult)),
            };

            if (!actionContext.Response.Headers.Contains(AccessControlAllowOrigin))
            {
                actionContext.Response.Headers.Add(AccessControlAllowOrigin, originHeaderdefault);
            }
            if (!actionContext.Response.Headers.Contains(AccessControlAllowCredentials))
            {
                actionContext.Response.Headers.Add(AccessControlAllowCredentials, "true");
            }
        }
    }
}