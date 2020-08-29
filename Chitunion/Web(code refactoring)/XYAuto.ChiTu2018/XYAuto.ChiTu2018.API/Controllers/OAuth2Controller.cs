using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Service.OAuth2;
using XYAuto.ChiTu2018.Service.OAuth2.Dto;
using XYAuto.ChiTu2018.Service.User;
using XYAuto.ChiTu2018.Service.Wechat;
using XYAuto.CTUtils.Log;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.API.Controllers
{
    public class OAuth2Controller : ApiController
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private string Domin = ConfigurationManager.AppSettings["Domin"];


        #region 临时测试用注释掉了
        /// <summary>
        /// 临时登陆（测试用，发布到线上时需要注释掉）
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [HttpGet]
        [CrossSite]
        public JsonResult MoniLogin(string userId)
        {
            int i = -1;
            string cookiesVal = string.Empty;
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out i))
            {
                cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(i);
            }
            return Common.Util.GetJsonDataByResult(cookiesVal);
        }
        #endregion
        /// <summary>
        /// 判断当前访问环境是否在微信端，若再的话，返回授权URL，需要前端配合进行跳转；若不在则返回空
        /// </summary>
        /// <returns></returns>
        [CrossSite]
        [HttpGet]
        [ApiLog]
        public JsonResult WeiXinCheck()
        {
            bool result = false;
            var url = HttpContext.Current.Request.UrlReferrer;
            if (url != null)
            {
                string resurl = $"{Domin}{"/api/OAuth2/Index?returnUrl="}{HttpUtility.UrlEncode(url.AbsoluteUri)}";

                var userAgent = HttpContext.Current.Request.UserAgent;
                if (string.IsNullOrWhiteSpace(userAgent) || (!userAgent.Contains("MicroMessenger")))//不在微信环境
                {
                    result = true;
                }
                else//在微信环境时
                {
                    //context.Response.Write($"document.write(\"<sc\"+\"ript> window.location='{resurl}';" + "</scr\"+\"ipt>\");");
                    XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
                    bool flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                    if (flag && lu != null && lu.Category == 29002)
                    {
                        LeWeiXinVisvitLogService.Instance.AddWeiXinVisvitInfo(lu.UserID, url.AbsoluteUri,
                            HttpContext.Current.Request.UserAgent);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }

                if (!result)//false，为微信端，需要授权；true，为不需要授权
                {
                    return Common.Util.GetJsonDataByResult(resurl, $"操作失败:为微信端，需要授权", -1);
                }
            }
            return Common.Util.GetJsonDataByResult(null);
        }

        [CrossSite]
        [HttpGet]
        public JsonResult CheckLogin()
        {
            var jsonResult = new JsonResult();
            var webHttp = HttpContext.Current;
            const string cookieXyApp = "xy_usertoken";
            var flag = false;//是否登陆标记

            Dictionary<string, object> ctLogin = new Dictionary<string, object>
            {
                {"UserID", -1},
                {"IsLogin", false},
                {"UserName", string.Empty},
                {"Mobile", string.Empty},
                {"TypeID", -1},
                {"RoleIDs", string.Empty},
                {"Category", -1},
                {"Source", -1},
                {"RegisterType", -1},
                {"WX_Nickname", string.Empty},
                {"WX_HeadimgUrl", string.Empty}
            };
            jsonResult.Status = -1;

            if (webHttp.Request.Cookies[cookieXyApp] != null)//汽车大全App_Cookies验证 
            {
                flag = false;
                var user = UserManageService.Instance.GetUserInfoByToken(webHttp.Request.Cookies[cookieXyApp].Value);
                if (user != null)
                {
                    ctLogin["UserID"] = user.UserID;
                    ctLogin["IsLogin"] = true;
                    ctLogin["UserName"] = user.UserName;
                    ctLogin["Mobile"] = user.Mobile;
                    ctLogin["TypeID"] = user.Type;
                    ctLogin["RoleIDs"] = string.Empty;
                    ctLogin["Category"] = user.Category;
                    ctLogin["Source"] = user.Source;
                    ctLogin["RegisterType"] = user.RegisterType;
                    ctLogin["WX_Nickname"] = string.Empty;
                    ctLogin["WX_HeadimgUrl"] = user.HeadimgURL;
                    jsonResult.Status = 0;
                }
            }
            else//网站Cookies验证
            {
                LoginUser lu;
                flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                LE_WeiXinUser weixinUser = null;
                if (flag && lu != null && (lu.Category == (int)UserCategoryEnum.媒体主 || lu.Category == (int)UserCategoryEnum.广告主))
                {
                    weixinUser = new LEWeiXinUserBO().GetModelByUserId(lu.UserID);
                }
                if (flag && lu != null)
                {
                    ctLogin["UserID"] = lu.UserID;
                    ctLogin["IsLogin"] = true;
                    ctLogin["UserName"] = lu.UserName;
                    ctLogin["Mobile"] = lu.Mobile;
                    ctLogin["TypeID"] = lu.Type;
                    ctLogin["RoleIDs"] = lu.RoleIDs;
                    ctLogin["Category"] = lu.Category;
                    ctLogin["Source"] = lu.Source;
                    ctLogin["RegisterType"] = (lu.RegisterType > 0 ? lu.RegisterType : -1);
                    ctLogin["WX_Nickname"] = (weixinUser != null ? weixinUser.nickname : "");
                    ctLogin["WX_HeadimgUrl"] = (weixinUser != null ? weixinUser.headimgurl : "");
                    jsonResult.Status = 0;
                }
            }
            // todo:数据库重构后修改
            //var url = HttpContext.Current.Request.UrlReferrer;
            //if (url != null && int.Parse(ctLogin["UserID"].ToString()) > 0)
            //{
            //    ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(int.Parse(ctLogin["UserID"].ToString()), url.AbsoluteUri, HttpContext.Current.Request.UserAgent);
            //}

            jsonResult.Result = ctLogin;
            return jsonResult;
        }
        /// <summary>
        /// 记录访问日志接口
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [CrossSite]
        public JsonResult LogRequest()
        {
            if (HttpContext.Current != null)
            {
                var uriRefer = HttpContext.Current.Request.UrlReferrer ?? HttpContext.Current.Request.Url;
                var userAgent = HttpContext.Current.Request.UserAgent;
                string ip = XYAuto.CTUtils.Html.RequestHelper.GetIpAddress();
                string msg = $"当前请求URL：{uriRefer},UserAgent:{userAgent},IP:{ip}";

                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(msg);
            }
            return Common.Util.GetJsonDataByResult("OK");
        }

        /// <summary>
        /// 获取微信授权访问页面（自动回调）
        /// </summary>
        /// <param name="returnUrl">用户尝试进入的需要登录的页面</param>
        [ApiLog]
        [HttpGet]
        public void Index(string returnUrl)
        {
            try
            {
                var session = HttpContext.Current.Session;

                string redirectUri = $"{Domin}{"/api/OAuth2/UserInfoCallback"}?returnUrl={returnUrl}";
                var state = "ChiTu" + DateTime.Now.Millisecond;
                session["state"] = state;
                var redirect = OAuthApi.GetAuthorizeUrl(appId, redirectUri, state, OAuthScope.snsapi_userinfo);
                Log4NetHelper.Default().Info("OAuth2Controller>Index :" + redirect);
                HttpContext.Current.Response.Redirect(redirect);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("OAuth2_Index 出错：" + ex.Message);
                Log4NetHelper.Default().Error(ex);
            }
        }

        [ApiLog]
        [HttpGet]
        public void UserInfoCallback(string code, string state, string returnUrl)
        {
            Log4NetHelper.Default().Info("UserInfoCallback :" + returnUrl);
            string redirectIndex = $"{Domin}{"/api/OAuth2/UserInfoCallback?returnUrl="}{returnUrl}";
            if (HttpContext.Current.Session["state"] != null && HttpContext.Current.Session["state"].ToString() != state)
            {
                HttpContext.Current.Session["state"] = null;
                HttpContext.Current.Response.Write("请重新进入");
            }
            HttpContext.Current.Session["state"] = null;
            //如果code返回的是个空值，则需要回到授权界面，重新授权
            if (string.IsNullOrEmpty(code))
            {
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Redirect(redirectIndex);
            }
            //通过回调函数返回的code来获取令牌  ，如果不懂可单步执行，看url的变化
            Log4NetHelper.Default().Info($"UserInfoCallback :GetAccessToken开始，Code：{code}");
            var accessToken = OAuthApi.GetAccessToken(appId, secret, code);
            Log4NetHelper.Default().Info($"UserInfoCallback :GetAccessToken结束，Code：{code}");
            if (accessToken.errcode != ReturnCode.请求成功)
            {
                //如果令牌的错误信息不等于请求成功，则需要重新返回授权界面
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Redirect(redirectIndex);
            }
            var isAuth = OAuthApi.Auth(accessToken.access_token, accessToken.openid);
            Log4NetHelper.Default().Info($"调用OAuthApi.Auth方法，参数access_token={accessToken.access_token},openid={accessToken.openid},结果={(isAuth != null ? isAuth.errmsg : "对象为空")}");

            #region 获取用户code
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(accessToken.access_token, accessToken.openid);
                Log4NetHelper.Default().Info($"调用OAuthApi.GetUserInfo方法，参数access_token={accessToken.access_token},openid={accessToken.openid},结果={(userInfo != null ? JsonConvert.SerializeObject(userInfo) : "对象为空")}");
                var userInfobase = UserApi.Info(appId, userInfo.openid, Language.zh_CN);
                Log4NetHelper.Default().Info($"调用Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info方法，参数appId={appId},openid={userInfo.openid},结果={(userInfobase != null ? JsonConvert.SerializeObject(userInfobase) : "对象为空")}");

                var user = LEWeiXinUserService.Instance.GetUnionAndUserId(userInfo.openid);
                Log4NetHelper.Default().Info($"调用 XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId方法，参数openid={userInfo.openid},结果={(user != null ? JsonConvert.SerializeObject(user) : "对象为空")}");

                if (!LEWeiXinUserService.Instance.IsExistOpenId(userInfo.openid))
                {
                    Log4NetHelper.Default().Info("OAuth2Controller>UserInfoCallback :IsExistOpneId IS false");
                    var wxuser = new WeiXinUserDto
                    {
                        subscribe = userInfobase.subscribe,
                        openid = userInfo.openid,
                        nickname = userInfo.nickname,
                        sex = userInfo.sex,
                        city = userInfo.city,
                        country = userInfo.country,
                        province = userInfo.province,
                        language = userInfobase.language,
                        headimgurl = userInfo.headimgurl,
                        subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfobase.subscribe_time),
                        unionid = userInfo.unionid,
                        remark = userInfobase.remark,
                        groupid = userInfobase.groupid,
                        tagid_list = string.Join(",", userInfobase.tagid_list),
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                        AuthorizeTime = DateTime.Now,
                        Status = userInfobase.subscribe == 1 ? 0 : -1,
                        RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号,
                        RegisterType = (int)RegisterTypeEnum.微信
                    };

                    Log4NetHelper.Default().Info("OAuth2Controller>UserInfoCallback :WeiXinUserOperation BEGIN");
                    #region 获取用户的渠道ID
                    wxuser.PromotionChannelID = OAuth2Service.Instance.GetPromotionChannelId(returnUrl);
                    #endregion

                    user = LEWeiXinUserService.Instance.WeiXinUserOperation(wxuser);
                    if (user != null)
                    {
                        LeInviteRecordService.Instance.FriendFollow((int)user.UserID);
                    }

                }
                else
                {
                    LEWeiXinUserService.Instance.UpdateStatusByOpenId(0, DateTime.Now, userInfo.openid);
                }
                if (user != null && user.UserID > 0)
                {
                    UserInfo.Instance.Passport((int)user.UserID);
                    LeWeiXinVisvitLogService.Instance.AddWeiXinVisvitInfo((int)user.UserID, returnUrl,
                        HttpContext.Current.Request.UserAgent);
                }
                //场景判断
                if (LEWeiXinUserService.Instance.GetUserSence((int)user.UserID))
                {
                    Log4NetHelper.Default().Info($"调用XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserSence方法,参数UserID={user.UserID},没有场景，返回returnUrl={returnUrl}");
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Clear();
                }
                returnUrl = returnUrl + (returnUrl.IndexOf('?') >= 0 ? "&" : "?") + "isauth=1";
                HttpContext.Current.Response.Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("OAuth2 Exception:" + ex.ToString());
            }
            #endregion
        }
    }
}
