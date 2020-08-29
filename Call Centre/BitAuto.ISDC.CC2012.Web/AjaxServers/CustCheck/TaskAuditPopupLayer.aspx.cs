using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck
{
    public partial class TaskAuditPopupLayer : PageBase
    {
        #region 属性定义
        public string PTIDS
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("PTIDS");
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }
    }
}