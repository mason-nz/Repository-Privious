using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AlloCustomer
{
    public partial class Export : BitAuto.ISDC.CC2012.Web.Base.PageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //增加“需求管理--客户分配”导出功能验证逻辑
                if (BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT10110103"))
                {
                    bindData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
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

            DataTable dt = YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetCustDemandInfo(query, "", 1, 10000, out RecordCount, BLL.Util.GetLoginUserID());

            DataTable dtNew = new DataTable();
            string[] arr = new string[5] { "客户名称", "需求总数量", "集客中需求量", "负责客服", "负责销售" };
            for (int i = 0; i < arr.Length; i++)
            {
                dtNew.Columns.Add(arr[i]);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtNew.NewRow();
                dr["客户名称"] = dt.Rows[i]["CustName"];
                dr["需求总数量"] = dt.Rows[i]["CountNum"].ToString();
                dr["集客中需求量"] = dt.Rows[i]["Countjkz"].ToString();
                dr["负责客服"] = dt.Rows[i]["KeFuName"];
                dr["负责销售"] = dt.Rows[i]["SaleName"];

                dtNew.Rows.Add(dr);
            }

            BLL.Util.ExportToCSV("客户分配导出信息", dtNew, false);

        }


        //DataTable dtNew = new DataTable();
        //Hashtable ht = new Hashtable();
        //ht.Add("CustName", "客户名称");
        //ht.Add("ExpectedNum", "需求总数量");
        //ht.Add("PracticalNum", "集客中需求量");
        //ht.Add("KeFuName", "负责客服");
        //ht.Add("SaleName", "负责销售");
        //for (int i = 0; i < dt.Columns.Count; i++)
        //{
        //    int isExist = false;
        //    foreach (DictionaryEntry de in ht)
        //    {
        //        if(de
        //    }
        //}
    }
}