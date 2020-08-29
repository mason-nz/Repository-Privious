using System.Collections.Generic;
using System.IO;
using System.Text;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.Entities.Request;
using System.Web.Configuration;
using Senparc.Weixin;
using XYAuto.ChiTu2018.WeChat.CommonService.MessageHandlers.CustomMessageHandler;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.WeChat.CommonService.CustomMessageHandler
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
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info($"OnTextRequest requestMessage:{JsonConvert.SerializeObject(requestMessage)}");
            string content = requestMessage.Content;
            if (requestMessage.Content == "分享教学")
            {
                var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.Add(new Article()
                {
                    Title = "我有一份赚钱秘籍，你想不想搞？",
                    Description = "来赤兔联盟，赚钱就是这么简单",
                    PicUrl = "http://imgcdn.chitunion.com/group4/M00/63/85/QQ0DAFqYz_KAKWkFAAGOmvO_Te0873.jpg",
                    Url = "http://mp.weixin.qq.com/s/_BVNGtgnCGQy0OJfJdYQ7w"
                });
                return responseMessage;
            }
            else if (requestMessage.Content == "赤兔" || requestMessage.Content == "赤兔联盟" || requestMessage.Content.ToUpper() == "H5")
            {
                var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.Add(new Article()
                {
                    Title = "想知道赤兔联盟是什么吗？",
                    Description = "赤兔联盟是什么，点了你就全知道",
                    PicUrl = "http://imgcdn.chitunion.com/group4/M00/71/2D/QQ0DAFqfsqOAV5xCAAXkeWKgqHM495.png",//"http://imgcdn.chitunion.com/group4/M00/6B/9F/QQ0DAFqfiHiAd3EVAAAr7RJ7lI0560.JPG",//http://imgcdn.chitunion.com/group4/M00/6D/D5/QQ0DAFqfkKOAcpJAAAAy9PdAlOY860.JPG
                    Url = "https://h5.xingyuanauto.com/201802/RedPaper/?hmsr=RedPaper"
                });
                return responseMessage;
            }
            else if (requestMessage.Content == "头条" || requestMessage.Content == "微博")
            {
                var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = $@"您已成功参加活动，并进入抽奖环节
我们会在活动结束后三个工作日内公布中奖名单，敬请关注！";
                return responseMessage;
            }
            //else if (content == "妇女节" || content == "38" ||
            //         content == "三八妇女节" || content == "三八女神节" ||
            //         content == "女神节" || content == "女神" ||
            //         content == "抽奖")
            //{
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageNews>();
            //    responseMessage.Articles.Add(new Article()
            //    {
            //        Title = "翻牌吧，女王大人！",
            //        Description = "女王大人，来试试手气呗~",
            //        PicUrl = "http://imgcdn.chitunion.com/group4/M00/8D/1D/Qw0DAFqhApiALznaAAeZAJd4sCg274.jpg",
            //        Url = "https://16226447-10.hd.faisco.cn/16226447/uwu6Si3MnBEZ-MSMKgAXIA/jrdpd.html?fromImgMsg=false"
            //    });
            //    return responseMessage;
            //}
            else if (content == "兔妹")
            {
                //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
                //responseMessage.Content = "你好，兔妹正在赶来的路上，马上就来哦~";
                var responseMessage = this.CreateResponseMessage<ResponseMessageImage>();
                responseMessage.Image.MediaId = XYAuto.CTUtils.Config.ConfigurationUtil.GetAppSettingValue("CommunicationMediaId");//公众号永久图片ID
                return responseMessage;
            }
            return GetDefaultResponseMessage();
        }


        private IResponseMessageBase GetDefaultResponseMessage()
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $@"Hi，我是兔妹

欢迎来到赤兔联盟，分享文章，有人阅读就有钱赚

点击 →<a href='{Domin}/moneyManager/make_money.html?channel=ctlmzdhf'>抢单赚钱</a>将文章分享出去

有不了解的地方，欢迎回复“兔妹”，添加我为好友，关于赤兔联盟的问题，兔妹都会为大家耐心解答。

兔妹出没的时间为工作日的9:30-18:00，欢迎来撩~";
            //            responseMessage.Content = $@"Hi，我是兔妹，等你好久啦~
            //你可以回复“兔妹”，添加我为好友
            //关于赤兔联盟的问题，兔妹都会为你耐心解答。
            //兔妹会手把手的教你如何操作哦~
            //兔妹出没的时间：9:30-18:00，欢迎来撩~";
            //            responseMessage.Content = $@"Hi，我是兔妹，等你好久啦~
            //你只需要点击 →<a href='{Domin}/moneyManager/make_money.html?channel=ctlmhuifu'>抢单赚钱</a>将文章分享出去，文章分享后，只要有人阅读就能有钱赚。
            //你可以回复“兔妹”，添加我为好友
            //兔妹会手把手的教你如何操作哦~
            //兔妹出没的时间：9:30-18:00，欢迎来撩~";
            //            responseMessage.Content = @"Hi，我是兔妹，如果您这边出现无法分享等问题，请不用担心，

            //赤兔系统暂时出现一些小问题，兔兔正在催促技术小哥哥加班加点进行修复哦，请耐心等待一段时间

            //如果有更多问题，请直接回复“兔妹”，兔妹会一对一帮您进行解答的~

            //兔妹出没的时间：9:30-18:00，其他时间可能回复比较慢，敬请谅解哦~";
            return responseMessage;
        }        

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
            /* 所有没有被处理的消息会默认返回这里的结果，
            * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
            * 只需要在这里统一发出委托请求，如：
            * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            * return responseMessage;
            */
            //return null;
            return GetDefaultResponseMessage();
            //var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "OpenId:" + responseMessage.FromUserName + "\r\n ToUser:" + responseMessage.ToUserName + "\r\n MsgType:" + responseMessage.MsgType;
            //return responseMessage;
        }
        
    }
}
