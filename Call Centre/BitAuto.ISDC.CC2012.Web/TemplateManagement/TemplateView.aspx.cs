using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TemplateView : PageBase
    {

        public string TTCode
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["ttcode"]) ? "" :
                     HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ttcode"]);
            }
        }

        public string TPageName = "";
        public string IsShowBtn = "";
        public string IsShowWorkOrderBtn = "";
        public string IsShowSendMsgBtn = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && TTCode != "")
            {
                Entities.QueryTPage query = new Entities.QueryTPage();
                query.TTCode = TTCode;

                int totalCount = 0;
                DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 999, out totalCount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    TPageName = dt.Rows[0]["TPName"].ToString();
                    IsShowBtn = dt.Rows[0]["IsShowBtn"].ToString();
                    IsShowWorkOrderBtn = dt.Rows[0]["IsShowWorkOrderBtn"].ToString();
                    IsShowSendMsgBtn = dt.Rows[0]["IsShowSendMsgBtn"].ToString();
                }
            }
        }
    }
}