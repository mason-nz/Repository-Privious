using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    /// <summary>
    /// OperHandler 的摘要说明
    /// </summary>
    public class OperHandler : IHttpHandler, IRequiresSessionState
    {

        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        private string TaskIDS
        {
            get
            {
                return HttpContext.Current.Request["TaskIDS"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TaskIDS"].ToString());
            }
        }

        private string AssignUserID
        {
            get
            {
                return HttpContext.Current.Request["AssignUserID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AssignUserID"].ToString());
            }
        }

        private string CRMStopCustApplyID
        {
            get
            {
                return HttpContext.Current.Request["CRMStopCustApplyID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CRMStopCustApplyID"].ToString());
            }
        }

        private string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString());
            }
        }

        //type=1:审批通过；type=2:驳回
        private string Type
        {
            get
            {
                return HttpContext.Current.Request["Type"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Type"].ToString());
            }
        }

        private string Reason
        {
            get
            {
                return HttpContext.Current.Request["Reason"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Reason"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            Oper o = new Oper();
            OperApproval oa = new OperApproval();
            switch (Action)
            {
                case "AssignTask":
                    o.AssignTaskByUseid(TaskIDS, AssignUserID, out msg);
                    break;
                case "RecedeTask":
                    o.RecedeTask(TaskIDS, AssignUserID, out msg);
                    break;
                case "OperDeal":
                    oa.OperDeal(int.Parse(CRMStopCustApplyID), TaskID, int.Parse(Type), Reason, out msg);
                    break;
            }
            context.Response.Write("{" + msg + "}");
        }

        private void IsOper()
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