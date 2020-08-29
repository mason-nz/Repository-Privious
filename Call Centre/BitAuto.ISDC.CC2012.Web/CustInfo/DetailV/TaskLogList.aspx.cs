using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class TaskLogList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private CustInfoHelper ch = new CustInfoHelper();
        private Entities.ProjectTaskInfo task;

        private void BindData()
        {
            task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(ch.TID);
            if (task == null) { return; }
            this.TID = task.PTID.ToString();

            DataTable table = BLL.ProjectTaskLog.Instance.GetProjectTaskLog(task.PTID);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_Contact.DataSource = table;
            }
            //绑定列表数据
            repeater_Contact.DataBind();

            ////分页控件
            //this.AjaxPager_Contact.InitPager(totalCount);
        }

        //获取状态
        public string getStatus(string status)
        {
            string _statusStr = string.Empty;

            int intval = 0;
            if (int.TryParse(status, out intval))
            {
                if (intval == 180003 || intval == 180004 || intval == 180010)
                {
                    _statusStr = "待审核";
                }
                else
                {
                    _statusStr = BLL.Util.GetEnumOptText(typeof(EnumProjectTaskStatus), intval);
                }
            }
            return _statusStr;
        }
    }
}