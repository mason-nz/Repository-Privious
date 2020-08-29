using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Web.Channels;
using System.Web.Security;
using Newtonsoft.Json;
namespace BitAuto.DSC.IM_2015.Web
{
    public partial class MOnlineService : System.Web.UI.Page
    {
        /// <summary>
        /// 网友涞源，业务线
        /// </summary>
        public string SourceType
        {
            get
            {
                return HttpContext.Current.Request.Form["SourceType"];
            }
        }
        public string GuidStr
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        /// <summary>
        /// 网友涞源，业务线
        /// </summary>
        public string LoginID
        {
            get
            {
                string loginstr = string.Empty;
                //惠买车
                if (SourceType == BLL.Util.GetSourceType("惠买车"))
                {
                    if (Request.Cookies["hmc_loginid"] != null)
                    {
                        loginstr = HttpContext.Current.Server.UrlDecode(Request.Cookies["hmc_loginid"].Value);
                    }
                }
                //易车商城
                if (SourceType == BLL.Util.GetSourceType("易车商城"))
                {
                    //if (Request.Cookies["sc_loginid"] != null)
                    //{
                    //    loginstr = HttpContext.Current.Server.UrlDecode(Request.Cookies["sc_loginid"].Value);
                    //}
                    loginstr = GetYCUserID();

                }
                if (SourceType == BLL.Util.GetSourceType("二手车") || SourceType == BLL.Util.GetSourceType("易车惠"))
                {
                    //if (HttpContext.Current.Request.Form["UserID"] != null)
                    //{
                    //    if (HttpContext.Current.Request.Form["UserID"].ToString() != string.Empty)
                    //    {
                    //        loginstr = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["UserID"]);
                    //    }

                    //}
                    loginstr = GetYCUserID();
                }
                if (SourceType == BLL.Util.GetSourceType("易车网") || SourceType == BLL.Util.GetSourceType("易车视频") || SourceType == BLL.Util.GetSourceType("易车报价") || SourceType == BLL.Util.GetSourceType("易车贷款") || SourceType == BLL.Util.GetSourceType("易车问答") || SourceType == BLL.Util.GetSourceType("易车口碑") || SourceType == BLL.Util.GetSourceType("易车养护") || SourceType == BLL.Util.GetSourceType("易车论坛") || SourceType == BLL.Util.GetSourceType("易车车友会") || SourceType == BLL.Util.GetSourceType("我的易车") || SourceType == BLL.Util.GetSourceType("易车购车类"))
                {
                    //if (Request.Cookies["yc_loginid"] != null)
                    //{
                    //    loginstr = HttpContext.Current.Server.UrlDecode(Request.Cookies["yc_loginid"].Value);
                    //}
                    loginstr = GetYCUserID();
                }

                return loginstr;
            }
        }
        /// <summary>
        /// 取userid
        /// </summary>
        /// <returns></returns>
        public string GetYCUserID()
        {
            string strUid = string.Empty;
            //如果是登录用户
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //取uid
                    FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = identity.Ticket;
                    //userId以json字符串的格式存储在ticket.UserData中，将其反序列化后可获取，一下为思路，具体可自行处理
                    var userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ticket.UserData);
                    if (userData != null)
                    {
                        userData.TryGetValue("UserId", out strUid);
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error(string.Format("OnlineService GetYCUserID方法出现异常,异常原因：{0}", ex.Message));
            }
            return strUid;
        }

        /// <summary>
        /// 最后访问页面url
        /// </summary>
        public string UserReferURL
        {
            get { return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]; }
        }
        /// <summary>
        /// 最后访问页面的title
        /// </summary>
        public string EPTitle
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["PageTitle"]); }
        }
        /// <summary>
        /// PostURL
        /// </summary>
        public string EPPostURL
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["SourceUrl"]); }
        }
        /// <summary>
        /// 业务线展示页面title
        /// </summary>
        public string ShowPageTitle
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["ShowPageTitle"]); }
        }
        /// <summary>
        /// 业务线展示页面URL
        /// </summary>
        public string ShowPageUrl
        {
            get { return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Form["ShowPageUrl"]); }
        }

        public ServeTime ST;
        public ServeTime ET;
        public string logoURL;
        public string AgentHeadURL;
        public string UserHeadURL;
        ///// <summary>
        ///// 是否已登录
        ///// </summary>
        ///// <returns></returns>
        //public string isLogined()
        //{
        //    //string flag = "0";
        //    //string ip = BLL.Util.IpToLong().ToString();
        //    //try
        //    //{
        //    //    flag = DefaultChannelHandler.StateManager.IpIsLogined(ip, SourceType);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //}

        //    //return flag;
        //}
        /// <summary>
        /// 验证域名
        /// </summary>
        /// <returns></returns>
        public string CheckDomain()
        {
            string flagstr = "0";
            bool flag = false;
            flag = BLL.Util.CheckDomainName(UserReferURL);
            if (flag)
            {
                flagstr = "1";
            }
            return flagstr;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取服务时间
                ST = new ServeTime(9, 0);
                ET = new ServeTime(18, 0);
                BLL.BaseData.Instance.ReadTime(out ST, out ET, SourceType);
                //惠买车,读取log,头像
                if (SourceType == BLL.Util.GetSourceType("惠买车"))
                {
                    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "hmc_sourcetype");
                }
                //易车商城,读取log,头像
                if (SourceType == BLL.Util.GetSourceType("易车商城"))
                {
                    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "sc_sourcetype");
                }
                //
                //易车视频,4|易车报价,5|易车贷款,7|易车问答,8|易车口碑,9|易车养护,10|易车论坛,11|易车车友会,12|我的易车,13|易车车币,14|易车购车类,15
                //惠买车,读取log,头像
                if (SourceType == BLL.Util.GetSourceType("易车网") || SourceType == BLL.Util.GetSourceType("易车视频") || SourceType == BLL.Util.GetSourceType("易车报价") || SourceType == BLL.Util.GetSourceType("易车贷款") || SourceType == BLL.Util.GetSourceType("易车问答") || SourceType == BLL.Util.GetSourceType("易车口碑") || SourceType == BLL.Util.GetSourceType("易车养护") || SourceType == BLL.Util.GetSourceType("易车论坛") || SourceType == BLL.Util.GetSourceType("易车车友会") || SourceType == BLL.Util.GetSourceType("我的易车") || SourceType == BLL.Util.GetSourceType("易车购车类"))
                {
                    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "yc_sourcetype");
                }
                //二手车,读取log,头像
                if (SourceType == BLL.Util.GetSourceType("二手车"))
                {
                    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "er_sourcetype");
                }
                //易车惠
                if (SourceType == BLL.Util.GetSourceType("易车惠"))
                {
                    BLL.BaseData.Instance.ReadSourceSet(out logoURL, out AgentHeadURL, out UserHeadURL, "ych_sourcetype");
                }
            }
        }
    }
}