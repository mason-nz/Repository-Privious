using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.EmployeeAgentExclusive
{
    /// <summary>
    /// Summary description for SetExclusive
    /// </summary>
    public class SetExclusive : IHttpHandler, IRequiresSessionState
    {

        public string UserIDs  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("UserIDs"); }
        }

        public string IsExclusive
        {
            get { return BLL.Util.GetCurrentRequestStr("IsExclusive"); }
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";

            string msg = string.Empty;
            string userid = UserIDs;
            string isexclusive = IsExclusive;

            //验证
            if (string.IsNullOrEmpty(isexclusive))
            {
                isexclusive = "0";
            }

            if (string.IsNullOrEmpty(userid))
            {
                context.Response.Write("false");
                return;
            }

            //
            bool flag = BLL.EmployeeAgent.Instance.SetEmployeeAgentExclusive(userid, isexclusive);
            msg = flag.ToString();

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