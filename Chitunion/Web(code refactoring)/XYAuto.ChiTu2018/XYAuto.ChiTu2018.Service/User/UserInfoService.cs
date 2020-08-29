using System;
using XYAuto.ChiTu2018.BO.User;

namespace XYAuto.ChiTu2018.Service.User
{
    /// <summary>
    /// 注释：UserInfoService
    /// 作者：lix
    /// 日期：2018/5/11 9:46:05
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class UserInfoService
    {
        #region 单例

        private UserInfoService() { }
        private static readonly Lazy<UserInfoService> Linstance = new Lazy<UserInfoService>(() => { return new UserInfoService(); });

        public static UserInfoService Instance => Linstance.Value;

        #endregion

        public dynamic GetUserInfo(int userId)
        {
            return new UserInfoBO().GetInfo(userId);
        }
    }
}
