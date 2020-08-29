using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.WebService.CRM
{
    public class SYSUserInfoServiceHelper
    {
        SYSUserInfoServiceProxy proxy = new SYSUserInfoServiceProxy();
        #region Instance
        public static readonly SYSUserInfoServiceHelper Instance = new SYSUserInfoServiceHelper();
        #endregion

        /// <summary>
        /// 根据cookies的内容，获取登录用户ID
        /// </summary>
        /// <param name="cookies">集中权限系统登录是的cookies的内容</param>
        /// <returns>若能找到用户ID，则返回，否则返回int默认值</returns>
        public int GetUserIdByCookies(string cookies)
        {
           string useridStr = proxy.GetUserIdByCookies(cookies);
           int userid = Entities.Constants.Constant.INT_INVALID_VALUE;
           if (string.IsNullOrEmpty(useridStr)==false && 
               int.TryParse(useridStr,out userid))
           {
               return userid;
           }
           return Entities.Constants.Constant.INT_INVALID_VALUE;
        }
    }

   class SYSUserInfoServiceProxy : SYS.UserInfoService.RightsService
   {
       public SYSUserInfoServiceProxy()
       {
           string strTimeout = System.Configuration.ConfigurationManager.AppSettings["SYSUserInfoServiceTimeout"];
           int timeout = 0;
           this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

           string url = System.Configuration.ConfigurationManager.AppSettings["SYSUserInfoService"];
           if (string.IsNullOrEmpty(url) == false)
           {
               this.Url = url;
           }
       }
   }
}
