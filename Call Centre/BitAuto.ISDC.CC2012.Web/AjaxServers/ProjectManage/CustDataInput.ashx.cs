using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.Util;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// CustDataInput 的摘要说明
    /// </summary>
    public class CustDataInput : IHttpHandler, IRequiresSessionState
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
                string msg = "";
              //  BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
                {
                    CustInfoImportHelper helper = new CustInfoImportHelper();
                    BLL.Loger.Log4Net.Info("1:" + helper.Action);
                    switch (helper.Action)
                    {
                        case "batchimport":
                            string tipinfo = "";
                            AJAXHelper.WrapJsonResponse(helper.BatchImport(out msg, out tipinfo), tipinfo, msg);
                            break;

                        default:
                            AJAXHelper.WrapJsonResponse(false, "", "没有对应的操作");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("导入数据出错：---" + ex.Message + ";堆栈：" + ex.StackTrace + "；全：" + ex);
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