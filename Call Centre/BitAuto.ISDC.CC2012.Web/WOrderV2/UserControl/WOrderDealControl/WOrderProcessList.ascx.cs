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
    public partial class WOrderProcessList : System.Web.UI.UserControl
    {
        /// tableid
        /// <summary>
        /// tableid
        /// </summary>
        public string TableHtmlId { get; set; }
        /// 工单ID
        /// <summary>
        /// 工单ID
        /// </summary>
        public string OrderId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
       
    }
}