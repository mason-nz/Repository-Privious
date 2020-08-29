using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class HistoryList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null
                           ? ""
                           : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"]);
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
            if (string.IsNullOrEmpty(TaskID)) { return; }
            else
            {
                DataTable dt = BLL.OrderCRMStopCustTaskOperationLog.Instance.GetListByTaskID(TaskID);
                //设置数据源
                if (dt != null && dt.Rows.Count > 0)
                {
                    repeater_Contact.DataSource = dt;
                    repeater_Contact.DataBind();
                }
            }
        }

        public string GetType(string type)
        {
            string result = string.Empty;
            int _type;
            if (int.TryParse(type, out _type))
            {
                result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustTaskOperStatus), _type);
            }

            return result;
        }

        public string GetTaskStatus(string status)
        {
            string result = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustTaskStatus), _status);
            }
            return result;
        }

    }
}