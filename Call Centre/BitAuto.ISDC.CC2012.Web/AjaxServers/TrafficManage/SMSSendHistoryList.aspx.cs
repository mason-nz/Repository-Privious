using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class SMSSendHistoryList : PageBase
    {
        #region 参数
        private string BeginTime
        {
            get
            {
                return Request["BeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["BeginTime"].ToString().Trim());
            }
        }

        private string EndTime
        {
            get
            {
                return Request["EndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["EndTime"].ToString().Trim());
            }
        }

        private string AgentGroup
        {
            get
            {
                return Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentGroup"].ToString().Trim());
            }
        }


        /// <summary>
        /// 短信状态
        /// </summary>
        public string SMSStatus
        {
            get
            {
                return Request["SMSStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["SMSStatus"].ToString().Trim());
            }
        }
        //手机号
        private string HandNum
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["HandNum"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["HandNum"].ToString()); }
        }
        //发送内容
        private string SendContent
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["SendContent"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SendContent"].ToString()); }
        }
        //接收人
        private string Reservicer
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["Reservicer"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Reservicer"].ToString()); }
        }
        //坐席
        private int RequestAgentUserID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentUserID"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["AgentUserID"])); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

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
            Entities.QuerySMSSendHistory query = new Entities.QuerySMSSendHistory();
            int _loginID = -2;
            _loginID = userID;
            query.LoginID = userID;
            if (!string.IsNullOrEmpty(BeginTime) && !string.IsNullOrEmpty(EndTime))
            {
                DateTime _begin;
                DateTime _end;
                if (DateTime.TryParse(BeginTime + " 00:00:00", out _begin))
                {
                    query.CreateTimeBegin = _begin;
                }
                if (DateTime.TryParse(EndTime + " 23:59:59", out _end))
                {
                    query.CreateTimeEnd = _end;
                }
            }
            if (!string.IsNullOrEmpty(AgentGroup) && AgentGroup != "-2")
            {
                int _bgid;
                if (int.TryParse(AgentGroup, out _bgid))
                {
                    query.BGID = _bgid;
                }
            }
            if (!string.IsNullOrEmpty(HandNum))
            {
                query.Phone = HandNum;
            }
            if (!string.IsNullOrEmpty(Reservicer))
            {
                query.Reservicer = Reservicer;
            }
            if (!string.IsNullOrEmpty(SendContent))
            {
                query.Content = SendContent;
            }
            if (RequestAgentUserID != -2)
            {
                query.CreateUserID = RequestAgentUserID;
            }
            if (!string.IsNullOrEmpty(SMSStatus) && SMSStatus != "-2")
            {
                int _smsstatus;
                if (int.TryParse(SMSStatus, out _smsstatus))
                {
                    query.Status = _smsstatus;
                }
            }
            DataTable dt = BLL.SMSSendHistory.Instance.GetSMSSendHistory(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        public string getCrmUrl(object CRMCustID, object custid)
        {
            if (CRMCustID == null || CRMCustID.ToString() == "")
            {
                return "&nbsp;";             
            }
            else
            {
                return "<a href='../../CustCheck/CrmCustSearch/CustDetail.aspx?CustID= " + CRMCustID + "'  target='_blank'>" + CRMCustID + "</a>&nbsp;";
            }
        }
    }
}