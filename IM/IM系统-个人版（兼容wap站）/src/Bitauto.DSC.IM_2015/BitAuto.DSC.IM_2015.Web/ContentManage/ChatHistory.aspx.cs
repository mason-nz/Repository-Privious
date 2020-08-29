using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.ContentManage
{
    public partial class ChatHistory : System.Web.UI.Page
    {
        public string WorkOrderViewUrl = ConfigurationManager.AppSettings["WorkOrderViewUrl"];
        public string CrmMemberDetail = ConfigurationManager.AppSettings["CrmMemberDetail"];
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //   //#region 绑定大区数据
            //   //selDistrict.DataSource = BLL.BaseData.Instance.GetAllDistrict();
            //   //selDistrict.DataTextField = "DistrictName";
            //   //selDistrict.DataValueField = "District";
            //   //selDistrict.DataBind();
            //   //selDistrict.Items.Insert(0, new ListItem("请选择", "-1"));
            //   //#endregion

            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            //}
            if (!IsPostBack)
            {
                 BindGroupData();
            }
        }
        private void BindGroupData()
        {
            //int userID = BLL.Util.GetLoginUserID();
            //管辖分组+所属分组 默认第一个：所属分组
            //DataTable dt = BLL.UserSendMessage.Instance.GetCurrentUserGroups(userID);
            DataTable dt = BLL.UserSendMessage.Instance.GetWOrderBusiType();
            sltBG.DataSource = dt;
            sltBG.DataValueField = "RecID";
            sltBG.DataTextField = "BusiTypeName";
            sltBG.DataBind();
            sltBG.Items.Insert(0, new ListItem("请选择", "-2"));
            
        }
    }
}