using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BitAuto.DSC.IM_DMS2014.Web.ContentManage
{
    public partial class ChatHistory : System.Web.UI.Page
    {
        public string WorkOrderViewUrl = ConfigurationManager.AppSettings["WorkOrderViewUrl"];
        public string CrmMemberDetail = ConfigurationManager.AppSettings["CrmMemberDetail"];
        public string WorkOrderViewUrlNew = ConfigurationManager.AppSettings["WorkOrderViewUrlNew"];

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            //if (!IsPostBack)
            //{
            //   //#region 绑定大区数据
            //   //selDistrict.DataSource = BLL.BaseData.Instance.GetAllDistrict();
            //   //selDistrict.DataTextField = "DistrictName";
            //   //selDistrict.DataValueField = "District";
            //   //selDistrict.DataBind();
            //   //selDistrict.Items.Insert(0, new ListItem("请选择", "-1"));
            //   //#endregion


            //}
        }
    }
}