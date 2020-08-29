/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 20:00:55
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Infrastructure;

namespace XYAuto.ChiTu2018.BO.Task
{
    public class LEWXUserSceneBO
    {
        private static ILEWXUserScene LEWXUserScene()
        {
            return IocMannager.Instance.Resolve<ILEWXUserScene>();
        }
        public int DeleteByRecId(int recId)
        {
            return LEWXUserScene().Delete(recId);
        }

        public LE_WXUserScene GetModel(Expression<Func<LE_WXUserScene, bool>> expression)
        {
            return LEWXUserScene().Retrieve(expression);
        }

        public IQueryable<LE_WXUserScene> GetList(Expression<Func<LE_WXUserScene, bool>> expression)
        {
            return LEWXUserScene().GetList(expression);
        }
        public LE_WXUserScene Insert(LE_WXUserScene model)
        {
            return LEWXUserScene().Add(model);
        }

        public LE_WXUserScene Update(LE_WXUserScene model)
        {
            return LEWXUserScene().Put(model);
        }

        public LE_WXUserScene GetModelByUserIdSceneId(int userId,int sceneId)
        {
            return LEWXUserScene().Retrieve(x => x.UserID == userId && x.SceneID == sceneId);
        }

        public IQueryable<LE_WXUserScene> GetListByStatusUserId(int status,int userId)
        {
            return LEWXUserScene().GetList(x => x.Status == status && x.UserID == userId);
        }
        public List<LE_WXUserScene> GetListByQuery(LE_WXUserScene query)
        {
            var queryTable = LEWXUserScene().Queryable();
            if (query.UserID > 0)
                queryTable = queryTable.Where(x => x.UserID == query.UserID);

            if (query.Status != null)
                queryTable = queryTable.Where(x => x.Status == query.Status);

            if (query.SceneID > 0)
                queryTable = queryTable.Where(x => x.SceneID == query.SceneID);

            return queryTable.ToList();
        }
    }
}
