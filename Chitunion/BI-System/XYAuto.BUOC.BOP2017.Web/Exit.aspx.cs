using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.Utils.Config;

namespace XYAuto.BUOC.BOP2017.Web
{
    public partial class Exit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string exitUrl = ConfigurationUtil.GetAppSettingValue("ExitAddress") + "/Login.aspx";
            try
            {
                //ITSC.Chitunion2017.Common.LoginUser lu = new ITSC.Chitunion2017.Common.LoginUser();
                //if (ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                //    lu.UserID > 0 && lu.Category == 29003)
                //{
                //    exitUrl = ConfigurationUtil.GetAppSettingValue("ExitSYSAddress") + "/login.aspx";
                //}
                ITSC.Chitunion2017.Common.UserInfo.Clear();
                Response.Redirect(exitUrl, false);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("退出赤兔商务运营系统时，出错", ex);
                Response.Redirect(exitUrl, false);
            }
        }
    }
}