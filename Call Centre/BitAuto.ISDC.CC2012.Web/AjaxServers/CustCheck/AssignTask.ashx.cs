using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck
{
    /// <summary>
    /// AssignTask 的摘要说明
    /// </summary>
    public class AssignTask : IHttpHandler, IRequiresSessionState
    {

        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        private string TaskIDS
        {
            get
            {
                return HttpContext.Current.Request["TaskIDS"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TaskIDS"].ToString());
            }
        }

        private string AssignUserID
        {
            get
            {
                return HttpContext.Current.Request["AssignUserID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AssignUserID"].ToString());
            }
        }

        public int userId = 0;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";

            CheckPar(out msg);

            if (msg == "")
            {
                userId = BLL.Util.GetLoginUserID();

                switch (Action)
                {
                    case "AssignTask":
                        AssignTaskByUseid(out msg);
                        break;
                    case "RecedeTask":
                        RecedeTask(out msg);
                        break;
                    case "StopTask":
                        StopTask(out msg);
                        break;

                }
            }

            if (msg == "")
            {
                msg = "success";
            }

            context.Response.Write(msg);
        }

        private void StopTask(out string msg)
        {
            msg = "";
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024BUT150104"))  //添加“任务列表--其它任务”结束验证逻辑
            {
                msg += "没有结束权限";
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
                msg += "请选择任务";
                return;
            }
            #endregion

            #region 判断任务状态

            //判断任务状态
            DataTable taskDt = BLL.ProjectTaskInfo.Instance.GetTaskInfoListByIDs(taskIDStr);
            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    if (dr["TaskStatus"].ToString() == "180014" || dr["TaskStatus"].ToString() == "180015" || dr["TaskStatus"].ToString() == "180016")
                    {
                        msg += "所选任务中，存在不允许结束的任务";
                        break;
                    }
                }
                if (msg != "")
                {
                    return;
                }
            }
            else
            {
                msg += "没找到对应任务";
            }

            List<Entities.CustLastOperTask> list_OperTask = new List<Entities.CustLastOperTask>();
            List<Entities.CustLastOperTask> list_OldOperTask = new List<Entities.CustLastOperTask>();

            DateTime operTime = DateTime.Now;

            for (int p = 0; p < taskDt.Rows.Count; p++)
            {
                if (taskDt.Rows[p]["CRMCustID"].ToString() != "")
                {
                    Entities.CustLastOperTask operTaskModel = new CustLastOperTask();
                    Entities.CustLastOperTask operOldTaskModel = null;
                    operOldTaskModel = BLL.CustLastOperTask.Instance.GetCustLastOperTask(taskDt.Rows[p]["CRMCustID"].ToString());
                    if (operOldTaskModel != null)
                    {
                        list_OldOperTask.Add(operOldTaskModel);

                        operTaskModel.CustID = taskDt.Rows[p]["CRMCustID"].ToString();
                        operTaskModel.TaskID = taskDt.Rows[p]["PTID"].ToString();
                        operTaskModel.TaskType = 1;
                        operTaskModel.LastOperTime = operTime;
                        operTaskModel.LastOperUserID = BLL.Util.GetLoginUserID();

                        list_OperTask.Add(operTaskModel);

                    }
                }
            }
            #endregion

            #region 改变状态
            foreach (string item in tidList)
            {
                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(item, EnumProjectTaskStatus.STOPTask, EnumProjectTaskOperationStatus.TaskFinish, operTime);
            }
            #endregion
        }

        /// <summary>
        /// 回收任务
        /// </summary>
        /// <param name="msg"></param>
        private void RecedeTask(out string msg)
        {
            msg = "";

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
                msg += "请选择任务";
                return;
            }
            #endregion

            #region 判断任务状态

            //判断任务状态
            DataTable taskDt = BLL.ProjectTaskInfo.Instance.GetTaskInfoListByIDs(taskIDStr);
            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    if (dr["TaskStatus"].ToString() != "180001" && dr["TaskStatus"].ToString() != "180000")
                    {
                        msg += "所选任务中，存在不允许收回的任务";
                        break;
                    }
                }
                if (msg != "")
                {
                    return;
                }
            }
            else
            {
                msg += "没找到对应任务";
            }

            List<Entities.CustLastOperTask> list_OperTask = new List<Entities.CustLastOperTask>();
            List<Entities.CustLastOperTask> list_OldOperTask = new List<Entities.CustLastOperTask>();

            DateTime operTime = DateTime.Now;

            for (int p = 0; p < taskDt.Rows.Count; p++)
            {
                if (taskDt.Rows[p]["CRMCustID"].ToString() != "")
                {
                    Entities.CustLastOperTask operTaskModel = new CustLastOperTask();
                    Entities.CustLastOperTask operOldTaskModel = null;
                    operOldTaskModel = BLL.CustLastOperTask.Instance.GetCustLastOperTask(taskDt.Rows[p]["CRMCustID"].ToString());
                    if (operOldTaskModel != null)
                    {
                        list_OldOperTask.Add(operOldTaskModel);

                        operTaskModel.CustID = taskDt.Rows[p]["CRMCustID"].ToString();
                        operTaskModel.TaskID = taskDt.Rows[p]["PTID"].ToString();
                        operTaskModel.TaskType = 1;
                        operTaskModel.LastOperTime = operTime;
                        operTaskModel.LastOperUserID = BLL.Util.GetLoginUserID();

                        list_OperTask.Add(operTaskModel);

                    }
                }
            }
            #endregion

            #region 删除

            BLL.ProjectTask_Employee.Instance.DeleteByIDs(taskIDStr);

            #endregion

            #region 改状态
            foreach (string item in tidList)
            {
                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(item, EnumProjectTaskStatus.NoSelEmployee, EnumProjectTaskOperationStatus.TaskBack, operTime);
            }
            #endregion
        }

        private void CheckPar(out string msg)
        {
            msg = "";

            if (Action == "")
            {
                msg += "参数不正确";
            }
            if (TaskIDS == "")
            {
                msg += "请选择任务";
            }

            if (Action == "AssignTask")
            {
                if (AssignUserID == "")
                {
                    msg += "用户ID参数不正确";
                }
            }
        }

        private void AssignTaskByUseid(out string msg)
        {
            msg = "";
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024BUT150102"))  //添加“任务列表--其它任务”回收验证逻辑
            {
                msg += "没有回收权限";
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
                msg += "请选择任务";
                return;
            }
            #endregion

            #region 判断任务状态

            //判断任务状态
            DataTable taskDt = BLL.ProjectTaskInfo.Instance.GetTaskInfoListByIDs(taskIDStr);
            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    if (dr["TaskStatus"].ToString() != "180012" && dr["TaskStatus"].ToString() != "180000" && dr["TaskStatus"].ToString() != "180001")
                    {
                        msg += "所选任务中，存在不允许分配的任务";
                        break;
                    }
                }
                if (msg != "")
                {
                    return;
                }
            }
            else
            {
                msg += "没找到对应任务";
            }


            List<Entities.CustLastOperTask> list_OperTask = new List<Entities.CustLastOperTask>();
            List<Entities.CustLastOperTask> list_OldOperTask = new List<Entities.CustLastOperTask>();

            DateTime operTime = DateTime.Now;

            for (int p = 0; p < taskDt.Rows.Count; p++)
            {
                if (taskDt.Rows[p]["CRMCustID"].ToString() != "")
                {
                    Entities.CustLastOperTask operTaskModel = new CustLastOperTask();
                    Entities.CustLastOperTask operOldTaskModel = null;
                    operOldTaskModel = BLL.CustLastOperTask.Instance.GetCustLastOperTask(taskDt.Rows[p]["CRMCustID"].ToString());
                    if (operOldTaskModel != null)
                    {
                        list_OldOperTask.Add(operOldTaskModel);

                        operTaskModel.CustID = taskDt.Rows[p]["CRMCustID"].ToString();
                        operTaskModel.TaskID = taskDt.Rows[p]["PTID"].ToString();
                        operTaskModel.TaskType = 1;
                        operTaskModel.LastOperTime = operTime;
                        operTaskModel.LastOperUserID = BLL.Util.GetLoginUserID();

                        list_OperTask.Add(operTaskModel);

                    }
                }
            }
            #endregion



            #region MyRegion

            Entities.ProjectTask_Employee model = null;
            List<Entities.ProjectTask_Employee> list = new List<Entities.ProjectTask_Employee>();

            foreach (string str in tidList)
            {
                model = new Entities.ProjectTask_Employee();

                model.PTID = str;
                model.UserID = int.Parse(AssignUserID);
                model.Status = 0;
                model.CreateTime = operTime;
                model.CreateUserID = userId;

                list.Add(model);
            }

            #endregion

            #region 删除原分配信息，插入新分配信息

            BLL.ProjectTask_Employee.Instance.DeleteByIDs(taskIDStr);

            foreach (Entities.ProjectTask_Employee item in list)
            {
                BLL.ProjectTask_Employee.Instance.Add(item);
            }

            //BLL.ProjectTask_Employee.Instance.UpdateStatus(taskIDStr, "180000");

            foreach (string item in tidList)
            {
                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(item, EnumProjectTaskStatus.NoAssign, EnumProjectTaskOperationStatus.TaskAllot, operTime);
            }
            #endregion
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}