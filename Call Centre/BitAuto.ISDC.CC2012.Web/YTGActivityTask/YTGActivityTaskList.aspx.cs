using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask
{
    public partial class YTGActivityTaskList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //功能权限
        protected bool right_allocation;        //分配权限
        protected bool right_withdraw;         //收回权限
        public int UserID ;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserID = BLL.Util.GetLoginUserID();
                right_allocation = BLL.Util.CheckRight(UserID, "SYS024MOD101503");
                right_withdraw = BLL.Util.CheckRight(UserID, "SYS024MOD101504");
            }

        }
    }
}