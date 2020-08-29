using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask
{
    public partial class YTGActivityProjectList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = BLL.Util.GetLoginUserID();

            if (!BLL.Util.CheckRight(userID, "SYS024BUT500605"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}