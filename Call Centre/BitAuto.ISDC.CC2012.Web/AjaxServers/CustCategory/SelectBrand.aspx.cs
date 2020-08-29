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
    public partial class SelectBrand : PageBase
    {
        #region 属性
        public string Requestname
        {
            get { return Request.QueryString["name"] != null ? Request.QueryString["name"].Trim() : string.Empty; }
        }
        public string RequestBrandIDs
        {
            get { return Request.QueryString["BrandIDs"] != null ? Request.QueryString["BrandIDs"].Trim() : string.Empty; }
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
            }
            DataTable dt = BLL.DealerInfo.Instance.GetBrandInfoByName(brandname, " a.Spell asc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterFriendCustMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterFriendCustMappingList.DataBind();

            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);

            if (!string.IsNullOrEmpty(RequestBrandIDs))
            {
                //加载要修改的品牌内容
                DataTable dttemp = BLL.DealerInfo.Instance.GetBrandInfo(RequestBrandIDs, " a.Spell asc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                if (dttemp != null && dttemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dttemp.Rows.Count; i++)
                    {
                        literalEditCont.Text += "<tr><td><a href=\"javascript:DelSelectCustBrand('" + dttemp.Rows[i]["brandid"] + "');\"  name='" + dttemp.Rows[i]["name"] + "' id='" + dttemp.Rows[i]["brandid"] + "'  ><img src=\"/Images/close.png\" title=\"删除\"/></a></td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["brandid"] + "</td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["name"] + "</td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["CustomBrandName"] + "</td>";
                        literalEditCont.Text += "<td  style=\"display:none;\"><a name=\"" + dttemp.Rows[i]["CustomBrandID"] + "\">" + dttemp.Rows[i]["CustomBrandID"] + "</a></td></tr>";
                    }
                }
            }
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