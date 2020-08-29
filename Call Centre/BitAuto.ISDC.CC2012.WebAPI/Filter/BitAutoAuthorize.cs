using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Properties;
using System.Web.Http;
using System.Web.Http.Routing;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using System.Configuration;
using Newtonsoft.Json;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.WebAPI.Helper;

namespace BitAuto.ISDC.CC2012.WebAPI.Filter
{
    public class BitAutoAuthorizeAttribute : AuthorizeAttribute
    {
        public bool NeedCheckIP = false;

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (NeedCheckIP)
            {
                if (CommonHelper.CheckIP())
                {
                    return true;
                }
            }
            var t = BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin();
            return t;
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            Loger.Log4Net.Info(string.Format("用户验证失败，用户IP {0}, 用户请求的地址为 {1},UrlRefer为 {2}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.Url, HttpContext.Current.Request.UrlReferrer));

            var s = "http://" + actionContext.Request.RequestUri.Authority + "/HTMLS/Partial/Login";
            var unAuthenResult = new CommonJsonResult() { ErrorNumber = -1000, ErrorMsg = string.Format("请先登录,登录地址为 {0}", s), Success = false };
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
            {
                Content = new StringContent(JsonConvert.SerializeObject(unAuthenResult))
            };
        }
    }
}