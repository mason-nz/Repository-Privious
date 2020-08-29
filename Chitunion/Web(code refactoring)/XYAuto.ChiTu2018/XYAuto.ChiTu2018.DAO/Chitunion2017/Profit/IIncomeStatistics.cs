namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Profit
{
    /// <summary>
    /// 注释：IIncomeStatistics
    /// 作者：zhanglb
    /// 日期：2018/5/14 17:11:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public interface IIncomeStatistics
    {
        /// <summary>
        /// 获取用户对应分类总收益
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="categoryId">分类(103001 订单统计;103002 邀请红包统计;103003 签到红包统计 )</param>
        /// <returns></returns>
        decimal GetTotalPriceByUserId(int userId, int categoryId);
    }
}
