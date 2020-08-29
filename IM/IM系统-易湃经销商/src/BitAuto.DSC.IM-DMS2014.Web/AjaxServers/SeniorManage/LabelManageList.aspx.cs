using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.SeniorManage
{
    public partial class LabelManageList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            DataTable dt = null;
            string where = string.Empty;
            int CurrentUserID = 0;
            CurrentUserID = BLL.Util.GetLoginUserID();
            string region = "";
            region = BLL.BaseData.Instance.GetAgentRegionByUserID(CurrentUserID.ToString());
            dt = BLL.GroupLabel.Instance.GetLabelConfig("AND bg.RegionID=" + region);

            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
        }
    }
}