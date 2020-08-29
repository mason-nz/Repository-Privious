using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Entities;

namespace BitAuto.DSC.IM_DMS2014.Web
{
    public partial class OnlineService : System.Web.UI.Page
    {
        /// <summary>
        /// 易湃传递的参数Key
        /// 
        /// </summary>
        public string EPKey
        {
            get { return HttpContext.Current.Request.Form["LoginKey"]; }
        }
        /// <summary>
        /// 易湃最后访问页面url
        /// </summary>
        public string UserReferURL
        {
            get { return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]; }
        }
        /// <summary>
        /// 易湃最后访问页面的title
        /// </summary>
        public string EPTitle
        {
            get { return HttpContext.Current.Request.Form["PageTitle"]; }
        }
        /// <summary>
        /// 易湃PostURL
        /// </summary>
        public string EPPostURL
        {
            get { return HttpContext.Current.Request.Form["SourceUrl"]; }
        }
        public ServeTime ST;
        public ServeTime ET;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取服务时间
                ST = new ServeTime(9, 0);
                ET = new ServeTime(18, 0);
                BLL.BaseData.Instance.ReadTime(out ST, out ET);
            }
        }
    }
}