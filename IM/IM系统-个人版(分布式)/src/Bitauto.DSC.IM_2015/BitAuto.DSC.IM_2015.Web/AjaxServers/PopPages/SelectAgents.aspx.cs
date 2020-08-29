using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.PopPages
{
    public partial class SelectAgents : System.Web.UI.Page
    {
        #region 属性
        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TrueName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TrueName"]);
            }
        }
        /// <summary>
        /// 所属分组
        /// </summary>
        public string BGID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["bgid"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["bgid"]);
            }
        }

        #endregion

        public int GroupLength = 5;
        public int PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            //BGID = string.IsNullOrEmpty(HttpContext.Current.Request["BGID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGID"]);
            if (!IsPostBack)
            {
                getAgentS();
            }
        }


        private void getAgentS()
        {
            var userid = BLL.Util.GetLoginUserID();


            //var lstClients = DefaultChannelHandler.GetAllCometClients();
            var lstClients = DefaultChannelHandler.StateManager.GetWCFAllAgents();

            var AgentClient = lstClients.Where(c => c.Status == (int)AgentStatus.Online && c.AgentID != userid).ToList();
            //var AgentClient = lstClients.Where(c => c.Type == 1 && c.Status == AgentStatus.Online).ToList();
            //var AgentClient = lstClients.Where(c => c.Type == 1 ).ToList();

            //var listBGID = (from c in AgentClient
            //                select new { value = c.InBGID, name = c.InBGIDName }).ToList();

            var lstAgent = (from c in AgentClient
                            select
                                new AgentS
                                {
                                    BGID = c.InBGID,
                                    BGName = c.InBGIDName,
                                    AgentName = c.AgentName,
                                    AgentNumber = c.AgentNum,
                                    AgentID = c.AgentID.ToString()
                                }
                ).ToList();

            //for (int i = 0; i < 45; i++)
            //{
            //    lstAgent.Add(new AgentS()
            //    {
            //        AgentID = i.ToString(),
            //        BGID = i.ToString(),
            //        BGName = i.ToString(),
            //        AgentName = (i % 2).ToString(),
            //        AgentNumber = i.ToString()
            //    });
            //}

            var Count = lstAgent.Count;
            IEnumerable<AgentS> lstAgentT = lstAgent;

            if (!string.IsNullOrEmpty(BGID) && BGID != "-1")
            {
                lstAgentT = lstAgent.Where(c => c.BGID == BGID);
            }
            if (!string.IsNullOrEmpty(TrueName))
            {
                lstAgentT = lstAgentT.Where(c => c.AgentName == TrueName);
            }
            lstAgent = lstAgentT.ToList();

            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
            {
                var lstDDl = (from p in lstAgent
                              group p by new
                              {
                                  value = p.BGID,
                                  name = p.BGName
                              }
                                  into g
                                  select new
                                  {
                                      g.Key.name,
                                      g.Key.value
                                  }).ToList();

                ddlBussiGroup.DataSource = lstDDl;
                ddlBussiGroup.DataBind();

                ddlBussiGroup.Items.Insert(0, new ListItem("全部", "-1"));
            }

            var lstBind = lstAgent.Skip((BLL.PageCommon.Instance.PageIndex - 1) * PageSize).Take(PageSize).ToList();


            DataTable dt = new DataTable();
            dt.Columns.Add("AgentID");
            dt.Columns.Add("BGID");
            dt.Columns.Add("BGName");
            dt.Columns.Add("AgentName");
            dt.Columns.Add("AgentNumber");

            DataRow dr;
            foreach (AgentS agentS in lstBind)
            {
                dr = dt.NewRow();
                dr["AgentID"] = agentS.AgentID;
                dr["BGID"] = agentS.BGID;
                dr["BGName"] = agentS.BGName;
                dr["AgentName"] = agentS.AgentName;
                dr["AgentNumber"] = agentS.AgentNumber;
                dt.Rows.Add(dr);
            }


            repterPersonlist.DataSource = dt;
            repterPersonlist.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, lstAgent.Count, PageSize, BLL.PageCommon.Instance.PageIndex, 2);
        }

    }

    public class AgentS
    {
        public string BGID;
        public string BGName;
        public string AgentName;
        public string AgentNumber;
        public string AgentID;

    }





}