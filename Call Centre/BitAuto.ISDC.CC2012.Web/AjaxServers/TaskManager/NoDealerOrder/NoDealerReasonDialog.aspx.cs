using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class NoDealerReasonDialog : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReason();
            }
        }

        private void BindReason()
        {
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.NoDealerReason));
            dllReason.DataValueField = "value";
            dllReason.DataTextField = "name";
            dllReason.DataSource = dt;
            dllReason.DataBind();
        }
    }
}