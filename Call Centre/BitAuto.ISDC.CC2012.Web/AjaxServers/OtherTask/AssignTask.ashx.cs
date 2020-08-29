using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Collections;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask
{
    /// <summary>
    /// AssignTask 的摘要说明
    /// </summary>
    public class AssignTask : IHttpHandler, IRequiresSessionState
    {
        #region 属性
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


        #endregion
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

        /// <summary>
        /// 回收任务
        /// </summary>
        /// <param name="msg"></param>
        private void RecedeTask(out string msg)
        {
            msg = "";
            //增加回收权限控制
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT150102"))
            {
                msg += "您没有收回操作权限";
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
            DataTable taskDt = BLL.OtherTaskInfo.Instance.GetTaskInfoListByIDs(taskIDStr);
            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    //可回收的任务：任务状态为2-未处理、3-处理中
                    if (dr["TaskStatus"].ToString() != "2" && dr["TaskStatus"].ToString() != "3")
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
            #endregion

            #region 删除

            BLL.ProjectTask_Employee.Instance.DeleteByIDs(taskIDStr);

            #endregion

            #region 改状态

            foreach (string item in tidList)
            {
                BLL.OtherTaskInfo.Instance.UpdateTaskStatus(item, Entities.OtheTaskStatus.Unallocated, Entities.EnumProjectTaskOperationStatus.TaskBack, "回收", userId);
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

        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="msg"></param>
        private void AssignTaskByUseid(out string msg)
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
            DataTable taskDt = BLL.OtherTaskInfo.Instance.GetTaskInfoListByIDs(taskIDStr);
            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    int isAutoCall = CommonFunction.ObjectToInteger(dr["isAutoCall"].ToString());
                    int projectacstatus = CommonFunction.ObjectToInteger(dr["ProjectACStatus"].ToString());
                    int taskstatus = CommonFunction.ObjectToInteger(dr["TaskStatus"].ToString());
                    //所能分配的任务为：1-未分配；未处理-2；处理中-3
                    if (taskstatus != 1 && taskstatus != 2 && taskstatus != 3)
                    {
                        msg += "所选任务中，存在不允许分配的任务";
                        break;
                    }
                    else if (isAutoCall == 1 && projectacstatus == (int)ProjectACStatus.P01_进行中)
                    {
                        msg += "所选任务中，存在正在进行中的自动外呼任务，无法分配任务";
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
                model.CreateTime = DateTime.Now;
                model.CreateUserID = userId;

                list.Add(model);
            }

            #endregion

            #region 删除原分配信息，插入新分配信息

            BLL.ProjectTask_Employee.Instance.DeleteByIDs(taskIDStr);

            foreach (Entities.ProjectTask_Employee item in list)
            {
                BLL.ProjectTask_Employee.Instance.Add(item);
                insertLog(null, item);
            }

            foreach (string item in tidList)
            {
                BLL.OtherTaskInfo.Instance.UpdateTaskStatus(item, Entities.OtheTaskStatus.Untreated, Entities.EnumProjectTaskOperationStatus.TaskAllot, "分配", userId);
            }

            #endregion
        }

        private void StopTask(out string msg)
        {
            msg = "";
            //增加结束权限控制
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT150104"))
            {
                msg += "您没有结束操作权限";
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
            DataTable taskDt = BLL.OtherTaskInfo.Instance.GetTaskInfoListByIDs(taskIDStr);
            if (taskDt != null)
            {
                foreach (DataRow dr in taskDt.Rows)
                {
                    if (dr["TaskStatus"].ToString() == "4" || dr["TaskStatus"].ToString() == "5")
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
            #endregion

            #region 改状态

            // BLL.ProjectTask_Employee.Instance.UpdateStatus(taskIDStr, "180016");

            foreach (string item in tidList)
            {
                BLL.OtherTaskInfo.Instance.UpdateTaskStatus(item, Entities.OtheTaskStatus.StopTask, Entities.EnumProjectTaskOperationStatus.TaskFinish, "结束", userId);
            }

            #endregion

        }

        //分配 插入日志 lxw 2013-6-5
        private void insertLog(Entities.ProjectTask_Employee oldModel, Entities.ProjectTask_Employee newModel)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("PTID", "任务ID");
            ht_FieldName.Add("UserID", "操作人");
            ht_FieldName.Add("Status", "状态");
            ht_FieldName.Add("CreateTime", "时间");
            ht_FieldName.Add("CreateUserID", "操作人");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            ht_FieldType.Add("UserID", "UserID");
            ht_FieldType.Add("CreateUserID", "UserID");

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            if (oldModel == null)//为空，则是新增
            {
                //插入日志
                BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

                logStr = "其他任务分配新增：" + userLogStr;
            }
            else //不为空，则是编辑
            {
                //插入日志 
                BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

                logStr = "其他任务分配编辑：" + userLogStr;
            }

            if (userLogStr != string.Empty)
            {
                BLL.Util.InsertUserLog(logStr);
            }
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