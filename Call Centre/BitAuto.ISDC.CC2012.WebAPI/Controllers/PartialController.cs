using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using BitAuto.ISDC.CC2012.WebAPI.Helper;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using BitAuto.Services.Organization.Remoting;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{
    public class PartialController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            System.Web.HttpContext.Current.Session["private_key"] = rsa.ToXmlString(true);

            //把公钥适当转换，准备发往客户端
            RSAParameters parameter = rsa.ExportParameters(true);
            ViewBag.strPublicKeyExponent = CommonHelper.BytesToHexString(parameter.Exponent);
            ViewBag.strPublicKeyModulus = CommonHelper.BytesToHexString(parameter.Modulus);
            rsa.Dispose();

            return PartialView();
        }

        [HttpPost]
        public ActionResult Login(string u, string p)
        {
            CommonJsonResult result = new CommonJsonResult();
            var currentContext = System.Web.HttpContext.Current;

            if (!string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(p))
            {
                string url = string.Empty;
                //域账号
                string username = u.ToLower();
                if (username.StartsWith("tech\\"))
                {
                    username = username.Substring(5, username.Length - 5);
                }
                LoginResult loginResult;
                try
                {
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.FromXmlString(currentContext.Session["private_key"].ToString());

                    byte[] resultP = rsa.Decrypt(CommonHelper.HexStringToBytes(p), false);
                    System.Text.ASCIIEncoding enc = new ASCIIEncoding();
                    string strPwdReal = enc.GetString(resultP);

                    //string password = Request.Form["pwd"];
                    string organizationService = ConfigurationManager.AppSettings["OrganizationService"];
                    IOrganizationService service = (IOrganizationService)Activator.GetObject(typeof(IOrganizationService),
                        organizationService);
                    loginResult = service.Login(username, strPwdReal);
                }
                catch (Exception ex)
                {
                    result.ErrorNumber = ex.GetHashCode();
                    result.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return Json(result);
                }

                if (loginResult == LoginResult.Success)
                {
                    result.ErrorNumber = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login(username);
                    if (result.ErrorNumber > 0)
                    {
                        currentContext.Session["UserName"] = username;
                        BitAuto.YanFa.SysRightManager.Common.UserInfo.passport(result.ErrorNumber);

                        result.ErrorNumber = 0; //登陆成功
                        string content = string.Format("用户{1}(ID:{0})登录成功。", currentContext.Session["userid"],
                            currentContext.Session["truename"]);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(
                            ConfigurationUtil.GetAppSettingValue("LoginLogModuleID"),
                            (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Login, content);
                        result.ErrorMsg = "登录成功";
                        result.Success = true;
                    }
                    else
                    {
                    }
                }
                else if (loginResult == LoginResult.Inactive)
                {
                    result.ErrorNumber = -6; //帐户被禁用
                    result.ErrorMsg = "账户被禁用";
                }
                else if (loginResult == LoginResult.UserNotExist)
                {
                    result.ErrorNumber = -7; //用户不存在
                    result.ErrorMsg = "用户不存在";
                }
                else if (loginResult == LoginResult.WrongPassword)
                {
                    result.ErrorNumber = -8; //密码错误
                    result.ErrorMsg = "密码错误";
                }
                else
                {
                    result.ErrorNumber = -1;
                    result.ErrorMsg = "未知错误";
                }
            }
            else
            {
                result.ErrorNumber = -100;
                result.ErrorMsg = "请输入用户名密码";
            }
            return Json(result);
        }
    }
}
