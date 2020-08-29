using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.DSC.IM2014.Server.Web.Test.Channels;
using BitAuto.DSC.IM2014.Core;

namespace BitAuto.DSC.IM2014.Server.Web.Test.AjaxServers
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string msg = "";
            string username = context.Request.Form["username"];
            string message = context.Request.Form["message"];
            string action = context.Request.Form["action"];
            if (action == "init")
            {
                try
                {
                    DefaultChannelHandler.StateManager.InitializeClient(
                        username, username, username, 3, 5,1);
                    msg = "loginok";
                    //Response.Redirect("chat.aspx?username=" + this.username.Text);
                }
                catch (CometException ce)
                {
                    if (ce.MessageId == CometException.CometClientAlreadyExists)
                    {
                        //  ok the comet client already exists, so we should really show
                        //  an error message to the user
                        //this.errorMessage.Text = "User is already logged into the chat application.";
                        msg = "User is already logged into the chat application.";
                    }
                }
            }
            else if (action == "sendmessage")
            {
                ChatMessage chatMessage = new ChatMessage();

                //
                //  get who the message is from
                CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);

                //  get the display name
                chatMessage.From = cometClient.DisplayName;
                chatMessage.Message = message;

                DefaultChannelHandler.StateManager.SendMessage(cometClient.PublicToken, "ChatMessage", chatMessage);

                // Add your operation implementation here
                //return;
                msg = "sendok";
            }
            else if (action == "closechat")
            {
                ChatMessage chatMessage = new ChatMessage();

                //
                //  get who the message is from
                CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(username);

                DefaultChannelHandler.StateManager.KillIdleCometClient(cometClient.PrivateToken);

                msg = "sendok";
            }


            context.Response.Write("{\"result\":\"" + msg + "\"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}