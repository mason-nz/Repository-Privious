using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCFAQList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (KnowledgeID != string.Empty)
            {//FAQ数据绑定
                DataTable dt = new DataTable();
                QueryKLFAQ query = new QueryKLFAQ();
                query.KLID = long.Parse(KnowledgeID);
                int totalCount = 0;
                dt = BLL.KLFAQ.Instance.GetKLFAQ(query, "", 1, 100,out totalCount);
                Rt_FAQList.DataSource = dt;
                Rt_FAQList.DataBind();
            }
        }

        public bool IsStart()
        {
            if (KnowledgeID == "")
            {
                return true;
            }
            else if (!BLL.KLFAQ.Instance.IsHaveFAQ(KnowledgeID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string KnowledgeID
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
    }
}