using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class WorkOrderList : PageBase
    {
        public bool canAdd = false;
        public bool canExport = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                canAdd = BLL.Util.CheckButtonRight("SYS024BUG100604");
                canExport = BLL.Util.CheckButtonRight("SYS024BUG100603");

                WorkOrderStatusBind();
                WorkOrderCategoryParentBind();
                PriorityLevelBind();
                rptAreaBind();

                BindWTagBGroup();
            }
        }

        /// <summary>
        /// 绑定工单标签分组
        /// </summary>
        private void BindWTagBGroup()
        { 
            int userID = BLL.Util.GetLoginUserID();
            //管辖分组+所属分组 默认第一个：所属分组
            DataTable dt = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userID);
            sltBG.DataSource = dt;
            sltBG.DataValueField = "BGID";
            sltBG.DataTextField = "Name";
            sltBG.DataBind();
            sltBG.Items.Insert(0, new ListItem("请选择","-2"));
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
            sltWorkOrderStatus.Items.Insert(0,new ListItem("请选择","-1"));
        }

        /// <summary>
        /// 工单分类绑定
        /// </summary>
        private void WorkOrderCategoryParentBind()
        {
            Entities.QueryWorkOrderCategory query = new Entities.QueryWorkOrderCategory();
            query.PID = 0;
            query.Status = 0;
            query.UseScopeStr = "1,2";
            int totalCount=0;

            DataTable dt = BLL.WorkOrderCategory.Instance.GetWorkOrderCategory(query,"",1,10000,out totalCount);
            sltParentCategory.DataSource=dt;
            sltParentCategory.DataTextField = "Name";
            sltParentCategory.DataValueField = "RecID";
            sltParentCategory.DataBind();
            sltParentCategory.Items.Insert(0,new ListItem("请选择","-1"));
        }

        /// <summary>
        /// 优先级绑定
        /// </summary>
        private void PriorityLevelBind()
        {
            sltPriorityLevel.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.PriorityLevelEnum));
            sltPriorityLevel.DataTextField = "name";
            sltPriorityLevel.DataValueField = "value";
            sltPriorityLevel.DataBind();
            sltPriorityLevel.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        private void rptAreaBind()
        {
            sltArea.DataSource = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAllDistrict();
            sltArea.DataTextField = "DepartName";
            sltArea.DataValueField = "DepartID";
            sltArea.DataBind();
            sltArea.Items.Insert(0, new ListItem("全部", "-1"));            
        }
    }
}