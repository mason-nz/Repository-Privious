using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class CustInfoView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string liWidth
        {
            get { return BLL.Util.GetCurrentRequestStr("liWidth"); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}