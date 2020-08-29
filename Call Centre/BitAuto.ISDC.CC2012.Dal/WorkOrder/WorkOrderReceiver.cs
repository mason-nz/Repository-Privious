using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WorkOrderReceiver : DataBase
    {
        #region Instance
        public static readonly WorkOrderReceiver Instance = new WorkOrderReceiver();
        #endregion

        #region const
        private const string P_WorkOrderReceiver_SELECT = "p_WorkOrderReceiver_Select";
        private const string P_WorkOrderReceiver_INSERT = "p_WorkOrderReceiver_Insert";
        private const string P_WorkOrderReceiver_UPDATE = "p_WorkOrderReceiver_Update";
        private const string P_WorkOrderReceiver_DELETE = "p_WorkOrderReceiver_Delete";
        #endregion

        #region Contructor
        protected WorkOrderReceiver()
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
        public DataTable GetWorkOrderReceiver(QueryWorkOrderReceiver query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where = GetWhereStr(query);

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderReceiver_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private string GetWhereStr(QueryWorkOrderReceiver query)
        {
            string where = "";
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And w.RecID=" + query.RecID.ToString(); ;
            }
            if (query.OrderID != Constant.STRING_EMPTY_VALUE)
            {
                where += " And w.OrderID='" + StringHelper.SqlFilter(query.OrderID) + "'";
            }
            if (query.ReceiverUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And w.ReceiverUserID=" + query.ReceiverUserID + "";
            }
            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " And w.CallID=" + query.CallID + "";
            }
            return where;
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderReceiver GetWorkOrderReceiver(int RecID)
        {
            QueryWorkOrderReceiver query = new QueryWorkOrderReceiver();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderReceiver(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderReceiver(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderReceiver LoadSingleWorkOrderReceiver(DataRow row)
        {
            Entities.WorkOrderReceiver model = new Entities.WorkOrderReceiver();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }

            model.OrderID = row["OrderID"].ToString();
            model.RevertContent = row["RevertContent"].ToString();

            if (row["ReceiverUserID"].ToString() != "")
            {
                model.ReceiverUserID = int.Parse(row["ReceiverUserID"].ToString());
            }

            model.ReceiverDepartName = row["ReceiverDepartName"].ToString();
            if (row["CallID"].ToString() != "")
            {
                model.CallID = long.Parse(row["CallID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["AudioURL"].ToString() != "")
            {
                model.AudioURL = row["AudioURL"].ToString();
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.WorkOrderReceiver model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
                    new SqlParameter("@RevertContent", SqlDbType.NVarChar,1000),			 
					new SqlParameter("@ReceiverUserID", SqlDbType.Int,4),				                   
                    new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,50),
	                new SqlParameter("@CallID", SqlDbType.BigInt,8),	
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.ReceiverUserID;
            parameters[4].Value = model.ReceiverDepartName;
            parameters[5].Value = model.CallID;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderReceiver_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderReceiver model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
                    new SqlParameter("@RevertContent", SqlDbType.NVarChar,1000),			 
					new SqlParameter("@ReceiverUserID", SqlDbType.Int,4),				                     
                    new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,50),
	                new SqlParameter("@CallID", SqlDbType.BigInt,8),	
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.ReceiverUserID;
            parameters[4].Value = model.ReceiverDepartName;
            parameters[5].Value = model.CallID;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WorkOrderReceiver_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderReceiver model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),
 					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
                    new SqlParameter("@RevertContent", SqlDbType.NVarChar,1000),			 
					new SqlParameter("@ReceiverUserID", SqlDbType.Int,4),				                    
                    new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,50),
	                new SqlParameter("@CallID", SqlDbType.BigInt,8),	
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.ReceiverUserID;
            parameters[4].Value = model.ReceiverDepartName;
            parameters[5].Value = model.CallID;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderReceiver_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderReceiver model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),
 					new SqlParameter("@OrderID", SqlDbType.NVarChar,20),			 
                    new SqlParameter("@RevertContent", SqlDbType.NVarChar,1000),			 
					new SqlParameter("@ReceiverUserID", SqlDbType.Int,4),				                    
                    new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,50),
	                new SqlParameter("@CallID", SqlDbType.BigInt,8),	
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.ReceiverUserID;
            parameters[4].Value = model.ReceiverDepartName;
            parameters[5].Value = model.CallID;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WorkOrderReceiver_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WorkOrderReceiver_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WorkOrderReceiver_DELETE, parameters);
        }
        #endregion

        /// 同步话务数据到工单回复表
        /// <summary>
        /// 同步话务数据到工单回复表
        /// </summary>
        /// <returns></returns>
        public int SyncCallDataForService()
        {
            int num = 0;
            string sql = @"UPDATE a
                                    SET 
                                    a.EstablishedTime=b.EstablishedTime,
                                    a.TallTime=b.TallTime,
                                    a.AudioURL=b.AudioURL
                                    FROM WorkOrderReceiver a
                                    INNER JOIN CallRecord_ORIG b ON (a.CallID=b.CallID)
                                    WHERE  ISNULL(a.CallID,0)>0 
                                    --数据不一致的才更新
                                    AND (ISNULL(a.AudioURL,'')<>ISNULL(b.AudioURL,'')
                                    OR ISNULL(a.TallTime,0)<>ISNULL(b.TallTime,0)
                                    OR ISNULL(a.EstablishedTime,'')<>ISNULL(b.EstablishedTime,''))";
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 5 * 60;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                conn.Open();
                num = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return num;
        }
    }
}
