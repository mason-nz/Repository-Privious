using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Core;
using BitAuto.DSC.IM_DMS2014.BLL;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_DMS2014.Web
{
    public partial class WebTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string para = EPDMSMember.GetTestDMSMemberUrl(7976, "强斐", "13811510109", "yogi_07@163.com", "财务主管");
            Uri url = new Uri("http://www.easypass.cn/WebTest.aspx?dd=ssss&cc=123");
            CometClient client = EPDMSMember.GetDMSMemberByUrl("测试页面", url, url, para);
        }
    }
}