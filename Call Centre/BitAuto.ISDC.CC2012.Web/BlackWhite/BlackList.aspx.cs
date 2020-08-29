using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite
{
    public partial class BlackList : PageBase
    {
        public bool right_add = true;
        public bool right_export = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataExportButton = BLL.Util.CheckButtonRight("SYS024BUT4081");
                if (!BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD401101"))
                {
                    Response.Write("对不起，您没有权限访问此页面");
                }
                if (BLL.Util.CheckButtonRight("SYS024BUT40110101"))
                {
                    right_export = true;
                }
            }
        }
    }
}