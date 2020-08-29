using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.LeadsTask.AjaxServers
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action { get { return (Request["Action"] + "").Trim(); } }
        public string TaskIDs { get { return (Request["TaskIDs"] + "").Trim(); } }
        private string ProjectName
        {
            get { return HttpContext.Current.Request["ProjectName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProjectName"].ToString()); }
        }
        public string AssignID { get { return (Request["AssignID"] + "").Trim(); } }
        public string UserID { get { return (Request["UserID"] + "").Trim(); } }
        private string RequestBeginDealTime
        {
            get { return HttpContext.Current.Request["BeginDealTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BeginDealTime"].ToString()); }
        }
        private string RequestEndDealTime
        {
            get { return HttpContext.Current.Request["EndDealTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EndDealTime"].ToString()); }
        }
        private string RequestTaskCBeginTime
        {
            get { return HttpContext.Current.Request["TaskCBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskCBeginTime"].ToString()); }
        }
        private string RequestTaskCEndTime
        {
            get { return HttpContext.Current.Request["TaskCEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskCEndTime"].ToString()); }
        }
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }

        private string RequestProvinceID
        {
            get { return HttpContext.Current.Request["ProvinceID"] == null ? "-1" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProvinceID"].ToString()); }
        }
        private string RequestCityID
        {
            get { return HttpContext.Current.Request["CityID"] == null ? "-1" : HttpUtility.UrlDecode(HttpContext.Current.Request["CityID"].ToString()); }
        }
        private string RequestTel
        {
            get { return HttpContext.Current.Request["Tel"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Tel"].ToString()); }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action)
            {
                case "assignTask": AssignTask(out msg);
                    break;
                case "taskIsAssign": TaskIsAssign(out msg);
                    break;
                case "recedeTask": RecedeTask(out msg);
                    break;
                case "getStatusNum": GetStatusNum(out msg);
                    break;
                case "checktel": checktel(out msg);
                    break;
            }
            context.Response.Write("{ " + msg + "}");
        }
        //检查电话是否可外呼
        private void checktel(out string msg)
        {
            msg = "result:'Yes'";
            if (!string.IsNullOrEmpty(RequestTel) && BlackWhiteList.Instance.PhoneNumIsNoDisturb(RequestTel) == 0)
            {
                msg = "result:'No'";
            }
        }
        //检查任务下是否已存在分配人
        private void TaskIsAssign(out string msg)
        {
            msg = string.Empty;
            //增加“任务列表--线索邀约 分配功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT101201"))
            {
                msg = "result:'noaccess',msg:'没有分配操作权限！'";
                return;
            }
            string[] arrTasks = TaskIDs.Split(',');

            if (arrTasks.Length == 0)
            {
                msg = "result:'false',msg:'请选择任务！'";
                return;
            }
            string tidsStr = "";
            for (int i = 0; i < arrTasks.Length; i++)
            {
                tidsStr += "'" + arrTasks[i] + "',";
            }
            DataTable dt = BLL.LeadsTask.Instance.GetTaskInfoListByIDs(tidsStr.TrimEnd(','));
            for (int i = 0, count = dt.Rows.Count; i < count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["AssignUserID"].ToString()) && dt.Rows[i]["AssignUserID"].ToString() != "-2")
                {
                    msg = "result:'false',msg:'" + dt.Rows[i]["AssignUserID"].ToString() + "'";
                    return;
                }
            }
            msg = "result:'true'";
        }

        //获取任务各状态下的数量
        private void GetStatusNum(out string msg)
        {
            msg = string.Empty;
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.LeadsTaskStatus));
            dt.Rows.Add("成功", "1");
            dt.Rows.Add("失败", "0");
            Hashtable ht = new Hashtable();

            Entities.QueryLeadsTask query = new Entities.QueryLeadsTask();
            query.LoginID = BLL.Util.GetLoginUserID();
            int _assignid = 0;
            if (int.TryParse(AssignID, out _assignid))
            {
                query.AssignUserID = _assignid;
            }
            if (ProjectName != "")
            {
                query.ProjectName = ProjectName;
            }
            if (RequestBeginDealTime != "")
            {
                query.BeginDealTime = RequestBeginDealTime;
            }
            if (RequestEndDealTime != "")
            {
                query.EndDealTime = RequestEndDealTime;
            }

            if (!string.IsNullOrEmpty(RequestTaskCBeginTime))
            {
                query.TaskCBeginTime = RequestTaskCBeginTime;
            }
            if (!string.IsNullOrEmpty(RequestTaskCEndTime))
            {
                query.TaskCEndTime = RequestTaskCEndTime;
            }
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                query.TaskID = RequestTaskID;
            }

            if (RequestProvinceID != "-1")
            {
                int reqProvinceId = -2;
                if (int.TryParse(RequestProvinceID, out reqProvinceId))
                {
                    query.ProvinceID = reqProvinceId;
                }
            }
            if (RequestCityID != "-1")
            {
                int reqCitId = -2;
                if (int.TryParse(RequestCityID, out reqCitId))
                {
                    query.CityID = reqCitId;
                }
            }
            if (!string.IsNullOrEmpty(RequestTel))
            {
                query.Tel = RequestTel;
            }

            DataTable dtCount = BLL.LeadsTask.Instance.GetStatusNum(query);

            //拼接起来
            for (int i = 0, len = dt.Rows.Count; i < len; i++)
            {
                DataRow dr = dt.Rows[i];
                string count = dtCount.Rows[0][dr["name"].ToString()].ToString();
                msg += "'" + dr["name"].ToString() + "':['" + dr["value"].ToString() + "','" + count + "'],";
            }

            msg = msg.Substring(0, msg.Length - 1);
        }

        //收回
        private void RecedeTask(out string msg)
        {
            msg = string.Empty;
            //增加”任务列表--线索邀约“收回功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT101202"))
            {
                msg = "result:'false',msg:'没有收回操作权限'";
                return;
            }
            validateTaskIDs("收回", out msg);
            if (msg != string.Empty) return;

            DateTime dtNow = DateTime.Now;

            //收回操作
            string[] arrTasks = TaskIDs.Split(',');
            for (int i = 0; i < arrTasks.Length; i++)
            {
                operLeads(arrTasks[i].ToString(), dtNow, 0, "收回");
            }

            msg = "result:'true',msg:'操作成功'";
        }

        //分配
        private void AssignTask(out string msg)
        {
            msg = string.Empty;
            validateTaskIDs("分配", out msg);
            if (msg != string.Empty) return;

            int _userid;
            if (!int.TryParse(UserID, out _userid))
            {
                msg = "result:'false',msg:'没有分配人，无法分配！'";
                return;
            }

            DateTime dtNow = DateTime.Now;

            //分配操作
            string[] arrTasks = TaskIDs.Split(',');
            for (int i = 0; i < arrTasks.Length; i++)
            {
                operLeads(arrTasks[i].ToString(), dtNow, _userid, "分配");
            }

            msg = "result:'true',msg:'操作成功'";
        }

        //验证
        private void validateTaskIDs(string desc, out string msg)
        {
            msg = string.Empty;
            string[] arrTasks = TaskIDs.Split(',');
            if (arrTasks.Length == 0)
            {
                msg = "result:'false',msg:'请选择任务！'";
                return;
            }

            string tidsStr = "";
            for (int i = 0; i < arrTasks.Length; i++)
            {
                tidsStr += "'" + arrTasks[i] + "',";
            }
            DataTable dt = BLL.LeadsTask.Instance.GetTaskInfoListByIDs(tidsStr.Substring(0, tidsStr.Length - 1));
            //检查任务的状态
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int _status = 0;
                if (!int.TryParse(dt.Rows[i]["Status"].ToString(), out _status))
                {
                    msg = "result:'false',msg:'任务ID为" + dt.Rows[i]["TaskID"].ToString() + "的状态有误！'";
                    return;
                }

                if (desc == "分配")
                {
                    if (_status != (int)Entities.LeadsTaskStatus.NoAllocation && _status != (int)Entities.LeadsTaskStatus.NoProcess)
                    {
                        msg = "result:'false',msg:'所选任务中，存在不允许分配的任务！'";
                        return;
                    }
                }
                else
                {
                    if (_status != (int)Entities.LeadsTaskStatus.NoProcess && _status != (int)Entities.LeadsTaskStatus.Processing)
                    {
                        msg = "result:'false',msg:'所选任务中，存在不允许收回的任务！'";
                        return;
                    }
                }

            }
        }

        //分配或收回操作
        private void operLeads(string taskID, DateTime dtNow, int userID, string desc)
        {
            int loginid = BLL.Util.GetLoginUserID();
            //更新LeadsTask
            Entities.LeadsTask model = BLL.LeadsTask.Instance.GetLeadsTask(taskID);
            if (model == null) return;

            if (desc == "分配")
            {
                model.Status = (int)Entities.LeadsTaskStatus.NoProcess;
                model.AssignUserID = userID;
            }
            else
            {
                model.Status = (int)Entities.LeadsTaskStatus.NoAllocation;
                model.AssignUserID = -2;
            }
            model.LastUpdateTime = dtNow;
            model.LastUpdateUserID = loginid;
            BLL.LeadsTask.Instance.Update(model);

            //插入LeadsTaskOperationLog
            Entities.LeadsTaskOperationLog model_Log = new Entities.LeadsTaskOperationLog();
            model_Log.CreateTime = dtNow;
            model_Log.CreateUserID = loginid;
            model_Log.Remark = desc;
            model_Log.TaskID = taskID;
            model_Log.TaskStatus = desc == "分配" ? (int)Entities.LeadsTaskStatus.NoProcess : (int)Entities.LeadsTaskStatus.NoAllocation;
            model_Log.OperationStatus = desc == "分配" ? (int)Entities.Leads_OperationStatus.Allocation : (int)Entities.Leads_OperationStatus.Recover;
            BLL.LeadsTaskOperationLog.Instance.Insert(model_Log);
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