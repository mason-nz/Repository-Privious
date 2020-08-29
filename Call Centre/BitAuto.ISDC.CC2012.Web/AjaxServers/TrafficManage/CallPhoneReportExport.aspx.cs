using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class CallPhoneReportExport : PageBase
    {
        #region 参数

        private string IVRScore
        {
            get
            {
                return Request["IVRScore"] == null ? "" :
                HttpUtility.UrlDecode(Request["IVRScore"].ToString().Trim());
            }
        }

        private string IncomingSource
        {
            get
            {
                return Request["IncomingSource"] == null ? "" :
                HttpUtility.UrlDecode(Request["IncomingSource"].ToString().Trim());
            }
        }

        private string Name
        {
            get
            {
                return Request["Name"] == null ? "" :
                HttpUtility.UrlDecode(Request["Name"].ToString().Trim());
            }
        }

        private string ANI
        {
            get
            {
                return Request["ANI"] == null ? "" :
                HttpUtility.UrlDecode(Request["ANI"].ToString().Trim());
            }
        }

        private string Agent
        {
            get
            {
                return Request["Agent"] == null ? "" :
                HttpUtility.UrlDecode(Request["Agent"].ToString().Trim());
            }
        }

        private string TaskID
        {
            get
            {
                return Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskID"].ToString().Trim());
            }
        }

        private string CallID
        {
            get
            {
                return Request["CallID"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallID"].ToString().Trim());
            }
        }

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

        private string AgentNum
        {
            get
            {
                return Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentNum"].ToString().Trim());
            }
        }
        private string PhoneNum
        {
            get
            {
                return Request["PhoneNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["PhoneNum"].ToString().Trim());
            }
        }

        private string TaskCategory
        {
            get
            {
                return Request["TaskCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskCategory"].ToString().Trim());
            }
        }

        public string Category
        {
            get
            {
                return Request["selCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["selCategory"].ToString().Trim());
            }
        }

        private string SpanTime1
        {
            get
            {
                return Request["SpanTime1"] == null ? "" :
                HttpUtility.UrlDecode(Request["SpanTime1"].ToString().Trim());
            }
        }

        private string SpanTime2
        {
            get
            {
                return Request["SpanTime2"] == null ? "" :
                HttpUtility.UrlDecode(Request["SpanTime2"].ToString().Trim());
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
        private string CallStatus
        {
            get
            {
                return Request["CallStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallStatus"].ToString().Trim());
            }
        }

        /// <summary>
        /// 查询类型：1月，2周，3日
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
        private string RequestBrowser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }
        public int RecordCount;
        public int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EndTime==""||BeginTime=="")
            {
                Response.Write("<script type='text/javascript'> alert('统计日期不能为空!');</script>");
                Response.End();
            }
            userID = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userID, "SYS024MOD400501"))
            {
                BindData();
            }
            else
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }

        }

        private void BindData()
        {           

            Entities.QueryCallRecordInfo query = BLL.CallRecordInfo.Instance.GetQueryModel(
                "", "", "", "", "", BeginTime, EndTime, "", "", "",
                "", "", AgentGroup, CallStatus, userID, "", "", "", "", "", "", ""
                );

            query.QueryType = RequestQueryType;
            query.CreateUserID = RequestAgentUserID;
            query.AgentNum = RequestAgentNum;
            if (RequestAgentUserID > 0)
            {
                query.Agent = this.RequestAgentUserID.ToString();//userid
            }
            DataTable dts = null;
            SqlConnection conn = null;
            DataTable dt2 = null;
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

                dts = CallRecordReport.Instance.GetCallOutReportData(query, conn, searchTableName, searchAgentTableName, 1, -1, out RecordCount);
                dt2 = CallRecordReport.Instance.GetCallOutDataReportTotal(query, conn, searchTableName, searchAgentTableName);

                dts.Columns.Remove("RN");
                dts.Columns.Remove("UserID");

               
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                CallRecordReport.Instance.CloseSqlConnection(conn);
            }


            object[] objArray = new object[dts.Columns.Count];
            objArray[0] = "合计共(" + RecordCount + ")项";
            objArray[1] = "--";
            objArray[2] = "--";
            // objArray[3] = 0;
            for (int j = 0; j < dt2.Columns.Count; j++)
            {
                objArray[j + 3] = dt2.Rows[0][j];
            }
            dts.Rows.Add(objArray);
            if (RequestQueryType == 4)
            {
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    dts.Rows[i]["p_gongshilv"] = "--";
                    dts.Rows[i]["T_SignIn"] = "--";
                }
            }
            ExportData(dts);
        }

        private void ExportData(DataTable dt)
        {
            if (dt != null)
            {
               
                //dt.Columns.Remove("BGID");
                //dt.Columns.Remove("pingFenCount");
                //dt.Columns.Remove("heGeCount");
                //dt.Columns.Remove("t_Score");

                dt.Columns["StartTime"].SetOrdinal(0);
                dt.Columns["TrueName"].SetOrdinal(1);
                dt.Columns["AgentNum"].SetOrdinal(2);
                dt.Columns["N_Total"].SetOrdinal(3);
                dt.Columns["N_ETotal"].SetOrdinal(4);
                dt.Columns["p_jietonglv"].SetOrdinal(5);
                dt.Columns["T_Talk"].SetOrdinal(6);
                dt.Columns["T_Ringing"].SetOrdinal(7);
                dt.Columns["T_SignIn"].SetOrdinal(8);
                dt.Columns["p_gongshilv"].SetOrdinal(9);
                dt.Columns["t_pjtime"].SetOrdinal(10);
                dt.Columns["t_pjring"].SetOrdinal(11);

                dt.Columns["StartTime"].ColumnName = "日期";
                dt.Columns["TrueName"].ColumnName = "客服";
                dt.Columns["AgentNum"].ColumnName = "工号";
                dt.Columns["N_Total"].ColumnName = "外呼电话总量";
                dt.Columns["N_ETotal"].ColumnName = "外呼接通量";
                dt.Columns["p_jietonglv"].ColumnName = "外呼接通率";
                dt.Columns["T_Talk"].ColumnName = "总通话时长";
                dt.Columns["T_Ringing"].ColumnName = "总振铃时长";
                dt.Columns["T_SignIn"].ColumnName = "总在线时长";
                 dt.Columns["p_gongshilv"].ColumnName = "工时利用率";
                dt.Columns["t_pjtime"].ColumnName = "平均通话时长(秒)";            
                dt.Columns["t_pjring"].ColumnName = "平均振铃时长(秒";

               

                //ExcelInOut.CreateEXCEL(dt, "呼出报表" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), RequestBrowser);
                BLL.Util.ExportToCSV("呼出报表" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }
    }
}