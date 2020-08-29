using System;
using System.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using System.Web.Http;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Models;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.BLL;
using System.Collections.Generic;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    public class OAuth2Controller : ApiController
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private string Domin = ConfigurationManager.AppSettings["Domin"];
        private string WebDomain = ConfigurationManager.AppSettings["WebDomain"];

        #region 临时测试用注释掉了
        //[HttpGet]
        //[CrossSite]
        //public Common.JsonResult MoniLogin(string userId)
        //{
        //    int i = -1;
        //    string cookiesVal = string.Empty;
        //    if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out i))
        //    {
        //        cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(i);
        //    }
        //    return Common.Util.GetJsonDataByResult(cookiesVal);
        //}
        #endregion

        /// <summary>
        /// 判断当前访问环境是否在微信端，若再的话，返回授权URL，需要前端配合进行跳转；若不在则返回空
        /// </summary>
        /// <returns></returns>
        [CrossSite]
        [HttpGet]
        public Common.JsonResult WeiXinCheck()
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
                    var coo = HttpContext.Current.Request.Cookies["ReWriteUserIDed"];
                    if (coo == null || (coo != null && coo.Value.ToString() != "2"))
                    {
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
                    }
                    else
                    {
                        //context.Response.Write($"document.write(\"<sc\"+\"ript> window.location='{resurl}';" + "</scr\"+\"ipt>\");");
                        XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
                        bool flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                        if (flag && lu != null && lu.Category == 29002)
                        {
                            ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(lu.UserID, url.AbsoluteUri, HttpContext.Current.Request.UserAgent);
                            result = true;
                        }
                    }

                }

                if (!result)//false，为微信端，需要授权；true，为不需要授权
                {
                    return Common.Util.GetJsonDataByResult(resurl, "Fail", -1);
                }
            }
            return Common.Util.GetJsonDataByResult(null);
        }

        /// <summary>
        /// 获取当前用户登陆信息，若未登陆，则返回空的框架
        /// </summary>
        /// <returns></returns>
        [CrossSite]
        [HttpGet]
        public Common.JsonResult CheckLogin()
        {
            var jsonResult = new JsonResult();
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            string cookie_XYApp = "xy_usertoken";
            bool flag = false;//是否登陆标记

            Dictionary<string, object> CTLogin = new Dictionary<string, object>();
            CTLogin.Add("UserID", -1);
            CTLogin.Add("IsLogin", false);
            CTLogin.Add("UserName", string.Empty);
            CTLogin.Add("Mobile", string.Empty);
            CTLogin.Add("TypeID", -1);
            CTLogin.Add("RoleIDs", string.Empty);
            CTLogin.Add("Category", -1);
            CTLogin.Add("Source", -1);
            CTLogin.Add("RegisterType", -1);
            CTLogin.Add("WX_Nickname", string.Empty);
            CTLogin.Add("WX_HeadimgUrl", string.Empty);
            CTLogin.Add("IsNewUserBy1YuanTX", false);//新增1元提现活动，是否为新老用户标识
            jsonResult.Status = -1;

            if (webHttp.Request.Cookies[cookie_XYApp] != null &&
                !string.IsNullOrEmpty(webHttp.Request.Cookies[cookie_XYApp].Value))//汽车大全App_Cookies验证
            {
                flag = false;
                var user = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByToken(webHttp.Request.Cookies[cookie_XYApp].Value);
                if (user != null)
                {
                    CTLogin["UserID"] = user.UserID;
                    CTLogin["IsLogin"] = true;
                    CTLogin["UserName"] = user.UserName;
                    CTLogin["Mobile"] = user.Mobile;
                    CTLogin["TypeID"] = user.TypeID;
                    CTLogin["RoleIDs"] = string.Empty;
                    CTLogin["Category"] = user.Category;
                    CTLogin["Source"] = user.Source;
                    CTLogin["RegisterType"] = user.RegisterType;
                    CTLogin["WX_Nickname"] = string.Empty;
                    CTLogin["WX_HeadimgUrl"] = user.HeadImgUrl;
                    jsonResult.Status = 0;
                }
            }
            else//网站Cookies验证
            {
                XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
                flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                ITSC.Chitunion2017.Entities.WeChat.WeiXinUser weixinUser = null;
                if (flag && lu != null && (lu.Category == 29001 || lu.Category == 29002))
                {
                    weixinUser = XYAuto.ITSC.Chitunion2017.Dal.WeChat.WeiXinUser.Instance.GetUserInfoByUserId(lu.UserID, (UserCategoryEnum)lu.Category);
                }
                if (flag)
                {
                    CTLogin["UserID"] = lu.UserID;
                    CTLogin["IsLogin"] = flag;
                    CTLogin["UserName"] = lu.UserName;
                    CTLogin["Mobile"] = lu.Mobile;
                    CTLogin["TypeID"] = lu.Type;
                    CTLogin["RoleIDs"] = lu.RoleIDs;
                    CTLogin["Category"] = lu.Category;
                    CTLogin["Source"] = lu.Source;
                    CTLogin["RegisterType"] = (lu.RegisterType > 0 ? lu.RegisterType : -1);
                    CTLogin["WX_Nickname"] = (weixinUser != null ? weixinUser.nickname : "");
                    CTLogin["WX_HeadimgUrl"] = (weixinUser != null ? weixinUser.headimgurl : "");
                    CTLogin["IsNewUserBy1YuanTX"] = ITSC.Chitunion2017.BLL.WeChat.ActivityVerifyBll.Instance.VerifyUserByUserId(lu.UserID);
                    jsonResult.Status = 0;
                }
            }

            var url = HttpContext.Current.Request.UrlReferrer;
            if (url != null && int.Parse(CTLogin["UserID"].ToString()) > 0)
            {
                ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(int.Parse(CTLogin["UserID"].ToString()), url.AbsoluteUri, HttpContext.Current.Request.UserAgent);
            }

            jsonResult.Result = CTLogin;
            return jsonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl">用户尝试进入的需要登录的页面</param>
        /// <returns></returns>
        [HttpGet]
        public void Index(string returnUrl)
        {
            var Session = HttpContext.Current.Session;

            string redirect_uri = $"{Domin}{"/api/OAuth2/UserInfoCallback"}?returnUrl={returnUrl}";
            string state = "ChiTu" + DateTime.Now.Millisecond;
            Session["state"] = state;
            string redirect = OAuthApi.GetAuthorizeUrl(appId, redirect_uri, state, Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);
            Loger.Log4Net.Info("OAuth2Controller>Index :" + redirect);
            HttpContext.Current.Response.Redirect(redirect);

        }

        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        [HttpGet]
        public void UserInfoCallback(string code, string state, string returnUrl)
        {
            Loger.Log4Net.Info("UserInfoCallback :" + returnUrl);
            //分支合并后，需要自行修改逻辑
            string redirect_index = $"{Domin}{"/api/OAuth2/Index?returnUrl="}{returnUrl}";
            if (HttpContext.Current.Session["state"] != null && HttpContext.Current.Session["state"].ToString() != state)
            {
                HttpContext.Current.Session["state"] = null;
                HttpContext.Current.Response.Write("请重新进入");
            }
            HttpContext.Current.Session["state"] = null;
            //如果code返回的是个空值，则需要回到授权界面，重新授权

            if (string.IsNullOrEmpty(code) ||
                (string.IsNullOrEmpty(code) == false &&
                 HttpContext.Current.Session["wx_auth_code"] != null &&
                 HttpContext.Current.Session["wx_auth_code"].ToString() == code)
               )
            {
                Loger.Log4Net.Info($"UserInfoCallback :Code为空或Code重复：Code={code}，调整URL={redirect_index}");
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Redirect(redirect_index);
            }
            else
            {
                HttpContext.Current.Session["wx_auth_code"] = code;
            }
            //通过回调函数返回的code来获取令牌  ，如果不懂可单步执行，看url的变化
            Loger.Log4Net.Info($"UserInfoCallback :GetAccessToken开始，Code：{code}");
            var accessToken = OAuthApi.GetAccessToken(appId, secret, code);
            Loger.Log4Net.Info($"UserInfoCallback :GetAccessToken结束，Code：{code}");
            if (accessToken.errcode != ReturnCode.请求成功)
            {
                //如果令牌的错误信息不等于请求成功，则需要重新返回授权界面
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Redirect(redirect_index);
            }
            var isAuth = OAuthApi.Auth(accessToken.access_token, accessToken.openid);

            {
                #region 获取用户code
                try
                {
                    OAuthUserInfo userInfo = OAuthApi.GetUserInfo(accessToken.access_token, accessToken.openid);
                    Loger.Log4Net.Info($"调用OAuthApi.GetUserInfo方法，参数access_token={accessToken.access_token},openid={accessToken.openid},结果={(userInfo != null ? JsonConvert.SerializeObject(userInfo) : "对象为空")}");
                    var userInfobase = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, userInfo.openid, Language.zh_CN);

                    Loger.Log4Net.Info($"调用Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info方法，参数appId={appId},openid={userInfo.openid},userInfobase={userInfo.unionid},结果={(userInfobase != null ? JsonConvert.SerializeObject(userInfobase) : "对象为空")}");
                    ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();

                    var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId(userInfo.openid);
                    Loger.Log4Net.Info($"调用 XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId方法，参数openid={userInfo.openid},结果={(user != null ? JsonConvert.SerializeObject(user) : "对象为空")}");

                    if (!ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.IsExistOpneId(userInfo.openid))
                    {

                        Loger.Log4Net.Info($"OAuth2Controller>UserInfoCallback :IsExistOpneId IS false openid:{userInfo.openid}");
                        wxuser.subscribe = userInfobase.subscribe;
                        wxuser.openid = userInfo.openid;
                        wxuser.nickname = userInfo.nickname;
                        wxuser.sex = userInfo.sex;
                        wxuser.city = userInfo.city;
                        wxuser.country = userInfo.country;
                        wxuser.province = userInfo.province;
                        wxuser.language = userInfobase.language;
                        wxuser.headimgurl = userInfo.headimgurl;
                        wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfobase.subscribe_time);
                        wxuser.unionid = userInfo.unionid;
                        wxuser.remark = userInfobase.remark;
                        wxuser.groupid = userInfobase.groupid;
                        wxuser.tagid_list = string.Join(",", userInfobase.tagid_list);
                        wxuser.CreateTime = DateTime.Now;
                        wxuser.LastUpdateTime = wxuser.CreateTime;
                        wxuser.AuthorizeTime = DateTime.Now;
                        wxuser.Status = userInfobase.subscribe == 1 ? 0 : -1;
                        wxuser.RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号;
                        wxuser.RegisterType = (int)RegisterTypeEnum.微信;
                        wxuser.RegisterIp = System.Web.HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
                        wxuser.UserType = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeixinInfo.Instance.GetRecIDByAppId(appId);
                        Loger.Log4Net.Info("OAuth2Controller>UserInfoCallback :WeiXinUserOperation BEGIN");
                        //获取用户的渠道ID
                        wxuser.PromotionChannelID = ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.GetPromotionChannelID(returnUrl);
                        Loger.Log4Net.Info($"OAuth2Controller>UserInfoCallback openid：{userInfo.openid},wxuserJSON:{JsonConvert.SerializeObject(wxuser)}");
                        bool resultUserId = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(wxuser);
                        if (resultUserId)
                            user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId(userInfo.openid);
                        else
                        {
                            Loger.Log4Net.Info($"OAuth2Controller>UserInfoCallback openid：{userInfo.openid} :WeiXinUserOperation 失败...");
                        }
                        if (user != null)
                        {
                            XYAuto.ITSC.Chitunion2017.BLL.WechatInvite.WechatInvite.Instance.FriendFollow(user.UserID);
                            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
                            Loger.Log4Net.Info($"OAuth2Controller>UserInfoCallback :UserID:{user.UserID},Passport1 end...");
                        }
                        else
                        {
                            Loger.Log4Net.Info($"OAuth2Controller>UserInfoCallback openid：{userInfo.openid} :GetUnionAndUserId NULL...");
                        }
                    }
                    else
                    {

                        ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(0, DateTime.Now, wxuser.openid);
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("ReWriteUserIDed")
                        {
                            Name = "ReWriteUserIDed",
                            Value = "2",
                            Domain = (string.IsNullOrEmpty(WebDomain) && !string.IsNullOrEmpty(HttpContext.Current.Request.Url.Host)) ? "." + HttpContext.Current.Request.Url.Host : WebDomain,
                            //Path = "",
                            Expires = DateTime.MaxValue
                        });
                        Loger.Log4Net.Info($"OAuth2Controller>UserInfoCallback :UserID:{user.UserID},Passport2 end...");
                    }
                    HttpContext.Current.Session.Remove("wx_auth_code");
                    if (user != null && user.UserID > 0)

                    {
                        string cookiesVal = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
                        Loger.Log4Net.Info($"调用XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport方法，参数UserID={user.UserID},结果={(user != null ? cookiesVal : "为空")},URL={returnUrl}");
                        //HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
                        ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(user.UserID, returnUrl, HttpContext.Current.Request.UserAgent);
                        //场景判断
                        if (XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserSence(user.UserID))
                        {
                            Loger.Log4Net.Info($"调用XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserSence方法,参数UserID={user.UserID},没有场景，返回returnUrl={returnUrl}");
                            HttpContext.Current.Response.Buffer = true;
                        }
                        //returnUrl = returnUrl + (returnUrl.IndexOf('?') >= 0 ? "&" : "?") + "isauth=1";
                        HttpContext.Current.Response.Redirect(returnUrl);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("Invalid CT_User");
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("OAuth2 Exception:", ex);
                }
                #endregion
            }
        }


        /// <summary>
        /// 获取pc端登录二维码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        //[CrossSite]
        public Common.JsonResult GetLoginQr([FromBody]ReqPostLoginQrDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null || request.LoginType == LoginType.None)
            {
                jsonResult.Status = -1;
                jsonResult.Message = $"请选择登录类型";
                return jsonResult;
            }
            jsonResult.Result = new QrLoginProvider().GetLoginQr(request);

            return jsonResult;
        }

        /// <summary>
        /// 记录访问日志接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CrossSite]
        public Common.JsonResult LogRequest()
        {
            if (HttpContext.Current != null)
            {
                var uriRefer = HttpContext.Current.Request.UrlReferrer ?? HttpContext.Current.Request.Url;
                var userAgent = HttpContext.Current.Request.UserAgent;
                string ip = XYAuto.ITSC.Chitunion2017.BLL.Util.GetIP("/api/oauth2/LogRequest");
                string msg = $"当前请求URL：{uriRefer},UserAgent:{userAgent},IP:{ip}";

                XYAuto.ITSC.Chitunion2017.BLL.Loger.RequestLog.Info(msg);
            }
            return Common.Util.GetJsonDataByResult("OK");
        }

        /// <summary>
        /// 记录微信JSSDK-Error日志接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CrossSite]
        public Common.JsonResult ErrorLog([FromBody]ReqWriteErrorLogDto reqWel)
        {
            if (reqWel != null)
            {
                var uriRefer = HttpContext.Current.Request.UrlReferrer ?? HttpContext.Current.Request.Url;
                var userAgent = HttpContext.Current.Request.UserAgent;
                string ip = XYAuto.ITSC.Chitunion2017.BLL.Util.GetIP("/api/oauth2/LogRequest");
                string msg = $"\r\n微信JsSDK,Error:{reqWel.ErrorContent}\r\n当前请求URL：{uriRefer},\r\nUserAgent:{userAgent},\r\nIP:{ip}";
                XYAuto.ITSC.Chitunion2017.BLL.Loger.RequestLog.Info(msg);
            }
            return Common.Util.GetJsonDataByResult("OK");
        }

        /// <summary>
        /// 模拟登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult SimulationLogin([FromBody]ReqSimulationLoginDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new QrLoginProvider().SimulationLogin(request);

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 校验pc端微信扫码是否登录成功
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult VerifyLogin([FromBody]ReqVerifyQrLoginDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new QrLoginProvider().VerifyQrLogin(request);

            return jsonResult.GetReturn(retValue);
        }

    }
    public static class ApiJsonResultExtend
    {
        public static JsonResult GetReturn(this JsonResult jsonResult, ReturnValue retValue)
        {
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Status = retValue.ErrorCode.ToInt();
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            jsonResult.Result = retValue.ReturnObject;
            return jsonResult;
        }
    }

    public class ReqWriteErrorLogDto
    {
        public string ErrorContent { set; get; }
    }
}