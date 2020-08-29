using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using Senparc.Weixin.MP.Helpers;
using XYAuto.ChiTu2018.API.Common;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Service.WeixinJSSDK.Dto.GetInvitationMediaId;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.API.Controllers
{
    public class WeixinJSSDKController : ApiController
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        [ApiLog]
        [HttpGet]
        public JsonResult SideInWeiXinBrowser()
        {
            try
            {
                var userAgent = HttpContext.Current.Request.UserAgent;
                if (string.IsNullOrWhiteSpace(userAgent) || (!userAgent.Contains("MicroMessenger") && !userAgent.Contains("Windows Phone")))
                {
                    return Util.GetJsonDataByResult(null, "在微信外部", -1);
                }
                else
                {
                    return Util.GetJsonDataByResult(null, "在微信内部");
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("[SideInWeiXinBrowser jssdkUiPackage]:" + ex.ToString() + "");
                return Util.GetJsonDataByResult(null, $"接口出错：{ex}", -1);
            }
        }
        [ApiLog]
        [HttpGet]
        public JsonResult GetInfo(string url)
        {
            try
            {
                string surl = string.Empty;
                if (url.Length > 0)
                {
                    surl = url;
                }
                else
                {
                    surl = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString() : HttpContext.Current.Request.Url.AbsoluteUri;
                }

                var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, secret, surl);
                var info = new XYAuto.ChiTu2018.API.Models.WeixinJSSDK.GetInfoDto
                {
                    AppId = jssdkUiPackage.AppId,
                    NonceStr = jssdkUiPackage.NonceStr,
                    Signature = jssdkUiPackage.Signature,
                    Timestamp = jssdkUiPackage.Timestamp
                };
                return Common.Util.GetJsonDataByResult(info, "Success", 0);

            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[GetInfo jssdkUiPackage]:" + ex.ToString() + "");
                return Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 获取邀请图片的mediaid
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetInvitationMediaId()
        {
            //    try
            //    {
            //        userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            //    }
            //    catch (Exception ex)
            //    {
            //        userId = 1625;
            //    }

            try
            {
                var errorMsg = string.Empty;
                var ret =
                    Service.WeixinJSSDK.WeixinJSSDKService.Instance.GetInvitationMediaId(new ReqDto()
                    {
                        appId = appId,
                        secret = secret
                    }, out errorMsg);
                return !string.IsNullOrEmpty(errorMsg)
                    ? Util.GetJsonDataByResult(null, $"操作失败:{errorMsg}", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetInvitationMediaId]报错", ex);
                return Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 分享动作保存日志接口，用户在微信里点击分享触发记录日志
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ShareLog([FromUri] string url)
        {            
            try
            {
                var errorMsg = string.Empty;
                var ret =
                    Service.WeixinJSSDK.WeixinJSSDKService.Instance.ShareLog(url, out errorMsg);
                return !string.IsNullOrEmpty(errorMsg)
                    ? Util.GetJsonDataByResult(null, $"操作失败:{errorMsg}", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error($"ShareLog出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
    }
}
