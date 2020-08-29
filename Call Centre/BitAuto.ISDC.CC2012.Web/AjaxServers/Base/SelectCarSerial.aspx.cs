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
    public partial class SelectCarSerial : PageBase
    {
        #region 属性

        public string Requestname
        {
            get { return Request.QueryString["Name"] != null ? Request.QueryString["Name"].Trim() : string.Empty; }
        }
        public string RequestMainBrandID
        {
            get { return Request.QueryString["BrandID"] != null ? Request.QueryString["BrandID"].Trim() : string.Empty; }
        }
        /*
        public string RequestBrandIDs
        {
            get { return Request.QueryString["BrandIDs"] != null ? Request.QueryString["BrandIDs"].Trim() : string.Empty; }
        }
         */

        public string RequestSerialIds
        {
            get { return Request.QueryString["SerialIds"] != null ? Request.QueryString["SerialIds"].Trim() : string.Empty; }
        }

        public int GroupLength = 5;
        public int PageSize = 10;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBll();
                DataBinds();
            }
        }

        private void BindBll()
        {
            BitAuto.YanFa.Crm2009.Entities.QueryCarBrand QueryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryCarBrand();
            int totalCount;
            DataTable dttemp = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandInfo(QueryBrandInfo, "Spell asc", 1, 10000, out totalCount);
            if (dttemp != null && dttemp.Rows.Count > 0)
            {
                ddlBrandList.DataValueField = "BrandID";
                ddlBrandList.DataTextField = "name";
                ddlBrandList.DataSource = dttemp;
                ddlBrandList.DataBind();
            }
            ddlBrandList.Items.Insert(0, new ListItem("请选择", "-2"));
        }

        private void DataBinds()
        {
            BitAuto.YanFa.Crm2009.Entities.QueryCarBrand QueryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryCarBrand();
            if (!string.IsNullOrEmpty(Requestname))
            {
                QueryBrandInfo.BrandName = Requestname.Trim();
            }
            if (!string.IsNullOrEmpty(RequestMainBrandID) && RequestMainBrandID != "-2")
            {
                QueryBrandInfo.BrandID = RequestMainBrandID.Trim();
            }
            int totalCount;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetCarSerial(QueryBrandInfo, "b.Spell, a.Spell", PageCommon.Instance.PageIndex, PageSize, out totalCount);

            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterSerialMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterSerialMappingList.DataBind();

            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);


            if (!string.IsNullOrEmpty(RequestSerialIds))
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCarBrand queryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryCarBrand();
                queryBrandInfo.SerialIds = RequestSerialIds;

                //加载要修改的品牌内容
                DataTable dttemp = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetCarSerial(queryBrandInfo, "b.Spell, a.Spell", 1, 10000, out totalCount);
                if (dttemp != null && dttemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dttemp.Rows.Count; i++)
                    {
                        literalEditCont.Text += "<tr><td><a href=\"javascript:DelSelectSerialBrand('" + dttemp.Rows[i]["SerialID"] + "');\"  name='" + dttemp.Rows[i]["name"] + "' id='" + dttemp.Rows[i]["SerialID"] + "'  ><img src=\"/Images/close.png\" title=\"删除\"/></a></td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["BrandName"] + "</td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["SerialID"] + "</td>";
                        literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["name"] + "</td>";
                    }
                }
            }


        }
        private string GetWhere()
        {
            string where = Request.Url.Query;
            if (where.StartsWith("?"))
            {
                where = where.TrimStart('?');
            }

            if (!where.Contains("name="))
            {
                where += "&Name=" + BLL.Util.EscapeString(Requestname);
            }
            if (!where.Contains("brandid="))
            {
                where += "&BrandID=" + BLL.Util.EscapeString(RequestMainBrandID);
            }
            if (!where.Contains("serialids="))
            {
                where += "&SerialIds=" + BLL.Util.EscapeString(RequestSerialIds);
            }
            return where;
        }
    }
}