using log4net;
using Newtonsoft.Json;
using Senparc.Weixin.Open;
using Senparc.Weixin.Open.ComponentAPIs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Interaction;
using XYAuto.ITSC.Chitunion2017.WxOAuth.Model;

namespace XYAuto.ITSC.Chitunion2017.WxOAuth
{
    public class WeChat : IHttpHandler
    {
        public WeChat()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        private static string component_AppId = WebConfigurationManager.AppSettings["Component_Appid"];
        private static string component_Secret = WebConfigurationManager.AppSettings["Component_Secret"];
        private static string component_Token = WebConfigurationManager.AppSettings["Component_Token"];
        private static string ashxPath = WebConfigurationManager.AppSettings["ASHXPath"];
        private static string component_EncodingAESKey = WebConfigurationManager.AppSettings["Component_EncodingAESKey"];
        private static string[] need_Rights = WebConfigurationManager.AppSettings["NeedOAuthRights"].Split(',');
        private static string redirectHost = WebConfigurationManager.AppSettings["RedirectHost"];
        private static string scanCodeSuccessRedirectUrl = WebConfigurationManager.AppSettings["ScanCodeSuccessRedirectUrl"];

        ILog logger = LogManager.GetLogger(typeof(WeChat));

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string method = context.Request.QueryString["m"];
                if (method.Equals("oauth"))
                {
                    #region 扫码主流程
                    int type = 0;
                    string redirectUrl = string.Empty;
                    int.TryParse(context.Request.Form["Type"], out type);
                    if (type.Equals((int)SourceTypeEnum.扫码授权))
                    {
                        #region 扫码
                        string preAuthCode = GetPreAuthCode();
                        if (string.IsNullOrWhiteSpace(preAuthCode))
                        {
                            HttpContext.Current.Response.Write("请等待几分钟再访问,需要预授权码");
                        }
                        else
                        {
                            //根据groupID的值判断是注册流程的授权 还是素材管理的授权
                            int groupID = 0;
                            int.TryParse(context.Request.Form["GroupID"], out groupID);
                            string wxAuthorizePage = string.Empty;
                            if (groupID == 0)
                            {
                                wxAuthorizePage = string.Format("https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={0}&pre_auth_code={1}&redirect_uri={2}", component_AppId, preAuthCode, ashxPath + "?m=RegistCallBack&appid=/$APPID$");
                            }
                            else
                            {
                                logger.Info("wxAuthorizePage:" + wxAuthorizePage);
                                wxAuthorizePage = string.Format("https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={0}&pre_auth_code={1}&redirect_uri={2}", component_AppId, preAuthCode, ashxPath + "?m=AuthorizeCallBack" + groupID + "&appid=/$APPID$");
                            }
                            HttpContext.Current.Response.Redirect(wxAuthorizePage);
                        }
                        #endregion
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("Type Error!");
                    }
                    #endregion
                }
                else if (method.Equals("notice"))
                {
                    Notice();
                }
                else if (method.Equals("RegistCallBack"))
                {
                    RegistCallBack();
                }
                else if (method.Contains("AuthorizeCallBack"))
                {
                    int groupID = int.Parse(method.Replace("AuthorizeCallBack", ""));
                    AuthorizeCallback(groupID);
                }
                else if (method.Equals("message"))
                {
                    MessageCallback();
                }
                else
                {
                    HttpContext.Current.Response.Write("请求错误!");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                HttpContext.Current.Response.Write("ProcessRequest错误!");
            }
        }

        /// <summary>
        /// 授权消息推送
        /// </summary>
        private void Notice()
        {
            try
            {
                #region 解析、解密
                string postData = GetPostData();
                string signature = HttpContext.Current.Request.QueryString["msg_signature"];
                string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = HttpContext.Current.Request.QueryString["nonce"];
                string sMsg = "";  //解析之后的明文
                int ret = 0;
                //对用户回复的数据进行解密。
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(component_Token, component_EncodingAESKey, component_AppId);
                ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postData, ref sMsg);
                if (ret != 0)
                {
                    logger.Error(string.Format("解密失败!ret:{0}", ret));
                    HttpContext.Current.Response.Write("fail");
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                    return;
                }
                #endregion 
                var messageHandler = Senparc.Weixin.Open.RequestMessageFactory.GetRequestEntity(sMsg);
                //接收component_verify_ticket
                if (messageHandler.InfoType == RequestInfoType.component_verify_ticket)
                {
                    logger.Info($"RequestInfoType.component_verify_ticket start");
                    RequestMessageComponentVerifyTicket componentVerifyTicket = messageHandler as RequestMessageComponentVerifyTicket;
                    logger.Debug("AppId=" + componentVerifyTicket.AppId + "  componentVerify=" + GetCache("verify_ticket"));
                    string componentVerify = componentVerifyTicket.ComponentVerifyTicket;
                    if (!string.IsNullOrEmpty(componentVerify))
                    {
                        //缓存ComponentVerifyTicket ，保存到数据库 
                        SetCache("verify_ticket", componentVerify);
                        BLL.WeixinOAuth.Instance.UpdateVerifyTicket(component_AppId, componentVerify, DateTime.Now);
                    }
                }
                else if (messageHandler.InfoType == RequestInfoType.authorized)
                {
                    #region 授权

                    logger.Info($"RequestInfoType.authorized start");
                    RequestMessageAuthorized authorized = messageHandler as RequestMessageAuthorized;
                    logger.Debug("授权 AppID:" + authorized.AuthorizerAppid);
                    //获取权限详情、授权公众号信息
                    string component_AccessToken = GetComponentAcessToken();
                    var authorizerInfo = ComponentApi.GetAuthorizerInfo(component_AccessToken, component_AppId, authorized.AuthorizerAppid);
                    int wxID = SaveAuthorizerInfo(authorized.AuthorizerAppid, authorizerInfo);
                    #endregion
                }
                else if (messageHandler.InfoType == RequestInfoType.updateauthorized)
                {//权限有变化才会进
                    #region 更新授权
                    logger.Info($"RequestInfoType.updateauthorized start");
                    RequestMessageUpdateAuthorized updateauthorized = messageHandler as RequestMessageUpdateAuthorized;
                    logger.Debug("更新授权 AppID:" + updateauthorized.AuthorizerAppid);
                    string component_AccessToken = GetComponentAcessToken();
                    var authorizerInfo = ComponentApi.GetAuthorizerInfo(component_AccessToken, component_AppId, updateauthorized.AuthorizerAppid);
                    int wxID = SaveAuthorizerInfo(updateauthorized.AuthorizerAppid, authorizerInfo);
                    #endregion
                }
                else if (messageHandler.InfoType == RequestInfoType.unauthorized)
                {
                    #region 取消授权
                    logger.Info($"RequestInfoType.unauthorized start");
                    RequestMessageUnauthorized unauthorized = messageHandler as RequestMessageUnauthorized;
                    logger.Debug("取消授权 AppID:" + unauthorized.AuthorizerAppid);
                    var wxInfo = BLL.LETask.LeWeixinOAuth.Instance.GetWeixinInfoByAppId(unauthorized.AuthorizerAppid);
                    if (wxInfo != null)
                    {
                        wxInfo.OAuthStatus = (int)OAuthStatusEnum.未授权;
                        wxInfo.SourceType = (int)SourceTypeEnum.验证码;
                        wxInfo.UserId = 0;//取消授权之后，userId 修改为0
                        BLL.LETask.LeWeixinOAuth.Instance.UpdateWeixinInfo(wxInfo);
                        BLL.WeixinOAuth.Instance.AddOAuthHistory(new Entities.WeixinOAuth.OAuthHistory() { AppID = unauthorized.AuthorizerAppid, WxID = wxInfo.RecID, Status = (int)OAuthStatusEnum.未授权, CreateTime = DateTime.Now });
                    }
                    #endregion
                }
                HttpContext.Current.Response.Write("success");
            }
            catch (Exception ex)
            {
                logger.Debug("出现异常{0}", ex);
            }
            return;
        }

        /// <summary>
        /// 注册流程授权回调
        /// </summary>
        private void RegistCallBack()
        {
            try
            {
                string accessToken = GetComponentAcessToken();
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    logger.Info("Need ComponentAcessToken !");
                }
                else
                {
                    // 使用授权码换取授权公众号的授权信息，并换取authorizer_access_token和authorizer_refresh_token
                    string authCode = HttpContext.Current.Request.QueryString["auth_code"];
                    if (!string.IsNullOrEmpty(authCode))
                    {
                        #region 第一次授权回调
                        var queryAuthRes = ComponentApi.QueryAuth(accessToken, component_AppId, authCode);
                        string wxAppID = queryAuthRes.authorization_info.authorizer_appid;

                        var authorizerInfo = ComponentApi.GetAuthorizerInfo(accessToken, component_AppId, wxAppID);
                        SaveAuthorizerInfo(wxAppID, authorizerInfo);

                        //跳转到扫码成功之后的页面
                        HttpContext.Current.Response.Redirect($"{scanCodeSuccessRedirectUrl}", false);
                        //HttpContext.Current.Response.End();

                        #region 之前的逻辑，看不懂（觉得有问题）

                        //var wxInfo = BLL.WeixinOAuth.Instance.GetWeixinInfoByAppID(wxAppID);
                        //if (wxInfo != null)
                        //{
                        //    #region 之前保存成功了
                        //    string wxAccessToken = queryAuthRes.authorization_info.authorizer_access_token;
                        //    string wxRefreshAccessToken = queryAuthRes.authorization_info.authorizer_refresh_token;

                        //    //判断公众号是否具有API权限
                        //    if (string.IsNullOrWhiteSpace(wxAccessToken))
                        //    {
                        //        logger.Debug("公众号AppID:" + wxAppID + "未具备API权限");
                        //    }
                        //    else
                        //    {
                        //        wxInfo.AccessToken = wxAccessToken;
                        //        wxInfo.RefreshAccessToken = wxRefreshAccessToken;
                        //        wxInfo.GetTokenTime = DateTime.Now;
                        //        logger.Info("WxID:" + wxInfo.RecID + " AccessToken:" + wxAccessToken);

                        //        #region 素材管理-更新Biz 用户管理-更新粉丝数
                        //        if (CheckHasRight(queryAuthRes.authorization_info, FuncscopeCategory.素材管理权限))
                        //        {
                        //            string biz = BLL.WeixinOAuth.Instance.InvokeAPIGetBiz(wxAccessToken);
                        //            if (string.IsNullOrWhiteSpace(biz))
                        //            {
                        //                wxInfo.Biz = biz;
                        //            }
                        //        }
                        //        if (CheckHasRight(queryAuthRes.authorization_info, FuncscopeCategory.用户管理权限))
                        //        {
                        //            int fanscount = BLL.WeixinOAuth.Instance.InvokeAPIGetFansCount(wxAccessToken);
                        //            if (fanscount > wxInfo.FansCount)
                        //            {
                        //                wxInfo.FansCount = fanscount;
                        //            }
                        //        }
                        //        #endregion

                        //        #region 补充信息
                        //        var wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(wxInfo.WxNumber, wxInfo.Biz);
                        //        if (wxCatch != null)
                        //        {
                        //            if (string.IsNullOrWhiteSpace(wxInfo.Biz))
                        //            {
                        //                wxInfo.Biz = wxCatch.Biz;
                        //            }
                        //            DateTime dt = Entities.Constants.Constant.DATE_INVALID_VALUE;
                        //            //全称
                        //            wxInfo.FullName = wxCatch.Name;
                        //            //注册时间
                        //            if (!string.IsNullOrWhiteSpace(wxCatch.RegTime) && DateTime.TryParse(wxCatch.RegTime, out dt))
                        //            {
                        //                wxInfo.RegTime = dt;
                        //            }
                        //            //简介
                        //            wxInfo.Summary = wxCatch.VerifyInfo.Length > 250 ? (wxCatch.VerifyInfo.Substring(0, 248) + "...") : wxCatch.VerifyInfo;
                        //            //统一社会信用代码
                        //            wxInfo.CreditCode = wxCatch.RegisteredId;
                        //            //前置许可经营范围
                        //            wxInfo.BusinessScope = wxCatch.FrontBusinessType;
                        //            //企业类型
                        //            wxInfo.EnterpriseType = wxCatch.EnterpriseType;
                        //            //企业成立日期
                        //            wxInfo.EnterpriseCreateDate = wxCatch.EnterpriseEstablishmentDate;
                        //            //企业营业期限
                        //            wxInfo.EnterpriseBusinessTerm = wxCatch.EnterpriseExpiredDate;
                        //            //企业认证日期
                        //            wxInfo.EnterpriseVerifyDate = wxCatch.VerifyDate;
                        //        }
                        //        #endregion

                        //        if (wxInfo.Status.Equals(-1))
                        //            wxInfo.Status = 1;
                        //        //todo:现在有点不一样，要替换原来的用户

                        //        wxInfo.UserId = GetUserInfo.UserID;
                        //        int rowcount = BLL.WeixinOAuth.Instance.UpdateWeixinInfo(wxInfo);
                        //        logger.Info(wxInfo.RecID + "更新基础信息" + (rowcount > 0 ? "成功" : "失败"));
                        //        Task.Factory.StartNew(() => FillAritcle(queryAuthRes, wxInfo));
                        //        //SaveMediaInfo(wxInfo);
                        //    }
                        //    HttpContext.Current.Response.Redirect(string.Format("{0}{1}", redirectHost, "/MediaManager/mediaWeChatList_new.html"), false);
                        //    #endregion
                        //}

                        #endregion


                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                HttpContext.Current.Response.Write("RegistCallBack错误!");
            }
        }

        /// <summary>
        /// 素材授权流程回调
        /// </summary>
        /// <param name="groupID"></param>
        private void AuthorizeCallback(int groupID)
        {
            try
            {
                string accessToken = GetComponentAcessToken();
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    logger.Info("Need ComponentAcessToken !");
                }
                else
                {
                    // 使用授权码换取授权公众号的授权信息，并换取authorizer_access_token和authorizer_refresh_token
                    string authCode = HttpContext.Current.Request.QueryString["auth_code"];
                    if (!string.IsNullOrEmpty(authCode))
                    {
                        #region 第一次授权回调
                        var queryAuthRes = ComponentApi.QueryAuth(accessToken, component_AppId, authCode);
                        string wxAppID = queryAuthRes.authorization_info.authorizer_appid;

                        var authorizerInfo = ComponentApi.GetAuthorizerInfo(accessToken, component_AppId, wxAppID);
                        SaveAuthorizerInfo(wxAppID, authorizerInfo);

                        var wxInfo = BLL.WeixinOAuth.Instance.GetWeixinInfoByAppID(wxAppID);
                        if (wxInfo != null)
                        {
                            #region 之前保存成功了
                            string wxAccessToken = queryAuthRes.authorization_info.authorizer_access_token;
                            string wxRefreshAccessToken = queryAuthRes.authorization_info.authorizer_refresh_token;

                            //判断公众号是否具有API权限
                            if (string.IsNullOrWhiteSpace(wxAccessToken))
                            {
                                logger.Debug("公众号AppID:" + wxAppID + "未具备API权限");
                            }
                            else
                            {
                                wxInfo.AccessToken = wxAccessToken;
                                wxInfo.RefreshAccessToken = wxRefreshAccessToken;
                                wxInfo.GetTokenTime = DateTime.Now;
                                logger.Info("WxID:" + wxInfo.RecID + " AccessToken:" + wxAccessToken);

                                #region 素材管理-更新Biz 用户管理-更新粉丝数
                                if (CheckHasRight(queryAuthRes.authorization_info, FuncscopeCategory.素材管理权限))
                                {
                                    string biz = BLL.WeixinOAuth.Instance.InvokeAPIGetBiz(wxAccessToken);
                                    if (string.IsNullOrWhiteSpace(biz))
                                    {
                                        wxInfo.Biz = biz;
                                    }
                                }
                                if (CheckHasRight(queryAuthRes.authorization_info, FuncscopeCategory.用户管理权限))
                                {
                                    int fanscount = BLL.WeixinOAuth.Instance.InvokeAPIGetFansCount(wxAccessToken);
                                    if (fanscount > wxInfo.FansCount)
                                    {
                                        wxInfo.FansCount = fanscount;
                                    }
                                }
                                #endregion

                                #region 补充信息
                                var wxCatch = BLL.WeixinOAuth.Instance.GetCatchData(wxInfo.WxNumber, wxInfo.Biz);
                                if (wxCatch != null)
                                {
                                    if (string.IsNullOrWhiteSpace(wxInfo.Biz))
                                    {
                                        wxInfo.Biz = wxCatch.Biz;
                                    }
                                    DateTime dt = Entities.Constants.Constant.DATE_INVALID_VALUE;
                                    //全称
                                    wxInfo.FullName = wxCatch.Name;
                                    //注册时间
                                    if (!string.IsNullOrWhiteSpace(wxCatch.RegTime) && DateTime.TryParse(wxCatch.RegTime, out dt))
                                    {
                                        wxInfo.RegTime = dt;
                                    }
                                    //简介
                                    wxInfo.Summary = wxCatch.VerifyInfo.Length > 250 ? (wxCatch.VerifyInfo.Substring(0, 248) + "...") : wxCatch.VerifyInfo;
                                    //统一社会信用代码
                                    wxInfo.CreditCode = wxCatch.RegisteredId;
                                    //前置许可经营范围
                                    wxInfo.BusinessScope = wxCatch.FrontBusinessType;
                                    //企业类型
                                    wxInfo.EnterpriseType = wxCatch.EnterpriseType;
                                    //企业成立日期
                                    wxInfo.EnterpriseCreateDate = wxCatch.EnterpriseEstablishmentDate;
                                    //企业营业期限
                                    wxInfo.EnterpriseBusinessTerm = wxCatch.EnterpriseExpiredDate;
                                    //企业认证日期
                                    wxInfo.EnterpriseVerifyDate = wxCatch.VerifyDate;
                                }
                                #endregion
                                if (wxInfo.Status.Equals(-1))
                                    wxInfo.Status = 1;
                                int rowcount = BLL.LETask.LeWeixinOAuth.Instance.UpdateWeixinInfo(wxInfo);
                                logger.Info(wxInfo.RecID + "更新基础信息" + (rowcount > 0 ? "成功" : "失败"));
                                Task.Factory.StartNew(() => FillAritcle(queryAuthRes, wxInfo));
                                SaveMediaInfo(wxInfo);
                            }
                            HttpContext.Current.Response.Redirect(string.Format("{0}{1}?GroupID={2}", redirectHost, "/SouceManager/graphicpublishing.html", groupID), false);
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                HttpContext.Current.Response.Write("AuthorizeCallback错误!");
            }

        }

        private void MessageCallback()
        {
            //处理微信普通消息，可以使用MessageHandler
            try
            {
                string postData = GetPostData();
                if (string.IsNullOrEmpty(postData))
                {
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
            }
            // WriteLog("回复: " + strContent);
            // HttpContext.Current.Response.Write(strContent);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 群发与通知-获取图文信息 多图文统计 互动参数
        /// </summary>
        /// <param name="queryAuthRes"></param>
        /// <param name="wxInfo"></param>
        private void FillAritcle(QueryAuthResult queryAuthRes, Entities.WeixinOAuth.WeixinInfo wxInfo)
        {
            try
            {
                var iw = BLL.Interaction.InteractionWeixin.Instance.GetEntityByWxID(wxInfo.RecID);
                if (iw == null)
                {
                    iw = new InteractionWeixin() { WxID = wxInfo.RecID, MeidaType = (int)MediaTypeEnum.微信, CreateTime = DateTime.Now, LastUpdateTime = DateTime.Now };
                }
                if (CheckHasRight(queryAuthRes.authorization_info, FuncscopeCategory.群发与通知权限))
                {//有权限的话 平均阅读数、最高阅读数、周更新次数从微信接口获取 
                    BLL.WeixinOAuth.Instance.SaveArticleInfo(wxInfo.RecID, wxInfo.AppID, wxInfo.AccessToken, iw);
                    BLL.WeixinOAuth.Instance.SaveInteraction(iw, wxInfo.WxNumber, true);
                }
                else
                {
                    BLL.WeixinOAuth.Instance.SaveInteraction(iw, wxInfo.WxNumber, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error("保存图文信息错误", ex);
            }
        }

        private string GetPostData()
        {
            int intLen = Convert.ToInt32(HttpContext.Current.Request.InputStream.Length);
            byte[] arr = new byte[intLen];
            HttpContext.Current.Request.InputStream.Read(arr, 0, intLen);
            return System.Text.Encoding.UTF8.GetString(arr);
        }

        /// <summary>
        /// 检查是否拥有某个权限
        /// </summary>
        /// <param name="info">授权者信息</param>
        /// <param name="func">功能目录</param>
        /// <returns></returns>
        private bool CheckHasRight(AuthorizationInfo info, FuncscopeCategory func)
        {
            if (info.func_info == null || info.func_info.Count.Equals(0))
            {
                return false;
            }
            if (info.func_info.Exists(i => i.funcscope_category.id.Equals(func)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取授权的状态 
        /// 39001-正常 
        /// 39002-异常
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private OAuthStatusEnum GetOAuthStatus(AuthorizationInfo info)
        {
            bool flag = false;
            logger.Debug("AuthorizationInfo:" + JsonConvert.SerializeObject(info));
            foreach (string right in need_Rights)
            {
                if (!info.func_info.Exists(f => ((int)f.funcscope_category.id).ToString().Equals(right)))
                {
                    flag = true;
                    break;
                }
            }
            return flag ? OAuthStatusEnum.授权异常 : OAuthStatusEnum.授权正常;
        }

        /// <summary>
        /// 用Component_Access_Token取Pre_Auth_Code
        /// </summary>
        /// <returns></returns>
        private string GetPreAuthCode()
        {
            try
            {
                string accessToken = GetComponentAcessToken();
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    logger.Debug("Need component_access_token,Please wait !");
                    return string.Empty;
                }
                var preAuthObj = ComponentApi.GetPreAuthCode(component_AppId, accessToken);
                logger.Debug("preAuthObj:" + JsonConvert.SerializeObject(preAuthObj));
                return preAuthObj.pre_auth_code;
            }
            catch (Exception ex)
            {
                logger.Error("获取预授权码异常", ex);
                return string.Empty;
            }
        }

        /// 用Verify_ticket取Component_Access_Token
        /// </summary>
        /// <returns></returns>
        private string GetComponentAcessToken()
        {
            try
            {
                DateTime accessTokenTime = new DateTime(1900, 1, 1);
                string accessToken = string.Empty;
                var component = BLL.WeixinOAuth.Instance.GetComponentDetail(component_AppId);
                if (component != null)
                {
                    accessTokenTime = component.AccessTokenTime;
                    accessToken = component.AccessToken;
                }
                if (string.IsNullOrWhiteSpace(accessToken) || accessTokenTime < DateTime.Now.AddHours(0).AddMinutes(-50))
                {
                    string verifyTicket = GetCache("verify_ticket");
                    if (string.IsNullOrWhiteSpace(verifyTicket))
                    {
                        logger.Debug("Need verify_ticket,Please wait !- IsNullOrWhiteSpace");
                        return string.Empty;
                    }
                    logger.Info("Old accessToken:" + accessToken + "   Old Token Time:" + accessTokenTime);
                    var accessTokenObj = ComponentApi.GetComponentAccessToken(component_AppId, component_Secret, verifyTicket);
                    accessToken = accessTokenObj.component_access_token;
                    logger.Debug("accessTokenObj:" + JsonConvert.SerializeObject(accessTokenObj));
                    BLL.WeixinOAuth.Instance.UpdateAccessToken(component_AppId, accessToken, DateTime.Now);
                }
                return accessToken;
            }
            catch (Exception ex)
            {
                logger.Error("获取component_access_token异常", ex);
                return string.Empty;
            }
        }

        public Common.LoginUser GetUserInfo
        {
            get { return Common.UserInfo.GetLoginUser() ?? new Common.LoginUser(); }
        }

        /// <summary>
        /// ①更新扫码方式的数据
        /// ②更新权限信息
        /// </summary>
        /// <param name="appID">appid</param>
        /// <param name="authorizerInfo">授权者信息</param>
        /// <returns></returns>
        private int SaveAuthorizerInfo(string appID, GetAuthorizerInfoResult authorizerInfo)
        {
            bool isAdd = true;
            try
            {
                var wxInfo = BLL.LETask.LeWeixinOAuth.Instance.GetWeixinInfoByAppId(appID);
                var oauthStatus = GetOAuthStatus(authorizerInfo.authorization_info);
                if (wxInfo == null)
                    wxInfo = new Entities.WeixinOAuth.WeixinInfo() { Status = 0, CreateTime = DateTime.Now };
                else
                {
                    isAdd = false;
                }
                wxInfo.AppID = appID;
                wxInfo.AccessToken = authorizerInfo.authorization_info.authorizer_access_token;
                wxInfo.RefreshAccessToken = authorizerInfo.authorization_info.authorizer_refresh_token;
                wxInfo.GetTokenTime = DateTime.Now;
                wxInfo.WxNumber = authorizerInfo.authorizer_info.alias;
                wxInfo.OriginalID = authorizerInfo.authorizer_info.user_name;
                wxInfo.NickName = authorizerInfo.authorizer_info.nick_name;
                wxInfo.ServiceType = (int)authorizerInfo.authorizer_info.service_type_info.id + Entities.Constants.Constant.ServiceTypeAdd;
                wxInfo.IsVerify = authorizerInfo.authorizer_info.verify_type_info.id.Equals(VerifyType.微信认证);
                wxInfo.VerifyType = (int)authorizerInfo.authorizer_info.verify_type_info.id + Entities.Constants.Constant.VerifyTypeAdd;
                wxInfo.HeadImg = authorizerInfo.authorizer_info.head_img;
                if (string.IsNullOrWhiteSpace(wxInfo.HeadImg))
                    wxInfo.HeadImg = "/images/default_touxiang.png";
                wxInfo.QrCodeUrl = authorizerInfo.authorizer_info.qrcode_url;
                wxInfo.OAuthStatus = (int)oauthStatus;
                wxInfo.SourceType = (int)SourceTypeEnum.扫码授权;
                wxInfo.ModifyTime = DateTime.Now;

                wxInfo.UserId = GetUserInfo.UserID;

                if (wxInfo.RecID > 0)
                    BLL.LETask.LeWeixinOAuth.Instance.UpdateWeixinInfo(wxInfo);
                else
                    wxInfo.RecID = BLL.LETask.LeWeixinOAuth.Instance.AddWeixinInfo(wxInfo);
                //权限相关
                int hisID = BLL.WeixinOAuth.Instance.AddOAuthHistory(new Entities.WeixinOAuth.OAuthHistory() { AppID = appID, WxID = wxInfo.RecID, Status = (int)oauthStatus, CreateTime = DateTime.Now });
                List<Entities.WeixinOAuth.OAuthDetail> rights = new List<Entities.WeixinOAuth.OAuthDetail>();
                foreach (var f in authorizerInfo.authorization_info.func_info)
                {
                    rights.Add(new Entities.WeixinOAuth.OAuthDetail()
                    {
                        HisID = hisID,
                        OAuthID = (int)f.funcscope_category.id + Entities.Constants.Constant.OauthIDAdd
                    });
                }
                BLL.WeixinOAuth.Instance.AddOAuthDetail(rights);
                return wxInfo.RecID;
            }
            catch (Exception ex)
            {

                logger.Error((isAdd ? "新增" : "更新") + "授权者信息错误(AppID:" + appID + ")", ex);
                return -1;
            }
        }

        #region V1.1.6
        /// <summary>
        /// 保存附表媒体信息
        /// </summary>
        public void SaveMediaInfo(Entities.WeixinOAuth.WeixinInfo wxInfo)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (ur.UserID > 0)
            {
                var one = BLL.Media.MediaWeixin.Instance.GetNormalEntityByWxID(wxInfo.RecID, ur.UserID, ur.IsAE);
                if (one == null)
                {
                    #region add
                    one = new Entities.Media.MediaWeixin()
                    {
                        Number = wxInfo.WxNumber,
                        Name = wxInfo.NickName,
                        HeadIconURL = wxInfo.HeadImg,
                        TwoCodeURL = wxInfo.QrCodeUrl,
                        FansCount = wxInfo.FansCount,
                        AuditStatus = (int)Entities.Enum.MediaAuditStatusEnum.FakePassed,
                        AuthType = (int)Entities.Enum.SourceTypeEnum.扫码授权,
                        CreateUserID = ur.UserID,
                        CreateTime = DateTime.Now,
                        LastUpdateUserID = ur.UserID,
                        LastUpdateTime = DateTime.Now,
                        Status = 0,
                        WxID = wxInfo.RecID
                    };
                    BLL.Media.MediaWeixin.Instance.Insert(one);
                    #endregion
                }
                else
                {
                    #region edit
                    one.Number = wxInfo.WxNumber;
                    one.Name = wxInfo.NickName;
                    one.HeadIconURL = wxInfo.HeadImg;
                    one.TwoCodeURL = wxInfo.QrCodeUrl;
                    if (wxInfo.FansCount > one.FansCount)
                        one.FansCount = wxInfo.FansCount;
                    one.AuthType = (int)Entities.Enum.SourceTypeEnum.扫码授权;
                    one.LastUpdateUserID = ur.UserID;
                    one.LastUpdateTime = DateTime.Now;
                    BLL.Media.MediaWeixin.Instance.Update(one);
                    #endregion
                }
            }
        }
        #endregion

        #region 缓存
        public static string GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey] == null ? string.Empty : objCache[CacheKey].ToString();
        }
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 处理消息 暂时不用
        //string authorizerAppID = HttpContext.Current.Request.QueryString["appid"];
        //authorizerAppID = authorizerAppID.Remove(0, 1);
        //string signature = HttpContext.Current.Request.QueryString["msg_signature"];
        //string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
        //string nonce = HttpContext.Current.Request.QueryString["nonce"];
        //string sMsg = string.Empty;  //解析之后的明文
        //int ret = 0;
        //string auth_code = string.Empty;
        //string url_auth_code = HttpContext.Current.Request.QueryString["auth_code"];
        //logger.Debug("Callback url_auth_code: " + url_auth_code);
        //string componentVerifyTicket = GetCache("verify_ticket");
        ////对用户回复的数据进行解密。
        //WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(component_Token, component_EncodingAESKey, component_AppId);
        //ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postData, ref sMsg);
        //if (ret != 0)
        //{
        //    logger.Debug(string.Format("解密失败!ret:{0}", ret));
        //    HttpContext.Current.Response.Write("fail");
        //    return;
        //}
        ////WriteLog("Callback sMsg: " + sMsg);
        //XDocument requestDocument = XDocument.Parse(sMsg);//转成XDocument
        //RequestMsgType msgType = MsgTypeHelper.GetRequestMsgType(requestDocument);
        //var result = Senparc.Weixin.MP.RequestMessageFactory.GetRequestEntity(sMsg);

        //if (test_WeiXin == authorizerAppID.Trim())
        //{
        //    #region 测试微信号
        //    logger.Debug("enter test_WeiXin logic");
        //    //WriteLog("全网发布消息: " + resultFromWx);
        //    RequestMessageText _requestMessageText = result as RequestMessageText;
        //    if (msgType == RequestMsgType.Text)
        //    {
        //        logger.Debug("is RequestMsgType.Text");
        //        #region Text
        //        if (_requestMessageText.Content.Contains("QUERY_AUTH_CODE"))
        //        {
        //            //模拟粉丝发送文本消息给专用测试公众号，第三方平台方需在5秒内返回空串表明暂时不回复，然后再立即使用客服消息接口发送消息回复粉丝

        //            var componentAccessToken = ComponentApi.GetComponentAccessToken(component_AppId, component_Secret, componentVerifyTicket).component_access_token;
        //            QueryAuthResult oAuthJoinResult = ComponentApi.QueryAuth(componentAccessToken, component_AppId, _requestMessageText.Content.Split(':')[1]);
        //            //调用发送客服消息api回复文本消息给粉丝
        //            WxJsonResult sendResult = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(oAuthJoinResult.authorization_info.authorizer_access_token, result.FromUserName, _requestMessageText.Content.Split(':')[1] + "_from_api");
        //            //WriteLog("sendResult.errmsg: " + sendResult.errmsg);
        //            //缓存auth_code用于以下文本消息与事件发送用
        //            SetCache("auth_code", _requestMessageText.Content.Split(':')[1]);
        //            logger.Debug(string.Format("auth_code:{0}", _requestMessageText.Content.Split(':')[1]));
        //        }
        //        else
        //        {
        //            //返回文本消息

        //            //string auth_code = Request.QueryString["auth_code");
        //            auth_code = GetCache("auth_code");

        //            var componentAccessToken = ComponentApi.GetComponentAccessToken(component_AppId, component_Secret, componentVerifyTicket).component_access_token;
        //            QueryAuthResult oAuthJoinResult = ComponentApi.QueryAuth(componentAccessToken, component_AppId, auth_code);
        //            //调用发送客服消息api回复文本消息给粉丝  
        //            WxJsonResult sendResult = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(oAuthJoinResult.authorization_info.authorizer_access_token, result.FromUserName, "TESTCOMPONENT_MSG_TYPE_TEXT_callback");
        //            //WriteLog("文本消息 sendResult.errmsg: " + sendResult.errmsg);

        //        }
        //        #endregion
        //    }
        //    else if (msgType == RequestMsgType.Event)
        //    {
        //        logger.Debug("is RequestMsgType.Event");
        //        #region Event
        //        //事件 //Content = "LOCATIONfrom_callback",
        //        auth_code = GetCache("auth_code");
        //        var componentAccessToken = GetComponentAcessToken();
        //        QueryAuthResult oAuthJoinResult = ComponentApi.QueryAuth(componentAccessToken, component_AppId, auth_code);
        //        //调用发送客服消息api回复文本消息给粉丝
        //        WxJsonResult sendResult = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(oAuthJoinResult.authorization_info.authorizer_access_token, result.FromUserName, requestDocument.Root.Element("Event").Value + "from_callback");
        //        //WriteLog("事件 sendResult.errmsg: " + sendResult.errmsg);
        //        #endregion
        //    }
        //    #endregion
        //}
        //else
        //{
        //    //WriteLog("普通用户请求: " + resultFromWx);
        //    //处理普通用户请求
        //}
        #endregion

    }
}
