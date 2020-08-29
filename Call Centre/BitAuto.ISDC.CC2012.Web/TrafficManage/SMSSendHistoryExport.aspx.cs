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
    public partial class SMSSendHistoryExport : PageBase
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

        private string RequestBrowser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }
        #endregion
        public int RecordCount;
        public int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //增加 短信清单 【导出】操作权限
            userID = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userID, "SYS024MOD40101"))
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
            DataTable dt = BLL.SMSSendHistory.Instance.GetSMSSendHistoryForExport(query, "a.CreateTime DESC", 1, -1, out RecordCount);
            ExportData(dt);
        }

        private void ExportData(DataTable dt)
        {
            if (dt != null)
            {
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() != "custname".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "Phone".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "TaskID".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "CRMCustID".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "Content".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "CreateTime".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "issuccess".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "TrueName".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "department".ToUpper()
                        && dt.Columns[i].ColumnName.ToUpper() != "title".ToUpper()
                        )
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName);
                    }
                    else
                    {
                        #region 修改列名

                        switch (dt.Columns[i].ColumnName)
                        {
                            case "custname": dt.Columns[i].ColumnName = "接收人"; break;
                            case "Phone": dt.Columns[i].ColumnName = "手机"; break;
                            case "TaskID": dt.Columns[i].ColumnName = "任务ID"; break;
                            case "CRMCustID": dt.Columns[i].ColumnName = "客户ID"; break;
                            case "Content": dt.Columns[i].ColumnName = "发送内容"; break;
                            case "CreateTime": dt.Columns[i].ColumnName = "发送时间"; break;
                            case "issuccess": dt.Columns[i].ColumnName = "是否成功"; break;
                            case "TrueName": dt.Columns[i].ColumnName = "客服"; break;
                            case "department": dt.Columns[i].ColumnName = "部门"; break;
                            case "title": dt.Columns[i].ColumnName = "职务"; break;
                        }

                        #endregion
                    }
                }
                //ExcelInOut.CreateEXCEL(dt, "呼出报表" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), RequestBrowser);
                BLL.Util.ExportToCSV("短信清单" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }
    }
}