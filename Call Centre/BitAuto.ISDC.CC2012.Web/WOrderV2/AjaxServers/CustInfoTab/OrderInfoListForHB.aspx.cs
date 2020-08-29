using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab
{
    public partial class OrderInfoListForHB : PageBase
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
        /// 页面ID
        /// <summary>
        /// 页面ID
        /// </summary>
        public string keyID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("keyid");
            }
        }
        /// 不显示查询条件
        /// <summary>
        /// 不显示查询条件
        /// </summary>
        public string Notext
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Notext");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}