using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class KLClassifyReport : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private int RequestPid
        {
            get
            {
                try
                {
                    int npid = Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["pid"]));
                    return npid == -1 ? 0 : npid;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        private string RequestMBeginTime // （开始时间）
        {
            get { return HttpContext.Current.Request["mBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["mBeginTime"].ToString()); }
        }
        private string RequestMEndTime    // （结束时间）
        {
            get { return HttpContext.Current.Request["mEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["mEndTime"].ToString()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3603"))//"功能管理—分类统计"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }

        public void BindData()
        {
            string dtWhere = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("v", "BGID", "CreateUserID", BLL.Util.GetLoginUserID());

            if (!string.IsNullOrEmpty(RequestMBeginTime))
            {
                dtWhere += " AND v.CreateTime>='" + StringHelper.SqlFilter(RequestMBeginTime) + " 0:00:00'";
            }
            if (!string.IsNullOrEmpty(RequestMEndTime))
            {
                dtWhere += " AND v.CreateTime<='" + StringHelper.SqlFilter(RequestMEndTime) + " 23:59:59'";
            }
            string RegionId =
                BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(BLL.Util.GetLoginUserID()).RegionID.ToString();

            //默认是北京区域
            RegionId = string.IsNullOrEmpty(RegionId) ? "1" : RegionId;
            DataTable dt = BLL.KnowledgeLib.Instance.GetClassifyReport(BLL.Util.GetLoginUserID(), RequestPid, dtWhere, RegionId);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, dt.Rows.Count, 10000, BLL.PageCommon.Instance.PageIndex, 1);
        }

        public string ShowUrl(string fid, string strName, string lev)
        {
            //<a href="/KnowledgeLib/KLClassifyReport.aspx?pid=<%#Eval("fid")%>">

            if (lev == "1")
            {
                return string.Format("<div title='{1}'><a href='/KnowledgeLib/KLClassifyReport.aspx?pid={0}'>{1}</a></div>", fid, strName);
            }
            else
            {
                return string.Format("<div title='{0}'>{0}</div>", strName);
            }
        }

    }
}