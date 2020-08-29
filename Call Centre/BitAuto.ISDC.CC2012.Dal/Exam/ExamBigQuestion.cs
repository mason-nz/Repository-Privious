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
    /// 数据访问类ExamBigQuestion。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:15 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamBigQuestion : DataBase
    {
        #region Instance
        public static readonly ExamBigQuestion Instance = new ExamBigQuestion();
        #endregion

        #region const
        private const string P_EXAMBIGQUESTION_SELECT = "p_ExamBigQuestion_Select";
        private const string P_EXAMBIGQUESTION_INSERT = "p_ExamBigQuestion_Insert";
        private const string P_EXAMBIGQUESTION_UPDATE = "p_ExamBigQuestion_Update";
        private const string P_EXAMBIGQUESTION_DELETE = "p_ExamBigQuestion_Delete";
        #endregion

        #region Contructor
        protected ExamBigQuestion()
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
        public DataTable GetExamBigQuestion(QueryExamBigQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.EPID != Constant.INT_INVALID_VALUE)
            {
                where += " and EPID=" + query.EPID;
            }
            if (query.BQID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.BQID=" + query.BQID;
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " and Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.BQDesc != Constant.STRING_INVALID_VALUE)
            {
                where += " and BQDesc like '%" + StringHelper.SqlFilter(query.BQDesc) + "%'";
            }
            if (query.AskCategory != Constant.INT_INVALID_VALUE)
            {
                where += " and AskCategory =" + query.AskCategory;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Status =" + query.Status;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBIGQUESTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        /// <summary>
        /// 根据试卷ID得到一个对象实体List add by qizq 2012-9-3
        /// </summary>
        public List<Entities.ExamBigQuestion> GetExamBigQuestionList(long EPID)
        {
            List<Entities.ExamBigQuestion> GetExamBigQuestionList = null;
            QueryExamBigQuestion query = new QueryExamBigQuestion();
            query.EPID = EPID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBigQuestion(query, "createtime asc", 1, 1000000, out count);
            if (count > 0)
            {
                GetExamBigQuestionList = new List<Entities.ExamBigQuestion>();
                for (int i = 0; i < count; i++)
                {
                    GetExamBigQuestionList.Add(LoadSingleExamBigQuestion(dt.Rows[i]));
                }
                return GetExamBigQuestionList;
            }
            else
            {
                return null;
            }
        }

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamBigQuestion GetExamBigQuestion(long BQID)
        {
            QueryExamBigQuestion query = new QueryExamBigQuestion();
            query.BQID = BQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBigQuestion(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamBigQuestion(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamBigQuestion LoadSingleExamBigQuestion(DataRow row)
        {
            Entities.ExamBigQuestion model = new Entities.ExamBigQuestion();

            if (row["BQID"].ToString() != "")
            {
                model.BQID = long.Parse(row["BQID"].ToString());
            }
            if (row["EPID"].ToString() != "")
            {
                model.EPID = long.Parse(row["EPID"].ToString());
            }
            model.Name = row["Name"].ToString();
            model.BQDesc = row["BQDesc"].ToString();
            if (row["AskCategory"].ToString() != "")
            {
                model.AskCategory = int.Parse(row["AskCategory"].ToString());
            }
            if (row["EachQuestionScore"].ToString() != "")
            {
                model.EachQuestionScore = int.Parse(row["EachQuestionScore"].ToString());
            }
            if (row["QuestionCount"].ToString() != "")
            {
                model.QuestionCount = int.Parse(row["QuestionCount"].ToString());
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
        public int Insert(Entities.ExamBigQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,300),
					new SqlParameter("@BQDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@EachQuestionScore", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EPID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.BQDesc;
            parameters[4].Value = model.AskCategory;
            parameters[5].Value = model.EachQuestionScore;
            parameters[6].Value = model.QuestionCount;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBIGQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamBigQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.Int,8),
					new SqlParameter("@EPID", SqlDbType.Int,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,300),
					new SqlParameter("@BQDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@EachQuestionScore", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EPID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.BQDesc;
            parameters[4].Value = model.AskCategory;
            parameters[5].Value = model.EachQuestionScore;
            parameters[6].Value = model.QuestionCount;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMBIGQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamBigQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@EPID", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,300),
					new SqlParameter("@BQDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@EachQuestionScore", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.BQID;
            parameters[1].Value = model.EPID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.BQDesc;
            parameters[4].Value = model.AskCategory;
            parameters[5].Value = model.EachQuestionScore;
            parameters[6].Value = model.QuestionCount;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBIGQUESTION_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamBigQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.Int,8),
					new SqlParameter("@EPID", SqlDbType.Int,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,300),
					new SqlParameter("@BQDesc", SqlDbType.NVarChar,500),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@EachQuestionScore", SqlDbType.Int,4),
					new SqlParameter("@QuestionCount", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.BQID;
            parameters[1].Value = model.EPID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.BQDesc;
            parameters[4].Value = model.AskCategory;
            parameters[5].Value = model.EachQuestionScore;
            parameters[6].Value = model.QuestionCount;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMBIGQUESTION_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long BQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt)};
            parameters[0].Value = BQID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBIGQUESTION_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long BQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt)};
            parameters[0].Value = BQID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMBIGQUESTION_DELETE, parameters);
        }
        #endregion

        ///add by qizq 2012-9-11
        /// <summary>
        /// 根据试卷id,题型，判断是否有该题型
        /// </summary>
        /// <returns></returns>
        public DataTable HaveAskCategoryByEPID(string epid, int askcategory)
        {
            QueryExamBigQuestion query = new QueryExamBigQuestion();
            query.EPID = CommonFunction.ObjectToInteger(epid,-1);
            query.AskCategory = askcategory;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBigQuestion(query, "createtime asc", 1, 1000000, out count);
            return dt;
        }
    }
}

