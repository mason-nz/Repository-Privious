using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CustBaseInfo
{
    public partial class GoToTaoCheBZC : PageBase
    {
        /// <summary>
        /// 呼入弹屏时，CTI送过来的电话
        /// </summary>
        public string RequestTel
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Tel");
            }
        }

        /// <summary>
        /// 跳转二手车业务URL时，带的参数menu
        /// menu=1，跳转到易车二手车管理页面
        /// menu=2，跳转到淘车通管理页面
        /// </summary>
        public string RequestMenu
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("menu");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RedirectTaoCheBZCUrl(RequestTel);
            }
        }

        private void RedirectTaoCheBZCUrl(string tel)
        {
            string domainName = BLL.Util.GetLoginUserName();
            long timeticks = DateTime.Now.Ticks;
            string key = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(domainName + "taoche!@#callcenter" + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"), "MD5");
            string url = System.Configuration.ConfigurationManager.AppSettings["TaoCheAXBZCUrl"];
            Response.Redirect(string.Format(url + "?username={0}&time={1}&callcenterkey={2}&menu={3}", domainName, timeticks, key, RequestMenu));
        }
    }
}