using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Services.Organization.Remoting;
using System.Configuration;
using BitAuto.Utils.Config;
namespace BitAuto.ISDC.CC2012.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected static string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");

        private string DomainAccount
        {
            get { return BLL.Util.GetCurrentRequestStr("DomainAccount"); }
        }
        private string Password
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Password")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //BitAuto.ISDC.CC2012.WebService.CRM.CRMYJKServiceHelper.Instance.GetDemandCallDetail("", 1);

                LoginLogic();
                //根据参数，实现自动登录，并跳转页面
                LoginByPara();
            }
        }

        private void LoginByPara()
        {
            string url = "login.aspx";
            if (!string.IsNullOrEmpty(DomainAccount) && !string.IsNullOrEmpty(Password))
            {
                string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                                   organizationService);
                LoginResult loginResult = service.Login(DomainAccount, Password);
                if (loginResult == LoginResult.Success)
                {

                    int ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(DomainAccount);
                    if (ret > 0)
                    {
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(ret);
                        BLL.Util.LoginPassport(ret, sysID);
                        url = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath").Replace("~", "");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")

                        DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(HttpContext.Current.Session["userid"]), sysID);
                        if (dtParent != null)
                        {
                            DataTable dtChild = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(HttpContext.Current.Session["userid"]), sysID, dtParent.Rows[0]["moduleid"].ToString());
                            if (dtChild.Rows.Count > 0)
                            {
                                url = dtChild.Rows[0]["url"].ToString();
                            }
                        }

                        ret = 1;//登陆成功
                        string content = string.Format("用户{1}(ID:{0})登录成功。", HttpContext.Current.Session["userid"], HttpContext.Current.Session["truename"]);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login, content);
                    }
                }
                Response.Redirect(url);
            }
        }

        private void LoginLogic()
        {
            string gourl = string.Empty;
            int userid = -1;
            try
            {
                if (BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin() &&
                    int.TryParse(Session["userid"].ToString(), out userid))
                {
                    DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(userid, sysID);
                    if (dtParent != null)
                    {
                        DataTable dtChild = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetChildModuleByUserId(userid, sysID, dtParent.Rows[0]["moduleid"].ToString());
                        if (dtChild.Rows.Count > 0)
                        {
                            gourl = dtChild.Rows[0]["url"].ToString();
                            Response.Redirect(gourl);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

        }
    }
}