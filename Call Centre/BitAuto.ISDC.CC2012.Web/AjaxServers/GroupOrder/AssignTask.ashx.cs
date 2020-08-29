using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder
{
    /// <summary>
    /// Summary description for AssignTask1
    /// </summary>
    public class AssignTask1 : IHttpHandler, IRequiresSessionState
    {

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string TaskIDS
        {
            get
            {
                if (Request["TaskIDS"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskIDS"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string AssignUserID
        {
            get
            {
                if (Request["AssignUserID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["AssignUserID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            //CustBaseInfoHelper helper = new CustBaseInfoHelper();
            //context.Response.ContentType = "text/plain";
            switch (Action.ToLower())
            {
                case "revoketask":
                    try
                    {
                        RevokeTask(out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'" + ex.Message + "'}";
                    }
                    break;
                case "assigntask":
                    try
                    {
                        AssignTask(out msg);
                        //msg = "{Result:'" + "yes" + "',CustID:'',ErrorMsg:''}";
                    }
                    catch (Exception ex)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'" + ex.Message + "'}";
                    }
                    break;
            }
            context.Response.Write(msg);
        }


        /// <summary>
        /// 收回任务
        /// </summary>
        /// <param name="msg"></param>
        public void RevokeTask(out string msg)
        {
            msg = string.Empty;

            string Result = "yes";
            string ErrorMsg = "";
            //不为空
            if (!string.IsNullOrEmpty(TaskIDS))
            {
                if (TaskIDS.IndexOf(',') > 0)
                {
                    for (int i = 0; i < TaskIDS.Split(',').Length; i++)
                    {
                        int userid = 0;
                        Entities.GroupOrderTask Model = BLL.GroupOrderTask.Instance.GetGroupOrderTask(Convert.ToInt32(TaskIDS.Split(',')[i]));

                        //判断任务状态，是否为已处理
                        if (Model.TaskStatus != (int)Entities.GroupTaskStatus.Processed)
                        {
                            //取要被收回任务的处理人
                            if (Model.AssignUserID != null && Model.AssignUserID != -2)
                            {
                                userid = Convert.ToInt32(Model.AssignUserID);
                            }
                            Model.AssignUserID = null;
                            Model.TaskStatus = (int)Entities.GroupTaskStatus.NoAllocation;
                            BLL.GroupOrderTask.Instance.Update(Model);
                            Entities.GroupOrderTaskOperationLog logmodel = new Entities.GroupOrderTaskOperationLog();
                            logmodel.TaskID = Model.TaskID;
                            logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Recover;
                            logmodel.TaskStatus = (int)Entities.GroupTaskStatus.NoAllocation;
                            logmodel.CreateTime = System.DateTime.Now;
                            logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                            BLL.GroupOrderTaskOperationLog.Instance.Insert(logmodel);
                        }
                        else
                        {
                            Result = "no";
                            ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态为已处理，不能回收。";
                        }

                    }
                }
                else
                {
                    int userid = 0;

                    Entities.GroupOrderTask Model = BLL.GroupOrderTask.Instance.GetGroupOrderTask(Convert.ToInt32(TaskIDS));

                    //判断任务状态，是否为已处理
                    if (Model.TaskStatus != (int)Entities.GroupTaskStatus.Processed)
                    {

                        //取要被收回任务的处理人
                        if (Model.AssignUserID != null && Model.AssignUserID != -2)
                        {
                            userid = Convert.ToInt32(Model.AssignUserID);
                        }
                        Model.AssignUserID = null;
                        Model.TaskStatus = (int)Entities.GroupTaskStatus.NoAllocation;
                        BLL.GroupOrderTask.Instance.Update(Model);

                        Entities.GroupOrderTaskOperationLog logmodel = new Entities.GroupOrderTaskOperationLog();
                        logmodel.TaskID = Model.TaskID;
                        logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Recover;
                        logmodel.TaskStatus = (int)Entities.GroupTaskStatus.NoAllocation;
                        logmodel.CreateTime = System.DateTime.Now;
                        logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                        BLL.GroupOrderTaskOperationLog.Instance.Insert(logmodel);
                    }
                    else
                    {
                        Result = "no";
                        ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态为已处理，不能回收。";
                    }
                }
            }
            msg = "{Result:'" + Result + "',CustID:'',ErrorMsg:'" + ErrorMsg + "'}";
        }



        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="msg"></param>
        private void AssignTask(out string msg)
        {
            msg = string.Empty;

            string Result = "yes";
            string ErrorMsg = "";

            //不为空
            if (!string.IsNullOrEmpty(TaskIDS) && !string.IsNullOrEmpty(AssignUserID))
            {
                if (TaskIDS.IndexOf(',') > 0)
                {
                    string[] arrayTaskIDS = TaskIDS.Split(',');
                    foreach (string taskid in arrayTaskIDS)
                    {
                        Entities.GroupOrderTask Model = BLL.GroupOrderTask.Instance.GetGroupOrderTask(Convert.ToInt32(taskid));

                        if (Model.TaskStatus == (int)Entities.GroupTaskStatus.NoAllocation)
                        {

                            Model.AssignUserID = Convert.ToInt32(AssignUserID);
                            Model.AssignTime = System.DateTime.Now;
                            Model.TaskStatus = (int)Entities.GroupTaskStatus.NoProcess;
                            BLL.GroupOrderTask.Instance.Update(Model);

                            //插入任务操作日志
                            Entities.GroupOrderTaskOperationLog logmodel = new Entities.GroupOrderTaskOperationLog();
                            logmodel.TaskID = Model.TaskID;
                            logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Allocation;
                            logmodel.TaskStatus = (int)Entities.GroupTaskStatus.NoProcess;
                            logmodel.CreateTime = System.DateTime.Now;
                            logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                            BLL.GroupOrderTaskOperationLog.Instance.Insert(logmodel);
                        }
                        else
                        {
                            Result = "no";
                            ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态不是待分配。";
                        }
                    }
                }
                else
                {
                    Entities.GroupOrderTask Model = BLL.GroupOrderTask.Instance.GetGroupOrderTask(Convert.ToInt32(TaskIDS));

                    if (Model.TaskStatus == (int)Entities.GroupTaskStatus.NoAllocation)
                    {

                        Model.AssignUserID = Convert.ToInt32(AssignUserID);
                        Model.AssignTime = System.DateTime.Now;
                        Model.TaskStatus = (int)Entities.GroupTaskStatus.NoProcess;
                        BLL.GroupOrderTask.Instance.Update(Model);


                        //插入任务操作日志
                        Entities.GroupOrderTaskOperationLog logmodel = new Entities.GroupOrderTaskOperationLog();
                        logmodel.TaskID = Model.TaskID;
                        logmodel.OperationStatus = (int)Entities.GO_OperationStatus.Allocation;
                        logmodel.TaskStatus = (int)Entities.GroupTaskStatus.NoProcess;
                        logmodel.CreateTime = System.DateTime.Now;
                        logmodel.CreateUserID = BLL.Util.GetLoginUserID();
                        BLL.GroupOrderTaskOperationLog.Instance.Insert(logmodel);
                    }
                    else
                    {
                        Result = "no";
                        ErrorMsg += "任务ID为" + Model.TaskID + "的任务状态不是待分配。";
                    }
                }
            }
            msg = "{Result:'" + Result + "',CustID:'',ErrorMsg:'" + ErrorMsg + "'}";

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