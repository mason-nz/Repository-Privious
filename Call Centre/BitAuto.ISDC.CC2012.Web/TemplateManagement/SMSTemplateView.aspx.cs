using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class SMSTemplateView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string RequestRecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }
        public string BGID = string.Empty;
        public string SCID = string.Empty;
        public string smstitle = string.Empty;
        public string smscontent = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(RequestRecID))
                {
                    int _recid = 0;
                    if (int.TryParse(RequestRecID, out _recid))
                    {
                        Entities.QuerySMSTemplate query = new Entities.QuerySMSTemplate();
                        query.RecID = _recid;
                        int rowcout = 0;
                        DataTable dt = BLL.SMSTemplate.Instance.GetSMSTemplate(query, "", 1, 1, out rowcout);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            BGID = dt.Rows[0]["bgname"].ToString();
                            SCID = dt.Rows[0]["scname"].ToString();
                            smstitle = dt.Rows[0]["Title"].ToString();
                            smscontent = dt.Rows[0]["Content"].ToString();
                        }
                    }
                }
            }
        }
    }
}