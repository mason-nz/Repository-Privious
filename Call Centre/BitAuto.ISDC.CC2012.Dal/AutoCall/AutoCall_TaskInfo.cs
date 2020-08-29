using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类AutoCall_TaskInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-09-14 09:57:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AutoCall_TaskInfo : DataBase
    {
        #region Instance
        public static readonly AutoCall_TaskInfo Instance = new AutoCall_TaskInfo();
        #endregion

        #region const
        private const string P_AUTOCALL_TASKINFO_SELECT = "p_AutoCall_TaskInfo_Select";
        private const string P_AUTOCALL_TASKINFO_INSERT = "p_AutoCall_TaskInfo_Insert";
        private const string P_AUTOCALL_TASKINFO_DELETE = "p_AutoCall_TaskInfo_Delete";
        #endregion

        #region Contructor
        protected AutoCall_TaskInfo()
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
        public DataTable GetAutoCall_TaskInfo(QueryAutoCall_TaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_TASKINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.AutoCall_TaskInfo GetAutoCall_TaskInfo(int ACTID)
        {
            QueryAutoCall_TaskInfo query = new QueryAutoCall_TaskInfo();
            query.ACTID = ACTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAutoCall_TaskInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleAutoCall_TaskInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.AutoCall_TaskInfo LoadSingleAutoCall_TaskInfo(DataRow row)
        {
            Entities.AutoCall_TaskInfo model = new Entities.AutoCall_TaskInfo();

            if (row["ACTID"].ToString() != "")
            {
                model.ACTID = int.Parse(row["ACTID"].ToString());
            }
            if (row["BusinessRecID"].ToString() != "")
            {
                model.BusinessRecID = int.Parse(row["BusinessRecID"].ToString());
            }
            model.BusinessID = row["BusinessID"].ToString();
            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            model.Phone = row["Phone"].ToString();
            model.PhonePrefix = row["PhonePrefix"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            //model.Timestamp=row["Timestamp"].ToString();
            if (row["ACStatus"].ToString() != "")
            {
                model.ACStatus = int.Parse(row["ACStatus"].ToString());
            }
            if (row["ServiceTakeTime"].ToString() != "")
            {
                model.ServiceTakeTime = DateTime.Parse(row["ServiceTakeTime"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.AutoCall_TaskInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ACTID", SqlDbType.Int,4),
					new SqlParameter("@BusinessRecID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@PhonePrefix", SqlDbType.VarChar,10),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					//new SqlParameter("@Timestamp", SqlDbType.Timestamp,8),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@ServiceTakeTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BusinessRecID;
            parameters[2].Value = model.BusinessID;
            parameters[3].Value = model.ProjectID;
            parameters[4].Value = model.Phone;
            parameters[5].Value = model.PhonePrefix;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;
            //parameters[8].Value = model.Timestamp;
            parameters[8].Value = model.ACStatus;
            parameters[9].Value = model.ServiceTakeTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_TASKINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.AutoCall_TaskInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ACTID", SqlDbType.Int,4),
					new SqlParameter("@BusinessRecID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
					new SqlParameter("@PhonePrefix", SqlDbType.VarChar,10),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					//new SqlParameter("@Timestamp", SqlDbType.Timestamp,8),
					new SqlParameter("@ACStatus", SqlDbType.Int,4),
					new SqlParameter("@ServiceTakeTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BusinessRecID;
            parameters[2].Value = model.BusinessID;
            parameters[3].Value = model.ProjectID;
            parameters[4].Value = model.Phone;
            parameters[5].Value = model.PhonePrefix;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;
            //parameters[8].Value = model.Timestamp;
            parameters[8].Value = model.ACStatus;
            parameters[9].Value = model.ServiceTakeTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_TASKINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int ACTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ACTID", SqlDbType.Int,4)};
            parameters[0].Value = ACTID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AUTOCALL_TASKINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int ACTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ACTID", SqlDbType.Int,4)};
            parameters[0].Value = ACTID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_AUTOCALL_TASKINFO_DELETE, parameters);
        }
        #endregion

        /// 根据项目导入任务到自动外呼任务表中
        /// <summary>
        /// 根据项目导入任务到自动外呼任务表中
        /// </summary>
        public int AutoCallTaskInfoUpdate(long projectid, int userid)
        {
            SqlParameter[] parameters = { 
                                        new SqlParameter("@ProjectID", SqlDbType.BigInt) { Value = projectid } ,
                                        new SqlParameter("@UserId", SqlDbType.BigInt) { Value = userid }};

            int a = 0;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "P_AutoCall_TaskInfo_Update";
                cmd.Parameters.AddRange(parameters);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5 * 60;
                conn.Open();
                a = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return a;

            //return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_AutoCall_TaskInfo_Update", parameters);
        }
        /// 根据手机号码查询当前时间正在通话的任务ID
        /// <summary>
        /// 根据手机号码查询当前时间正在通话的任务ID
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string GetCurrentTaskIDByPhone(string phone)
        {
            //查询锁定时间在5分钟内，且电话相等的任务
            string sql = @"SELECT TOP 1 BusinessID FROM dbo.AutoCall_TaskInfo
                                    WHERE Phone='" + phone + @"'
                                    AND DATEDIFF(mi, ServiceTakeTime ,GETDATE())<5
                                    ORDER BY ServiceTakeTime DESC";
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sql));
        }
    }
}

