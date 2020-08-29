using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Web.Channels;
using BitAuto.DSC.IM_2015.MainInterface;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.QueueManage
{
    public partial class WcfClientManagerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                DataBound();
            }
        }
        private void DataBound()
        {
            ProxyAgentClient[] agentarray = null;
            agentarray = DefaultChannelHandler.StateManager.GetWCFAllAgents();
            if (agentarray != null)
            {
                this.agentlist.DataSource = agentarray.ToList();

                this.agentlist.DataBind();
            }
            ProxyNetFriend[] wyarray = null;
            wyarray = DefaultChannelHandler.StateManager.GetWCFAllNetFriends();
            if (wyarray != null)
            {
                this.wylist.DataSource = wyarray.ToList();
                this.wylist.DataBind();
            }

            Core.CometWaitRequest[] RequestArray = DefaultChannelHandler.StateManager.GetAllRequest();
            if (ReqeustList != null)
            {
                this.ReqeustList.DataSource = RequestArray.ToList();
                this.ReqeustList.DataBind();
            }
            List<ProxyNetFriend> listFrind = new List<ProxyNetFriend>();
            foreach (SourceType type in BitAuto.DSC.IM_2015.BLL.Util.GetAllSourceType(false))
            {
                List<ProxyNetFriend> lFrind = DefaultChannelHandler.StateManager.GetWaitingCometClientsByBusinessLine(type.SourceTypeValue);
                if (lFrind != null)
                {
                    listFrind.AddRange(lFrind);
                }
            }
            if (listFrind != null)
            {
                dwylist.DataSource = listFrind;
                dwylist.DataBind();
            }
        }
    }
}