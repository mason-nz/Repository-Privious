using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    /// <summary>
    /// B_ProjectReport1 的摘要说明
    /// </summary>
    public class B_ProjectReport1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            
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