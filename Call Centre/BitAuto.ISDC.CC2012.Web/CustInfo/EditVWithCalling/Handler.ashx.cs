using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        CustInfoHelper helper = new CustInfoHelper();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool success = true;
                string result = "";
                string msg = "";

                switch (helper.Action)
                {
                    case "deleteccmember":
                        helper.DeleteCCMember();
                        break;
                    case "deletecccstmember":
                        helper.DeleteCCCstMember();
                        break;
                    default:
                        success = false;
                        msg = "请求参数错误";
                        break;
                }
                AJAXHelper.WrapJsonResponse(success, result, msg);
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