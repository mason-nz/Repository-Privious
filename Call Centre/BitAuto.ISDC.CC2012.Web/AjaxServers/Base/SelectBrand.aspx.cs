using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Base
{
    public partial class SelectBrand : PageBase
    {
        #region 属性

        public string Requestname
        {
            get { return Request.QueryString["name"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["name"].Trim()) : string.Empty; }
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
            BitAuto.YanFa.Crm2009.Entities.QueryCarBrand QueryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryCarBrand();
            if (!string.IsNullOrEmpty(Requestname))
            {
                QueryBrandInfo.BrandName = Requestname.Trim();
            }
            int totalCount;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandInfo(QueryBrandInfo, " Spell asc", PageCommon.Instance.PageIndex, PageSize, out totalCount);

            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterFriendCustMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterFriendCustMappingList.DataBind();

            litPagerDown1.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 3);


            //if(!string.IsNullOrEmpty(RequestCustID))
            //{
            //    QueryBrandInfo.CustID = RequestCustID;
            //    //加载要修改的品牌内容
            //    DataTable dttemp = BLL.CarBrand.Instance.GetCustBrandInfo(QueryBrandInfo, "", 1, 10000, out totalCount);
            //    if(dttemp!=null && dttemp.Rows.Count>0)
            //    {
            //        for (int i = 0; i < dttemp.Rows.Count; i++)
            //        {
            //            literalEditCont.Text += "<tr><td><a href=\"javascript:DelSelectCustBrand('" + dttemp.Rows[i]["brandid"] + "');\"  name='" + dttemp.Rows[i]["name"] + "' id='" + dttemp.Rows[i]["brandid"] + "'  ><img src=\"../Images/close.gif\" title=\"删除\"/></a></td>";
            //            literalEditCont.Text += "<td class=\"l\">"+dttemp.Rows[i]["brandid"]+"</td>";
            //            literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["name"] + "</td>";
            //        }
            //    }
            //}

            if (!string.IsNullOrEmpty(RequestBrandIDs))
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCarBrand query = new BitAuto.YanFa.Crm2009.Entities.QueryCarBrand();
                query.BrandIDs = RequestBrandIDs;

                //加载要修改的品牌内容
                DataTable dttemp = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandInfo(query, "", 1, 10000, out totalCount);
                if (dttemp != null && dttemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dttemp.Rows.Count; i++)
                    {
                        literalEditCont.Text += "<tr><td><a href=\"javascript:DelSelectCustBrand('" + dttemp.Rows[i]["brandid"] + "');\"  name='" + dttemp.Rows[i]["name"] + "' id='" + dttemp.Rows[i]["brandid"] + "'  ><img src=\"/Images/close.png\" title=\"删除\"/></a></td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["brandid"] + "</td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["name"] + "</td>";
                    }
                }
            }



        }
        //private string GetWhere()
        //{
        //    string where = Request.Url.Query;
        //    if (where.StartsWith("?"))
        //    {
        //        where = where.TrimStart('?');
        //    }

        //    if (!where.Contains("name="))
        //    {
        //        where += "&name=" + Crm2009.BLL.Util.EscapeString(Requestname);
        //    }
        //    if (!where.Contains("brandids="))
        //    {
        //        where += "&BrandIDs=" + Crm2009.BLL.Util.EscapeString(RequestBrandIDs);
        //    }
        //    return where;
        //}
    }
}