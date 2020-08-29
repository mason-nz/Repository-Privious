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
    /// ���ݷ�����CustEmail��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:13 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustEmail : DataBase
    {
        #region Instance
        public static readonly CustEmail Instance = new CustEmail();
        #endregion

        #region const
        private const string P_CUSTEMAIL_SELECT = "p_CustEmail_Select";
        private const string P_CUSTEMAIL_INSERT = "p_CustEmail_Insert";
        private const string P_CUSTEMAIL_UPDATE = "p_CustEmail_Update";
        private const string P_CUSTEMAIL_DELETE = "p_CustEmail_Delete";
        #endregion

        #region Contructor
        protected CustEmail()
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
        public DataTable GetCustEmail(QueryCustEmail query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.CustID != Constant.STRING_INVALID_VALUE &&query.CustID != "")
            {
                where += " AND CustID='" + Utils.StringHelper.SqlFilter(query.CustID)+"'";
            }
            if (query.Email != Constant.STRING_INVALID_VALUE && query.Email !="")
            {
                where += " AND Email='" + Utils.StringHelper.SqlFilter(query.Email) + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTEMAIL_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// ��ȡ�ͻ��µ������ʼ���Ϣ
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetCustEmail(string custId)
        {
            string sqlStr = " SELECT * FROM CustEmail WHERE CustID=@CustID";
            SqlParameter parameter = new SqlParameter("@CustID", custId);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.CustEmail GetCustEmail(string CustID, string Email)
        {
            QueryCustEmail query = new QueryCustEmail();
            query.CustID = CustID;
            query.Email = Email;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustEmail(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCustEmail(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CustEmail LoadSingleCustEmail(DataRow row)
        {
            Entities.CustEmail model = new Entities.CustEmail();

            model.CustID = row["CustID"].ToString();
            model.Email = row["Email"].ToString();
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
        public void Insert(Entities.CustEmail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.Email;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTEMAIL_INSERT, parameters);
        }

        public void Insert(SqlTransaction sqltran, Entities.CustEmail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.Email;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTEMAIL_INSERT, parameters);
        }

        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.CustEmail model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Email", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.Email;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTEMAIL_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(string CustID)
        {
            string sqlStr = "DELETE FROM CustEmail WHERE CustID=@CustID";
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
            parameters[0].Value = CustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }
        #endregion

    }
}

