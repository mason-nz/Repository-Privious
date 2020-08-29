using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.OtherTask
{
    public partial class OtherTaskList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //功能权限
        public bool right_allocation;        //分配权限
        public bool right_withdraw;         //收回权限
        public bool right_gameover;         //结束权限
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                right_allocation = BLL.Util.CheckRight(userID, "SYS024BUT150101");
                right_withdraw = BLL.Util.CheckRight(userID, "SYS024BUT150102");
                right_gameover = BLL.Util.CheckRight(userID, "SYS024BUT150104");
            }
        }
    }

}