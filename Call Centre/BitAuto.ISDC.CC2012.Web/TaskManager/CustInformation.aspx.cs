using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.TaskManager
{
    public partial class CustInformation : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string RequestCustType
        {
            get { return HttpContext.Current.Request["CustType"] == null ? "" : HttpContext.Current.Request["CustType"].ToString(); }
        }
        public string RequestCustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }
        public string RequestCallRecordID
        {
            get { return HttpContext.Current.Request["CallRecordID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CallRecordID"].ToString()); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustInfoViewControl.CustID = RequestCustID;
                CustInfoViewControl.NeedLinkToCustinfo = false;
            }
        }
    }
}