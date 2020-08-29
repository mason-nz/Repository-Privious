using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;
namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class UpdateUsersRight : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string UserName
        {
            get
            {
                return Request.QueryString["userNames"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["userNames"].ToString());
            }

        }

        public string UserIDs
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

        /// <summary>
        /// 是否修改单个用户(false:批量修改，true：单个设置)（单个用户时，要修改工号）
        /// </summary>
        public string IsModfiySingle
        {
            get
            {
                return HttpContext.Current.Request["single"] == null ? string.Empty : HttpContext.Current.Request["single"].ToString();
            }
        }

        /// <summary>
        /// 分组
        /// </summary>
        public int ArealID
        {
            get
            {
                return HttpContext.Current.Request["arealid"] == null ? -2 : 
                    int.Parse(
                    string.IsNullOrEmpty(HttpContext.Current.Request["arealid"].ToString().Trim())
                    ? "-2" : HttpContext.Current.Request["arealid"].ToString().Trim()
                    );
            }
        }

        /// <summary>
        /// 数据权限ID
        /// </summary>
       public string DataRightTyle
        {
            get
            {
                return HttpContext.Current.Request["dataright"] == null ? string.Empty : HttpContext.Current.Request["dataright"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID
                (sysID);
            if (db != null && db.Rows.Count > 0)
            {
                int userid = BLL.Util.GetLoginUserID();
                DataTable rolesDt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
                DataRow[] rows = rolesDt.Select("RoleName='超级管理员'");
                if (rows.Length == 0)
                {
                    //没有超级管理员角色，就删除超级管理员的选项
                    for (int i = 0; i < db.Rows.Count; i++)
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

            DataTable dtGroup = BLL.BusinessGroup.Instance.GetBusinessGroupByAreaID(ArealID);

            if (dtGroup != null && dtGroup.Rows.Count > 0)
            {

                Rpt_BusinessGroup.DataSource = dtGroup.Select("Status=0").CopyToDataTable();
                Rpt_BusinessGroup.DataBind();

            }

            if (IsModfiySingle == "false")
            {
                //如果是批量修改
                this.divAgentNum.Visible = false;
                this.divData.Visible = false;
            }
            else
            {
                BingData();
            }

        }

        private void BingData()
        {
            int intVal = 0;
            if (int.TryParse(UserIDs, out intVal))
            {
               DataTable dt= BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID
                   (int.Parse(UserIDs), ConfigurationUtil.GetAppSettingValue("ThisSysID"));
            }
        }


    }
}