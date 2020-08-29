/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 12:12:37
/// </summary>

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Extend.LE;
using XYAuto.CTUtils.Config;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Task.Impl
{
    public class LeTaskInfoImpl : RepositoryImpl<LE_TaskInfo>, ILeTaskInfo
    {
        public LE_TaskInfo LE_TaskInfoByPK(int id)
        {
            return Retrieve(w => w.RecID == id);
        }
        public IQueryable<LE_TaskInfo> GetQuerys()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<LE_TaskInfo> GetLETaskInfoList(int pageIndex, int pageSize, out int count)
        {
            return FindAll(w => 1 == 1, o => new { o.CreateTime }, SortOrder.Descending, pageIndex * pageSize, pageSize, out count);
        }


        public IEnumerable<LE_TaskInfo> GetDataByPage(int taskIndex, int topSize, int categoryId)
        {
            if (taskIndex > 0)
                return
                    context.Set<LE_TaskInfo>()
                        .Where(
                            x =>
                                x.RecID < taskIndex && x.Status == 194001 && x.TaskType == 192001 &&
                                x.CategoryID == categoryId)
                        .OrderByDescending(x => new { x.RecID })
                        .Take(topSize)
                        .ToList();
            else
            {
                return
                    context.Set<LE_TaskInfo>()
                        .Where(x => x.Status == 194001 && x.TaskType == 192001 && x.CategoryID == categoryId)
                        .OrderByDescending(x => new { x.RecID })
                        .Take(topSize)
                        .ToList();
            }
        }
        public LE_TaskInfo GetModel(Expression<Func<LE_TaskInfo, bool>> expression)
        {
            return Retrieve(expression);
        }

        public int GetReadNum(int taskId)
        {
            var randomNumber = ConfigurationUtil.GetAppSettingValue("GetRandomNumber", false) ?? "21,155.22";


            string SQL = "SELECT dbo.[f_GenArticleReadNum](CreateTime, @Jishu,@Xishu) FROM dbo.LE_TaskInfo WHERE RecID=@TaskID";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@Jishu",randomNumber.Split(',')[0]),
                new SqlParameter("@Xishu",randomNumber.Split(',')[1]),
                new SqlParameter("@TaskID",taskId) };

            return context.Database.SqlQuery<int>(SQL, sqlParams).FirstOrDefault();

        }

       
    }
}