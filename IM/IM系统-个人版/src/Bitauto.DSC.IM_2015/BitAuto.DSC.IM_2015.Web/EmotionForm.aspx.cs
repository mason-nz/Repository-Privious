using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class EmotionForm : System.Web.UI.Page
    {
        public string ReplyBoxID
        {
            get
            {
                return HttpContext.Current.Request["ReplyBoxID"] == null ? "" :
                HttpContext.Current.Request["ReplyBoxID"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}