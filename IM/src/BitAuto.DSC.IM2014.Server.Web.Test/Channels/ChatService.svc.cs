using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using BitAuto.DSC.IM2014.Core;
using BitAuto.DSC.IM2014.Server.Web.Test;

namespace BitAuto.DSC.IM2014.Server.Web.Test.Channels
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ChatService
    {
        // 要使用 HTTP GET，请添加 [WebGet] 特性。(默认 ResponseFormat 为 WebMessageFormat.Json)
        // 要创建返回 XML 的操作，
        //     请添加 [WebGet(ResponseFormat=WebMessageFormat.Xml)]，
        //     并在操作正文中包括以下行:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public void SendMessage(string clientPrivateToken, string message)
        {
            ChatMessage chatMessage = new ChatMessage();

            //
            //  get who the message is from
            CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(clientPrivateToken);

            //  get the display name
            chatMessage.From = cometClient.DisplayName;
            chatMessage.Message = message;

            DefaultChannelHandler.StateManager.SendMessage(cometClient.PublicToken, "ChatMessage", chatMessage);

            // Add your operation implementation here
            return;
        }

        // 在此处添加更多操作并使用 [OperationContract] 标记它们
    }
}
