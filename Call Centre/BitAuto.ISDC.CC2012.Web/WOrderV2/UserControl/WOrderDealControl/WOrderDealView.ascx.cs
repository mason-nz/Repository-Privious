using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderDealControl
{
    public partial class WOrderDealView : System.Web.UI.UserControl
    {
        /// 工单ID
        /// <summary>
        /// 工单ID
        /// </summary>
        public string OrderId { get; set; }
        /// tableid
        /// <summary>
        /// tableid
        /// </summary>
        public string TableHtmlId = "dealViewTab";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置处理记录控件属性
                //TODO:CodeReview 2016-08-10 控件名称规范（后面为什么添加数字1）
                this.WOrderProcessList1.OrderId = OrderId;
                this.WOrderProcessList1.TableHtmlId = TableHtmlId;
            }
        }
    }
}