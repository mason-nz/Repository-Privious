using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory
{
    public partial class DealerSelect : PageBase
    {
        #region 属性
        public string MemberName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MemberName"]);
            }
        }

        public string NeedPageLoad
        {
            get
            {
                return (HttpContext.Current == null || string.IsNullOrEmpty(HttpContext.Current.Request["npd"]) == true) ? "0" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["npd"]);
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

                this.txtMemberName.Value = MemberName;
                DateTime dtN = DateTime.Now;
                if (NeedPageLoad == "1")
                {
                    DataBinds();
                }
                BLL.Loger.Log4Net.Info(string.Format("【在“查询经销商”页面DealerSelect.aspx Step1--调用数据库耗时】{0}毫秒", (DateTime.Now - dtN).TotalMilliseconds));
            }
            BLL.Loger.Log4Net.Info(string.Format("【在“查询经销商”页面DealerSelect.aspx Step2--页面总耗时】{0}毫秒", (DateTime.Now - dt).TotalMilliseconds));
        }

        private void DataBinds()
        {
            if (!string.IsNullOrEmpty(MemberName))
            {
                int totalCount = 0;


                DataTable dt = BLL.DealerInfo.Instance.GetMemberInfo(MemberName,"","", "", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                if (dt != null && dt.Rows.Count > 0)
                {
                    repterFriendCustMappingList.DataSource = dt;
                    repterFriendCustMappingList.DataBind();
                }
                litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);
            }
        }

        private string GetWhere()
        {
            string where = "";
            string query = Request.Url.Query;

            if ((!MemberName.Equals("")) || MemberName != null)
            {
                where += "&MemberName=" + BLL.Util.EscapeString(MemberName);
            }
            where += "&npd=1&random=" + (new Random()).Next().ToString();
            return where;
        }
    }
}