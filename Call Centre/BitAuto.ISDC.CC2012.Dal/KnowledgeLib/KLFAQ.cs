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
    /// 数据访问类KLFAQ。
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
    public class KLFAQ : DataBase
    {
        #region Instance
        public static readonly KLFAQ Instance = new KLFAQ();
        #endregion

        #region const
        private const string P_KLFAQ_SELECT = "p_KLFAQ_Select";
        private const string P_KLFAQ_INSERT = "p_KLFAQ_Insert";
        private const string P_KLFAQ_UPDATE = "p_KLFAQ_Update";
        private const string P_KLFAQ_DELETE = "p_KLFAQ_Delete";
        #endregion

        #region Contructor
        protected KLFAQ()
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
        public DataTable GetKLFAQ(QueryKLFAQ query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.KLFAQID != Constant.INT_INVALID_VALUE)
            {
                where += " and KLFAQ.KLFAQID=" + query.KLFAQID.ToString();
            }
            if (query.KLID != Constant.INT_INVALID_VALUE)
            {
                where += " and KLFAQ.KLID=" + query.KLID.ToString();
            }
            if (query.KCIDs != Constant.STRING_INVALID_VALUE)
            {
                where += " and KnowLedgeLib.KCID in (select ID from f_Cid(" + Dal.Util.SqlFilterByInCondition(query.KCIDs) + "))";
            }
            if (query.Keywords != Constant.STRING_INVALID_VALUE)
            {
                where += " and (KLFAQ.Ask like '%" + StringHelper.SqlFilter(query.Keywords) + "%' or KLFAQ.Question like '%" + StringHelper.SqlFilter(query.Keywords) + "%')";
            }
            if (query.State != Constant.STRING_INVALID_VALUE)
            {
                where += " and KnowLedgeLib.Status=" + StringHelper.SqlFilter(query.State);
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLFAQ_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataSet GetKLFAQReport(int UserId, string order, string where, int currentPage, int pageSize)
        {
            var paras = new SqlParameter[]
            {                
                new SqlParameter("@userid",UserId), 
                new SqlParameter("@order",order), 
                new SqlParameter("@where",where), 
                new SqlParameter("@pageIndex",currentPage), 
                new SqlParameter("@pageSize",pageSize),                
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_GetKLFAQReport",
                paras);
            return ds;
        }


        public DataTable GetKLFAQForManage(QueryKnowledgeLib query, string order, int currentPage, int pageSize, string wherePlug, out int totalCount)
        {

            string where = wherePlug;


            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.Ask like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:00'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            if (query.MBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.ModifyTime>='" + StringHelper.SqlFilter(query.MBeginTime) + " 0:00:00'";
            }
            if (query.MEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.ModifyTime<='" + StringHelper.SqlFilter(query.MEndTime) + " 23:59:59'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CreateUserID=" + query.CreateUserID;
            }
            if (query.LastModifyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.ModifyUserid=" + query.LastModifyUserID;
            }
            if (query.StatusS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND (CASE when b.Status =5 THEN a.STATUS ELSE b.STATUS END) IN (" + Dal.Util.SqlFilterByInCondition(query.StatusS) + ")";
            }
            //if (query.StatusS != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " AND a.Status IN (" + query.StatusS + ")";
            //}




            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@kcid", query.KCID),
					new SqlParameter("@where", where),
					new SqlParameter("@order", order),
					new SqlParameter("@pagesize", pageSize),
					new SqlParameter("@pageIndex", currentPage),
					new SqlParameter("@totalCount", SqlDbType.Int, 4)
					};


            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_KLFAQ_Manage", parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照查询条件查询(供知识库管理列表页使用 刘学文)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetKLFAQ(QueryKnowledgeLib query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where = Dal.KnowledgeLib.Instance.getCommonWhere(2, query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLFAQ_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        #region 判断一个知识点下是否有FAQ
        /// <summary>
        /// 判断一个知识点下是否有ＦＡＱ
        /// </summary>
        ///
        /// <returns>BooL</returns>
        public bool IsHaveFAQ(string knoledgeID)
        {
            QueryKLFAQ query = new QueryKLFAQ();
            int totalCount = 0;
            string where = " and KLFAQ.KLID=" + StringHelper.SqlFilter(knoledgeID);

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = "";
            parameters[2].Value = 10;
            parameters[3].Value = 1;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLFAQ_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            if (ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KLFAQ GetKLFAQ(long KLFAQID)
        {
            QueryKLFAQ query = new QueryKLFAQ();
            query.KLFAQID = KLFAQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLFAQ(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleKLFAQ(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.KLFAQ LoadSingleKLFAQ(DataRow row)
        {
            Entities.KLFAQ model = new Entities.KLFAQ();

            if (row["KLFAQID"].ToString() != "")
            {
                model.KLFAQID = long.Parse(row["KLFAQID"].ToString());
            }
            if (row["KLID"].ToString() != "")
            {
                model.KLID = long.Parse(row["KLID"].ToString());
            }
            model.Ask = row["Ask"].ToString();
            model.Question = row["Question"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.KLFAQ model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLFAQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Question", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.Ask;
            parameters[3].Value = model.Question;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLFAQ_INSERT, parameters);
            return Convert.ToInt32(parameters[0].Value);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLFAQ model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLFAQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Question", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.Ask;
            parameters[3].Value = model.Question;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;
            parameters[8].Value = model.KCID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLFAQ_INSERT, parameters);
            return 1;//(int)parameters[0].Value
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.KLFAQ model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLFAQID", SqlDbType.BigInt,8),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Question", SqlDbType.NVarChar,2000),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.KLFAQID;
            parameters[1].Value = model.Ask;
            parameters[2].Value = model.Question;
            parameters[3].Value = model.ModifyTime;
            parameters[4].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLFAQ_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLFAQ model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLFAQID", SqlDbType.BigInt,8),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Question", SqlDbType.NVarChar,2000),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.KLFAQID;
            parameters[1].Value = model.Ask;
            parameters[2].Value = model.Question;
            parameters[3].Value = model.ModifyTime;
            parameters[4].Value = model.ModifyUserID;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLFAQ_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long KLFAQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLFAQID", SqlDbType.BigInt)};
            parameters[0].Value = KLFAQID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLFAQ_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLFAQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLFAQID", SqlDbType.Int)};
            parameters[0].Value = KLFAQID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLFAQ_DELETE, parameters);
        }
        /// <summary>
        /// 通过KLID知识点ID删除一条数据
        /// </summary>
        public int DeleteByKLID(SqlTransaction sqltran, long KLID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLID", SqlDbType.Int)};
            parameters[0].Value = KLID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "P_KLFAQ_DELETEByKLID", parameters);
        }
        #endregion


    }
}

