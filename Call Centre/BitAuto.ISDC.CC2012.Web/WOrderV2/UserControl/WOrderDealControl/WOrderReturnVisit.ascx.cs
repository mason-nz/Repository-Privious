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
    public partial class WOrderReturnVisit : System.Web.UI.UserControl
    {
        /// 工单ID
        /// <summary>
        /// 工单ID
        /// </summary>
        public string OrderId { get; set; }

        public WOrderReturnVisit()
        {
        }
        public WOrderReturnVisit(string orderid)
        {
            OrderId = orderid;
        }

        /// tableid
        /// <summary>
        /// tableid
        /// </summary>
        public string TableHtmlId = "wOrderReturnTab";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //设置处理记录控件属性
                this.ucWOrderProcessList.OrderId = OrderId;
                this.ucWOrderProcessList.TableHtmlId = TableHtmlId;
            }
        }
    }
}