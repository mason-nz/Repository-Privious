using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using System.Threading;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class DealerInfo : PageBase
    {
        /// 电话号码
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("tel");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
             
        }
    }
}