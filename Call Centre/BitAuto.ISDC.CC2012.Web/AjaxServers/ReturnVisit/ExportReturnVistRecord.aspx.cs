using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class ExportReturnVistRecord : PageBase
    {
        #region 定义属性



        public string RequestExport
        {
            get { return Request.QueryString["Export"] == null ? string.Empty : Request.QueryString["Export"].Trim(); }
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
        protected void Page_Load(object sender, EventArgs e)
        { 
            if ((Request["Export"] + "").Trim() == "true")
            {
                DataTable dt = BindData();
                ExprotExcel(dt);
            }
        }


        private DataTable BindData()
        {


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

            //DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordListExport(query, "ReturnVisit.createtime desc", 1, 1000000, out count);
            DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetReturnVisitReCordListExport(query);
            return dt;
        }

        private void ExprotExcel(DataTable dt)
        {

            //要导出的字段
            Dictionary<string, string> ExportColums = new Dictionary<string, string>();
            ExportColums.Add("area", "大区");
            ExportColums.Add("provinceid", "省区");
            ExportColums.Add("cityid", "城市");
            ExportColums.Add("custname", "客户名称");
            ExportColums.Add("name", "会员名称");
            ExportColums.Add("membercode", "会员id");
            ExportColums.Add("brandid", "会员主营品牌");
            ExportColums.Add("begintime", "访问时间");
            ExportColums.Add("cname", "联系人");
            ExportColums.Add("phonenum", "联系电话");
            ExportColums.Add("truename", "访问员");
            ExportColums.Add("dictname", "访问分类");
            ExportColums.Add("remark", "访问描述");
            ExportColums.Add("staff", "负责员工");

            //字段排序
            dt.Columns["area"].SetOrdinal(0);
            dt.Columns["ProvinceID"].SetOrdinal(1);
            dt.Columns["CityID"].SetOrdinal(2);
            dt.Columns["CustName"].SetOrdinal(3);
            dt.Columns["Name"].SetOrdinal(4);
            dt.Columns["MemberCode"].SetOrdinal(5);
            dt.Columns["brandID"].SetOrdinal(6);
            dt.Columns["begintime"].SetOrdinal(7);
            dt.Columns["CName"].SetOrdinal(8);
            dt.Columns["phonenum"].SetOrdinal(9);
            dt.Columns["truename"].SetOrdinal(10);
            dt.Columns["DictName"].SetOrdinal(11);
            dt.Columns["Remark"].SetOrdinal(12);
            dt.Columns["staff"].SetOrdinal(13);

            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (ExportColums.ContainsKey(dt.Columns[i].ColumnName.ToLower()))
                {
                    //字段时要导出的字段，改名
                    dt.Columns[i].ColumnName = ExportColums[dt.Columns[i].ColumnName.ToLower()];
                }
                else
                {
                    //不是要导出的字段，删除
                    dt.Columns.RemoveAt(i);
                }
            }

            BLL.Util.ExportToCSV("导出访问记录", dt);

        }
    }
}