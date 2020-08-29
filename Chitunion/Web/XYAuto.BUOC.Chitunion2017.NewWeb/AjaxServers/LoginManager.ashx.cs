using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.Utils.Config;
using System.Data;
using BLL = XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities.StockBroker;
using XYAuto.ITSC.Chitunion2017.WebService.ITSupport;
using System.Text.RegularExpressions;
using XYAuto.ITSC.Chitunion2017.Dal.EmployeeInfo;
using GeetestSDK;

namespace XYAuto.BUOC.Chitunion2017.NewWeb.AjaxServers
{
    /// <summary>
    /// LoginManager 的摘要说明
    /// </summary>
    public class LoginManager : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        private string GeetestConfig_ID = ConfigurationUtil.GetAppSettingValue("GeetestConfig_ID");
        private string GeetestConfig_Key = ConfigurationUtil.GetAppSettingValue("GeetestConfig_Key");
        protected string ThisSysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        protected string AddUserRoleIDs = ConfigurationUtil.GetAppSettingValue("AddUserRoleIDs");
        protected string SendSMS_RegisterContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_RegisterContentTemp");
        protected string SendSMS_ForgetPwdContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_ForgetPwdContentTemp");
        protected string SendSMS_ModifyMobileContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_ModifyMobileContentTemp");
        protected string SendSMS_WithdrawContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_WithdrawContentTemp");
        protected string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        private int RegisterIPCount_Stop = string.IsNullOrEmpty(ConfigurationUtil.GetAppSettingValue("RegisterIPCount_Stop", false)) ? 20 : int.Parse(ConfigurationUtil.GetAppSettingValue("RegisterIPCount_Stop", false));
        private int RegisterIPCount_Verify = string.IsNullOrEmpty(ConfigurationUtil.GetAppSettingValue("RegisterIPCount_Verify", false)) ? 2 : int.Parse(ConfigurationUtil.GetAppSettingValue("RegisterIPCount_Verify", false));
        private Dictionary<int, string> ListAddUserRoleIDs = new Dictionary<int, string>();

        #region 属性
        private string RequestAction
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr("action"); }
        }
        private string RequestUsername
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr("username"); }
        }

        private string RequestPWD
        {
            get { return BLL.Util.GetCurrentRequestFormStr("pwd"); }
        }

        private string RequestPWDConfirm
        {
            get { return BLL.Util.GetCurrentRequestFormStr("pwdConfirm"); }
        }
        private string RequestMobile
        {
            get { return BLL.Util.GetCurrentRequestFormStr("mobile"); }
        }
        private string RequestMobileCheckCode
        {
            get { return BLL.Util.GetCurrentRequestFormStr("mobileCheckCode"); }
        }

        private string RequestCheckCode
        {
            get { return BLL.Util.GetCurrentRequestFormStr("checkCode").ToLower(); }
        }

        private string RequestGourl
        {
            get { return BLL.Util.GetCurrentRequestFormStr("gourl"); }
        }

        /// <summary>
        /// 用户分类（29001—广告主；29002—媒体主）
        /// </summary>
        private int RequestCategory
        {
            get { return BLL.Util.GetCurrentRequestFormInt("category"); }
        }

        /// <summary>
        /// 标识是否内部用户，1-内部用户，其他-非内部用户
        /// </summary>
        private int RequestIsInside
        {
            get { return BLL.Util.GetCurrentRequestFormInt("isInside"); }
        }

        private string RequestGeetestChallenge
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr(GeetestLib.fnGeetestChallenge); }
        }
        private string RequestGeetestValidate
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr(GeetestLib.fnGeetestValidate); }
        }
        private string RequestGeetestSeccode
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr(GeetestLib.fnGeetestSeccode); }
        }
        #endregion 属性

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;

            switch (RequestAction.ToLower().Trim())
            {
                case "login":
                    Login(context);
                    break;
                case "register":
                    foreach (string item in AddUserRoleIDs.Split('|'))
                    {
                        int categoryID = int.Parse(item.Split(':')[0]);
                        string roleID = item.Split(':')[1];
                        ListAddUserRoleIDs.Add(categoryID, roleID);
                    }
                    Register(context);
                    break;
                case "sendmobilemsg_register":
                    SendMobileMsg(context, SendSMS_RegisterContentTemp, false);
                    break;
                case "sendmobilemsg_forgetpwd":
                    SendMobileMsg(context, SendSMS_ForgetPwdContentTemp, false);
                    break;
                case "sendmobilemsg_modifymobile":
                    SendMobileMsg(context, SendSMS_ModifyMobileContentTemp, true);
                    break;
                case "sendmobilemsg_withdraw":
                    SendMobileMsg(context, SendSMS_WithdrawContentTemp, true);
                    break;
                case "forgetpwd":
                    Forgetpwd(context);
                    break;
                case "checksmscode":
                    CheckSMSCode(context);
                    break;
                case "islaunchverify":
                    IsLaunchVerify(context);
                    break;
                case "simulationlogin":
                    SimulationLogin(context);
                    break;
                default:
                    break;
            }
            context.Response.End();
        }

        private void SimulationLogin(HttpContext context)
        {
            var userId = BLL.Util.GetCurrentRequestFormInt("userId");
            var loginCookie = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            BLL.Loger.Log4Net.Info($" SimulationLogin Passport 返回cookie信息:{loginCookie}");

            if (string.IsNullOrWhiteSpace(loginCookie))
            {
                context.Response.Write("{\"Status\":5006,\"Message\":\"Passport 用户模拟登录失败，未返回登录相关cookie\",\"Result\":null,\"IsOverdue\":null}");
            }

            context.Response.Write("{\"Status\":0,\"Message\":\"登录失败\",\"Result\":null,\"IsOverdue\":null}");

        }

        /// <summary>
        /// 是否开启极验验证码
        /// </summary>
        /// <param name="context"></param>
        private void IsLaunchVerify(HttpContext context)
        {
            int result = 0;
            int ipCount = XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.GetCountByModuleType((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册, ThisSysID);
            if (ipCount >= RegisterIPCount_Verify)
            {
                result = 1;
            }
            context.Response.Write(result);
        }

        private void CheckSMSCode(HttpContext context)
        {
            string msg = string.Empty;
            int result = -1;
            //if ((string)HttpRuntime.Cache.Get(RequestMobile) != RequestMobileCheckCode)
            //    msg = $"验证码错误!";
            //else
            //{
            //    result = 0;
            //    msg = $"成功!";
            //}
            if (RequestMobile != null && RequestMobile.Length > 0 &&
             RequestMobileCheckCode != null && RequestMobileCheckCode.Length > 0)
            {
                if (!BLL.Util.IsHandset(RequestMobile))
                {
                    msg = "请填写正确的手机号";
                }
                else if (BLL.Util.GetMobileCheckCodeByCache(RequestMobile) != RequestMobileCheckCode)
                {
                    msg = "您输入的手机短信验证码不正确";
                }
            }
            else
            {
                msg = "手机号或验证码错误";
            }
            if (string.IsNullOrEmpty(msg))
                result = 0;
            context.Response.Write("{ 'result':" + result + ",'msg':'" + msg + "'  }");
        }

        private void Forgetpwd(HttpContext context)
        {
            string msg = string.Empty;
            int result = -1;
            if (RequestMobile != null && RequestMobile.Length > 0 &&
                RequestPWD != null && RequestPWD.Length > 0 &&
                //RequestCheckCode != null && RequestCheckCode.Length > 0 &&
                RequestCategory > 0 &&
                RequestPWDConfirm != null && RequestPWDConfirm.Length > 0 &&
                RequestMobileCheckCode != null && RequestMobileCheckCode.Length > 0)
            {
                if (ForgetpwdVerifyLogic(context, out msg))
                {
                    string pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(RequestPWD + RequestCategory + LoginPwdKey, System.Text.Encoding.UTF8);
                    int ret = EmployeeInfo.Instance.UpdatePwdByCategoryAndMobile(RequestCategory, RequestMobile, pwd);
                    string content = string.Format("Category：{0}，Mobile：{1}，用户重置密码{2}。", RequestCategory, RequestMobile,
                        (ret > 0 ? "成功" : "失败"));
                    if (ret > 0)
                    {
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Modify, content);
                        result = 0;
                        msg = RequestGourl;
                    }
                    else
                    {
                        BLL.Loger.Log4Net.Info(content);
                        msg = "用户重置密码失败";
                        result = -1;
                    }
                }
            }

            context.Response.Write("{ 'result':" + result + ",'msg':'" + msg + "'  }");
        }

        /// <summary>
        /// 忘记密码验证逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool ForgetpwdVerifyLogic(HttpContext context, out string msg)
        {
            msg = string.Empty;
            bool flag = false;
            //Regex r = new Regex(@"(?!^[a-zA-Z]+$)(?!^[\d]+$)(?!^[^a-zA-Z-_\d]+$)^.{4,20}$");

            if (!BLL.Util.IsHandset(RequestMobile))
            {
                msg = "请填写正确的手机号";
            }
            else if (RequestPWD.Length < 6 || RequestPWD.Length > 20)
            {
                msg = "密码长度应为6~20位字符！";
            }
            else if (string.IsNullOrEmpty(RequestPWDConfirm) || RequestPWDConfirm != RequestPWD)
            {
                msg = "确认密码与密码不一致，请重新输入！";
            }
            //else if (context.Session["ValidateCode"] == null ||
            //    context.Session["ValidateCode"].ToString().ToLower() != RequestCheckCode)
            //{
            //    msg = "您输入的验证码不正确";
            //}
            else if (BLL.Util.GetMobileCheckCodeByCache(RequestMobile) != RequestMobileCheckCode)
            {
                msg = "您输入的手机短信验证码不正确";
            }
            //else if (EmployeeInfo.Instance.GetUserNameCount(RequestUsername.ToLower(), RequestCategory) > 0)
            //{
            //    msg = "该用户已存在，请重新输入！";
            //}
            else if (EmployeeInfo.Instance.GetMobileCount(RequestMobile, RequestCategory) <= 0)
            {
                msg = "该手机号未注册，请重新输入！";
            }
            else
            {
                flag = true;
            }

            return flag;
        }

        private void SendMobileMsg(HttpContext context, string sendSMS_ContentTemp, bool isCheckVCode = true)
        {
            string ret = string.Empty;
            if (isCheckVCode && (context.Session["ValidateCode"] == null ||
                context.Session["ValidateCode"].ToString().ToLower() != RequestCheckCode))
            {
                ret = "-9";//验证码无效
            }
            else
            if (BLL.Util.GetMobileCheckCodeTimesByCache(RequestMobile) > 2)
            {
                ret = "-10";//上次发送过验证码给到这个手机号，还没有过超时时间
            }
            else
            {
                string code = BLL.Util.GetMobileCheckCodeByCache(RequestMobile);
                if (code == null)
                {
                    Common.ValidateCode vc = new Common.ValidateCode();
                    code = vc.GetRandomCode(4, 1);
                }
                string content = string.Format(sendSMS_ContentTemp, code);
                SendMsgResult smr = SMSServiceHelper.Instance.SendMsgImmediately(RequestMobile, content);
                if (Convert.ToBoolean(smr.Result))
                //if (true)
                {
                    BLL.Util.AddWebCacheByMobile(RequestMobile, code);
                    ret = "0";//发送短信成功
                }
                else
                {
                    BLL.Loger.Log4Net.Info($"给{RequestMobile}手机，发送短信内容：{content}，失败，具体接口返回内容为：{smr.Message}");
                    ret = "-11";///发送短信失败
                }
            }
            context.Response.Write(ret.ToString());
        }

        private void Login(HttpContext context)
        {
            if ((RequestUsername != null && RequestUsername.Length > 0) && (RequestPWD != null && RequestPWD.Length > 0)
                 && (RequestCheckCode != null && RequestCheckCode.Length > 0)
                 && (RequestCategory > 0 || RequestIsInside == 1))
            {
                int ret = 0;
                string url = string.Empty;
                bool isRedirectURL = false;

                //账号
                string username = RequestUsername;
                if (context.Session["ValidateCode"] == null || context.Session["ValidateCode"].ToString().ToLower() != RequestCheckCode)
                {
                    ret = -6;
                }
                else
                {
                    if (RequestCategory == 29001 && RequestIsInside != 1)//广告主
                    {
                        //int dealerID = BLL.StockBroker.StockBroker.Instance.isStockBrokerUser(username);
                        //if (dealerID > 0)//dealerID，若大于0，说明是库存经纪人那边的账号，需要调用他们的登陆接口
                        //{
                        //    string msg = "";
                        //    LoginDto result = BLL.StockBroker.StockBroker.Instance.Login(username, RequestPWD, out msg);
                        //    if (result != null)
                        //    {
                        //        ret = dealerID;
                        //        if (result.status == 0)//账号停用时
                        //        {
                        //            isRedirectURL = true;
                        //            url = "http://j.chitunion.com/userInfo/toRecoverPWD";
                        //            ////物理清除StockBroker表数据
                        //            //BLL.StockBroker.StockBroker.Instance.DeleteByUserID(dealerID);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        context.Response.Write("-7," + BLL.Util.EscapeString(msg));
                        //        context.Response.End();
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        ret = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, RequestCategory);
                        //}
                    }
                    else if (RequestCategory == 29002 && RequestIsInside != 1)//媒体主
                    {
                        ret = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, RequestCategory);
                    }
                    //else if (RequestIsInside == 1)//内部用户
                    //{
                    //    ret = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, 29003);
                    //}

                    context.Session["ValidateCode"] = null;
                    if (ret > 0)//登陆成功
                    {
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(ret);
                        //    currentContext.Session["EmployeeNumber"] = BLL.Util.GetEmployeeNumberByUserID(ret);
                        //BLL.Util.LoginPassport(ret, sysID);
                        string gourl = ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath");//ConfigurationUtil.GetAppSettingValue("NotAccessMsgPagePath")

                        if (isRedirectURL)
                        {
                            context.Response.Write("-8," + BLL.Util.EscapeString(url));
                            context.Response.End();
                            return;
                        }
                        else if (!string.IsNullOrEmpty(RequestGourl))
                        {
                            gourl = RequestGourl;
                        }
                        else
                        {
                            switch (RequestCategory)
                            {
                                case 29001:
                                    gourl = "/manager/advertister/personal/PersonCenter.html";
                                    break;
                                case 29002:
                                    gourl = "/manager/media/personal/personalCenter.html";
                                    break;
                                default:
                                    gourl = "/usermanager/NotAccessMsgPage.html";
                                    break;
                            }

                            //if (RequestCategory == 29001 && ur != null && ur.IsAD)//广告主跳转统一这个页面
                            //{
                            //    gourl = "/OrderManager/wx_list.html";
                            //}
                            //else
                            //{
                            //    DataTable dtParent = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(ret);
                            //    if (dtParent != null && dtParent.Rows.Count > 0)
                            //    {
                            //        if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                            //        {
                            //            DataTable dtChild = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(ret, dtParent.Rows[0]["moduleid"].ToString());
                            //            if (dtChild.Rows.Count > 0)
                            //            {
                            //                gourl = dtChild.Rows[0]["url"].ToString();
                            //            }
                            //        }
                            //        else
                            //        {
                            //            gourl = dtParent.Rows[0]["url"].ToString();
                            //        }
                            //    }
                            //}
                        }
                        string content = string.Format("用户{1}(ID:{0})登录成功。", ret, username);
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.登陆, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, ThisSysID, ret);
                        ret = 1;//登陆成功
                        url = gourl;
                    }
                }
                context.Response.Write(ret.ToString() + "," + url);
                context.Response.End();

                return;
            }
        }

        private void Register(HttpContext context)
        {
            string msg = string.Empty;
            int result = -1;
            if (RequestUsername != null && RequestUsername.Length > 0 &&
                RequestPWD != null && RequestPWD.Length > 0 &&
                //RequestCheckCode != null && RequestCheckCode.Length > 0 &&
                RequestCategory > 0 &&
                RequestPWDConfirm != null && RequestPWDConfirm.Length > 0 &&
                RequestMobile != null && RequestMobile.Length > 0 &&
                RequestMobileCheckCode != null && RequestMobileCheckCode.Length > 0)
            {
                int ipCount = XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.GetCountByModuleType((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册, ThisSysID);

                if (ipCount > RegisterIPCount_Stop)//验证当同一个IP，当天注册量大于一定数量时，停止注册
                {
                    msg = "注册失败，请明天再试！";
                    result = -1;
                }
                else
                {
                    if (ipCount >= RegisterIPCount_Verify)//验证当同一个IP，当天注册量大于一定数量时，启动极验验证码逻辑
                    {
                        BLL.GeetestHelper geetest = new ITSC.Chitunion2017.BLL.GeetestHelper(GeetestConfig_ID, GeetestConfig_Key);
                        if (!geetest.VerifyCode(RequestGeetestChallenge, RequestGeetestValidate, RequestGeetestSeccode))
                        {
                            msg = "注册失败";
                            result = -1;
                        }
                    }
                    if (InsertVerifyLogic(context, out msg))
                    {
                        string pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(RequestPWD + RequestCategory + LoginPwdKey, System.Text.Encoding.UTF8);
                        int userID = EmployeeInfo.Instance.InsertUserInfo(RequestUsername, RequestMobile, pwd, RequestCategory);
                        if (userID > 0)
                        {
                            EmployeeInfo.Instance.InsertUserDetailAndRoleInfo(userID, RequestUsername, ListAddUserRoleIDs[RequestCategory], ThisSysID, userID);
                            string content = string.Format("用户{1}(ID:{0})注册成功。", userID, RequestUsername);
                            XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, ThisSysID, userID);
                            result = 0;
                            XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userID);
                            msg = (string.IsNullOrEmpty(RequestGourl) ? (RequestCategory == 29001 ? "/manager/advertister/personal/PersonCenter.html" : "/manager/media/personal/personalCenter.html") : RequestGourl);
                        }
                        else
                        {
                            msg = "注册失败";
                            result = -1;
                        }
                    }
                }
            }

            context.Response.Write("{ 'result':" + result + ",'msg':'" + msg + "'  }");
        }

        /// <summary>
        /// 插入DB前验证逻辑
        /// </summary>
        /// <returns></returns>
        private bool InsertVerifyLogic(HttpContext context, out string msg)
        {
            msg = string.Empty;
            bool flag = false;
            //Regex r = new Regex(@"(?!^[a-zA-Z]+$)(?!^[\d]+$)(?!^[^a-zA-Z-_\d]+$)^.{4,20}$");
            Regex r = new Regex(@"^[a-zA-Z][\w-]{3,19}$");
            Regex rhanzi = new Regex(@"[\u4e00-\u9fa5]");

            if (r.IsMatch(RequestUsername) == false ||
                (r.IsMatch(RequestUsername) && RequestUsername.IndexOf(" ") > 0) ||
                rhanzi.IsMatch(RequestUsername))
            {
                //msg = "用户名必须为字母、数字、\"_\"、\" - \"两种及以上组合的4-20个字符！";
                msg = "可使用字母、数字、\"_\"、\" - \"需以字母开头!";
            }
            else if (RequestPWD.Length < 6 || RequestPWD.Length > 20)
            {
                msg = "密码长度应为6~20位字符！";
            }
            else if (string.IsNullOrEmpty(RequestPWDConfirm) || RequestPWDConfirm != RequestPWD)
            {
                msg = "确认密码与密码不一致，请重新输入！";
            }
            else if (!BLL.Util.IsHandset(RequestMobile))
            {
                msg = "请填写正确的手机号";
            }
            //else if (context.Session["ValidateCode"] == null ||
            //    context.Session["ValidateCode"].ToString().ToLower() != RequestCheckCode)
            //{
            //    msg = "您输入的验证码不正确";
            //}
            else if (BLL.Util.GetMobileCheckCodeByCache(RequestMobile) != RequestMobileCheckCode)
            {
                msg = "您输入的手机短信验证码不正确";
            }
            else if (EmployeeInfo.Instance.GetUserNameCount(RequestUsername.ToLower(), RequestCategory) > 0)
            {
                msg = "该用户已存在，请重新输入！";
            }
            else if (EmployeeInfo.Instance.GetMobileCount(RequestMobile, RequestCategory) > 0)
            {
                msg = "该手机号码已存在，请重新输入！";
            }
            else
            {
                flag = true;
            }

            return flag;
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