using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    public partial class CustAssignUserList : System.Web.UI.Page
    {
        #region 属性
        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TrueName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TrueName"]);
            }
        }
        /// <summary>
        /// 所属分组
        /// </summary>
        public string BGID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["BGID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGID"]);
            }
        }
        /// <summary>
        /// 有数据权限传1，没有数据权限传0
        /// </summary>
        public string IsHaveCompetence
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["IsHaveCompetence"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["IsHaveCompetence"]);
            }
        }

        ///// <summary>
        ///// 选择项显示的分组
        ///// </summary>
        //public string DisplayGroupID
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(HttpContext.Current.Request["DisplayGroupID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["DisplayGroupID"]);
        //    }
        //}
        #endregion

        public int GroupLength = 5;
        public int PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            //BGID = string.IsNullOrEmpty(HttpContext.Current.Request["BGID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGID"]);
            if (!IsPostBack)
            {
                literal_title.Text = "选择客服";
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                ddlBussiGroupBind();
                BindData();
            }
        }

        /// <summary>
        /// 绑定分组
        /// </summary>
        private void ddlBussiGroupBind()
        {

            DataTable bgdt = null;
            //有数据权限
            if (IsHaveCompetence == "1")
            {
                int userid = BLL.Util.GetLoginUserID();
                //所属分组
                bgdt = BLL.BaseData.Instance.GetUserGroupDataRigth(BLL.Util.GetLoginUserID());

            }
            //没有数据权限
            else
            {
                bgdt = BLL.BaseData.Instance.GetUserGroupDataRigth();
            }
            //添加请选择
            DataRow dr = bgdt.NewRow();
            dr[0] = -1;
            dr[1] = "请选择";
            bgdt.Rows.InsertAt(dr, 0);


            //绑定数据
            ddlBussiGroup.DataSource = bgdt;
            ddlBussiGroup.DataTextField = "Name";
            ddlBussiGroup.DataValueField = "BGID";
            ddlBussiGroup.DataBind();

            //传入组id
            if (!string.IsNullOrEmpty(BGID)&&BGID!="-1")
            {
                ddlBussiGroup.Value = BGID;
            }
            else if (string.IsNullOrEmpty(BGID)||BGID=="-1")
            {
                ddlBussiGroup.Value = "-1";
            }
        }

        /// 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            int RecordCount = 0;
            QueryEmployeeSuper query = new QueryEmployeeSuper();
            //分页参数赋值
            if (string.IsNullOrEmpty(BGID) || BGID == "-1")
            {
                string str = "";
                foreach (ListItem item in ddlBussiGroup.Items)
                {
                    if (item.Value != "-1")
                    {
                        str += item.Value + ",";
                    }
                }
                if (str.Length > 0)
                {
                    query.BGIDs = str.TrimEnd(',');
                }
                else
                {
                    query.BGIDs = null;
                }
            }
            else
            {
                query.BGIDs = BGID;
            }

            //用户名称
            if (!string.IsNullOrEmpty(TrueName))
            {
                query.TrueName = TrueName;
            }
            //按条件找人：条件-部门，角色-
            DataTable dt = null;
            dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);

            repterPersonlist.DataSource = dt;
            repterPersonlist.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 2);
        }
    }
}