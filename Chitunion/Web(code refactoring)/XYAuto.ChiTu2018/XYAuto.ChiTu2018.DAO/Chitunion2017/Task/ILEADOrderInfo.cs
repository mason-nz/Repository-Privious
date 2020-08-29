using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task
{
    public interface ILEADOrderInfo : Repository<LE_ADOrderInfo>
    {
        /// <summary>
        /// 根据任务ID、用户ID获取订单明细
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IEnumerable<LE_ADOrderInfo> GetListByUserID(int taskID,int userID);
        /// <summary>
        /// 分页查询明细
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortPredicate"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<LE_ADOrderInfo> GetListByPage(Expression<Func<LE_ADOrderInfo, bool>> expression, Expression<Func<LE_ADOrderInfo, dynamic>> sortPredicate, SortOrder sortOrder, int pageIndex, int pageSize, out int count);
    }
}
