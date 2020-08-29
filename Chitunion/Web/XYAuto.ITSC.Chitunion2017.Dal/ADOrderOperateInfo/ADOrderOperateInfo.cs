using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class ADOrderOperateInfo : DataBase
    {
        public const string P_ADOrderOperateInfo_SELECT = "p_ADOrderOperateInfo_Select";

        #region Instance
        public static readonly ADOrderOperateInfo Instance = new ADOrderOperateInfo();
        #endregion

        #region Select
        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.ADOrderOperateInfo GetModel(string recid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,OrderID,SubOrderID,OptType,OrderStatus,RejectMsg,CreateTime,CreateUserID");
            strSql.Append(" FROM ADOrderOperateInfo ");
            strSql.Append(" WHERE RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)
			};
            parameters[0].Value = recid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ADOrderOperateInfo DataRowToModel(DataRow row)
        {
            Entities.ADOrderOperateInfo model = new Entities.ADOrderOperateInfo();
            if (row != null)
            {
                if (row["RecId"] != null && row["RecId"].ToString() != "")
                {
                    model.RecID = int.Parse(row["RecId"].ToString());
                }
                if (row["OrderID"] != null && row["OrderID"].ToString() != "")
                {
                    model.OrderID = row["OrderID"].ToString();
                }
                if (row["SubOrderID"] != null && row["SubOrderID"].ToString() != "")
                {
                    model.SubOrderID = row["SubOrderID"].ToString();
                }

                if (row["OptType"] != null && row["OptType"].ToString() != "")
                {
                    model.OptType = int.Parse(row["OptType"].ToString());
                }

                if (row["OrderStatus"] != null && row["OrderStatus"].ToString() != "")
                {
                    model.OrderStatus = int.Parse(row["OrderStatus"].ToString());
                }
                if (row["OptType"] != null && row["OptType"].ToString() != "")
                {
                    model.OptType = int.Parse(row["OptType"].ToString());
                }                
                if (row["RejectMsg"] != null && row["RejectMsg"].ToString() != "")
                {
                    model.RejectMsg = row["RejectMsg"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
                }

                
            }
            return model;
        }

        #region 根据订单号查询得到一个对象实体
        public Entities.ADOrderOperateInfo GetADOrderOperateInfo_ByOrderID(string orderid)
        {
            QueryADOrderOperateInfo query = new QueryADOrderOperateInfo();
            query.OrderID = orderid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetADOrderOperateInfo(query, " CreateTime desc ", 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleADOrderOperateInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }


        public Entities.ADOrderOperateInfo GetADOrderOperateInfo(int recid)
        {
            QueryADOrderOperateInfo query = new QueryADOrderOperateInfo();
            query.RecID = recid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetADOrderOperateInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleADOrderOperateInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ADOrderOperateInfo LoadSingleADOrderOperateInfo(DataRow row)
        {
            Entities.ADOrderOperateInfo model = new Entities.ADOrderOperateInfo();
            model.RecID = int.Parse(row["RecID"].ToString());

            if (row["OrderID"] != null && row["OrderID"].ToString() != "")
            {
                model.OrderID = row["OrderID"].ToString();
            }
            if (row["SubOrderID"] != null && row["SubOrderID"].ToString() != "")
            {
                model.SubOrderID = row["SubOrderID"].ToString();
            }           
            if (row["OptType"] != null && row["OptType"].ToString() != "")
            {
                model.OptType = int.Parse(row["OptType"].ToString());
            }
            if (row["OrderStatus"] != null && row["OrderStatus"].ToString() != "")
            {
                model.OrderStatus = int.Parse(row["OrderStatus"].ToString());
            }
            if (row["RejectMsg"] != null && row["RejectMsg"].ToString() != "")
            {
                model.RejectMsg = row["RejectMsg"].ToString();
            }
            if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
           
            return model;
        }
        #endregion

        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetADOrderOperateInfo(QueryADOrderOperateInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID = '" + query.OrderID + "'";
            }
            if (query.SubOrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.SubOrderID = '" + query.SubOrderID +"'";
            }
            if (query.OptType != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.OptType = " + query.OptType;
            }
           

            //DateTime beginTime;
            //if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            //{
            //    where += " and a.CreateTime>='" + beginTime + "'";
            //}
            //DateTime endTime;
            //if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            //{
            //    where += " and a.CreateTime<'" + endTime.AddDays(1) + "'";
            //}

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderOperateInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Entities.ADOrderOperateInfo model)
        {
            SqlParameter[] parameters = {                                                       
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@CurrentStatus", SqlDbType.Int,4),
                    new SqlParameter("@OrderStatus", SqlDbType.Int,4),
                    new SqlParameter("@RejectMsg",SqlDbType.VarChar,200),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)                 
                                        };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.SubOrderID;
            parameters[2].Value = model.OptType;
            parameters[3].Value = model.CurrentStatus;
            parameters[4].Value = model.OrderStatus;
            parameters[5].Value = model.RejectMsg;
            parameters[6].Value = model.CreateUserID;

            int retval = 0;
            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderOperateInfo_Insert", parameters);
            return retval;
        }
        #endregion
        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.ADOrderOperateInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID",SqlDbType.Int,4),
					new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@CurrentStatus", SqlDbType.Int,4),
                    new SqlParameter("@OrderStatus", SqlDbType.Int,4),
                    new SqlParameter("@RejectMsg",SqlDbType.VarChar,200),
                                        };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.SubOrderID;            
            parameters[3].Value = model.OptType;
            parameters[4].Value = model.CurrentStatus;
            parameters[5].Value = model.OrderStatus;
            parameters[6].Value = model.RejectMsg; 

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderOperateInfo_Update", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.Int,4)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderOperateInfo_Delete", parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "p_ADOrderOperateInfo_Delete", parameters);
        }
        #endregion
      
    }
}
