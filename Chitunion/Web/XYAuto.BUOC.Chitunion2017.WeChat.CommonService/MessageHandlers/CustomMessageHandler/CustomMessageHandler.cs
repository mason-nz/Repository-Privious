using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.Entities.Request;
using System.Web.Configuration;
using Senparc.Weixin;
using XYAuto.BUOC.Chitunion2017.WeChat.CommonService.MessageHandlers.CustomMessageHandler;

namespace XYAuto.BUOC.Chitunion2017.WeChat.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        private string Domin = WebConfigurationManager.AppSettings["Domin"];
        private string HuoDong_Domin = WebConfigurationManager.AppSettings["HuoDong_Domin"];
        private string WeChatMenuClickDataPath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("WeChatMenuClickDataPath", true);

        /// <summary>
        /// 模板消息集合（Key：checkCode，Value：OpenId）
        /// </summary>
        public static Dictionary<string, string> TemplateMessageCollection = new Dictionary<string, string>();

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            WeixinContext.ExpireMinutes = 3;

            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }

            //在指定条件下，不使用消息去重
            base.OmitRepeatedMessageFunc = requestMessage =>
            {
                var textRequestMessage = requestMessage as RequestMessageText;
                if (textRequestMessage != null && textRequestMessage.Content == "容错")
                {
                    return false;
                }
                return true;
            };
        }

        public CustomMessageHandler(RequestMessageBase requestMessage)
            : base(requestMessage)
        {
        }

        public override void OnExecuting()
        {
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //说明：实际项目中这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs
            //var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();

            //var requestHandler = requestMessage.StartHandler()
            //    //关键字不区分大小写，按照顺序匹配成功后将不再运行下面的逻辑

            //#region Default
            //    //Default不一定要在最后一个
            //    .Default(() =>
            //    {
            //        var result = new StringBuilder();
            //        result.AppendFormat("您刚才发送了文字信息：{0}\r\n\r\n", requestMessage.Content);

            //        if (CurrentMessageContext.RequestMessages.Count > 1)
            //        {
            //            result.AppendFormat("您刚才还发送了如下消息（{0}/{1}）：\r\n", CurrentMessageContext.RequestMessages.Count,
            //                CurrentMessageContext.StorageData);
            //            for (int i = CurrentMessageContext.RequestMessages.Count - 2; i >= 0; i--)
            //            {
            //                var historyMessage = CurrentMessageContext.RequestMessages[i];
            //                result.AppendFormat("{0} 【{1}】{2}\r\n",
            //                    historyMessage.CreateTime.ToString("HH:mm:ss"),
            //                    historyMessage.MsgType.ToString(),
            //                    (historyMessage is RequestMessageText)
            //                        ? (historyMessage as RequestMessageText).Content
            //                        : "[非文字类型]"
            //                    );
            //            }
            //            result.AppendLine("\r\n");
            //        }

            //        result.AppendFormat("如果您在{0}分钟内连续发送消息，记录将被自动保留（当前设置：最多记录{1}条）。过期后记录将会自动清除。\r\n",
            //            WeixinContext.ExpireMinutes, WeixinContext.MaxRecordCount);
            //        result.AppendLine("\r\n");
            //        defaultResponseMessage.Content = result.ToString();
            //        return defaultResponseMessage;
            //    });
            //#endregion
            //return requestHandler.GetResponseMessage() as IResponseMessageBase;

            var model = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetMenuDataByAppId(WeChatMenuClickDataPath, appId);
            if (model != null)
            {
                string content = requestMessage.Content;
                var result = model.CustomMsgList.FirstOrDefault(s => s.Key == content);
                if (result != null)
                {
                    switch (result.Type.ToLower())
                    {
                        case "msg":
                            var responseMessageMsg = this.CreateResponseMessage<ResponseMessageText>();
                            responseMessageMsg.Content = result.Value;
                            return responseMessageMsg;
                            break;
                        case "media":
                            var responseMessageMedia = this.CreateResponseMessage<ResponseMessageImage>();
                            responseMessageMedia.Image.MediaId = result.Value;
                            return responseMessageMedia;
                            break;
                        default:
                            return GetDefaultResponseMessage(model.DefaultCustomMsg);
                            break;
                    }
                }
                else
                {
                    return GetDefaultResponseMessage(model.DefaultCustomMsg);
                }
            }
            else
            {
                return GetDefaultResponseMessage(string.Empty);
            }


            //string content = requestMessage.Content;
            //if (requestMessage.Content == "分享教学")
            //{
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();
            //    responseMessage.Articles.Add(new Article()
            //    {
            //        Title = "我有一份赚钱秘籍，你想不想搞？",
            //        Description = "来赤兔联盟，赚钱就是这么简单",
            //        PicUrl = "http://imgcdn.chitunion.com/group4/M00/63/85/QQ0DAFqYz_KAKWkFAAGOmvO_Te0873.jpg",
            //        Url = "http://mp.weixin.qq.com/s/_BVNGtgnCGQy0OJfJdYQ7w"
            //    });
            //    return responseMessage;
            //}
            //else if (requestMessage.Content == "赤兔" || requestMessage.Content == "赤兔联盟" || requestMessage.Content.ToUpper() == "H5")
            //{
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();
            //    responseMessage.Articles.Add(new Article()
            //    {
            //        Title = "想知道赤兔联盟是什么吗？",
            //        Description = "赤兔联盟是什么，点了你就全知道",
            //        PicUrl = "http://imgcdn.chitunion.com/group4/M00/71/2D/QQ0DAFqfsqOAV5xCAAXkeWKgqHM495.png",//"http://imgcdn.chitunion.com/group4/M00/6B/9F/QQ0DAFqfiHiAd3EVAAAr7RJ7lI0560.JPG",//http://imgcdn.chitunion.com/group4/M00/6D/D5/QQ0DAFqfkKOAcpJAAAAy9PdAlOY860.JPG
            //        Url = "https://h5.xingyuanauto.com/201802/RedPaper/?hmsr=RedPaper"
            //    });
            //    return responseMessage;
            //}
            ////else if (content == "妇女节" || content == "38" ||
            ////         content == "三八妇女节" || content == "三八女神节" ||
            ////         content == "女神节" || content == "女神" ||
            ////         content == "抽奖")
            ////{
            ////    var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();
            ////    responseMessage.Articles.Add(new Article()
            ////    {
            ////        Title = "翻牌吧，女王大人！",
            ////        Description = "女王大人，来试试手气呗~",
            ////        PicUrl = "http://imgcdn.chitunion.com/group4/M00/8D/1D/Qw0DAFqhApiALznaAAeZAJd4sCg274.jpg",
            ////        Url = "https://16226447-10.hd.faisco.cn/16226447/uwu6Si3MnBEZ-MSMKgAXIA/jrdpd.html?fromImgMsg=false"
            ////    });
            ////    return responseMessage;
            ////}
            //else if (content == "兔妹")
            //{
            //    //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //    //responseMessage.Content = "你好，兔妹正在赶来的路上，马上就来哦~";
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageImage>();
            //    responseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CommunicationMediaId");//公众号永久图片ID
            //    return responseMessage;
            //}
            //else if (content == "兔多多" )
            //{
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageImage>();
            //    responseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("QRCodeMediaId");//公众号永久图片ID
            //    return responseMessage;
            //}
            //else if (content == "中奖" || content == "冲顶大会" || content == "冲顶" || content == "抽奖")
            //{
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageImage>();
            //    responseMessage.Image.MediaId = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Winner");//公众号永久图片ID
            //    return responseMessage;
            //}
            //return GetDefaultResponseMessage();
        }


        private IResponseMessageBase GetDefaultResponseMessage(string msg)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = msg;
            return responseMessage;
            //            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //            responseMessage.Content = $@"Hi，我是兔妹，有什么能够帮你的吗？

            //来赤兔，享无限惊喜，点击→<a href='{Domin}/index.html?channel=ctlmzdhf'>抢任务</a>

            //海量任务，奖励多多，等你来抢！

            //更多问题，欢迎回复{"\""}兔妹{"\""}进行咨询

            //（客服出没时间：工作日9: 30 - 18:00）";
            //            return responseMessage;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        //{
        //    var locationService = new LocationService();
        //    var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
        //    return responseMessage;
        //}

        /// <summary>
        /// 小视频
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        //{
        //    var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您刚才发送的是小视频";
        //    return responseMessage;
        //}

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        //{
        //    //一隔一返回News或Image格式
        //    if (base.WeixinContext.GetMessageContext(requestMessage).RequestMessages.Count() % 2 == 0)
        //    {
        //        var responseMessage = CreateResponseMessage<ResponseMessageNews>();

        //        responseMessage.Articles.Add(new Article()
        //        {
        //            Title = "您刚才发送了图片信息",
        //            Description = "您发送的图片将会显示在边上",
        //            PicUrl = requestMessage.PicUrl,
        //            Url = "http://wxtest-ct.qichedaquan.com"
        //        });
        //        responseMessage.Articles.Add(new Article()
        //        {
        //            Title = "第二条",
        //            Description = "第二条带连接的内容",
        //            PicUrl = requestMessage.PicUrl,
        //            Url = "http://wxtest-ct.qichedaquan.com"
        //        });

        //        return responseMessage;
        //    }
        //    else
        //    {
        //        var responseMessage = CreateResponseMessage<ResponseMessageImage>();
        //        responseMessage.Image.MediaId = requestMessage.MediaId;
        //        return responseMessage;
        //    }
        //}

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
        //    //上传缩略图
        //    //var accessToken = Containers.AccessTokenContainer.TryGetAccessToken(appId, appSecret);
        //    var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadTemporaryMedia(appId, UploadMediaFileType.image,
        //                                                 Server.GetMapPath("~/Images/Logo.jpg"));

        //    //设置音乐信息
        //    responseMessage.Music.Title = "天籁之音";
        //    responseMessage.Music.Description = "播放您上传的语音";
        //    responseMessage.Music.MusicUrl = "http://wxtest-ct.qichedaquan.com/Media/GetVoice?mediaId=" + requestMessage.MediaId;
        //    responseMessage.Music.HQMusicUrl = "http://wxtest-ct.qichedaquan.com/Media/GetVoice?mediaId=" + requestMessage.MediaId;
        //    responseMessage.Music.ThumbMediaId = uploadResult.media_id;

        //    //推送一条客服消息
        //    try
        //    {
        //        CustomApi.SendText(appId, WeixinOpenId, "本次上传的音频MediaId：" + requestMessage.MediaId);

        //    }
        //    catch {
        //    }

        //    return responseMessage;
        //}

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        //{
        //    var responseMessage = CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;

        //    #region 上传素材并推送到客户端

        //    Task.Factory.StartNew(async () =>
        //     {
        //         //上传素材
        //         var dir = Server.GetMapPath("~/App_Data/TempVideo/");
        //         var file = await MediaApi.GetAsync(appId, requestMessage.MediaId, dir);
        //         var uploadResult = await MediaApi.UploadTemporaryMediaAsync(appId, UploadMediaFileType.video, file, 50000);
        //         await CustomApi.SendVideoAsync(appId, base.WeixinOpenId, uploadResult.media_id, "这是您刚才发送的视频", "这是一条视频消息");
        //     }).ContinueWith(async task =>
        //     {
        //         if (task.Exception != null)
        //         {
        //             WeixinTrace.Log("OnVideoRequest()储存Video过程发生错误：", task.Exception.Message);

        //             var msg = string.Format("上传素材出错：{0}\r\n{1}",
        //                        task.Exception.Message,
        //                        task.Exception.InnerException != null
        //                            ? task.Exception.InnerException.Message
        //                            : null);
        //             await CustomApi.SendTextAsync(appId, base.WeixinOpenId, msg);
        //         }
        //     });

        //    #endregion

        //    return responseMessage;
        //}


        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        //        {
        //            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
        //            responseMessage.Content = string.Format(@"您发送了一条连接信息：
        //Title：{0}
        //Description:{1}
        //Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
        //            return responseMessage;
        //        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var model = XYAuto.ITSC.Chitunion2017.BLL.WeChat.MenuHelper.Instance.GetMenuDataByAppId(WeChatMenuClickDataPath, appId);
            if (model != null)
            {
                return GetDefaultResponseMessage(model.DefaultCustomMsg);
            }
            return null;

            /* 所有没有被处理的消息会默认返回这里的结果，
                * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
                * 只需要在这里统一发出委托请求，如：
                * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
                * return responseMessage;
                */
            //return null;
            //return GetDefaultResponseMessage();
            //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "OpenId:" + responseMessage.FromUserName + "\r\n ToUser:" + responseMessage.ToUserName + "\r\n MsgType:" + responseMessage.MsgType;
            //return responseMessage;
        }


        //public override IResponseMessageBase OnUnknownTypeRequest(RequestMessageUnknownType requestMessage)
        //{
        //    /*
        //     * 此方法用于应急处理SDK没有提供的消息类型，
        //     * 原始XML可以通过requestMessage.RequestDocument（或this.RequestDocument）获取到。
        //     * 如果不重写此方法，遇到未知的请求类型将会抛出异常（v14.8.3 之前的版本就是这么做的）
        //     */
        //    var msgType = MsgTypeHelper.GetRequestMsgTypeString(requestMessage.RequestDocument);
        //    var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
        //    responseMessage.Content = "未知消息类型：" + msgType;

        //    WeixinTrace.SendCustomLog("未知请求消息类型", requestMessage.RequestDocument.ToString());//记录到日志中

        //    return responseMessage;
        //}
    }
}
