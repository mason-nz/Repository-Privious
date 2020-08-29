using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Entities.UserManage;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    public partial class SwitchPage1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string RequestJumpPage = ITSC.Chitunion2017.BLL.Util.GetCurrentRequestQueryStr("JumpPage");
            //XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录开始-》" + RequestJumpPage);
            //System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            //Label1.Text = "cookie：" + token.Value;
            //string cookie_XYApp = token.Value;
            //if (!string.IsNullOrEmpty(cookie_XYApp))
            //{
            //    LoginUserInfo lu = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByToken(cookie_XYApp);
            //    if (lu != null)
            //    {
            //        XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》成功");
            //        Response.Redirect(RequestJumpPage);
            //    }
            //    else
            //    {
            //        XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》根据token未获取到登录信息");
            //        MsgTitle.Text = "大全登录-》根据token未获取到登录信息";
            //    }
            //}
            //else
            //{
            //    XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》未获取到cookie");
            //    MsgTitle.Text = "大全登录-》未获取到cookie";
            //}
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            //string RequestJumpPage = ITSC.Chitunion2017.BLL.Util.GetCurrentRequestQueryStr("JumpPage");
            //XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录开始-》" + RequestJumpPage);
            //System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            //TextBox1.Text = usertoken.Value;
            //if (!string.IsNullOrEmpty(usertoken.Value))
            ////string cookie_XYApp = "xy_usertoken";
            ////if (webHttp.Request.Cookies[cookie_XYApp]!=null &&!string.IsNullOrEmpty(webHttp.Request.Cookies[cookie_XYApp].Value))
            //{
            //    LoginUserInfo lu = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByToken(usertoken.Value);
            //    if (lu != null)
            //    {
            //        XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》成功");
            //        Response.Redirect(RequestJumpPage);
            //    }
            //    else
            //    {
            //        XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》根据token未获取到登录信息");
            //        MsgTitle.Text = "大全登录-》根据token未获取到登录信息";
            //    }
            //}
            //else
            //{
            //    XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》未获取到cookie");
            //    MsgTitle.Text = "大全登录-》未获取到cookie";
            //}
        }
    }
}