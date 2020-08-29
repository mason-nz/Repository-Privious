using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using BitAuto.ISDC.CC2012.Entities;
using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    /// <summary>
    /// InComingCallDetails1 的摘要说明
    /// </summary>
    public class InComingCallDetails1 : IHttpHandler, IRequiresSessionState
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

        bool success = true;
        string result = "";

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string message = "";
            if ((context.Request["Action"] + "").Trim() == "InComingCallTotal")
            {
                string newAgentIDS = "";
                #region   根据AgentNum找到对应的AgentIds
                if (string.IsNullOrEmpty(this.AgentID))
                {
                    if (!string.IsNullOrEmpty(this.AgentNum))
                    {
                        DataTable dtModel = BLL.EmployeeAgent.Instance.GetEmployeeAgentsByAgentNum(this.AgentNum);
                        string agentIDFromAgentNum = "";
                        for (int i = 0; i < dtModel.Rows.Count; i++)
                        {
                            agentIDFromAgentNum += "," + dtModel.Rows[i]["UserID"].ToString();
                        }

                        if (!string.IsNullOrEmpty(agentIDFromAgentNum))
                        {
                            agentIDFromAgentNum = agentIDFromAgentNum.Substring(1, agentIDFromAgentNum.Length - 1);
                        }

                        if (!string.IsNullOrEmpty(agentIDFromAgentNum))
                        {
                            newAgentIDS = agentIDFromAgentNum;
                        }
                        else
                        {
                            newAgentIDS = "-100";
                        }
                    }
                }
                else
                {
                    newAgentIDS = this.AgentID;
                }

                #endregion

                QueryCallRecordInfo query = new QueryCallRecordInfo();
                query.BeginTime = Convert.ToDateTime(this.StartTime);
                query.EndTime = Convert.ToDateTime(this.EndTime);
                query.QueryType = CommonFunction.ObjectToInteger(this.QueryType);
                query.LoginID = BLL.Util.GetLoginUserID();
                query.selBusinessType = this.BusinessType;
                query.AgentNum = this.AgentNum;//agentnum
                query.Agent = this.AgentID;//userid

                // string tableEndName = BLL.Util.CalcTableNameByMonth(3, CommonFunction.ObjectToDateTime(query.StartTime));
                DataTable dt = null;
                SqlConnection conn = null;
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
                    dt = CallRecordReport.Instance.GetCallInReportDataTotal(query, conn, searchTableName, searchAgentTableName);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    CallRecordReport.Instance.CloseSqlConnection(conn);
                }

                JsonData jsondata = new JsonData();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    jsondata.N_CallIsQuantity = "";
                    jsondata.T_RingingTime = "";
                    jsondata.T_TalkTime = "";
                    jsondata.T_AfterworkTime = "";
                    jsondata.T_SetLogin = "";
                    jsondata.P_WorkTimeUse = "";
                    jsondata.A_AverageRingTime = "";
                    jsondata.A_AverageTalkTime = "";
                    jsondata.A_AfterworkTime = "";
                    jsondata.T_SetBuzy = "";
                    jsondata.N_SetBuzy = "";
                    jsondata.A_AverageSetBusy = "";
                    jsondata.N_TransferIn = "";
                    jsondata.N_TransferOut = "";
                }
                else
                {
                    jsondata.N_CallIsQuantity = dt.Rows[0]["N_CallIsQuantity"].ToString();
                    jsondata.T_RingingTime = dt.Rows[0]["T_RingingTime"].ToString();
                    jsondata.T_TalkTime = dt.Rows[0]["T_TalkTime"].ToString();
                    jsondata.T_AfterworkTime = dt.Rows[0]["T_AfterworkTime"].ToString();
                    jsondata.T_SetLogin = dt.Rows[0]["T_SetLogin"].ToString();
                    jsondata.P_WorkTimeUse = dt.Rows[0]["P_WorkTimeUse"].ToString();
                    jsondata.A_AverageRingTime = dt.Rows[0]["A_AverageRingTime"].ToString();
                    jsondata.A_AverageTalkTime = dt.Rows[0]["A_AverageTalkTime"].ToString();
                    jsondata.A_AfterworkTime = dt.Rows[0]["A_AfterworkTime"].ToString();
                    jsondata.T_SetBuzy = dt.Rows[0]["T_SetBuzy"].ToString();
                    jsondata.N_SetBuzy = dt.Rows[0]["N_SetBuzy"].ToString();
                    jsondata.A_AverageSetBusy = dt.Rows[0]["A_AverageSetBusy"].ToString();
                    jsondata.N_TransferIn = dt.Rows[0]["N_TransferIn"].ToString();
                    jsondata.N_TransferOut = dt.Rows[0]["N_TransferOut"].ToString();
                }

                message = Newtonsoft.Json.JavaScriptConvert.SerializeObject(jsondata);
                context.Response.Write(message);
                context.Response.End();
            }
            else
            {
                success = false;
                message = "request error";
                BitAuto.ISDC.CC2012.BLL.AJAXHelper.WrapJsonResponse(success, result, message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class JsonData
        {
            public string N_CallIsQuantity { get; set; }
            public string T_RingingTime { get; set; }
            public string T_TalkTime { get; set; }
            public string T_AfterworkTime { get; set; }
            public string T_SetLogin { get; set; }
            public string P_WorkTimeUse { get; set; }
            public string A_AverageRingTime { get; set; }
            public string A_AverageTalkTime { get; set; }
            public string A_AfterworkTime { get; set; }
            public string T_SetBuzy { get; set; }
            public string N_SetBuzy { get; set; }
            public string A_AverageSetBusy { get; set; }
            public string N_TransferIn { get; set; }
            public string N_TransferOut { get; set; }
        }
    }
}