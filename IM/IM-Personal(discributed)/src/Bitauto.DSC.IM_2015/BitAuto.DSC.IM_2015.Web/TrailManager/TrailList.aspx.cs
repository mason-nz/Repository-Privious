using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace BitAuto.DSC.IM_2015.Web.TrailManager
{
    public partial class TrailList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!Page.IsPostBack)
            {
                BindBusinessGroup();
            }
        }

        private void BindBusinessGroup()
        {
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(BLL.Util.GetLoginUserID());
            ddGroup.DataSource = dt;
            ddGroup.DataValueField = "BGID";
            ddGroup.DataTextField = "Name";
            ddGroup.DataBind();

            ddGroup.Items.Insert(0, new ListItem("请选择","-1"));
        }
    }
}