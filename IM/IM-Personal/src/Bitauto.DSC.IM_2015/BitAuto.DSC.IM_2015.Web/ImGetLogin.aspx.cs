using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class ImGetLogin : System.Web.UI.Page
    {
        /// <summary>
        /// 传递的参数LoginID
        /// </summary>
        public string LoginID
        {
            get
            {
                return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["LoginID"]);
            }
        }
        /// <summary>
        /// 传递的参数SourceType
        /// </summary>
        public string SourceType
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["SourceType"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //惠买车
                if (SourceType == BLL.Util.GetSourceType("惠买车"))
                {
                    Response.AppendHeader("P3P", "CP=CAO PSA OUR");
                    Response.Cookies["hmc_loginid"].Value = HttpContext.Current.Server.UrlEncode(LoginID);
                    Response.Cookies["hmc_loginid"].Expires = DateTime.Now.AddDays(7);
                    //Response.Cookies["hmc_loginid"].Expires = DateTime.Now.AddHours(8);
                }
                //if (SourceType == BLL.Util.GetSourceType("易车商城"))
                //{
                //    Response.AppendHeader("P3P", "CP=CAO PSA OUR");
                //    Response.Cookies["sc_loginid"].Value = HttpContext.Current.Server.UrlEncode(LoginID);
                //    Response.Cookies["sc_loginid"].Expires = DateTime.Now.AddDays(7);
                //}
            }
        }
    }
}