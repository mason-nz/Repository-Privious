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
    /// ���ݷ�����SurveyPerson��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:18 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyPerson : DataBase
    {
        #region Instance
        public static readonly SurveyPerson Instance = new SurveyPerson();
        #endregion

        #region const
        private const string P_SURVEYPERSON_SELECT = "p_SurveyPerson_Select";
        private const string P_SURVEYPERSON_INSERT = "p_SurveyPerson_Insert";
        private const string P_SURVEYPERSON_UPDATE = "p_SurveyPerson_Update";
        private const string P_SURVEYPERSON_DELETE = "p_SurveyPerson_Delete";
        #endregion

        #region Contructor
        protected SurveyPerson()
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
        public DataTable GetSurveyPerson(QuerySurveyPerson query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            
            if (query.ExamPersonID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ExamPersonID=" + query.ExamPersonID;
            }
            if (query.SPIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SPIID=" + query.SPIID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// ���ݵ�����ĿID����ѯ������Ա��Ϣ
        /// </summary>
        /// <param name="spiId"></param>
        /// <returns></returns>
        public DataTable GetSurveyPersonBySPIID(int spiId)
        {
            string sqlStr = "SELECT * FROM SurveyPerson WHERE SPIID=@SPIID";
            SqlParameter parameter = new SqlParameter("SPIID", spiId);
            parameter.Value = spiId;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyPerson GetSurveyPerson(int RecID)
        {
            QuerySurveyPerson query = new QuerySurveyPerson();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyPerson(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyPerson(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyPerson LoadSingleSurveyPerson(DataRow row)
        {
            Entities.SurveyPerson model = new Entities.SurveyPerson();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["SPIID"].ToString() != "")
            {
                model.SPIID = int.Parse(row["SPIID"].ToString());
            }
            if (row["ExamPersonID"].ToString() != "")
            {
                model.ExamPersonID = int.Parse(row["ExamPersonID"].ToString());
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
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.SurveyPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.ExamPersonID;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.ExamPersonID;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.SurveyPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.ExamPersonID;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyPerson model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.ExamPersonID;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYPERSON_DELETE, parameters);
        }

        /// <summary>
        /// ɾ�������µĲ�����
        /// </summary>
        /// <param name="spiId"></param>
        /// <returns></returns>
        public int DeleteBySPIID(int spiId)
        {
            string sqlStr = "DELETE SurveyPerson WHERE SPIID=@SPIID";
            SqlParameter parameter = new SqlParameter("SPIID", spiId);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
        }
        #endregion

    }
}

