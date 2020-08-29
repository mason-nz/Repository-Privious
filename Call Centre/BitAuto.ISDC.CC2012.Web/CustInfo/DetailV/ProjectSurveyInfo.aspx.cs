using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class ProjectSurveyInfo : System.Web.UI.Page
    {
        public string RequestProjectID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["TaskID"]))
                {
                    return "";
                }
                else
                {
                    return Request["TaskID"];
                }
            }
        }
        public DataTable dt = null;
        public string str = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(RequestProjectID))
                {
                    dt = BLL.ProjectSurveyMapping.Instance.GetSurveyinfoByTaskID(RequestProjectID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            str += "<a href='SurveyInfo.aspx?SIID='" + dt.Rows[i]["SIID"].ToString() + "'+'&ProJectID='" + dt.Rows[i]["ProjectID"].ToString() + "'+'&TaskID='" + dt.Rows[i]["PTID"].ToString() + "' target='_blank'>" + dt.Rows[i]["Name"].ToString() + "</a>&nbsp;&nbsp;";
                        }
                    }
                }

            }
        }
    }
}