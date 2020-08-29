using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.ITSC.Chitunion2017.Entities.UserManage;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    /// <summary>
    /// SwitchPage 的摘要说明
    /// </summary>
    public class SwitchPage : IHttpHandler, IRequiresSessionState
    {
        private string RequestToken
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr("token"); }
        }
        private string RequestPromotionName
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestFormStr("promotionName"); }
        }
        

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录开始-》" + RequestToken);
            if (!string.IsNullOrEmpty(RequestToken))
            {
                int userID = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.InsertUserInfoByToken(RequestToken);
                if (userID > 0)
                {
                    XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》成功");
                    context.Response.Write(1);
                }
                else
                {
                    XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》根据token未获取到登录信息");
                    context.Response.Write(-1);
                }
            }
            else
            {
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("大全登录-》请求参数为空");
                context.Response.Write(-1);
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