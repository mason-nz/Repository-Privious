using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    public partial class test : System.Web.UI.Page
    {
        private string Code = "01A4F299-C87C-4358-A707-876DDBC57070";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VerifyLogic();
            }
        }
        protected bool VerifyLogic()
        {
            //XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
            if (Request.QueryString["code"] != null && Request.QueryString["code"] == Code)
            {
                return true;
            }
            XYAuto.Utils.ScriptHelper.ShowAlertScript(this.Page, "当前登陆人权限不足，无法访问");
            return false;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string userid = txtUserID.Value;
            if (!string.IsNullOrEmpty(userid))
            {
                string cookies = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(int.Parse(userid));
                litCookieText.Text = cookies;
            }
        }
    }
}