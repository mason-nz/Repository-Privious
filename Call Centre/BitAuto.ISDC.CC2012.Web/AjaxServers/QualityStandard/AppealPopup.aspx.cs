using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard
{
    public partial class AppealPopup : PageBase
    {
        public string RequestQS_RID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QS_RID")); }
        }
        public string RequestType
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Type")); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT600106") && !BLL.Util.CheckRight(userId, "SYS024BUT600406"))  
            { 
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}