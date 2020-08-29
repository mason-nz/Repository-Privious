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
    /// 数据访问类ExamPerson。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:19 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamPerson : DataBase
    {
        #region Instance
        public static readonly ExamPerson Instance = new ExamPerson();
        #endregion

        #region const
        private const string P_EXAMPERSON_SELECT = "p_ExamPerson_Select";
        private const string P_EXAMPERSON_INSERT = "p_ExamPerson_Insert";
        private const string P_EXAMPERSON_UPDATE = "p_ExamPerson_Update";
        private const string P_EXAMPERSON_DELETE = "p_ExamPerson_Delete";
        #endregion

        #region Contructor
        protected ExamPerson()
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
        public DataTable GetExamPerson(QueryExamPerson query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.EIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EIID=" + query.EIID;
            }
            if (query.MEIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND MEIID=" + query.MEIID;
            }
            if (query.EPID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EPID=" + query.EPID;
            }
            if (query.ExamType != Constant.INT_INVALID_VALUE)
            {
                where += " AND ExamType=" + query.ExamType;
            }
            if (query.ExamPerSonID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ExamPerSonID=" + query.ExamPerSonID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPERSON_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamPerson GetExamPerson(long EPID)
        {
            QueryExamPerson query = new QueryExamPerson();
            query.EPID = EPID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamPerson(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamPerson(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamPerson LoadSingleExamPerson(DataRow row)
        {
            Entities.ExamPerson model = new Entities.ExamPerson();

            if (row["EPID"].ToString() != "")
            {
                model.EPID = long.Parse(row["EPID"].ToString());
            }
            if (row["EIID"].ToString() != "")
            {
                model.EIID = long.Parse(row["EIID"].ToString());
            }
            if (row["MEIID"].ToString() != "")
            {
                model.MEIID = int.Parse(row["MEIID"].ToString());
            }
            if (row["ExamPerSonID"].ToString() != "")
            {
                model.ExamPerSonID = int.Parse(row["ExamPerSonID"].ToString());
            }
            if (row["ExamType"].ToString() != "")
            {
                model.ExamType = int.Parse(row["ExamType"].ToString());
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
        public int Insert(Entities.ExamPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPerSonID", SqlDbType.Int,4),
					new SqlParameter("@ExamType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPerSonID;
            parameters[4].Value = model.ExamType;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPERSON_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPerSonID", SqlDbType.Int,4),
					new SqlParameter("@ExamType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPerSonID;
            parameters[4].Value = model.ExamType;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMPERSON_INSERT, parameters);
            return Convert.ToInt32(parameters[0].Value);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPerSonID", SqlDbType.Int,4),
					new SqlParameter("@ExamType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.EPID;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPerSonID;
            parameters[4].Value = model.ExamType;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPERSON_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.BigInt,8),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPerSonID", SqlDbType.Int,4),
					new SqlParameter("@ExamType", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.EPID;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPerSonID;
            parameters[4].Value = model.ExamType;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPERSON_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long EIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt)};
            parameters[0].Value = EIID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMPERSON_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EIID", SqlDbType.BigInt)};
            parameters[0].Value = EIID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMPERSON_DELETE, parameters);
        }
        #endregion

    }
}

