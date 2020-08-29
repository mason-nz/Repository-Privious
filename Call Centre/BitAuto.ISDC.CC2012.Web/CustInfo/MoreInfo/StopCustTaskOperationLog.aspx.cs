using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class StopCustTaskOperationLog : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 参数
        private string TaskID
        {
            get
            {
                return Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskID"].ToString().Trim());
            }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        CustInfoHelper ch = new CustInfoHelper();
        private void BindData()
        {
            DataTable dt = BLL.OrderCRMStopCustTaskOperationLog.Instance.GetOrderCRMStopCustTaskOperationLog(TaskID, "ocrl.RecID DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList2.DataSource = dt;
                repeaterTableList2.DataBind();
            }
            litPagerDown22.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}