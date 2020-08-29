using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.WebAPI
{
    public partial class LoginOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.Session["UserName"] != null)
            {
                Context.Session.Remove("UserName");
            }
            Response.Redirect("Login.aspx");
        }
    }
}