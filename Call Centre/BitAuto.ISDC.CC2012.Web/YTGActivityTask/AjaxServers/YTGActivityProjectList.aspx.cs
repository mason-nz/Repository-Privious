using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers
{
    public partial class YTGActivityProjectList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string ProjectName
        {
            get { return HttpContext.Current.Request["projectName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["projectName"].ToString()); }
        }
        private string Status
        {
            get { return HttpContext.Current.Request["status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString()); }
        }

        private string Zhuti
        {
            get { return HttpContext.Current.Request["zhuti"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["zhuti"].ToString()); }
        }
        private string cbeginTime
        {
            get { return HttpContext.Current.Request["cbeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["cbeginTime"].ToString()); }
        }
        private string bbeginTime
        {
            get { return HttpContext.Current.Request["bbeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["bbeginTime"].ToString()); }
        }
        private string hbeginTime
        {
            get { return HttpContext.Current.Request["hbeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["hbeginTime"].ToString()); }
        }
        private string cendTime
        {
            get { return HttpContext.Current.Request["cendTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["cendTime"].ToString()); }
        }
        private string bendTime
        {
            get { return HttpContext.Current.Request["bendTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["bendTime"].ToString()); }
        }
        private string hendTime
        {
            get { return HttpContext.Current.Request["hendTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["hendTime"].ToString()); }
        }

        #endregion
        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        //绑定数据
        public void BindData()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(
                string.Format(
                    " AND b.BGID IN( SELECT bgid from dbo.UserGroupDataRigth WHERE UserID={0} UNION ALL SELECT bgid FROM dbo.EmployeeAgent WHERE UserID={0}) ",
                    BLL.Util.GetLoginUserID()));

            if (!string.IsNullOrEmpty(ProjectName))
            {
                sbWhere.Append(string.Format(" and b.Name like '%{0}%' ",  StringHelper.SqlFilter(ProjectName)));
            }
            if (!string.IsNullOrEmpty(Zhuti))
            {
                sbWhere.Append(string.Format(" and c.ActivityName like '%{0}%' ", StringHelper.SqlFilter(Zhuti)));                
            }

            if (!string.IsNullOrEmpty(Status))
            {
                sbWhere.Append(string.Format(" and b.Status  in({0}) ", BLL.Util.SqlFilterByInCondition(Status)));
            }
            if (!string.IsNullOrEmpty(cbeginTime))
            {
                sbWhere.Append(string.Format(" and b.CreateTime >='{0}' ", StringHelper.SqlFilter(cbeginTime)));
            }
            if (!string.IsNullOrEmpty(bbeginTime))
            {
                sbWhere.Append(string.Format(" and c.SignBeginTime >='{0}' ", StringHelper.SqlFilter(bbeginTime)));
            }
            if (!string.IsNullOrEmpty(hbeginTime))
            {
                sbWhere.Append(string.Format(" and c.ActiveBeginTime >='{0}' ", StringHelper.SqlFilter(hbeginTime)));
            }

            if (!string.IsNullOrEmpty(cendTime))
            {
                sbWhere.Append(string.Format(" and c.CreateTime <='{0} 23:59:59' ", StringHelper.SqlFilter(cendTime)));
            }
            if (!string.IsNullOrEmpty(bendTime))
            {
                sbWhere.Append(string.Format(" and c.SignBeginTime <='{0} 23:59:59' ", StringHelper.SqlFilter(bendTime)));
                //sbWhere.Append(string.Format(" and c.SignEndTime <'{0} 23:59:59' ", bendTime));
            }
            if (!string.IsNullOrEmpty(hendTime))
            {
                sbWhere.Append(string.Format(" and c.ActiveBeginTime <='{0} 23:59:59' ", StringHelper.SqlFilter(hendTime)));
                //sbWhere.Append(string.Format(" and c.ActiveEndTime <'{0} 23:59:59' ", hendTime));
            }

            DataTable dt = BLL.YTGActivityTask.Instance.GetYTGProjectTasks(sbWhere.ToString(), "b.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}