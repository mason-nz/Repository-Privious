using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCKLOptionLogList : System.Web.UI.UserControl
    {
        public string KLID
        {
            get
            {
                if (HttpContext.Current.Request["kid"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["kid"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindList();
            }
        }
        public void DataBindList()
        {
            if (!string.IsNullOrEmpty(KLID))
            {
                QueryKLOptionLog query = new QueryKLOptionLog();
                query.KLID = Convert.ToInt32(KLID);
                int totalcount = 0;
                DataTable dt = null;
                dt = BLL.KLOptionLog.Instance.GetKLOptionLog(query, string.Empty, 1, 1000000, out totalcount);
                this.repeaterTableList.DataSource = dt;
                this.repeaterTableList.DataBind();
            }


        }
    }
}