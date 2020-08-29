using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager
{
    public partial class TaskProcessDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        #endregion

        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        public string displayshow(string showstr)
        {
            if (string.IsNullOrEmpty(showstr))
            {
                return "display:none";
            }
            else
            {
                return "display:block";
            }

        }


        private void BindData()
        {
            QueryCustHistoryLog query = new QueryCustHistoryLog();
            if (RequestTaskID != "")
            {
                query.TaskID = RequestTaskID;
                DataTable dt = BLL.CustHistoryLog.Instance.GetCustHistoryLogHaveCallRecord(query, "SolveTime asc", 1, 10000, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
        }

        //根据受理人ID得到名称
        public string getEmployName(string eid)
        {
            string name = string.Empty;
            int id;
            if (int.TryParse(eid, out id))
            {
                name = "暂不实现";
            }
            return name;
        }

        //获取状态名称
        public string getStatusName(string status)
        {
            string name = string.Empty;
            switch (status)
            {
                case "110001": name = "已解决";
                    break;
                case "110002": name = "不解决";
                    break;
                case "110003": name = "未解决";
                    break;
            }
            return name;
        }

        //获取动作名称
        public string getActionName(string action, string nextUserEID)
        {
            string actionName = string.Empty;
            switch (int.Parse(action))
            {
                case (int)Entities.Action.ActionApplyTurn: actionName = "申请转出";
                    break;
                case (int)Entities.Action.ActionAgreeApplyTurn: actionName = "同意转出";
                    break;
                case (int)Entities.Action.ActionSumbit: actionName = "提交";
                    break;
                case (int)Entities.Action.ActionTurnOut: actionName = "转出";
                    int eid;
                    if (int.TryParse(nextUserEID, out eid))
                    {
                        //查出转到受理人的姓名
                        string nextName = "暂不实现";
                        actionName = "转出至" + nextName;
                    }
                    break;
                case (int)Entities.Action.ActionTurnOver: actionName = "结束";
                    break;
                case (int)Entities.Action.ActionCallOut: actionName = "呼出";
                    break;
            }
            return actionName;
        }

        public string ShowCallRecord(string action, string taskId, string callRecordID, string audioURL)
        {
            string returnStr = "&nbsp;";
            //如果是申请转出，显示呼入的录音
            if (action == "120001")
            {
                Entities.CallRecordInfo callRecordInfo = BLL.CallRecordInfo.Instance.GetCallRecordInfoByTaskID(taskId);
                if (callRecordInfo != null)
                {
                    returnStr = "<a href=\"" + callRecordInfo.AudioURL + "\">  <img  src=\"/Images/callTel.png\" border=\"0\" /></a>";
                }

            }
            //如果是呼出，显示呼出的录音
            else if (action == "120007" || action == "120003")
            {
                if (!string.IsNullOrEmpty(callRecordID) && callRecordID != "-2")
                {
                    returnStr = "<a href=\"" + audioURL + "\">  <img  src=\"/Images/callTel.png\" border=\"0\" /></a>";
                }
            }

            return returnStr;
        }
    }
}