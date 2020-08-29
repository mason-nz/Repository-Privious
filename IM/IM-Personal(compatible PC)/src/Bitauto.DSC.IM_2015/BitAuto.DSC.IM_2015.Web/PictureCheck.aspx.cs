using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class PictureCheck : System.Web.UI.Page
    {
        public string GuidStr
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["GuidStr"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}