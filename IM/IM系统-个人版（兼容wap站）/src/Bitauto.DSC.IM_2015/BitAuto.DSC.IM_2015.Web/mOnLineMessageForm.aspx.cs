using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class mOnLineMessageForm : System.Web.UI.Page
    {
        public string VisitID
        {
            get
            {
                return HttpContext.Current.Request["VisitID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["VisitID"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}