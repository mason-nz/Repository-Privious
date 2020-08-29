using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.Util;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage.CrmCustImport
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
                if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                {
                    BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                    CRMCustImportHelper helper = new CRMCustImportHelper();
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
                AJAXHelper.WrapJsonResponse(false, "", BLL.Util.EscapeString(ex.Message));
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