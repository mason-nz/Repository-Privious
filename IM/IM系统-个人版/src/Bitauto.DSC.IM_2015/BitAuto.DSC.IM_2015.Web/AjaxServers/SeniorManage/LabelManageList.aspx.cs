using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
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
            //获取管辖分组BGIDs
            DataTable dtbgid = BLL.BaseData.Instance.GetUserGroupDataRigth(CurrentUserID);
            string bgids = Constant.STRING_INVALID_VALUE;
            if (dtbgid != null && dtbgid.Rows.Count > 0)
            {
                foreach (DataRow row in dtbgid.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(bgids))
            {
                bgids = bgids.Substring(1); 
                dt = BLL.GroupLabel.Instance.GetLabelConfig(" AND bg.RegionID='" + region + "' AND bg.BGID IN (" + bgids + ")");
            }
            else
            {
                //dt = BLL.GroupLabel.Instance.GetLabelConfig("AND bg.RegionID=" + region);
                //dt = null;
                Response.Write("对不起，没有找到您管辖的分组数据！");
                Response.End();
            }

            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
        }
    }
}