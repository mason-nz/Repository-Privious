using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class QuestionAnsweredDetails : BitAuto.ISDC.CC2012.Web.Base.PageBase

    {
        public int RecordCount;
        public string NotAnswerCount;
        public int PageSize = 10;
        public int GroupLength = 8;
        public string SelType
        {
                get { return HttpContext.Current.Request["SelType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SelType"].ToString()); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3502"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }

        #region 数据列表绑定
        public void BindData()
        {
            KLRaiseQuestions query = new KLRaiseQuestions();
            query.CreateUserId = BLL.Util.GetLoginUserID();
            if (SelType == "2")  //未解答
            {
                query.Status = 0;
            }
 
            DataTable dt = BLL.Personalization.Instance.GetKLRaiseQuestionData(query, "CreateDate DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                NotAnswerCount = dt.Rows[0]["notAnswer"].ToString();
            }
            Rt_Question.DataSource = dt;
            Rt_Question.DataBind();


            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }
        #endregion
    }
}