using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class TagManagement : PageBase
    {
        //权限
        public bool IsRigthBusiTypeEdit = false;
        public bool IsRightTagEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024MOD1021"))
            {
                IsRigthBusiTypeEdit = true;
                IsRightTagEdit = true;
            }
        }
    }
}