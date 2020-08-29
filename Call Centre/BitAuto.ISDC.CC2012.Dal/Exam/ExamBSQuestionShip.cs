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
    /// 数据访问类ExamBSQuestionShip。
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
    public class ExamBSQuestionShip : DataBase
    {
        #region Instance
        public static readonly ExamBSQuestionShip Instance = new ExamBSQuestionShip();
        #endregion

        #region const
        private const string P_EXAMBSQUESTIONSHIP_SELECT = "p_ExamBSQuestionShip_Select";
        private const string P_EXAMBSQUESTIONSHIP_INSERT = "p_ExamBSQuestionShip_Insert";
        private const string P_EXAMBSQUESTIONSHIP_UPDATE = "p_ExamBSQuestionShip_Update";
        private const string P_EXAMBSQUESTIONSHIP_DELETE = "p_ExamBSQuestionShip_Delete";
        #endregion

        #region Contructor
        protected ExamBSQuestionShip()
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
        public DataTable GetExamBSQuestionShip(QueryExamBSQuestionShip query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.BQID != Constant.INT_INVALID_VALUE)
            {
                where += " and BQID =" + query.BQID;
            }
            if (query.KLQID != Constant.INT_INVALID_VALUE)
            {
                where += " and KLQID =" + query.KLQID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamBSQuestionShip GetExamBSQuestionShip(long BQID, long KLQID)
        {
            QueryExamBSQuestionShip query = new QueryExamBSQuestionShip();
            query.BQID = BQID;
            query.KLQID = KLQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBSQuestionShip(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamBSQuestionShip(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamBSQuestionShip LoadSingleExamBSQuestionShip(DataRow row)
        {
            Entities.ExamBSQuestionShip model = new Entities.ExamBSQuestionShip();

            if (row["BQID"].ToString() != "")
            {
                model.BQID = long.Parse(row["BQID"].ToString());
            }
            if (row["KLQID"].ToString() != "")
            {
                model.KLQID = long.Parse(row["KLQID"].ToString());
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
        public void Insert(Entities.ExamBSQuestionShip model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.BQID;
            parameters[1].Value = model.KLQID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.ExamBSQuestionShip model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.Int,8),
					new SqlParameter("@KLQID", SqlDbType.Int,8),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.BQID;
            parameters[1].Value = model.KLQID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamBSQuestionShip model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.BQID;
            parameters[1].Value = model.KLQID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamBSQuestionShip model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.BQID;
            parameters[1].Value = model.KLQID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long BQID, long KLQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt),
					new SqlParameter("@KLQID", SqlDbType.BigInt)};
            parameters[0].Value = BQID;
            parameters[1].Value = KLQID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_DELETE, parameters);
        }

        public void Delete(long BQID)
        {
            string strSQL = "DELETE dbo.ExamBSQuestionShip WHERE BQID="+BQID.ToString();

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSQL);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long BQID, long KLQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BQID", SqlDbType.BigInt),
					new SqlParameter("@KLQID", SqlDbType.BigInt)};
            parameters[0].Value = BQID;
            parameters[1].Value = KLQID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMBSQUESTIONSHIP_DELETE, parameters);
        }
        #endregion


        /// <summary>
        /// 根据大题id得到一个对象实体List add by qizq 2012-9-3
        /// </summary>
        public List<Entities.ExamBSQuestionShip> GetExamBSQuestionShipList(long BQID)
        {
            List<Entities.ExamBSQuestionShip> ExamBSQuestionShipList = null;
            QueryExamBSQuestionShip query = new QueryExamBSQuestionShip();
            query.BQID = BQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamBSQuestionShip(query, string.Empty, 1, 1000000, out count);
            if (count > 0)
            {
                ExamBSQuestionShipList = new List<Entities.ExamBSQuestionShip>();
                for (int i = 0; i < count; i++)
                {
                    ExamBSQuestionShipList.Add(LoadSingleExamBSQuestionShip(dt.Rows[i]));
                }
                return ExamBSQuestionShipList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据大题id得到一个知识库试题DataTable add by qizq 2012-9-3
        /// </summary>
        public DataTable GetKLQuestionData(long BQID)
        {
            string sqlStr = "SELECT *,'" + BQID.ToString() + "' as 'BQID' FROM KLQuestion WHERE klqid in (select klqid from ExamBsquestionship where  BQID=@BQID)";
            SqlParameter parameter = new SqlParameter("@BQID", BQID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }

       
    }
}

