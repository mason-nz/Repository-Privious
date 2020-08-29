using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class KnowledgeLibCount : PageBase
    {
        #region 属性
        private string RequestTitle
        {
            get { return HttpContext.Current.Request["Title"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Title"].ToString()); }
        }
        private string RequestKCID
        {
            get { return HttpContext.Current.Request["KCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCID"].ToString()); }
        }
        private string RequestProperty
        {
            get { return HttpContext.Current.Request["Property"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Property"].ToString()); }
        }
        private string RequestMBeginTime //修改时间 （开始时间）
        {
            get { return HttpContext.Current.Request["mBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["mBeginTime"].ToString()); }
        }
        private string RequestMEndTime    //修改时间 （结束时间）
        {
            get { return HttpContext.Current.Request["mEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["mEndTime"].ToString()); }
        }
        private string RequestModifyUserID
        {
            get { return HttpContext.Current.Request["ModifyUserID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ModifyUserID"].ToString()); }
        }
        private string RequestBeginTime //创建时间 （开始时间）
        {
            get { return HttpContext.Current.Request["BeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BeginTime"].ToString()); }
        }
        private string RequestEndTime    //创建时间 （结束时间）
        {
            get { return HttpContext.Current.Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EndTime"].ToString()); }
        }
        private string RequestStatus
        {
            get { return HttpContext.Current.Request["Status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString()); }
        }
        private string RequestCreateUserID
        {
            get { return HttpContext.Current.Request["CreateUserID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreateUserID"].ToString()); }
        }
        private string RequestCategory
        {
            get { return HttpContext.Current.Request["Category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Category"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        //判断权限 不具备权限的只能查看到自己提交、编辑的知识点信息
        int userID;
        bool right_Approval;  //审核通过
        bool right_Reject;  //驳回
        bool right_Move;    //移动
        bool right_Disable; //停用
        bool right_Delete;   //删除

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3602"))//"功能管理—上传统计"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                right_Approval = BLL.Util.CheckRight(userID, "SYS024BUT3101");//"知识点管理—审核通过知识点"功能验证逻辑
                right_Reject = BLL.Util.CheckRight(userID, "SYS024BUT3102");//"知识点管理—驳回知识点"功能验证逻辑
                right_Move = BLL.Util.CheckRight(userID, "SYS024BUT3103");//"知识点管理—移动知识点"功能验证逻辑
                right_Disable = BLL.Util.CheckRight(userID, "SYS024BUT3104");//"知识点管理—停用知识点"功能验证逻辑
                right_Delete = BLL.Util.CheckRight(userID, "SYS024BUT3105");//"知识点管理—删除知识点"功能验证逻辑
                BindData();
            }
        }
        public void BindData()
        {
            Entities.QueryKnowledgeLib query = new Entities.QueryKnowledgeLib();
            if (!right_Approval && !right_Reject && !right_Disable)     //如果不具备 审核通过、驳回、停用的权限 则只能查看到自己提交的信息
            {
                query.CreateUserID = userID;
            }

            if (RequestTitle != "")
            {
                query.Title = RequestTitle;
            }
            if (RequestKCID != "")
            {
                query.KCID = int.Parse(RequestKCID);
            }
            if (RequestProperty != "")
            {
                query.Property = RequestProperty;
            }
            if (RequestStatus != "")
            {
                query.StatusS = RequestStatus;
            }
            if (RequestCreateUserID != "")
            {
                query.CreateUserID = int.Parse(RequestCreateUserID);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = RequestBeginTime;
            }
            if (RequestEndTime != "")
            {
                query.EndTime = RequestEndTime;
            }
            if (RequestCategory != "")
            {
                query.Category = RequestCategory;
            }
            if (RequestMBeginTime != "")
            {
                query.MBeginTime = RequestMBeginTime;
            }
            if (RequestMEndTime != "")
            {
                query.MEndTime = RequestMEndTime;
            }
            if (RequestModifyUserID != "")
            {
                query.LastModifyUserID = int.Parse(RequestModifyUserID);
            }
            query.UserID = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.KnowledgeLib.Instance.GetKnowledgeLibCount(query, BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        //创建人
        public string getCreateUserName(string userID)
        {
            string userName = string.Empty;
            int _userID;
            if (int.TryParse(userID, out _userID))
            {
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
            }
            return userName;
        }
    }
}