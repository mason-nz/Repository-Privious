using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class AgentTimeState : PageBase
    {
        public int userid = -2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                userid = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        private void BindData()
        {

        }
    }
}