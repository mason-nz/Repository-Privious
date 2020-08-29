using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class AnswerPhoneExport : PageBase
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

        private string SelSolve
        {
            get
            {
                return Request["selSolve"] == null ? "" :
                HttpUtility.UrlDecode(Request["selSolve"].ToString().Trim());
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
        public string Category
        {
            get
            {
                return Request["selCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["selCategory"].ToString().Trim());
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
        /// 业务线
        /// </summary>
        public string selBusinessType
        {
            get
            {
                return Request["selBusinessType"] == null ? "" :
                HttpUtility.UrlDecode(Request["selBusinessType"].ToString().Trim());
            }
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
            //增加“来电记录--已接来电”导出功能验证逻辑
            userID = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userID, "SYS024BUT4011"))
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
            bool r;
            string info;
            DateTime st = CommonFunction.ObjectToDateTime(BeginTime);
            DateTime et = CommonFunction.ObjectToDateTime(EndTime);
            r = BLL.Util.CheckForSelectCallRecordORIG(st, et, out info);
            if (r == false)
            {
                return;
            }

            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;

            Entities.QueryCallRecordInfo query = BLL.CallRecordInfo.Instance.GetQueryModel(
                Name, ANI, Agent, TaskID, CallID, BeginTime, EndTime, AgentNum, PhoneNum, TaskCategory,
                SpanTime1, SpanTime2, AgentGroup, CallStatus, _loginID, _ownGroup, _oneSelf, Category, IVRScore, IncomingSource, selBusinessType, SelSolve
                );
            string tableEndName = BLL.Util.CalcTableNameByMonth(3, query.BeginTime.Value);
            DataTable dt = BLL.CallRecordInfo.Instance.GetCallRecordInfo(query, "c.CreateTime desc", 1, -1, tableEndName, out RecordCount);

            ExportData(dt);
        }

        private void ExportData(DataTable dt)
        {
            if (dt != null)
            {
                dt.Columns["TaskID"].SetOrdinal(0);
                dt.Columns["AgentNum"].SetOrdinal(1);
                dt.Columns["AgentName"].SetOrdinal(2);
                dt.Columns["ANI"].SetOrdinal(3);
                dt.Columns["PhoneNum"].SetOrdinal(4);
                dt.Columns["BeginTime"].SetOrdinal(5);
                dt.Columns["EndTime"].SetOrdinal(6);
                dt.Columns["TallTime"].SetOrdinal(7);
                dt.Columns["SwitchINNum_Name"].SetOrdinal(8);
                dt.Columns["SkillGroup_Name"].SetOrdinal(9);
                dt.Columns["Score"].SetOrdinal(10);

                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() != "TASKID"
                        && dt.Columns[i].ColumnName.ToUpper() != "AGENTNAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "AGENTNUM"
                        && dt.Columns[i].ColumnName.ToUpper() != "ANI"
                        && dt.Columns[i].ColumnName.ToUpper() != "PHONENUM"
                        && dt.Columns[i].ColumnName.ToUpper() != "BEGINTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "ENDTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "TALLTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "SWITCHINNUM_NAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "SKILLGROUP_NAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "SCORE"
                        )
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName);
                    }
                    else
                    {
                        #region 修改列名

                        switch (dt.Columns[i].ColumnName)
                        {
                            case "TaskID": dt.Columns[i].ColumnName = "任务ID"; break;
                            case "AgentNum": dt.Columns[i].ColumnName = "工号"; break;
                            case "AgentName": dt.Columns[i].ColumnName = "坐席"; break;
                            case "ANI": dt.Columns[i].ColumnName = "主叫号码"; break;
                            case "PhoneNum": dt.Columns[i].ColumnName = "被叫号码"; break;
                            case "BeginTime": dt.Columns[i].ColumnName = "开始时间"; break;
                            case "EndTime": dt.Columns[i].ColumnName = "结束时间"; break;
                            case "TallTime": dt.Columns[i].ColumnName = "通话时长(秒)"; break;
                            case "SwitchINNum_Name": dt.Columns[i].ColumnName = "业务线"; break;
                            case "SkillGroup_Name": dt.Columns[i].ColumnName = "技能组"; break;
                            case "Score": dt.Columns[i].ColumnName = "满意度"; break;
                        }

                        #endregion
                    }
                }
                BLL.Util.ExportToCSV("已接来电" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }
    }
}