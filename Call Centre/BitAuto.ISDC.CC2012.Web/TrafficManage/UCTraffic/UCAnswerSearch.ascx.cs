using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage.UCTraffic
{
    public partial class UCAnswerSearch : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            }
        }
    }
}