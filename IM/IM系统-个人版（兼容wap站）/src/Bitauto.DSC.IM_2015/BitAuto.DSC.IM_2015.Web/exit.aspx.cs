using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common = BitAuto.YanFa.SysRightManager.Common;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class exit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.clear();
            Response.Redirect(ConfigurationUtil.GetAppSettingValue("ExitAddress"));
        }
    }
}