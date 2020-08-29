using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class AssignTask : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string TrueName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TrueName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TrueName"]);
            }
        }
        public string TaskIDS
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TaskIDS"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TaskIDS"]);
            }
        }
        #endregion
        public int GroupLength = 5;
        public int PageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlBussiGroupBind();
                BindData();
            }
        }
        /// <summary>
        /// 绑定处理人
        /// </summary>
        private void ddlBussiGroupBind()
        {
            ddlBussiGroup.DataSource = BLL.BusinessGroup.Instance.GetAllBusinessGroup();
            ddlBussiGroup.DataTextField = "Name";
            ddlBussiGroup.DataValueField = "BGID";
            ddlBussiGroup.DataBind();
            ddlBussiGroup.Value = "9";
            ddlBussiGroup.Disabled = true;
        }
        private void BindData()
        {
            int RecordCount = 0;
            QueryEmployeeSuper query = new QueryEmployeeSuper();
            //分页参数赋值
            query.BGID = 9;
            if (!string.IsNullOrEmpty(TrueName))
            {
                query.TrueName = TrueName;
            }
            //按条件找人：条件-部门，角色-
            DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);

            repterPersonlist.DataSource = dt;
            repterPersonlist.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        
        public string getLoginUserID()
        {
            return BLL.Util.GetLoginUserID().ToString();
        }
    }
}