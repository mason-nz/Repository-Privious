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
    /// verifyrequest 的摘要说明
    /// </summary>
    public class verifyrequest : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        private string GeetestConfig_ID = ConfigurationUtil.GetAppSettingValue("GeetestConfig_ID");
        private string GeetestConfig_Key = ConfigurationUtil.GetAppSettingValue("GeetestConfig_Key");


        #region 属性
        private string RequestAction
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr("action"); }
        }

        private string RequestGeetestChallenge
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr(GeetestLib.fnGeetestChallenge); }
        }
        private string RequestGeetestValidate
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr(GeetestLib.fnGeetestValidate); }
        }
        private string RequestGeetestSeccode
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr(GeetestLib.fnGeetestSeccode); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;

            switch (RequestAction.ToLower().Trim())
            {
                case "veifygeetest":
                    VerifyGeetest();
                    break;
                default:
                    break;
            }
            context.Response.End();
        }

        private void VerifyGeetest()
        {
            try
            {
                GeetestLib geetest = new GeetestLib(GeetestConfig_ID, GeetestConfig_Key);
                Byte gt_server_status_code = (Byte)currentContext.Session[GeetestLib.gtServerStatusSessionKey];
                //String userID = (String)Session["userID"];
                int result = 0;
                //String challenge = Request.Form.Get(GeetestLib.fnGeetestChallenge);
                //String validate = Request.Form.Get(GeetestLib.fnGeetestValidate);
                //String seccode = Request.Form.Get(GeetestLib.fnGeetestSeccode);
                if (gt_server_status_code == 1)
                {
                    result = geetest.enhencedValidateRequest(RequestGeetestChallenge, RequestGeetestValidate, RequestGeetestSeccode);
                }
                else
                {
                    result = geetest.failbackValidateRequest(RequestGeetestChallenge, RequestGeetestValidate, RequestGeetestSeccode);
                }

                if (result == 1)
                {
                    currentContext.Response.Write("1");
                }
                else
                {
                    currentContext.Response.Write("0");
                }
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error($"调用极验接口验证逻辑异常,Challenge={RequestGeetestChallenge},Validate={RequestGeetestValidate},Seccode={RequestGeetestSeccode}", ex);
                currentContext.Response.Write("-1," + ex.Message + "}");
            }
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