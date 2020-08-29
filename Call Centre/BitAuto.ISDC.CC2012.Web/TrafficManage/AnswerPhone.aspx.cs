using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class AnswerPhone : PageBase
	{
        public bool ExportButton = false;//导出

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                ExportButton = BLL.Util.CheckButtonRight("SYS024BUT4011");
            }
		}
	}
}