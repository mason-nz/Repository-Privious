using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class EasypassRestPwdServiceHelper
    {
        EasypassRestPwdServiceProxy proxy = new EasypassRestPwdServiceProxy();
        //private string EasypassRestPwdServiceURL = System.Configuration.ConfigurationManager.AppSettings["EasypassRestPwdServiceURL"].ToString();//服务URL

        #region Instance
        public static readonly EasypassRestPwdServiceHelper Instance = new EasypassRestPwdServiceHelper();
        #endregion

        /// <summary>
        /// 根据经销商id查询用户的登录名
        /// </summary>
        /// <param name="dealerID">经销商id</param>
        /// <returns>返回用户的登录名</returns>
        public string GetAccountLoginNameByDealerId(int dealerID)
        {
            //return (string)WebServiceHelper.InvokeWebService(EasypassRestPwdServiceURL, "GetAccountLoginNameByDealerId", new object[] { dealerID });
            return proxy.GetAccountLoginNameByDealerId(dealerID);
        }

        /// <summary>
        /// 根据经销商id更新用户的登录名
        /// </summary>
        /// <param name="dealerID">经销商id</param>
        /// <param name="loginName">将要更新的用户登录名</param>
        /// <returns></returns>
        public string UpdateAccountLoginNameByDealerId(int dealerID, string loginName)
        {
            //return (string)WebServiceHelper.InvokeWebService(EasypassRestPwdServiceURL, "UpdateAccountLoginNameByDealerId", new object[] { dealerID, loginName });
            return proxy.UpdateAccountLoginName(dealerID, loginName);
        }

        /// <summary>
        /// 根据经销商id，重置密码，并发会员指定邮箱中
        /// </summary>
        /// <param name="MemberCode">经销商id</param>
        /// <returns>返回</returns>
        public bool GetLinkByUserLoginName(string memberCode)
        {
            string userLoginName = GetAccountLoginNameByDealerId(int.Parse(memberCode));
            //return (bool)WebServiceHelper.InvokeWebService(EasypassRestPwdServiceURL, "GetLinkByUserLoginName", new object[] { userLoginName });
            return proxy.GetLinkByUserLoginName(userLoginName);
        }
    }

    class EasypassRestPwdServiceProxy : EasyPass.RestPwdService.UserGetPwd
    {
        public EasypassRestPwdServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["EasypassRestPwdServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["EasypassRestPwdServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
