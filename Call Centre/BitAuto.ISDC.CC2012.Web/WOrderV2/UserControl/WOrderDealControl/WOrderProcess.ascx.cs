using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderDealControl
{
    public partial class WOrderProcess : System.Web.UI.UserControl
    {
        /// 工单ID
        /// <summary>
        /// 工单ID
        /// </summary>
        public string OrderId { get; set; }
        /// 权限控制
        /// <summary>
        /// 权限控制
        /// </summary>
        public string Right
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Right");
            }
        }
        /// 工单状态
        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderStatus WorkOrderStatus { get; set; }

        public WOrderProcess()
        {
        }

        public WOrderProcess(string orderid, WorkOrderStatus status)
        {
            OrderId = orderid;
            WorkOrderStatus = status;
        }

        /// tableid
        /// <summary>
        /// tableid
        /// </summary>
        public string TableHtmlId = "wOrderImgTab";
        /// 状态可选项
        /// <summary>
        /// 状态可选项
        /// </summary>
        public Dictionary<int, string> DictStauts = new Dictionary<int, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 设置状态下拉选择项
                SetStatusOption();
                //设置处理记录控件属性
                this.ucWOrderProcessList.OrderId = OrderId;
                this.ucWOrderProcessList.TableHtmlId = TableHtmlId;
                if (WorkOrderStatus==WorkOrderStatus.Completed||WorkOrderStatus==WorkOrderStatus.Pending)
                {
                    this.ucWOrderProcessList.Visible = false;
                }
            }
        }

        /// 设置状态下拉选择项
        /// <summary>
        /// 设置状态下拉选择项
        /// </summary>
        private void SetStatusOption()
        {
            if (WorkOrderStatus == WorkOrderStatus.Pending)//工单状态为“待审核”时选项：待处理、已处理、已关闭，默认为“待处理”；
            {
                DictStauts.Add((int)WorkOrderStatus.Untreated, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Untreated));
                DictStauts.Add((int)WorkOrderStatus.Processed, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Processed));
                DictStauts.Add((int)WorkOrderStatus.Closed, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Closed));
            }

            else if (WorkOrderStatus == WorkOrderStatus.Untreated)//工单状态为“待处理”时选项：待处理、处理中、已处理，默认为“处理中”；
            {
                DictStauts.Add((int)WorkOrderStatus.Processing, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Processing));
                DictStauts.Add((int)WorkOrderStatus.Untreated, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Untreated));
                DictStauts.Add((int)WorkOrderStatus.Processed, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Processed));
            }

            else if (WorkOrderStatus == WorkOrderStatus.Processing)//工单状态为“处理中”时选项：处理中、已处理，默认为“已处理”；选择“已处理”时，隐藏接收人和抄送人
            {
                DictStauts.Add((int)WorkOrderStatus.Processed, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Processed));
                DictStauts.Add((int)WorkOrderStatus.Processing, BLL.Util.GetEnumOptText(typeof(WorkOrderStatus), (int)WorkOrderStatus.Processing));
            }
        }
    }
}