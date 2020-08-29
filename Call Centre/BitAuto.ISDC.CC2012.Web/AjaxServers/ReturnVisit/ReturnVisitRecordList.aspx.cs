using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.YanFa.Crm2009.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class ReturnVisitRecordList : PageBase
    {
        #region 定义属性


        public string RequestPageSize
        {
            get { return Request.QueryString["pageSize"] == null ? PageCommon.Instance.PageSize.ToString() : Request.QueryString["pageSize"].Trim(); }
        }
        public string RequestCustName
        {
            get { return Request.QueryString["CustName"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["CustName"].Trim()); }
        }
        public string RequestCustID
        {
            get { return Request.QueryString["CustID"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["CustID"].Trim()); }
        }
        public string RequestProvinceID
        {
            get { return Request.QueryString["Province"] == null ? "-1" : HttpContext.Current.Server.UrlDecode(Request.QueryString["Province"]); }
        }
        public string RequestCityID
        {
            get { return Request.QueryString["City"] == null ? "-1" : HttpContext.Current.Server.UrlDecode(Request.QueryString["City"]); }
        }
        public string RequestCountyID
        {
            get { return Request.QueryString["County"] == null ? "-1" : HttpContext.Current.Server.UrlDecode(Request.QueryString["County"]); }
        }

        //默认为-2，客户类型
        public string RequestCustType
        {
            get { return Request["CustType"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["CustType"].ToString()); }
        }
        //默认为0,业务线
        public string TypeID
        {
            get { return Request["TypeID"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["TypeID"].ToString()); }
        }
        //默认为-2，回访类型
        public string VisitType
        {
            get { return Request["VisitType"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["VisitType"].ToString()); }
        }
        public string VisitUserid
        {
            get { return Request["VisitUserid"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["VisitUserid"].ToString()); }
        }

        public string StartTime
        {
            get { return Request["StartTime"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["StartTime"].ToString()); }
        }
        public string EndTime
        {
            get { return Request["EndTime"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["EndTime"].ToString()); }
        }
        /// <summary>
        /// 集采项目名
        /// </summary>
        public string RequestProjectName
        {
            get { return Request["ProjectName"] == null ? string.Empty : HttpContext.Current.Server.UrlDecode(Request["ProjectName"].ToString()); }
        }
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;
        public string FzPerson = string.Empty;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {    
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }
        #endregion

        #region function
        private void BindData()
        {






            //       url += '&VisitUserid=' + escape(SelectUserid);
            //       url += '&ProjectName=' + escapeStr(txtSearchProjectName);

            //int totcount = 0;
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }

            Entities.QueryReturnVisitRecord query = new Entities.QueryReturnVisitRecord();


            if (!string.IsNullOrEmpty(RequestCustName))
            {
                query.CustName = RequestCustName.Trim();
            }
            if (!string.IsNullOrEmpty(RequestCustID))
            {
                query.CustID = RequestCustID.Trim();
            }

            if (!string.IsNullOrEmpty(RequestCustType))
            {
                query.CustType = RequestCustType;
            }
            if (!string.IsNullOrEmpty(TypeID))
            {
                query.TypeID = TypeID;
            }
            if (!string.IsNullOrEmpty(VisitType))
            {
                query.VisitType = VisitType;
            }
            if (!string.IsNullOrEmpty(VisitUserid))
            {
                query.VisitUserid = VisitUserid;
            }

            if (!string.IsNullOrEmpty(RequestProvinceID) && int.Parse(RequestProvinceID) > 0)
            {
                query.ProvinceID = RequestProvinceID.Trim();
            }

            if (!string.IsNullOrEmpty(RequestCityID) && int.Parse(RequestCityID) > 0)
            {
                query.CityID = RequestCityID.Trim();

            }
            if (!string.IsNullOrEmpty(RequestCountyID) && int.Parse(RequestCountyID) > 0)
            {
                query.CountyID = RequestCountyID.Trim();

            }


            //add lxw 12.6.8 最近访问时间 
            if (!string.IsNullOrEmpty(StartTime))
            {
                query.StartTime = DateTime.Parse(StartTime);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                query.EndTime = DateTime.Parse(EndTime);
            }
            //if (Contact != -2)
            //{
            //    query.Contact = Contact;
            //}
            if (!string.IsNullOrEmpty(RequestProjectName))
            {
                query.ProjectName = RequestProjectName.Trim();
            }

            //if (!string.IsNullOrEmpty(ReqeustCCProjectName))
            //{
            //    query.ReqeustCCProjectName = ReqeustCCProjectName;
            //}
            int count;
            DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordList(query, "ReturnVisit.createtime desc", PageCommon.Instance.PageIndex, PageSize, out count);
            RecordCount = count;
            rplist.DataSource = dt;
            rplist.DataBind();
            litPagerDown1.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, PageCommon.Instance.PageIndex, 1);

        }



        protected string getReturnVisitLast(string CustID)
        {

            int totalCount;
            BitAuto.YanFa.Crm2009.Entities.QueryReturnVisit QueryReturnVisit = new BitAuto.YanFa.Crm2009.Entities.QueryReturnVisit();
            QueryReturnVisit.CustID = CustID;
            DataTable table = BitAuto.YanFa.Crm2009.BLL.ReturnVisit.Instance.GetReturnVisit(QueryReturnVisit, 1, 1, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                return table.Rows[0]["createtime"].ToString();
            }

            return "";
        }
        #endregion
    }
}