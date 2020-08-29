using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.Extensions
{
    /// <summary>
    /// 注释：GetPageExtension EF 分页扩展方法
    /// 作者：lix
    /// 日期：2018/5/15 14:16:24
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public static class GetPageExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="list"> 数据源 </param>
        /// <param name="expression">where条件</param>
        /// <param name="order"> 排序表达式 </param>
        /// <param name="sortOrder">排序规则</param>
        /// <param name="page"> 第几页 </param>
        /// <param name="size"> 每页记录数 </param>
        /// <param name="count"> 记录总数 </param>
        /// <returns></returns>
        public static IQueryable<T> Pagination<T, TKey>(this IQueryable<T> list, Expression<Func<T, bool>> expression, Expression<Func<T, TKey>> order, SortOrder sortOrder, int page, int size, out int count)
        {
            count = list.Where(expression).Count();
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return list.Where(expression).OrderBy(order).Skip((page - 1) * size).Take(size);

                case SortOrder.Descending:
                    return list.Where(expression).OrderByDescending(order).Skip((page - 1) * size).Take(size);
            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");
        }
    }
}
