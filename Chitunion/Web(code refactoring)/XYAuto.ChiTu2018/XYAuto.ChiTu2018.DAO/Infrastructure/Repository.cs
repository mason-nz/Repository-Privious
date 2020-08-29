using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Entities;

namespace XYAuto.ChiTu2018.DAO.Infrastructure
{
    /// <summary>
    /// 注释：IRepository
    /// 作者：guansl
    /// 日期：2014/7/9 20:22:03
    /// </summary>
    public interface Repository<T>: IBaseRepository where T : class, new()
    {
        /// <summary>
        /// 根据对象全局唯一标识删除对象
        /// </summary>
        /// <param name="id">对象全局唯一标识</param>
        /// <returns>删除的对象数量</returns>
        int Delete(int id);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回更新后实体</returns>
        T Put(T entity);

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// 根据条件表达式检索对象
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        /// <exception cref = "ArgumentNullException" > source 为 null</exception>
        T Retrieve(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 返回全部结果集
        /// </summary>
        /// <returns></returns>
        List<T> FindAll();

        /// <summary>
        /// 返回全部结果集(TQueryable)
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Queryable();

        /// <summary>
        /// 返回全部结果集
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <param name="sortPredicate">排序表达式</param>
        /// <param name="sortOrder">0：升序1：降序</param>
        /// <param name="skip">取后多少条</param>
        /// <param name="take">取前多少条</param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        List<T> FindAll(Expression<Func<T, bool>> expression, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int skip, int take, out int total);

        ///// <summary>
        ///// 更新指定字段
        ///// </summary>
        ///// <param name="model">实体</param>
        ///// <param name="fileds">更新字段数组</param>
        ///// <returns></returns>
        //int UpdateModelFields(T model, List<string> fileds);

    }
}
