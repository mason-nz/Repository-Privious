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
    /// 数据访问类WorkOrderCategory。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:20 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderCategory : DataBase
    {
        #region Instance
        public static readonly WorkOrderCategory Instance = new WorkOrderCategory();
        #endregion

        #region const
        private const string P_WORKORDERCATEGORY_SELECT = "p_WorkOrderCategory_Select";
        private const string P_WORKORDERCATEGORY_INSERT = "p_WorkOrderCategory_Insert";
        private const string P_WORKORDERCATEGORY_UPDATE = "p_WorkOrderCategory_Update";
        private const string P_WORKORDERCATEGORY_DELETE = "p_WorkOrderCategory_Delete";
        #endregion

        #region Contructor
        protected WorkOrderCategory()
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
        public DataTable GetWorkOrderCategory(QueryWorkOrderCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and recID = " + query.RecID;
            }
            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " and level=" + query.Level;
            }
            if (query.PID != Constant.INT_INVALID_VALUE)
            {
                where += " and pid=" + query.PID;
            }
            if (query.UseScopeStr != Constant.STRING_INVALID_VALUE)
            {
                where += " and usescope in (" + Dal.Util.SqlFilterByInCondition(query.UseScopeStr) + ")";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERCATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderCategory GetWorkOrderCategory(int RecID)
        {
            QueryWorkOrderCategory query = new QueryWorkOrderCategory();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderCategory(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderCategory(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderCategory LoadSingleWorkOrderCategory(DataRow row)
        {
            Entities.WorkOrderCategory model = new Entities.WorkOrderCategory();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["PID"].ToString() != "")
            {
                model.PID = int.Parse(row["PID"].ToString());
            }
            if (row["Level"].ToString() != "")
            {
                model.Level = int.Parse(row["Level"].ToString());
            }
            if (row["UseScope"].ToString() != "")
            {
                model.UseScope = int.Parse(row["UseScope"].ToString());
            }
            if (row["OrderNum"].ToString() != "")
            {
                model.OrderNum = int.Parse(row["OrderNum"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.WorkOrderCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@PID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@UseScope", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.PID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.UseScope;
            parameters[5].Value = model.OrderNum;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERCATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@PID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@UseScope", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.PID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.UseScope;
            parameters[5].Value = model.OrderNum;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERCATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@PID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@UseScope", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.PID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.UseScope;
            parameters[5].Value = model.OrderNum;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERCATEGORY_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@PID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@UseScope", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.PID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.UseScope;
            parameters[5].Value = model.OrderNum;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERCATEGORY_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERCATEGORY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERCATEGORY_DELETE, parameters);
        }
        #endregion

        public DataTable GetCategoryFullName(string CategoryID)
        {
            string sql = "";
            sql = @"SELECT  *
                      FROM    TF_GetWorkOrderCategory(" + StringHelper.SqlFilter(CategoryID) + ")";

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return ds.Tables[0];
        }

    }
}

