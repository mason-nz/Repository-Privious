/*----------------------------------------------------------------
    Copyright (C) 2018 Senparc
    
    文件名：CustomMessageHandler_Events.cs
    文件功能描述：自定义MessageHandler
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.BUOC.Chitunion2017.WeChat.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        //private string WeChatMenuClickDataPath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath", true);
        private string WeixinAppId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeixinAppId", true);

        //public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        //{
        //    // 预处理文字或事件类型请求。
        //    // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
        //    // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
        //    // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
        //    // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
        //    // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

        //    if (requestMessage.Content == "OneClick")
        //    {
        //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
        //        strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
        //        return strongResponseMessage;
        //    }
        //    return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        //}

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配

            var list = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetWxMenuConfigData(WeChatMenuClickDataPath);
            if (list != null && list.Count > 0)
            {
                var result = list.FirstOrDefault(s => s.AppId == WeixinAppId);
                if (result != null && result.MenuClickList != null)
                {
                    bool finded = false;
                    foreach (var clickData in result.MenuClickList)
                    {
                        if (clickData.EventKey == requestMessage.EventKey)
                        {
                            var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                            reponseMessage = strongResponseMessage;
                            strongResponseMessage.Image.MediaId = clickData.MediaId;
                            finded = true;
                        }
                    }
                    if (!finded)
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                }
            }
            //switch (requestMessage.EventKey)
            //{
            //    case "10":
            //    case "Cooperation"://商务合作   Cooperation
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CooperationMediaId");
            //        }
            //        break;
            //    case "11":
            //    case "Communication"://加群交流   
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CommunicationMediaId");
            //        }
            //        break;
            //    case "BackupWeixinNum"://备用微信号   
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("BackupWeixMediaId");
            //        }
            //        break;
            //    case "Winner"://赢大奖   
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Winner");
            //        }
            //        break;
            //    default:
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //            strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
            //            reponseMessage = strongResponseMessage;
            //        }
            //        break;
            //}

            return reponseMessage;
        }

        /// <summary>
        /// 进入事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        //{
        //    var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
        //    responseMessage.Content = "您刚才发送了ENTER事件请求。";
        //    return responseMessage;
        //}

        /// <summary>
        /// 位置事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        //{
        //    //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "这里写什么都无所谓，比如：上帝爱你！";
        //    return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
        //}

        /// <summary>
        /// 通过二维码扫描关注扫描事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            Loger.Log4Net.Info($"OnEvent_ScanRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            //通过扫描关注
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            try
            {
                if (!string.IsNullOrEmpty(requestMessage.EventKey))
                {
                    var sceneId = requestMessage.EventKey.Replace("qrscene_", "");
                    var provider = new QrLoginProvider();
                    var reqKeyValue = provider.VerifyScenceStr(sceneId);
                    if (reqKeyValue != null)
                    {
                        Loger.Log4Net.Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录:EventKey={requestMessage.EventKey} VerifyScenceStr 成功");
                        if (reqKeyValue.t == ReqMessageType.Pc端登录)
                        {
                            Loger.Log4Net.Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录 进行 SimulationLogin 模拟登录-开始");
                            //pc端扫码登录业务
                            var retValue = provider.SimulationLogin(new ReqSimulationLoginDto
                            {
                                WeixinOpendId = requestMessage.FromUserName,
                                Ticket = reqKeyValue.v,
                                EventKey = requestMessage.EventKey
                            });
                            if (retValue.HasError)
                            {
                                Loger.Log4Net.Info(
                                    $"OnEvent_ScanRequest 扫码事件推送 Pc端登录 SimulationLogin失败:{retValue.Message}");
                            }
                            else
                            {
                                Loger.Log4Net.Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录 进行 SimulationLogin 模拟登录-成功-结束");
                            }
                            return new SuccessResponseMessage();
                        }
                        Loger.Log4Net.Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录 VerifyScenceStr-结束");
                    }
                }

                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    ScanQR(requestMessage);
                });
                var temp = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(requestMessage.FromUserName);
                if (temp == null)
                {
                    //responseMessage.Content = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");
                    responseMessage.Content = GetWelcomeMsg();
                    return responseMessage;
                }
                else
                {
                    if (temp.Status == -1)
                    {

                        //responseMessage.Content = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");
                        responseMessage.Content = GetWelcomeMsg();
                        return responseMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("OnEvent_ScanRequest" + ex.ToString());
                return new SuccessResponseMessage();
            }
            return new SuccessResponseMessage();
            //return responseMessage;
        }

        private void ScanQR(RequestMessageEvent_Scan requestMessage)
        {
            ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();

            #region 扫码事件
            var openId = requestMessage.FromUserName;
            Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
            if (userInfo == null)
            {
                while (true)
                {
                    userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
                    if (userInfo != null)
                    {
                        break;
                    }
                }
            }
            if (ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.IsExistOpneId(userInfo.openid))
            {
                Loger.Log4Net.Info("UpdateStatusByOpneId：" + userInfo.openid);
                ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(0, DateTime.Now, userInfo.openid);
                var user = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(userInfo.openid);
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
            }
            else
            {
                #region 插入微信用户
                try
                {
                    Loger.Log4Net.Info($"OnEvent_ScanRequest 插入微信用户openid:{userInfo.openid}");
                    wxuser.subscribe = userInfo.subscribe;
                    wxuser.openid = userInfo.openid;
                    wxuser.nickname = userInfo.nickname;
                    wxuser.sex = userInfo.sex;
                    wxuser.city = userInfo.city;
                    wxuser.country = userInfo.country;
                    wxuser.province = userInfo.province;
                    wxuser.language = userInfo.language;
                    wxuser.headimgurl = userInfo.headimgurl;
                    wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfo.subscribe_time);
                    wxuser.unionid = userInfo.unionid;
                    wxuser.remark = userInfo.remark;
                    wxuser.groupid = userInfo.groupid;
                    wxuser.tagid_list = string.Join(",", userInfo.tagid_list);
                    wxuser.UserID = 0;
                    wxuser.CreateTime = DateTime.Now;
                    wxuser.LastUpdateTime = wxuser.CreateTime;
                    //wxuser.QRcode = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode(appId, userInfo.openid);
                    //wxuser.InvitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(wxuser.QRcode, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
                    wxuser.Status = userInfo.subscribe == 1 ? 0 : -1;
                    wxuser.RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号;
                    wxuser.RegisterType = (int)RegisterTypeEnum.微信;
                    wxuser.UserType = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeixinInfo.Instance.GetRecIDByAppId(appId);

                    if (!string.IsNullOrEmpty(requestMessage.EventKey))
                    {
                        try
                        {
                            var qrJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ReqKeyValueDto>(requestMessage.EventKey.Replace("qrscene_", ""));
                            if (qrJson != null)
                            {
                                wxuser.Source = (int)qrJson.t;
                                wxuser.Inviter = qrJson.v;
                                if (wxuser.Source == 102)
                                {
                                    wxuser.Source = (int)ReqMessageType.场景;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            wxuser.Inviter = requestMessage.EventKey.Replace("qrscene_", "");
                        }
                    }
                    bool insertUser = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(wxuser);
                    if (insertUser)
                    {
                        var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId(userInfo.openid);
                        XYAuto.ITSC.Chitunion2017.BLL.WechatInvite.WechatInvite.Instance.FriendFollow(user.UserID);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Info("ScanQR：" + ex.ToString());
                    //return responseMessage;
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 打开网页事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        //{
        //    //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
        //    return responseMessage;
        //}

        /// <summary>
        /// 群发完成事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "接收到了群发完成的信息。";
        //    return responseMessage;
        //}


        private string GetWelcomeMsg()
        {
            var model = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetMenuDataByAppId(WeChatMenuClickDataPath, appId);
            return model == null ? string.Empty : model.SubscribeMsg;
        }

        /// <summary>
        /// 用户关注后，要发送的消息（1元提现活动）
        /// </summary>
        private void SendSubscribedMsgInfo(RequestMessageEvent_Subscribe requestMessage)
        {
            try
            {
                string domain = ConfigurationManager.AppSettings["Domin"];
                Loger.Log4Net.Info($"SendSubscribedMsgInfo-FromUserName:{requestMessage.FromUserName}");
                Loger.Log4Net.Info($"SendSubscribedMsgInfo-IsNewUserByOpenId:{XYAuto.ITSC.Chitunion2017.BLL.WeChat.ActivityVerifyBll.Instance.IsNewUserByOpenId(requestMessage.FromUserName)}");
                if (requestMessage != null && XYAuto.ITSC.Chitunion2017.BLL.WeChat.ActivityVerifyBll.Instance.IsNewUserByOpenId(requestMessage.FromUserName))
                {
                    var model = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetMenuDataByAppId(WeChatMenuClickDataPath, WeixinAppId);

                    Loger.Log4Net.Info($"SendSubscribedMsgInfo-GetMenuDataByAppId:{JsonConvert.SerializeObject(model)}");
                    if (model != null && model.SubArticleInfo != null)
                    {
                        string url = model.SubArticleInfo.Url.StartsWith("/") ? domain + model.SubArticleInfo.Url : model.SubArticleInfo.Url;
                        List<Article> listAr = new List<Article>
                        {
                            new Article()
                            {
                                //Title = "Title",
                                //Description = "Description",
                                //PicUrl = "http://imgcdn.chitunion.com/group4/M00/00/B4/Qw0DAFsXsF2AdnZIAAAm7YXb6-M798.JPG",
                                //Url = "https://www.bejson.com/"
                                Title = model.SubArticleInfo.Title,
                                Description = model.SubArticleInfo.Description,
                                PicUrl = model.SubArticleInfo.PicUrl,
                                Url =url
                            }
                        };
                        Task.Factory.StartNew(async () =>
                        {
                            await
                                Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendNewsAsync(WeixinAppId, requestMessage.FromUserName, listAr);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("SendSubscribedMsgInfo", ex);
            }
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            Loger.Log4Net.Info($"OnEvent_SubscribeRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            if (!string.IsNullOrEmpty(requestMessage.EventKey))
            {
                var sceneId = requestMessage.EventKey.Replace("qrscene_", "");
                var provider = new QrLoginProvider();
                var reqKeyValue = provider.VerifyScenceStr(sceneId);
                if (reqKeyValue != null)
                {
                    Loger.Log4Net.Info($"OnEvent_SubscribeRequest 订阅（关注）事件 （扫码事件推送） Pc端登录 VerifyScenceStr 成功");
                    if (reqKeyValue.t == ReqMessageType.Pc端登录)
                    {
                        //pc端扫码登录业务
                        Loger.Log4Net.Info($"OnEvent_ScanRequest 订阅（关注）事件 （扫码事件推送） Pc端登录 进行 SimulationLogin 模拟登录-开始");
                        var retValue = provider.SimulationLogin(new ReqSimulationLoginDto
                        {
                            WeixinOpendId = requestMessage.FromUserName,
                            Ticket = reqKeyValue.v,
                            EventKey = requestMessage.EventKey
                        });
                        if (retValue.HasError)
                        {
                            Loger.Log4Net.Info($"OnEvent_SubscribeRequest （扫码事件推送） Pc端登录 SimulationLogin失败:{retValue.Message}");
                        }
                        else
                        {
                            Loger.Log4Net.Info($"OnEvent_SubscribeRequest （扫码事件推送） Pc端登录 进行 SimulationLogin 模拟登录-成功-结束");
                        }
                        return new SuccessResponseMessage();
                    }
                    Loger.Log4Net.Info($"OnEvent_SubscribeRequest （扫码事件推送） Pc端登录 VerifyScenceStr-结束");
                }
            }
            ITSC.Chitunion2017.Entities.WeChat.WeiXinUser wxuser = new ITSC.Chitunion2017.Entities.WeChat.WeiXinUser();



            #region 获取用户信息
            var openId = requestMessage.FromUserName;
            Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
            Loger.Log4Net.Info($"OnEvent_SubscribeRequest调用Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info方法，参数appId={appId},openid={userInfo.openid},结果={(userInfo != null ? JsonConvert.SerializeObject(userInfo) : "对象为空")}");
            if (userInfo == null)
            {
                while (true)
                {
                    userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
                    if (userInfo != null)
                    {
                        break;
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(requestMessage.EventKey))
            {
                try
                {
                    var qrJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ReqKeyValueDto>(requestMessage.EventKey.Replace("qrscene_", ""));
                    if (qrJson != null)
                    {
                        wxuser.Source = (int)qrJson.t;
                        wxuser.Inviter = qrJson.v;
                    }
                }
                catch (Exception ex)
                {
                    wxuser.Source = 1;
                    wxuser.Inviter = requestMessage.EventKey.Replace("qrscene_", "");
                }
            }
            var temp = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(requestMessage.FromUserName);

            if (temp == null)
            {
                responseMessage.Content = GetWelcomeMsg();
                //responseMessage.Content = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");
                #region 微信用户
                try
                {
                    wxuser.subscribe = userInfo.subscribe;
                    wxuser.openid = userInfo.openid;
                    wxuser.nickname = userInfo.nickname;
                    wxuser.sex = userInfo.sex;
                    wxuser.city = userInfo.city;
                    wxuser.country = userInfo.country;
                    wxuser.province = userInfo.province;
                    wxuser.language = userInfo.language;
                    wxuser.headimgurl = userInfo.headimgurl;
                    wxuser.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfo.subscribe_time);
                    wxuser.unionid = userInfo.unionid;
                    wxuser.remark = userInfo.remark;
                    wxuser.groupid = userInfo.groupid;
                    wxuser.tagid_list = string.Join(",", userInfo.tagid_list);
                    wxuser.UserID = 0;
                    wxuser.CreateTime = DateTime.Now;
                    wxuser.LastUpdateTime = wxuser.CreateTime;
                    //wxuser.QRcode = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.CreateQrCode(appId, userInfo.openid);
                    //if (wxuser.QRcode != null)
                    //{
                    //    wxuser.InvitationQR = ITSC.Chitunion2017.BLL.WeChat.ImageOperate.Instance.GetActivityQRcode(wxuser.QRcode, Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomWidth")), Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ImgZoomHight")));
                    //}
                    wxuser.Status = userInfo.subscribe == 1 ? 0 : -1;
                    wxuser.RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号;
                    wxuser.RegisterType = (int)RegisterTypeEnum.微信;
                    wxuser.UserType = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeixinInfo.Instance.GetRecIDByAppId(appId);

                    bool insertUser = ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.WeiXinUserOperation(wxuser);
                    if (insertUser)
                    {

                        var user = XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUnionAndUserId(userInfo.openid);
                        if (temp == null)
                        {
                            XYAuto.ITSC.Chitunion2017.BLL.WechatInvite.WechatInvite.Instance.FriendFollow(user.UserID);
                        }
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(user.UserID);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Info("[订阅（关注）事件]" + ex.ToString());
                }
                #endregion

                SendSubscribedMsgInfo(requestMessage);

                return responseMessage;
            }
            else
            {
                //openId 存在的情况也有可能是新用户
                SendSubscribedMsgInfo(requestMessage);

                if (temp.Status == -1)
                {
                    responseMessage.Content = GetWelcomeMsg();
                    //responseMessage.Content = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");
                    //XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(0, DateTime.Now, openId);
                    temp.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfo.subscribe_time);
                    temp.subscribe = 1;
                    temp.LastUpdateTime = DateTime.Now;
                    temp.Source = wxuser.Source == 102 ? 3 : wxuser.Source;
                    temp.Inviter = wxuser.Inviter;
                    temp.Status = 0;
                    ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.Update(temp);
                    XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(temp.UserID);
                    return responseMessage;
                }

            }
            return new SuccessResponseMessage();
            //return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            Loger.Log4Net.Info($"OnEvent_UnsubscribeRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(-1, requestMessage.CreateTime, requestMessage.FromUserName);
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件(scancode_push)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之扫码推事件";
        //    return responseMessage;
        //}

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之扫码推事件且弹出“消息接收中”提示框";
        //    return responseMessage;
        //}

        /// <summary>
        /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出拍照或者相册发图";
        //    return responseMessage;
        //}

        /// <summary>
        /// 事件之弹出系统拍照发图(pic_sysphoto)
        /// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出系统拍照发图";
        //    return responseMessage;
        //}

        /// <summary>
        /// 事件之弹出微信相册发图器(pic_weixin)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出微信相册发图器";
        //    return responseMessage;
        //}

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        //{
        //    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "事件之弹出地理位置选择器";
        //    return responseMessage;
        //}

        /// <summary>
        /// 事件之发送模板消息返回结果
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //        public override IResponseMessageBase OnEvent_TemplateSendJobFinishRequest(RequestMessageEvent_TemplateSendJobFinish requestMessage)
        //        {
        //            switch (requestMessage.Status)
        //            {
        //                case "success":
        //                    //发送成功

        //                    break;
        //                case "failed:user block":
        //                    //送达由于用户拒收（用户设置拒绝接收公众号消息）而失败
        //                    break;
        //                case "failed: system failed":
        //                    //送达由于其他原因失败
        //                    break;
        //                default:
        //                    throw new WeixinException("未知模板消息状态：" + requestMessage.Status);
        //            }

        //            //注意：此方法内不能再发送模板消息，否则会造成无限循环！

        //            try
        //            {
        //                var msg = @"已向您发送模板消息
        //状态：{0}
        //MsgId：{1}
        //（这是一条来自MessageHandler的客服消息）".FormatWith(requestMessage.Status, requestMessage.MsgID);
        //                CustomApi.SendText(appId, WeixinOpenId, msg);//发送客服消息
        //            }
        //            catch (Exception e)
        //            {
        //                Senparc.Weixin.WeixinTrace.SendCustomLog("模板消息发送失败", e.ToString());
        //            }


        //            //无需回复文字内容
        //            //return requestMessage
        //            //    .CreateResponseMessage<ResponseMessageNoResponse>();
        //            return null;
        //        }

        #region 微信认证事件推送

        //public override IResponseMessageBase OnEvent_QualificationVerifySuccess(RequestMessageEvent_QualificationVerifySuccess requestMessage)
        //{
        //    return new SuccessResponseMessage();
        //}

        #endregion
    }
}