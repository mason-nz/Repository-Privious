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
    /// 数据访问类SurveyOptionSkipQuestion。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-11-30 02:12:02 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyOptionSkipQuestion : DataBase
    {
        #region Instance
        public static readonly SurveyOptionSkipQuestion Instance = new SurveyOptionSkipQuestion();
        #endregion

        #region const
        private const string P_SURVEYOPTIONSKIPQUESTION_SELECT = "p_SurveyOptionSkipQuestion_Select";
        private const string P_SURVEYOPTIONSKIPQUESTION_INSERT = "p_SurveyOptionSkipQuestion_Insert";
        private const string P_SURVEYOPTIONSKIPQUESTION_UPDATE = "p_SurveyOptionSkipQuestion_Update";
        private const string P_SURVEYOPTIONSKIPQUESTION_DELETE = "p_SurveyOptionSkipQuestion_Delete";
        #endregion

        #region Contructor
        protected SurveyOptionSkipQuestion()
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
        public DataTable GetSurveyOptionSkipQuestion(QuerySurveyOptionSkipQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and RecID=" + query.RecID;
            }
            if (query.SOID != Constant.INT_INVALID_VALUE)
            {
                where += " and SOID=" + query.SOID;
            }
            if (query.SQID != Constant.INT_INVALID_VALUE)
            {
                where += " and SQID=" + query.SQID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyOptionSkipQuestion GetSurveyOptionSkipQuestion(int RecID)
        {
            QuerySurveyOptionSkipQuestion query = new QuerySurveyOptionSkipQuestion();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyOptionSkipQuestion(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyOptionSkipQuestion(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyOptionSkipQuestion LoadSingleSurveyOptionSkipQuestion(DataRow row)
        {
            Entities.SurveyOptionSkipQuestion model = new Entities.SurveyOptionSkipQuestion();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["SOID"].ToString() != "")
            {
                model.SOID = int.Parse(row["SOID"].ToString());
            }
            if (row["SQID"].ToString() != "")
            {
                model.SQID = int.Parse(row["SQID"].ToString());
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
        public int Insert(Entities.SurveyOptionSkipQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SOID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyOptionSkipQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SOID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SurveyOptionSkipQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SOID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyOptionSkipQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SOID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYOPTIONSKIPQUESTION_DELETE, parameters);
        }
        #endregion


        public Entities.SurveyOptionSkipQuestion GetModelBySoid(int Soid)
        {
            Entities.SurveyOptionSkipQuestion retModel = null;
            Entities.QuerySurveyOptionSkipQuestion query = new Entities.QuerySurveyOptionSkipQuestion();
            query.SOID = Soid;
            int totalCount = 0;
            DataTable dt = GetSurveyOptionSkipQuestion(query, "", 1, 999, out totalCount);

            if (dt != null && dt.Rows.Count > 0)
            {
                retModel = LoadSingleSurveyOptionSkipQuestion(dt.Rows[0]);
            }

            return retModel;
        }

        /// <summary>
        /// 根据所要跳的题取所有选项跳题信息
        /// </summary>
        /// <param name="dtquestion"></param>
        /// <returns></returns>
        public DataTable GetAllOptionSkip(DataTable dtquestion)
        {
            DataTable dt = null;
            string where = string.Empty;
            if (dtquestion != null && dtquestion.Rows.Count > 0)
            {
                where += "(";
                for (int i = 0; i < dtquestion.Rows.Count; i++)
                {
                    where += dtquestion.Rows[i]["SQID"].ToString() + ",";
                }
                where = where.Substring(0, where.Length - 1);
                where += ")";
                string sqlstr = "SELECT * FROM [SurveyOptionSkipQuestion] where [SQID] in " + where + " and status=0";
                dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            }
            return dt;
        }
    }
}

