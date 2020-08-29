using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamScoreManagement
{
    public partial class ExamScoreList : PageBase
    {
        public bool ExportButton = false;//导出
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExportButton = BLL.Util.CheckButtonRight("SYS024BUT3203");
                BindCatage();

                //int userid = BLL.Util.GetLoginUserID();
                //DataTable db = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUseBusinessGroup(userid);
                //Rpt_Group.DataSource = db;
                //Rpt_Group.DataBind();
            }
        }

        /// <summary>
        /// 绑定分类
        /// </summary>
        private void BindCatage()
        {
            QueryExamCategory query = new QueryExamCategory();
            query.Type = 1;
            int total = 0;
            DataTable dt = BLL.ExamCategory.Instance.GetExamCategory(query, "", 1, 100000, out total);
            if (dt != null)
            {
                RptCatage.DataSource = dt;
                RptCatage.DataBind();
            }
        }
    }
}