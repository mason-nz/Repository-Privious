using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class CallPhoneReportList : PageBase
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
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        public string CallStatus
        {
            get
            {
                return Request["CallStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallStatus"].ToString().Trim());
            }
        }
        /// <summary>
        /// 查询类型：1月，2周，3日 4小时
        /// </summary>
        private int RequestQueryType
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["QueryType"]) ? 1 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["QueryType"])); }
        }
        private string RequestAgentNum
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentNum"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString()); }
        }
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
            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;

            //默认显示上周一周话务数据
            //上周一时间=today-(6+today.dayofweek)
            string sBeginTime = "", sEndTime = "";
            if (string.IsNullOrEmpty(BeginTime) && string.IsNullOrEmpty(EndTime))
            {
                DateTime LastMonday = DateTime.Now.AddDays(-(6 + Convert.ToDouble(DateTime.Now.DayOfWeek)));
                DateTime LastSunday = LastMonday.AddDays(6);

                sBeginTime = LastMonday.ToShortDateString();
                sEndTime = LastSunday.ToShortDateString();
            }
            else
            {
                sBeginTime = BeginTime;
                sEndTime = EndTime;
            }

            Entities.QueryCallRecordInfo query = BLL.CallRecordInfo.Instance.GetQueryModel(
                "", "", "", "", "", sBeginTime, sEndTime, "", "", "",
                "", "", AgentGroup, CallStatus, _loginID, _ownGroup, _oneSelf, "", "", "", "", ""
                );

            query.QueryType = RequestQueryType;
            query.CreateUserID = RequestAgentUserID;
            query.AgentNum = RequestAgentNum;
            if (RequestAgentUserID>0)
            {
                query.Agent = this.RequestAgentUserID.ToString();//userid
            }
         
            DataTable dt = null;
            SqlConnection conn = null;
            try
            {
                conn = CallRecordReport.Instance.CreateSqlConnection();
                string msg = "";
                string searchTableName = "Report_CallRecord_Hour";
                string searchAgentTableName = "Report_AgentState_Hour";
                if (query.QueryType == 4 && DateTime.Now.Date == Convert.ToDateTime(BeginTime).Date)
                {
                    Dictionary<ReportTempType, string> dic1 = CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Hour, out msg);
                    searchTableName = dic1[ReportTempType.Hour];

                    Dictionary<ReportTempType, string> dic2 = CallRecordReport.Instance.CreateReportAgentStateStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Hour, out msg);
                    searchAgentTableName = dic2[ReportTempType.Hour];
                }
                else if (query.QueryType != 4 && DateTime.Now.Date == Convert.ToDateTime(BeginTime).Date && DateTime.Now.Date == Convert.ToDateTime(EndTime).Date)
                {
                    Dictionary<ReportTempType, string> dic1 = CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Day, out msg);
                    searchTableName = dic1[ReportTempType.Day];

                    Dictionary<ReportTempType, string> dic2 = CallRecordReport.Instance.CreateReportAgentStateStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Day, out msg);
                    searchAgentTableName = dic2[ReportTempType.Day];
                }
                else if (query.QueryType == 4)
                {
                    searchTableName = "Report_CallRecord_Hour";
                    searchAgentTableName = "Report_AgentState_Hour";
                }
                else
                {
                    searchTableName = "Report_CallRecord_Day";
                    searchAgentTableName = "Report_AgentState_Day";
                }
                dt = CallRecordReport.Instance.GetCallOutReportData(query, conn, searchTableName, searchAgentTableName, BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CallRecordReport.Instance.CloseSqlConnection(conn);
            }

            if (query.QueryType == 4)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["p_gongshilv"] = "--";
                    dt.Rows[i]["T_SignIn"] = "--";
                }
            }
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        public string GetViewUrl(string TaskTypeID, string TaskID, string carType, string source)
        {
            return BLL.CallRecordInfo.Instance.GetViewUrl(TaskTypeID, TaskID, carType, source);
        }
    }
}