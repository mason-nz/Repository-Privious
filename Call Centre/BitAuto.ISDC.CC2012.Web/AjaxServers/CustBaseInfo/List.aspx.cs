using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo
{
    public partial class CustBaseInfoList : PageBase
    {
        #region 属性定义
        private string RequestCustName
        {
            get { return Request["CustName"] == null ? "" : HttpUtility.UrlDecode(Request["CustName"].ToString()); }
        }
        private string RequestSexs
        {
            get { return Request["Sexs"] == null ? "" : HttpUtility.UrlDecode(Request["Sexs"].ToString()); }
        }
        private string RequestCustTel
        {
            get { return Request["CustTel"] == null ? "" : HttpUtility.UrlDecode(Request["CustTel"].ToString()); }
        }
        private string RequestProvinceID
        {
            get { return Request["ProvinceID"] == null ? "" : HttpUtility.UrlDecode(Request["ProvinceID"].ToString()); }
        }
        private string RequestCityID
        {
            get { return Request["CityID"] == null ? "" : HttpUtility.UrlDecode(Request["CityID"].ToString()); }
        }
        private string RequestCountyID
        {
            get { return Request["CountyID"] == null ? "" : HttpUtility.UrlDecode(Request["CountyID"].ToString()); }
        }
        #endregion

        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        public bool DelButtonAuth = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DelButtonAuth = BLL.Util.CheckButtonRight("SYS024BUT2101");
                CustBaseInfoDataBind();
            }
        }

        private void CustBaseInfoDataBind()
        {
            QueryCustBasicInfo query = new QueryCustBasicInfo();
            query.CustName = RequestCustName;
            query.Sexs = RequestSexs;
            query.CustTel = RequestCustTel;
            query.ProvinceID = CommonFunction.ObjectToInteger(RequestProvinceID, -1);
            query.CityID = CommonFunction.ObjectToInteger(RequestCityID, -1);
            query.CountyID = CommonFunction.ObjectToInteger(RequestCountyID, -1);

            repeaterTableList.DataSource = BLL.CustBasicInfo.Instance.GetCustBasicInfo(query, "cb.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);
            repeaterTableList.DataBind();
            AjaxPager_Custs.PageSize = 20;
            AjaxPager_Custs.InitPager(RecordCount);
        }

        public string GetOperLink(string tels)
        {
            string[] tel = tels.Split(',');
            if (tel.Length == 1)
            {
                return "<a href='/WOrderV2/AddWOrderInfo.aspx?" + BLL.WOrderRequest.AddWOrderComeIn_CustPool(tel[0]).ToString() + "' target='_blank'>添加工单</a>";
            }
            else if (tel.Length > 1)
            {
                string urls = "";
                foreach (string phone in tel)
                {
                    urls += BLL.WOrderRequest.AddWOrderComeIn_CustPool(phone).ToString() + ",";
                }
                urls.TrimEnd(',');
                return "<a href='javascript:void(0)' onclick='AddWOrderV2(this,\"" + tels + "\",\"" + urls + "\");'>添加工单</a>";
            }
            else return "";
        }
    }
}