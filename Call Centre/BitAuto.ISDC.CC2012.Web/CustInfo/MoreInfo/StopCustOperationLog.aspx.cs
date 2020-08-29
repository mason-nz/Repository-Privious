using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class StopCustOperationLog : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 参数
        private string TaskID
        {
            get
            {
                return Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(Request["TaskID"].ToString().Trim());
            }
        }

        #endregion
        public int GroupLength = 5;
        public int PageSize = 10;
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
            DataTable dt = BLL.OrderCRMStopCustTaskOperationLog.Instance.GetOrderCRMStopCustTaskOperationLog(TaskID, "ocrl.RecID DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repterPersonlist.DataSource = dt;
            repterPersonlist.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 4);
        }
    }
}