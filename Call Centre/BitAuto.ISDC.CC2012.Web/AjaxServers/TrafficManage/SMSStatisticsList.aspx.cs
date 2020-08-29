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
    public partial class SMSStatisticsList : PageBase
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
        //工号
        private string AgentNum
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentNum"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString()); }
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
            if (!string.IsNullOrEmpty(AgentNum))
            {
                query.AgentNum = AgentNum;
            }

            if (RequestAgentUserID != -2)
            {
                query.CreateUserID = RequestAgentUserID;
            }

            DataTable dt = BLL.SMSSendHistory.Instance.GetSMSHistroyStatistics(query, "", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                int Recordcountz = 0;
                DataTable dtZ = BLL.SMSSendHistory.Instance.GetSMSHistroyStatistics(query, "", 1, 1000000, out Recordcountz);
                int sendZ = 0;
                int sendSuccess = 0;
                int sendFail = 0;
                if (dtZ != null && dtZ.Rows.Count > 0)
                {
                    for (int i = 0; i < dtZ.Rows.Count; i++)
                    {
                        int _sendz = 0;
                        if (int.TryParse(dtZ.Rows[i]["znum"].ToString(), out _sendz))
                        {
                            sendZ += _sendz;
                        }
                        int _sendsuccess = 0;
                        if (int.TryParse(dtZ.Rows[i]["successnum"].ToString(), out _sendsuccess))
                        {
                            sendSuccess += _sendsuccess;
                        }
                        int _sendfail = 0;
                        if (int.TryParse(dtZ.Rows[i]["failnum"].ToString(), out _sendfail))
                        {
                            sendFail += _sendfail;
                        }
                    }
                }
                DataRow row = dt.NewRow();
                row["truename"] = "汇总";
                row["agentnum"] = "--";
                row["znum"] = sendZ;
                row["successnum"] = sendSuccess;
                row["failnum"] = sendFail;
                dt.Rows.Add(row);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}