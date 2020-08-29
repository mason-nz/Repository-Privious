using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Profit.Impl
{
    /// <summary>
    /// 注释：IIncomeStatisticsCategory
    /// 作者：zhanglb
    /// 日期：2018/5/14 17:09:25
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class IncomeStatisticsImpl : RepositoryImpl<LE_IncomeStatisticsCategory>, IIncomeStatistics
    {
        public decimal GetTotalPriceByUserId(int userId, int categoryId)
        {
            var incomeStatistics = Retrieve(t => t.UserID == userId);
            return incomeStatistics?.IncomePrice ?? 0;
        }
    }
}
