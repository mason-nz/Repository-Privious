/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 14:01:22
/// </summary>
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.Task;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Extend.LE;
using XYAuto.ChiTu2018.Infrastructure.LeTask;
using XYAuto.CTUtils.Config;

namespace XYAuto.ChiTu2018.BO.Task
{
    public class LeTaskInfoBO
    {
        private static ILeTaskInfo LeTaskInfo()
        {
            return IocMannager.Instance.Resolve<ILeTaskInfo>();
        }

        public LE_TaskInfo GeTaskInfo(int recId)
        {
            return LeTaskInfo().LE_TaskInfoByPK(recId);
        }

        public IEnumerable<LE_TaskInfo> GetDataByPage(int taskIndex, int topSize, int categoryId)
        {
            //return LeTaskInfo().GetDataByPage(taskIndex, topSize, categoryID);

            var queryTable = LeTaskInfo().Queryable().Where(x => x.Status == 194001 && x.TaskType == 192001);
            if (taskIndex > 0)
                queryTable = queryTable.Where(x => x.RecID < taskIndex);

            if (categoryId > 0)
                queryTable = queryTable.Where(x => x.CategoryID == categoryId);

            return queryTable.OrderByDescending(x => new { x.RecID })
                .Take(topSize);
        }
        public bool GetUserSence(int userId)
        {
            return IocMannager.Instance.Resolve<ILEWXUserScene>().GetUserSence(userId);
        }

        public LE_TaskInfo GetModel(Expression<Func<LE_TaskInfo, bool>> expression)
        {
            return LeTaskInfo().GetModel(expression);
        }

        public int GetReadNum(int taskId)
        {
            return LeTaskInfo().GetReadNum(taskId);
        }
        public LE_TaskInfo GetModelByMaterialId(int materialId)
        {
            return LeTaskInfo().GetModel(x => x.MaterialID == materialId);
        }
        public LE_TaskInfo GetModelByRecId(int recId)
        {
            return LeTaskInfo().GetModel(x => x.RecID == recId);
        }
        public List<LE_TaskInfo> GetListByQuery(LE_TaskInfo query)
        {
            var queryTable = LeTaskInfo().Queryable();
            if (query.Status > 0)
                queryTable = queryTable.Where(x => x.Status == query.Status);

            if (query.CategoryID > 0)
                queryTable = queryTable.Where(x => x.CategoryID == query.CategoryID);

            return queryTable.ToList();
        }

        public LE_TaskInfo GetRandomInfo()
        {
            return LeTaskInfo().Queryable().Where(p => 0 >= DbFunctions.DiffDays(p.EndTime, new DateTime().Date)).OrderBy(a => Guid.NewGuid()).Take(1).FirstOrDefault();
        }
       
    }
}
