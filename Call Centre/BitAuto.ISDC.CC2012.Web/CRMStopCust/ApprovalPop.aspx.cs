using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class ApprovalPop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string CRMStopCustApplyID
        {
            get
            {
                return HttpContext.Current.Request["CRMStopCustApplyID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CRMStopCustApplyID"].ToString());
            }
        }

        public string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString());
            }
        } 
        public string Type
        {
            get
            {
                return HttpContext.Current.Request["Type"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Type"].ToString());
            }
        }

        public string title = string.Empty;
        public string desc = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                title = Type == "1" ? "审核通过" : "驳回";
                desc = Type == "1" ? "审核意见" : "驳回理由";
            }
        }

    }
}