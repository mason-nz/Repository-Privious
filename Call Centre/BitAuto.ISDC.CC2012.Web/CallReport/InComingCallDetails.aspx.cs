using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage;

namespace BitAuto.ISDC.CC2012.Web.CallReport
{
    public partial class InComingCallDetails : PageBase
    {
        //public string UserArea = "0";
        public bool IsExport = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsExport = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT4061");
                //Entities.EmployeeAgent employeeagent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(BLL.Util.GetLoginUserID());
                //if (employeeagent != null)
                //{
                //    //北京
                //    if (employeeagent.RegionID == 1)
                //    {
                //        UserArea = "1";
                //    }
                //    else if (employeeagent.RegionID == 2)
                //    {
                //        UserArea = "2";
                //    }
                //}
            }
        }
    }

}