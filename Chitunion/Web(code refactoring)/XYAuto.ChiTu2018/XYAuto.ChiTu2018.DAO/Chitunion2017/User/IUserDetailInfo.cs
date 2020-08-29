/**
*
*创建人：lixiong
*创建时间：2018/5/11 9:32:26
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.User
{
    public  interface IUserDetailInfo : Repository<UserDetailInfo>
    {
        //UserDetailInfo GetUserDetailByUserId(int userId);
        int UpdateUserDetail(UserDetailInfo userDetailInfo);
    }
}
