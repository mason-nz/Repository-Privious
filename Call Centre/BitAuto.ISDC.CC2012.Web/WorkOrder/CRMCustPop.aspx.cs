using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class CRMCustPop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性

        public string NeedPageLoad
        {
            get
            {
                return (HttpContext.Current == null || string.IsNullOrEmpty(HttpContext.Current.Request["npd"]) == true) ? "0" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["npd"]);
            }
        }

        public string CustName
        {
            get
            {
                if (Request["CustName"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustName"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion
        public int GroupLength = 5;
        public int PageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            if (!IsPostBack)
            { 
                DateTime dtN = DateTime.Now;
                DataBinds();
                BLL.Loger.Log4Net.Info(string.Format("【在“客户名称查询”页面CRMCustPop.aspx，--数据库耗时】{0}毫秒", (DateTime.Now - dtN).TotalMilliseconds));
            }
            BLL.Loger.Log4Net.Info(string.Format("【在“客户名称查询”页面CRMCustPop.aspx，--总耗时】{0}毫秒", (DateTime.Now - dt).TotalMilliseconds));
        }

        private void DataBinds()
        {
            int totalCount = 0;


            if (!string.IsNullOrEmpty(CustName))
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustInfo query = new YanFa.Crm2009.Entities.QueryCustInfo();
                if (CustName != "")
                {
                    query.CustName = CustName;
                }
                if (NeedPageLoad == "1")
                {
                    DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "",
                        PageCommon.Instance.PageIndex, PageSize, out totalCount);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        repterFriendCustMappingList.DataSource = dt;
                        repterFriendCustMappingList.DataBind();
                    }
                    litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount,
                        PageSize, PageCommon.Instance.PageIndex, 1);
                }
            }
        }

        private string GetWhere()
        {
            string where = "";
            string query = Request.Url.Query;

            if ((!CustName.Equals("")) || CustName != null)
            {
                where += "&CustName=" + BLL.Util.EscapeString(CustName);
            }
            where += "&npd=1&random=" + (new Random()).Next().ToString();
            return where;
        }

        public string GetTypeName(string typeID)
        {
            string name = "";
            int _typeID;
            if (int.TryParse(typeID, out _typeID))
            {
                name = BLL.Util.GetEnumOptText(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomType), _typeID);
            }

            return name;

        }

    }
}