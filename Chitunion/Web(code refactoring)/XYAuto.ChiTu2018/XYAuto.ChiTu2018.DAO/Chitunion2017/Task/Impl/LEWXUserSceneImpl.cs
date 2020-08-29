/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 18:41:15
/// </summary>

using System;
using System.Linq;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task.Impl
{
    public sealed class LEWXUserSceneImpl : RepositoryImpl<LE_WXUserScene>, ILEWXUserScene
    {
        public int DeleteByRecId(int recId)
        {
            return Delete(recId);
        }

        public IQueryable<LE_WXUserScene> GetList(Expression<Func<LE_WXUserScene, bool>> expression)
        {
            return context.Set<LE_WXUserScene>().Where(expression);
        }

        public LE_WXUserScene GetModel(Expression<Func<LE_WXUserScene, bool>> expression)
        {
            return Retrieve(expression);
        }

        public bool GetUserSence(int userId)
        {
            return context.Set<LE_WXUserScene>().Any(x => x.UserID == userId);
        }

        public LE_WXUserScene Insert(LE_WXUserScene model)
        {
            return Add(model);
        }

        public LE_WXUserScene Update(LE_WXUserScene model)
        {
            return Put(model);
        }
    }
}
