using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class UserChatTest : System.Web.UI.Page
    {
        public string username = string.Empty;
        public string UserReferURL = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取页面涞源
                UserReferURL = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
                //取网友标识
                username = GetUserIMID();


            }
        }

        /// <summary>
        /// 取网友标识，如果Cookie存在，在Cookie中取，不存在创建
        /// </summary>
        /// <returns></returns>
        private string GetUserIMID()
        {
            //HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies["UserIMID"];
            //if (Cookie != null)
            //{
            //    return Cookie.Value.ToString();
            //}
            //else
            //{
            //HttpCookie Cookienew = new HttpCookie("UserIMID");
            //Cookie.Domain = ".xxx.com";//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
            //Cookie.Expires = DateTime.Now.AddDays(strDay);
            string Guidstr = System.Guid.NewGuid().ToString();
            //Cookienew.Value = Guidstr;
            //System.Web.HttpContext.Current.Response.Cookies.Add(Cookienew);
            return Guidstr;
            //}
        }
    }
}