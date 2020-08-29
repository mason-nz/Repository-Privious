using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM2014.Server.Web.Channels;
using BitAuto.DSC.IM2014.Core;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
                DefaultChannelHandler.StateManager.InitializeClient(
                    this.username.Text, this.username.Text, this.username.Text, 5, 5,(Int32)Entities.UserType.Agent);

                //Response.Redirect("ChatAgent.aspx?username=" + this.username.Text);
                Response.Redirect("/Agent/AgentDefault.aspx?AgentID=" + this.username.Text);
            }
            catch (CometException ce)
            {
                if (ce.MessageId == CometException.CometClientAlreadyExists)
                {
                    //  ok the comet client already exists, so we should really show
                    //  an error message to the user
                    this.errorMessage.Text = "User is already logged into the chat application.";
                }
            }
        }
    }
}