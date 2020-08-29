using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.APPReport2016.WebAPI.Html
{
    public partial class PopTitle : System.Web.UI.Page
    {
        public string Content
        {
            get { return BLL.Util.GetCurrentRequestStr("Content"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{   
                
            //}
        }
    }
}