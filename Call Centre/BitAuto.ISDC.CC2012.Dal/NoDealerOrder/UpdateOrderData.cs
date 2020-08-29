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
    /// ���ݷ�����UpdateOrderData��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UpdateOrderData : DataBase
    {
        #region Instance
        public static readonly UpdateOrderData Instance = new UpdateOrderData();
        #endregion

        #region const
        private const string P_UPDATEORDERDATA_SELECT = "p_UpdateOrderData_Select";
        private const string P_UPDATEORDERDATA_INSERT = "p_UpdateOrderData_Insert";
        private const string P_UPDATEORDERDATA_UPDATE = "p_UpdateOrderData_Update";
        private const string P_UPDATEORDERDATA_DELETE = "p_UpdateOrderData_Delete";
        #endregion

        #region Contructor
        protected UpdateOrderData()
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
        public DataTable GetUpdateOrderData(QueryUpdateOrderData query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TaskID='" + StringHelper.SqlFilter(query.TaskID)+"'";
            }
            if (query.IsUpdate != Constant.INT_INVALID_VALUE)
            {
                where += " AND IsUpdate=" + query.IsUpdate;
            }
            if (query.UpdateType != Constant.INT_INVALID_VALUE)
            {
                where += " AND UpdateType=" + query.UpdateType;
            }

            if (query.ConsultType != Constant.INT_INVALID_VALUE)
            {
                where += " AND ConsultType=" + query.ConsultType;
            }
            if (query.ConsultRecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ConsultRecID=" + query.ConsultRecID;
            }
            if (query.APIType != Constant.INT_INVALID_VALUE)
            {
                where += " AND APIType=" + query.APIType;
            }
            if (query.YPOrderID != Constant.INT_INVALID_VALUE)
            {
                where += " AND YPOrderID=" + query.YPOrderID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_UPDATEORDERDATA_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.UpdateOrderData GetUpdateOrderData(int RecID)
        {
            QueryUpdateOrderData query = new QueryUpdateOrderData();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUpdateOrderData(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleUpdateOrderData(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.UpdateOrderData LoadSingleUpdateOrderData(DataRow row)
        {
            Entities.UpdateOrderData model = new Entities.UpdateOrderData();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
         
                model.TaskID = row["TaskID"].ToString();
           
            if (row["YPOrderID"].ToString() != "")
            {
                model.YPOrderID = int.Parse(row["YPOrderID"].ToString());
            }
            if (row["UpdateType"].ToString() != "")
            {
                model.UpdateType = int.Parse(row["UpdateType"].ToString());
            }
            model.UpdateErrorMsg = row["UpdateErrorMsg"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["IsUpdate"].ToString() != "")
            {
                model.IsUpdate = int.Parse(row["IsUpdate"].ToString());
            }
            if (row["UpdateDateTime"].ToString() != "")
            {
                model.UpdateDateTime = DateTime.Parse(row["UpdateDateTime"].ToString());
            }
            if (row["ConsultType"].ToString() != "")
            {
                model.ConsultType = int.Parse(row["ConsultType"].ToString());
            }
            if (row["ConsultRecID"].ToString() != "")
            {
                model.ConsultRecID = int.Parse(row["ConsultRecID"].ToString());
            }
            if (row["APIType"].ToString() != "")
            {
                model.APIType = int.Parse(row["APIType"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            return model;
        }

        public List<Entities.UpdateOrderData> GetUpdateOrderDataList(Entities.QueryUpdateOrderData query)
        {
            List<Entities.UpdateOrderData> list = new List<Entities.UpdateOrderData>();

            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUpdateOrderData(query, string.Empty, 1, 999999, out count);
            if (count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleUpdateOrderData(dr));
                }
            }
            else
            {
                return null;
            }
            return list;
        }

        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.UpdateOrderData model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UpdateType", SqlDbType.Int,4),
					new SqlParameter("@UpdateErrorMsg", SqlDbType.NText),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                     new SqlParameter("@IsUpdate", SqlDbType.Int,4),
                        new SqlParameter("@ConsultType", SqlDbType.Int,4),
                           new SqlParameter("@ConsultRecID", SqlDbType.Int,4),
                              new SqlParameter("@APIType", SqlDbType.Int,4),
                    new SqlParameter("@UpdateDateTime", SqlDbType.DateTime),
                           new SqlParameter("@CustID", SqlDbType.VarChar,20)             };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.YPOrderID;
            parameters[3].Value = model.UpdateType;
            parameters[4].Value = model.UpdateErrorMsg;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.IsUpdate;
            parameters[8].Value = model.ConsultType;
            parameters[9].Value = model.ConsultRecID;
            parameters[10].Value = model.APIType;
            parameters[11].Value = model.UpdateDateTime;
            parameters[12].Value = model.CustID;


            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_UPDATEORDERDATA_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UpdateOrderData model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UpdateType", SqlDbType.Int,4),
					new SqlParameter("@UpdateErrorMsg", SqlDbType.NText),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsUpdate", SqlDbType.Int,4),
                      new SqlParameter("@ConsultType", SqlDbType.Int,4),
                           new SqlParameter("@ConsultRecID", SqlDbType.Int,4),
                              new SqlParameter("@APIType", SqlDbType.Int,4),
                    new SqlParameter("@UpdateDateTime", SqlDbType.DateTime),
                           new SqlParameter("@CustID", SqlDbType.VarChar,20) };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.YPOrderID;
            parameters[3].Value = model.UpdateType;
            parameters[4].Value = model.UpdateErrorMsg;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.IsUpdate;
            parameters[8].Value = model.ConsultType;
            parameters[9].Value = model.ConsultRecID;
            parameters[10].Value = model.APIType;
            parameters[11].Value = model.UpdateDateTime;
            parameters[12].Value = model.CustID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_UPDATEORDERDATA_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.UpdateOrderData model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UpdateType", SqlDbType.Int,4),
					new SqlParameter("@UpdateErrorMsg", SqlDbType.NText),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsUpdate", SqlDbType.Int,4),
                      new SqlParameter("@ConsultType", SqlDbType.Int,4),
                           new SqlParameter("@ConsultRecID", SqlDbType.Int,4),
                              new SqlParameter("@APIType", SqlDbType.Int,4),
                    new SqlParameter("@UpdateDateTime", SqlDbType.DateTime),
                           new SqlParameter("@CustID", SqlDbType.VarChar,20) };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.YPOrderID;
            parameters[3].Value = model.UpdateType;
            parameters[4].Value = model.UpdateErrorMsg;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.IsUpdate;
            parameters[8].Value = model.ConsultType;
            parameters[9].Value = model.ConsultRecID;
            parameters[10].Value = model.APIType;
            parameters[11].Value = model.UpdateDateTime;
            parameters[12].Value = model.CustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_UPDATEORDERDATA_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UpdateOrderData model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UpdateType", SqlDbType.Int,4),
					new SqlParameter("@UpdateErrorMsg", SqlDbType.NText),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsUpdate", SqlDbType.Int,4),
                      new SqlParameter("@ConsultType", SqlDbType.Int,4),
                           new SqlParameter("@ConsultRecID", SqlDbType.Int,4),
                              new SqlParameter("@APIType", SqlDbType.Int,4),
                    new SqlParameter("@UpdateDateTime", SqlDbType.DateTime), 
                           new SqlParameter("@CustID", SqlDbType.VarChar,20)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.YPOrderID;
            parameters[3].Value = model.UpdateType;
            parameters[4].Value = model.UpdateErrorMsg;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.IsUpdate;
            parameters[8].Value = model.ConsultType;
            parameters[9].Value = model.ConsultRecID;
            parameters[10].Value = model.APIType;
            parameters[11].Value = model.UpdateDateTime;
            parameters[12].Value = model.CustID;
            return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_UPDATEORDERDATA_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_UPDATEORDERDATA_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_UPDATEORDERDATA_DELETE, parameters);
        }
        #endregion

    }
}

