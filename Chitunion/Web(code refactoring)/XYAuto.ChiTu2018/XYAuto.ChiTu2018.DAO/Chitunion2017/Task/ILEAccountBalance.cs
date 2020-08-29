using System.Collections.Generic;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task
{
    public interface ILEAccountBalance : Repository<LE_AccountBalance>
    {
        /// <summary>
        /// 根据订单号获取收益明细
        /// </summary>
        /// <param name="orderIDs">以逗号","分隔的订单号拼接字符串</param>
        /// <returns></returns>
        IEnumerable<LE_AccountBalance> GetListByTaskIDUserID(string orderIDs);        
    }
}
