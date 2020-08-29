using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.Statistics.MemberIDImport
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 集中权限系统登录是的cookies的内容
        /// </summary>
        public string LoginCookiesContent
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LoginCookiesContent");
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                {
                    MemberIDImportHelper helper = new MemberIDImportHelper();
                    switch (helper.Action)
                    {
                        case "batchimport":
                            string msg = "";
                            AJAXHelper.WrapJsonResponse(helper.BatchImport(out msg), "", msg);
                            break;
                        default:
                            AJAXHelper.WrapJsonResponse(false, "", "没有对应的操作");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                AJAXHelper.WrapJsonResponse(false, "", ex.Message);
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