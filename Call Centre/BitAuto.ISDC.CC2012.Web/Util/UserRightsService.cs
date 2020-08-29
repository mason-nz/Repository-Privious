using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.Util
{
    public class UserRightsService
    {
        /// <summary>
        /// 根据cookies的内容，获取登录用户ID
        /// </summary>
        /// <param name="cookiesContent">集中权限系统登录是的cookies的内容</param>
        /// <returns>若能找到用户ID，则返回，否则返回int默认值</returns>
        public static int CheckLoginUserIDByCookies(string cookiesContent)
        {
            int userid = BitAuto.ISDC.CC2012.WebService.CRM.SYSUserInfoServiceHelper.Instance.GetUserIdByCookies(cookiesContent);
            if (userid <= 0)
            {
                BLL.Loger.Log4Net.Warn("集中权限系统登录失败，cookiesContent的内容" + cookiesContent + "; userid="+userid);
                AJAXHelper.WrapJsonResponse(false, "您还没有登录,请先登录!", "<script>alert('您还没有登录,请先登录!');top.location.href='/login.aspx?gourl='+encodeURIComponent(window.location);</script>");
              
            }
            return userid;
        }

        
    }
}