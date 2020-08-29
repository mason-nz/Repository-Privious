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
    /// 数据访问类OrderNewCarLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:31 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderNewCarLog : DataBase
    {
        #region Instance
        public static readonly OrderNewCarLog Instance = new OrderNewCarLog();
        #endregion

        #region const
        private const string P_ORDERNEWCARLOG_SELECT = "p_OrderNewCarLog_Select";
        private const string P_ORDERNEWCARLOG_INSERT = "p_OrderNewCarLog_Insert";
        private const string P_ORDERNEWCARLOG_UPDATE = "p_OrderNewCarLog_Update";
        private const string P_ORDERNEWCARLOG_DELETE = "p_OrderNewCarLog_Delete";
        #endregion

        #region Contructor
        protected OrderNewCarLog()
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
        public DataTable GetOrderNewCarLog(QueryOrderNewCarLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCARLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderNewCarLog GetOrderNewCarLog(long RecID)
        {
            QueryOrderNewCarLog query = new QueryOrderNewCarLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderNewCarLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderNewCarLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.OrderNewCarLog LoadSingleOrderNewCarLog(DataRow row)
        {
            Entities.OrderNewCarLog model = new Entities.OrderNewCarLog();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["YPOrderID"].ToString() != "")
            {
                model.YPOrderID = int.Parse(row["YPOrderID"].ToString());
            }
            model.UserName = row["UserName"].ToString();
            model.UserPhone = row["UserPhone"].ToString();
            model.UserMobile = row["UserMobile"].ToString();
            model.UserMail = row["UserMail"].ToString();
            if (row["UserGender"].ToString() != "")
            {
                model.UserGender = int.Parse(row["UserGender"].ToString());
            }
            if (row["LocationID"].ToString() != "")
            {
                model.LocationID = int.Parse(row["LocationID"].ToString());
            }
            model.LocationName = row["LocationName"].ToString();
            model.UserAddress = row["UserAddress"].ToString();
            if (row["OrderCreateTime"].ToString() != "")
            {
                model.OrderCreateTime = DateTime.Parse(row["OrderCreateTime"].ToString());
            }
            if (row["OrderPrice"].ToString() != "")
            {
                model.OrderPrice = decimal.Parse(row["OrderPrice"].ToString());
            }
            if (row["OrderQuantity"].ToString() != "")
            {
                model.OrderQuantity = int.Parse(row["OrderQuantity"].ToString());
            }
            model.OrderRemark = row["OrderRemark"].ToString();
            if (row["CarID"].ToString() != "")
            {
                model.CarID = int.Parse(row["CarID"].ToString());
            }
            model.CarFullName = row["CarFullName"].ToString();
            if (row["CarPrice"].ToString() != "")
            {
                model.CarPrice = decimal.Parse(row["CarPrice"].ToString());
            }
            model.CarColor = row["CarColor"].ToString();
            model.CarPromotions = row["CarPromotions"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row.Table.Columns.Contains("DealerID") && row["DealerID"].ToString() != "")
            {
                model.DealerID = int.Parse(row["DealerID"].ToString());
            }
            if (row.Table.Columns.Contains("OrderType") && row["OrderType"].ToString() != "")
            {
                model.OrderType = int.Parse(row["OrderType"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.OrderNewCarLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@LocationID", SqlDbType.Int,4),
					new SqlParameter("@LocationName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderQuantity", SqlDbType.Int,4),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarFullName", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@CarPromotions", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@DealerID", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.LocationID;
            parameters[8].Value = model.LocationName;
            parameters[9].Value = model.UserAddress;
            parameters[10].Value = model.OrderCreateTime;
            parameters[11].Value = model.OrderPrice;
            parameters[12].Value = model.OrderQuantity;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarID;
            parameters[15].Value = model.CarFullName;
            parameters[16].Value = model.CarPrice;
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.CarPromotions;
            parameters[19].Value = model.CreateTime;
            parameters[20].Value = model.DealerID;
            parameters[21].Value = model.OrderType;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCARLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderNewCarLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@LocationID", SqlDbType.Int,4),
					new SqlParameter("@LocationName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderQuantity", SqlDbType.Int,4),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarFullName", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@CarPromotions", SqlDbType.NVarChar,1024),
                    new SqlParameter("@DealerID", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4)};
            //new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.LocationID;
            parameters[8].Value = model.LocationName;
            parameters[9].Value = model.UserAddress;
            parameters[10].Value = model.OrderCreateTime;
            parameters[11].Value = model.OrderPrice;
            parameters[12].Value = model.OrderQuantity;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarID;
            parameters[15].Value = model.CarFullName;
            parameters[16].Value = model.CarPrice;
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.CarPromotions;
            parameters[19].Value = model.DealerID;
            parameters[20].Value = model.OrderType;
            //parameters[19].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERNEWCARLOG_INSERT, parameters);
            model.RecID = (long)parameters[0].Value;
            return model.RecID;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderNewCarLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@LocationID", SqlDbType.Int,4),
					new SqlParameter("@LocationName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderQuantity", SqlDbType.Int,4),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarFullName", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@CarPromotions", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.LocationID;
            parameters[8].Value = model.LocationName;
            parameters[9].Value = model.UserAddress;
            parameters[10].Value = model.OrderCreateTime;
            parameters[11].Value = model.OrderPrice;
            parameters[12].Value = model.OrderQuantity;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarID;
            parameters[15].Value = model.CarFullName;
            parameters[16].Value = model.CarPrice;
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.CarPromotions;
            parameters[19].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCARLOG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderNewCarLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@LocationID", SqlDbType.Int,4),
					new SqlParameter("@LocationName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderQuantity", SqlDbType.Int,4),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarFullName", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@CarPromotions", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.LocationID;
            parameters[8].Value = model.LocationName;
            parameters[9].Value = model.UserAddress;
            parameters[10].Value = model.OrderCreateTime;
            parameters[11].Value = model.OrderPrice;
            parameters[12].Value = model.OrderQuantity;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarID;
            parameters[15].Value = model.CarFullName;
            parameters[16].Value = model.CarPrice;
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.CarPromotions;
            parameters[19].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERNEWCARLOG_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCARLOG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCARLOG_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 根据易湃订单ID，判断数据是否存在
        /// </summary>
        /// <param name="YPOrderID">易湃订单ID</param>
        /// <returns></returns>
        public bool IsExistsByYPOrderID(int YPOrderID)
        {
            string sql = "SELECT YPOrderID FROM dbo.OrderNewCarLog WHERE YPOrderID=" + YPOrderID;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取最大易湃订单ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID(int ordertype)
        {
            string sql = "SELECT ISNULL(MAX(YPOrderID),0) FROM dbo.OrderNewCarLog Where ordertype=" + ordertype;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }
    }
}

