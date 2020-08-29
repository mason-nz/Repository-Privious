using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class RejectReason : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性 
        public string RequestKLID
        {
            get { return HttpContext.Current.Request["KLID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KLID"].ToString()); }
        }
        public string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()).ToLower(); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {  
        }
    }
}