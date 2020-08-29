using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils.Config;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class EmployeeAgentMutilOption : PageBase
    {
        /// 当前编辑用户
        /// <summary>
        /// 当前编辑用户
        /// </summary>
        public string UserIDs
        {
            get
            {
                return Request.QueryString["UserIDs"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["UserIDs"].ToString());
            }
        }
        /// 当前编辑用户姓名
        /// <summary>
        /// 当前编辑用户姓名
        /// </summary>
        public string UserNames
        {
            get
            {
                return Request.QueryString["UserNames"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["UserNames"].ToString());
            }
        }
        /// 区域
        /// <summary>
        /// 区域
        /// </summary>
        public string AreaID
        {
            get
            {
                return Request.QueryString["AreaID"] == null ? String.Empty : Request.QueryString["AreaID"].ToString();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT5101"))
                {
                    BindRoles();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限")); 
                    Response.End();
                }
            }
        }
        
        /// <summary>
        /// 绑定角色（CC和IM共有的角色）
        /// </summary>
        private void BindRoles()
        {
            //绑定
            Rpt_Role.DataSource = BLL.EmployeeAgent.Instance.GetCCAndIMRoles();
            Rpt_Role.DataBind();
        }
    }
}