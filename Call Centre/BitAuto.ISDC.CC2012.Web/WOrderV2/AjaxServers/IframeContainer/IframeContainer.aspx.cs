using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers
{
    public partial class IframeContainer : PageBase
    {
        public string Url { get { return BLL.Util.GetCurrentRequestStr("Url"); } }
        public string Height { get { return BLL.Util.GetCurrentRequestStr("height"); } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}