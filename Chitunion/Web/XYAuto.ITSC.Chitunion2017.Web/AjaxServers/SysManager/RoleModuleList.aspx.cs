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
    public partial class RoleModuleList : PageBase
    {
        #region field
        private DataTable tableModule;
        private string m_roleId;
        public string RoleId
        {
            get
            {
                if (Request.QueryString["RoleID"] != null && Request.QueryString["RoleID"] != "")
                {
                    m_roleId = Request.QueryString["RoleID"];
                }
                return m_roleId;
            }
            set
            {

                m_roleId = value;
            }
        }
        //系统ID
        private string m_SysID;
        public string SysID
        {
            get
            {
                if (Request.QueryString["SysID"] != null && Request.QueryString["SysID"] != "")
                {
                    m_SysID = Request.QueryString["SysID"];
                }
                return m_SysID;
            }
            set
            {

                m_SysID = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataRole();
                getModuleAll();
                BindModule();
            }
        }

        #region event
        /// <summary>
        /// 绑定左边的repater并且设置好链接地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repeaterRole_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litRole = e.Item.FindControl("litRole") as Literal;
                if (litRole != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "RoleID").ToString() == RoleId)
                    {
                        litRole.Text = " <li class=\"linknow\"><a href='#t' onclick=\"javascript:clickE('" + SysID + "','" + DataBinder.Eval(e.Item.DataItem, "RoleID") + "')\">" + DataBinder.Eval(e.Item.DataItem, "rolename") + "</a></li>";
                    }
                    else
                    {
                        litRole.Text = " <li><a href='#t' onclick=\"javascript:clickE('" + SysID + "','" + DataBinder.Eval(e.Item.DataItem, "RoleID") + "')\">" + DataBinder.Eval(e.Item.DataItem, "rolename") + "</a></li>";
                    }
                }
            }
        }
        /// <summary>
        /// 设置角色
        /// </summary>
        /// <returns></returns>
        //public bool SaveRole()
        //{
        //        string ids = "";
        //        foreach (TreeNode node in tvModule.CheckedNodes)
        //        {
        //            if (node.Depth != 0)
        //            {
        //                ids += node.Value + ":";
        //            }
        //        }
        //        ids = ids.TrimEnd(':');
        //        if (Bll.RoleModule.Instance.InsertRoleModuleAll(RoleId, ids, SysID) > 0)
        //        {
        //            //ScriptHelper.ShowAlertAndRedirectScript("修改成功", "../AjaxServers/SystemManager/RoleModuleList.aspxNew?SysID=" + SysID + "&RoleID=" + Request.QueryString["RoleID"]);
        //            //return true;
        //        }
        //        else
        //        {
        //            //ScriptHelper.ShowAlertScript("修改失败");
        //            return false;
        //        }
        //}
        #endregion

        #region function
        /// <summary>
        /// 根据系统SysID绑定角色信息
        /// </summary>
        private void BindDataRole()
        {
            DataTable tableRole = BLL.SysRight.RoleInfo.Instance.GetRoleInfoBySysID(SysID);
            repeaterRole.DataSource = tableRole;
            repeaterRole.DataBind();
        }
        /// <summary>
        /// 获取所有的模块信息
        /// </summary>
        private void getModuleAll()
        {
            tableModule = BLL.SysRight.ModuleInfo.Instance.GetModuleInfoByRoleId(RoleId, SysID);
        }
        /// <summary>
        /// 绑定一级模块
        /// </summary>
        private void BindModule()
        {
            DataView dvBigModule = GetNewDataTable(tableModule, "pid=''");
            //设置数据源
            DataTable dt = dvBigModule.ToTable();
            DataRow dr = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                TreeNode node = new TreeNode(dr["modulename"].ToString(), dr["moduleid"].ToString());
                if (dr["y"].ToString() == "1")
                {
                    node.Checked = true;
                }
                node.NavigateUrl = "#" + dr["Level"].ToString() + dr["moduleid"].ToString();
                node.SelectAction = TreeNodeSelectAction.Expand;
                tvModule.Nodes.Add(node);
                BindChildModule(node);
            }
        }
        /// <summary>
        /// 绑定二级模块
        /// </summary>
        /// <param name="node"></param>
        private void BindChildModule(TreeNode node)
        {
            DataView dvChildModule = GetNewDataTable(tableModule, "pid='" + node.Value + "'");
            //设置数据源
            DataTable dt = dvChildModule.ToTable();
            DataRow dr = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                TreeNode childNode = new TreeNode(dr["modulename"].ToString(), dr["moduleid"].ToString());
                if (dr["y"].ToString() == "1")
                {
                    childNode.Checked = true;
                }
                childNode.NavigateUrl = "#" + dr["Level"].ToString() + dr["moduleid"].ToString();
                childNode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(childNode);
                BindChildModule(childNode);
            }
        }
        #endregion
    }
}