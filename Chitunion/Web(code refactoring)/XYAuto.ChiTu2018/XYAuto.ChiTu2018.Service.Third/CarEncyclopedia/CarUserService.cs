using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.Third.CarEncyclopedia
{
    /// <summary>
    /// 注释：CarUserInfoService
    /// 作者：zhanglb
    /// 日期：2018/6/12 18:34:57
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class CarUserService
    {
        #region Instance
        private static readonly Lazy<CarUserService> Linstance = new Lazy<CarUserService>(() => new CarUserService());

        public static CarUserService Instance => Linstance.Value;
        #endregion
        /// <summary>
        /// 根据汽车大全用户Token，调用大全API接口，返回用户信息
        /// </summary>
        /// <param name="token">汽车大全用户Token</param>
        /// <returns>返回用户信息</returns>
        public CarUserInfo GetUserInfoByToken(string token)
        {
            var url = CTUtils.Config.ConfigurationUtil.GetAppSettingValue("GetCarEncyclopediaUser");
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = "http://m.api.qichedaquan.com/user/userinfo/api/{0}?app_id=5d81db99484c0019cab240b3d04e9a4a";
                }
                url = string.Format(url, token);
                //CTUtils.Html.HtmlHelper.HttpGetRequest(url);
                CarUserResult carUser = null;
                if (carUser != null && carUser.Code == 10000)
                {
                    return carUser.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"[GetUserInfoByToken]抓取错误URL:{url}", ex);
                return null;
            }
        }
    }
}
