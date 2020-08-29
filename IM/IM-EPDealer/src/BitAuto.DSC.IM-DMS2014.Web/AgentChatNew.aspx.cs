using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Web.Channels;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class AgentChatNew : System.Web.UI.Page
    {
        public string AgentIMID
        {
            get
            {
                string agentid = "";
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["AgentIMID"]))
                {
                    agentid = HttpContext.Current.Request["AgentIMID"].ToString();
                    //Cache.Remove("agent_IMID");
                    //Cache.Insert("agent_IMID", agentid, null, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    //if (Cache["agent_IMID"] != null)
                    if (HttpContext.Current.Session["agent_IMID"]!=null)
                    {
                        agentid = HttpContext.Current.Session["agent_IMID"].ToString();
                    }
                }

                //return string.IsNullOrEmpty(HttpContext.Current.Request["AgentIMID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["AgentIMID"]);
                return agentid;
            }
        }
        public int AgentState
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["AgentState"]) == true ? -1 : Convert.ToInt16(HttpContext.Current.Request["AgentState"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            }
        }
    }
}