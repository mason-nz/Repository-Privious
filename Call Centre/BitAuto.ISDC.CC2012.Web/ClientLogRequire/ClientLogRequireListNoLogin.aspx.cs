using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.ClientLogRequire
{
    public partial class ClientLogRequireListNoLogin : System.Web.UI.Page
    {
        public string NoLogin
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("nologin");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (NoLogin != BitAuto.ISDC.CC2012.Web.ClientLogRequire.Ajax.LogRequireHandler.NoLoginKey)
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}