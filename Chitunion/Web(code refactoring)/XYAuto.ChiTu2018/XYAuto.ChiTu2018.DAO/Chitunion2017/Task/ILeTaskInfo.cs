/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 12:01:28
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Extend.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task
{
    public interface ILeTaskInfo : Repository<LE_TaskInfo>
    {
        IQueryable<LE_TaskInfo> GetQuerys();

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        LE_TaskInfo LE_TaskInfoByPK(int id);
        /// <summary>
        /// 分頁獲取數據
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<LE_TaskInfo> GetLETaskInfoList(int pageIndex, int pageSize, out int count);
        IEnumerable<LE_TaskInfo> GetDataByPage(int taskIndex, int topSize, int categoryId);

        /// <summary>
        /// 获取任务对象
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        LE_TaskInfo GetModel(Expression<Func<LE_TaskInfo, bool>> expression);
        /// <summary>
        /// 根据任务ID获取阅读数
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        int GetReadNum(int taskId);
        
    }
}
