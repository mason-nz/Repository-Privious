using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class YTGActivityTask : CommonBll
    {
        private YTGActivityTask() { }
        private static YTGActivityTask _instance = null;
        public static new YTGActivityTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGActivityTask();
                }
                return _instance;
            }
        }
        /// 根据status分组，获取各状态下数量
        /// <summary>
        /// 根据status分组，获取各状态下数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetStatusNum(Entities.QueryYTGActivityTaskInfo query)
        {
            return Dal.YTGActivityTask.Instance.GetStatusNum(query);
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetYTGTask(QueryYTGActivityTaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.YTGActivityTask.Instance.GetYTGLeadsTask(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetYTGProjectTasks(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.YTGActivityTask.Instance.GetYTGProjectTasks(where, order, currentPage, pageSize, out totalCount);
        }

        //根据任务ID串获取任务信息
        public DataTable GetYTGTaskInfoListByIDs(string TaskIDS)
        {
            return Dal.YTGActivityTask.Instance.GetYTGTaskInfoListByIDs(TaskIDS);
        }

        public void UpdateYTGTaskStatus(string strTaskId, string strAssignUser, int nLoginUser, int status)
        {
            Dal.YTGActivityTask.Instance.UpdateYTGTaskStatus(strTaskId, strAssignUser,nLoginUser, status);
        }

        public DataTable GetYTGProjectTasksForExport(QueryYTGActivityTaskInfo query)
        {
            string c1 = Util.GetCaseWhenByEnum(typeof(YTGActivityPlanBuyCarTime));
            string c2 = Util.GetCaseWhenByEnum(typeof(YTGActivityTaskFailReason));
            string c3 = Util.GetCaseWhenByEnum(typeof(YTGActivityTaskStatus));
            return Dal.YTGActivityTask.Instance.GetYTGProjectTasksForExport(query, c1, c2, c3);
        }
    }
}
