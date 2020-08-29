using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class SelectCustinfoPoper : PageBase
    {
        public string CrmCustID
        {
            get { return BLL.Util.GetCurrentRequestStr("CrmCustID"); }
        }
        public string UserName
        {
            get { return BLL.Util.GetCurrentRequestStr("UserName"); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustUserBind();
            }
        }

        private void CustUserBind()
        {
            int totalCount = 0;
            if (!string.IsNullOrEmpty(CrmCustID))
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustInfo mode = new YanFa.Crm2009.Entities.QueryCustInfo();
                mode.CustID = CrmCustID;
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(mode, "", BLL.PageCommon.Instance.PageIndex, 10, out totalCount);
                rptUser.DataSource = dt;
                rptUser.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 10, totalCount, 10, BLL.PageCommon.Instance.PageIndex, 3);
            }
            else
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustInfo mode = new YanFa.Crm2009.Entities.QueryCustInfo();
                mode.CustName = UserName;
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(mode, "", BLL.PageCommon.Instance.PageIndex, 10, out totalCount);
                rptUser.DataSource = dt;
                rptUser.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 10, totalCount, 10, BLL.PageCommon.Instance.PageIndex, 3);
            }
        }
    }
}