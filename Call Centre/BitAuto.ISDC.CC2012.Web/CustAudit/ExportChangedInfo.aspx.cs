using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustAudit
{
    public partial class ExportChangedInfo : PageBase
	{
        public bool right_btnExport;
        private int userID;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                right_btnExport = BLL.Util.CheckRight(userID, "SYS024BUT130301");
            }
		}
	}
}