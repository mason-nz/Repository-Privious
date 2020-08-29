using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    public partial class ProjectSelect : System.Web.UI.Page
    {
        #region 定义属性

        public string RequestPageSize
        {
            get { return Request.QueryString["pageSize"] == null ? PageCommon.Instance.PageSize.ToString() : Request.QueryString["pageSize"].Trim(); }
        }
        //项目名称
        public string RequestProjectName
        {
            get { return Request.QueryString["ProjectName"] == null ? string.Empty : Request.QueryString["ProjectName"]; }
        }
        //分组
        public string RequestBGID
        {
            get { return Request.QueryString["BGID"] == null ? string.Empty : Request.QueryString["BGID"]; }
        }

        public int PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;
        public string FzPerson = string.Empty;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                ddlBussiGroupBind();
                BindData();
            }

        }
        #endregion

        private void ddlBussiGroupBind()
        {
            DataTable bgdt = new DataTable();

            int userid = BLL.Util.GetLoginUserID();
            //所属分组
            bgdt = BLL.EmployeeSuper.Instance.GetCurrentUseBusinessGroup(userid);

            //添加请选择
            if (bgdt != null)
            {
                DataRow dr = bgdt.NewRow();
                dr[0] = "-2";
                dr[1] = "请选择";
                bgdt.Rows.InsertAt(dr, 0);
            }
            //绑定数据
            ddlBussiGroup.DataSource = bgdt;
            ddlBussiGroup.DataTextField = "Name";
            ddlBussiGroup.DataValueField = "BGID";
            ddlBussiGroup.DataBind();
        }

        #region function
        private void BindData()
        {
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            if (!string.IsNullOrEmpty(RequestProjectName))
            {
                query.Name = RequestProjectName;
            }
            if (!string.IsNullOrEmpty(RequestBGID))
            {
                int _bgid = 0;
                if (int.TryParse(RequestBGID, out _bgid))
                {
                    query.BGID = _bgid;
                }
            }
            //项目类型
            query.Sources = "4,6";
            int count;
            DataTable dt;
            dt = BLL.ProjectInfo.Instance.GetLastestProjectByUserID(query, " a.createtime desc", BLL.PageCommon.Instance.PageIndex, PageSize, out count, userID);
            RecordCount = count;
            repterProjectlist.DataSource = dt;
            repterProjectlist.DataBind();
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, PageCommon.Instance.PageIndex, 2);
        }
        #endregion
    }
}