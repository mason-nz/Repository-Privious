using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class QuestionManageDetails : PageBase
    {

        public string Title
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Title").ToString();
            }
        }
        public string CreateUserName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CreateUserName").ToString();
            }
        }
        public string CreateBeginTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CreateBeginTime").ToString();
            }
        }
        public string CreateEndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CreateEndTime").ToString();
            }
        }
        public string KLCId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("KCType").ToString();
            }
        }
        public string AnswerUserId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AnswerUserId").ToString();
            }
        }
        public string AnswerBeginTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AnswerBeginTime").ToString();
            }
        }
        public string AnswerEndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("AnswerEndTime").ToString();
            }
        }
        public string Status
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Status").ToString();
            }
        }
        public string RegionID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("RegionID").ToString();
            }
        }

        public int PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3608"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                BindData();
            }
        }
        //绑定数据
        public void BindData()
        {
            Entities.QueryKLRaiseQuestions query = new Entities.QueryKLRaiseQuestions();
            string questionedUserName = "";
            if (!string.IsNullOrEmpty(Title))
            {
                query.Title = Title;
            }
            if (!string.IsNullOrEmpty(CreateUserName))
            {
                if (CreateUserName != "-1")
                {
                    questionedUserName = " And a.CreateUserName='" + StringHelper.SqlFilter(CreateUserName) + "'";
                }
            }
            if (!string.IsNullOrEmpty(CreateBeginTime))
            {
                query.CreateBeginTime = Convert.ToDateTime(CreateBeginTime);
            }
            if (!string.IsNullOrEmpty(CreateEndTime))
            {
                query.CreateEndTime = Convert.ToDateTime(CreateEndTime);
            }
            if (!string.IsNullOrEmpty(KLCId))
            {
                if (KLCId != "-1")
                {
                    query.KLCId = int.Parse(KLCId);
                }
            }
            if (!string.IsNullOrEmpty(RegionID))
            {
                query.RegionID = int.Parse(RegionID);
            }
            if (!string.IsNullOrEmpty(AnswerUserId))
            {
                if (AnswerUserId != "-1")
                {
                    query.AnswerUser = int.Parse(AnswerUserId);
                }
            }
            if (!string.IsNullOrEmpty(AnswerBeginTime))
            {
                query.AnswerBeginTime = Convert.ToDateTime(AnswerBeginTime);
            }
            if (!string.IsNullOrEmpty(AnswerEndTime))
            {
                query.AnswerEndTime = Convert.ToDateTime(AnswerEndTime);
            }
            if (!string.IsNullOrEmpty(Status))
            {
                query.QueryStatuses = Status;
            }

            string bgids = "";
            int userid = BLL.Util.GetLoginUserID();
            DataTable dtbgid = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userid);
            foreach (DataRow row in dtbgid.Rows)
            {
                bgids += "," + row["BGID"].ToString();
            }
            if (bgids != "")
            {
                bgids = bgids.Substring(1, bgids.Length - 1);

            }
            query.QueryBGIDs = bgids;

           
            int RecordCount = 0;

            DataTable dt = BLL.Personalization.Instance.GetKLRaiseQuestionDataForManage(query, questionedUserName, "a.NewCreateDate desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterList.DataSource = dt;
            repeaterList.DataBind();

            AjaxPager.PageSize = 10;
            AjaxPager.InitPager(RecordCount);
        }
    }
}