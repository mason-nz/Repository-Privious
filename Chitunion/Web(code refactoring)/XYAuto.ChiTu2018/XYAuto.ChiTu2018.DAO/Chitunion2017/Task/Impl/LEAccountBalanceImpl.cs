/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 14:31:00
/// </summary>

using System.Collections.Generic;
using System.Linq;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task.Impl
{
    public class LEAccountBalanceImpl : RepositoryImpl<LE_AccountBalance>, ILEAccountBalance
    {
        public IEnumerable<LE_AccountBalance> GetListByTaskIDUserID(string orderIDs)
        {
            return context.Set<LE_AccountBalance>().Where(x => orderIDs.Contains(x.OrderID.ToString())).OrderByDescending(x => new { x.RecID });
        }
    }
}
