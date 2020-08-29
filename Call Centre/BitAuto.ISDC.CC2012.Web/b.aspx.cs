using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web
{
    public partial class b : System.Web.UI.Page
    {
        public string ID
        {
            get
            {
                return Request["id"] == null ? string.Empty : Request["id"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ID != string.Empty)
                {
                    Session["id"] = ID;
                }
                if (ID == string.Empty && Session["id"] != null)
                {
                    txtID.Value = Session["id"].ToString();
                    Session["id"] = null;
                }
            }
        }
    }
}