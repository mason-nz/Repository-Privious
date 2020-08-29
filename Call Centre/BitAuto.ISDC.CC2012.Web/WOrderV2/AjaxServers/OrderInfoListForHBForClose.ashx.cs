using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers
{
    /// <summary>
    /// OrderInfoListForHBForClose 的摘要说明
    /// </summary>
    public class OrderInfoListForHBForClose : IHttpHandler, IRequiresSessionState
    {
        public string Keyid
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("keyid");
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string name = "OrderInfoListForHB_AjaxList_" + Keyid;
            System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            if (webHttp != null && webHttp.Session[name] != null)
            {
                webHttp.Session.Remove(name);
            }
            context.Response.Write("success");
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