using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace BitAuto.DSC.IM2014.Server.Web.Controls
{
    public partial class Top : System.Web.UI.UserControl
    {
        /// <summary>
        /// 坐席编号
        /// </summary>
        public string AgentIMID
        {
            get
            {
                string agentid = "";
                if (Session["agent_IMID"] != null)
                {
                    agentid = Session["agent_IMID"].ToString();
                }
                return agentid;
            }
        }
        /// <summary>
        /// 坐席姓名
        /// </summary>
        public string AgentUserName
        {
            get
            {
                string agent_UserName = "";
                if (Session["agent_UserName"] != null)
                {
                    agent_UserName = Session["agent_UserName"].ToString();
                }
                return agent_UserName;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //如果没有登录，则跳到登录页面
            if (Session["userid"] == null)
            {
                string usrPath = Page.Request.Path.ToString().ToLower();
                Response.Redirect("login.aspx?gourl=" + usrPath);
            }
            if (!IsPostBack)
            {
                //是否有当前请求页面权限
                bool result = false;
                string usrPath = Page.Request.Path.ToString().ToLower();
                usrPath = usrPath.Substring(usrPath.LastIndexOf("/") + 1);
                //取当前登录人的一级菜单模块，包括样式，图片，连接地址，模块编号
                this.Lit_menus.Text = PageUtil.GetBigModules("<li><a {$lab_class$} onclick=\"openwindow('{$lab_url$}','{$lab_moduleid$}')\"><img src='{$imgurl$}' border='0'/><br />{$lab_name$}</a></li>", "cur", usrPath, Convert.ToInt32(Session["userid"]), ref result);
                //如果没有当前请求页面权限，则登录验证通过后，跳到其有权限的第一个页面
                if (result == false)
                {
                    //根据userid，和系统编号，通过集中权限系统提供的方法取有权限的一级菜单
                    DataTable dtParent = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(Session["userid"]), ConfigurationManager.AppSettings["ThisSysID"].ToString());
                    //如果没有权限则跳到错误信息提示页面
                    if (dtParent != null)
                    {
                        if (dtParent.Rows.Count > 0)
                        {
                            //根据一级菜单编号和userid，取该一级菜单下第一个子菜单
                            this.Response.Redirect(PageUtil.GetSubTopOneUrl(dtParent.Rows[0]["moduleID"].ToString(), Convert.ToInt32(Session["userid"])));
                        }
                        else
                        {
                            Response.Redirect("ErrorPage/NotAccessMsgPage.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("ErrorPage/NotAccessMsgPage.aspx");
                    }
                }
            }
        }
    }
}