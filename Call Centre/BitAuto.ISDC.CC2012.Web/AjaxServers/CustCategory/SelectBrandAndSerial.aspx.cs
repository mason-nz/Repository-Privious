using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory
{
    public partial class SelectBrandAndSerial : PageBase
    {
        #region 属性
        public string Requestname
        {
            get { return Request.QueryString["name"] != null ? HttpUtility.UrlDecode(Request.QueryString["name"].Trim()) : string.Empty; }
        }
        public string RequestBrandIDs
        {
            get { return Request.QueryString["BrandIDs"] != null ? HttpUtility.UrlDecode(Request.QueryString["BrandIDs"].Trim()) : string.Empty; }
        }
        public int GroupLength = 5;
        public int PageSize = 10;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DataBinds();
            }

        }

        private void DataBinds()
        {
            int totalCount;

            string brandname = string.Empty;
            if (!string.IsNullOrEmpty(Requestname))
            {
                brandname = Requestname.Trim();
                txtBrandName.Value = brandname;
            }
            //DataTable dt = BLL.DealerInfo.Instance.GetBrandInfoByName(brandname, " a.Spell asc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
            DataTable dt = BLL.BuyCarInfo.Instance.GetInfo(brandname, "car_brand.name,Car_Serial.Name", PageCommon.Instance.PageIndex, PageSize, out totalCount);
            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterFriendCustMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterFriendCustMappingList.DataBind();

            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);


        }
        private string GetWhere()
        {
            string where = "";
            string query = Request.Url.Query;

            if ((!Requestname.Equals("")) || Requestname != null)
            {
                where += "&name=" + BLL.Util.EscapeString(Requestname);
            }
            if ((!RequestBrandIDs.Equals("")) || RequestBrandIDs != null)
            {
                where += "&BrandIDs=" + BLL.Util.EscapeString(RequestBrandIDs);
            }
            where += "&random=" + (new Random()).Next().ToString();
            return where;
        }
    }
}