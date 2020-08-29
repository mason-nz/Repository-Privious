using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    public partial class QuestionUpdate : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action").ToString();
            }
        }
        public string Id
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("id").ToString();
            }
        }
        public string txtTilte = "";
        public string txtType = "";
        public string txtContent = "";
        public string txtAnswer = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userID, "SYS024MOD3608"))
                {
                    if (Action == "update")
                    {
                        this.Title = "问题更新";
                    }
                    else if (Action == "answer")
                    {
                        this.Title = "问题解答";
                    }
                    BindData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }

        private void BindData()
        {
            DataTable dt = BLL.Personalization.Instance.GetQuestionDetailsById(Id);
            if (dt != null && dt.Rows.Count > 0)
            {
                hidKLRId.Value = dt.Rows[0]["Id"].ToString();
                txtTilte = dt.Rows[0]["Title"].ToString();
                txtType = dt.Rows[0]["KCName"].ToString();
                txtContent = dt.Rows[0]["CONTENT"].ToString();
                txtAnswer = dt.Rows[0]["AnswerContent"] == null ? "" : dt.Rows[0]["AnswerContent"].ToString().Trim();
            }

            repeaterList.DataSource = BLL.Personalization.Instance.GetQuestionOperationLogById(Id);
            repeaterList.DataBind();
        }
    }
}