using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.WebAPI.Helper;

namespace BitAuto.ISDC.CC2012.WebAPI.AjaxServices
{
    public partial class AutoCallItems : System.Web.UI.Page
    {
        #region 属性
        private string Phone
        {
            get { return HttpContext.Current.Request["phone"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["phone"].ToString()); }
        }

        private string Pro
        {
            get { return HttpContext.Current.Request["pro"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["pro"].ToString()); }
        }

        private string ACStatus
        {
            get { return HttpContext.Current.Request["ac"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ac"].ToString()); }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int RecordCount;
        private int userID = 0;
        public int GroupLength = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }
        //绑定数据
        private void BindData()
        {
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            DataTable dt = CommonHelper.GetAutoCallItemMoniterList(Pro, Phone, ACStatus, PageSize, BLL.PageCommon.Instance.PageIndex, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 100);
        }
    }
}