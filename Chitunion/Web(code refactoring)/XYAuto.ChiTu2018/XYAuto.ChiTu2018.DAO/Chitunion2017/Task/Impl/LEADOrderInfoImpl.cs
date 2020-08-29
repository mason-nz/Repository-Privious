/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 14:22:57
/// </summary>

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task.Impl
{
    public sealed class LEADOrderInfoImpl : RepositoryImpl<LE_ADOrderInfo>, ILEADOrderInfo
    {
        public IEnumerable<LE_ADOrderInfo> GetListByUserID(int taskID, int userID)
        {
            return context.Set<LE_ADOrderInfo>().Where(x => x.RecID == taskID && x.UserID == userID).OrderByDescending(x => new { x.RecID }).ToList();
        }
        public List<LE_ADOrderInfo> GetListByPage(Expression<Func<LE_ADOrderInfo, bool>> expression, Expression<Func<LE_ADOrderInfo, dynamic>> sortPredicate, SortOrder sortOrder, int pageIndex, int pageSize, out int count)
        {
            int skip = pageIndex * pageSize;
            count = context.Set<LE_ADOrderInfo>().Where(expression).Count();
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return context.Set<LE_ADOrderInfo>().Where(expression).OrderBy(sortPredicate).Skip(skip).Take(pageSize).ToList();

                case SortOrder.Descending:
                    return context.Set<LE_ADOrderInfo>().Where(expression).OrderByDescending(sortPredicate).Skip(skip).Take(pageSize).ToList();

            }
            return null;
        }
        public IEnumerable<LE_ADOrderInfo> GetList(int pageIndex, int pageSize, out int count)
        {
            return FindAll(w => 1 == 1, o => new { o.CreateTime }, SortOrder.Descending, pageIndex * pageSize, pageSize, out count);
        }
    }
}
