using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Web.Base;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager
{
    public partial class ViewRoleByUser : PageBase
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID
        {
            get { return Request.QueryString["roleID"] + ""; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }
        /// <summary>
        /// 绑定角色拥有者的数据
        /// </summary>
        public void BindData()
        {
            if (RoleID != "")
            {
                int totalCount;
                DataTable dt = BLL.SysRight.UserRole.Instance.GetUserRole("and UserRole.RoleID='" + RoleID + "'", PageIndex, PageSize, out totalCount);

                if (dt.Rows.Count > 0)
                {
                    repeater.DataSource = dt;
                }
                ajaxPager.InitPager(totalCount, "divContent");
                //绑定列表数据
                repeater.DataBind();
            }
        }
        protected string GetStatus(object obj)
        {
            string status = obj.ToString();
            string statusName = "";
            if (status == "0")
            {
                statusName = "启用";
            }
            else if (status == "1")
            {
                statusName = "停用";
            }
            else if (status == "-1")
            {
                statusName = "删除";
            }
            return statusName;
        }
    }
}