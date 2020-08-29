/**
*
*创建人：lixiong
*创建时间：2018/5/9 9:53:20
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using System;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.LE
{
    /// <summary>
    /// todo:这里是必须要再次实现Repository接口的
    /// </summary>
    public interface ILeWithdrawalsDetail : Repository<LE_WithdrawalsDetail>
    {
        void UpdateStatus(int recId);

        /// <summary>
        /// 提交提现申请
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>item1:提现id item2:提交过程 id>0 && true 才是真正的成功</returns>
        Tuple<int, bool> PostApplication(LE_WithdrawalsDetail entity);

    }
}
