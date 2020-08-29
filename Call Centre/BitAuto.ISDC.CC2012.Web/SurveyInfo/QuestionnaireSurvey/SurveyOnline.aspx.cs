using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.QuestionnaireSurvey
{
    public partial class SurveyOnline :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {  
        }

        //绑定分类
        public string BindCategory()
        {
            string categoryStr = string.Empty;

            Entities.QuerySurveyCategory query = new Entities.QuerySurveyCategory();
            query.GroupName = "质检培训组";
            query.TypeId = 1;

            int count;

            DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "", 1, 10000, out count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                categoryStr += "<span>";
                categoryStr += "<input name='examCategory' type='checkbox' value='" + dt.Rows[i]["SCID"].ToString() + "' />";
                categoryStr += "<em onclick='emChkIsChoose(this)'>" + dt.Rows[i]["Name"].ToString() + "</em>";
                categoryStr += "</span>";
            }

            return categoryStr;
        }
    }
}