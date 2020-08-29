using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    /// <summary>
    /// CommonHandler 的摘要说明
    /// </summary>
    public class CommonHandler : IHttpHandler
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        #region 属性定义

        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// 需要加密的信息
        /// <summary>
        /// 需要加密的信息
        /// </summary>
        public string EncryptInfo
        {
            get
            {
                if (Request["EncryptInfo"] != null)
                {
                    return HttpUtility.UrlDecode(Request["EncryptInfo"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "encryptstring":
                    EncryptString(out msg);
                    break;
            }
            context.Response.Write(msg);
        }
        /// 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="msg"></param>
        private void EncryptString(out string msg)
        {
            msg = BLL.Util.EncryptString(EncryptInfo);
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