using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.EPLogin.Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string urlstr = "http://attend.oa.bitauto.com/admin/AttendLogTable.aspx";
            System.Uri aa = new System.Uri(urlstr);

        }
    }
}