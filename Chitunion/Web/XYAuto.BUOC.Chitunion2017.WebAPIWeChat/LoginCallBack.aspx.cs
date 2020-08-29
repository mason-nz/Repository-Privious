using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    public partial class LoginCallBack : System.Web.UI.Page
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];

        protected void Page_Load(object sender, EventArgs e)
        {
            Loger.Log4Net.Info($"LoginCallBack start..");
            var code = Request["code"];
            var state = Request["state"];
            
            Loger.Log4Net.Info($"LoginCallBack code:{code}  state:{state}");
            if (!string.IsNullOrWhiteSpace(code))
            {
                var accessToken = OAuthApi.GetAccessToken(appId, secret, code);
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(accessToken.access_token, accessToken.openid);
                var userInfobase = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(appId, userInfo.openid, Language.zh_CN);

                Loger.Log4Net.Info($"LoginCallBack accessToken:{JsonConvert.SerializeObject(accessToken)} ");
                Loger.Log4Net.Info($"LoginCallBack userInfobase:{JsonConvert.SerializeObject(userInfobase)} ");
            }
            Loger.Log4Net.Info($"LoginCallBack end..");
        }

        private void ToLogin()
        {
            //todo:
            //1.模拟用户登录，写入cookie
            //2.判断用户openid是否存在系统中
        }
    }
}