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
    /// ���ݷ�����CallRecord_ORIG_Authorizer��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-17 10:17:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG_Authorizer : DataBase
    {
        #region Instance
        public static readonly CallRecord_ORIG_Authorizer Instance = new CallRecord_ORIG_Authorizer();
        #endregion

        #region const
        private const string P_CALLRECORD_ORIG_AUTHORIZER_SELECT = "p_CallRecord_ORIG_Authorizer_Select";
        private const string P_CALLRECORD_ORIG_AUTHORIZER_INSERT = "p_CallRecord_ORIG_Authorizer_Insert";
        private const string P_CALLRECORD_ORIG_AUTHORIZER_UPDATE = "p_CallRecord_ORIG_Authorizer_Update";
        private const string P_CALLRECORD_ORIG_AUTHORIZER_DELETE = "p_CallRecord_ORIG_Authorizer_Delete";
        #endregion

        #region Contructor
        protected CallRecord_ORIG_Authorizer()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetCallRecord_ORIG_Authorizer(string where, out int totalCount)
        {
            string sql = "SELECT * FROM CallRecord_ORIG_Authorizer Where 1=1 ";
            sql += where;
            //sql += " Order by " + order;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                totalCount = ds.Tables[0].Rows.Count;
                return ds.Tables[0];
            }
            else
            {
                totalCount = 0;
                return null;
            }
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.CallRecord_ORIG_Authorizer GetCallRecord_ORIG_Authorizer(int RecID)
        {
            //QueryCallRecord_ORIG_Authorizer query = new QueryCallRecord_ORIG_Authorizer();
            //query.RecID = RecID;
            string sql = string.Format(" And RecID={0}", RecID);
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCallRecord_ORIG_Authorizer(sql, out count);
            if (count > 0)
            {
                return LoadSingleCallRecord_ORIG_Authorizer(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CallRecord_ORIG_Authorizer LoadSingleCallRecord_ORIG_Authorizer(DataRow row)
        {
            Entities.CallRecord_ORIG_Authorizer model = new Entities.CallRecord_ORIG_Authorizer();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.IP = row["IP"].ToString();
            model.AuthorizeCode = row["AuthorizeCode"].ToString();
            model.Remark = row["Remark"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG_Authorizer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,15),
					new SqlParameter("@AuthorizeCode", SqlDbType.VarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.IP;
            parameters[2].Value = model.AuthorizeCode;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.Status;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_AUTHORIZER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.CallRecord_ORIG_Authorizer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,15),
					new SqlParameter("@AuthorizeCode", SqlDbType.VarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.IP;
            parameters[2].Value = model.AuthorizeCode;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.Status;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLRECORD_ORIG_AUTHORIZER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Authorizer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,15),
					new SqlParameter("@AuthorizeCode", SqlDbType.VarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.IP;
            parameters[2].Value = model.AuthorizeCode;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_AUTHORIZER_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CallRecord_ORIG_Authorizer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,15),
					new SqlParameter("@AuthorizeCode", SqlDbType.VarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.IP;
            parameters[2].Value = model.AuthorizeCode;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLRECORD_ORIG_AUTHORIZER_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_AUTHORIZER_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CALLRECORD_ORIG_AUTHORIZER_DELETE, parameters);
        }
        #endregion

    }
}

