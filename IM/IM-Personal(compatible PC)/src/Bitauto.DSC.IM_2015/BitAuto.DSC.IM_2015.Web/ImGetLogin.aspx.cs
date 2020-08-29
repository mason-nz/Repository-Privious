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
                if (SourceType == BLL.Util.GetSourceType("易车商城"))
                {
                    Response.AppendHeader("P3P", "CP=CAO PSA OUR");
                    BLL.Loger.Log4Net.Info("传递商城cookie开始:" + HttpContext.Current.Server.UrlEncode(LoginID));
                    Response.Cookies["sc_loginid"].Value = HttpContext.Current.Server.UrlEncode(LoginID);
                    Response.Cookies["sc_loginid"].Expires = DateTime.Now.AddDays(7);
                    BLL.Loger.Log4Net.Info("传递商城cookie结束:" + HttpContext.Current.Server.UrlEncode(LoginID));
                }
                if (SourceType == BLL.Util.GetSourceType("二手车"))
                {
                    Response.AppendHeader("P3P", "CP=CAO PSA OUR");
                    BLL.Loger.Log4Net.Info("传递二手车cookie开始:" + HttpContext.Current.Server.UrlEncode(LoginID));
                    Response.Cookies["er_loginid"].Value = HttpContext.Current.Server.UrlEncode(LoginID);
                    Response.Cookies["er_loginid"].Expires = DateTime.Now.AddDays(7);
                    BLL.Loger.Log4Net.Info("传递二手车cookie结束:" + HttpContext.Current.Server.UrlEncode(LoginID));
                    string callback = Request["callback"];
                    Response.Write(callback + "({msg:'success'})");

                }
                if (SourceType == BLL.Util.GetSourceType("易车网") || SourceType == BLL.Util.GetSourceType("易车视频") || SourceType == BLL.Util.GetSourceType("易车报价") || SourceType == BLL.Util.GetSourceType("易车贷款") || SourceType == BLL.Util.GetSourceType("易车问答") || SourceType == BLL.Util.GetSourceType("易车口碑") || SourceType == BLL.Util.GetSourceType("易车养护") || SourceType == BLL.Util.GetSourceType("易车论坛") || SourceType == BLL.Util.GetSourceType("易车车友会") || SourceType == BLL.Util.GetSourceType("我的易车") || SourceType == BLL.Util.GetSourceType("易车购车类"))
                {
                    Response.AppendHeader("P3P", "CP=CAO PSA OUR");
                    BLL.Loger.Log4Net.Info("传递易车网cookie开始:" + HttpContext.Current.Server.UrlEncode(LoginID));
                    Response.Cookies["yc_loginid"].Value = HttpContext.Current.Server.UrlEncode(LoginID);
                    Response.Cookies["yc_loginid"].Expires = DateTime.Now.AddDays(7);
                    BLL.Loger.Log4Net.Info("传递易车网cookie结束:" + HttpContext.Current.Server.UrlEncode(LoginID));
                }
            }
        }
    }
}