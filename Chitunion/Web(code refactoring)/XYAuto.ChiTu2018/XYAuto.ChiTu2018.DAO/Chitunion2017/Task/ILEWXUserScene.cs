using System;
using System.Linq;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task
{
    public interface ILEWXUserScene : Repository<LE_WXUserScene>
    {
        /// <summary>
        /// 根据用户ID查询是否有选中的分类
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool GetUserSence(int userId);

        /// <summary>
        /// 获取分类对象
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        LE_WXUserScene GetModel(Expression<Func<LE_WXUserScene, bool>> expression);

        /// <summary>
        /// 获取对象明细
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<LE_WXUserScene> GetList(Expression<Func<LE_WXUserScene, bool>> expression);

        /// <summary>
        /// 根据主键删除分类
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        int DeleteByRecId(int recId);

        /// <summary>
        /// 更新分类对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        LE_WXUserScene Update(LE_WXUserScene model);

        /// <summary>
        /// 新增分类对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        LE_WXUserScene Insert(LE_WXUserScene model);
    }
}
