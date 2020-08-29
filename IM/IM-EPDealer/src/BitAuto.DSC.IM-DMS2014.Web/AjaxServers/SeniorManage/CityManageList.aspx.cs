using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Entities;
using System.Data;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.SeniorManage
{
    public partial class CityManageList : System.Web.UI.Page
    {
        #region 属性
        /// 大区
        /// <summary>
        /// 大区
        /// </summary>
        public string DistrictID { get { return BLL.Util.GetCurrentRequestQueryStr("DistrictID"); } }
        /// 城市群
        /// <summary>
        /// 城市群
        /// </summary>
        public string CityGroupID { get { return BLL.Util.GetCurrentRequestQueryStr("CityGroupID"); } }
        /// 坐席
        /// <summary>
        /// 坐席
        /// </summary>
        public string UserID { get { return BLL.Util.GetCurrentRequestQueryStr("UserID"); } }
        /// 无坐席
        /// <summary>
        /// 无坐席
        /// </summary>
        public string IsHave { get { return BLL.Util.GetCurrentRequestQueryStr("IsHave"); } }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int RecordCount = 0;
            QueryCityGroupAgent query = new QueryCityGroupAgent();
            query.DistrictID = DistrictID;
            query.CityGroupID = CityGroupID;
            query.UserID = UserID;
            query.IsHave = IsHave;
            DataTable dt = BLL.CityGroupAgent.Instance.GetCityGroupAgent(query, "a.CityGroup", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            rpt_citygroup.DataSource = dt;
            rpt_citygroup.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 5, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 10);
        }
    }
}