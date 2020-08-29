using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.LeadsTask
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //功能权限
        protected bool right_allocation;        //分配权限
        protected bool right_withdraw;         //收回权限

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                right_allocation = BLL.Util.CheckRight(userID, "SYS024BUT101201");
                right_withdraw = BLL.Util.CheckRight(userID, "SYS024BUT101202");
            }

        }
    }
}