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
    public partial class LeadTaskCallRecordList : PageBase
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
                BindData();
            }
        }
        private void BindData()
        {
            if (!string.IsNullOrEmpty(TaskID))
            {
                string tableEndName = "";
                //只查询现表数据
                DataTable table = BLL.CallRecordInfo.Instance.GetCallRecordByTaskID(TaskID, tableEndName);
                repeater_Contact.DataSource = table;
                repeater_Contact.DataBind();
            }
        }
    }
}