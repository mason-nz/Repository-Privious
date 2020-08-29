using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Web.Controls;

namespace BitAuto.DSC.IM_DMS2014.Web.SeniorManage
{
    public partial class FreProblemList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
        }
    }
}