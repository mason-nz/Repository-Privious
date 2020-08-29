using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web
{
    public partial class lxwTest : System.Web.UI.Page
    {
        private string RecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                Entities.CustBasicInfo ci = new Entities.CustBasicInfo();
                ci.CustName = "test" + i + "_" + RecID;
                ci.CreateTime = DateTime.Now;
                BLL.CustBasicInfo.Instance.Insert(ci);
            }

        }
    }
}