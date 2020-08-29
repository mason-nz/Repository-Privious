using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类OtherTaskInfo。
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
    public class OtherTaskInfo : DataBase
    {
        #region Instance
        public static readonly OtherTaskInfo Instance = new OtherTaskInfo();
        #endregion

        #region const
        private const string P_OTHERTASKINFO_SELECT = "p_OtherTaskInfo_Select";
        private const string P_OTHERTASKINFO_INSERT = "p_OtherTaskInfo_Insert";
        private const string P_OTHERTASKINFO_UPDATE = "p_OtherTaskInfo_Update";
        private const string P_OTHERTASKINFO_DELETE = "p_OtherTaskInfo_Delete";
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
            string where = string.Empty;
            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " and PTID='" + Utils.StringHelper.SqlFilter(query.PTID) + "'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
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
            string where = string.Empty;

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
                where += " AND OtherTaskInfo.PTID='" + StringHelper.SqlFilter(query.PTID) + "'";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectInfo.ProjectID=" + query.ProjectID;
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND OtherTaskInfo.CustID='" + StringHelper.SqlFilter(query.CustID) + "' ";
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ProjectInfo.Name like '%" + StringHelper.SqlFilter(query.ProjectName) + "%'";
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
                where += " AND OtherTaskInfo.LastOptTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:000' AND OtherTaskInfo.LastOptTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:29'";
            }
            if (query.Statuss != Constant.STRING_INVALID_VALUE && query.Statuss != "-1")
            {
                where += " and (";
                for (int i = 0; i < Util.SqlFilterByInCondition(query.Statuss).Split(',').Length; i++)
                {
                    where += " OtherTaskInfo.TaskStatus='" + Util.SqlFilterByInCondition(query.Statuss).Split(',')[i] + "' or";
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
                where += " And Exists (SELECT CustID FROM Crm2009.dbo.CustInfo WHERE OtherTaskInfo.CustID=CustID And Crm2009.dbo.CustInfo.CustName like '%" + SqlFilter(query.CustName) + "%')";
            }


            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OtherTaskInfo_SelectByList", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OtherTaskInfo GetOtherTaskInfo(string PTID)
        {
            QueryOtherTaskInfo query = new QueryOtherTaskInfo();
            query.PTID = PTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOtherTaskInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOtherTaskInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.OtherTaskInfo LoadSingleOtherTaskInfo(DataRow row)
        {
            Entities.OtherTaskInfo model = new Entities.OtherTaskInfo();

            model.PTID = row["PTID"].ToString();
            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            model.RelationTableID = row["RelationTableID"].ToString();
            model.RelationID = row["RelationID"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["LastOptTime"].ToString() != "")
            {
                model.LastOptTime = DateTime.Parse(row["LastOptTime"].ToString());
            }
            if (row["LastOptUserID"].ToString() != "")
            {
                model.LastOptUserID = int.Parse(row["LastOptUserID"].ToString());
            }
            if (row["TaskStatus"].ToString() != "")
            {
                model.TaskStatus = int.Parse(row["TaskStatus"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["GUID"].ToString() != "")
            {
                model.GUID = new Guid(row["GUID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.OtherTaskInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@RelationTableID", SqlDbType.NVarChar,20),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastOptTime", SqlDbType.DateTime),
					new SqlParameter("@LastOptUserID", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.RelationTableID;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.LastOptTime;
            parameters[7].Value = model.LastOptUserID;
            parameters[8].Value = model.TaskStatus;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CustID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKINFO_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.OtherTaskInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@RelationTableID", SqlDbType.NVarChar,20),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int),
					new SqlParameter("@LastOptTime", SqlDbType.DateTime),
					new SqlParameter("@LastOptUserID", SqlDbType.Int),
					new SqlParameter("@TaskStatus", SqlDbType.Int),
					new SqlParameter("@Status", SqlDbType.Int),
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.RelationTableID;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.LastOptTime;
            parameters[7].Value = model.LastOptUserID;
            parameters[8].Value = model.TaskStatus;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CustID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_OTHERTASKINFO_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据(更新个人用户属性自定义表值到任务表)
        /// </summary>
        public int Update(Entities.OtherTaskInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),					
                    new SqlParameter("@CustName",SqlDbType.NVarChar,50),
                    new SqlParameter("@Sex",SqlDbType.Int,4),
                    new SqlParameter("@TelePhone",SqlDbType.NVarChar,50),
                    new SqlParameter("@DataType",SqlDbType.Int,4),
                    new SqlParameter("@CustID",SqlDbType.NVarChar,50)};

            parameters[0].Value = model.PTID;
            parameters[1].Value = model.CustNameTemp;
            parameters[2].Value = model.SexTemp;
            parameters[3].Value = model.TelePhoneTemp;
            parameters[4].Value = model.DataTypeTemp;
            parameters[5].Value = model.CustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string PTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,50)};
            parameters[0].Value = PTID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_OTHERTASKINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string PTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,50)};
            parameters[0].Value = PTID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_OTHERTASKINFO_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 根据任务id取，项目id，任务状态，TField信息，表单名称，自定义数据表系统名称
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public DataTable GetTFieldInfoByTaskID(string TaskID)
        {
            DataTable dt = null;
            string sqlstr = "select a.PTID,a.ProjectID,a.TaskStatus,b.*,c.TTName,d.TpName from OtherTaskinfo a left join Tfield b on a.RelationtableID=b.TTCOde left join TTable c on a.RelationtableID=c.TTCode  left join Tpage d on a.RelationtableID=d.TTCode where a.status=0 and a.PTID='" + Utils.StringHelper.SqlFilter(TaskID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            dt = ds.Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据TTCode取自定义数据表信息
        /// </summary>
        /// <param name="TTCode"></param>
        /// <returns></returns>
        public DataTable GetTTableByTTCode(string TTCode)
        {
            DataTable dt = null;
            string sqlstr = "select * from TTable where TTCode='" + Utils.StringHelper.SqlFilter(TTCode) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            dt = ds.Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据自定义数据表主键，和自定义表名称取自定义表数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="CustomTable"></param>
        /// <returns></returns>
        public DataTable GetCustomTable(string RelationID, string CustomTable)
        {
            DataTable dt = null;
            string sqlstr = "select * from " + CustomTable + " where RecID='" + Utils.StringHelper.SqlFilter(RelationID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            dt = ds.Tables[0];
            return dt;
        }


        /// <summary>
        /// 更新任务状态(其他任务)
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="taskStatus"></param>
        /// <param name="description"></param>
        public void UpdateTaskStatus(string tId, Entities.OtheTaskStatus taskStatus, Entities.EnumProjectTaskOperationStatus operationStaus, string description, int userID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@TaskStatus", SqlDbType.Int),
					new SqlParameter("@Description", SqlDbType.VarChar,200),
					new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@OperationStatus", SqlDbType.Int)
            };
            parameters[0].Value = tId;
            parameters[1].Value = (int)taskStatus;
            parameters[2].Value = description;
            parameters[3].Value = userID;
            parameters[4].Value = (int)operationStaus;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OtherTaskInfo_UpdateTaskStatus", parameters);
        }


        /// <summary>
        /// 根据自定义数据表主键，自定义数据表表名称，保存数据
        /// </summary>
        /// <param name="RelationID"></param>
        /// <param name="TTCode"></param>
        /// <param name="Dic"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool SaveCustomData(string RelationID, string TTableName, Dictionary<string, string> Dic, int Status)
        {
            bool flag = false;
            string str = "";
            str = " update " + Utils.StringHelper.SqlFilter(TTableName) + " set ";
            foreach (string key in Dic.Keys)
            {
                str += key + "='" + Utils.StringHelper.SqlFilter(Dic[key]) + "',";
            }
            str += " Status=" + Status;
            str += "  where RecID=" + RelationID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, str, null);
            flag = true;
            return flag;
        }

        /// <summary>
        /// 根据projectID得到自定义的表字段
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetTFieldTableByProjectID(int projectID, string where)
        {
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int),
					new SqlParameter("@where", SqlDbType.VarChar) 
            };
            parameters[0].Value = projectID;
            parameters[1].Value = where;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OtherTaskInfo_GetTFieldByProjectID", null);
            dt = ds.Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据ProjectID得到自定义的表的数据信息
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns></returns>
        public DataTable GetTemptInfoByProjectID(string ProjectID)
        {
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.VarChar,10) 
            };
            parameters[0].Value = ProjectID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OtherTaskInfo_GetTemptInfoByPTID", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// 供项目导出单元测试使用
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable ExportGetTemptInfoByProjectID(string ProjectID)
        {
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.VarChar,10) 
            };
            parameters[0].Value = ProjectID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OtherTaskInfo_ExportGetTemptInfoByPTID", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataTable GetTaskInfoListByIDs(string TaskIDS)
        {
            string sqlStr = @"SELECT a.*,
                                        CASE WHEN ISNULL(b.ACTID,0)=0 THEN 0 ELSE 1 END AS isAutoCall,
                                        ISNULL(b.ACStatus,0) AS ACStatus,
                                        ISNULL(c.ACStatus,0) AS  ProjectACStatus
                                        FROM dbo.OtherTaskInfo a
                                        LEFT JOIN dbo.AutoCall_TaskInfo  b ON a.PTID=b.BusinessID
                                        LEFT JOIN dbo.AutoCall_ProjectInfo c ON a.ProjectID=c.ProjectID 
                                        WHERE a.PTID IN  (" + Dal.Util.SqlFilterByInCondition(TaskIDS) + ")";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到其他任务的所属坐席
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllOtherTaskUserID()
        {
            string sqlStr = " select distinct UserID from ProjectTask_Employee where PTID like 'OTH%' ";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
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
            string sql = @"SELECT TOP " + top + " PTID FROM dbo.OtherTaskInfo WHERE PTID>='" + StringHelper.SqlFilter(minID) + "' AND PTID<='" + StringHelper.SqlFilter(maxID) + "' ORDER BY PTID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 根据ProjectID获取指定数量的未分配的任务IDs
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TopCount"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetOtherTaskIDsByProjectId(int ProjectID, int TopCount, out int totalCount)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int) ,
                    new SqlParameter("@TopCount", SqlDbType.Int) ,
                    new SqlParameter("@totalRecorder", SqlDbType.Int)
                                        };
            parameters[0].Value = ProjectID;
            parameters[1].Value = TopCount;
            parameters[2].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetAssigningPTIDsByProjectID", parameters);
            totalCount = (int)(parameters[2].Value);
            return ds.Tables[0];
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
            string sql = @"SELECT count(*) FROM dbo.OtherTaskInfo WHERE PTID>='" + StringHelper.SqlFilter(minID) + "' AND PTID<='" + StringHelper.SqlFilter(maxID) + "'";
            return CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }
        public string GetNotDistrictCountAndTaskCount(string ProjectID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int) ,
                    new SqlParameter("@NotDistrictAndTaskCount", SqlDbType.VarChar,20)
                                        };
            parameters[0].Value = ProjectID;
            parameters[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetNotDistrictedCountAndTaskCount", parameters);
            return parameters[1].Value.ToString();
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
            string sql = @"UPDATE dbo.OtherTaskInfo
                                    SET TaskStatus=2
                                    WHERE PTID IN (" + "SELECT TOP " + top + " PTID FROM dbo.OtherTaskInfo WHERE PTID>='" + StringHelper.SqlFilter(minID) + "' AND PTID<='" + StringHelper.SqlFilter(maxID) + "' ORDER BY PTID" + ")";
            int a = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);

            //更新对应的自动外呼任务表数据中的Status
            string sql1 = @"UPDATE dbo.AutoCall_TaskInfo
                                    SET Status=2
                                    WHERE BusinessID IN (" + "SELECT TOP " + top + " PTID FROM dbo.OtherTaskInfo WHERE PTID>='" + StringHelper.SqlFilter(minID) + "' AND PTID<='" + StringHelper.SqlFilter(maxID) + "' ORDER BY PTID" + ")";
            int b = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql1);
            return a;
        }
        /// <summary>
        /// 根据项目id更新相应的任务ids的状态（状态改为2：已分配）
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TaskIDs"></param>
        /// <returns></returns>
        public int UpdateTaskStatusByTaskIDs(int ProjectID, string TaskIDs, int operuserid)
        {
            string strSql = string.Empty;
            string strSql2 = string.Empty;
            string projectSource = GetProjectSourceType(ProjectID);
            switch (projectSource)
            {
                case "1":
                case "2":
                case "3":
                    strSql = @"UPDATE dbo.ProjectTaskInfo SET 
                                TaskStatus=2 ,
                                LastOptUserID=" + operuserid + @",
                                LastOptTime=GETDATE()  
                                WHERE ProjectID=" + ProjectID + " AND TaskStatus=1 AND Status=0 and PTID IN(" + TaskIDs + ")";
                    break;
                case "4":
                    strSql = @"UPDATE dbo.OtherTaskInfo SET 
                                TaskStatus=2,
                                LastOptUserID=" + operuserid + @",
                                LastOptTime=GETDATE()  
                                WHERE ProjectID=" + ProjectID + " AND TaskStatus=1 and PTID IN (" + TaskIDs + ")";
                    break;
                case "5":
                case "6":
                    strSql = @"UPDATE dbo.LeadsTask SET 
                                Status=2 
                                WHERE ProjectID=" + ProjectID + "  AND Status=1 and TaskID IN(" + TaskIDs + ")";
                    strSql2 = @"UPDATE a SET 
                                a.AssignUserID=b.UserID,
                                a.LastUpdateTime=b.CreateTime,
                                a.LastUpdateUserID = b.CreateUserID
                                FROM LeadsTask a
                                INNER JOIN ProjectTask_Employee b ON a.TaskID=b.PTID
                                WHERE a.Status=2 AND ISNULL(a.AssignUserID,-2)<0
                                AND ISNULL(b.UserID,-2)>0";
                    break;
                case "7":
                    strSql = @"UPDATE dbo.YTGActivityTask SET 
                                Status=2 
                                WHERE ProjectID=" + ProjectID + " AND Status=1 and TaskID IN (" + TaskIDs + ")";
                    strSql2 = @"UPDATE a SET 
                                a.AssignUserID=b.UserID,
                                a.LastUpdateTime=b.CreateTime,
                                a.LastUpdateUserID = b.CreateUserID
                                FROM YTGActivityTask a
                                INNER JOIN ProjectTask_Employee b ON a.TaskID=b.PTID
                                WHERE a.Status=2 AND ISNULL(a.AssignUserID,-2)<0
                                AND ISNULL(b.UserID,-2)>0";
                    break;
            }
            if (string.IsNullOrEmpty(strSql))
            {
                return -1;
            }
            else
            {
                int retVal1 = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
                if (projectSource == "5" || projectSource == "6" || projectSource == "7")
                {
                    SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql2);
                }
                return retVal1;
            }
        }

        public string GetProjectSourceType(int ProjectID)
        {
            string strSql = "SELECT top 1 source FROM dbo.ProjectInfo WHERE ProjectID=" + ProjectID + " AND Status=1";
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql));
        }

        /// 删除重复的电话号码-其他任务导入
        /// <summary>
        /// 删除重复的电话号码-其他任务导入
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="tableName"></param>
        /// <param name="phoneCol"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> DelSameTelForImportOtherData(long projectid, string tableName, string phoneCol, string[] rangeid, BlackListCheckType blacklistchecktype, Action<string> logFunc)
        {
            logFunc("数据校验：tableName=" + tableName + " phoneCol=" + phoneCol +
                " rangeid[0]=" + rangeid[0] + " rangeid[1]=" + rangeid[1] +
                " projectid=" + projectid + " blacklistchecktype=" + blacklistchecktype);

            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                conn.Open();
                //查找重复的需要删除的数据
                string s_sql = @"SELECT * FROM (
                                        SELECT RecID," + phoneCol + @",row_number() OVER (PARTITION BY " + phoneCol + @" ORDER BY RecID ASC) AS rowid
                                        FROM ( 
                                                SELECT * FROM " + tableName + @" WHERE (RecID IN (SELECT RelationID FROM dbo.ProjectDataSoure WHERE ProjectID=" + projectid + @")) --已有的项目下的数据
                                                UNION ALL
                                                SELECT * FROM " + tableName + @" WHERE (RecID>=" + rangeid[0] + @" AND RecID<=" + rangeid[1] + @")--刚刚导入的数据
                                                ) t
                                         ) tmp WHERE tmp.rowid<>1 And ISNULL(tmp." + phoneCol + @",'')<>''
                                        ORDER BY tmp.RecID";
                DataTable dt = SqlHelper.ExecuteDataset(conn, CommandType.Text, s_sql).Tables[0];
                List<string> delids = CommonFunction.DataTableToList<string>(dt, "RecID", "RecID", "");
                dic.Add("重复数据", delids);
                logFunc("重复数据校验耗时：" + sw.Elapsed.ToString());
                sw.Reset();
                sw.Start();

                //校验项目验证方式
                if ((blacklistchecktype & BlackListCheckType.BT2_CC) == BlackListCheckType.BT2_CC)
                {
                    s_sql = @"SELECT RecID FROM " + tableName + @"
                                    WHERE " + phoneCol + @" IN (
                                        SELECT PhoneNum FROM dbo.BlackWhiteList WHERE Status=0 AND Type=0)
                                    AND RecID>=" + rangeid[0] + @" 
                                    AND RecID<=" + rangeid[1];
                    dt = SqlHelper.ExecuteDataset(conn, CommandType.Text, s_sql).Tables[0];
                    List<string> ccdel = CommonFunction.DataTableToList<string>(dt, "RecID", "RecID", "");
                    dic.Add("CC黑名单", ccdel);
                    logFunc("CC黑名单校验耗时：" + sw.Elapsed.ToString());
                    sw.Reset();
                    sw.Start();
                }

                if ((blacklistchecktype & BlackListCheckType.BT1_CRM) == BlackListCheckType.BT1_CRM)
                {
                    string s_tmp = @"SELECT OfficeTel as phone
                                            INTO #tmp1
                                                FROM (
                                                SELECT REPLACE(OfficeTel,'-','') AS OfficeTel,1 AS typeid
                                                FROM CRM2009.dbo.ContactInfo 
                                                WHERE Status=0 AND ISNULL(OfficeTel,'')<>'' AND ISNUMERIC(REPLACE(OfficeTel,'-',''))=1
                                                UNION 
                                                SELECT REPLACE(Phone,'-','') AS Phone,2 AS typeid
                                                FROM CRM2009.dbo.ContactInfo
                                                WHERE Status=0 AND ISNULL(Phone,'')<>'' AND ISNUMERIC(REPLACE(Phone,'-',''))=1
                                                UNION 
                                                SELECT REPLACE(Phone,'-','') AS Phone,3 AS typeid
                                                FROM CRM2009.dbo.DMSMember
                                                WHERE Status=0 AND ISNULL(Phone,'')<>'' AND ISNUMERIC(REPLACE(Phone,'-',''))=1
                                                UNION 
                                                SELECT REPLACE(OfficeTel,'-','') AS OfficeTel,4 AS typeid
                                                FROM CRM2009.dbo.CustInfo
                                                WHERE Status=0 AND ISNULL(Officetel,'')<>'' AND ISNUMERIC(REPLACE(OfficeTel,'-',''))=1
                                                ) tmp";
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, s_tmp);

                    s_sql = @"SELECT RecID 
                                FROM " + tableName + @"
                                INNER JOIN #tmp1 ON " + phoneCol + @"=#tmp1.phone
                                WHERE  RecID>=" + rangeid[0] + " AND RecID<=" + rangeid[1];

                    dt = SqlHelper.ExecuteDataset(conn, CommandType.Text, s_sql).Tables[0];
                    List<string> crmdel = CommonFunction.DataTableToList<string>(dt, "RecID", "RecID", "");
                    dic.Add("CRM黑名单", crmdel);
                    logFunc("CRM黑名单校验耗时：" + sw.Elapsed.ToString());
                    sw.Reset();
                    sw.Start();
                }
                sw.Stop();
            }
            //返回结果
            return dic;
        }
        /// 获取需要结束的任务
        /// <summary>
        /// 获取需要结束的任务
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public DataTable GetStopForOtherTaskInfoByList(int ProjectID)
        {
            string sql = "SELECT PTID FROM OtherTaskInfo WHERE Status=0 AND TaskStatus Not IN (4,5) AND ProjectID=" + ProjectID;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}

