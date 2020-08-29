using System;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Web.UI.WebControls;
using System.Web.UI;
using BitAuto.YanFa.SysRightManager.Common;

namespace BitAuto.ISDC.CC2012.Web.Controls
{
    public partial class Top1 : System.Web.UI.MasterPage
    {
        public string YPFanXianURL = ConfigurationUtil.GetAppSettingValue("EPEmbedCCHBugCar_URL");//易湃签入CC惠买车任务页面，APPID
        public string TaskURL = ConfigurationUtil.GetAppSettingValue("EPEmbedCC_HBuyCarTaskURL");//易湃签入CC惠买车任务页面，APPID
        public string EPEmbedCC_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC惠买车页面，APPID
        public string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        public string parentID = "";//模块大类ID
        public string currentID = "";//当前子功能ID
        public string childStr = "", roleName = "";
        public bool IsShow = true;//是否显示菜单内容
        public bool IsMoniLogin = false;//是否显示模拟登录链接
        protected void Page_Load(object sender, EventArgs e)
        {
            currentID = FootPage.GetModuleID();
            parentID = PageBase.GetPid();
            int userid = BLL.Util.GetLoginUserID();
            roleName = UserInfo.Instance.GetRoleInfo(userid, sysID);
            IsMoniLogin = BLL.Util.CheckRight(userid, "SYS024BUT5104");//是否显示模拟登录链接
            if (IsShow)
            {
                DataTable dtParent = UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(Session["userid"]), sysID);
                dtParent.Columns.Add("classDesc", typeof(string));
                for (int i = 0; i < dtParent.Rows.Count; i++)
                {
                    DataTable dtChild = UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(Session["userid"]), sysID, dtParent.Rows[i]["moduleID"].ToString());
                    dtChild.DefaultView.RowFilter = "url <>''";
                    DataTable dtChildFilter = dtChild.DefaultView.ToTable();
                    if (dtChildFilter.Rows.Count > 0)
                    {
                        dtParent.Rows[i]["url"] = dtChildFilter.Rows[0]["url"];
                    }

                    if (parentID.ToString() == dtParent.Rows[i]["moduleID"].ToString())
                    {
                        dtParent.Rows[i]["classDesc"] = "current";
                        litMenuNav1.Text = "<a href='" + dtParent.Rows[i]["url"].ToString().Trim() + "' class='linkBlue' menulevel='1'>" + dtParent.Rows[i]["moduleName"].ToString().Trim() + "</a>";
                        dtChild.Columns.Add("classDesc", typeof(string));
                        for (int j = 0; j < dtChild.Rows.Count; j++)
                        {
                            if (currentID.ToString() == dtChild.Rows[j]["moduleID"].ToString())
                            {
                                dtChild.Rows[j]["classDesc"] = "current";
                                litMenuNav2.Text = "<a href='" + dtChild.Rows[j]["url"].ToString().Trim() + "' class='linkBlue' menulevel='2'>" + dtChild.Rows[j]["moduleName"].ToString().Trim() + "</a>";
                                //zxq 2012.08.28
                                litMenuNavTitle.Text = dtChild.Rows[j]["moduleName"].ToString().Trim();
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
                        dtChild.DefaultView.RowFilter = "PID='" + dtP.Rows[0]["PID"].ToString() + "'";

                        if (dtChild.DefaultView.ToTable().Rows.Count > 0)
                        {
                            dtChild.Columns.Add("classDesc", typeof(string));

                            dtParent.Rows[i]["classDesc"] = "current";
                            litMenuNav1.Text = "<a href='" + dtParent.Rows[i]["url"].ToString().Trim() + "' class='linkBlue'  menulevel='1'>" + dtParent.Rows[i]["moduleName"].ToString().Trim() + "</a>";

                            this.childRpt.DataSource = dtChild;
                            this.childRpt.DataBind();
                        }
                    }


                }

                //删除无效链接的一级菜单 强斐 2015-11-24
                DataRow[] drs = dtParent.Select();
                foreach (DataRow dr in drs)
                {
                    if (string.IsNullOrEmpty(dr["url"].ToString()))
                    {
                        dtParent.Rows.Remove(dr);
                    }
                }

                this.parentRpt.DataSource = dtParent;
                this.parentRpt.DataBind();
            }
        }


        protected void childRpt_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string moduleID = DataBinder.Eval(e.Item.DataItem, "moduleID").ToString().Trim();
                Repeater rept = e.Item.FindControl("thirdRpt") as Repeater;
                DataTable dtChild = UserInfo.Instance.GetChildModuleByUserId(Convert.ToInt32(Session["userid"]), sysID, moduleID);
                if (dtChild != null)
                {
                    dtChild.DefaultView.RowFilter = "url <>''";
                    dtChild = dtChild.DefaultView.ToTable();
                    dtChild.Columns.Add("classDesc", typeof(string));

                    if (dtChild != null && dtChild.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtChild.Rows.Count; i++)
                        {
                            if (currentID.ToString() == dtChild.Rows[i]["moduleID"].ToString())
                            {
                                dtChild.Rows[i]["classDesc"] = "current";
                                litMenuNav3.Text = "&gt; <label menulevel='3'>" + dtChild.Rows[i]["moduleName"].ToString().Trim() + "</label>";
                                //zxq 2010.08.28
                                //litMenuNav3.Text = "&gt; " + dtChild.Rows[i]["moduleName"].ToString().Trim();
                                litMenuNavTitle.Text = dtChild.Rows[i]["moduleName"].ToString().Trim();
                                DataTable dtP = UserInfo.Instance.GetModuleByModuleID(sysID, dtChild.Rows[i]["PID"].ToString());

                                if (dtP != null && dtP.Rows.Count > 0)
                                {
                                    litMenuNav2.Text = "<a href='" + dtP.Rows[0]["url"].ToString().Trim() + "' class='linkBlue' menulevel='2'>" + dtP.Rows[0]["moduleName"].ToString().Trim() + "</a>";
                                }
                            }
                        }
                        rept.DataSource = dtChild;
                        rept.DataBind();
                    }
                }
            }
        }

    }
}