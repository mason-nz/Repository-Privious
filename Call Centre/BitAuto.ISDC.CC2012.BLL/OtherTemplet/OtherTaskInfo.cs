using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类OtherTaskInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-20 03:24:41 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OtherTaskInfo
    {
        #region Instance
        public static readonly OtherTaskInfo Instance = new OtherTaskInfo();
        #endregion

        #region Contructor
        protected OtherTaskInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetOtherTaskInfo(QueryOtherTaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OtherTaskInfo.Instance.GetOtherTaskInfo(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 按照查询条件查询 (其他任务列表页) add lxw
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetOtherTaskInfoByList(QueryOtherTaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OtherTaskInfo.Instance.GetOtherTaskInfoByList(query, order, currentPage, pageSize, out totalCount);
        }
        public string GetWhereStringForLog(QueryOtherTaskInfo query)
        {
            string where = string.Empty;
            try
            {
                #region 数据权限判断
                if (query.LoginID != Constant.INT_INVALID_VALUE)
                {
                    string whereDataRight = "";
                    whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("ProjectInfo", "te", "BGID", "userID", query.LoginID);

                    where += whereDataRight;
                }
                #endregion

                if (query.PTID != Constant.STRING_INVALID_VALUE)
                {
                    where += " AND OtherTaskInfo.PTID='" + query.PTID + "'";
                }

                if (query.ProjectID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND ProjectInfo.ProjectID=" + query.ProjectID;
                }

                if (query.CustID != Constant.STRING_INVALID_VALUE)
                {
                    where += " AND OtherTaskInfo.CustID='" + query.CustID + "' ";
                }

                if (query.ProjectName != Constant.STRING_INVALID_VALUE)
                {
                    where += " AND ProjectInfo.Name like '%" + query.ProjectName + "%'";
                }

                if (query.Oper != Constant.INT_INVALID_VALUE)
                {
                    where += " AND LastOptUserID =" + query.Oper;
                }

                if (query.CreateUserID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND OtherTaskInfo.CreateUserID =" + query.CreateUserID;
                }

                if (query.BGID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND ProjectInfo.BGID=" + query.BGID;
                }

                if (query.SCID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND PCatageID=" + query.SCID;
                }
                if (query.BeginTime != Constant.STRING_INVALID_VALUE && query.EndTime != Constant.STRING_INVALID_VALUE)
                {
                    where += " AND OtherTaskInfo.LastOptTime>='" + query.BeginTime + " 0:00:000' AND OtherTaskInfo.LastOptTime<='" + query.EndTime + " 23:59:29'";
                }

                if (query.Statuss != Constant.STRING_INVALID_VALUE && query.Statuss != "-1")
                {
                    where += " and (";
                    for (int i = 0; i < query.Statuss.Split(',').Length; i++)
                    {
                        where += " OtherTaskInfo.TaskStatus='" + query.Statuss.Split(',')[i] + "' or";
                    }
                    where = where.Substring(0, where.Length - 2);
                    where += ")";
                }
                if (query.Agent != Constant.INT_INVALID_VALUE)
                {
                    where += " and te.UserID=" + query.Agent;
                }
                if (query.CustName != Constant.STRING_INVALID_VALUE)
                {
                    where += " And Exists (SELECT CustID FROM Crm2009.dbo.CustInfo WHERE OtherTaskInfo.CustID=CustID And Crm2009.dbo.CustInfo.CustName like '%" + query.CustName + "%')";
                }
                return where;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("获取查询字符串时出现异常，Message：" + ex.Message + "； StackTrace：" + ex.StackTrace);
                return "";
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OtherTaskInfo.Instance.GetOtherTaskInfo(new QueryOtherTaskInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        /// 获取需要结束的任务
        /// <summary>
        /// 获取需要结束的任务
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetStopForOtherTaskInfoByList(int ProjectID)
        {
            return Dal.OtherTaskInfo.Instance.GetStopForOtherTaskInfoByList(ProjectID);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OtherTaskInfo GetOtherTaskInfo(string PTID)
        {

            return Dal.OtherTaskInfo.Instance.GetOtherTaskInfo(PTID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByPTID(string PTID)
        {
            QueryOtherTaskInfo query = new QueryOtherTaskInfo();
            query.PTID = PTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOtherTaskInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.OtherTaskInfo model)
        {
            Dal.OtherTaskInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.OtherTaskInfo model)
        {
            Dal.OtherTaskInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.OtherTaskInfo model)
        {
            return Dal.OtherTaskInfo.Instance.Update(model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string PTID)
        {

            return Dal.OtherTaskInfo.Instance.Delete(PTID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string PTID)
        {

            return Dal.OtherTaskInfo.Instance.Delete(sqltran, PTID);
        }

        #endregion

        /// <summary>
        /// 根据任务id取，项目id，任务状态，TField信息，表单名称，自定义数据表系统名称
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetTFieldInfoByTaskID(string TaskID)
        {
            return Dal.OtherTaskInfo.Instance.GetTFieldInfoByTaskID(TaskID);
        }
        /// <summary>
        /// 根据TTCode取TTable信息
        /// </summary>
        /// <param name="TTCode"></param>
        /// <returns></returns>
        public DataTable GetTTableByTTCode(string TTCode)
        {
            return Dal.OtherTaskInfo.Instance.GetTTableByTTCode(TTCode);
        }
        /// <summary>
        /// 根据数据表主键，数据表编号取数据表中数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="TTCode"></param>
        /// <returns></returns>
        public DataTable GetCustomTable(string RelationID, string TTCode)
        {
            DataTable dt = null;
            dt = Dal.OtherTaskInfo.Instance.GetTTableByTTCode(TTCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Dal.OtherTaskInfo.Instance.GetCustomTable(RelationID, dt.Rows[0]["TTName"].ToString());
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 根据自定义数据表主键，自定义数据表表名称，保存数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="TTCode"></param>
        /// <param name="Dic"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool SaveCustomData(string TaskID, string RelationID, string TTCode, Dictionary<string, string> Dic, int Status)
        {
            bool flag = false;
            DataTable dt = null;
            dt = Dal.OtherTaskInfo.Instance.GetTTableByTTCode(TTCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                flag = Dal.OtherTaskInfo.Instance.SaveCustomData(RelationID, dt.Rows[0]["TTName"].ToString(), Dic, Status);

                //判断是否有话务结果，有则冗余保存-强斐-2015年11月2日
                int? IsEstablish = ConvertToInt(Dic, "IsEstablish");
                int? NotEstablishReason = ConvertToInt(Dic, "NotEstablishReason");
                int? IsSuccess = ConvertToInt(Dic, "IsSuccess");
                int? NotSuccessReason = ConvertToInt(Dic, "NotSuccessReason");
                if (IsEstablish != null || NotEstablishReason != null || IsSuccess != null || NotSuccessReason != null)
                {
                    long project = Dal.OtherTaskInfo.Instance.GetOtherTaskInfo(TaskID).ProjectID;
                    //存在话务结果
                    BLL.CallResult_ORIG_Task.Instance.InseretOrUpdateOneData(TaskID, project, ProjectSource.S4_其他任务, IsEstablish, NotEstablishReason, IsSuccess, NotSuccessReason);
                }
            }

            return flag;
        }

        private int? ConvertToInt(Dictionary<string, string> Dic, string key)
        {
            if (Dic.ContainsKey(key) && !string.IsNullOrEmpty(Dic[key]))
            {
                return (int?)CommonFunction.ObjectToInteger(Dic[key]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新任务状态(其他任务)
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="taskStatus"></param>
        /// <param name="description"></param>
        public void UpdateTaskStatus(string tId, Entities.OtheTaskStatus taskStatus, Entities.EnumProjectTaskOperationStatus operationStaus, string description, int userID)
        {
            //BLL.Loger.Log4Net.Info(string.Format("BitAuto.ISDC.CC2012.BLL.OtherTaskInfo.UpdateTaskStatus，参数：TID={0},taskStatus={1},更新逻辑开始...", tId, (int)taskStatus));
            Dal.OtherTaskInfo.Instance.UpdateTaskStatus(tId, taskStatus, operationStaus, description, userID);
            //BLL.Loger.Log4Net.Info(string.Format("BitAuto.ISDC.CC2012.BLL.OtherTaskInfo.UpdateTaskStatus，参数：TID={0},taskStatus={1},更新逻辑结束", tId, (int)taskStatus));
        }

        /// <summary>
        /// 根据projectID得到自定义的表字段
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetTFieldTableByProjectID(int projectID, string where)
        {
            return Dal.OtherTaskInfo.Instance.GetTFieldTableByProjectID(projectID, where);
        }
        /// <summary>
        /// 根据ProjectID得到自定义的表的数据信息
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public DataTable GetTemptInfoByProjectID(string ProjectID)
        {
            return Dal.OtherTaskInfo.Instance.GetTemptInfoByProjectID(ProjectID);
        }

        /// <summary>
        /// 供项目导出单元测试使用
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable ExportGetTemptInfoByProjectID(string ProjectID)
        {
            return Dal.OtherTaskInfo.Instance.ExportGetTemptInfoByProjectID(ProjectID);
        }

        public DataTable GetTaskInfoListByIDs(string TaskIDS)
        {
            return Dal.OtherTaskInfo.Instance.GetTaskInfoListByIDs(TaskIDS);
        }

        /// <summary>
        /// 得到其他任务的所属坐席
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllOtherTaskUserID()
        {
            return Dal.OtherTaskInfo.Instance.GetAllOtherTaskUserID();
        }

        public void GetTaskOrderInfoValues(string PTID, out string ProvinceID, out string CityID, out string YXBrandID, out string YXSerialID)
        {
            ProvinceID = "";
            CityID = "";
            YXBrandID = "";
            YXSerialID = "";
            //BLL.Loger.Log4Net.Info(string.Format("BitAuto.ISDC.CC2012.BLL.OtherTaskInfo，根据任务id:{0}取意向车型，意向品牌，省份，城市，开始...", PTID));
            Entities.OtherTaskInfo taskInfo = BLL.OtherTaskInfo.Instance.GetOtherTaskInfo(PTID);
            if (taskInfo != null)
            {
                Entities.TTable ttable = BLL.TTable.Instance.GetTTableByTTCode(taskInfo.RelationTableID);
                if (ttable != null)
                {
                    DataTable TempDataTable = Dal.OtherTaskInfo.Instance.GetCustomTable(taskInfo.RelationID, ttable.TTName);
                    if (TempDataTable.Rows.Count == 1)
                    {
                        for (int colIndex = TempDataTable.Columns.Count - 1; colIndex >= 0; colIndex--)
                        {
                            if (TempDataTable.Columns[colIndex].ColumnName.IndexOf("_Province") != -1)
                            {
                                //存在省份字段
                                ProvinceID = TempDataTable.Rows[0][colIndex].ToString();
                            }
                            if (TempDataTable.Columns[colIndex].ColumnName.IndexOf("_City") != -1)
                            {
                                //存在城市字段
                                CityID = TempDataTable.Rows[0][colIndex].ToString();
                            }
                            if (TempDataTable.Columns[colIndex].ColumnName.IndexOf("_YXBrand") != -1)
                            {
                                //存在意向品牌字段
                                YXBrandID = TempDataTable.Rows[0][colIndex].ToString();
                            }
                            if (TempDataTable.Columns[colIndex].ColumnName.IndexOf("_YXSerial") != -1)
                            {
                                //存在意向车型字段
                                YXSerialID = TempDataTable.Rows[0][colIndex].ToString();
                            }
                        }
                    }
                }
            }
            //BLL.Loger.Log4Net.Info(string.Format("BitAuto.ISDC.CC2012.BLL.OtherTaskInfo，根据任务id:{0}取意向车型，意向品牌，省份，城市，结束", PTID));
        }

        /// 获取任务信息，通过id区间值
        /// <summary>
        /// 获取任务信息，通过id区间值
        /// </summary>
        /// <param name="minID"></param>
        /// <param name="maxID"></param>
        /// <returns></returns>
        public DataTable GetOtherTaskInfoByMaxIDAndMinID(string minID, string maxID, int top)
        {
            return Dal.OtherTaskInfo.Instance.GetOtherTaskInfoByMaxIDAndMinID(minID, maxID, top);
        }
        /// 根据ProjectID获取指定数量的未分配的任务IDs
        /// <summary>
        /// 根据ProjectID获取指定数量的未分配的任务IDs
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TopCount"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetOtherTaskIDsByProjectId(int ProjectID, int TopCount, out int totalCount)
        {
            return Dal.OtherTaskInfo.Instance.GetOtherTaskIDsByProjectId(ProjectID, TopCount, out totalCount);
        }
        /// 获取任务信息，通过id区间值
        /// <summary>
        /// 获取任务信息，通过id区间值
        /// </summary>
        /// <param name="minID"></param>
        /// <param name="maxID"></param>
        /// <returns></returns>
        public int GetOtherTaskCountByMaxIDAndMinID(string minID, string maxID)
        {
            return Dal.OtherTaskInfo.Instance.GetOtherTaskCountByMaxIDAndMinID(minID, maxID);
        }
        public string GetNotDistrictCountAndTaskCount(string ProjectID)
        {
            return Dal.OtherTaskInfo.Instance.GetNotDistrictCountAndTaskCount(ProjectID);
        }
        /// 批量更新状态
        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="minID"></param>
        /// <param name="maxID"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public int UpdateOtherTaskCountByMaxIDAndMinID(string minID, string maxID, int top)
        {
            return Dal.OtherTaskInfo.Instance.UpdateOtherTaskCountByMaxIDAndMinID(minID, maxID, top);
        }
        /// 根据项目id更新相应的任务ids的状态（状态改为2：已分配）
        /// <summary>
        /// 根据项目id更新相应的任务ids的状态（状态改为2：已分配）
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TaskIDs"></param>
        /// <returns></returns>
        public int UpdateTaskStatusByTaskIDs(int ProjectID, string TaskIDs, int operuserid)
        {
            return Dal.OtherTaskInfo.Instance.UpdateTaskStatusByTaskIDs(ProjectID, TaskIDs, operuserid);
        }
        /// 删除重复的电话号码-其他任务导入
        /// <summary>
        /// 删除重复的电话号码-其他任务导入
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="ttname"></param>
        /// <param name="tfname"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> DelSameTelForImportOtherData(long projectid, string ttname, string tfname, string[] rangeid, BlackListCheckType blacklistchecktype)
        {
            return Dal.OtherTaskInfo.Instance.DelSameTelForImportOtherData(projectid, ttname, tfname, rangeid, blacklistchecktype, new Action<string>(BLL.Loger.Log4Net.Info));
        }
    }
}

