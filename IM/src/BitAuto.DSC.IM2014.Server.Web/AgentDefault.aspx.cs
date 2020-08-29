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
    public partial class AgentDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            if (!IsExistAgentIMID(this.username.Text))
            {
                //Response.Write("<script type='text/javascript'>alert('您的帐号不对，请联系系统管理员!');<script>");
                this.errorMessage.Text = "您的帐号不存在，请联系管理员!";
                return;
            }

            try
            {
                int connectionTimeoutSeconds = BLL.Util.GetConnectionTimeoutSeconds();
                int connectionIdleSeconds = BLL.Util.GetConnectionIdleSecondsAgent();               

                DefaultChannelHandler.StateManager.InitializeClient(
                    this.username.Text, this.username.Text, this.username.Text, connectionTimeoutSeconds, connectionIdleSeconds, (Int32)Entities.UserType.Agent);

                //Response.Redirect("ChatAgent.aspx?username=" + this.username.Text);
                //Response.Redirect("AgentChat.aspx?AgentIMID=" + this.username.Text + "&AgentState=1");
                Response.Redirect("AgentChatNew.aspx?AgentIMID=" + this.username.Text + "&AgentState=1");
                //Response.Write("<script language=javascript>window.open('AgentChat.aspx?AgentIMID=" + this.username.Text + "&AgentState=1','newwindow','height=100px,width=600px,status=no,toolbar=no, menubar=no,location=no,scrollbars=no,resizeable=no,top=300,left=200')</script>");
            }
            catch (CometException ce)
            {
                if (ce.MessageId == CometException.CometClientAlreadyExists)
                {
                    //  ok the comet client already exists, so we should really show
                    //  an error message to the user
                    //this.errorMessage.Text = "User is already logged into the chat application.";
                    this.errorMessage.Text = "您已经登录，不能重复登录";
                }
            }
        }

        /// <summary>
        /// 从集中权限系统取坐席标识，如果帐号存在则允许登录
        /// </summary>
        /// <returns></returns>
        private bool IsExistAgentIMID(string username)
        {
            return BLL.AgentInfo.Instance.IsExistsByAgentID(username);
        }
    }
}