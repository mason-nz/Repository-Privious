using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.ErrorPage
{
    public partial class NotAccessMsgPage : System.Web.UI.Page
    {
        public string ERRORMessage
        {
            get { return BLL.Util.GetCurrentRequestStr("msg").ToString();} 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ERRORMessage))
            {
                Response.Write("您没有访问“客户呼叫中心系统”的权限");
                Response.End();
            }
            else
            {
                Response.Write(ERRORMessage);
                Response.End();
            }
        }
    }
}