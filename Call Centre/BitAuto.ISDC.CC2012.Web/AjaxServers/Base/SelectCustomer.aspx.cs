using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Base
{
    public partial class SelectCustomer : PageBase
    {
        #region 属性定义
        public string RequestSearch
        {
            get { return Request.QueryString["search"] == null ? string.Empty : Request.QueryString["search"].ToString().Trim(); }
        }
        public string RequestType
        {
            get { return Request.QueryString["type"] == null ? string.Empty : Request.QueryString["type"].ToString().Trim(); }
        }
        public string RequestCustID
        {
            get { return Request.QueryString["CustID"] == null ? string.Empty : Request.QueryString["CustID"].ToString().Trim(); }
        }
        public string RequestCustName
        {
            get { return Request.QueryString["CustName"] == null ? string.Empty : Request.QueryString["CustName"].ToString().Trim(); }
        }
        public string RequestProvinceID
        {
            get { return Request.QueryString["provinceid"] == null ? "-1" : Request.QueryString["provinceid"].ToString().Trim(); }
        }
        public string RequestCityID
        {
            get { return Request.QueryString["cityid"] == null ? "-1" : Request.QueryString["cityid"].ToString().Trim(); }
        }
        public string RequestHasSaleNetwork
        {
            get { return Request["HasSaleNetwork"]; }
        }
        public string RequestCustType
        {
            get { return Request["CustType"]; }
        }
        public int PageSize = 10;
        public int GroupLength = 8;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData("yes");
            switch (RequestType)
            {
                case "1":
                    spanTitle.InnerHtml = "选择所属厂商";
                    break;
                case "2":
                    spanTitle.InnerHtml = "选择所属集团";
                    break;
                case "3":
                    spanTitle.InnerHtml = "选择所属交易市场";
                    break;
                case "4":
                    spanTitle.InnerHtml = "选择所属4S";
                    break;
                default:
                    spanTitle.InnerHtml = "选择客户";
                    break;
            }
        }

        private void LoadData(string requestSearch)
        {
            if (requestSearch == "yes")
            {
                //查询
                int totalCount = 0;
                BitAuto.YanFa.Crm2009.Entities.QueryCustInfo query = new BitAuto.YanFa.Crm2009.Entities.QueryCustInfo();
                if (RequestCustID != "")
                {
                    query.CustID = RequestCustID;
                }
                if (RequestCustName != "")
                {
                    query.CustName = RequestCustName;
                }
                if (RequestProvinceID != "-1" && RequestProvinceID != "")
                {
                    query.ProvinceID = RequestProvinceID;
                }
                if (RequestCityID != "-1" && RequestCityID != "")
                {
                    query.CityID = RequestCityID;
                } query.Status = 0;
                DataTable table;
                switch (RequestType)
                {
                    case "1":
                        query.TypeID = ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Company).ToString();
                        table = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "CreateTime Desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                        break;
                    case "2":
                        query.TypeID = ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc).ToString();
                        table = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "CreateTime Desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                        break;
                    case "3":
                        query.TypeID = ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.TradeMarket).ToString(); ;
                        table = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "CreateTime Desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                        break;
                    case "4":
                        query.TypeID = "20003";
                        spanTitle.InnerHtml = "选择所属4S";
                        table = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "CreateTime Desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                        break;
                    default:
                        //显示全部客户，但是要加权限判断
                        int userID = int.Parse(Session["userid"].ToString());
                        string departID = Session["departid"].ToString();
                        table = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, userID, departID, "CreateTime Desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                        break;
                }

                //设置数据源
                if (table != null && table.Rows.Count > 0)
                {
                    repterCustomerList.DataSource = table;
                }
                //绑定列表数据
                repterCustomerList.DataBind();

                litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);
            }
        }

        private string GetWhere()
        {
            string where = "1=1";
            if (!string.IsNullOrEmpty(RequestSearch))
            {
                where += "&search=" + BLL.Util.EscapeString(RequestSearch);
            }
            if (!string.IsNullOrEmpty(RequestType))
            {
                where += "&type=" + BLL.Util.EscapeString(RequestType);
            }
            if (!string.IsNullOrEmpty(RequestCustID))
            {
                where += "&custid=" + BLL.Util.EscapeString(RequestCustID);
            }
            if (!string.IsNullOrEmpty(RequestCustName))
            {
                where += "&custname=" + BLL.Util.EscapeString(RequestCustName);
            }
            if (!string.IsNullOrEmpty(RequestProvinceID))
            {
                where += "&provinceid=" + BLL.Util.EscapeString(RequestProvinceID);
            }
            if (!string.IsNullOrEmpty(RequestCityID))
            {
                where += "&cityid=" + BLL.Util.EscapeString(RequestCityID);
            }
            if (!string.IsNullOrEmpty(RequestHasSaleNetwork))
            {
                where += "&hasSaleNetwork=" + BLL.Util.EscapeString(RequestHasSaleNetwork);
            }
            if (!string.IsNullOrEmpty(Request["CustType"]))
            {
                where += "&CustType=" + BLL.Util.EscapeString(Request["CustType"]);
            }
            return where;
        }
    }
}
