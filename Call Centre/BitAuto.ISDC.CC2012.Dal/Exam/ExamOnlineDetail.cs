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
    /// ���ݷ�����ExamOnlineDetail��
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
    public class ExamOnlineDetail : DataBase
    {
        #region Instance
        public static readonly ExamOnlineDetail Instance = new ExamOnlineDetail();
        #endregion

        #region const
        private const string P_EXAMONLINEDETAIL_SELECT = "p_ExamOnlineDetail_Select";
        private const string P_EXAMONLINEDETAIL_INSERT = "p_ExamOnlineDetail_Insert";
        private const string P_EXAMONLINEDETAIL_UPDATE = "p_ExamOnlineDetail_Update";
        private const string P_EXAMONLINEDETAIL_DELETE = "p_ExamOnlineDetail_Delete";
        #endregion

        #region Contructor
        protected ExamOnlineDetail()
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
        public DataTable GetExamOnlineDetail(QueryExamOnlineDetail query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.EOLDID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EOLDID=" + query.EOLDID;
            }
            if (query.EOLID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EOLID=" + query.EOLID;
            }
            if (query.BQID != Constant.INT_INVALID_VALUE)
            {
                where += " AND BQID=" + query.BQID;
            }
            if (query.EPID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EPID=" + query.EPID;
            }
            if (query.KLQID != Constant.INT_INVALID_VALUE)
            {
                where += " AND KLQID=" + query.KLQID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ExamOnlineDetail GetExamOnlineDetail(long EOLDID)
        {
            QueryExamOnlineDetail query = new QueryExamOnlineDetail();
            query.EOLDID = EOLDID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamOnlineDetail(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamOnlineDetail(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamOnlineDetail LoadSingleExamOnlineDetail(DataRow row)
        {
            Entities.ExamOnlineDetail model = new Entities.ExamOnlineDetail();

            if (row["EOLDID"].ToString() != "")
            {
                model.EOLDID = long.Parse(row["EOLDID"].ToString());
            }
            if (row["EOLID"].ToString() != "")
            {
                model.EOLID = int.Parse(row["EOLID"].ToString());
            }
            if (row["EPID"].ToString() != "")
            {
                model.EPID = int.Parse(row["EPID"].ToString());
            }
            if (row["BQID"].ToString() != "")
            {
                model.BQID = long.Parse(row["BQID"].ToString());
            }
            if (row["KLQID"].ToString() != "")
            {
                model.KLQID = long.Parse(row["KLQID"].ToString());
            }
            if (row["Score"].ToString() != "")
            {
                model.Score = int.Parse(row["Score"].ToString());
            }
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
        public int Insert(Entities.ExamOnlineDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLDID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLID", SqlDbType.Int,4),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EOLID;
            parameters[2].Value = model.EPID;
            parameters[3].Value = model.BQID;
            parameters[4].Value = model.KLQID;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLDID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLID", SqlDbType.Int,4),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EOLID;
            parameters[2].Value = model.EPID;
            parameters[3].Value = model.BQID;
            parameters[4].Value = model.KLQID;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.ExamOnlineDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLDID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLID", SqlDbType.Int,4),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.EOLDID;
            parameters[1].Value = model.EOLID;
            parameters[2].Value = model.EPID;
            parameters[3].Value = model.BQID;
            parameters[4].Value = model.KLQID;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLDID", SqlDbType.BigInt,8),
					new SqlParameter("@EOLID", SqlDbType.Int,4),
					new SqlParameter("@EPID", SqlDbType.Int,4),
					new SqlParameter("@BQID", SqlDbType.BigInt,8),
					new SqlParameter("@KLQID", SqlDbType.BigInt,8),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.EOLDID;
            parameters[1].Value = model.EOLID;
            parameters[2].Value = model.EPID;
            parameters[3].Value = model.BQID;
            parameters[4].Value = model.KLQID;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreaetUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long EOLDID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLDID", SqlDbType.BigInt)};
            parameters[0].Value = EOLDID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EOLDID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLDID", SqlDbType.BigInt)};
            parameters[0].Value = EOLDID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINEDETAIL_DELETE, parameters);
        }
         
        #endregion

        //add by qizq 2012-9-7
        /// <summary>
        /// ���ݿ���id������С�⣬����С��÷�
        /// </summary>
        /// <param name="EOLID"></param>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <param name="Score"></param>
        public void UpdateByEOLID(string EOLID, string BQID, string KLQID, string Score)
        {
            string sqlstr = "update ExamOnlineDetail set score='" + StringHelper.SqlFilter(Score) + "' where EOLID='" + StringHelper.SqlFilter(EOLID) + "' and BQID='" + StringHelper.SqlFilter(BQID) + "' and KLQID='" + StringHelper.SqlFilter(KLQID) + "'";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS,CommandType.Text,sqlstr,null);
        }

    }
}

