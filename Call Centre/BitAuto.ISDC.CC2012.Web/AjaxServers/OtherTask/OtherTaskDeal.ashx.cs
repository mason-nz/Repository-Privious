using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using System.Collections;
using BitAuto.ISDC.CC2012.WebService.Market;
using BitAuto.Utils;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask
{
    /// <summary>
    /// OtherTaskDeal 的摘要说明
    /// </summary>
    public class OtherTaskDeal : IHttpHandler, IRequiresSessionState
    {
        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }
        private string RelationID
        {
            get
            {
                return HttpContext.Current.Request["RelationID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RelationID"].ToString());
            }
        }
        private string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString());
            }
        }
        private string RelationTableID
        {
            get
            {
                return HttpContext.Current.Request["RelationTableID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["RelationTableID"].ToString());
            }
        }
        private string TTCode
        {
            get
            {
                return HttpContext.Current.Request["TTCode"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TTCode"].ToString());
            }
        }


        //个人属性模板-客户姓名
        private string CustName
        {
            get
            {
                return HttpContext.Current.Request["CustName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString());
            }
        }

        private string TelePhones
        {
            get
            {
                return HttpContext.Current.Request["TelePhones"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TelePhones"].ToString());
            }
        }

        //个人属性模板-客户性别
        private string Sex
        {
            get
            {
                return HttpContext.Current.Request["Sex"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Sex"].ToString());
            }
        }

        //个人属性模板-客户电话
        private string TelePhone
        {
            get
            {
                return HttpContext.Current.Request["TelePhone"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TelePhone"].ToString());
            }
        }

        //取自定义表单数据
        private string Body
        {
            get
            {
                return HttpContext.Current.Request["Body"] == null ? "" : HttpContext.Current.Request["Body"].ToString();
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 活动guid串
        /// </summary>
        private string ActivityGuidStr
        {
            get
            {
                return HttpContext.Current.Request["ActivityGuidStr"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ActivityGuidStr"].ToString());
            }
        }
        public int userId = 0;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";
            if (msg == "")
            {
                userId = BLL.Util.GetLoginUserID();

                switch (Action)
                {
                    case "GetCustomDataInfo":
                        if (string.IsNullOrEmpty(RelationID) || string.IsNullOrEmpty(RelationTableID))
                        {
                            msg = "参数错误";
                        }
                        else
                        {
                            GetCustomDataInfo(RelationID, RelationTableID, out msg);
                        }
                        break;
                    case "SavecustomData":
                        if (string.IsNullOrEmpty(RelationID) || string.IsNullOrEmpty(RelationTableID) || string.IsNullOrEmpty(Body) || string.IsNullOrEmpty(TaskID))
                        {
                            msg = "参数错误";
                        }
                        else if (VerifyLogic(TaskID, userId, ref msg))
                        {
                            SavecustomData(RelationID, TaskID, RelationTableID, Body, out msg);
                        }

                        break;
                    case "SubcustomData":
                        if (string.IsNullOrEmpty(RelationID) || string.IsNullOrEmpty(RelationTableID) || string.IsNullOrEmpty(Body) || string.IsNullOrEmpty(TaskID))
                        {
                            msg = "参数错误";
                        }
                        else if (VerifyLogic(TaskID, userId, ref msg))
                        {
                            SubcustomData(RelationID, TaskID, RelationTableID, Body, out msg);
                        }
                        break;
                    case "IsShowBtnByTTCode":
                        IsShowBtnByTTCode(TTCode, out msg);

                        break;
                    case "GetCallRecordORGIHistory":
                        GetCallRecordORGIHistory(out msg);

                        break;
                    case "GetActivityInfo":
                        //取活动信息，通过活动guid串
                        GetActivityInfoByGuidStr(out msg);
                        break;
                    case "CheckTelByProjectID":
                        CheckTelByProjectID(out msg);
                        break;
                }
            }
            context.Response.Write(msg);
        }

        private bool VerifyLogic(string taskID, int userId, ref string msg)
        {
            bool flag = false;
            Entities.OtherTaskInfo model = BLL.OtherTaskInfo.Instance.GetOtherTaskInfo(taskID);
            if (model != null)
            {
                switch (model.TaskStatus)
                {
                    case (Int32)Entities.OtheTaskStatus.Processed:
                        msg = "该任务已处理完毕！";
                        break;
                    case (Int32)Entities.OtheTaskStatus.StopTask:
                        msg = "该任务已结束！";
                        break;
                    case (Int32)Entities.OtheTaskStatus.Unallocated:
                        msg = "该任务未分配处理人！";
                        break;
                    default:
                        DataTable dtEmployee = BLL.ProjectTask_Employee.Instance.GetProjectTask_Employee(taskID);
                        if (dtEmployee != null && dtEmployee.Rows.Count > 0)
                        {
                            if (userId.ToString() != dtEmployee.Rows[0]["userid"].ToString())
                            {
                                msg = "您没有该任务的处理权限！";
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            msg = "该任务未分配处理人！";
                        }
                        break;
                }
            }
            return flag;
        }
        private void CheckTelByProjectID(out string msg)
        {
            msg = "Yes";
            if (!string.IsNullOrEmpty(TaskID) && !string.IsNullOrEmpty(TelePhone))
            {
                //根据TaskID取projectID
                Entities.OtherTaskInfo model = BLL.OtherTaskInfo.Instance.GetOtherTaskInfo(TaskID);
                if (model != null)
                {
                    bool flag = BLL.BlackWhiteList.Instance.CheckPhoneAndTelIsInBlackList(model.ProjectID, TelePhone);
                    if (flag)
                    {
                        msg = "No";
                    }
                }
            }
        }
        //取活动信息，通过活动guid串
        private void GetActivityInfoByGuidStr(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(ActivityGuidStr))
            {
                string[] Guidstr = ActivityGuidStr.Split(',');
                DataSet ds = MarketServiceHelper.Instance.GetDataXml(Guidstr);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{'Name':'" + BLL.Util.EscapeString(StringHelper.SqlFilter(dt.Rows[i]["name"].ToString())) + "','Guid':'" + BLL.Util.EscapeString(StringHelper.SqlFilter(dt.Rows[i]["Guid"].ToString())) + "','URL':'" + BLL.Util.EscapeString(StringHelper.SqlFilter(dt.Rows[i]["URL"].ToString())) + "'},");
                    }
                    msg = sb.ToString();
                    if (msg != "")
                    {
                        if (msg.EndsWith(","))
                            msg = msg.Substring(0, msg.Length - 1);

                        msg = "[" + msg + "]";
                    }
                }
            }


        }
        //插入任务操作日志
        private void SaveTaskLog(string TaskID, int TaskStatus, int OperationStatus)
        {
            Entities.ProjectTaskLog model = new Entities.ProjectTaskLog();
            model.PTID = TaskID;
            model.CreateUserID = userId;
            model.CreateTime = System.DateTime.Now;
            model.TaskStatus = TaskStatus;
            model.OperationStatus = OperationStatus;
            BLL.ProjectTaskLog.Instance.InserOtherTaskLog(model);
        }


        /// <summary>
        /// 取自定义表数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="RelationTableID"></param>
        /// <param name="jsonStr"></param>
        private void GetCustomDataInfo(string RelationID, string RelationTableID, out string jsonStr)
        {
            jsonStr = "";
            DataTable dt = null;
            dt = BLL.OtherTaskInfo.Instance.GetCustomTable(RelationID, RelationTableID);

            if (dt != null)
            {
                jsonStr = BLL.Util.DateTableToJson2ForOtherDeal(dt);
            }
        }

        private void GetCallRecordORGIHistory(out string msg)
        {
            msg = string.Empty;
            List<jsonData> resultList = new List<jsonData>();
            List<string[]> retList;
            BLL.CustBasicInfo.Instance.GetCallRecordORGIHistory(TelePhones, TaskID, out retList);
            StringBuilder sb = new StringBuilder();
            foreach (string[] item in retList)
            {
                sb.Append("{'result':'" + item[0] + "','Tel':'" + item[1] + "','CustID':'" + item[2] + "'},");
            }
            msg = sb.ToString();
            if (msg.EndsWith(","))
                msg = msg.Substring(0, msg.Length - 1);
            msg = "[" + msg + "]";
        }

        /// <summary>
        /// 获取IsShowBtn的值
        /// </summary>
        /// <param name="TTCode"></param>
        /// <returns></returns>
        private void IsShowBtnByTTCode(string TTCode, out string msg)
        {
            msg = string.Empty;

            Entities.QueryTPage query = new Entities.QueryTPage();
            query.TTCode = TTCode;
            int totalCount = 0;
            DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 1, out totalCount);
            if (totalCount > 0)
            {
                if (dt.Rows[0]["IsShowBtn"].ToString() == "1")
                {
                    msg = "'IsShowBtn':'true',";
                }

                if (dt.Rows[0]["IsShowWorkOrderBtn"].ToString() == "1")
                {
                    msg += "'IsShowWorkOrderBtn':'true',";
                }
                if (dt.Rows[0]["IsShowSendMsgBtn"].ToString() == "1")
                {
                    msg += "'IsShowSendMsgBtn':'true',";
                }
                if (dt.Rows[0]["IsShowQiCheTong"].ToString() == "1")
                {
                    msg += "'IsShowQiCheTong':'true',";
                }
                if (dt.Rows[0]["IsShowSubmitOrder"].ToString() == "1")
                {
                    msg += "'IsShowSubmitOrder':'true',";
                }

            }

            if (msg == string.Empty)
            {
                msg = "{'result':'false'}";
            }
            else
            {
                msg = "{'result':'true'," + msg.TrimEnd(',') + "}";
            }

        }
        /// <summary>
        /// 保存自定义表单数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="RelationTableID"></param>
        /// <param name="Body"></param>
        /// <param name="msg"></param>
        private void SavecustomData(string RelationID, string TaskID, string RelationTableID, string Body, out string msg)
        {
            msg = "";
            Dictionary<string, string> customdata = GetcustomData(Body);
            bool flag = BLL.OtherTaskInfo.Instance.SaveCustomData(TaskID, RelationID, RelationTableID, customdata, 0);
            if (!flag)
            {
                msg = "保存自定义表单数据失败";
            }
            else
            {
                BLL.OtherTaskInfo.Instance.UpdateTaskStatus(TaskID, Entities.OtheTaskStatus.Processing, Entities.EnumProjectTaskOperationStatus.TaskSave, "保存", userId);

                //如果是使用了个人属性模板，需要将自定义表的数据更新到任务表，以供后期查询
                if (!string.IsNullOrEmpty(CustName) && !string.IsNullOrEmpty(Sex) && !string.IsNullOrEmpty(TelePhone))
                {
                    Entities.OtherTaskInfo model = new Entities.OtherTaskInfo();
                    model.PTID = TaskID;
                    model.CustNameTemp = CustName;
                    model.SexTemp = Convert.ToInt32(Sex);
                    model.TelePhoneTemp = TelePhone;
                    model.DataTypeTemp = 1;
                    BLL.OtherTaskInfo.Instance.Update(model);
                }
            }

        }
        /// <summary>
        /// 提交自定义表单数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="RelationTableID"></param>
        /// <param name="Body"></param>
        /// <param name="msg"></param>
        private void SubcustomData(string RelationID, string TaskID, string RelationTableID, string Body, out string msg)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            msg = "";
            BLL.Loger.Log4Net.Info("【提交自定义表单数据提交Step1】开始,任务ID为：" + TaskID);

            Dictionary<string, string> customdata = GetcustomData(Body);
            bool flag = BLL.OtherTaskInfo.Instance.SaveCustomData(TaskID, RelationID, RelationTableID, customdata, 1);
            BLL.Loger.Log4Net.Info("【提交自定义表单数据提交Step2】结束，返回值：" + flag.ToString());
            if (!flag)
            {
                msg = "提交自定义表单数据失败";
                BLL.Loger.Log4Net.Info("【提交自定义表单数据提交Step3】提交自定义表单数据失败");
            }
            else
            {
                BLL.Loger.Log4Net.Info("【提交自定义表单数据提交Step4】更新其他任务状态开始");
                BLL.OtherTaskInfo.Instance.UpdateTaskStatus(TaskID, Entities.OtheTaskStatus.Processed, Entities.EnumProjectTaskOperationStatus.TaskSubmit, "提交", userId);
                BLL.Loger.Log4Net.Info("【提交自定义表单数据提交Step5】更新其他任务状态结束");

                //如果是使用了个人属性模板，需要将自定义表的数据更新到任务表，以供后期查询
                if (!string.IsNullOrEmpty(CustName) && !string.IsNullOrEmpty(Sex) && !string.IsNullOrEmpty(TelePhone))
                {
                    Entities.OtherTaskInfo model = new Entities.OtherTaskInfo();
                    model.PTID = TaskID;
                    model.CustNameTemp = CustName;
                    model.SexTemp = Convert.ToInt32(Sex);
                    model.TelePhoneTemp = TelePhone;
                    model.DataTypeTemp = 1;
                    BLL.Loger.Log4Net.Info(string.Format("提交自定义表单数据提交Step6】BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask.OtherTaskDeal.ashx，参数：PTID={0},将自定义表的数据更新到任务表开始...", TaskID));
                    BLL.OtherTaskInfo.Instance.Update(model);
                    BLL.Loger.Log4Net.Info(string.Format("提交自定义表单数据提交Step7】BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask.OtherTaskDeal.ashx，参数：PTID={0},将自定义表的数据更新到任务表结束", TaskID));
                }
            }
            stopwatch.Stop();
            BLL.Loger.Log4Net.Info(string.Format("【提交自定义表单数据提交Step8】总耗时：{0}毫秒", stopwatch.Elapsed.TotalMilliseconds));
        }
        /// <summary>
        /// 取推荐活动GUID串
        /// </summary>
        /// <param name="customdata"></param>
        /// <returns></returns>
        protected string GetActiveGuidStr(Dictionary<string, string> customdata)
        {
            string ActivityGUIDStr = string.Empty;
            foreach (string key in customdata.Keys)
            {
                //如果是活动id串
                if (key.Substring(key.Length - 9, 9) == "_Activity")
                {
                    if (customdata[key].Trim().Length > 0)
                    {
                        ActivityGUIDStr = customdata[key].Trim();
                    }
                }
            }
            return ActivityGUIDStr;
        }
        /// <summary>
        /// 根据json串，把数据放在键值对对象了
        /// </summary>
        /// <param name="Body"></param>
        /// <returns></returns>
        protected Dictionary<string, string> GetcustomData(string Body)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<string, string> customData = null;
            if (!string.IsNullOrEmpty(Body))
            {
                Body = Body.Substring(0, Body.Length - 1);
                customData = new Dictionary<string, string>();
                for (int i = 0; i < Body.Split(',').Length; i++)
                {
                    string Datastr = Body.Split(',')[i];

                    customData.Add(Datastr.Split(':')[0], HttpUtility.UrlDecode(Datastr.Split(':')[1]));
                }
            }
            sw.Stop();
            BLL.Loger.Log4Net.Info("BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask.GetcustomData(body),用时：" + sw.Elapsed);
            return customData;
        }
    }

    public class jsonData
    {
        public string Tel;
        public string CustID;
        public string result;
    }
}