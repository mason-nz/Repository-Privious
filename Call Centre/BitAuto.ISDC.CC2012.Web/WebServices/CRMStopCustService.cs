using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    public static class CRMStopCustWebService
    {
        /// CRM同步插入客户核实表
        /// <summary>
        /// CRM同步插入客户核实表
        /// </summary>
        /// <param name="CRMStopCustApplyID"></param>
        /// <param name="CustID"></param>
        /// <param name="ApplyerID"></param>
        /// <param name="AreaName"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="StopStatus"></param>
        /// <param name="Remark"></param>
        /// <param name="DeleteMemberID"></param>
        /// <param name="ApplyType"></param>
        /// <param name="ApplyReason"></param>
        /// <param name="ApplyRemark"></param>
        /// <returns></returns>
        public static string Insert(int CRMStopCustApplyID, string CustID, int ApplyerID, string AreaName, DateTime ApplyTime, int StopStatus, string Remark, string DeleteMemberID, int ApplyType, int ApplyReason, int ApplyRemark)
        {
            //插入主表
            int recID = InsertStopCustApply(CRMStopCustApplyID, CustID, ApplyerID, AreaName, ApplyTime, StopStatus, Remark, DeleteMemberID, ApplyType, ApplyReason, ApplyRemark);
            //创建任务
            string taskID = InsertTask(recID, ApplyerID, CustID);
            //记录操作日志
            InsertOperationLog(taskID, ApplyerID, Remark, StopCustTaskStatus.NoAllocation, StopCustTaskOperStatus.Sync);
            return taskID;
        }
        /// 插入主表 StopCustApply
        /// <summary>
        /// 插入主表 StopCustApply
        /// </summary>
        /// <param name="CRMStopCustApplyID"></param>
        /// <param name="CustID"></param>
        /// <param name="ApplyerID"></param>
        /// <param name="AreaName"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="StopStatus"></param>
        /// <param name="Remark"></param>
        /// <param name="DeleteMemberID"></param>
        /// <param name="ApplyType"></param>
        /// <param name="ApplyReason"></param>
        /// <param name="ApplyRemark"></param>
        /// <returns></returns>
        private static int InsertStopCustApply(int CRMStopCustApplyID, string CustID, int ApplyerID, string AreaName, DateTime ApplyTime, int StopStatus, string Remark, string DeleteMemberID, int ApplyType, int ApplyReason, int ApplyRemark)
        {
            Entities.StopCustApply model = new Entities.StopCustApply();
            model.CRMStopCustApplyID = CRMStopCustApplyID;
            model.CustID = CustID;
            model.ApplyerID = ApplyerID;
            model.AreaName = AreaName;
            model.ApplyTime = ApplyTime;
            model.StopStatus = StopStatus;
            model.Remark = Remark;
            model.DeleteMemberID = DeleteMemberID;
            model.ApplyType = ApplyType;
            model.ApplyReason = ApplyReason;
            model.ApplyRemark = ApplyRemark;
            int recID = BLL.StopCustApply.Instance.Insert(model);
            //插入日志
            model.RecID = recID;
            return recID;
        }
        /// 创建任务
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="relationID"></param>
        /// <param name="userID"></param>
        /// <param name="custID"></param>
        /// <returns></returns>
        private static string InsertTask(int relationID, int userID, string custID)
        {
            DateTime dtime = DateTime.Now;
            //生成任务
            OrderCRMStopCustTaskInfo model_task = new OrderCRMStopCustTaskInfo();
            model_task.RelationID = relationID;
            model_task.TaskStatus = 1;
            model_task.BGID = 2; //写死分组 数据清洗部（西安）
            model_task.Status = 0;
            model_task.CreateTime = dtime;
            model_task.CreateUserID = userID;
            string taskID = BLL.OrderCRMStopCustTask.Instance.Insert(model_task);
            return taskID;
        }
        /// 记录操作日志
        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="userID"></param>
        /// <param name="info"></param>
        /// <param name="taskStatus"></param>
        /// <param name="operStatus"></param>
        public static void InsertOperationLog(string taskID, int userID, string info, StopCustTaskStatus taskStatus, StopCustTaskOperStatus operStatus)
        {
            //记录任务生成日志
            OrderCRMStopCustTaskOperationLogInfo model_taskLog = new OrderCRMStopCustTaskOperationLogInfo();
            model_taskLog.TaskID = taskID;
            model_taskLog.OperationStatus = (int)operStatus;//同步
            model_taskLog.TaskStatus = (int)taskStatus;
            model_taskLog.CreateTime = DateTime.Now;
            model_taskLog.CreateUserID = userID;
            model_taskLog.Remark = info;
            BLL.OrderCRMStopCustTaskOperationLog.Instance.Insert(model_taskLog);
        }
    }
}