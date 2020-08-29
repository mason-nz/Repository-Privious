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
    /// 数据访问类WorkOrderActivity。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-01-13 04:21:11 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderActivity : DataBase
    {
        #region Instance
        public static readonly WorkOrderActivity Instance = new WorkOrderActivity();
        #endregion

        #region const
        private const string P_WORKORDERACTIVITY_SELECT = "p_WorkOrderActivity_Select";
        private const string P_WORKORDERACTIVITY_INSERT = "p_WorkOrderActivity_Insert";
        //private const string P_WORKORDERACTIVITY_UPDATE = "p_WorkOrderActivity_Update";
        private const string P_WORKORDERACTIVITY_DELETE = "p_WorkOrderActivity_Delete";
        #endregion

        #region Contructor
        protected WorkOrderActivity()
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
        public DataTable GetWorkOrderActivity(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = query;
            
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERACTIVITY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderActivity GetWorkOrderActivity(string OrderID, Guid ActivityGUID)
        {
            //QueryWorkOrderActivity query = new QueryWorkOrderActivity();
            //query.OrderID = OrderID;
            //query.ActivityGUID = ActivityGUID;
            string query = string.Format(" And OrderID='{0}' And ActivityGUID='{1}'",
                StringHelper.SqlFilter(OrderID), StringHelper.SqlFilter(ActivityGUID.ToString()));
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderActivity(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderActivity(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderActivity LoadSingleWorkOrderActivity(DataRow row)
        {
            Entities.WorkOrderActivity model = new Entities.WorkOrderActivity();

            model.OrderID = row["OrderID"].ToString();
            model.ActivityGUID = new Guid(row["ActivityGUID"].ToString());
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
        public void Insert(Entities.WorkOrderActivity model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@ActivityGUID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.ActivityGUID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERACTIVITY_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.WorkOrderActivity model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@ActivityGUID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.ActivityGUID;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERACTIVITY_INSERT, parameters);
        }
        #endregion

        #region Update
        ///// <summary>
        /////  更新一条数据
        ///// </summary>
        //public int Update(Entities.WorkOrderActivity model)
        //{
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@OrderID", SqlDbType.VarChar,20),
        //            new SqlParameter("@ActivityGUID", SqlDbType.UniqueIdentifier,16),
        //            new SqlParameter("@CreateTime", SqlDbType.DateTime),
        //            new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
        //    parameters[0].Value = model.OrderID;
        //    parameters[1].Value = model.ActivityGUID;
        //    parameters[2].Value = model.CreateTime;
        //    parameters[3].Value = model.CreateUserID;

        //    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERACTIVITY_UPDATE, parameters);
        //}
        ///// <summary>
        /////  更新一条数据
        ///// </summary>
        //public int Update(SqlTransaction sqltran, Entities.WorkOrderActivity model)
        //{
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@OrderID", SqlDbType.VarChar,20),
        //            new SqlParameter("@ActivityGUID", SqlDbType.UniqueIdentifier,16),
        //            new SqlParameter("@CreateTime", SqlDbType.DateTime),
        //            new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
        //    parameters[0].Value = model.OrderID;
        //    parameters[1].Value = model.ActivityGUID;
        //    parameters[2].Value = model.CreateTime;
        //    parameters[3].Value = model.CreateUserID;

        //    return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERACTIVITY_UPDATE, parameters);
        //}
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID, Guid ActivityGUID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@ActivityGUID", SqlDbType.UniqueIdentifier)};
            parameters[0].Value = OrderID;
            parameters[1].Value = ActivityGUID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERACTIVITY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID, Guid ActivityGUID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@ActivityGUID", SqlDbType.UniqueIdentifier)};
            parameters[0].Value = OrderID;
            parameters[1].Value = ActivityGUID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERACTIVITY_DELETE, parameters);
        }
        #endregion

    }
}

