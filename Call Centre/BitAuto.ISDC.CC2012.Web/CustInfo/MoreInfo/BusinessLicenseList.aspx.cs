using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class BusinessLicenseList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region Properties

        //public int CurrentPage
        //{
        //    get
        //    {
        //        return this.AjaxPager.CurrentPage;
        //    }
        //}

        //public int PageSize
        //{
        //    get
        //    {
        //        return this.AjaxPager.PageSize;
        //    }
        //}

        public string CustID
        {
            get { return Request["CustID"] + ""; }
        }

        public string DataSource
        {
            get { return Request["DataSource"] + ""; }
        }
        public int PageSize
        {
            get
            {
                return this.AjaxPager1.PageSize;
            }
        }
        //public BitAuto.YanFa.Crm2009.Web.Controls.AjaxPager AjaxPager
        //{
        //    get { return this.AjaxPager_BL; }
        //}

        #endregion

        //public BLHelper helper = new BLHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加入 年检记录的访问权限
                //int userId = BLL.Util.GetLoginUserID();
                //if (!BLL.Util.CheckRight(userId, "SYS024BUT2205"))
                //{
                //    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                //    Response.End();
                //}
            }
            //查询
            int totalCount = 0;
            DataTable table = new DataTable();

            BitAuto.YanFa.Crm2009.Entities.QueryBusinessLicense query = new BitAuto.YanFa.Crm2009.Entities.QueryBusinessLicense();
            query.CustID = this.CustID;

            table = BitAuto.YanFa.Crm2009.BLL.BusinessLicense.Instance.Get(query, PagerHelper.GetCurrentPage(), PageSize, out totalCount);
            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater.DataSource = table;
            }
            //绑定列表数据
            repeater.DataBind();
            //分页控件 
            AjaxPager1.PageSize = 5;
            AjaxPager1.InitPager(totalCount);  
        }
    }
}