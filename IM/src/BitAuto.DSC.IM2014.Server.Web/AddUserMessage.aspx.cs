using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class AddUserMessage : System.Web.UI.Page
    {
        public string UserMessageIMID
        {
            get
            {


                if (!string.IsNullOrEmpty(Request["UserMessageIMID"]))
                {
                    return Request["UserMessageIMID"];
                }
                else
                {
                    return "";
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}