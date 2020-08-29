using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class AgentTimeStateList : PageBase
    {
        #region 参数      
        private int RequestBGID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["BGID"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"])); }
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
            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;

            Entities.QueryAgentState query = new QueryAgentState();
            query.LoginID = _loginID;
            query.BGID = RequestBGID;

            int totalCount = 0;
            DataTable dt = BLL.AgentTimeState.Instance.GetAllStateStatisticsFromDB(query, " Name asc ", 1, 99999, out totalCount);//BLL.CallRecordInfo.Instance.GetCallPhoneReport(query, "StatisticsTime DESC,AgentName ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }            
        }

        public string GetViewUrl(string TaskTypeID, string TaskID, string carType, string source)
        {
            string url = "";

            url = BLL.CallRecordInfo.Instance.GetViewUrl(TaskTypeID, TaskID, carType, source);

            return url;
        }
    }
}