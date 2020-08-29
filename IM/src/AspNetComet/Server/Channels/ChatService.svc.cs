using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using MethodWorx.AspNetComet.Core;

namespace Server.Channels
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ChatService
    {
        // Add [WebGet] attribute to use HTTP GET
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

            DefaultChannelHandler.StateManager.SendMessage("ChatMessage", chatMessage);

            // Add your operation implementation here
            return;
        }
    }
}
