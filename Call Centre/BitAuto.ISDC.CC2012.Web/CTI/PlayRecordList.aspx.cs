using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CTI
{
    public partial class PlayRecordList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性定义
        public string RequestRecordURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("RecordURL"); }
        }
        public string RequestWorkOrderID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("OrderID"); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRecordUrl();
            }
        }

        private void BindRecordUrl()
        {
            if (!string.IsNullOrEmpty(RequestWorkOrderID))
            {
                DataTable dt = null;
                //取接通时间和话务时长
                dt = BLL.WorkOrderInfo.Instance.GetWorkOrderRecordUrl_OrderID(RequestWorkOrderID);
                if (dt != null)
                {
                    rptRecordUrlList.DataSource = dt;
                    rptRecordUrlList.DataBind();
                }
            }
        }
    }
}