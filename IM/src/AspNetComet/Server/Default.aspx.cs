using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Server.Channels;

using MethodWorx.AspNetComet.Core;

namespace Server
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
                    this.username.Text, this.username.Text, this.username.Text, 5, 5);

                Response.Redirect("chat.aspx?username=" + this.username.Text);
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
