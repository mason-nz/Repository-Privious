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
    public partial class WorkloadStat : PageBase
    {
        #region 参数
        private string RequestCreateUserID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CreateUserID")); }
        }
        private string RequestGroupID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("GroupID")); }
        }
        public string RequestBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BeginTime")); }
        }
        public string RequestEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("EndTime")); }
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
                if (!BLL.Util.CheckRight(userId, "SYS024MOD6303"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindWorkloadData();
                }
            }
        }

        private void BindWorkloadData()
        {
            string where = string.Empty;
            int userId = 0;
            if (int.TryParse(RequestCreateUserID, out userId) && userId > 0)
            {
                where += " And qsr.CreateUserID=" + userId;
            }
            int groupId = 0;
            if (int.TryParse(RequestGroupID, out groupId) && groupId > 0)
            {
                where += " And CreateUserID IN (SELECT userid FROM UserGroupDataRigth WHERE BGID=" + groupId + " )";
            }
            DateTime beginTime;
            DateTime endTime;
            if (DateTime.TryParse(RequestBeginTime, out beginTime))
            {
                where += " And qsr.CreateTime>='" + beginTime.ToString() + "'";
            }
            if (DateTime.TryParse(RequestEndTime, out endTime))
            {
                where += " And qsr.CreateTime<='" + endTime.Add(new TimeSpan(23, 59, 59)).ToString() + "'";
            }
            DateTime recordBeginTime;
            DateTime recordEndTime;
            if (DateTime.TryParse(RequestRecordBeginTime, out recordBeginTime))
            {
                where += " And cri.CreateTime>='" + recordBeginTime.ToString() + "'";
            }
            if (DateTime.TryParse(RequestRecordEndTime, out recordEndTime))
            {
                where += " And cri.CreateTime<='" + recordEndTime.Add(new TimeSpan(23, 59, 59)).ToString() + "'";
            }

            #region 数据权限判断
            int loginUserId = BLL.Util.GetLoginUserID();

            string whereDataRight = "";
            whereDataRight = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("cri", "BGID", "CreateUserID", loginUserId);

            where += whereDataRight;
            #endregion

            int totalCount = 0;
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultWorkloadStat(where, "", BLL.PageCommon.Instance.PageIndex, pageSize, tableEndName, out totalCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            AjaxPager.PageSize = pageSize;
            AjaxPager.InitPager(totalCount);
        }

        public string GetGroupName(string userId)
        {
            int id = 0;
            string name = string.Empty;
            if (int.TryParse(userId, out id))
            {
                name = BLL.UserGroupDataRigth.Instance.GetUserGroupNamesStr(id);
            }

            return name;
        }
    }
}