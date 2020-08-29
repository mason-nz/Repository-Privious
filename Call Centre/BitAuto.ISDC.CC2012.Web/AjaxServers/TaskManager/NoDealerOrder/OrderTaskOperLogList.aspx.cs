using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class OrderTaskOperLogList : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
            QueryOrderTaskOperationLog query = new QueryOrderTaskOperationLog();
            long _taskID;
            if (long.TryParse(RequestTaskID, out _taskID))
            {
                query.TaskID = _taskID;
            }

            DataTable dt = BLL.OrderTaskOperationLog.Instance.GetOrderTaskOperationLogHaveCall(query, "a.CreateTime asc", 1, 10000, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
        }

        //根据CreateUserID得到名称
        public string getEmployName(string uid)
        {
            string name = string.Empty;
            int _uid;
            if (int.TryParse(uid, out _uid))
            {
                name = BLL.Util.GetNameInHRLimitEID(_uid);
            }
            return name;
        }

        //获取任务状态名称
        public string getStatusName(string status)
        {
            string name = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                name = BLL.Util.GetEnumOptText(typeof(Entities.TaskStatus), _status);
            }
            return name;
        }

        //获取动作名称
        public string getActionName(string action, string remark)
        {
            string actionName = string.Empty;
            int _action;
            if (int.TryParse(action, out _action))
            {
                actionName = BLL.Util.GetEnumOptText(typeof(Entities.OperationStatus), _action);
                if ((actionName == "分配" || actionName == "回收") && remark != "" && remark != "-2")
                {
                    actionName += "至" + remark;
                }
            }
            return actionName;
        }

        //根据任务状态如果是-提交，则去OrderTask表查找“理由”和“备注”信息
        public string[] getNoDealReason(string operStatus, string taskID)
        {
            string[] noDealReason = new string[2];

            if (int.Parse(operStatus) == (int)Entities.OperationStatus.Submit)
            {
                int _taskID;
                if (int.TryParse(taskID, out _taskID))
                {
                    Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(_taskID);
                    if (model != null)
                    {
                        string strReason = BLL.Util.GetEnumOptText(typeof(Entities.NoDealerReason), int.Parse(model.NoDealerReasonID.ToString()));
                        if (strReason != "")
                        {
                            noDealReason[0] = "理由：" + strReason + "<br/>";
                            noDealReason[1] = "理由：" + strReason + "；";
                        }
                        if (model.NoDealerReason != null && model.NoDealerReason != "")
                        {
                            string strRemark = model.NoDealerReason;
                            if (strRemark.Length >= 50)
                            {
                                strRemark = strRemark.Substring(0, 50) + "...";
                            }
                            noDealReason[0] += "备注：" + strRemark + "";
                            noDealReason[1] += "备注：" + model.NoDealerReason + "";
                        }
                    }
                }
            }
            return noDealReason;
        }
    }
}