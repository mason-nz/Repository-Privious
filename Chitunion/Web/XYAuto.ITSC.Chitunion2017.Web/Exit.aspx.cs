using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.Web
{
    public partial class Exit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string exitUrl = ConfigurationUtil.GetAppSettingValue("ExitAddress") + "/Login.aspx";
            try
            {
                Chitunion2017.Common.LoginUser lu = new Chitunion2017.Common.LoginUser();
                if (Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                    lu.UserID > 0 && lu.Category == 29003)
                {
                    exitUrl = ConfigurationUtil.GetAppSettingValue("ExitSYSAddress") + "/login.aspx";
                }
                Chitunion2017.Common.UserInfo.Clear();
                Response.Redirect(exitUrl, false);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("退出赤兔系统时，出错", ex);
                Response.Redirect(exitUrl,false);
            }
        }
    }
}