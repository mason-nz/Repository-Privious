﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class mSatisfactionForm : System.Web.UI.Page
    {
        public string CSID
        {
            get
            {
            
                return HttpContext.Current.Request["CSID"] == null ? "" :
                HttpContext.Current.Request["CSID"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
 
        }
    }
}