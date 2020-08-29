using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCQuestionList : System.Web.UI.UserControl
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

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
         
        public void BindData()
        {
            Entities.QueryKLQuestion query = new Entities.QueryKLQuestion();
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
                query.LibStatus = int.Parse(RequestStatus);
            }
            if (RequestCreateUserID != "")
            {
                query.LibCreateUserID = int.Parse(RequestCreateUserID);
            }
            if (query.BeginTime != "")
            {
                query.BeginTime = query.BeginTime;
            }
            if (query.EndTime != "")
            {
                query.EndTime = query.EndTime;
            }
            if (RequestCategory != "")
            {
                query.Category = RequestCategory;
            }
            DataTable dt = BLL.KLQuestion.Instance.GetKLQuestion(query, "KLQuestion.CreateTime DESC", 1, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
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
                userName = BLL.Util.GetEmployeeNameByEID(BLL.Util.GetHrEIDByLimitEID(_userID));
            }
            return userName;
        }

        //状态名称
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
                    case 1: statusStr = "待审批";
                        break;
                    case -1: statusStr = "驳回";
                        break;
                    case 2: statusStr = "审核通过";
                        break;
                    case 3: statusStr = "已停用";
                        break;
                    case 4: statusStr = "已删除";
                        break;
                }
            }
            return statusStr;
        }

        //操作
        public string getOperator()
        {
            string operStr = string.Empty;

            return operStr;
        }
    }
}