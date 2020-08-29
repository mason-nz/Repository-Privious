using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    public partial class LoginTest : System.Web.UI.Page
    {
        private string RequestGourl
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestQueryStr("gourl"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("exit.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string mobile = txtMobile.Text;
            int category = 29002;
            if (radiobtnGGZ.Checked)
            {
                category = 29002;
            }
            else if (radiobtnMTZ.Checked)
            {
                category = 29001;
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                bool flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.MoniLogin(mobile, category);
                if (flag)
                {
                    if (string.IsNullOrEmpty(RequestGourl))
                    {
                        XYAuto.Utils.ScriptHelper.ShowAlertAndRedirectScript(this, "登陆成功", "/moneyManager/make_money.html");
                    }
                    else
                    {
                        XYAuto.Utils.ScriptHelper.ShowAlertAndRedirectScript(this, "登陆成功", RequestGourl);
                    }

                }
                else
                {
                    XYAuto.Utils.ScriptHelper.ShowAlertScript(this, "登陆失败");
                }
            }
            else
            {
                XYAuto.Utils.ScriptHelper.ShowAlertScript(this, "请输入手机号");
            }
        }
    }
}