using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            CustBaseInfoHelper helper = new CustBaseInfoHelper();
            string msg = string.Empty;
            switch (helper.Action.ToLower())
            {
                case "getdepartid":
                    helper.GetAreaDistrictID(out msg);
                    break;
                case "sendsms":
                    helper.SendSMSToPeople(out msg);
                    break;
            }
            context.Response.Write(msg);
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