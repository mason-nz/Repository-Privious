using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类ProjectTask_Employee 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_Employee
    {
        #region Instance
        public static readonly ProjectTask_Employee Instance = new ProjectTask_Employee();
        #endregion

        #region Contructor
        protected ProjectTask_Employee()
        { }
        #endregion


        public DataTable GetProjectTask_Employee(QueryProjectTask_Employee query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_Employee.Instance.GetProjectTask_Employee(query, order, currentPage, pageSize, out totalCount);
        }
        public DataTable GetProjectTask_Employee(string TaskID)
        {
            return Dal.ProjectTask_Employee.Instance.GetProjectTask_Employee(TaskID);
        }



        public void DeleteByIDs(string taskIDStr)
        {
            Dal.ProjectTask_Employee.Instance.DeleteByIDs(taskIDStr);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(Entities.ProjectTask_Employee model)
        {
            return Dal.ProjectTask_Employee.Instance.Add(model);
        }

        /// 清空已分配的人员
        /// <summary>
        /// 清空已分配的人员
        /// </summary>
        /// <param name="MinOtherTaskID"></param>
        /// <param name="MaxOtherTaskID"></param>
        /// <param name="assi_total"></param>
        public void ClearProjectTaskEmployee(string minID, string maxID, int top)
        {
            Dal.ProjectTask_Employee.Instance.ClearProjectTaskEmployee(minID, maxID, top);
        }
        /// 清空已分配的人员
        /// <summary>
        /// 清空已分配的人员
        /// </summary>
        /// <param name="MinOtherTaskID"></param>
        /// <param name="MaxOtherTaskID"></param>
        /// <param name="assi_total"></param>
        public void ClearProjectTaskEmployee(string ptids)
        {
            Dal.ProjectTask_Employee.Instance.ClearProjectTaskEmployee(ptids);
        }
    }
}

