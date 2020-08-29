using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.UControl
{
    public partial class CustSalesControl : System.Web.UI.UserControl
    {
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string crmCustID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}