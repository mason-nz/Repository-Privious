using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Web.Base;
using XYAuto.Utils;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Web.SysManager
{
    public partial class RoleInfoList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
            if (Request.QueryString["SysID"] != null)
            {
                ScriptHelper.ShowCustomScript(Page, "openAjaxPopupCommen('" + Request.QueryString["SysID"] + "');");//  openAjaxPopupCommen(DepartID,ShowUserAll)
            }
        }
        /// <summary>
        /// 获取所有的系统名称
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSysInfoAll()
        {
            return BLL.SysRight.SysInfo.Instance.GetSysInfoAll();
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        public void BindData()
        {
            tv.Nodes.Clear();
            DataTable dt = GetSysInfoAll();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode node = new TreeNode();
                    node.NavigateUrl = "javascript:openAjaxPopupCommen('" + dt.Rows[i]["SysID"] + "')";
                    node.Text = dt.Rows[i]["SysName"].ToString();
                    node.Expanded = true;
                    tv.Nodes.Add(node);
                    DataTable datatable = getRole(dt.Rows[i]["SysID"].ToString());
                    if (datatable.Rows.Count > 0)
                    {
                        treeData(dt.Rows[i]["SysID"].ToString(), node);
                    }
                }
            }
            tv.DataBind();
            Expandnodes(tv, false);
            FindInTree("javascript:openAjaxPopupCommen('" + Request.QueryString["SysID"] + "')");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysID"></param>
        /// <param name="treenode"></param>
        private static void treeData(string SysID, TreeNode treenode)
        {
            DataTable dt = getRole(SysID);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode node = new TreeNode();
                    node.NavigateUrl = "javascript:openAjaxViewRoleByUser('" + dt.Rows[i]["RoleID"] + "')";
                    node.Text = dt.Rows[i]["RoleName"].ToString();
                    node.Expanded = true;
                    treenode.ChildNodes.Add(node);
                }
            }
        }
        /// <summary>
        /// 通过系统ID获取对应的角色信息
        /// </summary>
        /// <param name="SysID"></param>
        /// <returns></returns>
        public static DataTable getRole(string SysID)
        {
            return BLL.SysRight.RoleInfo.Instance.GetRoleInfoBySysID(SysID);
        }
        /// <summary>
        ///  Expandnodes(Me.TreeView1,   true)展开全部节点，Expandnodes(Me.TreeView1,   false)折叠全部节点
        /// </summary>
        /// <param name="treeview"></param>
        /// <param name="Expand"></param>
        private static void Expandnodes(TreeView treeview, bool Expand)
        {
            foreach (TreeNode n in treeview.Nodes)
            {
                if (n.ChildNodes.Count > 0)
                {
                    n.Expanded = Expand;
                    Expandnodes(n, Expand);
                }
            }
        }
        /// <summary>
        /// 子节点遍历折叠
        /// </summary>
        /// <param name="node"></param>
        /// <param name="Expand"></param>
        private static void Expandnodes(TreeNode node, bool Expand)
        {
            foreach (TreeNode n in node.ChildNodes)
            {
                if (n.ChildNodes.Count > 0)
                {
                    n.Expanded = Expand;
                    Expandnodes(n, Expand);
                }
            }
        }
        private void FindInTree(string strNodeName)
        {
            foreach (TreeNode tn in tv.Nodes)
            {
                if (tn.NavigateUrl != strNodeName)
                {
                    FindInTree(tn, strNodeName);
                }
                else
                {

                    tn.Expanded = true;
                    setParent(tn);
                    return;
                }
            }
        }

        private void FindInTree(TreeNode objTreeNode, string strNodeName)
        {
            foreach (TreeNode tn in objTreeNode.ChildNodes)
            {
                if (tn.NavigateUrl != strNodeName)
                {
                    FindInTree(tn, strNodeName);
                }
                else
                {
                    tn.Expanded = true;
                    setParent(tn);
                    return;
                }
            }
        }

        private void setParent(TreeNode objTreeNode)
        {
            if (objTreeNode.Parent != null)
            {
                objTreeNode.Parent.Expanded = true;
                setParent(objTreeNode.Parent);
            }
        }
    }
}