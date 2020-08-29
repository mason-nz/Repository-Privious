/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 14:24:52
/// </summary>
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Query;

namespace XYAuto.ChiTu2018.BO.Task
{
    public class LEADOrderInfoBO
    {
        private ILEADOrderInfo LEADOrderInfo()
        {
            return IocMannager.Instance.Resolve<ILEADOrderInfo>();
        }

        public List<LE_ADOrderInfo> GetList(Expression<Func<LE_ADOrderInfo, bool>> expression)
        {
            return LEADOrderInfo().Queryable().Where(expression).ToList();
        }
        public List<LE_ADOrderInfo> GetListByUserID(int taskID, int userID)
        {
            return LEADOrderInfo().GetListByUserID(taskID, userID).ToList();
        }
        public LE_ADOrderInfo GetModel(Expression<Func<LE_ADOrderInfo, bool>> expression)
        {
            return LEADOrderInfo().Retrieve(expression);
        }
        //public List<LE_ADOrderInfo> GetListByPage(Expression<Func<LE_ADOrderInfo, bool>> expression, Expression<Func<LE_ADOrderInfo, dynamic>> sortPredicate, SortOrder sortOrder, int pageIndex, int pageSize, out int count)
        //{
        //    return LEADOrderInfo().GetListByPage(expression, sortPredicate, sortOrder, pageIndex, pageSize, out count);
        //}
        public List<LE_ADOrderInfo> GetListByPage(LE_ADOrderInfoQuery queryArgs, out int count)
        {
            var query =
                LEADOrderInfo().Queryable().Where(x => x.Status == queryArgs.Status && x.UserID == queryArgs.UserID);
            count = query.Count();
            return query.OrderByDescending(x => x.CreateTime).Skip((queryArgs.PageIndex - 1) * queryArgs.PageSize).
                Take(queryArgs.PageSize).ToList();
        }
        public LE_ADOrderInfo GetModelByRecId(int recId)
        {
            return LEADOrderInfo().Retrieve(x => x.RecID == recId);
        }
        public LE_ADOrderInfo GetModelByRecIdUserId(int recId, int userId)
        {
            return LEADOrderInfo().Retrieve(x => x.RecID == recId && x.UserID == userId);
        }
        public LE_ADOrderInfo GetModelByTaskidUserId(int taskId, int userId)
        {
            return LEADOrderInfo().Retrieve(x => x.TaskID == taskId && x.UserID == userId);
        }
        public List<LE_ADOrderInfo> GetListByTaskidUserId(int taskId, int userId)
        {
            return LEADOrderInfo().Queryable().Where(x => x.TaskID == taskId && x.UserID == userId).ToList();
        }
        public List<LE_ADOrderInfo> GetListByQuery(LE_ADOrderInfoQuery query)
        {
            var queryTable = LEADOrderInfo().Queryable();

            if (query.UserID > 0)
                queryTable = queryTable.Where(x => x.UserID == query.UserID);

            //queryTable = queryTable.Where(x => x.CreateTime.Value.Date. == DateTime.Now.Date.AddDays(query.Days));
            if (query.Days == 0) return queryTable.ToList();
            var currentDate = DateTime.Now.AddDays(query.Days);
            queryTable =
                queryTable.Where(
                    x =>
                        x.CreateTime.Value.Year == currentDate.Year && x.CreateTime.Value.Month == currentDate.Month &&
                        x.CreateTime.Value.Day == currentDate.Day);
            return queryTable.ToList();
        }

        public int GetUserOrderCount(int userId)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var startDate = Convert.ToDateTime(date);
            var endDate = Convert.ToDateTime(startDate.AddDays(1));
            return LEADOrderInfo().Queryable().AsNoTracking().Count(s => s.UserID == userId && s.CreateTime >= startDate && s.CreateTime < endDate);

            //return LEADOrderInfo().Queryable().AsNoTracking().Count(s => s.UserID == userId && DbFunctions.DiffDays(s.CreateTime, DateTime.Now) == 0);
        }
    }
}
