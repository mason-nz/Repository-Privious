using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class SMSStatistics : PageBase
    {
        public bool IsExport = false;//是否可以“导出”功能
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                IsExport = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD40111");
            }
        }
    }
}