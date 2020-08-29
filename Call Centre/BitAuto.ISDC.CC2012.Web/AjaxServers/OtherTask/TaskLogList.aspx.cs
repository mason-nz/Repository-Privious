using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask
{
    public partial class TaskLogList : PageBase
    {
        public string TaskID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["TaskID"]))
                {
                    return Request["TaskID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定任务处理记录
                BindData();
            }
        }
        /// <summary>
        /// 绑定任务处理记录
        /// </summary>
        private void BindData()
        {
            DataTable table = null;
            if (!string.IsNullOrEmpty(TaskID))
            {
                table = BLL.ProjectTaskLog.Instance.GetProjectTaskLog(TaskID);
            }

            ////设置数据源
            //if (table != null && table.Rows.Count > 0)
            //{
            repeater_Contact.DataSource = table;
            //}
            //绑定列表数据
            repeater_Contact.DataBind();
        }

        //获取状态
        public string getStatus(string status)
        {
            string _statusStr = string.Empty;

            int intval = 0;
            if (int.TryParse(status, out intval))
            {
                //到时候要修改
                _statusStr = BLL.Util.GetEnumOptText(typeof(EnumProjectTaskStatus), intval);
            }
            return _statusStr;
        }
    }
}