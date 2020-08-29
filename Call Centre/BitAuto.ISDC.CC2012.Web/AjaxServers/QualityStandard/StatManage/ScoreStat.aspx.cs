using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.StatManage
{
    public partial class ScoreStat : PageBase
    {
        #region 属性定义
        private string RequestUserID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("UserID")); }
        }
        private string RequestGroupID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("GroupID")); }
        }     
        public string RequestRecordBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("RecordBeginTime")); }
        }
        public string RequestRecordEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("RecordEndTime")); }
        }
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD6301"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                BindScoreData();
            }
        }

        private void BindScoreData()
        {
            QueryQS_ScoreStat query = new QueryQS_ScoreStat();
            query.RequestUserID = RequestUserID;
            query.RequestGroupID = RequestGroupID;       
            query.RequestRecordBeginTime = RequestRecordBeginTime;
            query.RequestRecordEndTime = RequestRecordEndTime;

            int totalCount = 0;

            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultScoreStat(query, "", BLL.PageCommon.Instance.PageIndex, pageSize, tableEndName, out totalCount);

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            AjaxPager.PageSize = pageSize;
            AjaxPager.InitPager(totalCount);
        }
    }
}