using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM2014.Server.Web.Controls
{
    public partial class ControlManage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool result = false;
            string usrPath = Page.Request.Path.ToString().ToLower();
            usrPath = usrPath.Substring(usrPath.LastIndexOf("/") + 1);
            this.Lit_menus.Text = PageUtil.GetSubModules("<li><a {$lab_class$} href='{$lab_url$}'>{$lab_name$}</a></li>", usrPath, Convert.ToInt32(Session["userid"]), "SYS032MOD1002", ref result);
            if (result == false)
            {
                Response.Redirect("ErrorPage/NotAccessMsgPage.aspx");
            }
        }
    }
}