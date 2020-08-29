using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        public string Action
        {
            get
            {
                if (HttpContext.Current.Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string TaskID
        {
            get
            {
                if (HttpContext.Current.Request["TaskID"] != null)
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string Reason
        {
            get
            {
                if (HttpContext.Current.Request["Reason"] != null)
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request["Reason"].ToString());
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
            string errorMsg = string.Empty;

            switch (Action)
            {
                case "DeleteTask": DeleteTask(out errorMsg);
                    break;
            }

            if (errorMsg != string.Empty)
            {
                msg = "result:'false',msg:'" + errorMsg + "操作失败'";
            }
            else
            {
                msg = "result:'true'";
            }

            context.Response.Write("{" + msg + "}");
        }

        private void DeleteTask(out string errorMsg)
        {
            errorMsg = string.Empty;

            Validate(out errorMsg);

            if (errorMsg != string.Empty)
            {
                return;
            }

            long taskID = long.Parse(TaskID);

            Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(taskID);
            BLL.OrderTask.Instance.Delete(taskID);

            Entities.OrderTaskOperationLog oLog = new Entities.OrderTaskOperationLog();
            oLog.TaskID = taskID;
            oLog.OperationStatus = (int)Entities.OperationStatus.Delete;
            oLog.TaskStatus = model.TaskStatus;
            oLog.Remark = Reason;
            oLog.CreateTime = DateTime.Now;
            oLog.CreateUserID = BLL.Util.GetLoginUserID();
            BLL.OrderTaskOperationLog.Instance.Insert(oLog);

            BLL.Util.InsertUserLog("【无主订单】任务ＩＤ为【" + TaskID + "】被删除！删除原因：" + Reason);
        }

        private void Validate(out string errorMsg)
        {
            errorMsg = string.Empty;

            if (Reason.Trim() == string.Empty)
            {
                errorMsg = "删除原因不能为空！";
                return;
            }

            long taskID;
            if (!long.TryParse(TaskID, out taskID))
            {
                errorMsg = "任务ID有错误！";
                return;
            }

            Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(taskID);

            if (model == null)
            {
                errorMsg = "该任务不存在！";
                return;
            }

            if (model.Status == -1)
            {
                errorMsg = "该任务已被删除！";
                return;
            }

            if (model.TaskStatus == (int)Entities.TaskStatus.Processed)
            {
                errorMsg = "该任务已提交，不可被删除！";
                return;
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