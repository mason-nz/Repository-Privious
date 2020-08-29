using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustCategory
{
    public partial class ResetPwd : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string Email
        {
            get { return HttpContext.Current.Request["Email"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Email"].ToString()); }
        }
        public string MemberCode
        {
            get { return HttpContext.Current.Request["MemberCode"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["MemberCode"].ToString()); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}