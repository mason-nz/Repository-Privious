using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class WorkOrderList : PageBase
    {
        public bool canExport = false;

        //添加工单的参数
        public string Param = "";
        //CC站点目录
        public string ExitAddress { get { return BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress"); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                canExport = BLL.Util.CheckButtonRight("SYS024BUT102102A");
                Param = BLL.WOrderRequest.AddWOrderComeIn_NoPhone(Entities.ModuleSourceEnum.M02_工单).ToString();
                WorkOrderStatusBind();
                WOrderCategoryEnumBind();
            }
        }
        /// <summary>
        /// 工单状态绑定
        /// </summary>
        private void WorkOrderStatusBind()
        {
            sltWorkOrderStatus.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.WorkOrderStatus));
            sltWorkOrderStatus.DataTextField = "name";
            sltWorkOrderStatus.DataValueField = "value";
            sltWorkOrderStatus.DataBind();
            sltWorkOrderStatus.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        /// <summary>
        /// 工单类型绑定
        /// </summary>
        private void WOrderCategoryEnumBind()
        {
            sltWorkCategory.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.WOrderCategoryEnum));
            sltWorkCategory.DataTextField = "name";
            sltWorkCategory.DataValueField = "value";
            sltWorkCategory.DataBind();
            sltWorkCategory.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}