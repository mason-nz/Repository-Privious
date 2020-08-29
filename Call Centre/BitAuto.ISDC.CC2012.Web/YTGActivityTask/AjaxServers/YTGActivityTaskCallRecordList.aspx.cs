using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers
{
    public partial class YTGActivityTaskCallRecordList : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
                string tableEndName = "";//只查询现表数据
                DataTable table = BLL.CallRecordInfo.Instance.GetCallRecordByTaskID(TaskID, tableEndName);
                repeater_Contact.DataSource = table;
                //绑定列表数据
                repeater_Contact.DataBind();
            }
        }
    }
}