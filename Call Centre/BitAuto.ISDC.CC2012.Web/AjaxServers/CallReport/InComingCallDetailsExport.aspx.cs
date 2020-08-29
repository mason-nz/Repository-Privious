using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class InComingCallDetailsExport : PageBase
    {

        #region
        public string AgentID  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_AgentID"); }
        }
        public string AgentNum
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_AgentNum"); }
        }
        public string StartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_StartTime"); }
        }
        public string EndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_EndTime"); }
        }
        public string BusinessType
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_BusinessType"); }
        }
        public string QueryArea
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_QueryArea"); }
        }
        public string QueryType
        {
            get { return BLL.Util.GetCurrentRequestStr("ep_QueryType"); }
        }
        #endregion

        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
                {
                    Response.Write("<script type='text/javascript'> alert('统计日期不能为空!');</script>");
                    Response.End();
                }
                //增加 呼入报表 【导出】操作权限
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT4061"))
                {
                    ExportData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }

        private void ExportData()
        {          

            QueryCallRecordInfo query = new QueryCallRecordInfo();
            query.BeginTime = Convert.ToDateTime(this.StartTime);
            query.EndTime = Convert.ToDateTime(this.EndTime);
            query.QueryType = CommonFunction.ObjectToInteger(this.QueryType);
            query.LoginID = BLL.Util.GetLoginUserID();
            query.selBusinessType = this.BusinessType;
            query.AgentNum = this.AgentNum;//agentnum
            query.Agent = this.AgentID;//userid

            DataTable dt = null;
            SqlConnection conn = null;
            DataTable dtSum = null;
            try
            {
                conn = CallRecordReport.Instance.CreateSqlConnection();
                string msg = "";
                string searchTableName = "Report_CallRecord_Day";
                string searchAgentTableName = "Report_AgentState_Day";
                if (DateTime.Now.Date == query.BeginTime.Value.Date && DateTime.Now.Date == query.EndTime.Value.Date)
                {
                    Dictionary<ReportTempType, string> dic1 = CallRecordReport.Instance.CreateReportCallRecordStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Day, out msg);
                    searchTableName = dic1[ReportTempType.Day];

                    Dictionary<ReportTempType, string> dic2 = CallRecordReport.Instance.CreateReportAgentStateStatForDayTmp(conn, query.BeginTime.Value, ReportTempType.Day, out msg);
                    searchAgentTableName = dic2[ReportTempType.Day];
                }
                dt = CallRecordReport.Instance.GetCallInReportData(query, conn, 1, -1, searchTableName, searchAgentTableName, out RecordCount);
                dtSum = CallRecordReport.Instance.GetCallInReportDataTotal(query, conn, searchTableName, searchAgentTableName);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CallRecordReport.Instance.CloseSqlConnection(conn);
            }

            if (dt != null)
            {

                if (dtSum != null && dtSum.Rows.Count > 0)
                {

                    DataRow dr = dt.NewRow();
                    dr["StartTime"] = "合计（共" + RecordCount + "项）";
                    dr["TrueName"] = "--";
                    dr["AgentNum"] = "--";
                    dr["N_CallIsQuantity"] = dtSum.Rows[0]["N_CallIsQuantity"];
                    dr["T_RingingTime"] = dtSum.Rows[0]["T_RingingTime"];
                    dr["T_TalkTime"] = dtSum.Rows[0]["T_TalkTime"];
                    dr["T_AfterworkTime"] = dtSum.Rows[0]["T_AfterworkTime"];
                    dr["T_SetLogin"] = dtSum.Rows[0]["T_SetLogin"];
                    dr["P_WorkTimeUse"] = dtSum.Rows[0]["P_WorkTimeUse"];
                    dr["A_AverageRingTime"] = dtSum.Rows[0]["A_AverageRingTime"];
                    dr["A_AverageTalkTime"] = dtSum.Rows[0]["A_AverageTalkTime"];
                    dr["A_AfterworkTime"] = dtSum.Rows[0]["A_AfterworkTime"];
                    dr["T_SetBuzy"] = dtSum.Rows[0]["T_SetBuzy"];
                    dr["N_SetBuzy"] = dtSum.Rows[0]["N_SetBuzy"];
                    dr["A_AverageSetBusy"] = dtSum.Rows[0]["A_AverageSetBusy"];
                    dr["N_TransferOut"] = dtSum.Rows[0]["N_TransferOut"];
                    dr["N_TransferIn"] = dtSum.Rows[0]["N_TransferIn"];

                    dt.Rows.Add(dr);
                }
                dt.Columns.Remove("rn");
                dt.Columns.Remove("UserID");
                dt.Columns["StartTime"].SetOrdinal(0);
                dt.Columns["TrueName"].SetOrdinal(1);
                dt.Columns["AgentNum"].SetOrdinal(2);
                dt.Columns["N_CallIsQuantity"].SetOrdinal(3);
                dt.Columns["T_RingingTime"].SetOrdinal(4);
                dt.Columns["T_TalkTime"].SetOrdinal(5);
                dt.Columns["T_AfterworkTime"].SetOrdinal(6);
                dt.Columns["T_SetLogin"].SetOrdinal(7);
                dt.Columns["P_WorkTimeUse"].SetOrdinal(8);
                dt.Columns["A_AverageRingTime"].SetOrdinal(9);
                dt.Columns["A_AverageTalkTime"].SetOrdinal(10);
                dt.Columns["A_AfterworkTime"].SetOrdinal(11);
                dt.Columns["T_SetBuzy"].SetOrdinal(12);
                dt.Columns["N_SetBuzy"].SetOrdinal(13);
                dt.Columns["A_AverageSetBusy"].SetOrdinal(14);
                dt.Columns["N_TransferOut"].SetOrdinal(15);
                dt.Columns["N_TransferIn"].SetOrdinal(16);

                dt.Columns["StartTime"].ColumnName = "日期";
                dt.Columns["TrueName"].ColumnName = "客服";
                dt.Columns["AgentNum"].ColumnName = "工号";
                dt.Columns["N_CallIsQuantity"].ColumnName = "电话总接通量";
                dt.Columns["T_RingingTime"].ColumnName = "总振铃时长";
                dt.Columns["T_TalkTime"].ColumnName = "总通话时长";
                dt.Columns["T_AfterworkTime"].ColumnName = "总话后时长";
                dt.Columns["T_SetLogin"].ColumnName = "总在线时长";
                dt.Columns["P_WorkTimeUse"].ColumnName = "工时利用率";
                dt.Columns["A_AverageRingTime"].ColumnName = "平均振铃时长(秒)";
                dt.Columns["A_AverageTalkTime"].ColumnName = "平均通话时长(秒)";
                dt.Columns["A_AfterworkTime"].ColumnName = "平均话后时长(秒)";
                dt.Columns["T_SetBuzy"].ColumnName = "置忙总时长";
                dt.Columns["N_SetBuzy"].ColumnName = "置忙次数";
                dt.Columns["A_AverageSetBusy"].ColumnName = "平均置忙时长(秒)";
                dt.Columns["N_TransferOut"].ColumnName = "电话转出次数";
                dt.Columns["N_TransferIn"].ColumnName = "电话转入次数";




                BLL.Util.ExportToCSV("呼入报表" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }
    }
}