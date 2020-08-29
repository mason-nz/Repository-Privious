using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers
{
    public partial class WorkOrderCreateUserPoper : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region
        private string UserName
        {
            get { return BLL.Util.GetCurrentRequestStr("UserName"); }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                WorkOrderUserBind();
            }
        }

        private void WorkOrderUserBind()
        {
            int totalCount = 0;
            string where = " And TrueName like '%" + UserName.Trim() + "%'";
            DataTable dt = BLL.WorkOrderInfo.Instance.GetCreateUser(where, "", BLL.PageCommon.Instance.PageIndex, 10, out totalCount);
            rptUser.DataSource = dt;
            rptUser.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 10, totalCount, 10, BLL.PageCommon.Instance.PageIndex, 3);
        }
    }
}