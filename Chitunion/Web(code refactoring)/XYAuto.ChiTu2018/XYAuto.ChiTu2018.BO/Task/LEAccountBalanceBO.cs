using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.BO.Task
{
    /// <summary>
    /// 注释：<NAME>
    /// 作者：lihf
    /// 日期：2018/5/9 14:35:19
    /// </summary>
    public class LEAccountBalanceBO
    {
        private static ILEAccountBalance LEAccountBalance()
        {
            return IocMannager.Instance.Resolve<ILEAccountBalance>();
        }
        public List<LE_AccountBalance> GetListByTaskIDUserID(string orderIDs)
        {
            return LEAccountBalance().GetListByTaskIDUserID(orderIDs).ToList();
        }

        public List<LE_AccountBalance> GetList(Expression<Func<LE_AccountBalance, bool>> expression)
        {
            return LEAccountBalance().Queryable().Where<LE_AccountBalance>(expression).ToList();
        }

        public List<LE_AccountBalance> GetListByUserIDOrderId(int userId,int orderId)
        {
            return
                LEAccountBalance()
                    .Queryable()
                    .Where<LE_AccountBalance>(x => x.CreateUserID == userId && x.OrderID == orderId)
                    .ToList();
        }
    }
}
