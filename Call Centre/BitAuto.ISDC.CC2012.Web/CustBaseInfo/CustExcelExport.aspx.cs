using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustBaseInfo
{
    public partial class CustExcelExport : PageBase
    {

        /// <summary>
        /// 要导出的字段
        /// </summary>
        public string Fields
        {
            get
            {
                if (HttpContext.Current.Request["field"] != null)
                {
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["field"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where
        {
            get
            {
                if (HttpContext.Current.Request["where"] != null)
                {
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["where"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string RequestBrowser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }

        #region 属性定义
        private string RequestCustName
        {
            get { return Request["CustName"] == null ? "" : HttpUtility.UrlDecode(Request["CustName"].ToString()); }
        }
        private string RequestSexs
        {
            get { return Request["Sexs"] == null ? "" : HttpUtility.UrlDecode(Request["Sexs"].ToString()); }
        }
        private string RequestCallTime
        {
            get { return Request["CallTime"] == null ? "" : HttpUtility.UrlDecode(Request["CallTime"].ToString()); }
        }
        private string RequestCustTel
        {
            get { return Request["CustTel"] == null ? "" : HttpUtility.UrlDecode(Request["CustTel"].ToString()); }
        }
        //经销商名称
        private string RequestDealerName
        {
            get { return Request["DealerName"] == null ? "" : HttpUtility.UrlDecode(Request["DealerName"].ToString()); }
        }
        private string RequestBeginTime
        {
            get { return Request["BeginTime"] == null ? "" : HttpUtility.UrlDecode(Request["BeginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(Request["EndTime"].ToString()); }
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
        private string RequestAreaIDS
        {
            get { return Request["AreaIDS"] == null ? "" : HttpUtility.UrlDecode(Request["AreaIDS"].ToString()); }
        }
        private string RequestDataSourceS
        {
            get { return Request["DataSourceS"] == null ? "" : HttpUtility.UrlDecode(Request["DataSourceS"].ToString()); }
        }
        private string RequestCityScopes
        {
            get { return Request["CityScopes"] == null ? "" : HttpUtility.UrlDecode(Request["CityScopes"].ToString()); }
        }
        private string CustCategorys
        {
            get { return Request["CustCategorys"] == null ? "" : HttpUtility.UrlDecode(Request["CustCategorys"].ToString()); }
        }
        private string AgentNum
        {
            get { return Request["AgentNum"] == null ? "" : HttpUtility.UrlDecode(Request["AgentNum"].ToString()); }
        }
        private int countOfRecords = 0;
        public int CountOfRecords { get { return countOfRecords; } }
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public bool DelButtonAuth = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ExportCustInfo();
        }


        private void ExportCustInfo()
        {
           //功能改造，代码失效删除
        }
    }
}