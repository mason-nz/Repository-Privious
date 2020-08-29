using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.Utils.Config;

namespace XYAuto.BUOC.ChiTuData2017.Web.AjaxServers
{
    /// <summary>
    /// LoginManager 的摘要说明
    /// </summary>
    public class LoginManager : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        protected string ThisSysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        //protected string AddUserRoleIDs = ConfigurationUtil.GetAppSettingValue("AddUserRoleIDs");
        //protected string SendSMS_RegisterContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_RegisterContentTemp");
        //protected string SendSMS_ForgetPwdContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_ForgetPwdContentTemp");
        //protected string SendSMS_ModifyMobileContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_ModifyMobileContentTemp");
        protected string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        //private Dictionary<int, string> ListAddUserRoleIDs = new Dictionary<int, string>();

        #region 属性
        private string RequestAction
        {
            get { return BLL.Util.GetCurrentRequestFormStr("action"); }
        }
        private string RequestUsername
        {
            get { return BLL.Util.GetCurrentRequestFormStr("username"); }
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
            get { return BLL.Util.GetCurrentRequestFormStr("checkCode"); }
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
        private string RequestOldPwd
        {
            get { return BLL.Util.GetCurrentRequestStr("oldPwd"); }
        }
        private string RequestNewPwd
        {
            get { return BLL.Util.GetCurrentRequestStr("newPwd"); }
        }
        private string RequestNewPwdConfirm
        {
            get { return BLL.Util.GetCurrentRequestStr("newPwdConfirm"); }
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
                case "modifypwd":
                    ModifyPwd(context);
                    break;
                default:
                    break;
            }
            context.Response.End();
        }

        private void ModifyPwd(HttpContext context)
        {
            string msg = string.Empty;
            int result = -1;
            if (RequestOldPwd != null && RequestOldPwd.Length > 0 &&
                RequestNewPwd != null && RequestNewPwd.Length > 0)
            {
                if (ModifyPwdVerifyLogic(context, out msg))
                {
                    //string pwd = XYAuto.Utils.Security.DESEncryptor.MD5Hash(RequestPWD + 29004 + LoginPwdKey, System.Text.Encoding.UTF8);
                    //int ret = EmployeeInfo.Instance.UpdatePwdByCategoryAndMobile(RequestCategory, RequestMobile, RequestPWD);
                    bool ret = XYAuto.ITSC.Chitunion2017.Common.UserInfo.ModifyPwd(RequestNewPwd);
                    string content = string.Format("用户ID：{0}，重置密码{1}。", XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID(), (ret ? "成功" : "失败"));
                    if (XYAuto.ITSC.Chitunion2017.Common.UserInfo.ModifyPwd(RequestNewPwd))
                    {
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Modify, content, string.Empty, 0);
                        result = 0;
                        msg = RequestGourl;
                    }
                    else
                    {
                        XYAuto.BUOC.ChiTuData2017.BLL.BLLLoger.Log4Net.Info(content);
                        msg = "用户重置密码失败";
                        result = -1;
                    }
                }
            }

            context.Response.Write("{ 'result':" + result + ",'msg':'" + msg + "'  }");
        }

        private bool ModifyPwdVerifyLogic(HttpContext context, out string msg)
        {
            msg = string.Empty;
            bool flag = false;
            //Regex r = new Regex(@"(?!^[a-zA-Z]+$)(?!^[\d]+$)(?!^[^a-zA-Z-_\d]+$)^.{4,20}$");

            if (string.IsNullOrEmpty(RequestOldPwd))
            {
                msg = "旧密码不能为空";
            }
            else if (!XYAuto.ITSC.Chitunion2017.Common.UserInfo.VerifyPwd(RequestOldPwd))
            {
                msg = "旧密码输入不正确";
            }
            else if (RequestNewPwd.Length < 6 || RequestNewPwd.Length > 20)
            {
                msg = "密码长度应为6~20位字符！";
            }
            else if (string.IsNullOrEmpty(RequestNewPwdConfirm) || RequestNewPwdConfirm != RequestNewPwd)
            {
                msg = "确认密码与密码不一致，请重新输入！";
            }
            else
            {
                flag = true;
            }

            return flag;
        }

        private void Login(HttpContext context)
        {
            if ((RequestUsername != null && RequestUsername.Length > 0) && (RequestPWD != null && RequestPWD.Length > 0)
                 && (RequestCheckCode != null && RequestCheckCode.Length > 0))
            {
                int ret = 0;
                string url = string.Empty;
                bool isRedirectURL = false;

                //账号
                string username = RequestUsername;
                if (context.Session["ValidateCode"] == null || context.Session["ValidateCode"].ToString().ToLower() != RequestCheckCode.ToLower())
                {
                    ret = -6;
                }
                else
                {
                    ret = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Login(username, RequestPWD, 29004);
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
                            DataTable dtParent = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(ret);
                            if (dtParent != null && dtParent.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                                {
                                    DataTable dtChild = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(ret, dtParent.Rows[0]["moduleid"].ToString());
                                    if (dtChild.Rows.Count > 0)
                                    {
                                        gourl = dtChild.Rows[0]["url"].ToString();
                                    }
                                }
                                else
                                {
                                    gourl = dtParent.Rows[0]["url"].ToString();
                                }
                            }
                        }
                        string content = string.Format("用户{1}(ID:{0})登录成功。", ret, username);
                        ret = 1;//登陆成功
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Select, content, string.Empty, 0);
                        url = gourl;
                    }
                }
                context.Response.Write(ret.ToString() + "," + url);
                context.Response.End();

                return;
            }
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