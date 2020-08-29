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
    /// 数据访问类KLReadTag。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:08 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KLReadTag : DataBase
    {
        #region Instance
        public static readonly KLReadTag Instance = new KLReadTag();
        #endregion

        #region const
        private const string P_KLREADTAG_SELECT = "p_KLReadTag_Select";
        private const string P_KLREADTAG_INSERT = "p_KLReadTag_Insert";
        private const string P_KLREADTAG_UPDATE = "p_KLReadTag_Update";
        private const string P_KLREADTAG_DELETE = "p_KLReadTag_Delete";
        #endregion

        #region Contructor
        protected KLReadTag()
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
        public DataTable GetKLReadTag(QueryKLReadTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.KLID != Constant.INT_INVALID_VALUE)
            {
                where += " AND  KLID =" + query.KLID + "";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND  UserID =" + query.UserID + "";
            }
            if (query.ReadTag != Constant.INT_INVALID_VALUE)
            {
                where += " AND  ReadTag =" + query.ReadTag + "";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLREADTAG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KLReadTag GetKLReadTag(long RecID)
        {
            QueryKLReadTag query = new QueryKLReadTag();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLReadTag(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleKLReadTag(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.KLReadTag LoadSingleKLReadTag(DataRow row)
        {
            Entities.KLReadTag model = new Entities.KLReadTag();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["KLID"].ToString() != "")
            {
                model.KLID = long.Parse(row["KLID"].ToString());
            }
            if (row["UserID"].ToString() != "")
            {
                model.UserID = int.Parse(row["UserID"].ToString());
            }
            if (row["ReadTag"].ToString() != "")
            {
                model.ReadTag = int.Parse(row["ReadTag"].ToString());
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
        public int Insert(Entities.KLReadTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@ReadTag", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.ReadTag;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLREADTAG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLReadTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.Int,8),
					new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@ReadTag", SqlDbType.Int),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.ReadTag;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLREADTAG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.KLReadTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@ReadTag", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.ReadTag;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLREADTAG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLReadTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@ReadTag", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.ReadTag;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLREADTAG_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLREADTAG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLREADTAG_DELETE, parameters);
        }
        /// <summary>
        /// 根据UserID和KLID删除相关数据
        /// </summary>
        public int DeleteByUserID(SqlTransaction sqltran, int UserID, long klid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@KLID", SqlDbType.BigInt)};
            parameters[0].Value = UserID;
            parameters[1].Value = klid;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "P_KLREADTAG_DELETEByUserID", parameters);
        }
        #endregion

        /// <summary>
        /// 根据知识点ID，更新阅读标记为"未读"
        /// </summary>
        /// <param name="sqltran"></param>
        /// <param name="KLID"></param>
        /// <returns></returns>
        public int UpdateTagByKLID(SqlTransaction sqltran, long KLID, int Tag)
        {
            string sqlStr = "UPDATE dbo.KLReadTag SET ReadTag=" + Tag + " WHERE KLID=" + KLID + "";
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sqlStr);

        }

        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <param name="kid">知识点ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="tag">1 已读  0未读</param>
        public int ModifyReadTag(int kid, int userID, int tag)
        {
            string sqlStr = "UPDATE dbo.KLReadTag SET ReadTag=" + tag + " WHERE KLID=" + kid + " and UserID=" + userID + "";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }
    }
}

