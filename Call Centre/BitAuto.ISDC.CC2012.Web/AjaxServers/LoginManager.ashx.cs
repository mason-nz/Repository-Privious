using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using BitAuto.Utils.Config;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using System.Data;
using BitAuto.Utils.Security;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// LoginManager 的摘要说明
    /// </summary>
    public class LoginManager : IHttpHandler, IRequiresSessionState
    {
        protected static string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        private static bool isLogAjaxStat = Convert.ToBoolean(Utils.Config.ConfigurationUtil.GetAppSettingValue("IsLogAjaxStat"));
        public HttpContext currentContext;

        #region 属性
        private string RequestisVal
        {
            get { return currentContext.Request.Form["RequestisVal"] == null ? string.Empty : currentContext.Request.Form["RequestisVal"].Trim(); }
        }

        private string RequestUsername
        {
            get { return currentContext.Request.Form["username"] == null ? string.Empty : currentContext.Request.Form["username"].Trim(); }
        }

        private string RequestOldPWD
        {
            get { return currentContext.Request.Form["oldpwd"] == null ? string.Empty : currentContext.Request.Form["oldpwd"].Trim(); }
        }

        private string RequestPWD
        {
            get { return currentContext.Request.Form["pwd"] == null ? string.Empty : currentContext.Request.Form["pwd"].Trim(); }
        }

        private string RequestGoURL
        {
            get { return currentContext.Request.Form["gourl"] == null ? string.Empty : System.Web.HttpUtility.UrlDecode(currentContext.Request.Form["gourl"].Trim()); }
        }

        private string RequestAction
        {
            get { return currentContext.Request.Form["Action"] == null ? string.Empty : System.Web.HttpUtility.UrlDecode(currentContext.Request.Form["Action"].Trim()); }
        }
        private int RequestDurationTime
        {
            get { return BLL.Util.GetCurrentRequestFormInt("DurationTime"); }
        }
        private string RequestCurrentURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CurrentURL"); }
        }

        /// <summary>
        /// 加载时长（毫秒）
        /// </summary>
        private string RequestLoadTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("LoadTime"); }
        }
        /// <summary>
        /// 请求URL（不带参数）
        /// </summary>
        private string RequestRequestURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("RequestURL"); }
        }
        /// <summary>
        /// 请求URL后参数
        /// </summary>
        private string RequestRequestURLPara
        {
            get { return BLL.Util.GetCurrentRequestFormStr("RequestURLPara"); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;
            if (currentContext.Request.Form["isVal"] == "yes")
            {
                Login(context);
            }
            if (currentContext.Request.Form["isVal"] == "login2")
            {
                Login2(context);
            }
            if (currentContext.Request.Form["isVal"] == "updatepwd")
            {
                UpdateUserPassword(context);
            }
            else if (RequestAction == "StatPageTime")
            {
                if (isLogAjaxStat)
                {
                    Uri uri = new Uri(RequestCurrentURL);
                    string info = string.Format("RequestURL:{0}|Para:{1}|LoadTime:{2}|TrueName:{3}|BGName:{4}|RoleName|{5}",
                        uri.Host + uri.AbsolutePath, uri.Query, RequestDurationTime, BLL.Util.GetLoginRealName(),
                        BLL.Util.GetSessionValue("Login_BGName"), BLL.Util.GetSessionValue("Login_RoleName"));
                    BLL.Util.LogForWeb("WebLog", info, "页面监控");
                }
                context.Response.Write("{\"Result\":\"" + isLogAjaxStat + "\"}");
            }
            else if (RequestAction == "StatAjaxPageTime")
            {
                if (isLogAjaxStat)
                {
                    Uri uri = new Uri(Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress") + RequestRequestURL);
                    string info = string.Format("RequestURL:{0}|Para:{1}|LoadTime:{2}|TrueName:{3}|BGName:{4}|RoleName|{5}",
                        uri.Host + uri.AbsolutePath, uri.Query, RequestLoadTime, BLL.Util.GetLoginRealName(),
                        BLL.Util.GetSessionValue("Login_BGName"), BLL.Util.GetSessionValue("Login_RoleName"));
                    BLL.Util.LogForWeb("AjaxLog", info, "页面监控");
                }
                context.Response.Write("{\"Result\":\"" + isLogAjaxStat + "\"}");
            }

            context.Response.End();
        }

        private void Login(HttpContext context)
        {

            //获取登录IP，判断是否在黑名单中，是则提示密码错误
            var ip = BitAuto.YanFa.SysRightManager.Common.IpLimitHelper.GetIpAddress();
            if (BitAuto.YanFa.SysRightManager.Common.IpLimitHelper.CheckIsBlack(ip))
            {
                context.Response.Write("-8,");
                context.Response.End();
                return;
            }

            if ((RequestUsername != null && RequestUsername.Length > 0) && (RequestPWD != null && RequestPWD.Length > 0))
            {
                int ret = 0;
                string url = string.Empty;

                //域账号
                string username = RequestUsername.ToLower();

                if (username.StartsWith("tech\\"))
                {
                    username = username.Substring(5, username.Length - 5);
                }
                //string password = Request.Form["pwd"];
                string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                                   organizationService);
                LoginResult loginResult = service.Login(username, RequestPWD);
                if (loginResult == LoginResult.Success)
                {

                    ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(username);
                    if (ret > 0)
                    {
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
                        currentContext.Session["EmployeeNumber"] = BLL.Util.GetEmployeeNumberByUserID(ret);
                        BLL.Util.LoginPassport(ret, sysID);
                        string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath").Replace("~", "");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")
                        if (!string.IsNullOrEmpty(RequestGoURL))
                        {
                            gourl = RequestGoURL;
                        }
                        else
                        {
                            DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(currentContext.Session["userid"]), sysID);
                            if (dtParent != null)
                            {
                                DataTable dtChild = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(currentContext.Session["userid"]), sysID, dtParent.Rows[0]["moduleid"].ToString());
                                if (dtChild.Rows.Count > 0)
                                {
                                    gourl = dtChild.Rows[0]["url"].ToString();
                                }
                            }
                        }
                        ret = 1;//登陆成功
                        string content = string.Format("用户{1}(ID:{0})登录成功。", currentContext.Session["userid"], currentContext.Session["truename"]);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login, content);
                        url = gourl;
                    }
                    else
                    {
                    }
                }
                else if (loginResult == LoginResult.Inactive)
                {
                    ret = -6;//帐户被禁用
                }
                else if (loginResult == LoginResult.UserNotExist)
                {
                    ret = -7;//用户不存在
                    BitAuto.YanFa.SysRightManager.Common.IpLimitHelper.LoginFailOperateIp(ip);
                }
                else if (loginResult == LoginResult.WrongPassword)
                {
                    ret = -8;//密码错误
                    BitAuto.YanFa.SysRightManager.Common.IpLimitHelper.LoginFailOperateIp(ip);
                }
                else
                {
                    ret = -1;
                }
                //}

                //int msg = Bll.UserInfo.Login(Page.Request.Form["username"].Trim(), Page.Request.Form["pwd"].Trim());
                context.Response.Write(ret.ToString() + "," + url);
                context.Response.End();
                return;
            }
        }

        private void Login2(HttpContext context)
        {
            if ((RequestUsername != null && RequestUsername.Length > 0) && (RequestPWD != null && RequestPWD.Length > 0))
            {
                int ret = 0;
                string url = string.Empty;

                //域账号
                string username = RequestUsername.ToLower();

                ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(username, DESEncryptor.Encrypt(RequestPWD));
                if (ret > 0)
                {
                    currentContext.Session["UserName"] = username;
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
                    string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath").Replace("~", "");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")
                    if (!string.IsNullOrEmpty(RequestGoURL))
                    {
                        gourl = RequestGoURL;
                    }
                    else
                    {
                        DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(currentContext.Session["userid"]), sysID);
                        if (dtParent != null)
                        {
                            DataTable dtChild = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(currentContext.Session["userid"]), sysID, dtParent.Rows[0]["moduleid"].ToString());
                            if (dtChild.Rows.Count > 0)
                            {
                                gourl = dtChild.Rows[0]["url"].ToString();
                            }
                        }
                    }
                    ret = 1;//登陆成功
                    string content = string.Format("用户{1}(ID:{0})登录成功。", currentContext.Session["userid"], currentContext.Session["truename"]);
                    BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login, content);
                    url = gourl;
                }
                else if (ret == -1)
                {
                    ret = -8;
                }
                else if (ret == -2)
                {
                    ret = -7;
                }
                //int msg = Bll.UserInfo.Login(Page.Request.Form["username"].Trim(), Page.Request.Form["pwd"].Trim());
                context.Response.Write(ret.ToString() + "," + url);
                context.Response.End();
                return;
            }
        }

        private void UpdateUserPassword(HttpContext context)
        {
            string msg = VerifyData();
            if (msg == string.Empty)
            {
                try
                {
                    string oldPwd = DESEncryptor.Encrypt(RequestOldPWD);
                    string pwd = DESEncryptor.Encrypt(RequestPWD);
                    int ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(RequestUsername, oldPwd);
                    if (ret > 0)
                    {
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.UpdateUserPassword(ret, pwd);
                        string content = string.Format("用户{1}(ID:{0})修改密码{2}为{3}成功。", ret, RequestUsername, oldPwd, pwd);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
                        msg = "success";
                    }
                    else if (ret == -1)
                    {
                        msg = "原密码不正确";
                    }
                    else if (ret == -2)
                    {
                        msg = "不存在此帐号";
                    }
                    else if (ret == -3)
                    {
                        msg = "此帐号已停用";
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            context.Response.Write(msg);
            context.Response.End();
        }

        private string VerifyData()
        {
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(RequestUsername))
            {
                errorMsg = "帐号名称不能为空";
            }
            else if (string.IsNullOrEmpty(RequestOldPWD))
            {
                errorMsg = "原密码不能为空";
            }
            else if (string.IsNullOrEmpty(RequestPWD))
            {
                errorMsg = "新密码不能为空";
            }
            else if (RequestPWD.Length < 6 || RequestPWD.Length > 10)
            {
                errorMsg = "新密码长度必须在6-10个字符之间";
            }

            return errorMsg;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}