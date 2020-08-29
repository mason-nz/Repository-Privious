using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;
namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class SelectEmployeePop : PageBase
    {
        #region 属性

        public string Requestname
        {
            get { return Request.QueryString["name"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["name"].Trim()) : string.Empty; }
        }
        public string RequestBrandIDs
        {
            get { return Request.QueryString["CustIDs"] != null ? Request.QueryString["CustIDs"].Trim() : string.Empty; }
        }
        public int GroupLength = 5;
        public int PageSize = 10;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT140102"))  //添加“任务列表--客户回访问”回收功能验证逻辑
                {
                    DataBinds();
                }
                else
                {
                    Response.Redirect(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }

        }

        private void DataBinds()
        {
            int totalCount;
            DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetCustUserForCC(RequestBrandIDs, " TrueName asc", PageCommon.Instance.PageIndex, PageSize, out totalCount);

            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterFriendCustMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterFriendCustMappingList.DataBind();

            litPagerDown1.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 3);

        }


    }
}