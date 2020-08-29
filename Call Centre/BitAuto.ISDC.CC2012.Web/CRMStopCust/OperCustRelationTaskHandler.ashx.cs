using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    /// <summary>
    /// OperCustRelationTaskHandler 的摘要说明
    /// </summary>
    public class OperCustRelationTaskHandler : IHttpHandler, IRequiresSessionState
    {

        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }
        private string CustID
        {
            get
            {
                return HttpContext.Current.Request["CustID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString());
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
        private string TaskType
        {
            get
            {
                return HttpContext.Current.Request["TaskType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TaskType"].ToString());
            }
        }
        private string LastOperTime
        {
            get
            {
                return HttpContext.Current.Request["LastOperTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["LastOperTime"].ToString());
            }
        }
        private string LastOperUserID
        {
            get
            {
                return HttpContext.Current.Request["LastOperUserID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["LastOperUserID"].ToString());
            }
        }
        private string ApplyType
        {
            get
            {
                return HttpContext.Current.Request["ApplyType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ApplyType"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            switch (Action)
            {
                case "CustLastOperTask":
                    break;
                case "GetApplyReason": GetApplyReason(out msg);
                    break;
                case "GetApplyRemark": GetApplyRemark(out msg);
                    break;
            }
            context.Response.Write(msg);
        }
        /// <summary>
        /// 取停用申请原因
        /// </summary>
        /// <param name="msg"></param>
        public void GetApplyReason(out string msg)
        {
            msg = string.Empty;
            DataTable dt = null;
            //如果是启用申请
            if (ApplyType == "2")
            {
                dt = BLL.Util.GetEnumDataTable(typeof(Entities.EnableApplyReason));
            }
            else
            {
                dt = BLL.Util.GetEnumDataTable(typeof(Entities.StopApplyReason));
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        msg += "[";
                    }
                    msg += "{value:'" + dt.Rows[i]["value"].ToString() + "',name:'" + dt.Rows[i]["Name"].ToString() + "'}";
                    if (i < dt.Rows.Count - 1)
                    {
                        msg += ",";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "]";
                    }
                }
            }
        }
        /// <summary>
        /// 取停用申请原因
        /// </summary>
        /// <param name="msg"></param>
        public void GetApplyRemark(out string msg)
        {
            msg = string.Empty;
            DataTable dt = null;
            //如果是启用申请
            if (ApplyType == "2")
            {
                dt = BLL.Util.GetEnumDataTable(typeof(Entities.EnableRemark));
            }
            else
            {
                dt = BLL.Util.GetEnumDataTable(typeof(Entities.StopRemark));
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        msg += "[";
                    }
                    msg += "{value:'" + dt.Rows[i]["value"].ToString() + "',name:'" + dt.Rows[i]["Name"].ToString() + "'}";
                    if (i < dt.Rows.Count - 1)
                    {
                        msg += ",";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "]";
                    }
                }
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