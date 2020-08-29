using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.EmployeeAgentExclusive
{
    public partial class EmployeeAgentExclusiveList : System.Web.UI.Page
    {
        public bool right_set = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = BLL.Util.GetLoginUserID();
            right_set = BLL.Util.CheckRight(userId, "SYS024BUT501001");
        }
    }
}