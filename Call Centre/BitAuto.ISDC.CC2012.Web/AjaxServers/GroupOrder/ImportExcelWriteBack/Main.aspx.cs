using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.GroupOrder.ImportExcelWriteBack
{
    public partial class Main : PageBase
    {

        public string userId
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["CurrentUserId"]) ? string.Empty : this.Request["CurrentUserId"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("[Main-GroupOrderListImport]Page_Load enter...");
        }
    }
}