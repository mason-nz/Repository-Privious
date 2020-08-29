using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustomerCallin
{
    public partial class ExclusiveMissedCallsList : PageBase
    {
        public bool ExportButton = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExportButton = BLL.Util.CheckButtonRight("SYS024BUT4121");
            }
        }
    }
}