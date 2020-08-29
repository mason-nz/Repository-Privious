using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.BusinessReport
{
    public partial class B_ProjectReport : PageBase
    {
        public bool right_Export = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            int currentUserID = BLL.Util.GetLoginUserID();
            right_Export = BLL.Util.CheckRight(currentUserID, "SYS024MOD8101");
        }
    }
}