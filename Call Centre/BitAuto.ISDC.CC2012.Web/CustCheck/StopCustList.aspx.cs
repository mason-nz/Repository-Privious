using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustCheck
{
    public partial class StopCustList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    { 
        /// <summary>
        /// masj add 分配任务
        /// </summary>
        public bool AssignTask = false;

        /// <summary>
        /// masj add 收回任务
        /// </summary>
        public bool RevokeTask = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AssignTask = BLL.Util.CheckButtonRight("SYS024BUT130401");
                RevokeTask = BLL.Util.CheckButtonRight("SYS024BUT130402");
                ddlAreaBind();
            }
        }

        private void ddlAreaBind()
        {
            ddlArea.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.EnumArea));
            ddlArea.DataTextField = "name";
            ddlArea.DataValueField = "value";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}