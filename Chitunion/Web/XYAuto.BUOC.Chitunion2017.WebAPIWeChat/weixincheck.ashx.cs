using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    /// <summary>
    /// weixincheck 的摘要说明
    /// </summary>
    public class weixincheck : IHttpHandler
    {
        //private bool RequestIsCheckWeiXin
        //{
        //    get { return GetCurrentRequestQueryStr("IsCheckWeiXin"); }
        //}
        //private bool GetCurrentRequestQueryStr(string r)
        //{
        //    bool flag = false;
        //    if (System.Web.HttpContext.Current.Request.QueryString[r] == null)
        //    {
        //        return flag;
        //    }
        //    else
        //    {
        //        string query = System.Web.HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request.QueryString[r].ToString().Trim().Replace("+", "%2B"));
        //        bool.TryParse(query, out flag);
        //        return flag;
        //    }
        //}

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();

            //var url = context.Request.UrlReferrer;            
            //HttpContext.Current.Response.Redirect(resurl);

            //XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
            //bool flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
            //if (flag && lu != null && (lu.Category == 29001 || lu.Category == 29002))
            //{
            //    //flag = true;
            //    ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(lu.UserID, url.AbsoluteUri);
            //}
            //else
            {
                var uriRefer = HttpContext.Current.Request.UrlReferrer;
                var userAgent = HttpContext.Current.Request.UserAgent;
                if (string.IsNullOrWhiteSpace(userAgent) || (!userAgent.Contains("MicroMessenger")))
                {

                    //string closeWindowStr = "var userAgent = navigator.userAgent; if (userAgent.indexOf('Firefox') != -1 || userAgent.indexOf('Chrome') !=-1) { window.location.href='about: blank'; }else if(userAgent.indexOf('Android') > -1 || userAgent.indexOf('Linux') > -1){ window.opener=null;window.open('about:blank','_self','').close(); }else { window.pener = null; window.open('about: blank', '_self'); window.close(); }";
                    //HttpContext.Current.Response.Write("document.write(\"<sc\"+\"ript> alert('仅在微信内部访问');" + closeWindowStr + " </scr\"+\"ipt>\");");
                    //HttpContext.Current.Response.End();
                }
                else if (uriRefer != null)
                {
                    //未授权时，调整页面进行授权
                    if (string.IsNullOrEmpty(uriRefer.Query) ||
                        (!string.IsNullOrEmpty(uriRefer.Query) && uriRefer.Query.ToLower().IndexOf("isauth=1") < 0))
                    {
                        string resurl = $"{ConfigurationManager.AppSettings["Domin"]}{"/api/OAuth2/Index?returnUrl="}{HttpUtility.UrlEncode(uriRefer.AbsoluteUri)}";
                        context.Response.Write($"document.write(\"<sc\"+\"ript> window.location='{resurl}';" + "</scr\"+\"ipt>\");");
                        return;
                    }
                    //授权之后，添加访问日志
                    XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
                    bool flag = XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu);
                    if (flag && lu != null && (lu.Category == 29001 || lu.Category == 29002))
                    {

                        ITSC.Chitunion2017.BLL.ChituMedia.WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(lu.UserID, uriRefer.AbsoluteUri, HttpContext.Current.Request.UserAgent);
                    }
                }
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