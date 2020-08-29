﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2018.AdminWeb
{
    public partial class exit : System.Web.UI.Page
    {
        private string RequestGourl
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestQueryStr("gourl"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string exitUrl = ConfigurationUtil.GetAppSettingValue("ExitAddress");
            try
            {
                //XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
                //if (XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                //    lu.UserID > 0)
                //{
                //    exitUrl = exitUrl + "/login.aspx";
                //}
                exitUrl = exitUrl + "/login.aspx";
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
                Response.Redirect(string.IsNullOrEmpty(RequestGourl) ? exitUrl : exitUrl + "?gourl=" + System.Web.HttpUtility.UrlEncode(RequestGourl), false);
            }
            catch (Exception ex)
            {
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("退出赤兔系统时，出错", ex);
                Response.Redirect(exitUrl, false);
            }
        }
    }
}