using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ExamPaperStorage
{
    public partial class ExamPaperList : PageBase
    {
        public bool AddPaperButton = false;//添加试卷
        public bool AddQueryButton = false;//添加试题

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AddPaperButton = BLL.Util.CheckButtonRight("SYS024BUT3201");
                AddQueryButton = BLL.Util.CheckButtonRight("SYS024BUT3202");

                BindCatage();

                BindState();

                BindCreatUser();
              
            }
        }

        /// <summary>
        /// 绑定创建人
        /// </summary>
        private void BindCreatUser()
        {
            DataSet ds = BLL.ExamPaper.Instance.GetAllCreateUsers();
            if (ds != null && ds.Tables.Count > 0)
            {
                this.selCreateUser.DataValueField = "UserID";
                this.selCreateUser.DataTextField = "TrueName";
                this.selCreateUser.DataSource = ds.Tables[0];
                this.selCreateUser.DataBind();
                this.selCreateUser.Items.Insert(0, new ListItem("请选择", "-1"));
            }
        }

        /// <summary>
        /// 绑定状态
        /// </summary>
        private void BindState()
        {
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.ExamPaperState));
            RptState.DataSource = dt;
            RptState.DataBind();
        }

        /// <summary>
        /// 绑定分类
        /// </summary>
        private void BindCatage()
        {
            Entities.QueryExamCategory query = new Entities.QueryExamCategory();
            query.Type = 2;
            int totalCount = 0;

            DataTable dt = BLL.ExamCategory.Instance.GetExamCategory(query, "", 1, 999, out totalCount);
            if (dt != null)
            {
                RptCatage.DataSource = dt;
                RptCatage.DataBind();
            }
        }
    }
}