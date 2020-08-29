using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.CustCheck
{
    /// <summary>
    /// MemberCheckHandler 的摘要说明
    /// </summary>
    public class MemberCheckHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            string msg = "";
            MemberCheckHelper helper = new MemberCheckHelper();
            try
            {
                switch (helper.Action.ToLower())
                {
                    case "savecheckinfo"://保存核实信息
                        helper.SaveMemberInfo();
                        break;
                    case "submitcheckinfo"://提交核实信息
                        helper.SubmitMemberInfo();
                        break;
                    case "deletecheckinfo"://提交删除核实信息
                        helper.DeleteMemberInfo();
                        break;
                    case "checkauth":
                        helper.CheckUserAuthForMember();
                        break;
                    default:
                        msg = "请求参数错误";
                        break;
                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            HttpContext.Current.Response.Write(msg);
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