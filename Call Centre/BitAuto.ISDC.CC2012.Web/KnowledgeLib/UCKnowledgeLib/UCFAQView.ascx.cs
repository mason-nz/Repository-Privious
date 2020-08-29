using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCFAQView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (KnowledgeID != "")
            {//FAQ数据绑定
                DataTable dt = new DataTable();
                QueryKLFAQ query = new QueryKLFAQ();
                long nKnowledgeID = -1;
               if(long.TryParse(KnowledgeID,out nKnowledgeID));
                {
                    query.KLID = nKnowledgeID;
                    int totalCount = 0;
                    dt = BLL.KLFAQ.Instance.GetKLFAQ(query, "", 1, 100, out totalCount);
                    dt.Columns.Add("Num");
                    int num = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Num"] = num.ToString();
                        num++;
                    }
                    Rt_FAQList.DataSource = dt;
                    Rt_FAQList.DataBind();
                }
               
            }
        }

        public bool IsStart()
        {
            if (KnowledgeID == string.Empty)
            {
                return false;
            }
            else if (BLL.KLFAQ.Instance.IsHaveFAQ(KnowledgeID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  知识点ID
        /// </summary>
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