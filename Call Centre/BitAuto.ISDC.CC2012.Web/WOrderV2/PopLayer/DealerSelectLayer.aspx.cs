using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class DealerSelectLayer : PageBase
    {
        #region 属性
        /// <summary>
        /// 经销商名称
        /// </summary>
        public string MemberName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MemberName"]);
            }
        }
        /// <summary>
        /// 经销商ID
        /// </summary>
        public string MemberCode
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberCode"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MemberCode"]);
            }
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustId
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CustId"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CustId"]);
            }
        }
        public string NeedMemeberCode
        {
            get
            {
                return (HttpContext.Current == null || string.IsNullOrEmpty(HttpContext.Current.Request["nmc"]) == true) ? "0" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["nmc"]);
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
                    DataBinds();
                BLL.Loger.Log4Net.Info(string.Format("【在“查询经销商”页面DealerSelect.aspx Step1--调用数据库耗时】{0}毫秒", (DateTime.Now - dtN).TotalMilliseconds));
            }
            BLL.Loger.Log4Net.Info(string.Format("【在“查询经销商”页面DealerSelect.aspx Step2--页面总耗时】{0}毫秒", (DateTime.Now - dt).TotalMilliseconds));
        }

        private void DataBinds()
        {
            if (!string.IsNullOrEmpty(MemberName.Trim()) || !string.IsNullOrEmpty(CustId.Trim()) || !string.IsNullOrEmpty(MemberCode.Trim()))
            {
                DataTable dt = null;
                int totalCount = 0;
                if (!string.IsNullOrEmpty(MemberName.Trim()) && NeedMemeberCode == "1")
                {
                    dt = BLL.DealerInfo.Instance.GetMemberInfo(MemberName.Trim(), "", CustId.Trim(), "", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                }
                else
                {
                    dt = BLL.DealerInfo.Instance.GetMemberInfo(MemberName.Trim(), MemberCode.Trim(), CustId.Trim(), "", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                }
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        repterFriendCustMappingList.DataSource = dt;
                        repterFriendCustMappingList.DataBind();
                    }
                    catch (Exception ex)
                    {
                        BLL.Loger.Log4Net.Error(ex.StackTrace);
                    }
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
            if ((!MemberCode.Equals("")) || MemberCode != null)
            {
                where += "&MemberCode=" + BLL.Util.EscapeString(MemberCode);
            }
            if ((!CustId.Equals("")) || CustId != null)
            {
                where += "&CustId=" + BLL.Util.EscapeString(CustId);
            }
            where += "&nmc=1&random=" + (new Random()).Next().ToString();
            return where;
        }
    }
}