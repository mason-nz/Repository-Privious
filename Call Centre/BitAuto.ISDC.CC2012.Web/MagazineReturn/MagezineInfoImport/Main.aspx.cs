using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.MagazineReturn.MagezineInfoImport
{
    public partial class Main : PageBase
    {
        public string UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //增加“客户池--易湃客户”杂志回访导入功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            UserID = userId.ToString();
            if (!BLL.Util.CheckRight(userId, "SYS024BUG200301"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}