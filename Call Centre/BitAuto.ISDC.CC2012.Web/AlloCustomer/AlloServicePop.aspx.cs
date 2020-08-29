using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AlloCustomer
{
    public partial class AlloServicePop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        private string RequestBGID
        {
            get { return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString()); }
        }
        private string RequestName
        {
            get { return HttpContext.Current.Request["Name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Name"].ToString()); }
        }

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

        private void BindData()
        {
            int RecordCount = 0;
            //按条件找人：条件-部门，角色-
            DataTable dt = BLL.EmployeeAgent.Instance.GetKeFuByYiJiKe(RequestName, RequestBGID, "ui.UserID", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);

            repterPersonlist.DataSource = dt;
            repterPersonlist.DataBind();

            litPagerDown1.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 4);
        }

        private void ddlBussiGroupBind()
        {
            ddlBussiGroup.DataSource = BLL.BusinessGroup.Instance.GetAllBusinessGroup();
            ddlBussiGroup.DataTextField = "Name";
            ddlBussiGroup.DataValueField = "BGID";
            ddlBussiGroup.DataBind();
            ddlBussiGroup.Items.Insert(0, new ListItem("请选择", "-1")); 
        }

    }
}