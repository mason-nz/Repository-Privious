using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public partial class SendSMSPoper : PageBase
    {
        public string DMSCode
        {
            get
            {
                return HttpContext.Current.Request["DMSCode"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSCode"].ToString());
            }
        }
        public string CustName
        {
            get
            {
                return HttpContext.Current.Request["CustName"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString());
            }
        }
        public string DMSTEL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(DMSCode))
                {
                     int memberCode = 0;
                     if (int.TryParse(DMSCode, out memberCode))
                     {
                         DataSet ds = WebService.DealerInfoServiceHelper.Instance.GetDealer400(memberCode);
                         if (ds != null && ds.Tables[0].Rows.Count > 0)
                         {
                             DMSTEL = ds.Tables[0].Rows[0][2].ToString();
                         }
                     }
                }
            }
        }
    }
}