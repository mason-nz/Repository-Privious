using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class FAQList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var a = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(BLL.Util.GetLoginUserID());
            if (a.RegionID.HasValue)
            {
                RegionID = a.RegionID.Value;
            }
        }

        public int RegionID = -2;
    }
}