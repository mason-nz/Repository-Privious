using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.Utils.Config;

namespace XYAuto.BUOC.ChiTuData2017.Web
{
    public partial class LoginExternal : System.Web.UI.Page
    {
        public string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
                //if (XYAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin())
                if (XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                    lu.UserID > 0)
                {
                    #region 记录修改日志
                    //string LogModuleID = XYAuto.YanFa.SysRightsManager.Entities.SRMActionType.Login;
                    //int ActionType = (int)XYAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login;
                    //string Content = "用户" + Session["truename"] + "(ID:" + Session["userid"] + ")登陆成功";
                    //XYAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(LogModuleID, ActionType, Content);
                    #endregion
                    LoginOK(lu);
                }
                else
                {
                    //Response.Redirect(ConfigurationUtil.GetAppSettingValue("ExitAddress"));
                }
            }
        }
        private void LoginOK(XYAuto.ITSC.Chitunion2017.Common.LoginUser lu)
        {
            //Entities.UserInfo userinfo = Bll.UserInfo.Instance.GetUserInfo(Convert.ToInt32(Session["userid"]));
            //string username = string.Empty;
            //if (userinfo != null)
            //{ 
            //    username = userinfo.UserName;
            //}

            string gourl = "";
            if (Request.QueryString["gourl"] != null)
            {
                gourl = Request.QueryString["gourl"];
            }
            else
            {
                //gourl = "SystemManager/ShowAllSys.aspx";
                //Response.Redirect(gourl);

                DataTable dtChild = null;
                DataTable dtParent = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(lu.UserID);
                if (dtParent != null)
                {
                    if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                    {
                        dtChild = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(lu.UserID, dtParent.Rows[0]["moduleid"].ToString());
                        if (dtChild.Rows.Count > 0)
                        {
                            gourl = dtChild.Rows[0]["url"].ToString();
                        }
                    }
                    else
                    {
                        gourl = dtParent.Rows[0]["url"].ToString();
                    }
                    Response.Redirect(gourl, false);
                }




                //if (username != "admin" && username != "fangxc" && username != "ruiyh" && username != "cheng"
                //    && username != "cw-zhangxa"
                //    && username != "Andy")
                //{
                //    #region 判断如果有新CRM的权限，直接跳转到新CRM
                //    DataTable dstTable = SysRightManager.Common.UserInfo.Instance.GetSysList(Convert.ToInt32(Session["userid"]));
                //    var drs = dstTable.Select("SysID='SYS027'");
                //    if (drs.Length >= 1)
                //    {
                //        gourl = drs[0]["SysURL"].ToString();
                //    }
                //    #endregion
                //}
                //else
                //{
                //    gourl = "SystemManager/ShowAllSys.aspx";
                //}
                try
                {
                    if (Request.QueryString["c"] != null &&
                        dtChild != null && dtChild.Rows.Count > 0)
                    {
                        //DataTable dtParent = XYAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(lu.UserID, sysID);

                        //DataTable dtChild = XYAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(lu.UserID, sysID, dtParent.Rows[0]["moduleid"].ToString());
                        gourl = dtChild.Rows[0]["url"].ToString();
                    }
                }
                catch { }
            }
            if (gourl == "")
            {
                gourl = "SystemManager/ShowAllSys.aspx";
            }
            try
            {
                int userid = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                //string cacheName = BLL.Util.getUserTopParentCacheName(userid.ToString());
                //System.Web.HttpRuntime.Cache.Remove(cacheName);
            }
            catch { }
            Response.Redirect(gourl);
        }
        protected static string ThisSysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        //protected void Button1_ServerClick(object sender, EventArgs e)
        //{
        //    if (ValidatecodeTool.ValidateInputcode(txtValidate.Value.Trim(), false) == false)
        //    {
        //        XYAuto.Utils.ScriptHelper.ShowAlertScript("验证码错误!");
        //    }
        //    else
        //    {
        //        string username = Request.Form["username"].Trim();
        //        string password = XYAuto.Utils.Security.DESEncryptor.Encrypt(Request.Form["password"].Trim());
        //        int ret = XYAuto.YanFa.SysRightManager.Common.UserInfo.Login(username, password);

        //        if (ret > 0)
        //        {
        //            XYAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
        //            BLL.UserLoginInfo.Instance.InsertLog("用户名为　" + username + "　的用户登陆成功");
        //            BLL.UserLoginInfo.Instance.InsertUserLoginInfoLog("用户名为　" + username + "　的用户登陆成功");
        //            string gourl = "";
        //            if (Request.QueryString["gourl"] != null)
        //                gourl = Request.QueryString["gourl"];
        //            else
        //            {
        //                gourl = "SystemManager/ShowAllSys.aspx";

        //            }
        //            if (gourl == "")
        //            {
        //                gourl = "SystemManager/ShowAllSys.aspx";
        //            }
        //            try
        //            {
        //                string cacheName = BLL.Util.getUserTopParentCacheName(Session["userid"].ToString());
        //                System.Web.HttpRuntime.Cache.Remove(cacheName);
        //            }
        //            catch { }
        //            Response.Redirect(gourl);
        //        }
        //        else
        //        {
        //            //Biz.Log.LoginLog("<font color=red>登陆失败信息：用户名 " + username + " 密码 " + password + " IP " + Request.ServerVariables["REMOTE_ADDR"] + " </font>");
        //            BLL.UserLoginInfo.Instance.InsertLog("用户名为　" + username + "　的用户登录失败");
        //            BLL.UserLoginInfo.Instance.InsertUserLoginInfoLog("用户名为　" + username + "　的用户登录失败");
        //            string msg = "用户名密码错误,请重新输入!";
        //            if (ret == -3)
        //                msg = "该用户名已被停用，请与管理员联系";
        //            XYAuto.Utils.ScriptHelper.ShowAlertScript(msg);
        //        }
        //    }
        //}
    }
}
