using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using BitAuto.Utils.Config;
using System.Configuration;
using BitAuto.Services.Organization.Remoting;
using System.Data;
using BitAuto.YanFa.SysRightManager.Common;
using System.Web.Caching;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    /// <summary>
    /// LoginManager 的摘要说明
    /// </summary>
    public class LoginManager : IHttpHandler, IRequiresSessionState
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
            context.Response.ContentType = "text/plain";
            currentContext = context;
            string UserID = string.Empty;//登录后集中权限系统中UserID
            string TrueName = string.Empty;//登录后集中权限系统中真实姓名

            if (currentContext.Request.Form["isVal"] == "yes")
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
                        UserID = ret.ToString();
                        if (ret > 0)
                        {
                            #region 取跳转url
                            BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
                            TrueName = currentContext.Session["truename"].ToString();

                            string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath").Replace("~", "");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")
                            if (!string.IsNullOrEmpty(RequestGoURL))
                            {
                                gourl = RequestGoURL;
                            }
                            else
                            {
                                DataTable dtParent = UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(currentContext.Session["userid"]), sysID);
                                if (dtParent != null)
                                {
                                    DataTable dtChild = UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(currentContext.Session["userid"]), sysID, dtParent.Rows[0]["moduleid"].ToString());
                                    if (dtChild.Rows.Count > 0)
                                    {
                                        gourl = dtChild.Rows[0]["url"].ToString();

                                    }
                                }
                            }
                            ret = 1;//登陆成功
                            string content = string.Format("用户{1}(ID:{0})登录成功。", UserID, TrueName);
                            BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login, content);
                            url = gourl;
                            #endregion
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
                    //}

                    //int msg = Bll.UserInfo.Login(Page.Request.Form["username"].Trim(), Page.Request.Form["pwd"].Trim());
                    if (ret == 1)
                    {
                        #region IM系统验证逻辑
                        //string agentid = "";
                        //agentid = BLL.BaseData.Instance.GetAgentNumByUserID(UserID);//根据UserID获取CC系统坐席工号（AgentID）

                        //string usernameagent = "";
                        //bool isExist = BLL.AgentInfo.Instance.IsExistAgentIMIDByDomainAccount(username, out agentid, out usernameagent);
                        //if (isExist)
                        //{
                        //    //ret = 1;
                        //}
                        //else
                        //{
                        //    //是否有IM系统授权,有则初始化数据到IM系统
                        //    //无，则提示IM系统收号不存在
                        //    //走到这，说明授权没有问题
                        //    string msg = "";
                        //    if (BLL.AgentInfo.Instance.InitAgentInfo(username, out usernameagent, out agentid, out msg))
                        //    {
                        //        //ret = 1;
                        //    }
                        //    else
                        //    {
                        //        ret = -10;//IM系统帐号初始化失败
                        //    }
                        //}
                        //if (ret == 1)
                        //{
                        bool isExistClient = false;
                        try
                        {
                            isExistClient = DefaultChannelHandler.isExistClient(UserID);
                        }
                        catch (Exception ex)
                        {
                            BLL.Loger.Log4Net.Error(string.Format("LoginManager             {0}", ex.Message));
                        }


                        if (isExistClient)
                        {
                            ret = -11;//该帐号在IM系统已登录
                        }
                        else
                        {
                            HttpContext.Current.Session["agent_IMID"] = UserID;
                            HttpContext.Current.Session["agent_UserName"] = TrueName;
                        }

                        //}
                        #endregion
                    }
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