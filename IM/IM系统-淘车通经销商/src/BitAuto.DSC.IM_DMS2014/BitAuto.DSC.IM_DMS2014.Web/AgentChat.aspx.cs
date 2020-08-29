using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.BLL;

namespace BitAuto.DSC.IM_DMS2014.Web
{
    public partial class AgentChat : System.Web.UI.Page
    {
        public string AgentId = "12";
        public int BGID = 12;
        public string WorkOrderUrl = ConfigurationManager.AppSettings["WorkOrderUrl"];
        public int TimeidleAgent = Convert.ToInt32(ConfigurationManager.AppSettings["TimeidleAgent"]) * 1000;

        protected void Page_Load(object sender, EventArgs e)
        {
            int nAid = BLL.Util.GetLoginUserID();
            AgentId = nAid.ToString();
            BGID = BLL.BaseData.Instance.GetAgentBGIDByUserID(nAid);
            if (!IsPostBack)
            {
                GetAllLableWithCM();
            }


        }

        private void GetAllLableWithCM()
        {
            DataTable dt = ComSentence.Instance.GetAllLableWithCM(BGID);
            Dictionary<int, string> dtLabel = new Dictionary<int, string>();
            StringBuilder sbReuslt = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                if (!dtLabel.ContainsKey(Convert.ToInt32(dr[0])))
                {
                    if (dtLabel.Keys.Count > 0)
                    {
                        sbReuslt.Append("</ul></li>");
                    }

                    sbReuslt.Append("<li style='position: relative;'>");
                    sbReuslt.Append(dr[1].ToString());
                    sbReuslt.Append("<div class='tip_arrow' style='top: -4px; left: 50px; z-index: 9999;'></div>");
                    sbReuslt.Append(
                        "<ul style='position: absolute; z-index: 999; left: 65px; top: -25px; border: #e0e0e0 1px solid; width: 200px; background: #FFF; border-radius: 10px; height:400px;overflow-y:auto;'>");
                    sbReuslt.Append(string.Format("<li class='item'>{0}</li>", dr[3]));
                    dtLabel.Add(Convert.ToInt32(dr[0]), dr[1].ToString());
                }
                else
                {
                    sbReuslt.Append(string.Format("<li class='item'>{0}</li>", dr[3]));
                }
            }
            sbReuslt.Append("</ul></li>");

            ulCM.InnerHtml = sbReuslt.ToString();
        }
    }
}