using GeetestSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.Utils.Config;

namespace XYAuto.BUOC.Chitunion2017.NewWeb.ajaxservers.geetest
{
    /// <summary>
    /// getcaptcha 的摘要说明
    /// </summary>
    public class getcaptcha : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        private string GeetestConfig_ID = ConfigurationUtil.GetAppSettingValue("GeetestConfig_ID");
        private string GeetestConfig_Key = ConfigurationUtil.GetAppSettingValue("GeetestConfig_Key");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            currentContext = context;
            context.Response.Write(getCaptcha());
            context.Response.End();
        }
        private String getCaptcha()
        {
            GeetestLib geetest = new GeetestLib(GeetestConfig_ID, GeetestConfig_Key);
            //String userID = "test";
            Byte gtServerStatus = geetest.preProcess("web", ITSC.Chitunion2017.BLL.Util.GetIPAddress());
            currentContext.Session[GeetestLib.gtServerStatusSessionKey] = gtServerStatus;
            //context.Session["userID"] = userID;
            return geetest.getResponseStr();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}