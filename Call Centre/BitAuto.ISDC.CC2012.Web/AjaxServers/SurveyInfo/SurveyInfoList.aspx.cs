using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo
{
    public partial class SurveyInfoList : PageBase
    {
        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }
        private string RequestStatus
        {
            get { return HttpContext.Current.Request["status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString()); }
        }
        private string RequestIsAvailable
        {
            get { return HttpContext.Current.Request["isAvailable"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["isAvailable"].ToString()); }
        }
        private string RequestGroup
        {
            get { return HttpContext.Current.Request["group"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["group"].ToString()); }
        }
        private string RequestCategory
        {
            get { return HttpContext.Current.Request["category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["category"].ToString()); }
        }
        private string RequestCreater
        {
            get { return HttpContext.Current.Request["creater"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["creater"].ToString()); }
        }
        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["beginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["beginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["endTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["endTime"].ToString()); }
        }
        #endregion

        //按钮权限
        public bool right_edit; //编辑
        public bool right_delete;   //删除
        public bool right_newQuestionPaper; //生成新问卷
        public bool right_noUsed;   //停用
        public bool right_used;     //可用
        public bool right_view;     //查看
        private int userID;

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT5009"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                right_edit = BLL.Util.CheckRight(userID, "SYS024BUT5003");
                right_delete = BLL.Util.CheckRight(userID, "SYS024BUT5004");
                right_newQuestionPaper = BLL.Util.CheckRight(userID, "SYS024BUT5005");
                right_noUsed = BLL.Util.CheckRight(userID, "SYS024BUT5006");
                right_used = BLL.Util.CheckRight(userID, "SYS024BUT5007");
                right_view = BLL.Util.CheckRight(userID, "SYS024BUT5008");

                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            Entities.QuerySurveyInfo query = new Entities.QuerySurveyInfo();

            if (RequestName != "")
            {
                query.Name = StringHelper.SqlFilter(RequestName);
            }
            if (RequestStatus != "")
            {
                query.Statuss = StringHelper.SqlFilter(RequestStatus);
            }
            if (RequestIsAvailable != "")
            {
                query.IsAvailables = StringHelper.SqlFilter(RequestIsAvailable);
            }
            if (RequestGroup != "")
            {
                query.BGID = int.Parse(RequestGroup);
            }
            if (RequestCategory != "")
            {
                query.SCID = int.Parse(RequestCategory);
            }
            if (RequestCreater != "")
            {
                query.CreateUserID = int.Parse(RequestCreater);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = StringHelper.SqlFilter(RequestBeginTime);
            }
            if (RequestEndTime != "")
            {
                query.EndTime = StringHelper.SqlFilter(RequestEndTime);
            }

            query.LoginID = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.SurveyInfo.Instance.GetSurveyInfo(query, "SurveyInfo.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //获取操作人
        public string getOperator(string createrID)
        {
            string _operator = string.Empty;
            int _createrID;
            if (int.TryParse(createrID, out _createrID))
            {
                _operator = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_createrID);
            }
            return _operator;
        }

        //获取状态
        public string getStatus(string status)
        {
            string _statusStr = string.Empty;
            int _status;
            if (int.TryParse(status, out _status))
            {
                switch (_status)
                {
                    case 0: _statusStr = "未完成";
                        break;
                    case 1: _statusStr = "未使用";
                        break;
                    case 2: _statusStr = "已使用";
                        break;
                    case -1: _statusStr = "已删除";
                        break;
                }
            }
            return _statusStr;
        }

        //得到操作链接
        public string getOperLink(string status, string siid, string isAvailable)
        {
            string operLinkStr = string.Empty;

            operLinkStr += rightEdit(siid, status) + rightDelete(status, siid) + rightNewQuestionPaper(siid) + rightNoUsed(siid, status, isAvailable) + rightUsed(siid, status, isAvailable) + rightView(siid);

            return operLinkStr;
        }

        //编辑按钮权限：有按钮权限 且 状态为0-未完成、1-未使用
        private string rightEdit(string siid, string status)
        {
            string rightStr = string.Empty;

            if (right_edit && (status == "0" || status == "1"))
            {
                rightStr = "<a href='SurveyInfoEdit.aspx?SIID=" + siid + "' target='_blank' name='a_edit'>编辑</a>&nbsp;";
            }
            return rightStr;
        }

        //删除按钮权限：有功能权限 且 状态为0-未完成或者1-未使用
        private string rightDelete(string status, string siid)
        {
            string rightStr = string.Empty;

            if (right_delete && (status == "0" || status == "1"))
            {
                rightStr = "<a href='javascript:void(0);' onclick='javascript:ajaxPager_update(" + siid + ",-1,null,\"是否确认将该问卷删除？\")' name='a_delete'>删除</a>&nbsp;";
            }
            return rightStr;
        }

        //生成新问卷按钮权限：有功能权限 且 状态为已使用（根据该文件在项目表里是否有记录判定）
        private string rightNewQuestionPaper(string siid)
        {
            string rightStr = string.Empty;

            if (right_newQuestionPaper && judgeIsUsed(siid))
            {
                rightStr = "<a href='javascript:void(0);' onclick='javascript:ajaxPager_createNewQuestionPaper(" + siid + ")' name='a_newQuestionPaper'>生成新问卷</a>&nbsp;";
            }
            return rightStr;
        }

        //停用按钮权限：有功能权限 且 状态为1-未使用或2-已使用 且 是否可用状态为1-启用
        private string rightNoUsed(string siid, string status, string isAvailable)
        {
            string rightStr = string.Empty;

            if (right_noUsed && (status == "1" || status == "2") && isAvailable == "1")
            {
                rightStr = "<a href='javascript:void(0);' onclick='javascript:ajaxPager_update(" + siid + ",null,0,\"是否确认将该问卷置为停用？\")' name='a_noUsed'>停用</a>&nbsp;";
            }
            return rightStr;
        }

        //可用按钮权限：有功能权限 且 状态为1-未使用或2-已使用 且 是否可用状态为0-停用
        private string rightUsed(string siid, string status, string isAvailable)
        {
            string rightStr = string.Empty;

            if (right_used && (status == "1" || status == "2") && isAvailable == "0")
            {
                rightStr = "<a href='javascript:void(0);' onclick='javascript:ajaxPager_update(" + siid + ",null,1,\"是否确认将该问卷置为可用？\")' name='a_used'>启用</a>&nbsp;";
            }
            return rightStr;
        }

        //查看按钮权限：有功能权限
        private string rightView(string siid)
        {
            string rightStr = string.Empty;

            if (right_view)
            {
                rightStr = "<a target=\"_blank\" href='SurveyInfoView.aspx?SIID=" + siid + "' name='a_view'>查看</a>&nbsp;";
            }
            return rightStr;
        }

        //判断其状态是否是已使用，是-返回true；否-返回false
        public bool judgeIsUsed(string siid)
        {
            return true;
            bool isUsed = false;
            int _siid;
            if (int.TryParse(siid, out _siid))
            {
                Entities.QuerySurveyProjectInfo query = new Entities.QuerySurveyProjectInfo();
                query.SIID = _siid;
                int count;
                DataTable dt = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query, "", 1, 10000, out count);
                if (dt.Rows.Count > 0)
                {
                    isUsed = true;
                }
            }

            return isUsed;
        }
    }
}