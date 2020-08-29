using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CallReport
{
    public partial class SatisfactionReport : PageBase
    {
        private int userID;
        public int RegionID = 2;
        /// <summary>
        /// 导出按钮权限
        /// </summary>
        public bool DataExportButton = false;

        TelNumManage Manage
        {
            get
            {
                return BitAuto.ISDC.CC2012.BLL.CallDisplay.Manage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                DataExportButton = BLL.Util.CheckButtonRight("SYS024BUT4003");
            }
        }
    }
}