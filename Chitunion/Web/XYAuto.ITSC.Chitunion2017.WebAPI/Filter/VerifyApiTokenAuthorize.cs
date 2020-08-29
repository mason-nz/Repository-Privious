/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 10:59:29
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Filter
{
    public class VerifyApiTokenAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private readonly string _embedChiTuDesStr = Utils.Config.ConfigurationUtil.GetAppSettingValue("EmbedChiTu_DesStr");
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string OriginHeaderdefault = "http://client.chitunion.com";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        /// <summary>
        /// 获取request
        /// </summary>
        /// <returns></returns>
        protected HttpRequestBase GetRequest(HttpActionContext actionContext)
        {
            HttpContextBase context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            return request;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var context = GetRequest(actionContext);
            var accessToken = context.QueryString["AccessToken"];
            var p = context.QueryString["p"];
            var appid = context.QueryString["appid"].ToInt();
            var sign = context.QueryString["sign"];
            var requetLog = string.Format("{0}请求的url地址：{1}{0}请求的校验信息:{2}{0}",
                        System.Environment.NewLine, actionContext.Request.RequestUri,
                        $"AccessToken={accessToken}&p={p}&appid={appid}&sign={sign}");

            Loger.Log4Net.Info(requetLog);

            //验签的开关
            if (!AppSettingsForZhy.Instance.ZhySignOff)
            {
                base.OnActionExecuting(actionContext);
                return;
            }
            if (string.IsNullOrWhiteSpace(p))
            {
                Hand(actionContext, "sign校验失败,请输入参数：p");
                return;
            }
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                if (!ValidateTicket(accessToken))
                {
                    Hand(actionContext, "AccessToken验证信息过期或不存在");
                }
                else
                {
                    p = p.Replace(" ", "+");
                    var md5Code = MD5Hash(appid + accessToken + p + _embedChiTuDesStr, Encoding.UTF8);
                    if (md5Code != sign)
                    {
                        Hand(actionContext, "sign校验失败");
                        return;
                    }
                    Chitunion2017.Common.Entities.AuthorizeLogin model = new Chitunion2017.Common.Entities.AuthorizeLogin()
                    {
                        APPID = appid,
                        MD5Code = accessToken
                    };
                    Chitunion2017.Common.AuthorizeLogin.Instance.Update(model);
                }
            }
            else
            {
                Hand(actionContext, "请输入accessToken相关参数");
            }
            base.OnActionExecuting(actionContext);
        }

        private string MD5Hash(string strText, Encoding encoding)
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = encoding.GetBytes(strText);
            byte[] buffer2 = md.ComputeHash(bytes);
            string str = null;
            for (int i = 0; i < buffer2.Length; i++)
            {
                string str2 = buffer2[i].ToString("x");
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                str = str + str2;
            }
            return str;
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string accessToken)
        {
            //解密Ticket
            return Chitunion2017.Common.AuthorizeLogin.Instance.Verification(accessToken);
        }

        private void Hand(HttpActionContext actionContext, string message)
        {
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;
            var unAuthenResult = new JsonResult()
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = $"身份验证失败|Msg:{message}",
                Result = false
            };
            //记录身份验证
            var requetLog = string.Format("{0}请求的url地址：{1}{0}返回的结果:{2}{0}",
                System.Environment.NewLine, actionContext.Request.RequestUri, unAuthenResult.Message);
            Loger.Log4Net.Warn(requetLog);

            actionContext.Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(unAuthenResult)),
            };

            if (!actionContext.Response.Headers.Contains(AccessControlAllowOrigin))
            {
                actionContext.Response.Headers.Add(AccessControlAllowOrigin, OriginHeaderdefault);
            }
            if (!actionContext.Response.Headers.Contains(AccessControlAllowCredentials))
            {
                actionContext.Response.Headers.Add(AccessControlAllowCredentials, "true");
            }
        }
    }

    public class VerifyApiTokenAuthorize : AuthorizeAttribute
    {
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string OriginHeaderdefault = "http://client.chitunion.com";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;
            var unAuthenResult = new JsonResult()
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Message = "Token身份验证失败",
                Result = false
            };
            actionContext.Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(unAuthenResult)),
            };

            if (!actionContext.Response.Headers.Contains(AccessControlAllowOrigin))
            {
                actionContext.Response.Headers.Add(AccessControlAllowOrigin, OriginHeaderdefault);
            }
            if (!actionContext.Response.Headers.Contains(AccessControlAllowCredentials))
            {
                actionContext.Response.Headers.Add(AccessControlAllowCredentials, "true");
            }
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string accessToken)
        {
            //解密Ticket
            return Chitunion2017.Common.AuthorizeLogin.Instance.Verification(accessToken);
        }
    }
}