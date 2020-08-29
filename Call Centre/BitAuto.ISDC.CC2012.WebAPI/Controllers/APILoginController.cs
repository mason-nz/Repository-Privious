using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using BitAuto.Services.Organization.Remoting;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{

    public class APILoginController : ApiController
    {
        //[NonAction]
        [HttpGet]
        public CommonJsonResult CommonLogin(string u, string p)
        {
            CommonJsonResult result = new CommonJsonResult();
            var currentContext = HttpContext.Current;

            if (!string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(p))
            {

                string url = string.Empty;

                //域账号
                string username = u.ToLower();

                if (username.StartsWith("tech\\"))
                {
                    username = username.Substring(5, username.Length - 5);
                }
                //string password = Request.Form["pwd"];
                string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                                                                   organizationService);
                LoginResult loginResult = service.Login(username, p);
                if (loginResult == LoginResult.Success)
                {

                    result.ErrorNumber = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(username);
                    if (result.ErrorNumber > 0)
                    {
                        currentContext.Session["UserName"] = username;
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(result.ErrorNumber);

                        result.ErrorNumber = 0;//登陆成功
                        string content = string.Format("用户{1}(ID:{0})登录成功。", currentContext.Session["userid"], currentContext.Session["truename"]);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login, content);
                        result.ErrorMsg = "登录成功";
                        result.Success = true;
                    }
                    else
                    {
                    }
                }
                else if (loginResult == LoginResult.Inactive)
                {
                    result.ErrorNumber = -6;//帐户被禁用
                    result.ErrorMsg = "账户被禁用";
                }
                else if (loginResult == LoginResult.UserNotExist)
                {
                    result.ErrorNumber = -7;//用户不存在
                    result.ErrorMsg = "用户不存在";
                }
                else if (loginResult == LoginResult.WrongPassword)
                {
                    result.ErrorNumber = -8;//密码错误
                    result.ErrorMsg = "密码错误";
                }
                else
                {
                    result.ErrorNumber = -1;
                    result.ErrorMsg = "未知错误";
                }
            }

            return result;

        }
    }



}
