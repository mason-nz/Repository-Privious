using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.WebAPI
{
    public partial class AutoCallItemMonitor : System.Web.UI.Page
    {
        public string strName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.Session["UserName"] == null || string.IsNullOrEmpty(Context.Session["UserName"].ToString()))
            {
                Response.Redirect("Login.aspx?red=/AutoCallItemMonitor.aspx");
            }
            else
            {
                strName = Context.Session["UserName"].ToString();
            }

        }
    }
}