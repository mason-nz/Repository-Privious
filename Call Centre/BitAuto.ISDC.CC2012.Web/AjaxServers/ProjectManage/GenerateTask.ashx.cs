using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;
using System.Threading;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// GenerateTask 的摘要说明
    /// </summary>
    public class GenerateTask : IHttpHandler, IRequiresSessionState
    {
        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }

        private string MinOtherTaskID
        {
            get
            {
                return HttpContext.Current.Request["MinOtherTaskID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MinOtherTaskID"].ToString());
            }
        }

        private string MaxOtherTaskID
        {
            get
            {
                return HttpContext.Current.Request["MaxOtherTaskID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MaxOtherTaskID"].ToString());
            }
        }

        private string Data
        {
            get
            {
                return HttpContext.Current.Request["Data"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Data"].ToString());
            }
        }

        private string Source
        {
            get
            {
                return HttpContext.Current.Request["source"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["source"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            string data = "";
            if (msg == "")
            {
                switch (Action)
                {
                    case "GenerateTask":
                        int userID = BLL.Util.GetLoginUserID();
                        if (!BLL.Util.CheckRight(userID, "SYS024BUT500604"))
                        {
                            msg = "您没有执行此操作的权限";
                        }
                        else
                        {
                            CheckPar(out msg);
                            DoGenerateTask(out msg, out data);
                        }
                        break;
                    case "AssignEmployee":
                        AssignEmployee(out msg, out data);
                        break;
                    case "AssignEmployeeNew":
                        AssignEmployeeNew(out msg, out data);
                        break;
                }
            }

            if (msg == "")
            {
                msg = "success";
            }
            if (data == "")
            {
                data = "{}";
            }
            string r = "{msg:'" + msg + "',data:" + data + "}";

            context.Response.Write(r);
        }

        /// 分配时，不允许并行执行
        /// <summary>
        /// 分配时，不允许并行执行
        /// </summary>
        private static readonly object assignlock = new object();
        /// 批量分配任务
        /// <summary>
        /// 批量分配任务
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        private void AssignEmployeeNew(out string msg, out string data)
        {
            msg = "";
            data = "";
            try
            {
                int projectid;
                if (int.TryParse(ProjectID, out projectid))
                {
                    string strPTIDs = "";
                    string error = "";
                    //人员表-空表
                    DataTable emp_dt = BLL.ProjectTask_Employee.Instance.GetProjectTask_Employee("-1");

                    int assi_total = 0;
                    //坐席每人应该分配多条个任务 从前台传递来的
                    Dictionary<int, int> agents = GetEmployeeData(out assi_total);
                    //坐席每人应该分配多条个任务 从前台传递来的
                    KeyValuePair<int, int>[] agents_array = agents.ToArray();

                    lock (assignlock)
                    {
                        DateTime d = DateTime.Now;
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        BLL.Loger.Log4Net.Info("批量分配任务锁定开始------");
                        //数据库中未分配的任务数量
                        int NotDistrictTaskCount = GetDbNotAssignCount();
                        if (NotDistrictTaskCount < assi_total)
                        {
                            msg = "需要分配的任务数不足，无法分配，请重新选择数量！";
                            return;
                        }
                        BLL.Loger.Log4Net.Info("查询未分配的总数 耗时：" + sw.Elapsed);
                        sw.Reset();
                        sw.Start();
                        //获取需要分配的任务ids
                        int totalTaskIdCount;
                        //查询未分配的任务，并清理ProjectTask_Employee表
                        DataTable task_dt = BLL.OtherTaskInfo.Instance.GetOtherTaskIDsByProjectId(projectid, assi_total, out totalTaskIdCount);
                        //任务ID列表
                        strPTIDs = CreateEMPdata(task_dt, ref emp_dt, agents_array);
                        if (strPTIDs == "")
                        {
                            msg = "没有可以分配的任务！";
                            return;
                        }
                        BLL.Loger.Log4Net.Info("构造数据 耗时：" + sw.Elapsed);
                        sw.Reset();
                        sw.Start();
                        //重新清理ProjectTask_Employee strPTIDs
                        BLL.ProjectTask_Employee.Instance.ClearProjectTaskEmployee(strPTIDs);
                        //批量入库                   
                        BLL.Util.BulkCopyToDB(emp_dt, CommonBll.CC_conn, "ProjectTask_Employee", out error);
                        BLL.Loger.Log4Net.Info("BulkCopyToDB 耗时：" + sw.Elapsed);
                        sw.Reset();
                        sw.Start();
                        //更新任务状态和同步分配人
                        BLL.OtherTaskInfo.Instance.UpdateTaskStatusByTaskIDs(projectid, strPTIDs, BLL.Util.GetLoginUserID());
                        BLL.Loger.Log4Net.Info("UpdateTaskStatusByTaskIDs 耗时：" + sw.Elapsed);
                        sw.Stop();
                        BLL.Loger.Log4Net.Info("批量分配任务锁定结束------ 锁定时长：" + (DateTime.Now - d).ToString());
                    }
                    BcpAssignLogs(emp_dt);
                }
                else
                {
                    msg = "项目id异常！";
                }
            }
            catch (Exception ex)
            {
                msg = "分配任务时出现异常";
                BLL.Loger.Log4Net.Error("分配任务时出现异常:", ex);
            }
        }
        /// 获取数据库未分配任务的数量
        /// <summary>
        /// 获取数据库未分配任务的数量
        /// </summary>
        /// <returns></returns>
        private int GetDbNotAssignCount()
        {
            int NotDistrictTaskCount = 0;
            string[] arrCount = BLL.OtherTaskInfo.Instance.GetNotDistrictCountAndTaskCount(ProjectID).Split(',');
            if (arrCount.Length == 2)
            {
                int notdistrictcount;
                if (int.TryParse(arrCount[0], out notdistrictcount))
                {
                    NotDistrictTaskCount = notdistrictcount;
                }
            }
            return NotDistrictTaskCount;
        }
        /// 构建人员分配表
        /// <summary>
        /// 构建人员分配表
        /// </summary>
        /// <param name="task_dt"></param>
        /// <param name="emp_dt"></param>
        /// <param name="agents_array"></param>
        /// <returns></returns>
        private static string CreateEMPdata(DataTable task_dt, ref DataTable emp_dt, KeyValuePair<int, int>[] agents_array)
        {
            string strPTIDs = "";
            //构建表数据
            DateTime now = DateTime.Now;
            int i = 0, j = 0;
            foreach (DataRow dr in task_dt.Rows)
            {
                //***************PTID,UserID,Status,CreateTime,CreateUserID************//
                DataRow emp_dr = emp_dt.NewRow();
                emp_dr["PTID"] = dr["PTID"];
                strPTIDs += ",'" + dr["PTID"] + "'";
                emp_dr["Status"] = 0;
                emp_dr["CreateTime"] = now;
                emp_dr["CreateUserID"] = BLL.Util.GetLoginUserID();
                //分配人员
                int userid = -1;
                if (j >= agents_array[i].Value)
                {
                    //当前人完成
                    j = 0;
                    i++;
                }
                userid = agents_array[i].Key;
                j++;
                emp_dr["UserID"] = userid;

                emp_dt.Rows.Add(emp_dr);
            }
            if (strPTIDs.Length > 1)
            {
                strPTIDs = strPTIDs.Substring(1);
            }
            return strPTIDs;
        }
        /// 批量插入分配日志
        /// <summary>
        /// 批量插入分配日志
        /// </summary>
        /// <param name="emp_dt"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private void BcpAssignLogs(DataTable emp_dt)
        {
            if (emp_dt.Rows.Count == 0)
                return;
            //批量插入日志
            DataTable logdt = BLL.ProjectTaskLog.Instance.GetProjectTaskLog("-1");
            foreach (DataRow dr in emp_dt.Rows)
            {
                DataRow newdr = logdt.NewRow();
                newdr["PTID"] = dr["PTID"];
                newdr["TaskStatus"] = (int)OtheTaskStatus.Untreated;
                newdr["OperationStatus"] = (int)EnumProjectTaskOperationStatus.TaskAllot;
                newdr["Description"] = "批量分配";
                newdr["CreateTime"] = DateTime.Now;
                newdr["CreateUserID"] = dr["CreateUserID"];
                logdt.Rows.Add(newdr);
            }
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("PTID", "PTID"));
            list.Add(new SqlBulkCopyColumnMapping("TaskStatus", "TaskStatus"));
            list.Add(new SqlBulkCopyColumnMapping("OperationStatus", "OperationStatus"));
            list.Add(new SqlBulkCopyColumnMapping("Description", "Description"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            list.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
            string error = "";
            BLL.Util.BulkCopyToDB(logdt, CommonBll.CC_conn, "ProjectTaskLog", 10000, list, out error);
        }
        /// 获取坐席情况
        /// <summary>
        /// 获取坐席情况
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetEmployeeData(out int total)
        {
            total = 0;
            Dictionary<int, int> dic = new Dictionary<int, int>();
            string[] array = Data.Split(',');
            foreach (string key in array)
            {
                string[] a = key.Split(':');
                if (a.Length == 2)
                {
                    int userid = CommonFunction.ObjectToInteger(a[0]);
                    int task = CommonFunction.ObjectToInteger(a[1]);
                    if (userid * task != 0)
                    {
                        dic[userid] = task;
                        total += task;
                    }
                }
            }
            return dic;
        }

        private void CheckPar(out  string msg)
        {
            msg = "";
            int intVal = 0;

            if (ProjectID == "")
            {
                msg += "项目ID参数不正确";
            }
            if (!int.TryParse(ProjectID, out intVal))
            {
                msg += "项目ID参数格式不正确";
            }

        }

        /// 生成任务
        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="msg"></param>
        public void DoGenerateTask(out string msg, out string data)
        {
            msg = "";
            data = "";
            int userId = BLL.Util.GetLoginUserID();

            Entities.ProjectInfo ProjectModel = BLL.ProjectInfo.Instance.GetProjectInfo(int.Parse(ProjectID));
            if (ProjectModel != null)
            {
                if (ProjectModel.Status != 0 && ProjectModel.Status != 1)
                {
                    msg += "项目的当前状态不允许生成任务";
                    return;
                }
                else
                {
                    Entities.QueryProjectDataSoure dsQuery = new Entities.QueryProjectDataSoure();
                    dsQuery.ProjectID = int.Parse(ProjectID);
                    dsQuery.Status = 0;

                    int totalCount = 0;
                    DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(dsQuery, "", 1, 99999999, out totalCount);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //Source:1：数据清洗-Excel；2：数据清洗-Crm；3：客户回访；4：其他任务
                        switch (Source)
                        {
                            case "1":
                            case "2":
                            case "3":
                                GenByDataTable(ProjectModel, userId, dt, out msg);
                                break;
                            case "4":
                                GenByOtherTaskDataTable(ProjectModel, userId, dt, out msg, out data);
                                int? acstatus = BLL.ProjectInfo.Instance.GetProjectAutoCallACStatus(long.Parse(ProjectID));
                                if (acstatus.HasValue && (acstatus.Value == (int)ProjectACStatus.P01_进行中 || acstatus.Value == (int)ProjectACStatus.P02_暂停中))
                                {
                                    //有自动外呼，且状态=进行中和暂停中 导入数据到自动外呼表中
                                    BLL.AutoCall_TaskInfo.Instance.AutoCallTaskInfoUpdate_Async(long.Parse(ProjectID));
                                }
                                if (acstatus.HasValue && acstatus.Value == (int)ProjectACStatus.P01_进行中)
                                {
                                    //只有进行中的不弹出分配框
                                    data = data.Replace("@open", "false");
                                }
                                else
                                {
                                    data = data.Replace("@open", "true");
                                }
                                break;
                        }
                    }
                    else
                    {
                        msg += "此项目没有关联的数据";
                    }
                }
            }
            else
            {
                msg += "没有找到对应的项目";
            }
        }
        private void GenByDataTable(Entities.ProjectInfo ProjectModel, int userId, DataTable dt, out string msg)
        {
            msg = "";

            #region 根据数据关联生成任务

            List<Entities.ProjectTaskInfo> list = new List<Entities.ProjectTaskInfo>();
            Entities.ProjectTaskInfo ptaskModel = null;

            List<Entities.CustLastOperTask> list_OperTask = new List<Entities.CustLastOperTask>();
            List<Entities.CustLastOperTask> list_OldOperTask = new List<Entities.CustLastOperTask>();

            int maxcount = BLL.ProjectTaskInfo.Instance.GetMax();//从ProjectTaskInfo表中查找最大值

            string PixStr = "";
            if (ProjectModel.Source == 1)
            {
                PixStr = "IMP";
            }
            else if (ProjectModel.Source == 2)
            {
                PixStr = "CRM";
            }
            DateTime dtime = DateTime.Now;

            foreach (DataRow dr in dt.Rows)
            {
                ptaskModel = new Entities.ProjectTaskInfo();

                ptaskModel.PTID = PixStr + (++maxcount).ToString().PadLeft(7, '0');

                ptaskModel.PDSID = long.Parse(dr["PDSID"].ToString());
                ptaskModel.ProjectID = int.Parse(ProjectID);
                ptaskModel.CustName = dr["CustName"].ToString(); //客户名称
                ptaskModel.Source = int.Parse(dr["Source"].ToString());
                ptaskModel.RelationID = dr["RelationID"].ToString();
                ptaskModel.CrmCustID = dr["CustID"].ToString();//CRM客户ID
                ptaskModel.CreateTime = dtime;
                ptaskModel.CreateUserID = userId;
                ptaskModel.LastOptTime = dtime;
                ptaskModel.LastOptUserID = userId;
                ptaskModel.TaskStatus = (int)(EnumProjectTaskStatus.NoSelEmployee);
                ptaskModel.Status = 0;
                ptaskModel.CustType = dr["CustType"].ToString();

                list.Add(ptaskModel);

                if (PixStr == "CRM")
                {
                    Entities.CustLastOperTask operTaskModel = new BitAuto.ISDC.CC2012.Entities.CustLastOperTask();

                    if (ptaskModel.CrmCustID != "")
                    {
                        Entities.CustLastOperTask oldModel = BLL.CustLastOperTask.Instance.GetCustLastOperTask(ptaskModel.CrmCustID);
                        if (oldModel == null)
                        {
                            operTaskModel.CustID = ptaskModel.CrmCustID;
                            operTaskModel.TaskID = ptaskModel.PTID;
                            operTaskModel.TaskType = 1;
                            operTaskModel.LastOperUserID = operTaskModel.CreateUserID = BLL.Util.GetLoginUserID();
                            operTaskModel.LastOperTime = operTaskModel.CreateTime = dtime;
                            list_OperTask.Add(operTaskModel);
                            list_OldOperTask.Add(null);
                        }
                        else
                        {
                            operTaskModel.CustID = ptaskModel.CrmCustID;
                            operTaskModel.TaskID = ptaskModel.PTID;
                            operTaskModel.LastOperUserID = BLL.Util.GetLoginUserID();
                            operTaskModel.LastOperTime = dtime;
                            list_OperTask.Add(operTaskModel);
                            list_OldOperTask.Add(oldModel);
                        }
                    }
                }
            }

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            try
            {
                //add by qizq 2013-3-11 4S和非4S电话营销
                if (ProjectModel.Source != 3) //Modify By Chybin At 2014-3-31
                {
                    #region 生成任务
                    foreach (Entities.ProjectTaskInfo item in list)
                    {
                        BLL.ProjectTaskInfo.Instance.Add(tran, item);
                    }
                    BLL.ProjectLog.Instance.InsertProjectLog(ProjectModel.ProjectID, ProjectLogOper.L6_生成任务, "生成任务" + list.Count + "条", tran);
                    #endregion
                }

                #region 修改项目状态
                ProjectModel.Status = 1;
                BLL.ProjectInfo.Instance.Update(tran, ProjectModel);
                #endregion

                #region 修改关联数据状态
                BLL.ProjectDataSoure.Instance.UpdateStatusByProjectId(tran, "1", (int)ProjectModel.ProjectID);
                #endregion

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                msg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }



            #endregion
        }

        #region 生成任务（其他任务） add lxw 13.3.25

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectModel">项目</param>
        /// <param name="userId"></param>
        /// <param name="dt">项目的DataSource数据</param>
        /// <param name="msg"></param>
        public void GenByOtherTaskDataTable(Entities.ProjectInfo ProjectModel, int userId, DataTable dt, out string msg, out string data)
        {
            msg = "";
            data = "";

            #region panduan

            bool IsExistCrmCustID = false;
            string ttName = "";
            string tfName = "";

            List<Entities.TField> fieldlist = BLL.TField.Instance.GetTFieldListByTTCode(ProjectModel.TTCode);
            Entities.TField fielditem = fieldlist.Find(delegate(Entities.TField o) { return o.TFShowCode == "100014"; });
            if (fielditem != null)
            {
                IsExistCrmCustID = true;
                tfName = fielditem.TFName;

                Entities.TTable tableModel = BLL.TTable.Instance.GetTTableByTTCode(ProjectModel.TTCode);
                if (tableModel != null)
                {
                    ttName = tableModel.TTName;
                }
            }

            #endregion

            #region 根据数据关联生成任务
            List<Entities.OtherTaskInfo> list = new List<Entities.OtherTaskInfo>();
            int maxcount = BLL.ProjectTaskInfo.Instance.GetMaxRecID();//从OtherTaskInfo表中查找最大值，不是ProjectTaskInfo表中查找最大值
            DataTable SourceOtherTaskDt = new DataTable();
            SourceOtherTaskDt.Columns.Add("PTID");
            SourceOtherTaskDt.Columns.Add("ProjectID", typeof(int));
            SourceOtherTaskDt.Columns.Add("RelationTableID");
            SourceOtherTaskDt.Columns.Add("RelationID");
            SourceOtherTaskDt.Columns.Add("CreateTime", typeof(DateTime));
            SourceOtherTaskDt.Columns.Add("CreateUserID", typeof(int));
            SourceOtherTaskDt.Columns.Add("LastOptTime", typeof(DateTime));
            SourceOtherTaskDt.Columns.Add("LastOptUserID", typeof(int));
            SourceOtherTaskDt.Columns.Add("TaskStatus", typeof(int));
            SourceOtherTaskDt.Columns.Add("Status", typeof(int));
            SourceOtherTaskDt.Columns.Add("CustID");

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                DataRow newDr = SourceOtherTaskDt.NewRow();

                string PixStr = "";
                if (ProjectModel.Source == 4)
                {
                    PixStr = "OTH";
                }

                newDr["PTID"] = PixStr + (++maxcount).ToString().PadLeft(7, '0');
                //记录最小id和最大id
                if (i == 0)
                {
                    data += "{open:'@open',MinOtherTaskID:'" + newDr["PTID"] + "',";
                }
                if (i + 1 == dt.Rows.Count)
                {
                    data += "MaxOtherTaskID:'" + newDr["PTID"] + "'}";
                }

                newDr["ProjectID"] = int.Parse(ProjectID);
                newDr["RelationTableID"] = ProjectModel.TTCode;
                newDr["RelationID"] = dr["RelationID"].ToString();
                newDr["CreateTime"] = DateTime.Now;
                newDr["CreateUserID"] = userId;
                newDr["LastOptTime"] = DateTime.Now;
                newDr["LastOptUserID"] = userId;
                newDr["TaskStatus"] = (int)(OtheTaskStatus.Unallocated);
                newDr["Status"] = 0;
                if (IsExistCrmCustID)
                {
                    newDr["CustID"] = GetCustID(ProjectModel, ttName, tfName, dr["RelationID"].ToString());
                }
                else
                {
                    newDr["CustID"] = "";
                }
                SourceOtherTaskDt.Rows.Add(newDr);
                i++;
            }

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            try
            {
                BLL.Util.BulkCopyToDB(SourceOtherTaskDt, connectionstrings, "OtherTaskInfo", tran, out msg);
                BLL.ProjectLog.Instance.InsertProjectLog(ProjectModel.ProjectID, ProjectLogOper.L6_生成任务, "生成任务" + SourceOtherTaskDt.Rows.Count + "条", tran);

                if (msg == "")
                {
                    #region 修改项目状态
                    ProjectModel.Status = 1;
                    BLL.ProjectInfo.Instance.Update(tran, ProjectModel);
                    #endregion

                    #region 修改关联数据状态
                    BLL.ProjectDataSoure.Instance.UpdateStatusByProjectId(tran, "1", dt, (int)ProjectModel.ProjectID);
                    #endregion

                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                msg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        private string GetCustID(Entities.ProjectInfo projectInfo, string ttName, string tfName, string RelationID)
        {
            string custid = "";
            string msg = "";

            DataTable tempTableDt = BLL.TTable.Instance.GetDataByRelationIDs(RelationID, ttName, out msg);
            if (tempTableDt != null && tempTableDt.Rows.Count > 0)
            {
                custid = tempTableDt.Rows[0][tfName + "_crmcustid_name"].ToString();
            }

            return custid;
        }

        #endregion

        /// 其他任务-分配坐席--废弃，替代：AssignEmployeeNew
        /// <summary>
        /// 其他任务-分配坐席--废弃，替代：AssignEmployeeNew
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        private void AssignEmployee(out string msg, out string data)
        {
            msg = "";
            data = "";
            try
            {
                int assi_total = 0;
                Dictionary<int, int> agents = GetEmployeeData(out assi_total);
                int task_total = BLL.OtherTaskInfo.Instance.GetOtherTaskCountByMaxIDAndMinID(MinOtherTaskID, MaxOtherTaskID);
                if (task_total < assi_total)
                {
                    throw new Exception("分配任务数大于总任务数！");
                }
                //获取需要分配的任务
                DataTable task_dt = BLL.OtherTaskInfo.Instance.GetOtherTaskInfoByMaxIDAndMinID(MinOtherTaskID, MaxOtherTaskID, assi_total);
                //清空上次分配人
                BLL.ProjectTask_Employee.Instance.ClearProjectTaskEmployee(MinOtherTaskID, MaxOtherTaskID, assi_total);
                //人员表-空表
                DataTable emp_dt = BLL.ProjectTask_Employee.Instance.GetProjectTask_Employee("-1");
                //构建表数据
                DateTime now = DateTime.Now;
                int i = 0, j = 0;
                KeyValuePair<int, int>[] agents_array = agents.ToArray();
                foreach (DataRow dr in task_dt.Rows)
                {
                    //PTID,UserID,Status,CreateTime,CreateUserID
                    DataRow emp_dr = emp_dt.NewRow();
                    emp_dr["PTID"] = dr["PTID"];
                    emp_dr["Status"] = 0;
                    emp_dr["CreateTime"] = now;
                    emp_dr["CreateUserID"] = BLL.Util.GetLoginUserID();
                    //分配人员
                    int userid = -1;
                    if (j >= agents_array[i].Value)
                    {
                        //当前人完成
                        j = 0;
                        i++;
                    }
                    userid = agents_array[i].Key;
                    j++;
                    emp_dr["UserID"] = userid;

                    emp_dt.Rows.Add(emp_dr);
                }
                //批量入库
                string error = "";
                BLL.Util.BulkCopyToDB(emp_dt, ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC"), "ProjectTask_Employee", out error);
                //修改任务表状态 ：未处理
                BLL.OtherTaskInfo.Instance.UpdateOtherTaskCountByMaxIDAndMinID(MinOtherTaskID, MaxOtherTaskID, assi_total);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
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