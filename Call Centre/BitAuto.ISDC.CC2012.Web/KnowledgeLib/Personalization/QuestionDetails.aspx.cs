using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    public partial class QuestionDetails : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string Id
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("id").ToString();
            }
        }
        public string txtTilte;
        public string txtType;
        public string txtContent;
        public string txtAnswer;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        
        private void BindData()
        {
            DataTable dt = BLL.Personalization.Instance.GetQuestionDetailsById(Id);
            if (dt != null && dt.Rows.Count > 0)
            {
                txtTilte = dt.Rows[0]["Title"].ToString();
                txtType = dt.Rows[0]["KCName"].ToString();
                txtContent = dt.Rows[0]["CONTENT"].ToString();
                txtAnswer = dt.Rows[0]["AnswerContent"].ToString();
            }

            repeaterList.DataSource = BLL.Personalization.Instance.GetQuestionOperationLogById(Id);
            repeaterList.DataBind();
        }
    }
}