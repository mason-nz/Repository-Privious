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
    /// 数据访问类CustLastOperTask。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-23 10:59:12 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustLastOperTask : DataBase
    {
        public static readonly CustLastOperTask Instance = new CustLastOperTask();

        private const string P_CUSTLASTOPERTASK_SELECT = "p_CustLastOperTask_Select";
        private const string P_CUSTLASTOPERTASK_INSERT = "p_CustLastOperTask_Insert";
        private const string P_CUSTLASTOPERTASK_UPDATE = "p_CustLastOperTask_Update";

        protected CustLastOperTask()
        { }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCustLastOperTask(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            //string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTLASTOPERTASK_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustLastOperTask GetCustLastOperTask(string CustID)
        {
            string where = string.Format(" And CustID='{0}'", StringHelper.SqlFilter(CustID));
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustLastOperTask(where, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCustLastOperTask(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CustLastOperTask LoadSingleCustLastOperTask(DataRow row)
        {
            Entities.CustLastOperTask model = new Entities.CustLastOperTask();

            model.CustID = row["CustID"].ToString();
            model.TaskID = row["TaskID"].ToString();
            if (row["TaskType"].ToString() != "")
            {
                model.TaskType = int.Parse(row["TaskType"].ToString());
            }
            if (row["LastOperTime"].ToString() != "")
            {
                model.LastOperTime = DateTime.Parse(row["LastOperTime"].ToString());
            }
            if (row["LastOperUserID"].ToString() != "")
            {
                model.LastOperUserID = int.Parse(row["LastOperUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.CustLastOperTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@TaskType", SqlDbType.Int,4),
					new SqlParameter("@LastOperTime", SqlDbType.DateTime),
					new SqlParameter("@LastOperUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.TaskType;
            parameters[3].Value = model.LastOperTime;
            parameters[4].Value = model.LastOperUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTLASTOPERTASK_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CustLastOperTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@TaskType", SqlDbType.Int,4),
					new SqlParameter("@LastOperTime", SqlDbType.DateTime),
					new SqlParameter("@LastOperUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.TaskType;
            parameters[3].Value = model.LastOperTime;
            parameters[4].Value = model.LastOperUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTLASTOPERTASK_INSERT, parameters);
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CustLastOperTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@TaskType", SqlDbType.Int,4),
					new SqlParameter("@LastOperTime", SqlDbType.DateTime),
					new SqlParameter("@LastOperUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.TaskType;
            parameters[3].Value = model.LastOperTime;
            parameters[4].Value = model.LastOperUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTLASTOPERTASK_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CustLastOperTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@TaskType", SqlDbType.Int,4),
					new SqlParameter("@LastOperTime", SqlDbType.DateTime),
					new SqlParameter("@LastOperUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.TaskType;
            parameters[3].Value = model.LastOperTime;
            parameters[4].Value = model.LastOperUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTLASTOPERTASK_UPDATE, parameters);
        }
    }
}

