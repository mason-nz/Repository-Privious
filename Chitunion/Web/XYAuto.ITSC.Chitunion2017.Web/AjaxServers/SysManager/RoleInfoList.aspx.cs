using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Web.Base;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager
{
    public partial class RoleInfoList : PageBase
    {
        public string SysID
        {
            get { return Request.QueryString["sysID"] + ""; }
        }
        //protected bool isSysRole = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();

                //CheckSysRole();
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        public void BindData()
        {
            //查询
            int totalCount;
            DataTable table = Retrieve(out totalCount);
            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                //设置分页控件

                repeater.DataSource = table;
            }

            ajaxPager.InitPager(totalCount, "divContent");
            //绑定列表数据
            repeater.DataBind();
        }
        /// <summary>
        /// 通过SysID获取角色信息
        /// </summary>
        /// <param name="totalCount">返回</param>
        /// <returns></returns>
        private DataTable Retrieve(out int totalCount)
        {
            //查询
            QueryRoleInfo queryRoleInfo = new QueryRoleInfo();
            if (SysID != "")
            {
                queryRoleInfo.SysID = SysID;
            }
            DataTable table = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(queryRoleInfo
                , PageIndex, PageSize, out totalCount);
            return table;
        }

        //private void CheckSysRole()
        //{
        //    if (string.IsNullOrEmpty(SysID))
        //    {
        //        return;
        //    }
        //    isSysRole = BLL.SysRight.UserRole.Instance.IsSysRole(Chitunion2017.Common.UserInfo.GetLoginUserID());
        //}
    }
}