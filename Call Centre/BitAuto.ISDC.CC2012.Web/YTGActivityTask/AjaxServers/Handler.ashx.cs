using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers
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
        public string loginUser { get { return (Request["lg"] + "").Trim(); } }

        //private string RequestBeginDealTime
        //{
        //    get { return HttpContext.Current.Request["BeginDealTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BeginDealTime"].ToString()); }
        //}
        //private string RequestEndDealTime
        //{
        //    get { return HttpContext.Current.Request["EndDealTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EndDealTime"].ToString()); }
        //}

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

        #region  保存任务处理信息

        private string RequestUserName
        {
            get { return HttpContext.Current.Request["UserName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["UserName"].ToString()); }
        }
        private string RequestSex
        {
            get { return HttpContext.Current.Request["Sex"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Sex"].ToString()); }
        }
        private string RequestTestDriveProvinceID
        {
            get { return HttpContext.Current.Request["TestDriveProvinceID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TestDriveProvinceID"].ToString()); }
        }
        private string RequestTestDriveCityID
        {
            get { return HttpContext.Current.Request["TestDriveCityID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TestDriveCityID"].ToString()); }
        }
        private string RequestDCarSerialID
        {
            get { return HttpContext.Current.Request["DCarSerialID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DCarSerialID"].ToString()); }
        }
        private string RequestPBuyCarTime
        {
            get { return HttpContext.Current.Request["PBuyCarTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PBuyCarTime"].ToString()); }
        }
        private string RequestIsSuccess
        {
            get { return HttpContext.Current.Request["IsSuccess"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsSuccess"].ToString()); }
        }
        private string RequestIsJT
        {
            get { return HttpContext.Current.Request["IsJT"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsJT"].ToString()); }
        }
        private string RequestFailReason
        {
            get { return HttpContext.Current.Request["FailReason"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["FailReason"].ToString()); }
        }
        private string RequestRemark
        {
            get { return HttpContext.Current.Request["Remark"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Remark"].ToString()); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "assigntask": AssignTask(out msg);
                    break;
                case "taskisassign": TaskIsAssign(out msg);
                    break;
                case "recedetask": RecedeTask(out msg);
                    break;
                case "getstatusnum": GetStatusNum(out msg);
                    break;
                case "saveytgtaskinfo":
                    SaveYTGTaskInfo(out msg);
                    break;
                case "submitytgtaskinfo":
                    SubmitYTGTaskInfo(out msg);
                    break;
            }
            context.Response.Write("{ " + msg + "}");
        }

        private void SubmitYTGTaskInfo(out string msg)
        {
            msg = "";
            DealTask(2, out msg);
        }

        private void SaveYTGTaskInfo(out string msg)
        {
            msg = "";
            DealTask(1, out msg);
        }
        /// <summary>
        /// ordertype 1是保存，2是提交
        /// </summary>
        /// <param name="ordertype"></param>
        protected void DealTask(int ordertype, out string msg)
        {
            msg = "";
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                Entities.YTGActivityTaskInfo model = BLL.YTGActivityTask.Instance.GetComAdoInfo<YTGActivityTaskInfo>(RequestTaskID);
                if (model != null)
                {
                    if (model.Status != (int)Entities.YTGActivityTaskStatus.Processing && model.Status != (int)Entities.YTGActivityTaskStatus.NoProcess)
                    {
                        #region 如果任务被撤销任然保存任务,魏淑珍要求的
                        if (model.Status == (int)Entities.LeadsTaskStatus.ReBack)
                        {
                            model.LastUpdateTime = System.DateTime.Now;
                            model.LastUpdateUserID = BLL.Util.GetLoginUserID(); ;

                            //姓名
                            model.UserName = RequestUserName;

                            //性别
                            model.Sex = CommonFunction.ObjectToInteger(RequestSex, -2);

                            //试驾省
                            model.TestDriveProvinceID = CommonFunction.ObjectToInteger(RequestTestDriveProvinceID, -1);

                            //试驾城市
                            model.TestDriveCityID = CommonFunction.ObjectToInteger(RequestTestDriveCityID, -1);

                            //意向车型
                            model.DCarSerialID = CommonFunction.ObjectToInteger(RequestDCarSerialID, -2);

                            //预计购车时间
                            model.PBuyCarTime = CommonFunction.ObjectToInteger(RequestPBuyCarTime, -2);

                            //是否成功
                            model.IsSuccess = CommonFunction.ObjectToInteger(RequestIsSuccess, -2);

                            //失败原因
                            model.FailReason = CommonFunction.ObjectToInteger(RequestFailReason, -2);

                            //是否接通
                            model.IsJT = CommonFunction.ObjectToInteger(RequestIsJT, -2);

                            //备注信息
                            model.Remark = RequestRemark;

                            //更新任务信息
                            try
                            {
                                BLL.YTGActivityTask.Instance.UpdateComAdoInfo(model);
                            }
                            catch (Exception ex)
                            {
                                BLL.Loger.Log4Net.Info("被撤销的任务更新任务信息失败：" + ex.Message);
                            }

                        }
                        msg = "{\"Result\":false,\"Msg\":\"任务不处于处理状态！\"}";
                        #endregion
                    }
                    else
                    {
                        #region 保存或提交订单信息，修改任务状态，插入任务操作状态
                        model.LastUpdateTime = System.DateTime.Now;
                        model.LastUpdateUserID = BLL.Util.GetLoginUserID();

                        //姓名
                        model.UserName = RequestUserName;

                        //性别
                        model.Sex = CommonFunction.ObjectToInteger(RequestSex, -2);

                        //试驾省
                        model.TestDriveProvinceID = CommonFunction.ObjectToInteger(RequestTestDriveProvinceID, -1);

                        //试驾城市
                        model.TestDriveCityID = CommonFunction.ObjectToInteger(RequestTestDriveCityID, -1);

                        //意向车型
                        model.DCarSerialID = CommonFunction.ObjectToInteger(RequestDCarSerialID, -2);

                        //预计购车时间
                        model.PBuyCarTime = CommonFunction.ObjectToInteger(RequestPBuyCarTime, -2);

                        //是否成功
                        model.IsSuccess = CommonFunction.ObjectToInteger(RequestIsSuccess, -2);

                        //失败原因
                        model.FailReason = CommonFunction.ObjectToInteger(RequestFailReason, -2);

                        //是否接通
                        model.IsJT = CommonFunction.ObjectToInteger(RequestIsJT, -2);

                        //备注信息
                        model.Remark = RequestRemark;

                        //保存
                        if (ordertype == 1)
                        {
                            model.Status = (int)Entities.YTGActivityTaskStatus.Processing;
                        }
                        //提交
                        else if (ordertype == 2)
                        {
                            model.Status = (int)Entities.YTGActivityTaskStatus.Processed;
                        }

                        //更新任务信息
                        bool blSuccess = false;
                        try
                        {
                            blSuccess = BLL.YTGActivityTask.Instance.UpdateComAdoInfo<Entities.YTGActivityTaskInfo>(model);
                        }
                        catch (Exception ex)
                        {
                            BLL.Loger.Log4Net.Info("更新任务信息失败：" + ex.Message);
                            msg = "{\"Result\":false,\"Msg\":\"更新任务信息失败！\"}";
                        }

                        //插入任务操作日志
                        DealLog(ordertype);
                        #endregion

                        #region 提交处理
                        if (ordertype == 2)
                        {
                            #region 写入要回传的数据
                            Entities.YTGCallResultNoticeInfo resultNoticeModel = new Entities.YTGCallResultNoticeInfo();
                            if (blSuccess)
                            {
                                resultNoticeModel.TaskID = RequestTaskID;
                                resultNoticeModel.Status = 0;
                                resultNoticeModel.CreateTime = DateTime.Now;
                                try
                                {
                                    BLL.YTGCallResultNotice.Instance.InsertComAdoInfo<Entities.YTGCallResultNoticeInfo>(resultNoticeModel);
                                }
                                catch (Exception ex)
                                {
                                    BLL.Loger.Log4Net.Info("写入回传数据失败：" + ex.Message);
                                    msg = "{\"Result\":false,\"Msg\":\"写入回传数据失败！\"}";
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    msg = "{\"Result\":false,\"Msg\":\"任务不存在！\"}";
                }
            }
        }

        /// <summary>
        /// ordertype 1是保存，2是提交
        /// </summary>
        /// <param name="ordertype"></param>
        protected void DealLog(int ordertype)
        {
            Entities.YTGActivityTaskLogInfo logmodel = new Entities.YTGActivityTaskLogInfo();
            logmodel.Remark = RequestRemark;
            logmodel.TaskID = RequestTaskID;
            logmodel.CreateTime = System.DateTime.Now;
            logmodel.CreateUserID = BLL.Util.GetLoginUserID(); ;
            if (ordertype == 1)
            {
                logmodel.OperationStatus = (int)Entities.YTGActivityOperationStatus.Save;
                logmodel.TaskStatus = (int)Entities.YTGActivityTaskStatus.Processing;
                logmodel.Remark = "保存";
            }
            else if (ordertype == 2)
            {
                logmodel.OperationStatus = (int)Entities.YTGActivityOperationStatus.Submit;
                logmodel.TaskStatus = (int)Entities.YTGActivityTaskStatus.Processed;
                logmodel.Remark = "提交";
            }
            try
            {
                BLL.YTGActivityTaskLog.Instance.InsertComAdoInfo<Entities.YTGActivityTaskLogInfo>(logmodel);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("记录操作日志失败：" + ex.Message);
            }
        }







        //检查任务下是否已存在分配人
        private void TaskIsAssign(out string msg)
        {
            msg = string.Empty;
            //增加“易团购--邀约处理”分配功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024MOD101503"))
            {
                msg = "result:'noaccess',msg:'没有分配操作权限'";
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
            DataTable dt = BLL.YTGActivityTask.Instance.GetYTGTaskInfoListByIDs(tidsStr.TrimEnd(','));
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
            //DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.YTGActivityTaskStatus));
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("value");
            dt.Rows.Add("待分配", "1");
            dt.Rows.Add("待处理", "2");
            dt.Rows.Add("处理中", "3");
            dt.Rows.Add("已处理", "4");
            dt.Rows.Add("已结束", "5");
            dt.Rows.Add("成功", "1");
            dt.Rows.Add("失败", "0");
            Hashtable ht = new Hashtable();

            Entities.QueryYTGActivityTaskInfo query = new Entities.QueryYTGActivityTaskInfo();
            query.LoginID = Convert.ToInt32(loginUser); //BLL.Util.GetLoginUserID();
            int _assignid = 0;
            if (int.TryParse(AssignID, out _assignid))
            {
                query.AssignUserID = _assignid;
            }
            if (ProjectName != "")
            {
                query.ProjectName = ProjectName;
            }
            //if (RequestBeginDealTime != "")
            //{
            //    query.BeginDealTime = RequestBeginDealTime;
            //}
            //if (RequestEndDealTime != "")
            //{
            //    query.EndDealTime = RequestEndDealTime;
            //}

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


            DataTable dtCount = BLL.YTGActivityTask.Instance.GetStatusNum(query);

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
            //增加“易团购--邀约处理”收回功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024MOD101504"))
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
            DataTable dt = BLL.YTGActivityTask.Instance.GetYTGTaskInfoListByIDs(tidsStr.Substring(0, tidsStr.Length - 1));
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
                    if (_status != (int)Entities.YTGActivityTaskStatus.NoAllocation && _status != (int)Entities.YTGActivityTaskStatus.NoProcess)
                    {
                        msg = "result:'false',msg:'所选任务中，存在不允许分配的任务！'";
                        return;
                    }
                }
                else
                {
                    if (_status != (int)Entities.YTGActivityTaskStatus.NoProcess && _status != (int)Entities.YTGActivityTaskStatus.Processing)
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
            int nStatus = 1;
            string strAssignUserID = string.Empty;
            if (desc == "分配")
            {
                strAssignUserID = userID.ToString();
                nStatus = (int)Entities.YTGActivityTaskStatus.NoProcess;
            }
            else
            {
                //回收
                strAssignUserID = "-2";
                nStatus = (int)Entities.YTGActivityTaskStatus.NoAllocation;
            }

            BLL.YTGActivityTask.Instance.UpdateYTGTaskStatus(taskID, strAssignUserID, loginid, nStatus);

            //插入日志
            YTGActivityTaskLogInfo loginfo = new YTGActivityTaskLogInfo();
            loginfo.TaskID = taskID;
            //记录本次操作的动作
            loginfo.OperationStatus = desc == "分配"
                ? (int)Entities.YTGActivityOperationStatus.Allocation
                : (int)Entities.YTGActivityOperationStatus.Recover;

            //记录操作完成后任务的状态。
            loginfo.TaskStatus = desc == "分配"
                ? (int)Entities.YTGActivityTaskStatus.NoProcess
                : (int)Entities.YTGActivityTaskStatus.NoAllocation;

            loginfo.Remark = desc;
            loginfo.CreateTime = DateTime.Now;
            BLL.YTGActivityTaskLog.Instance.InsertComAdoInfo<YTGActivityTaskLogInfo>(loginfo);



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