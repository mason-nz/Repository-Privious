using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite
{
    public partial class WhiteList : PageBase
    {
        public bool right_add = true;
        public bool right_export = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataExportButton = BLL.Util.CheckButtonRight("SYS024BUT4081");

                if (!BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD401102"))
                {
                    Response.Write("对不起，您没有权限访问此页面");
                }
            }
        }
    }
}