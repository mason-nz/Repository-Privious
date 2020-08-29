using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.WebServices;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public class OperApproval
    {
        //type=1:审批通过；type=2:驳回
        public void OperDeal(int CRMStopCustApplyID, string taskID, int type, string reason, out string msg)
        {
            msg = "result:'false'";

            Entities.OrderCRMStopCustTaskInfo model_task = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(taskID);
            if (model_task == null)
            {
                msg = "result:'false',msg:'未找到该任务'";
                return;
            }

            if (model_task.AssignUserID_Value != BLL.Util.GetLoginUserID())
            {
                msg = "result:'false',msg:'该任务不属于您！'";
                return;
            }

            bool result = type == 1 ? BitAuto.YanFa.Crm2009.BLL.CustApply.Instance.ApprovePass(CRMStopCustApplyID, reason) :
                BitAuto.YanFa.Crm2009.BLL.CustApply.Instance.ApproveRefuse(CRMStopCustApplyID, reason);

            BLL.Util.InsertUserLog("CRM客户停用任务ID=" + taskID + " 审核操作：" + (type == 1 ? "审核通过时" : "驳回时") + "，调用CRM接口，结果：" + (result == false ? "失败" : "成功"));

            if (result)
            {
                //审核，已处理
                string relationID = UpdateTask(taskID);

                if (relationID != string.Empty)
                {
                    int intRelationID;
                    if (int.TryParse(relationID, out intRelationID))
                    {
                        UpdateStopCustApply(type, intRelationID, reason, taskID.ToString());
                        //审核，已处理
                        InsertOperationLog(type, (int)Entities.StopCustTaskStatus.Sumbit, taskID, reason);
                        msg = "result:'true'";
                    }
                    else
                    {
                        msg = "result:'false',msg:'RelationID不能转换为int类型，导致更新客户停用状态不能执行'";
                    }
                }

            }
            else
            {
                msg = "result:'false',msg:'接口返回失败'";
            }
        }

        private string UpdateTask(string taskID)
        {
            string relationID = string.Empty;
            Entities.OrderCRMStopCustTaskInfo model_task = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(taskID);
            if (model_task != null)
            {
                //审核，已处理
                model_task.TaskStatus = (int)Entities.StopCustTaskStatus.Sumbit;//已提交
                model_task.SubmitTime = DateTime.Now;
                BLL.OrderCRMStopCustTask.Instance.Update(model_task);
                relationID = model_task.RelationID.ToString();
            }
            return relationID;
        }

        //type=1:审批通过；type=2:驳回
        private void UpdateStopCustApply(int type, int RecID, string Reson, string taskID)
        {
            Entities.StopCustApply model = BLL.StopCustApply.Instance.GetStopCustApply(RecID);
            Entities.StopCustApply oldModel = BLL.StopCustApply.Instance.GetStopCustApply(RecID);
            DateTime dtime = DateTime.Now;
            model.AuditTime = dtime;
            if (type == 1)
            {
                //审批通过，待停用或待启用
                model.StopStatus = (int)Entities.StopCustStopStatus.NoDisabled;
                //审核意见
                model.AuditOpinion = Reson;
            }
            else
            {
                //驳回
                model.StopStatus = (int)Entities.StopCustStopStatus.Reject;
                //驳回理由
                model.RejectReason = Reson;
            }
            BLL.StopCustApply.Instance.Update(model);
            Log.InsertLogStopCustApply(oldModel, model, BLL.Util.GetLoginUserID());
        }


        private void InsertOperationLog(int type, int taskStatus, string taskID, string reason)
        {
            //记录任务生成日志
            Entities.OrderCRMStopCustTaskOperationLogInfo model_taskLog = new Entities.OrderCRMStopCustTaskOperationLogInfo();
            model_taskLog.TaskID = taskID;
            model_taskLog.OperationStatus = (int)Entities.StopCustTaskOperStatus.Finished;
            model_taskLog.TaskStatus = taskStatus;
            model_taskLog.CreateTime = DateTime.Now;
            model_taskLog.CreateUserID = BLL.Util.GetLoginUserID();
            string desc = type == 1 ? "审核意见：" : "驳回理由：";
            model_taskLog.Remark = desc + reason;
            BLL.OrderCRMStopCustTaskOperationLog.Instance.Insert(model_taskLog);
        }
    }
}