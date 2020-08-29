using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class InComingCallDetails : PageBase
    {
        #region
        public string AgentID  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentID"); }
        }
        public string AgentNum
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentNum"); }
        }
        public string StartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("StartTime"); }
        }
        public string EndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndTime"); }
        }
        public string BusinessType
        {
            get { return BLL.Util.GetCurrentRequestStr("BusinessType"); }
        }
        public string QueryArea
        {
            get { return BLL.Util.GetCurrentRequestStr("QueryArea"); }
        }
        public string QueryType
        {
            get { return BLL.Util.GetCurrentRequestStr("QueryType"); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int RecordCount;
        public int userID = 0;
        public int GroupLength = 8;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindInComingCallData();
            }
        }

        private void BindInComingCallData()
        {
            QueryCallRecordInfo query = new QueryCallRecordInfo();
            query.BeginTime = Convert.ToDateTime(this.StartTime);
            query.EndTime = Convert.ToDateTime(this.EndTime);
            query.QueryType = CommonFunction.ObjectToInteger(this.QueryType);
            query.LoginID = BLL.Util.GetLoginUserID();
            query.selBusinessType = this.BusinessType;
            query.AgentNum = this.AgentNum;//agentnum
            query.Agent = this.AgentID;//userid

            //  string tableEndName = BLL.Util.CalcTableNameByMonth(3, CommonFunction.ObjectToDateTime(query.StartTime));
            DataTable dts = null;
            SqlConnection conn = null;
     
            try
            {
                string msg = "";
                string searchTableName = "Report_CallRecord_Day";
                string searchAgentTableName = "Report_AgentState_Day";
                conn = CallRecordReport.Instance.CreateSqlConnection();
                if (DateTime.Now.Date == query.BeginTime.Value.Date && DateTime.Now.Date == query.EndTime.Value.Date)
                {
                    Dictionary<ReportTempType, string> dic1 = CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Day, out msg);
                    searchTableName = dic1[ReportTempType.Day];

                    Dictionary<ReportTempType, string> dic2 = CallRecordReport.Instance.CreateReportAgentStateStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Day, out msg);
                    searchAgentTableName = dic2[ReportTempType.Day];
                }
                dts = CallRecordReport.Instance.GetCallInReportData(query, conn, BLL.PageCommon.Instance.PageIndex, PageSize, searchTableName, searchAgentTableName, out RecordCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CallRecordReport.Instance.CloseSqlConnection(conn);
            }
            repeaterList.DataSource = dts;
            repeaterList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}