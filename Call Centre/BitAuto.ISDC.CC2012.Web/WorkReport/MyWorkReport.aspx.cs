using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WorkReport
{
    public partial class MyWorkReport : PageBase
    {
        public string WpUrl = CommonFunction.ObjectToString(ConfigurationManager.AppSettings["WpUrl"]);
        public string SysUrl = CommonFunction.ObjectToString(ConfigurationManager.AppSettings["SysUrl"]);

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}