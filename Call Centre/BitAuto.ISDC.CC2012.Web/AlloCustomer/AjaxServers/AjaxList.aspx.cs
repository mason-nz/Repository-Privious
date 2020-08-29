using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AlloCustomer.AjaxServers
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string RequestCustName
        {
            get { return HttpContext.Current.Request["CustName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString()); }
        }
        private string RequestProvince
        {
            get { return HttpContext.Current.Request["Province"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Province"].ToString()); }
        }
        private string RequestCity
        {
            get { return HttpContext.Current.Request["City"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["City"].ToString()); }
        }
        private string RequestCounty
        {
            get { return HttpContext.Current.Request["County"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["County"].ToString()); }
        }
        private string RequestArea
        {
            get { return HttpContext.Current.Request["Area"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Area"].ToString()); }
        }
        private string RequestKeFuName
        {
            get { return HttpContext.Current.Request["KeFuName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KeFuName"].ToString()); }
        }

        private bool isAssign = false;
        private bool isRecede = false;
        private int userID = 0;

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                userID = BLL.Util.GetLoginUserID();
                isAssign = BLL.Util.CheckRight(userID, "SYS024BUT10110101");
                isRecede = BLL.Util.CheckRight(userID, "SYS024BUT10110102");

                bindData();
            }

        }

        private void bindData()
        {
            YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery query = new YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery();
            if (RequestProvince != "")
            {
                query.CustProvinceID = RequestProvince;
            }
            if (RequestCity != "")
            {
                query.CustCityID = RequestCity;
            }
            if (RequestCounty != "")
            {
                query.CustCountyID = RequestCounty;
            }
            if (RequestCustName != "")
            {
                query.CustName = RequestCustName;
            }
            if (RequestArea != "")
            {
                query.CustDepartment = RequestArea;
            }
            if (RequestKeFuName != "")
            {
                query.KeFuName = RequestKeFuName;
            }
            //过滤撤销状态
            query.Where = "and YJKDemandInfo.Status <>" + (int)BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus.Revoke;

            int RecordCount = 0;

            DataTable dt = YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetCustDemandInfo(query, "", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount, userID);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //得到操作
        protected string getOperLink(string custid)
        {
            return getAssignLink(custid) + getRecedeLink(custid);
        }

        //分配链接
        private string getAssignLink(string custid)
        {
            return isAssign ? "<a href='javascript:;' onclick='operAssign(\"" + custid + "\")'>分配</a>&nbsp;" : string.Empty;
        }

        //收回链接
        private string getRecedeLink(string custid)
        {
            return isRecede ? "<a href='javascript:;' onclick='operRecede(\"" + custid + "\")'>收回</a>&nbsp;" : string.Empty;
        }

    }
}