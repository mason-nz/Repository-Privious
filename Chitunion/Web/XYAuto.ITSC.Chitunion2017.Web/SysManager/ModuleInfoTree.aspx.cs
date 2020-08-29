using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Web.Base;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.Web.SysManager
{
    public partial class ModuleInfoTree : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBinds();
            }
            if (Request.QueryString["SysID"] != null)
            {
                ScriptHelper.ShowCustomScript(Page, "openAjaxPopupCommen('" + Request.QueryString["SysID"] + "','" + Request.QueryString["ModuleID"] + "');");//  openAjaxPopupCommen(DepartID,ShowUserAll)
            }
        }


        #region funtion
        public void DataBinds()
        {
            tv.Nodes.Clear();

            DataTable dt = BLL.SysRight.SysInfo.Instance.GetSysInfoAll();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode node = new TreeNode();
                    node.NavigateUrl = "javascript:openAjaxPopupCommen('" + dt.Rows[i]["SysID"] + "','')";
                    node.Text = dt.Rows[i]["SysName"].ToString();
                    tv.Nodes.Add(node);
                    node.Expanded = true;
                    DataTable dtP = getSubData(dt.Rows[i]["SysID"].ToString(), "");
                    if (dtP.Rows.Count > 0)
                    {
                        treeData(dt.Rows[i]["SysID"].ToString(), "", node);
                    }
                }
            }
            tv.DataBind();
            Expandnodes(tv, false);
            FindInTree("javascript:openAjaxPopupCommen('" + Request.QueryString["SysID"] + "','" + Request.QueryString["ModuleID"] + "')");
        }
        private static void treeData(string sysID, string Pid, TreeNode treenode)
        {

            DataTable dt = getSubData(sysID, Pid);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode node = new TreeNode();
                    node.NavigateUrl = "javascript:openAjaxPopupCommen('" + dt.Rows[i]["SysID"] + "','" + dt.Rows[i]["ModuleID"] + "')";
                    node.Text = dt.Rows[i]["ModuleName"].ToString();
                    treenode.ChildNodes.Add(node);
                    node.Expanded = true;
                    DataTable dtP = getSubData(dt.Rows[i]["SysID"].ToString(), dt.Rows[i]["ModuleID"].ToString());
                    if (dtP.Rows.Count > 0)
                    {
                        treeData(dt.Rows[i]["SysID"].ToString(), dt.Rows[i]["ModuleID"].ToString(), node);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="Pid"></param>
        /// <returns></returns>
        private static DataTable getSubData(string sysID, string Pid)
        {
            int o;
            QueryModuleInfo queryModuleInfo = new QueryModuleInfo();
            queryModuleInfo.SysID = sysID;
            queryModuleInfo.PID = Pid;
            return BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(queryModuleInfo, "OrderNum", 1, 100000, out o);
        }



        /// <summary>
        /// 
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
        #endregion
    }
}