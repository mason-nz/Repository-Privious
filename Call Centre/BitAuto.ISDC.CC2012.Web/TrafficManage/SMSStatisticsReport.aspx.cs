using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class SMSStatisticsReport : PageBase
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
                //增加 短信统计 【导出】操作权限
                userID = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userID, "SYS024MOD40111"))
                {
                    BindData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
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

            DataTable dt = BLL.SMSSendHistory.Instance.GetSMSHistroyStatistics(query, "", 1, 1000000, out RecordCount);
            if (dt != null)
            {
                dt.Columns.Add("starttime", typeof(string));
                dt.Columns.Add("endtime", typeof(string));
                int sendZ = 0;
                int sendSuccess = 0;
                int sendFail = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["starttime"] = BeginTime;
                        dt.Rows[i]["endtime"] = EndTime;
                        int _sendz = 0;
                        if (int.TryParse(dt.Rows[i]["znum"].ToString(), out _sendz))
                        {
                            sendZ += _sendz;
                        }
                        int _sendsuccess = 0;
                        if (int.TryParse(dt.Rows[i]["successnum"].ToString(), out _sendsuccess))
                        {
                            sendSuccess += _sendsuccess;
                        }
                        int _sendfail = 0;
                        if (int.TryParse(dt.Rows[i]["failnum"].ToString(), out _sendfail))
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
                row["starttime"] = BeginTime;
                row["endtime"] = EndTime;
                dt.Rows.Add(row);

            }
            ExportData(dt);
        }
        private void ExportData(DataTable dt)
        {
            if (dt != null)
            {
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() != "truename".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "agentnum".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "znum".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "successnum".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "failnum".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "starttime".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "endtime".ToUpper()
                        )
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName);
                    }
                    else
                    {
                        #region 修改列名

                        switch (dt.Columns[i].ColumnName)
                        {
                            case "truename": dt.Columns[i].ColumnName = "客服"; break;
                            case "agentnum": dt.Columns[i].ColumnName = "工号"; break;
                            case "znum": dt.Columns[i].ColumnName = "短信发送量"; break;
                            case "successnum": dt.Columns[i].ColumnName = "成功量"; break;
                            case "failnum": dt.Columns[i].ColumnName = "失败量"; break;
                            case "starttime": dt.Columns[i].ColumnName = "统计开始时间"; break;
                            case "endtime": dt.Columns[i].ColumnName = "统计结束时间"; break;
                        }

                        #endregion
                    }
                }
                BLL.Util.ExportToCSV("短信统计" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }
    }
}