using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TagSelectPopNew : PageBase
    {
        public string name
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("name"); }
        }
        public string val
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("val"); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}