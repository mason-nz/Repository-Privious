using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Web.Channels;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.CustInfo
{
    public partial class VisitCustInfo : System.Web.UI.Page
    {
        public string LoginID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["loginid"]))
                {
                    return HttpContext.Current.Request["loginid"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestVisitID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["VisitID"]))
                {
                    return HttpContext.Current.Request["VisitID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestCSID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["CSID"]))
                {
                    return HttpContext.Current.Request["CSID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string URLTitle;
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
            }
        }
    }
}