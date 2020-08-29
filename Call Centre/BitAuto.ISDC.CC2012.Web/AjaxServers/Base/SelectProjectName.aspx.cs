using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Base
{
    public partial class SelectProjectName : PageBase
    {
        public string ProjectName
        {
            get { return Request.QueryString["ProjectName"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["ProjectName"].Trim()) : string.Empty; }
        }
        public int GroupLength = 5;
        public int PageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBinds();
            }

        }

        private void DataBinds()
        {

            //BLL.ProjectTask_ReturnVisit.Instance.GetJiCaiProjectByName();
            int totalCount;
            DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetJiCaiProjectByName(ProjectName, "", PageCommon.Instance.PageIndex, PageSize, out totalCount);
            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterFriendCustMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterFriendCustMappingList.DataBind();


            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);
        }
    }
}