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
    /// 数据访问类CustHistoryTemplateMapping。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-09 02:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustHistoryTemplateMapping : DataBase
    {
        #region Instance
        public static readonly CustHistoryTemplateMapping Instance = new CustHistoryTemplateMapping();
        #endregion

        #region const
        private const string P_CUSTHISTORYTEMPLATEMAPPING_SELECT = "p_CustHistoryTemplateMapping_Select";
        private const string P_CUSTHISTORYTEMPLATEMAPPING_INSERT = "p_CustHistoryTemplateMapping_Insert";
        private const string P_CUSTHISTORYTEMPLATEMAPPING_UPDATE = "p_CustHistoryTemplateMapping_Update";
        private const string P_CUSTHISTORYTEMPLATEMAPPING_DELETE = "p_CustHistoryTemplateMapping_Delete";
        #endregion

        #region Contructor
        protected CustHistoryTemplateMapping()
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
        public DataTable GetCustHistoryTemplateMapping(QueryCustHistoryTemplateMapping query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.SolveUserEID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SolveUserEID=" + query.SolveUserEID.ToString();
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYTEMPLATEMAPPING_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustHistoryTemplateMapping GetCustHistoryTemplateMapping(long RecID)
        {
            QueryCustHistoryTemplateMapping query = new QueryCustHistoryTemplateMapping();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCustHistoryTemplateMapping(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCustHistoryTemplateMapping(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CustHistoryTemplateMapping LoadSingleCustHistoryTemplateMapping(DataRow row)
        {
            Entities.CustHistoryTemplateMapping model = new Entities.CustHistoryTemplateMapping();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.TaskID = row["TaskID"].ToString();
            if (row["TemplateID"].ToString() != "")
            {
                model.TemplateID = int.Parse(row["TemplateID"].ToString());
            }
            if (row["SolveUserEID"].ToString() != "")
            {
                model.SolveUserEID = int.Parse(row["SolveUserEID"].ToString());
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
        public int Insert(Entities.CustHistoryTemplateMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.TemplateID;
            parameters[3].Value = model.SolveUserEID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYTEMPLATEMAPPING_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CustHistoryTemplateMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@SolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.TemplateID;
            parameters[3].Value = model.SolveUserEID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYTEMPLATEMAPPING_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CUSTHISTORYTEMPLATEMAPPING_DELETE, parameters);
        }
        #endregion

    }
}

