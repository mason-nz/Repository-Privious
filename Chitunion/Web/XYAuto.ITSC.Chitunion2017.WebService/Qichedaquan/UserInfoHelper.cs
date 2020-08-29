using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.Qichedaquan
{
    public class UserInfoHelper
    {
        #region Instance
        public static readonly UserInfoHelper Instance = new UserInfoHelper();
        #endregion

        /// <summary>
        /// 根据汽车大全用户Token，调用大全API接口，返回用户信息
        /// </summary>
        /// <param name="token">汽车大全用户Token</param>
        /// <returns>返回用户信息</returns>
        public UserInfo GetUserInfoByToken(string token)
        {
            string url = ConfigurationManager.AppSettings["GetCarEncyclopediaUser"];
            try
            {

                if (string.IsNullOrEmpty(url))
                {
                    url = "http://m.api.qichedaquan.com/user/userinfo/api/{0}?app_id=5d81db99484c0019cab240b3d04e9a4a";
                }
                url = string.Format(url, token);
                UserResult urt = Common.HttpHelper.HttpWebRequestCreate<UserResult>(url, "", Common.RequsetType.GET, (int)Common.RequestContentType.JSON);
                Common.Loger.Log4Net.Info("调用大全获取用户接口：" + JsonConvert.SerializeObject(urt));
                if (urt != null && urt.Code == 10000)
                {
                    return urt.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Common.Loger.Log4Net.Error($"[GetUserInfoByToken]抓取错误URL:{url}", ex);
                Common.Loger.Log4Net.Error($"[GetUserInfoByToken]抓取错误", ex);
                return null;
            }
        }
    }
}
