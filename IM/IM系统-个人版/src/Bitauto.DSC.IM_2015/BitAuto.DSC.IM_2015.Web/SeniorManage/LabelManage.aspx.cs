using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class LabelManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindData();
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            }
        }

        
    }
}