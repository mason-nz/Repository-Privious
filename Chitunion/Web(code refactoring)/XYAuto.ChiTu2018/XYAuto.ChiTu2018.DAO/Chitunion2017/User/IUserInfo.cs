/**
*
*创建人：lixiong
*创建时间：2018/5/9 10:55:39
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using System;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Extend.User;
using XYAuto.ChiTu2018.Entities.ThirdModel;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.User
{
    /// <summary>
    /// todo:这里是必须要再次实现Repository接口的
    /// </summary>
    public interface IUserInfo : Repository<UserInfo>
    {
        /// <summary>
        /// 更新分类对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UserInfo Update(UserInfo model);

        /// <summary>
        /// 合并用户基本数据（老的用户覆盖新的)
        /// </summary>
        /// <param name="oldUserId">老用户Id</param>
        /// <param name="newUserId">新用户Id</param>
        /// <returns>item1:id item2:提交过程 id>0 && true 才是真正的成功</returns>
        Tuple<int, bool> CleanUserInfoForM(int oldUserId, int newUserId);

        /// <summary>
        /// 更新用户手机号
        /// </summary>
        /// <param name="useInfo"></param>
        /// <returns></returns>
        bool UpdateUserMobile(UserInfo useInfo);
        /// <summary>
        /// 更新汽车大全用户信息
        /// </summary>
        /// <param name="carUser"></param>
        /// <returns></returns>
        int UpdateCarUser(CarToChiTuUser carUser);

    }
}
