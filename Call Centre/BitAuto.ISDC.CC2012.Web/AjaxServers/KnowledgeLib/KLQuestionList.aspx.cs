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
    public partial class KLQuestionList : PageBase
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
        private string RequestQuestionStatus
        {
            get { return HttpContext.Current.Request["QuestionStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QuestionStatus"].ToString()); }
        }
        #endregion

        //判断权限 不具备权限的只能查看到自己提交、编辑的知识点信息
        int userID;
        bool right_Approval;  //审核通过
        bool right_Reject;  //驳回
        bool right_Move;    //移动
        bool right_Disable; //停用
        bool right_Delete;   //删除
        bool right_Edit;   //编辑
        bool right_QuestionDisable; //停用试题

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                right_Approval = BLL.Util.CheckRight(userID, "SYS024BUT3101");//"知识点管理—审核通过知识点"功能验证逻辑
                right_Reject = BLL.Util.CheckRight(userID, "SYS024BUT3102");//"知识点管理—驳回知识点"功能验证逻辑
                right_Move = BLL.Util.CheckRight(userID, "SYS024BUT3103");//"知识点管理—移动知识点"功能验证逻辑
                right_Disable = BLL.Util.CheckRight(userID, "SYS024BUT3104");//"知识点管理—停用知识点"功能验证逻辑
                right_Delete = BLL.Util.CheckRight(userID, "SYS024BUT3105");//"知识点管理—删除知识点"功能验证逻辑
                right_Edit = BLL.Util.CheckRight(userID, "SYS024BUT3108");//"知识点管理—编辑知识点"功能验证逻辑
                //试题停用按钮，如果没有权限，隐藏
                right_QuestionDisable = BLL.Util.CheckRight(userID, "SYS024BUT3107");//"知识点管理—停用试题"功能验证逻辑
                if (!right_QuestionDisable)
                {
                    btnDisable.Style.Add("display", "none");
                }
                BindData();
            }
        }
        public void BindData()
        {
            Entities.QueryKLQuestion query = new Entities.QueryKLQuestion();
            //if (!right_Approval && !right_Reject && !right_Disable)     //如果不具备 审核通过、驳回、停用的权限 则只能查看到自己提交的信息
            //{
            //    query.CreateUserID = userID;
            //}

            if (RequestTitle != "")
            {
                query.Ask = RequestTitle;
            }
            if (RequestKCID != "")
            {
                query.KCID = int.Parse(RequestKCID);
            }
            //if (RequestProperty != "")
            //{
            //    query.Property = RequestProperty;
            //}
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
                query.ModifyUserID = Convert.ToInt32(RequestModifyUserID);
            }
            if (RequestCategory != "")
            {
                query.AskCategorys = RequestCategory;
            }
            if (RequestQuestionStatus != "")
            {
                query.QuestionStatusS = RequestQuestionStatus;
            }

            string wherePlug = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", BLL.Util.GetLoginUserID());
            DataTable dt = BLL.KLQuestion.Instance.GetKLQuestionManage(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, wherePlug, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //试题分类
        public string getAskCategory(string askCategory)
        {
            string askCategoryStr = string.Empty;
            int _askCategory;
            if (int.TryParse(askCategory, out _askCategory))
            {
                switch (_askCategory)
                {
                    case 1: askCategoryStr = "单选";
                        break;
                    case 2: askCategoryStr = "复选";
                        break;
                    case 3: askCategoryStr = "主观题";
                        break;
                    case 4: askCategoryStr = "判断";
                        break;
                }
            }
            return askCategoryStr;
        }
        //分类
        public string getCategory(string kcid)
        {
            string categoryStr = string.Empty;
            int _kcid;
            if (int.TryParse(kcid, out _kcid))
            {
                Entities.KnowledgeCategory model = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(_kcid);
                if (model != null)
                {
                    categoryStr = model.Name;
                }
            }
            return categoryStr;
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

        //知识点状态名称
        public string getStatusName(string status)
        {
            string statusStr = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                switch (_status)
                {
                    case 0: statusStr = "未提交";
                        break;
                    case 1: statusStr = "待审核";
                        break;
                    case -1: statusStr = "已驳回";
                        break;
                    case 2: statusStr = "已发布";
                        break;
                    case 3: statusStr = "已停用";
                        break;
                    case 4: statusStr = "已删除";
                        break;
                }
            }
            return statusStr;
        }
        //试题状态名称
        public string getQuestionStatusName(string status)
        {
            string statusStr = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                switch (_status)
                {
                    case 0: statusStr = "已发布";
                        break;
                    case -2: statusStr = "已停用";
                        break;
                }
            }
            return statusStr;
        }

        //操作；libStatus：关联知识库信息 状态；状态为0-未提交；1-待审核；-1驳回；2-审核通过：显示编辑；其余状态3-停用；4-删除不显示
        public string getOperator(string libStatus, string klid, string createUserId)
        {
            string operStr = string.Empty;
            int _klid;
            if (int.TryParse(klid, out _klid))
            {
                if ((libStatus != "3" && libStatus != "4") && (right_Edit || createUserId == userID.ToString()))
                {
                    operStr = "<a href='KnowledgeEdit.aspx?kid=" + _klid + "#question' target='_blank'>编辑</a>&nbsp;";
                }
            }
            return operStr;
        }

        //如果试题状态是已发布（正常），则可以停用,否则禁用；
        public string getCheckStr(string questionStatus, string klqid)
        {
            string checkStr = "<input type='checkbox' name='chkOper' value='" + klqid + "'";
            if (questionStatus == "-2")
            {
                checkStr += " disabled='disabled'";
            }
            checkStr += " />";
            return checkStr;
        }
    }
}