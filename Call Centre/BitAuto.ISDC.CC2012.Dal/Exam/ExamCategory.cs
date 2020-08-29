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
    /// 数据访问类ExamCategory。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:16 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamCategory : DataBase
    {
        #region Instance
        public static readonly ExamCategory Instance = new ExamCategory();
        #endregion

        #region const
        private const string P_EXAMCATEGORY_SELECT = "p_ExamCategory_Select";
        private const string P_EXAMCATEGORY_INSERT = "p_ExamCategory_Insert";
        private const string P_EXAMCATEGORY_UPDATE = "p_ExamCategory_Update";
        private const string P_EXAMCATEGORY_DELETE = "p_ExamCategory_Delete";
        #endregion

        #region Contructor
        protected ExamCategory()
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
        public DataTable GetExamCategory(QueryExamCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " and Type=" + query.Type.ToString();
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " and Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.ECID != Constant.INT_INVALID_VALUE)
            {
                where += " and  ECID =" + query.ECID + "";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamCategory GetExamCategory(int ECID)
        {
            QueryExamCategory query = new QueryExamCategory();
            query.ECID = ECID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamCategory(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamCategory(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamCategory LoadSingleExamCategory(DataRow row)
        {
            Entities.ExamCategory model = new Entities.ExamCategory();

            if (row["ECID"].ToString() != "")
            {
                model.ECID = int.Parse(row["ECID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["Type"].ToString() != "")
            {
                model.Type = int.Parse(row["Type"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
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
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ExamCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.ECID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ECID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.ECID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int ECID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ECID", SqlDbType.Int,4)};
            parameters[0].Value = ECID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int ECID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ECID", SqlDbType.Int,4)};
            parameters[0].Value = ECID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMCATEGORY_DELETE, parameters);
        }
        #endregion

    }
}

