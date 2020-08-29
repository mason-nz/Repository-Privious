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
    /// 业务逻辑类ProjectTaskInfo 的摘要说明。
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
    public class ProjectTaskInfo
    {
        #region Instance
        public static readonly ProjectTaskInfo Instance = new ProjectTaskInfo();
        #endregion

        #region Contructor
        protected ProjectTaskInfo()
        { }
        #endregion

        /// <summary>
        /// 批量为员工分配任务（单选）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool InsertProjectTaskInfo_Employee(Entities.ProjectTaskInfo model, int userId)
        {
            return Dal.ProjectTaskInfo.Instance.InsertProjectTaskInfo_Employee(model, userId);
        }

        public bool BatchInsertProjectTaskInfo_Employee(QueryExcelCustInfo info, int userId)
        {
            return Dal.ProjectTaskInfo.Instance.BatchInsertProjectTaskInfo_Employee(info, userId);
        }

        public bool CrmBatchInsertProjectTaskInfo_Employee(Entities.QueryCrmCustInfo query, int userId, int batch)
        {
            return Dal.ProjectTaskInfo.Instance.CrmBatchInsertProjectTaskInfo_Employee(query, userId, batch);
        }
        public bool TaskIsAssigned(int source, string relationID, int batch)
        {
            return Dal.ProjectTaskInfo.Instance.SelectTasksBySourceAndRelationID(source, relationID, batch);
        }

        /// <summary>
        /// 按照任务ID查询符合条件的一条记录
        /// </summary>
        /// <param name="tid">索引ID</param>
        public Entities.ProjectTaskInfo GetProjectTaskInfo(string tid)
        {
            return Dal.ProjectTaskInfo.Instance.GetProjectTaskInfo(tid);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryProjectTaskInfo"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetProjectTaskInfo(QueryProjectTaskInfo queryProjectTaskInfo, int currentPage, int pageSize, out int totalCount, int userID)
        {
            return Dal.ProjectTaskInfo.Instance.GetProjectTaskInfo(queryProjectTaskInfo, currentPage, pageSize, out totalCount, userID);
        }

        public DataTable GetProjectTaskInfoForTaskRecord(QueryProjectTaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTaskInfo.Instance.GetProjectTaskInfoForTaskRecord(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 更新任务状态
        /// </summary>
        public void UpdateTaskStatus(string tId, Entities.EnumProjectTaskStatus taskStatus, Entities.EnumProjectTaskOperationStatus operationStatus, string description, DateTime dtime)
        {
            Dal.ProjectTaskInfo.Instance.UpdateTaskStatus(tId, taskStatus, operationStatus, description, Util.GetLoginUserID(), dtime);
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        public void UpdateTaskStatus(string tId, Entities.EnumProjectTaskStatus taskStatus, Entities.EnumProjectTaskOperationStatus operationStatus, DateTime dtime)
        {
            this.UpdateTaskStatus(tId, taskStatus, operationStatus, "", dtime);
        }

        /// <summary>
        /// 更新状态到未处理（任务回收）
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="description"></param>
        public void UpdateTaskStatusToNoAssign(string tid)
        {
            Dal.ProjectTask_Employee.Instance.DeleteProjectTask_Employee(tid);
            Dal.ProjectTaskInfo.Instance.UpdateTaskStatusToNull(tid);
            DeleteRelationInfoByTID(tid);
        }

        /// <summary>
        /// 删除关联信息（客户、客户联系人、客户品牌、会员、会员品牌）
        /// </summary>
        /// <param name="tid"></param>
        public void DeleteRelationInfoByTID(string tid)
        {
            BLL.ProjectTask_Cust_Contact.Instance.DeleteContactByTID(tid);//删除客户联系人
            BLL.ProjectTask_Cust_Brand.Instance.DeleteByTID(tid);//删除客户品牌
            BLL.ProjectTask_DMSMember.Instance.DeleteByTID(tid);//删除车易通信息

            BLL.ProjectTask_CSTMember.Instance.DeleteByTID(tid);//删除车商通会员信息
            BLL.ProjectTask_Cust.Instance.DeleteByTID(tid);//删除客户信息
        }
        /// <summary>
        /// 更新任务附加状态
        /// </summary>
        public void InsertOrUpdateTaskAdditionalStatus(string tId, string status, string description)
        {
            Dal.ProjectTaskInfo.Instance.InsertOrUpdateTaskAdditionalStatus(tId, status, description);
        }

        /// <summary>
        /// 得到任务附加状态
        /// </summary>
        public void GetTaskAdditionalStatus(string tId, out string status, out string description)
        {
            Dal.ProjectTaskInfo.Instance.GetTaskAdditionalStatus(tId, out status, out   description);
        }

        public void UpdateCrmCustID(string tId, string crmCustID)
        {
            Dal.ProjectTaskInfo.Instance.UpdateCrmCustID(tId, crmCustID);
        }

        /// <summary>
        /// 统计新增客户任务状态
        /// </summary>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public void StatProjectTaskInfo(int source, Entities.QueryExcelCustInfo query, out int allTaskCount, out int noManageCount, out int managingCount, out int manageFinshedCount)
        {
            Dal.ProjectTaskInfo.Instance.StatProjectTaskInfo(source, query, out allTaskCount, out noManageCount, out managingCount, out manageFinshedCount);
        }

        /// <summary>
        /// 统计crm客户任务状态
        /// </summary>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public void StatProjectTaskInfo(int source, Entities.QueryCrmCustInfo query, out int allTaskCount, out int noManageCount, out int managingCount, out int manageFinshedCount)
        {
            Dal.ProjectTaskInfo.Instance.StatProjectTaskInfo(source, query, out allTaskCount, out noManageCount, out managingCount, out manageFinshedCount);
        }

        public string[] StatCC_UserMappingTasks(Entities.QueryCrmCustInfo query)
        {
            return Dal.ProjectTaskInfo.Instance.StatCC_UserMappingTasks(query);
        }

        public int StatProjectTaskInfoByCustID(string custId, int source)
        {
            return Dal.ProjectTaskInfo.Instance.StatProjectTaskInfoByCustID(custId, source);
        }

        /// <summary>
        /// 任务是否属于这个人
        /// </summary>
        public bool TaskBelongToUser(string tID, int userID)
        {
            return Dal.ProjectTaskInfo.Instance.TaskBelongToUser(tID, userID);
        }

        /// <summary>
        /// 获取批次列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetBatchList()
        {
            return Dal.ProjectTaskInfo.Instance.GetBatchList();
        }

        public void Add(SqlTransaction sqltran, Entities.ProjectTaskInfo model)
        {
            Dal.ProjectTaskInfo.Instance.Add(sqltran, model);
        }

        /// <summary>
        /// 根据某个表获取总数量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public int GetMax()
        {
            return Dal.ProjectTaskInfo.Instance.GetMax();
        }

        public DataTable getCreateUser()
        {
            return Dal.ProjectTaskInfo.Instance.getCreateUser();
        }

        public DataTable getOpterUser()
        {
            return Dal.ProjectTaskInfo.Instance.getOpterUser();
        }

        public DataTable getEmpleeUser()
        {
            return Dal.ProjectTaskInfo.Instance.getEmpleeUser();
        }

        public DataTable GetTaskInfoListByIDs(string TaskIDS)
        {
            return Dal.ProjectTaskInfo.Instance.GetTaskInfoListByIDs(TaskIDS);
        }

        /// <summary>
        /// 导出的任务
        /// </summary>
        /// <param name="taskIDStr"></param>
        /// <returns></returns>
        public DataTable GetExportTaskList(string taskIDStr)
        {
            return Dal.ProjectTaskInfo.Instance.GetExportTaskList(taskIDStr);
        }


        public Entities.QueryProjectTaskInfo GetQuery(string RequestName, string RequestCustName, string RequestStatuss,
             string AdditionalStatus, string RequestGroup, string RequestCategory,
            string RequestCreater, string RequestUserId, string RequestBeginTime, string RequestEndTime, string RequestOptUserId,
            string CRMBrandIDs, string NoCRMBrand, string TaskID, string CustID, string RequestOperationStatus, string CustType, string LastOperStartTime, string LastOperEndTime
            )
        {
            Entities.QueryProjectTaskInfo query = new Entities.QueryProjectTaskInfo();

            #region MyRegion

            if (RequestName != "")
            {
                query.ProjectName = RequestName;
            }
            if (RequestCustName != "")
            {
                query.CustName = RequestCustName;
            }
            if (CustID != "")
            {
                query.CRMCustID = CustID;
            }

            ///状态列表
            if (RequestStatuss != "" && RequestStatuss != "-1")
            {
                if (RequestStatuss == "180099")
                {
                    query.TaskStatus_s = "180003,180004,180010";
                }
                else
                {
                    query.TaskStatus_s = RequestStatuss;
                }

            }
            if (RequestOperationStatus != "")
            {
                query.OperationStatus_s = RequestOperationStatus;
            }

            if (AdditionalStatus != "")
            {
                query.AdditionalStatus = AdditionalStatus;
            }

            if (RequestGroup != "")
            {
                query.BGID = RequestGroup;
            }
            if (RequestCategory != "")
            {
                query.PCatageID = RequestCategory;
            }
            if (RequestCreater != "")
            {
                query.CreateUserID = int.Parse(RequestCreater);
            }
            if (RequestUserId != "")
            {
                query.EmployeeID = int.Parse(RequestUserId);
            }
            if (RequestOptUserId != "")
            {
                query.LastOptUserID = int.Parse(RequestOptUserId);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = RequestBeginTime;
            }
            if (RequestEndTime != "")
            {
                query.EndTime = RequestEndTime;
            }
            if (CRMBrandIDs != "")
            {
                query.CRMBrandIDs = CRMBrandIDs;
            }
            if (NoCRMBrand != "")
            {
                query.NoCRMBrand = NoCRMBrand;
            }
            if (TaskID != "")
            {
                query.PTID = TaskID;
            }
            if (CustType != "" && CustType != "-1")
            {
                query.CustType = CustType;
            }
            if (LastOperStartTime != "")
            {
                query.LastOperStartTime = LastOperStartTime;
            }
            if (LastOperEndTime != "")
            {
                query.LastOperEndTime = LastOperEndTime;
            }
            #endregion
            return query;
        }

        /// <summary>
        /// type,1是核实，2是其他任务,取任务所对项目信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable GetProjectInfoByTaskID(string TaskID, string Type)
        {
            return Dal.ProjectTaskInfo.Instance.GetProjectInfoByTaskID(TaskID, Type);

        }

        public int GetMaxRecID()
        {
            return Dal.ProjectTaskInfo.Instance.GetMaxRecID();
        }
    }
}

