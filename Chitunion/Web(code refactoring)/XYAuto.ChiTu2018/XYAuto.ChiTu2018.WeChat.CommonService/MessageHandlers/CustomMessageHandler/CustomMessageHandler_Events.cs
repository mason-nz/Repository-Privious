/*----------------------------------------------------------------
    Copyright (C) 2018 Senparc
    
    文件名：CustomMessageHandler_Events.cs
    文件功能描述：自定义MessageHandler
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Service.OAuth2.Dto;
using XYAuto.ChiTu2018.Service.Wechat;
using XYAuto.ChiTu2018.Service.Wechat.Dto;

namespace XYAuto.ChiTu2018.WeChat.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                case "10":
                case "Cooperation"://商务合作   Cooperation
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Image.MediaId = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("CooperationMediaId");
                    }
                    break;
                case "11":
                case "Communication"://加群交流   
                case "BackupWeixinNum"://备用微信号   
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Image.MediaId = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("BackupWeixMediaId");
                    }
                    break;
                case "Winner"://赢大奖   
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Image.MediaId = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Winner");
                    }
                    break;
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;
        }

        
        /// <summary>
        /// 通过二维码扫描关注扫描事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            //通过扫描关注
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            try
            {
                #region pc端扫码登录业务
                //if (!string.IsNullOrEmpty(requestMessage.EventKey))
                //{
                //    var sceneId = requestMessage.EventKey.Replace("qrscene_", "");
                //    var provider = new QrLoginProvider();
                //    var reqKeyValue = provider.VerifyScenceStr(sceneId);
                //    if (reqKeyValue != null)
                //    {
                //        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录:EventKey={requestMessage.EventKey} VerifyScenceStr 成功");
                //        if (reqKeyValue.t == ReqMessageType.Pc端登录)
                //        {
                //            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录 进行 SimulationLogin 模拟登录-开始");
                //            //pc端扫码登录业务
                //            var retValue = provider.SimulationLogin(new ReqSimulationLoginDto
                //            {
                //                WeixinOpendId = requestMessage.FromUserName,
                //                Ticket = reqKeyValue.v,
                //                EventKey = requestMessage.EventKey
                //            });
                //            if (retValue.HasError)
                //            {
                //                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(
                //                    $"OnEvent_ScanRequest 扫码事件推送 Pc端登录 SimulationLogin失败:{retValue.Message}");
                //            }
                //            else
                //            {
                //                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录 进行 SimulationLogin 模拟登录-成功-结束");
                //            }
                //            return new SuccessResponseMessage();
                //        }
                //        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest 扫码事件推送 Pc端登录 VerifyScenceStr-结束");
                //    }
                //}
                #endregion

                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    ScanQR(requestMessage);
                });
                var temp =
                    XYAuto.ChiTu2018.Service.Wechat.LEWeiXinUserService.Instance.GetUnionAndUserId(
                        requestMessage.FromUserName);
                if (temp == null)
                {
                    responseMessage.Content = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");

                    return responseMessage;
                }
                else
                {
                    if (temp.Status == -1)
                    {

                        responseMessage.Content = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");

                        return responseMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("OnEvent_ScanRequest" + ex.ToString());
                return new SuccessResponseMessage();
            }
            return new SuccessResponseMessage();
        }

        private void ScanQR(RequestMessageEvent_Scan requestMessage)
        {
            var wxuser = new WeiXinUserDto();

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

            if (LEWeiXinUserService.Instance.IsExistOpenId(userInfo.openid))
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("UpdateStatusByOpneId：" + userInfo.openid);
                LEWeiXinUserService.Instance.UpdateStatusByOpenId(0, DateTime.Now, userInfo.openid);
                var user = LEWeiXinUserService.Instance.GetUnionAndUserId(userInfo.openid);
                if (user != null)
                    XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport((int) user.UserID);
            }
            else
            {
                #region 插入微信用户
                try
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest 插入微信用户openid:{userInfo.openid}");
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
                    wxuser.Status = userInfo.subscribe == 1 ? 0 : -1;
                    wxuser.RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号;
                    wxuser.RegisterType = (int)RegisterTypeEnum.微信;
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
                    var insertUser = LEWeiXinUserService.Instance.WeiXinUserOperation(wxuser);
                    if (insertUser!=null)
                    {
                        LeInviteRecordService.Instance.FriendFollow((int) insertUser.UserID);
                    }
                }
                catch (Exception ex)
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("ScanQR：" + ex.ToString());
                    //return responseMessage;
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_SubscribeRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            #region PC端扫码
            //if (!string.IsNullOrEmpty(requestMessage.EventKey))
            //{
            //    var sceneId = requestMessage.EventKey.Replace("qrscene_", "");
            //    var provider = new QrLoginProvider();
            //    var reqKeyValue = provider.VerifyScenceStr(sceneId);
            //    if (reqKeyValue != null)
            //    {
            //        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_SubscribeRequest 订阅（关注）事件 （扫码事件推送） Pc端登录 VerifyScenceStr 成功");
            //        if (reqKeyValue.t == ReqMessageType.Pc端登录)
            //        {
            //            //pc端扫码登录业务
            //            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_ScanRequest 订阅（关注）事件 （扫码事件推送） Pc端登录 进行 SimulationLogin 模拟登录-开始");
            //            var retValue = provider.SimulationLogin(new ReqSimulationLoginDto
            //            {
            //                WeixinOpendId = requestMessage.FromUserName,
            //                Ticket = reqKeyValue.v,
            //                EventKey = requestMessage.EventKey
            //            });
            //            if (retValue.HasError)
            //            {
            //                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_SubscribeRequest （扫码事件推送） Pc端登录 SimulationLogin失败:{retValue.Message}");
            //            }
            //            else
            //            {
            //                XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_SubscribeRequest （扫码事件推送） Pc端登录 进行 SimulationLogin 模拟登录-成功-结束");
            //            }
            //            return new SuccessResponseMessage();
            //        }
            //        XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_SubscribeRequest （扫码事件推送） Pc端登录 VerifyScenceStr-结束");
            //    }
            //}
            #endregion

            #region 获取用户信息
            var openId = requestMessage.FromUserName;
            Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, openId, Language.zh_CN);
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_SubscribeRequest调用Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info方法，参数appId={appId},openid={userInfo.openid},结果={(userInfo != null ? JsonConvert.SerializeObject(userInfo) : "对象为空")}");
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

            var wxuser = new WeiXinUserDto();
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
            var temp = LEWeiXinUserService.Instance.GetUnionAndUserId(requestMessage.FromUserName);//XYAuto.ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.GetUserInfo(requestMessage.FromUserName);
            if (temp == null)
            {
                responseMessage.Content = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");
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
                    wxuser.LastUpdateTime = DateTime.Now;
                    wxuser.Status = userInfo.subscribe == 1 ? 0 : -1;
                    wxuser.RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号;
                    wxuser.RegisterType = (int)RegisterTypeEnum.微信;
                    var insertUser = LEWeiXinUserService.Instance.WeiXinUserOperation(wxuser);
                    if (insertUser?.UserID != null)
                    {
                        LeInviteRecordService.Instance.FriendFollow((int) insertUser.UserID);
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport((int) insertUser.UserID);
                    }
                }
                catch (Exception ex)
                {
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("[订阅（关注）事件]" + ex.ToString());
                }
                #endregion
                return responseMessage;
            }

            if (temp.Status != -1) return new SuccessResponseMessage();

            responseMessage.Content = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("Welcome");
            temp.subscribe_time = DateTimeHelper.GetDateTimeFromXml(userInfo.subscribe_time);
            temp.subscribe = 1;
            temp.LastUpdateTime = DateTime.Now;
            temp.Source = wxuser.Source == 102 ? 3 : wxuser.Source;
            temp.Inviter = wxuser.Inviter;
            temp.Status = 0;
            LEWeiXinUserService.Instance.Update(temp);
            if (temp.UserID != null) XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport((int) temp.UserID);

            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnEvent_UnsubscribeRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            //ITSC.Chitunion2017.BLL.WeChat.WeiXinUser.Instance.UpdateStatusByOpneId(-1, requestMessage.CreateTime, requestMessage.FromUserName);
            LEWeiXinUserService.Instance.UpdateStatusByOpenId(-1, requestMessage.CreateTime,
                responseMessage.FromUserName);
            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
            return responseMessage;
        }       
    }
}