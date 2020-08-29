using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class SMSSend : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        #region 参数

        public string CustName
        {
            get
            {
                return HttpContext.Current.Request["CustName"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString());
            }
        }
        public string CustSex
        {
            get
            {
                return HttpContext.Current.Request["CustSex"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CustSex"].ToString());
            }
        }
        public string Tel1
        {
            get
            {
                return HttpContext.Current.Request["Tel1"] == null ? 
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Tel1"].ToString());
            }
        }
        public string Tel2
        {
            get
            {
                return HttpContext.Current.Request["Tel2"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Tel2"].ToString());
            }
        }
        public string DMSCode
        {
            get
            {
                return HttpContext.Current.Request["DMSCode"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSCode"].ToString());
            }
        }
        public string DMSName
        {
            get
            {
                return HttpContext.Current.Request["DMSName"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSName"].ToString());
            }
        }
        public string DMSAddress
        {
            get
            {
                return HttpContext.Current.Request["DMSAddress"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSAddress"].ToString());
            }
        }
        public string DMSTel
        {
            get
            {
                return HttpContext.Current.Request["DMSTel"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSTel"].ToString());
            }
        }
        public string DMSCity
        {
            get
            {
                return HttpContext.Current.Request["DMSCity"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSCity"].ToString());
            }
        }
        public string DMSLevel
        {
            get
            {
                return HttpContext.Current.Request["DMSLevel"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSLevel"].ToString());
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                

            }
        }
    }
}