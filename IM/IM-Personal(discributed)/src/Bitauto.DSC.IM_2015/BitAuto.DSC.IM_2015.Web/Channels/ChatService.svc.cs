using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Core.Messages;

namespace BitAuto.DSC.IM_2015.Web.Channels
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ChatService
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        // Add more operations here and mark them with [OperationContract]

        [OperationContract]
        public void SendMessage(string clientPrivateToken, string message)
        {
            //ChatMessage chatMessage = new ChatMessage();

            ////
            ////  get who the message is from
            //CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(clientPrivateToken);

            ////  get the display name
            //chatMessage.From = cometClient.DisplayName;
            //chatMessage.Message = message;



            //DefaultChannelHandler.StateManager.SendMessage(cometClient.PublicToken, "ChatMessage", chatMessage);

            //// Add your operation implementation here
            //return;



            StringBuilder chatMessage = new StringBuilder();
            CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(clientPrivateToken);

            //  get the display name
            //chatMessage.From = cometClient.DisplayName;
            //chatMessage.Message = message;

            chatMessage.Append("{");
            chatMessage.Append("\"From\":\"" + cometClient.DisplayName + "\",");
            chatMessage.Append("\"Message\":\"" + message + "\"");
            chatMessage.Append("}");


            //DefaultChannelHandler.StateManager.SendMessage(cometClient.PrivateToken, "ChatMessage", chatMessage.ToString(),"127.0.0.1");

            // Add your operation implementation here
            return;
        }
    }
}
