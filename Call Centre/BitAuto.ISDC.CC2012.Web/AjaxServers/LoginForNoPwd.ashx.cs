using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.SessionState;
using BitAuto.Utils.Config;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// LoginForNoPass 的摘要说明
    /// </summary>
    public class LoginForNoPass : IHttpHandler, IRequiresSessionState
    {

        protected static string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
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

        private string RequestPWD
        {
            get { return currentContext.Request.Form["pwd"] == null ? string.Empty : currentContext.Request.Form["pwd"].Trim(); }
        }

        private string RequestGoURL
        {
            get { return currentContext.Request.Form["gourl"] == null ? string.Empty : System.Web.HttpUtility.UrlDecode(currentContext.Request.Form["gourl"].Trim()); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            currentContext = context;
            int userID = BLL.Util.GetLoginUserID();
            bool IsRight = BLL.Util.CheckRight(userID, "SYS024BUT5104");
            if (IsRight && currentContext.Request.Form["isVal"] == "yes")
            {

                if ((RequestUsername != null && RequestUsername.Length > 0))
                {
                    int ret = 0;
                    string url = string.Empty;


                    //工号
                    //string agentNum = Page.Request.Form["AgentNum"] + "";
                    //if (string.IsNullOrEmpty(agentNum) == false
                    //    && BLL.CC_Employee_Agent.Instance.IsExist(agentNum, Convert.ToInt32(Session["userid"])) == false)//有工号
                    //{
                    //    ret = -9;//工号错误
                    //}
                    //else
                    //{


                    //域账号
                    string username = RequestUsername.ToLower();
                    //string adminUserName = "weisz";//Add=2013-03-21 Masj 模拟登陆魏淑珍账号验证登陆权限 //this.RequestAdminUsername.ToLower();
                    string adminUserName = BLL.Util.GetLoginUserName();//Add=2014-07-07 Masj 模拟登陆账号为当前登录者的账号

                    if (username.StartsWith("tech\\"))
                    {
                        username = username.Substring(5, username.Length - 5);
                    }
                    //string password = Request.Form["pwd"];
                    string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                    IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                                       organizationService);

                    LoginResult loginResult = service.Login(adminUserName, RequestPWD);
                    if (loginResult == LoginResult.Success)
                    {
                        ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(username);
                        if (ret > 0)
                        {
                            BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
                            string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath").Replace("~", "");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")
                            if (!string.IsNullOrEmpty(RequestGoURL))
                            {
                                gourl = RequestGoURL;
                            }
                            else
                            {
                                gourl = ConfigurationUtil.GetAppSettingValue("ExitAddress");
                                //DataTable dtParent = UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(currentContext.Session["userid"]), sysID);
                                //if (dtParent != null)
                                //{
                                //    DataTable dtChild = UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(currentContext.Session["userid"]), sysID, dtParent.Rows[0]["moduleid"].ToString());
                                //    if (dtChild.Rows.Count > 0)
                                //    {
                                //        gourl = dtChild.Rows[0]["url"].ToString();
                                //    }
                                //}
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
                    }
                    else if (loginResult == LoginResult.WrongPassword)
                    {
                        ret = -8;//密码错误
                    }


                    //int msg = Bll.UserInfo.Login(Page.Request.Form["username"].Trim(), Page.Request.Form["pwd"].Trim());
                    context.Response.Write(ret.ToString() + "," + url);
                    context.Response.End();
                    return;
                }
            }
            context.Response.Write("-1,");
            context.Response.End();
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