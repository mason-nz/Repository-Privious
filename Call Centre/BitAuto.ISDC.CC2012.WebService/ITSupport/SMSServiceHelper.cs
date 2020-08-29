using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebService
{
    public class SMSServiceHelper
    {
        SMSServiceProxy proxy = new SMSServiceProxy();

        #region Instance
        public static readonly SMSServiceHelper Instance = new SMSServiceHelper();
        //private string SMSServiceURL = System.Configuration.ConfigurationManager.AppSettings["SMSServiceURL"];//发送手机短信接口
        private string SMSKey = System.Configuration.ConfigurationManager.AppSettings["SMSKey"];//发送手机短信Key
        const string ProductCode = "6116";//组合key的前缀字符
        #endregion

        //#region Contructor
        //protected SMSServiceHelper()
        //{ }
        //#endregion

        /// <summary>
        /// md5加密算法 
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="content">发送内容</param>
        /// <returns></returns>
        public string MixMd5(string mobile, string content)
        {
            return proxy.MixMd5(ProductCode + mobile + content + SMSKey);
        }

        /// 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNums">手机号码</param>
        /// <param name="content">发送手机短信内容</param>
        /// <param name="dt">发送时间(当前时间+1小时)</param>
        /// <param name="md5">MD5加密Key</param>
        /// <returns></returns>
        public int SendMsgImmediately(string phoneNums, string content, DateTime dt, string md5)
        {
            return proxy.SendMsgImmediately(ProductCode, phoneNums, content, string.Empty, dt, md5);
        }
    }

    class SMSServiceProxy : ITSupport.SMSService.MsgService
    {
        public SMSServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["SMSServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["SMSServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
