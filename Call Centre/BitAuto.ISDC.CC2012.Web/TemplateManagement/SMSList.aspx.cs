using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class SMSList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
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
        private string RequestSMSTitle
        {
            get { return HttpContext.Current.Request["SMSTitle"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SMSTitle"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        //功能权限
        bool right_edit;        //编辑权限
        bool right_delete;      //删除权限

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD5104"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                right_edit = BLL.Util.CheckRight(userID, "SYS024MOD510402");
                right_delete = BLL.Util.CheckRight(userID, "SYS024MOD510403");
                BindData();
            }
        }

        public string Gethref(string url)
        {
            return url;
        }
        //绑定数据
        public void BindData()
        {
            Entities.QuerySMSTemplate query = new Entities.QuerySMSTemplate();

            if (!string.IsNullOrEmpty(RequestGroup))
            {
                query.BGID = int.Parse(RequestGroup);
            }
            if (!string.IsNullOrEmpty(RequestCategory))
            {
                query.SCID = int.Parse(RequestCategory);
            }
            if (!string.IsNullOrEmpty(RequestCreater))
            {
                query.CreateUserID = int.Parse(RequestCreater);
            }
            if (!string.IsNullOrEmpty(RequestSMSTitle))
            {
                query.Title = StringHelper.SqlFilter(RequestSMSTitle);
            }
            query.LoginID = BLL.Util.GetLoginUserID();

            int RecordCount = 0;

            DataTable dt = BLL.SMSTemplate.Instance.GetSMSTemplate(query, "a.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
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
        //得到操作链接
        public string getOperLink(string recID)
        {
            string operLinkStr = string.Empty;
            operLinkStr += rightEdit(recID);
            operLinkStr += rightDelete(recID);
            return operLinkStr;
        }
        //编辑按钮权限：有按钮权限 
        private string rightEdit(string recID)
        {
            string rightStr = string.Empty;

            if (right_edit)
            {
                rightStr = "<a href='javascript:void(0)' onclick='EditTemplate(" + recID + ")' name='a_edit'>编辑</a>&nbsp;";
            }
            return rightStr;
        }

        //删除按钮权限：有功能权限 
        private string rightDelete(string recID)
        {
            string rightStr = string.Empty;

            if (right_delete)
            {
                rightStr = "<a href='javascript:deleteTemplate(" + recID + ")'   name='a_delete'>删除</a>&nbsp;";
            }
            return rightStr;
        }
    }
}