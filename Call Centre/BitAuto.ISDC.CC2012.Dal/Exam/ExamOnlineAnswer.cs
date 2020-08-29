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
    /// ���ݷ�����ExamOnlineAnswer��
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
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
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
        /// �õ�һ������ʵ��
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
        ///  ����һ������
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
        ///  ����һ������
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
        ///  ����һ������
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
        ///  ����һ������
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
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEANSWER_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINEANSWER_DELETE, parameters);
        }

        #endregion

        #region add by qizq ���߿���
        /// <summary>
        /// ȡѡ���ѡ��
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
        /// ȡС��÷�
        /// </summary>
        /// <returns></returns>
        public DataTable Getfenshu(string EIID, string Type, string Personid, string BQID, string KLQID)
        {

            string sqlstr = "select * from ExamOnlineDetail where BQID='" + StringHelper.SqlFilter(BQID) + "' and KLQID='" + StringHelper.SqlFilter(KLQID) + "' and EOLID in (select EOLID from ExamOnline where EIID='" + StringHelper.SqlFilter(EIID) + "' and isMakeUp='" + StringHelper.SqlFilter(Type) + "' and ExamPersonID='" + StringHelper.SqlFilter(Personid) + "')";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
        }

        /// <summary>
        /// ȡ�ܷ���
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

