using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    /// <summary>
    /// CallPhoneReport 的摘要说明
    /// </summary>
    public class CallPhoneReport : IHttpHandler, IRequiresSessionState
    {
        #region 参数
        private string BeginTime
        {
            get
            {
                return HttpContext.Current.Request["BeginTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["BeginTime"].ToString().Trim());
            }
        }

        private string EndTime
        {
            get
            {
                return HttpContext.Current.Request["EndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["EndTime"].ToString().Trim());
            }
        }

        private string AgentGroup
        {
            get
            {
                return HttpContext.Current.Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AgentGroup"].ToString().Trim());
            }
        }


        /// <summary>
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        public string CallStatus
        {
            get
            {
                return HttpContext.Current.Request["CallStatus"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CallStatus"].ToString().Trim());
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
        bool success = true;
        string result = "";
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string message = "";
          if ((context.Request["Action"] + "").Trim() == "CallPhoneReportTotalNew")
            {
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

                int _loginID = BLL.Util.GetLoginUserID();

                Entities.QueryCallRecordInfo query = BLL.CallRecordInfo.Instance.GetQueryModel(
                    "", "", "", "", "", sBeginTime, sEndTime, "", "", "",
                    "", "", AgentGroup, CallStatus, _loginID, "", "", "", "", "", "", ""
                    );

                query.QueryType = RequestQueryType;
                query.CreateUserID = RequestAgentUserID;
                query.AgentNum = RequestAgentNum;
                if (RequestAgentUserID > 0)
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

                    dt = CallRecordReport.Instance.GetCallOutDataReportTotal(query, conn, searchTableName, searchAgentTableName);
                    if (query.QueryType == 4)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["p_gongshilv"] = "--";
                            dt.Rows[i]["T_SignIn"] = "--";
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    CallRecordReport.Instance.CloseSqlConnection(conn);
                }

                JsonDataNew jsondata = new JsonDataNew();
                if (string.IsNullOrEmpty(dt.Rows[0]["N_Total"].ToString()))
                {
                    jsondata.N_Total = 0;
                    jsondata.N_ETotal = 0;
                    jsondata.p_gongshilv = "0.00%";
                    jsondata.T_Talk = "0.00";
                    jsondata.T_SignIn = "--";
                    jsondata.T_Ringing = "0.00";
                    jsondata.t_pjtime = "0.00";
                    jsondata.t_pjring = "0.00";
                    jsondata.p_jietonglv = "--";
                }
                else
                {
                    jsondata.p_gongshilv = dt.Rows[0]["p_gongshilv"].ToString();
                    jsondata.T_Talk = dt.Rows[0]["T_Talk"].ToString();
                    jsondata.T_SignIn = dt.Rows[0]["T_SignIn"].ToString();
                    jsondata.T_Ringing = dt.Rows[0]["T_Ringing"].ToString();
                    jsondata.N_Total = Convert.ToInt32(dt.Rows[0]["N_Total"]);
                    jsondata.t_pjtime = dt.Rows[0]["t_pjtime"].ToString();
                    jsondata.N_ETotal = Convert.ToInt32(dt.Rows[0]["N_ETotal"]);
                    jsondata.t_pjring = dt.Rows[0]["t_pjring"].ToString();
                    jsondata.p_jietonglv = dt.Rows[0]["p_jietonglv"].ToString();
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
    }

    public class JsonDataNew
    {
        public int N_Total { get; set; }
        public int N_ETotal { get; set; }
        public string p_jietonglv { get; set; }
        public string T_Talk { get; set; }
        public string T_Ringing { get; set; }
        public string T_SignIn { get; set; }
        public string p_gongshilv { get; set; }
        public string t_pjtime { get; set; }
        public string t_pjring { get; set; }
    }
}