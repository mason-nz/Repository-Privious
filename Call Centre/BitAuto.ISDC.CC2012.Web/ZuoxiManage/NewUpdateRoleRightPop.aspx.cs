using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class NewUpdateRoleRightPop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string UserName
        {
            get
            {
                return Request.QueryString["userNames"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["userNames"].ToString());
            }

        }

        public string UserID
        {
            get
            {
                return Request.QueryString["userIDs"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["userIDs"].ToString());
            }

        }
        public string AgentNum
        {
            get
            {
                return Request.QueryString["agentNum"] == null ? String.Empty : Request.QueryString["agentNum"].ToString();
            }
        }

        public string UserRolesIDs = "";//角色
        public string AtGroupID = "";//所在组
        public string ManagerGroupIDs = "";//管辖分组
        public string AreaID = "";//所属区域

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindControl();
            }
        }

        private void DataBindControl()
        {
            BindRoles();

            //BindGroup();

            ShowRoles();

            GetGroupInfo();
        }

        private void GetGroupInfo()
        {
            //所属分组
            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(int.Parse(UserID));
            if (model != null)
            {
                AtGroupID = model.BGID.ToString();
                AreaID = model.RegionID.ToString();

            }

            //管辖分组
            Entities.QueryUserGroupDataRigth query = new Entities.QueryUserGroupDataRigth();
            query.UserID = int.Parse(UserID);
            int totalCount = 0;
            DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigth(query, "", 1, 1000, out totalCount);
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                ManagerGroupIDs += dr["BGID"].ToString() + ",";
            }
            if (ManagerGroupIDs != "")
            {
                ManagerGroupIDs = ManagerGroupIDs.Substring(0, ManagerGroupIDs.Length - 1);
            }


        }

        private void ShowRoles()
        {
            DataTable dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(int.Parse(UserID), ConfigurationUtil.GetAppSettingValue("ThisSysID"));
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserRolesIDs += dr["RoleID"].ToString() + ",";
                }

                if (UserRolesIDs != "")
                {
                    UserRolesIDs = UserRolesIDs.Substring(0, UserRolesIDs.Length - 1);
                }
            }
        }

        private void BindRoles()
        {
            string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID(sysID);
            if (db != null && db.Rows.Count > 0)
            {
                int userid = BLL.Util.GetLoginUserID();
                DataTable rolesDt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
                DataRow[] rows = rolesDt.Select("RoleName='超级管理员'");
                if (rows.Length == 0)
                {
                    //没有超级管理员角色，就删除超级管理员的选项
                    for (int i=0;i<db.Rows.Count;i++)
                    {
                        if (db.Rows[i]["RoleName"].ToString() == "超级管理员")
                        {
                            db.Rows.RemoveAt(i);
                            break;
                        }
                    }
                }

                Rpt_Role.DataSource = db;
                Rpt_Role.DataBind();
            }
        }
    }
}