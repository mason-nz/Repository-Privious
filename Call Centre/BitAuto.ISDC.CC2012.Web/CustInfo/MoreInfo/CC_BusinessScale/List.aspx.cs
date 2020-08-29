using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_BusinessScale
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
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

        public string TID
        {
            get { return Request["TID"] + ""; }
        }

        public string Action
        {
            get { return Request["Action"] + ""; }
        }
        public int CurrentPage
        {
            get
            {
                return this.AjaxPager1.CurrentPage;
            }
        }

        public int PageSize
        {
            get
            {
                return this.AjaxPager1.PageSize;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindData();
            }
        }
        private void BindData()
        {
            //查询
            int totalCount = 0;
            if (!string.IsNullOrEmpty(TID))
            {
                Entities.QueryProjectTask_BusinessScale qci = new Entities.QueryProjectTask_BusinessScale();
                qci.PTID = TID;
                qci.Status = 0;
                DataTable table = BLL.ProjectTask_BusinessScale.Instance.GetProjectTask_BusinessScale(qci, "CreateTime desc", 1, 20, out totalCount);

                //设置数据源
                if (table != null && table.Rows.Count > 0)
                {
                    repeater.DataSource = table;
                }
                //绑定列表数据
                repeater.DataBind();
                //分页控件
                this.AjaxPager1.InitPager(totalCount, "BusinessScaleInfoContent", PageSize, CurrentPage);
            }
        }

        public string GetBusinessScale(string ID, Type type)
        {
            string result = "";

            if (!string.IsNullOrEmpty(ID)) result = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(type, int.Parse(ID));
            return result;
        }
    }
}