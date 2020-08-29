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
    /// 数据访问类WorkOrderTagMapping。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:22 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderTagMapping : DataBase
    {
        #region Instance
        public static readonly WorkOrderTagMapping Instance = new WorkOrderTagMapping();
        #endregion

        #region const
        private const string P_WORKORDERTAGMAPPING_SELECT = "p_WorkOrderTagMapping_Select";
        private const string P_WORKORDERTAGMAPPING_INSERT = "p_WorkOrderTagMapping_Insert";
        private const string P_WORKORDERTAGMAPPING_UPDATE = "p_WorkOrderTagMapping_Update";
        private const string P_WORKORDERTAGMAPPING_DELETE = "p_WorkOrderTagMapping_Delete";
        #endregion

        #region Contructor
        protected WorkOrderTagMapping()
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
        public DataTable GetWorkOrderTagMapping(QueryWorkOrderTagMapping query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderTagMapping GetWorkOrderTagMapping(int RecID)
        {
            QueryWorkOrderTagMapping query = new QueryWorkOrderTagMapping();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderTagMapping(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderTagMapping(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderTagMapping LoadSingleWorkOrderTagMapping(DataRow row)
        {
            Entities.WorkOrderTagMapping model = new Entities.WorkOrderTagMapping();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.OrderID = row["OrderID"].ToString();
            if (row["TagID"].ToString() != "")
            {
                model.TagID = int.Parse(row["TagID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.WorkOrderTagMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@TagID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.TagID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderTagMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@TagID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.TagID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderTagMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@TagID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.TagID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderTagMapping model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@TagID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.TagID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERTAGMAPPING_DELETE, parameters);
        }

        public int DeleteByOrderID(string OrderID)
        {
            string sql = "delete WORKORDERTAGMAPPING where orderID='" + StringHelper.SqlFilter(OrderID) + "'";
            return int.Parse(SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql).ToString());
        }
        #endregion

    }
}

