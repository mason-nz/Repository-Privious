using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// SCLogin 的摘要说明
    /// </summary>
    public class SCLogin : IHttpHandler, IRequiresSessionState
    {
        private string RequestAction
        {
            get { return BLL.Util.GetCurrentRequestFormStr("action"); }
        }
        private string YICHEMALLLoginURL = System.Configuration.ConfigurationManager.AppSettings["YICHEMALLLoginURL"];


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (RequestAction.ToLower() == "verifylogin")
            {
                string domainName = BLL.Util.GetLoginUserName();
                string content = WebService.YiCheMall.CCLoginHelper.Instance.CCLogin(domainName);
                BLL.Loger.Log4Net.Info("调用易车商城接口" + YICHEMALLLoginURL + ",当前域账号：" + domainName + ",返回信息：" + content);
                context.Response.Write(content);
            }
            
            context.Response.End();
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