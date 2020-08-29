using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class StatesDetails : PageBase
    {
        #region 定义属性


        public string RequestPageSize
        {
            get
            {
                return Request.QueryString["pageSize"] == null
                    ? PageCommon.Instance.PageSize.ToString()
                    : Request.QueryString["pageSize"].Trim();
            }
        }

        public string AgentID
        {
            get
            {
                return Request.QueryString["u"] == null
                    ? string.Empty
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["u"].Trim());
            }
        }

        public string AgentNum
        {
            get
            {
                return Request.QueryString["n"] == null
                    ? string.Empty
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["n"].Trim());
            }
        }

        public string StartTime
        {
            get
            {
                return Request.QueryString["t"] == null
                    ? string.Empty
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["t"]);
            }
        }

        public string State
        {
            get
            {
                return Request.QueryString["s"] == null
                    ? "-1"
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["s"]);
            }
        }

        public string AgentAuxState
        {
            get
            {
                return Request.QueryString["as"] == null
                    ? "-1"
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["as"]);
            }
        }

        #endregion

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount = 0;
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }

        }

        private void BindData()
        {
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }

            Entities.QueryAgentStateDetail query = new Entities.QueryAgentStateDetail() { LoginID = BLL.Util.GetLoginUserID().ToString() };



            if (!string.IsNullOrEmpty(AgentID))
            {
                query.AgentID = AgentID.Trim();
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                query.AgentNum = AgentNum.Trim();
            }

            if (!string.IsNullOrEmpty(StartTime))
            {
                query.StartTime = StartTime;
            }
            if (!string.IsNullOrEmpty(State))
            {
                query.State = State;
            }
            if (!string.IsNullOrEmpty(AgentAuxState))
            {
                query.AgentAuxState = AgentAuxState;
            }

            int count;
            DataTable dt = BitAuto.ISDC.CC2012.BLL.AgentStateDetail.Instance.GetStateDetail(query, PageCommon.Instance.PageIndex, PageSize, BLL.Util.GetLoginUserID(), out count);
            RecordCount = count;
            repeaterList.DataSource = dt;
            repeaterList.DataBind();
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize,
                PageCommon.Instance.PageIndex, 1);

        }
    }
}