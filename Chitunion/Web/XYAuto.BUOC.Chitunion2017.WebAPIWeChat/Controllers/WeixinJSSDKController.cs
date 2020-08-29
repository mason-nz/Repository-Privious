using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]

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
                    return Common.Util.GetJsonDataByResult(null, "在微信外部", -1);
                }
                else
                {
                    return Common.Util.GetJsonDataByResult(null, "在微信内部");
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[SideInWeiXinBrowser jssdkUiPackage]:" + ex.ToString() + "");
                return Common.Util.GetJsonDataByResult(null, "Fail", -1);
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
                WeiXinInfoRespDTO info = new WeiXinInfoRespDTO();
                info.AppId = jssdkUiPackage.AppId;
                info.NonceStr = jssdkUiPackage.NonceStr;
                info.Signature = jssdkUiPackage.Signature;
                info.Timestamp = jssdkUiPackage.Timestamp;
                return Common.Util.GetJsonDataByResult(info, "Success", 0);

            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[GetInfo jssdkUiPackage]:" + ex.ToString() + "");
                return Common.Util.GetJsonDataByResult(null, "Fail", -1);
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
            string pathimg = string.Empty;
            string localimg = string.Empty;
            try
            {
                int userId = -2;
                try
                {
                    userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception ex)
                {
                    userId = 1625;
                }
                Loger.Log4Net.Info("[GetInvitationMediaId]: userId=" + userId);
                //var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
                //if (user == null)
                //    return Common.Util.GetJsonDataByResult(null, "操作失败：用户不存在", 0);
                string userQC = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode(appId, userId, Senparc.Weixin.MP.QrCode_ActionName.QR_SCENE);
                string invitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(userQC, userId);
                InvitationQRAndUrlRepDTO entity = new InvitationQRAndUrlRepDTO { MediaID = "", InvitationQR = invitationQR };
                return Common.Util.GetJsonDataByResult(entity, "Success", 0);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[GetInvitationMediaId]: " + localimg + "  ex:" + ex.ToString() + "", ex);
                return Common.Util.GetJsonDataByResult(null, "GetInvitationMediaId接口出错：" + ex.Message, -1);
            }
        }

        /// <summary>
        /// •分享动作保存日志接口，用户在微信里点击分享触发记录日志
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult ShareLog([FromUri] string url)
        {
            try
            {
                string errorMsg = string.Empty;
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(userId);
                Loger.Log4Net.Info($"[ShareLog]UserID:{userId},UserOpenID:{user.openid},url:{url}");
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult("操作成功") : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info($"ShareLog出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
    }
}
