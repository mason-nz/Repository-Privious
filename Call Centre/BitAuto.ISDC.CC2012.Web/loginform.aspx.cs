using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web
{
    public partial class loginform : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           int userID = BLL.Util.GetLoginUserID();

           bool IsRight = BLL.Util.CheckRight(userID, "SYS024BUT5104");
           if (!IsRight)
           {
               BitAuto.Utils.ScriptHelper.ShowCustomScript(this.Page, "alert('您当前没有权限访问！');window.close();");
           }
        }
       
    }
}