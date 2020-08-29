using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.YanFa.Crm2009.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class SecondCarList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region Properties

        public int CurrentPage
        {
            get
            {
                return this.AjaxPager.CurrentPage;
            }
        }

        public int PageSize
        {
            get
            {
                return this.AjaxPager.PageSize;
            }
        }

        public string CustID
        {
            get { return Request["CustID"] + ""; }
        }


        public BitAuto.ISDC.CC2012.Web.Controls.AjaxPager AjaxPager
        {
            get { return this.AjaxPager1; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //加入 二手车规模 页面权限
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT2206"))
                {
                    BindData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }
        private void BindData()
        {
            //查询
            int totalCount = 0;
            BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo qci = new BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo();
            qci.CustID = this.CustID;
            DataTable table = BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.GetBusinessScaleInfo(qci, "CreateTime desc", CurrentPage, PageSize, out totalCount);

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

        public string GetBusinessScale(string ID)
        {
            string result = "";

            if (!string.IsNullOrEmpty(ID)) result = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(int.Parse(ID));
            return result;
        }

    }
}