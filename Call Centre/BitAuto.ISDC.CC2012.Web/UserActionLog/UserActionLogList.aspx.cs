using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.UserActionLog
{
    public partial class UserActionLogList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestStartTime
        {
            get { return Request["StartTime"] == null ? "" : HttpUtility.UrlDecode(Request["StartTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(Request["EndTime"].ToString()); }
        }
        private string RequestUserName
        {
            get { return Request["usrName"] == null ? "" : HttpUtility.UrlDecode(Request["usrName"].ToString()); }

        }
        private string RequestLoginInfo
        {
            get { return Request["LoginInfo"] == null ? "" : HttpUtility.UrlDecode(Request["LoginInfo"].ToString()); }

        }

        #endregion

        public int PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD5004"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindList();
                }
            }
        }

        //绑定数据列表
        private void BindList()
        {
            Entities.QueryUserActionLog query = new Entities.QueryUserActionLog();
            if (RequestStartTime != "")
            {
                query.StartTime = StringHelper.SqlFilter(RequestStartTime);
            }
            if (RequestEndTime != "")
            {
                query.EndTime = StringHelper.SqlFilter(RequestEndTime);
            }
            if (RequestUserName != "")
            {
                query.TrueName = StringHelper.SqlFilter(RequestUserName);
            }
            if (RequestLoginInfo != "")
            {
                query.Loginfo = StringHelper.SqlFilter(RequestLoginInfo);
            }

            DataTable dt = BLL.UserActionLog.Instance.GetUserActionLog(query, " CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            AjaxPager_Custs.PageSize = 20;
            AjaxPager_Custs.InitPager(RecordCount);
        }
    }
}