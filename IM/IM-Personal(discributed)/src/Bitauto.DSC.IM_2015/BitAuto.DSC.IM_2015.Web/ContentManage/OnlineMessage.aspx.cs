using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.Web.ContentManage
{
    public partial class OnlineMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSourceType();
            }
        }
        /// <summary>
        /// 绑定业务来源
        /// </summary>
        private void BindSourceType()
        {
            ddlSourceType.DataSource = BLL.Util.GetAllSourceType(true);
            ddlSourceType.DataValueField = "SourceTypeValue";
            ddlSourceType.DataTextField = "SourceTypeName";
            ddlSourceType.DataBind();
        }
    }
}