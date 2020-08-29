using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.Common
{
    public partial class MutilAgentChoose : System.Web.UI.Page
    {
        #region 属性
        /// 选择方式
        /// <summary>
        /// 选择方式
        /// </summary>
        public string Select { get { return BLL.Util.GetCurrentRequestQueryStr("select"); } }
        /// 城市群列表
        /// <summary>
        /// 城市群列表
        /// </summary>
        public string CityGroups { get { return BLL.Util.GetCurrentRequestQueryStr("citygroups"); } }
        /// 已选人员
        /// <summary>
        /// 已选人员
        /// </summary>
        public string ChooseUsers { get { return BLL.Util.GetCurrentRequestQueryStr("chooseusers"); } }
        /// 标题
        /// <summary>
        /// 标题
        /// </summary>
        public string DefineTitle { get { return Server.UrlDecode(BLL.Util.GetCurrentRequestQueryStr("definetitle")); } }

        /// 分组
        /// <summary>
        /// 分组
        /// </summary>
        public string BGID { get { return BLL.Util.GetCurrentRequestQueryStr("BGID"); } }
        /// 用户名称
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get { return BLL.Util.GetCurrentRequestQueryStr("UserName"); } }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserGroup();
                BindAgentData();
            }
        }

        /// 绑定管辖分组
        /// <summary>
        /// 绑定管辖分组
        /// </summary>
        private void BindUserGroup()
        {
            int userid = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(userid);
            rpt_group.DataSource = dt;
            rpt_group.DataBind();
        }
        /// 绑定坐席数据
        /// <summary>
        /// 绑定坐席数据
        /// </summary>
        private void BindAgentData()
        {
            int RecordCount = 0;
            QueryAgentInfo query = new QueryAgentInfo();
            query.BGIDs = BGID;
            query.Name = UserName;
            query.CityGroups = CityGroups;
            DataTable dt = BLL.BaseData.Instance.GetAgentInfoData(query, "UserID", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            rpt_agent.DataSource = dt;
            rpt_agent.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 5, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 11);
        }
    }
}