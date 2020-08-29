using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using static System.Data.Entity.Core.Objects.EntityFunctions;

namespace XYAuto.ChiTu2018.BO.LE
{
    /// <summary>
    /// 注释：LeIncomeDetailBO
    /// 作者：lix
    /// 日期：2018/5/23 15:28:01
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeIncomeDetailBO
    {
        private readonly ILeIncomeDetail _leIncomeDetail;
        public LeIncomeDetailBO()
        {
            //_leIncomeDetail = new IoCConfig().Resolve<ILeIncomeDetail>();
            _leIncomeDetail = IocMannager.Instance.Resolve<ILeIncomeDetail>();
        }

        /// <summary>
        /// 获取用户今日收益
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetTodayIncome(int userId)
        {
            return _leIncomeDetail.Queryable().AsNoTracking().Where(s => s.UserID == userId && DiffDays(DateTime.Now.AddDays(-1), s.IncomeTime).Value == 0)
                 .Sum(s => s.IncomePrice) ?? 0;
        }
    }
}
