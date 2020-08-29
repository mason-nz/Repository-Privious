using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TagEdit : PageBase
    {
        #region 定义属性
        public int BGID
        {
            get { return Request.QueryString["BGID"] == null ? -1 : Convert.ToInt32(Request.QueryString["BGID"]); }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}