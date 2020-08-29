using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class ProjectLog : PageBase
    {
        public int PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        public string ProjectID
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["projectid"] == null)
                {
                    return "";
                }
                int intVal = 0;

                if (!int.TryParse(HttpContext.Current.Request.QueryString["projectid"], out intVal))
                {
                    return "";
                }
                else
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["projectid"].ToString());
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            long projectid = CommonFunction.ObjectToLong(ProjectID);
            DataTable dt = BLL.ProjectLog.Instance.GetProjectLog(projectid, BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}