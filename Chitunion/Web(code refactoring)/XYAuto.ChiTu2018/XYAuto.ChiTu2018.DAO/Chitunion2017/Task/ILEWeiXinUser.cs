/// <summary>
/// 注释：ILEWeiXinUser
/// 作者：lihf
/// 日期：2018/5/10 9:42:55
/// 版权所有：Copyright  2018 行圆汽车-分发业务中心
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task
{
    public interface ILEWeiXinUser : Repository<Entities.Chitunion2017.LE.LE_WeiXinUser>
    {
        //LE_WeiXinUser GetUserInfoByUserId(int userId, Entities.Enum.UserInfo.UserInfoCategoryEnum userCategoryEnum);
        LE_WeiXinUser GetModelByUserId(int userId);

        /// <summary>
        /// 微信用户授权更新信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int WeiXinUserOperationForUpdateWxUser(Entities.Extend.User.WeiXinUserOperateDo entity);

    }
}
