using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public bool right_AssignTask = false;
        public bool right_RevokeTask = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                right_AssignTask = BLL.Util.CheckButtonRight("SYS024BUT130401");
                right_RevokeTask = BLL.Util.CheckButtonRight("SYS024BUT130402");
                rp_AreaOptions.DataSource = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAllDistrict();
                rp_AreaOptions.DataBind();

                DataTable dt1 = BLL.Util.GetEnumDataTable(typeof(StopCustTaskStatus));
                ddlTaskStatusRepeater.DataSource = dt1;
                ddlTaskStatusRepeater.DataBind();

                DataTable dt2 = BLL.Util.GetEnumDataTable(typeof(StopCustStopStatus));
                ddlStopStatusRepeater.DataSource = dt2;
                ddlStopStatusRepeater.DataBind();
            }
        }
    }
}