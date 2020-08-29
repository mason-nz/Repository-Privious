using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_DMS2014.Web.ErrorPage
{
    public partial class NotAccessMsgPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("您没有访问“IM即时通讯系统”的权限");
        }
    }
}