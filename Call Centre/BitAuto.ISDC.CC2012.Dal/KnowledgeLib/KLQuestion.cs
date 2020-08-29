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
    /// 数据访问类KLQuestion。
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
    public class KLQuestion : DataBase
    {
        #region Instance
        public static readonly KLQuestion Instance = new KLQuestion();
        #endregion

        #region const
        private const string P_KLQUESTION_SELECT = "p_KLQuestion_Select";
        private const string P_KLQUESTION_INSERT = "p_KLQuestion_Insert";
        private const string P_KLQUESTION_UPDATE = "p_KLQuestion_Update";
        private const string P_KLQUESTION_DELETE = "p_KLQuestion_Delete";
        private const string P_KLQUESTION_SELECTBYIDS = "p_KLQuestion_SelectByIDs";

        #endregion

        #region Contructor
        protected KLQuestion()
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
        public DataTable GetKLQuestion(QueryKLQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.KLID != Constant.INT_INVALID_VALUE)
            {
                where += " And KLQuestion.KLID=" + query.KLID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQUESTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
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
        public DataTable GetKLQuestion(QueryKnowledgeLib query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where = Dal.KnowledgeLib.Instance.getCommonWhere(3, query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQUESTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照查询条件查询(供知识库管理列表页使用 刘学文 10.24)
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetKLQuestionMnage(QueryKLQuestion query, string order, int currentPage, int pageSize, string wherePlus, out int totalCount)
        {
            string where = wherePlus;


            if (query.Ask != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.Ask like '%" + StringHelper.SqlFilter(query.Ask) + "%'";
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
            if (query.ModifyUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.ModifyUserid=" + query.ModifyUserID;
            }
            if (query.StatusS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND (CASE when b.Status =5 THEN a.STATUS ELSE b.STATUS END) IN (" + Dal.Util.SqlFilterByInCondition(query.StatusS) + ")";
            }
            if (query.AskCategorys != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.askcategory IN (" + Dal.Util.SqlFilterByInCondition(query.AskCategorys) + ")";
            }




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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_KLQuestion_Manage", parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];

        }

        public string getQuestionWhere(QueryKLQuestion query)
        {
            string where = string.Empty;

            //问题 标题
            if (query.Ask != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KLQuestion.Ask like '%" + StringHelper.SqlFilter(query.Ask) + "%'";
            }
            //知识点状态 
            if (query.StatusS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KnowledgeLib.Status IN (" + Dal.Util.SqlFilterByInCondition(query.StatusS) + ")";
            }
            //创建时间
            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KLQuestion.CreateTime>='" + StringHelper.SqlFilter(query.BeginTime) + " 0:00:00'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KLQuestion.CreateTime<='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }
            //创建人
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KLQuestion.CreateUserID=" + query.CreateUserID;
            }
            //试题状态
            if (query.QuestionStatusS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND KLQuestion.Status IN (" + Dal.Util.SqlFilterByInCondition(query.QuestionStatusS) + ")";
            }
            //KCID
            if (query.KCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KnowLedgeLib.KCID=" + query.KCID;
            }
            return where;
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.KLQuestion GetKLQuestion(long KLQID)
        {
            string sqlStr = "SELECT * FROM KLQuestion WHERE KLQID=@KLQID";
            SqlParameter parameter = new SqlParameter("@KLQID", KLQID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleKLQuestion(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.KLQuestion LoadSingleKLQuestion(DataRow row)
        {
            Entities.KLQuestion model = new Entities.KLQuestion();

            if (row["KLQID"].ToString() != "")
            {
                model.KLQID = long.Parse(row["KLQID"].ToString());
            }
            if (row["KLID"].ToString() != "")
            {
                model.KLID = long.Parse(row["KLID"].ToString());
            }
            if (row["AskCategory"].ToString() != "")
            {
                model.AskCategory = int.Parse(row["AskCategory"].ToString());
            }
            model.Ask = row["Ask"].ToString();
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
        public int Insert(Entities.KLQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.AskCategory;
            parameters[3].Value = model.Ask;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.Int,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),new SqlParameter("@KCID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.AskCategory;
            parameters[3].Value = model.Ask;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;
            parameters[9].Value = model.KCID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.KLQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.KLQID;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.AskCategory;
            parameters[3].Value = model.Ask;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQUESTION_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLID", SqlDbType.BigInt,8),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.KLQID;
            parameters[1].Value = model.KLID;
            parameters[2].Value = model.AskCategory;
            parameters[3].Value = model.Ask;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ModifyTime;
            parameters[8].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLQUESTION_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long KLQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt)};
            parameters[0].Value = KLQID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQUESTION_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long KLQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@KLQID", SqlDbType.BigInt)};
            parameters[0].Value = KLQID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_KLQUESTION_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 此试题是否已经使用
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public bool IsUsed(long KLQID)
        {
            string sqlStr = "SELECT * FROM ExamOnlineDetail AS eod JOIN ExamOnlineAnswer As eol ON eod.EOLDID=eol.EOLDID";
            sqlStr += " WHERE KLQID=@KLQID";
            SqlParameter parameter = new SqlParameter("@KLQID", KLQID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据试题IDs获取试题
        /// </summary>
        /// <param name="SmallQIDs"></param>
        /// <param name="QustionType"></param>
        /// <param name="order"></param>
        /// <param name="p_2"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQuestionByIDs(string KCID, string QustionName, string SmallQIDs, string QustionType, string order, int currentPage, int pageSize, int loginUser, out int totalCount)
        {
            string where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("k", "BGID", "CreateUserID", loginUser);
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@smallqids", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@type", SqlDbType.NVarChar, 200),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
                    new SqlParameter("@kcid", SqlDbType.NVarChar, 50),
					new SqlParameter("@qustionname", SqlDbType.NVarChar, 200),
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = SmallQIDs;
            parameters[1].Value = QustionType;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Value = KCID;
            parameters[6].Value = QustionName;
            parameters[7].Value = where;
            parameters[8].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_KLQUESTION_SELECTBYIDS, parameters);
            totalCount = (int)(parameters[8].Value);
            return ds.Tables[0];
        }
    }
}

