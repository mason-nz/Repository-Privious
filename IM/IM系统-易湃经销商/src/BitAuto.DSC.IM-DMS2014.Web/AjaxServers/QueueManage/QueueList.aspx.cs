using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Core;
using BitAuto.DSC.IM_DMS2014.Web.Channels;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.QueueManage
{
    public partial class QueueList : System.Web.UI.Page
    {
        private string AgentID
        {

            //get { return HttpContext.Current.Request["agentid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["agentid"].ToString()); }
            get { return BLL.Util.GetLoginUserID().ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            BindRpt();
        }

        private void BindRpt()
        {
            List<CometClient> lstCometWait = new List<CometClient>();

            var comet = DefaultChannelHandler.StateManager.GetCometClient(AgentID);
            if (comet != null && comet.WaitUserList.Length > 0)
            {
                foreach (var l in comet.WaitUserList)
                {
                    lstCometWait.Add(DefaultChannelHandler.StateManager.GetCometClient(l.ToString()));
                }
            }

            //lstCometWait.Add(new CometClient() { MemberName = "mmname", Address = "地址", CityGroupName = "城市群", UserReferTitle = "title", LastConBeginTime = new DateTime(2014, 11, 3), ConverSTime = new DateTime(2014, 11, 3, 18, 30, 50) });
            //lstCometWait.Add(new CometClient() { MemberName = "mmname", Address = "地址", CityGroupName = "城市群", UserReferTitle = "title", LastConBeginTime = new DateTime(2014, 11, 3), ConverSTime = new DateTime(2014, 11, 3, 18, 30, 50) });
            //lstCometWait.AddRange(DefaultChannelHandler.StateManager.GetAllCometClients());
            rpt.DataSource = lstCometWait;
            rpt.DataBind();

        }

        /*
             lstCometWait[0].MemberName; lstCometWait[0].Address; lstCometWait[0].CityGroupName;
    lstCometWait[0].LastConBeginTime; lstCometWait[0].LastMessageTime; lstCometWait[0].Distribution;
    lstCometWait[0].UserReferTitle; lstCometWait[0].ConverSTime
         */
        public string GetConnetTime(DateTime dts)
        {

            var t = (DateTime.Now - dts);
            return string.Format("{0}:{1}:{2}", FormatNumber(t.Hours), FormatNumber(t.Minutes), FormatNumber(t.Seconds));
        }

        private string FormatNumber(int num)
        {
            if (num < 10)
            {
                return "0" + num.ToString();
            }
            else
            {
                return num.ToString();
            }
        }

    }
}