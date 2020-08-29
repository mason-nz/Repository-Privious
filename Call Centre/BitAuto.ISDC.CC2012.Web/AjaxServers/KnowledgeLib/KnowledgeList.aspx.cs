using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    public partial class KnowledgeList : PageBase
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

        //判断权限 不具备权限的只能查看到自己提交、编辑的知识点信息
        int userID;
        bool right_Approval;  //审核通过
        bool right_Reject;  //驳回
        bool right_Move;    //移动
        bool right_Disable; //停用
        bool right_Delete;   //删除
        bool right_Edit;     //编辑

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3601"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    right_Approval = BLL.Util.CheckRight(userID, "SYS024BUT3101");
                    right_Reject = BLL.Util.CheckRight(userID, "SYS024BUT3102");
                    right_Move = BLL.Util.CheckRight(userID, "SYS024BUT3103");
                    right_Disable = BLL.Util.CheckRight(userID, "SYS024BUT3104");
                    right_Delete = BLL.Util.CheckRight(userID, "SYS024BUT3105");
                    right_Edit = BLL.Util.CheckRight(userID, "SYS024BUT3108");
                    BindData();
                }
            }
        }

        public void BindData()
        {
            Entities.QueryKnowledgeLib query = new QueryKnowledgeLib();
            //if (!right_Approval && !right_Reject && !right_Disable)     //如果不具备 审核通过、驳回、停用的权限 则只能查看到自己提交的信息
            //{
            //    query.CreateUserID = userID;
            //} --mod by yangyh 开放权限
            if (!right_Approval)
            {
                btnApproval.Style.Add("display", "none");
            }
            if (!right_Reject)
            {
                btnReject.Style.Add("display", "none");
            }
            if (!right_Disable)
            {
                btnDisable.Style.Add("display", "none");
            }
            if (!right_Move)
            {
                btnMove.Style.Add("display", "none");
            }
            if (!right_Delete)
            {
                btnDelete.Style.Add("display", "none");
            }

            if (RequestTitle != "")
            {
                query.Title = RequestTitle;
            }
            if (RequestKCID != "")
            {
                query.KCIDS = RequestKCID;
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
            if (RequestCategory != "")
            {
                query.Category = RequestCategory;
            }
            string wherePlus = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("KnowledgeLib", "BGID", "CreateUserID", BLL.Util.GetLoginUserID()) + "   AND KnowledgeLib.Status!=5 ";

            DataTable dt = BLL.KnowledgeLib.Instance.GetKnowledgeLib(query, "LastModifyTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount, wherePlus);
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
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
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

        //操作 根据状态获取不同操作；
        //状态：0-未提交（编辑、删除）；1-待审核（驳回、审核、删除(管理员)）；-1-驳回（编辑、删除）；2-审核通过（编辑、停用、删除）；3-已停用（删除）；
        public string getOperator(string klid, string status, string createUserId)
        {
            string operStr = string.Empty;
            int _klid;
            if (int.TryParse(klid, out _klid))
            {
                switch (status)
                {
                    case "0"://未提交
                    case "-1"://驳回
                        if (right_Edit || createUserId == userID.ToString())
                        {
                            operStr += edit(_klid);
                        }
                        if (right_Delete || createUserId == userID.ToString())
                        {
                            operStr += delete(_klid);
                        }
                        break;
                    case "1"://待审核 
                        if (right_Approval)
                        {
                            operStr += approval(_klid);
                        }
                        if (right_Delete)
                        {
                            operStr += delete(_klid);
                        }
                        break;
                    case "2":
                        //if (right_Approval && right_Reject && right_Disable) //已发布后只有管理员有权限编辑、停用、删除
                        //{
                        if (right_Edit || createUserId == userID.ToString())
                        {
                            operStr += edit(_klid);
                        }
                        if (right_Delete || createUserId == userID.ToString())
                        {
                            operStr += delete(_klid);
                        }
                        if (right_Disable) //已发布后只有管理员有权限停用、删除
                        {
                            operStr += disable(_klid);
                        }
                        //}
                        break;
                    case "3":
                        if (right_Delete || createUserId == userID.ToString())
                        {
                            operStr += delete(_klid);
                        }
                        break;
                }
            }
            return operStr;
        }
        //编辑
        public string edit(int klid)
        {
            return "<a href='KnowledgeEdit.aspx?kid=" + klid + "' target='_blank'>编辑</a>&nbsp;";
        }
        //删除
        public string delete(int klid)
        {
            return "<a href='javascript:void(0)' onclick='javascript:LibDelete(" + klid + ")'>删除</a>&nbsp;";
        }
        //驳回
        public string reject(int klid)
        {
            if (right_Reject)
            {
                return "<a href='KnowledgeLibAudit.aspx?KID=" + klid + "' target='_blank'>驳回</a>&nbsp;";
            }
            else { return ""; }
        }
        //审核
        public string approval(int klid)
        {
            if (right_Approval)
            {
                return "<a href='KnowledgeLibAudit.aspx?KID=" + klid + "' target='_blank'>审核</a>&nbsp;";
            }
            else { return ""; }
        }
        //停用
        public string disable(int klid)
        {
            if (right_Disable)
            {
                return "<a href='javascript:void(0)' onclick='javascript:LibDisable(" + klid + ")'>停用</a>&nbsp;";
            }
            else { return ""; }
        }
    }
}