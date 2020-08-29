using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.ITSC.Chitunion2017.Web
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int d = 123;
            var authorizationCode = Request.HttpMethod.ToLower() == "get" ?
             Request.QueryString["authorization_code"]
             : Request.Form["authorization_code"];
            var access_token = Request.HttpMethod.ToLower() == "get" ?
             Request.QueryString["access_token"]
             : Request.Form["access_token"];
            var refresh_token = Request.HttpMethod.ToLower() == "get" ?
             Request.QueryString["refresh_token"]
             : Request.Form["refresh_token"];
            BLL.Loger.Log4Net.Info("Request.HttpMethod:" + Request.HttpMethod);
            BLL.Loger.Log4Net.Info("authorizationCode:" + authorizationCode);
            BLL.Loger.Log4Net.Info("refresh_token:" + refresh_token);
            BLL.Loger.Log4Net.Info("access_token:" + access_token);
            if (!IsPostBack)
            {
                //XYAuto.ITSC.Chitunion2017.WebService.GDT.ServiceHelper serviceHelper = new WebService.GDT.ServiceHelper();

                //var result = serviceHelper.GetToAuthorizeUrl(HttpUtility.UrlEncode("http://www.chitunion.com/AjaxServers/LoginManager.ashx"), "xy");

                Response.Write("code1:" + Request["authorization_code"]);
            }
            else
            {
                Response.Write("code2:" + Request["authorization_code"]);
            }
        }
    }
}