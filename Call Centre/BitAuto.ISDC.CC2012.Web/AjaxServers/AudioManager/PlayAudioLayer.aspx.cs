using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.AudioManager
{
    public partial class PlayAudioLayer : PageBase
    {

        #region 属性定义
        public string RequestAudioURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("AudioURL"); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}