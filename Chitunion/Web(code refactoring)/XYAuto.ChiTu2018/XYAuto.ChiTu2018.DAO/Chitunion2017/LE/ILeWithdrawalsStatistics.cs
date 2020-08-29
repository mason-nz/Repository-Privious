using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.LE
{
    /// <summary>
    /// 注释：ILeWithdrawalsStatistics
    /// 作者：lix
    /// 日期：2018/5/11 10:53:27
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public interface ILeWithdrawalsStatistics : Repository<LE_WithdrawalsStatistics>
    {
        /// <summary>
        /// 用户提现-更新资金账户统计-修改用户资金冻结
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        int UpdatePostWithdrawas(int userId, decimal money);


        /// <summary>
        /// 添加用户金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="incomPrice"></param>
        /// <returns></returns>
        int AddWithdrawals(int userId, decimal incomPrice);
    }
}
