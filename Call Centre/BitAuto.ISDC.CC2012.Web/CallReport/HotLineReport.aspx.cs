using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CallReport
{
    public partial class HotLineReport : PageBase
    {
        public bool DataExportButton = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataExportButton = BLL.Util.CheckButtonRight("SYS024BUT4081");
            }
        }
    }
}