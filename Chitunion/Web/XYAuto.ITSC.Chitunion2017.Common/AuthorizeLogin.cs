using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Common
{
    public class AuthorizeLogin
    {
        private string LoginPwdKey = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        public readonly static AuthorizeLogin Instance = new AuthorizeLogin();

        public void Insert(Entities.AuthorizeLogin model)
        {
            Dal.AuthorizeLogin.Instance.Insert(model);
        }

        public bool Verification(int appid, string accessToken)
        {
            return Dal.AuthorizeLogin.Instance.Verification(appid, accessToken);
        }

        public bool Verification(string accessToken)
        {
            return Dal.AuthorizeLogin.Instance.Verification(accessToken);
        }

        public void Update(Entities.AuthorizeLogin model)
        {
            Dal.AuthorizeLogin.Instance.Update(model);
        }

        public int p_UserBroker_Insert(Dictionary<string, string> dict,out string username)
        {
            return Dal.AuthorizeLogin.Instance.p_UserBroker_Insert(dict, out username);
        }

        public int p_UserBroker_Update(int userID, string mobile, out string msg)
        {
            return Dal.AuthorizeLogin.Instance.p_UserBroker_Update(userID, mobile, out msg);
        }
    }
}
