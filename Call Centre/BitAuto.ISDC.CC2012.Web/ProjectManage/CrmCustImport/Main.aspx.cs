using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage.CrmCustImport
{
    public partial class Main : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserID = BLL.Util.GetLoginUserID().ToString();
        }
    }
}