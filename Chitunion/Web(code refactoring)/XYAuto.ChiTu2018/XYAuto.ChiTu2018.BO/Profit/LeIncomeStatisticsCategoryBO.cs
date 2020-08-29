using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Profit;

namespace XYAuto.ChiTu2018.BO.Profit
{
    /// <summary>
    /// 注释：IncomeStatisticsBO
    /// 作者：zhanglb
    /// 日期：2018/5/14 17:25:07
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeIncomeStatisticsCategoryBO
    {
        private IIncomeStatistics IncomeStatistics()
        {
            return IocMannager.Instance.Resolve<IIncomeStatistics>();
        }
        /// <summary>
        /// 获取用户对应分类总收益
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="categoryId">分类(103001 订单统计;103002 邀请红包统计;103003 签到红包统计 )</param>
        /// <returns></returns>
        public decimal GetTotalPriceByUserId(int userId, int categoryId)
        {
            return IncomeStatistics().GetTotalPriceByUserId(userId, categoryId);
        }

    }
}
