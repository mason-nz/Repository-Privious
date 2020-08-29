using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using BitAuto.YanFa.SysRightManager.Common;
using System.Data;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.Web.Controls
{
    public partial class Top : System.Web.UI.MasterPage
    {
        public string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        public string parentID = "";//模块大类ID
        public string currentID = "";//当前子功能ID
        public string childStr = "", roleName = "";
        public int UserID = 0;//当前客服登陆后台集中权限系统UserID

        protected void Page_Load(object sender, EventArgs e)
        {
            currentID = FootPage.GetModuleID();
            parentID = PageBase.GetPid();
            UserID = BLL.Util.GetLoginUserID();
            //roleName = UserInfo.Instance.GetRoleInfo(Convert.ToInt32(Session["userid"].ToString()), sysID);
            roleName = UserInfo.Instance.GetRoleInfo(UserID, sysID);

            DataTable dtParent = UserInfo.Instance.GetParentModuleInfoByUserID(UserID, sysID);
            dtParent.Columns.Add("classDesc", typeof(string));
            for (int i = 0; i < dtParent.Rows.Count; i++)
            {
                DataTable dtChild = UserInfo.Instance.GetChildModuleByUserId(UserID, sysID, dtParent.Rows[i]["moduleID"].ToString());
                dtChild.DefaultView.RowFilter = "url <>''";
                DataTable dtChildFilter = dtChild.DefaultView.ToTable();
                if (dtChildFilter.Rows.Count > 0)
                {
                    dtParent.Rows[i]["url"] = dtChildFilter.Rows[0]["url"];
                }

                if (parentID.ToString() == dtParent.Rows[i]["moduleID"].ToString())
                {
                    dtParent.Rows[i]["classDesc"] = "cur";
                    //litMenuNav1.Text = "<a href='" + dtParent.Rows[i]["url"].ToString().Trim() + "' class='linkBlue' menulevel='1'>" + dtParent.Rows[i]["moduleName"].ToString().Trim() + "</a>";
                    dtChild.Columns.Add("classDesc", typeof(string));
                    for (int j = 0; j < dtChild.Rows.Count; j++)
                    {
                        if (currentID.ToString() == dtChild.Rows[j]["moduleID"].ToString())
                        {
                            dtChild.Rows[j]["classDesc"] = "cur";
                        }
                        else
                        {
                            dtChild.Rows[j]["classDesc"] = "";
                        }
                    }
                    this.childRpt.DataSource = dtChild;
                    this.childRpt.DataBind();
                }
                else
                {
                    dtParent.Rows[i]["classDesc"] = "";
                }
                DataTable dtP = UserInfo.Instance.GetModuleByModuleID(sysID, parentID.ToString());
                if (dtP != null && dtP.Rows.Count > 0)
                {
                    dtChild.DefaultView.RowFilter = "PID='" + StringHelper.SqlFilter(dtP.Rows[0]["PID"].ToString()) + "'";

                    if (dtChild.DefaultView.ToTable().Rows.Count > 0)
                    {
                        dtChild.Columns.Add("classDesc", typeof(string));
                        dtParent.Rows[i]["classDesc"] = "cur";
                        this.childRpt.DataSource = dtChild;
                        this.childRpt.DataBind();
                    }
                }


            }
            this.parentRpt.DataSource = dtParent;
            this.parentRpt.DataBind();
        }
    }
}