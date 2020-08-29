using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.Utils.Config;
using System.Data;
using BLL = XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.WebService.ITSupport;
using XYAuto.BUOC.Chitunion2018.WebWeChat.ajaxservers;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.BUOC.Chitunion2018.WebWeChat.AjaxServers
{
    /// <summary>
    /// LoginManager 的摘要说明
    /// </summary>
    public class LoginManager : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        protected string SendSMS_ModifyMobileContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_ModifyMobileContentTemp");
        protected string SendSMS_WithdrawContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_WithdrawContentTemp");
        protected string SendSMS_RegisterContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_RegisterContentTemp");
        //protected string SendSMS_LoginContentTemp = ConfigurationUtil.GetAppSettingValue("SendSMS_LoginContentTemp");

        #region 属性
        private string RequestAction
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr("action"); }
        }

        private string RequestMobile
        {
            get { return BLL.Util.GetCurrentRequestFormStr("mobile"); }
        }
        private string RequestMobileCheckCode
        {
            get { return BLL.Util.GetCurrentRequestFormStr("mobileCheckCode"); }
        }
        private string RequestSaveMobile
        {
            get { return BLL.Util.GetCurrentRequestFormStr("Mobile"); }
        }
        //private string RequestSaveValidateCode
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("ValidateCode"); }
        //}
        //private int RequestSaveType
        //{
        //    get { return BLL.Util.GetCurrentRequestFormInt("Type"); }
        //}
        //private string RequestSaveTrueName
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("TrueName"); }
        //}
        //private string RequestSaveBLicenceURL
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("BLicenceURL"); }
        //}
        //private string RequestSaveIdentityNo
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("IdentityNo"); }
        //}
        //private string RequestSaveIDCardFrontURL
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("IDCardFrontURL"); }
        //}
        //private string RequestSaveAccountName
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("AccountName"); }
        //}
        //private int RequestSaveAccountType
        //{
        //    get { return BLL.Util.GetCurrentRequestFormInt("AccountType"); }
        //}

        #endregion 属性

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;

            switch (RequestAction.ToLower().Trim())
            {
                case "sendmobilemsg_modifymobile":
                    SendMobileMsg(context, SendSMS_ModifyMobileContentTemp);
                    break;
                case "sendmobilemsg_register":
                    SendMobileMsg(context, SendSMS_RegisterContentTemp);
                    break;
                case "sendmobilemsg_withdraw":
                    SendMobileMsg(context, SendSMS_WithdrawContentTemp);
                    break;
                //case "login":
                //    SendMobileMsg(context, SendSMS_LoginContentTemp);
                //    break;
                case "checksmscode":
                    CheckSMSCode(context);
                    break;
                case "saveattestation":
                    SaveAttestation(context);
                    break;
                default:
                    break;
            }
            context.Response.End();
        }
        private void SendMobileMsg(HttpContext context, string sendSMS_ContentTemp)
        {
            string ret = string.Empty;
            if (!BLL.Util.IsHandset(RequestMobile))
            { ret = "-12"; }
            else
            {
                if (RequestAction.ToLower().Trim() == "sendmobilemsg_register" && XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.IsAddedMobile(RequestMobile.Trim()))
                {
                    ret = "-13";
                }
                else
                {
                    if (BLL.Util.GetMobileCheckCodeTimesByCache(RequestMobile) > 2)
                    {
                        ret = "-10";//上次发送过验证码给到这个手机号，还没有过超时时间
                    }
                    else
                    {
                        string code = BLL.Util.GetMobileCheckCodeByCache(RequestMobile);
                        if (code == null)
                        {
                            ValidateCode vc = new ValidateCode();
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
                }
            }
            context.Response.Write(ret.ToString());
        }
        private void CheckSMSCode(HttpContext context)
        {
            string msg = string.Empty;
            int result = -1;
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
            context.Response.Write("{ \"result\":" + result + ",\"msg\":\"" + msg + "\"}");
        }
        public void Login(HttpContext context)
        {
            string msg = string.Empty;
            int result = -1;
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
                int userId = BLL.UserManage.UserManage.Instance.GetUserIdByMobile(RequestMobile, (int)UserCategoryEnum.媒体主);
                if (userId > 0)
                {
                    string content = string.Format("用户{1}(ID:{0})登录成功。", userId, RequestMobile);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.登陆, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, "", userId);
                    XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
                }
                else
                {
                    var ip = System.Web.HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
                    userId = BLL.UserManage.UserManage.Instance.InsertUser(RequestMobile, ip);
                    if (userId <= 0)
                    {
                        msg = "登录失败，请稍后重试";
                    }
                    else
                    {
                        XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
                        string content = string.Format("用户{1}(ID:{0})注册成功。", userId, RequestMobile);
                        XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog((int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.注册, (int)XYAuto.ITSC.Chitunion2017.Common.LogInfo.ActionType.Add, content, "", userId);
                    }
                }
            }
            else
            {
                msg = "手机号或验证码错误";
            }
            if (string.IsNullOrEmpty(msg))
                result = 0;
            context.Response.Write("{ \"result\":" + result + ",\"msg\":\"" + msg + "\"}");
        }
        private void SaveAttestation(HttpContext context)
        {
            int status = 0;
            try
            {
                string msg = string.Empty;
                if (RequestSaveMobile != null && RequestSaveMobile.Length > 0 &&
        RequestMobileCheckCode != null && RequestMobileCheckCode.Length > 0)
                {
                    if (!BLL.Util.IsHandset(RequestSaveMobile))
                    {
                        msg = "请填写正确的手机号";
                    }
                    else if (BLL.Util.GetMobileCheckCodeByCache(RequestSaveMobile) != RequestMobileCheckCode)
                    {
                        msg = "您输入的手机短信验证码不正确";
                    }
                }
                else
                {
                    msg = "手机号或验证码错误";
                }

                if (string.IsNullOrEmpty(msg))
                {
                    ModifyAttestationReqDto dto = new ModifyAttestationReqDto();
                    msg = BLL.UserManage.UserManage.Instance.SaveMobile(RequestSaveMobile.Trim());
                }
                if (!string.IsNullOrEmpty(msg))
                    status = -1;
                else msg = "ok";
                context.Response.Write("{ \"result\":" + status + ",\"msg\":\"" + msg + "\"}");
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"用户保存失败：", ex);
                context.Response.Write("{ \"result\":-2,\"msg\":\" 保存失败，请重试\"}");
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