using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class OnlineService : System.Web.UI.Page
    {
        /// <summary>
        /// 网友涞源，业务线
        /// </summary>
        public string SourceType
        {
            get { return HttpContext.Current.Request.Form["SourceType"]; }
        }

        /// <summary>
        /// 网友涞源，业务线
        /// </summary>
        public string LoginID
        {
            get
            {
                string loginstr = string.Empty;
                //惠买车
                if (SourceType == BLL.Util.GetSourceType("惠买车"))
                {
                    if (Request.Cookies["hmc_loginid"] != null)
                    {
                        loginstr = HttpContext.Current.Server.UrlDecode(Request.Cookies["hmc_loginid"].Value);
                    }
                }
                //易车商城
                if (SourceType == BLL.Util.GetSourceType("易车商城"))
                {
                    if (Request.Cookies["sc_loginid"] != null)
                    {
                        loginstr = HttpContext.Current.Server.UrlDecode(Request.Cookies["sc_loginid"].Value);
                    }
                }
                return loginstr;
            }
        }
        /// <summary>
        /// 最后访问页面url
        /// </summary>
        public string UserReferURL
        {
            get { return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]; }
        }
        /// <summary>
        /// 最后访问页面的title
        /// </summary>
        public string EPTitle
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["PageTitle"]); }
        }
        /// <summary>
        /// PostURL
        /// </summary>
        public string EPPostURL
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["SourceUrl"]); }
        }

        /// <summary>
        /// 业务线展示页面title
        /// </summary>
        public string ShowPageTitle
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["ShowPageTitle"]); }
        }
        /// <summary>
        /// 业务线展示页面URL
        /// </summary>
        public string ShowPageUrl
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["ShowPageUrl"]); }
        }

        public ServeTime ST;
        public ServeTime ET;
        public string logoURL;
        public string AgentHeadURL;
        public string UserHeadURL;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取服务时间
                ST = new ServeTime(9, 0);
                ET = new ServeTime(18, 0);
                BLL.BaseData.Instance.ReadTime(out ST, out ET);
                //惠买车,读取log,头像
                if (SourceType == BLL.Util.GetSourceType("惠买车"))
                {
                    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "hmc_sourcetype");
                }
                //易车商城,读取log,头像
                //if (SourceType == BLL.Util.GetSourceType("易车商城"))
                //{
                //    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "sc_sourcetype");
                //}
                //
            }
        }
    }
}