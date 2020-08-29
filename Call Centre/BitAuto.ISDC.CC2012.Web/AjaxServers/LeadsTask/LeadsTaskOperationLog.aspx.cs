using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.LeadsTask
{
    public partial class LeadsTaskOperationLog : PageBase
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
            Entities.QueryLeadsTaskOperationLog query = new Entities.QueryLeadsTaskOperationLog();

            query.TaskID = RequestTaskID;

            DataTable dt = BLL.LeadsTaskOperationLog.Instance.GetLeadsTaskOperationLog(query, "CreateTime asc", 1, 10000, out RecordCount);
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
        //获取动作名称
        public string getActionName(string action)
        {
            string actionName = string.Empty;
            int _action;
            if (int.TryParse(action, out _action))
            {
                actionName = BLL.Util.GetEnumOptText(typeof(Entities.Leads_OperationStatus), _action);
                //if ((actionName == "分配" || actionName == "回收") && remark != "" && remark != "-2")
                //{
                //    actionName += "至" + remark;
                //}
            }
            return actionName;
        }
        //获取任务状态名称
        public string getStatusName(string status)
        {
            string name = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                name = BLL.Util.GetEnumOptText(typeof(Entities.LeadsTaskStatus), _status);
            }
            return name;
        }
    }
}