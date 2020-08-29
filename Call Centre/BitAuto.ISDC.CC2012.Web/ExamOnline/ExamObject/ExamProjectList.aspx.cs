using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
namespace BitAuto.ISDC.CC2012.Web.ExamObject
{
    public partial class ExamProjectList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CateBind();

            //int userId = BLL.Util.GetLoginUserID();
            //DataTable dtArea = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUseBusinessGroup(userId); ;
            //DataRow row = dtArea.NewRow();
            //row[0] = "-1";
            //row[1] = "请选择";
            //dtArea.Rows.InsertAt(row, 0);

            //rp_AreaOptions.DataSource = dtArea;
            //rp_AreaOptions.DataBind();

            BindCreatUser();
        }

        #region 绑定项目分类
        public void CateBind()
        {
            DataTable dt = new DataTable();
            QueryExamCategory query = new QueryExamCategory();
            query.Type = 1;
            int total = 0;
            dt = BLL.ExamCategory.Instance.GetExamCategory(query, " CreateTime", 1, 10, out total);
            Rpt_Cate.DataSource = dt;
            Rpt_Cate.DataBind();
        }
        #endregion

        #region 绑定创建人
        /// <summary>
        /// 绑定创建人
        /// </summary>
        private void BindCreatUser()
        {
            DataSet ds = BLL.ExamInfo.Instance.GetAllCreateUsers();
            if (ds != null && ds.Tables.Count > 0)
            {
                Rpt_CreateUser.DataSource = ds.Tables[0];
                Rpt_CreateUser.DataBind();
            }
        }
        #endregion
    }
}