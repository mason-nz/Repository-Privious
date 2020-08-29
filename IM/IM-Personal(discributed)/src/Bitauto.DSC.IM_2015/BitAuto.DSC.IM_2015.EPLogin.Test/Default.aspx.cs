using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Concurrent;

namespace BitAuto.DSC.IM_2015.EPLogin.Test
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConcurrentQueue<string> IISCometMessages = new ConcurrentQueue<string>();

            string result = null;
            IISCometMessages.TryDequeue(out result);

            Response.Write(result);
        }
    }
}