using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class SendMessageLayer : PageBase
    {
        public string JsonStr { get { return BLL.Util.GetCurrentRequestStr("JsonData"); } }

        public CommonCallJsonData JsonData = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JsonData = CommonCallJsonData.GetCommonCallJsonData(JsonStr);
            }
        }
    }
}