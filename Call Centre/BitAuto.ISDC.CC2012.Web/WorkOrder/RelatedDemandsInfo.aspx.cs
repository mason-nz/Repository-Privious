using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.YanFa.Crm2009.Entities.YJKDemand;
using BitAuto.ISDC.CC2012.BLL;
using System.Configuration;
using BitAuto.YanFa.Crm2009.Entities;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class RelatedDemandsInfo : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string CustId
        {
            get { return Request.QueryString["CustId"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["CustId"].Trim()) : string.Empty; }
        }
        public string PhoneNum
        {
            get { return Request.QueryString["phoneNum"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["phoneNum"].Trim()) : string.Empty; }
        }
        public string CarBrandID
        {
            get { return Request.QueryString["carBrandID"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["carBrandID"].Trim()) : string.Empty; }
        }
        public string CarTypeID
        {
            get { return Request.QueryString["carTypeID"] != null ? System.Web.HttpUtility.UrlDecode(Request.QueryString["carTypeID"].Trim()) : string.Empty; }
        }
        public int GroupLength = 5;
        public int PageSize = 5;
        public string strUrl = ConfigurationManager.AppSettings["DemandDetailsUrl"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                txtCustomMobileNum.Value = PhoneNum;
                GetCarBrand();
                DataBinds();
            }
        }

        private void DataBinds()
        {
            int totalCount = 0;
            YJKDemandQuery query = new YJKDemandQuery();
            if (!string.IsNullOrEmpty(CustId))
            {
                query.CustID = CustId;
            }
            if (!string.IsNullOrEmpty(CarBrandID) && CarBrandID != "-1")
            {
                query.BrandIDs = CarBrandID;
            }
            if (!string.IsNullOrEmpty(CarTypeID) && CarTypeID != "-1")
            {
                query.SerialIDs = CarTypeID;
                query.BrandIDs = "";
            }
            //+ (string.IsNullOrEmpty(txtCustomMobileNum.Value) == true ? "" : " and YJKDemandInfo.ContactMobile='" + txtCustomMobileNum.Value.Trim() + "'");
            if (!string.IsNullOrEmpty(txtCustomMobileNum.Value))
            {
                query.BuyerMobile = txtCustomMobileNum.Value.Trim();
            }

            query.Where = " AND YJKDemandInfo.Status in (" + (int)YJKDemandStatus.Start + "," + (int)YJKDemandStatus.End + "," + (int)YJKDemandStatus.Stop + ") ";

            DataTable dt = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandInfo(query, "YJKDemandInfo.InsertTime desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repterFriendCustMappingList.DataSource = dt;
            }
            //绑定列表数据
            repterFriendCustMappingList.DataBind();
            string url = BLL.Util.GetUrl();
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);

        }
        /// <summary>
        /// 获取传入的客户下的所有品牌
        /// </summary>
        private void GetCarBrand()
        {
            selCarBrand.DataSource = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandBrands(CustId); ;

            selCarBrand.DataValueField = "BrandIDs";
            selCarBrand.DataTextField = "BrandNames";
            selCarBrand.DataBind();
            ListItem item = new ListItem();
            item.Value = "-1";
            item.Text = "请选择品牌";
            selCarBrand.Items.Insert(0, item);
        }

        public string GetStatusList(string statusValue)
        {
            string actionName = string.Empty;
            int _statusValue;
            if (int.TryParse(statusValue, out _statusValue))
            {
                actionName = BLL.Util.GetEnumOptText(typeof(BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus), _statusValue);
            }
            return actionName;

        }

        public string GetOrderInfo(string demandid)
        {
            if (!string.IsNullOrEmpty(txtCustomMobileNum.Value))
            {
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetBuyerData(demandid, txtCustomMobileNum.Value.Trim());

                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("<div style=' margin:0px; padding:0px; text-align:left;width:100%;'>");
                    str.Append("<span style='clear:both; float:left;line-height:20px;width:100%;'>" + dt.Rows[0]["CarName"].ToString().Trim() + "</span>");

                    str.Append("<span style='clear:both; float:left;line-height:20px;width:100%;'>下单时间：" + (string.IsNullOrEmpty(dt.Rows[0]["OrderTime"].ToString().Trim()) == true ? "" : Convert.ToDateTime(dt.Rows[0]["OrderTime"].ToString().Trim()).ToString("yyyy-MM-dd")) + "</span>");

                    decimal reducedPrice = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToDecimal(dt.Rows[0]["ReducedPrice"]);
                    if (reducedPrice != 0)
                    {
                        str.Append("<span style='clear:both; float:left;line-height:20px;width:100%;'>降价优惠：" + reducedPrice.ToString("0.00") + "万元</span>");
                    }

                    decimal bagWorth = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToDecimal(dt.Rows[0]["GiftBagWorth"]);
                    string bagContent = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToString(dt.Rows[0]["GiftBagContent"]);

                    if (bagWorth != 0 || !string.IsNullOrEmpty(bagContent))
                    {
                        str.Append("<span style='clear:both; float:left;line-height:20px;width:100%;'>礼包信息：" + (bagWorth == 0 ? "" : bagWorth.ToString("0.00") + "元&nbsp;&nbsp;") + bagContent + "</span>");
                    }
                    string otherPrivilege = BitAuto.YanFa.Crm2009.Entities.CommonFunction.ObjectToString(dt.Rows[0]["OtherPrivilege"]);
                    if (!string.IsNullOrEmpty(otherPrivilege))
                    {
                        str.Append("<span style='clear:both; float:left;line-height:20px;width:100%;'>其他优惠：" + otherPrivilege + "</span>");
                    }
                    str.Append("</div>");
                    return str.ToString();
                }
            }
            return "……";
        }


    }
}