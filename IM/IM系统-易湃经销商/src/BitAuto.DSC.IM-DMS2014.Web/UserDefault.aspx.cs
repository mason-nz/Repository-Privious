using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Web.Channels;
using BitAuto.DSC.IM2014.Core;
using BitAuto.DSC.IM_DMS2014.Core;

namespace BitAuto.DSC.IM2014.Server.Web
{
    public partial class UserDefault : System.Web.UI.Page
    {
        public string Reset
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["Reset"]))
                {
                    return Request["Reset"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取页面涞源
                string UserReferURL = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
                //取网友标识
                string UseIMID = GetUserIMID();
                //判断是否存在该标识的网友实体，如果存在提示不能重复登录
                CometClient cometmodel = null;
                try
                {
                    cometmodel = BitAuto.DSC.IM_DMS2014.Core.DefaultChannelHandler.StateManager.GetCometClient(UseIMID);
                }
                catch (Exception)
                {

                }
                if (cometmodel != null)
                {
                    ////如果Reset是1说明是重新登录
                    //if (Reset == "1")
                    //{
                    //    //清空客户端实体对象
                    //    DefaultChannelHandler.StateManager.KillIdleCometClient(cometmodel.PrivateToken);
                    //    DefaultChannelHandler.StateManager.KillWaitRequest(cometmodel.PrivateToken);
                    //}
                    //else
                    //{
                    Response.Write("<script type='text/javascript'>alert('您已经登录，不能重复登录');window.opener = null; window.open('', '_self'); window.close();</script>");
                    //}
                }
                else
                {
                    //cometmodel=DefaultChannelHandler.StateManager.InitializeClient(
                      //  UseIMID, UseIMID, UseIMID, 3, 600, UserReferURL, (Int32)Entities.UserType.User);
                    Response.Redirect("UserChat.aspx?username=" + UseIMID);
                }
            }
        }
        /// <summary>
        /// 取网友标识，如果Cookie存在，在Cookie中取，不存在创建
        /// </summary>
        /// <returns></returns>
        private string GetUserIMID()
        {
            HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies["UserIMID"];
            if (Cookie != null)
            {
                return Cookie.Value.ToString();
            }
            else
            {
                HttpCookie Cookienew = new HttpCookie("UserIMID");
                //Cookie.Domain = ".xxx.com";//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                //Cookie.Expires = DateTime.Now.AddDays(strDay);
                string Guidstr = System.Guid.NewGuid().ToString();
                Cookienew.Value = Guidstr;
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookienew);
                return Guidstr;
            }

        }
    }
}