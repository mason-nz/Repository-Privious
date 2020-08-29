using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Web.WebServices;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public class Oper
    {
        public void AssignTaskByUseid(string TaskIDS, string AssignUserID, out string msg)
        {
            msg = "result:'false'";
            string error = string.Empty;

            if (TaskIDS == string.Empty)
            {
                msg = "result:'false',error:'任务ID串不能为空'";
                return;
            }

            #region ID列表串
            string taskIDStr = "";
            string[] tidList = TaskIDS.Split(',');
            foreach (string item in tidList)
            {
                taskIDStr += "'" + item + "',";
            }
            if (taskIDStr != "")
            {
                taskIDStr = taskIDStr.Substring(0, taskIDStr.Length - 1);
            }
            else
            {
                error += "请选择任务";
                return;
            }
            #endregion

            #region 判断任务状态

            //判断任务状态
            DataTable taskDt = BLL.OrderCRMStopCustTask.Instance.GetListByTaskIDs(taskIDStr);

            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    //所能分配的任务为：1-未分配；未处理-2；处理中-3
                    if (dr["TaskStatus"].ToString() != "1" && dr["TaskStatus"].ToString() != "2" && dr["TaskStatus"].ToString() != "3")
                    {
                        error += "所选任务中，存在不允许分配的任务";
                        break;
                    }
                }
            }
            else
            {
                error += "没找到对应任务";
            }

            if (error != "")
            {
                msg = "result:'false',error:'" + error + "'";
                return;
            }
            #endregion

            DateTime dtime = DateTime.Now;
            foreach (string tid in tidList)
            {
                Entities.OrderCRMStopCustTaskInfo model = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(tid);
                //分配任务，待处理
                model.TaskStatus = (int)Entities.StopCustTaskStatus.NoProcess;
                model.AssignUserID = int.Parse(AssignUserID);
                model.AssignTime = dtime;
                model.Status = 0;
                BLL.OrderCRMStopCustTask.Instance.Update(model);

                InsertOperationLog(tid, (int)Entities.StopCustTaskOperStatus.Allocation, (int)Entities.StopCustTaskStatus.NoProcess,
                    "分配坐席：" + BLL.EmployeeAgent.Instance.GetAgentNumberAndUserNameByUserId(model.AssignUserID.Value));
            }
            msg = "result:'true'";
        }
        public void RecedeTask(string TaskIDS, string AssignUserID, out string msg)
        {
            msg = "result:'false'";
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT130402"))  //增加“任务列表--客户核实”回收功能验证逻辑
            {
                msg = "result:'false',error:'没有回收权限'";
                return;
            }
            string error = string.Empty;

            if (TaskIDS == string.Empty)
            {
                msg = "result:'false',error:'任务ID串不能为空'";
                return;
            }

            #region ID列表串

            string taskIDStr = "";

            string[] tidList = TaskIDS.Split(',');

            foreach (string item in tidList)
            {
                taskIDStr += "'" + item + "',";
            }
            if (taskIDStr != "")
            {
                taskIDStr = taskIDStr.Substring(0, taskIDStr.Length - 1);
            }
            else
            {
                error += "请选择任务";
                return;
            }

            #endregion

            #region 判断任务状态

            //判断任务状态
            DataTable taskDt = BLL.OrderCRMStopCustTask.Instance.GetListByTaskIDs(taskIDStr);

            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    //所能回收的任务为： 未处理-2；处理中-3
                    if (dr["TaskStatus"].ToString() != "2" && dr["TaskStatus"].ToString() != "3")
                    {
                        error += "所选任务中，存在不允许回收的任务";
                        break;
                    }
                }
            }
            else
            {
                error += "没找到对应任务";
            }

            if (error != "")
            {
                msg = "result:'false',error:'" + error + "'";
                return;
            }
            #endregion

            DateTime dtime = DateTime.Now;
            foreach (string tid in tidList)
            {
                Entities.OrderCRMStopCustTaskInfo model = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(tid);
                //回收任务，未分配
                int oldAssignUserID = model.AssignUserID.GetValueOrDefault(-1);
                model.TaskStatus = (int)Entities.StopCustTaskStatus.NoAllocation;
                model.AssignUserID = -2;
                model.AssignTime = dtime;
                model.Status = 0;
                BLL.OrderCRMStopCustTask.Instance.Update(model);

                InsertOperationLog(tid, (int)Entities.StopCustTaskOperStatus.Recover, (int)model.TaskStatus, "回收坐席：" + BLL.EmployeeAgent.Instance.GetAgentNumberAndUserNameByUserId(oldAssignUserID));
            }
            msg = "result:'true'";
        }
        private void InsertOperationLog(string taskid, int operStatus, int taskStatus, string remark)
        {
            Entities.OrderCRMStopCustTaskOperationLogInfo model_Log = new Entities.OrderCRMStopCustTaskOperationLogInfo();
            model_Log.TaskID = taskid;
            model_Log.OperationStatus = operStatus;
            model_Log.TaskStatus = taskStatus;
            model_Log.Remark = remark;
            model_Log.CreateTime = DateTime.Now;
            model_Log.CreateUserID = BLL.Util.GetLoginUserID();
            BLL.OrderCRMStopCustTaskOperationLog.Instance.Insert(model_Log);
        }
    }
}