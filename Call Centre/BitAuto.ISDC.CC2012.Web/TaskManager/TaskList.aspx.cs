using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TaskManager
{
    public partial class TaskList1 : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        /// 导出按钮权限
        /// </summary>
        public bool DataExportButton = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataExportButton = BLL.Util.CheckButtonRight("SYS024BUT1103");
            }
        }
      
    }
}