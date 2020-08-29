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
    /// 数据访问类ExamOnlineAnswer。
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
    public class ExamOnlineAnswer : DataBase
    {
        #region Instance
        public static readonly ExamOnlineAnswer Instance = new ExamOnlineAnswer();
        #endregion

        #region const
        private const string P_EXAMONLINEANSWER_SELECT = "p_ExamOnlineAnswer_Select";
        private const string P_EXAMONLINEANSWER_INSERT = "p_ExamOnlineAnswer_Insert";
        private const string P_EXAMONLINEANSWER_UPDATE = "p_ExamOnlineAnswer_Update";
        private const string P_EXAMONLINEANSWER_DELETE = "p_ExamOnlineAnswer_Delete";
        #endregion

        #region Contructor
        protected ExamOnlineAnswer()
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
        public DataTable GetExamOnlineAnswer(QueryExamOnlineAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID;
            }
            if (query.EOLDID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EOLDID=" + query.EOLDID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEANSWER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamOnlineAnswer GetExamOnlineAnswer(long RecID)
        {
            QueryExamOnlineAnswer query = new QueryExamOnlineAnswer();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamOnlineAnswer(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamOnlineAnswer(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamOnlineAnswer LoadSingleExamOnlineAnswer(DataRow row)
        {
            Entities.ExamOnlineAnswer model = new Entities.ExamOnlineAnswer();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["EOLDID"].ToString() != "")
            {
                model.EOLDID = int.Parse(row["EOLDID"].ToString());
            }
            model.ExamAnswer = row["ExamAnswer"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreaetUserID"].ToString() != "")
            {
                model.CreaetUserID = int.Parse(row["CreaetUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ExamOnlineAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLDID", SqlDbType.Int,4),
					new SqlParameter("@ExamAnswer", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EOLDID;
            parameters[2].Value = model.ExamAnswer;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreaetUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEANSWER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLDID", SqlDbType.Int,4),
					new SqlParameter("@ExamAnswer", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EOLDID;
            parameters[2].Value = model.ExamAnswer;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreaetUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINEANSWER_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamOnlineAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLDID", SqlDbType.Int,4),
					new SqlParameter("@ExamAnswer", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.EOLDID;
            parameters[2].Value = model.ExamAnswer;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreaetUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEANSWER_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLDID", SqlDbType.Int,4),
					new SqlParameter("@ExamAnswer", SqlDbType.NVarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.EOLDID;
            parameters[2].Value = model.ExamAnswer;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreaetUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEANSWER_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEANSWER_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINEANSWER_DELETE, parameters);
        }

        #endregion

        #region add by qizq 在线考试
        /// <summary>
        /// 取选择的选项
        /// </summary>
        /// <returns></returns>
        public DataTable GetSelected(string EIID, string Type, string Personid, string BQID, string KLQID)
        {

            string sqlstr = "select * from ExamOnlineAnswer where EOLDID in (select EOLDID from ExamOnlineDetail where BQID='" + StringHelper.SqlFilter(BQID) + "' and KLQID='" + StringHelper.SqlFilter(KLQID) + "' and EOLID in (select EOLID from ExamOnline where EIID='" + StringHelper.SqlFilter(EIID) + "' and isMakeUp='" + StringHelper.SqlFilter(Type) + "' and ExamPersonID='" + StringHelper.SqlFilter(Personid) + "'))";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
        }

        public DataTable GetSelected(string EOLID, string BQID, string KLQID)
        {

            string sqlstr = "select * from ExamOnlineAnswer where EOLDID in (select EOLDID from ExamOnlineDetail where BQID='" + StringHelper.SqlFilter(BQID) + "' and KLQID='" + StringHelper.SqlFilter(KLQID) + "' and EOLID='" + StringHelper.SqlFilter(EOLID) + "')";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
        }

        /// <summary>
        /// 取小题得分
        /// </summary>
        /// <returns></returns>
        public DataTable Getfenshu(string EIID, string Type, string Personid, string BQID, string KLQID)
        {

            string sqlstr = "select * from ExamOnlineDetail where BQID='" + StringHelper.SqlFilter(BQID) + "' and KLQID='" + StringHelper.SqlFilter(KLQID) + "' and EOLID in (select EOLID from ExamOnline where EIID='" + StringHelper.SqlFilter(EIID) + "' and isMakeUp='" + StringHelper.SqlFilter(Type) + "' and ExamPersonID='" + StringHelper.SqlFilter(Personid) + "')";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
        }

        /// <summary>
        /// 取总分数
        /// </summary>
        /// <returns></returns>
        public DataTable GetSumScore(string EIID, string Type, string Personid)
        {

            string sqlstr = "select * from ExamOnline where EIID='" + StringHelper.SqlFilter(EIID) + "' and isMakeUp='" + StringHelper.SqlFilter(Type) + "' and ExamPersonID='" + StringHelper.SqlFilter(Personid) + "'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
        }
        #endregion

    }
}

