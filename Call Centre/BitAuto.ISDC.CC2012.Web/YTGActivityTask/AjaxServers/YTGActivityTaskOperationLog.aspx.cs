using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers
{
    public partial class YTGActivityTaskOperationLog : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
 
        private void BindData()
        {
            Entities.YTGActivityTaskLogInfo query = new Entities.YTGActivityTaskLogInfo();

            query.TaskID = RequestTaskID;

            DataTable dt = BLL.YTGActivityTaskLog.Instance.GetYTGActivityTaskOperationLog(query, "CreateTime asc", 1, 10000, out RecordCount);
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
                actionName = BLL.Util.GetEnumOptText(typeof(Entities.YTGActivityOperationStatus), _action);
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
                name = BLL.Util.GetEnumOptText(typeof(Entities.YTGActivityTaskStatus), _status);
            }
            return name;
        }
    }
}