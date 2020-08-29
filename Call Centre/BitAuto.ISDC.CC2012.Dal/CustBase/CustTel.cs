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
    /// 数据访问类CustTel。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:15 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustTel : DataBase
    {
        #region Instance
        public static readonly CustTel Instance = new CustTel();
        #endregion

        #region const
        private const string P_CUSTTEL_SELECT = "p_CustTel_Select";
        private const string P_CUSTTEL_INSERT = "p_CustTel_Insert";
        private const string P_CUSTTEL_UPDATE = "p_CustTel_Update";
        private const string P_CUSTTEL_DELETE = "p_CustTel_Delete";
        #endregion

        #region Contructor
        protected CustTel()
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
        public DataTable GetCustTel(QueryCustTel query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.CustID != Constant.STRING_INVALID_VALUE && query.CustID != "")
            {
                where += " AND CustID='" + Utils.StringHelper.SqlFilter(query.CustID) + "'";
            }

            if (query.Tel != Constant.STRING_INVALID_VALUE && query.Tel != "")
            {
                where += " AND Tel='" + Utils.StringHelper.SqlFilter(query.Tel) + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTTEL_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 查询客户下的所有电话
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetCustTel(string custId)
        {
            string sqlStr = "SELECT * FROM CustTel WHERE CustID=@CustID ORDER BY CreateTime DESC";
            SqlParameter parameter = new SqlParameter("@CustID", custId);
            DataSet dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return dt.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustTel GetCustTel(string CustID, string Tel)
        {
            CustID = SqlFilter(CustID);
            Tel = SqlFilter(Tel);
            QueryCustTel query = new QueryCustTel();
            query.CustID = CustID;
            query.Tel = Tel;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustTel(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCustTel(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CustTel LoadSingleCustTel(DataRow row)
        {
            Entities.CustTel model = new Entities.CustTel();

            model.CustID = row["CustID"].ToString();
            model.Tel = row["Tel"].ToString();
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
        public void Insert(Entities.CustTel model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.Tel;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTTEL_INSERT, parameters);
        }

        public void Insert(SqlTransaction sqltran, Entities.CustTel model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.Tel;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CUSTTEL_INSERT, parameters);
        }



        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CustTel model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CustID;
            parameters[1].Value = model.Tel;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTTEL_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 根据客户编号删除此客户下的电话
        /// </summary>
        public int Delete(string CustID)
        {
            string sqlStr = "DELETE FROM CustTel WHERE CustID=@CustID";
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
            parameters[0].Value = CustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }
        #endregion

    }
}

