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
    /// 数据访问类OrderRelpaceCarLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderRelpaceCarLog : DataBase
    {
        #region Instance
        public static readonly OrderRelpaceCarLog Instance = new OrderRelpaceCarLog();
        #endregion

        #region const
        private const string P_ORDERRELPACECARLOG_SELECT = "p_OrderRelpaceCarLog_Select";
        private const string P_ORDERRELPACECARLOG_INSERT = "p_OrderRelpaceCarLog_Insert";
        private const string P_ORDERRELPACECARLOG_UPDATE = "p_OrderRelpaceCarLog_Update";
        private const string P_ORDERRELPACECARLOG_DELETE = "p_OrderRelpaceCarLog_Delete";
        #endregion

        #region Contructor
        protected OrderRelpaceCarLog()
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
        public DataTable GetOrderRelpaceCarLog(QueryOrderRelpaceCarLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderRelpaceCarLog GetOrderRelpaceCarLog(long RecID)
        {
            QueryOrderRelpaceCarLog query = new QueryOrderRelpaceCarLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderRelpaceCarLog(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderRelpaceCarLog(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.OrderRelpaceCarLog LoadSingleOrderRelpaceCarLog(DataRow row)
        {
            Entities.OrderRelpaceCarLog model = new Entities.OrderRelpaceCarLog();

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
            if (row["ReplacementCarId"].ToString() != "")
            {
                model.ReplacementCarId = int.Parse(row["ReplacementCarId"].ToString());
            }
            if (row["ReplacementCarBuyYear"].ToString() != "")
            {
                model.ReplacementCarBuyYear = int.Parse(row["ReplacementCarBuyYear"].ToString());
            }
            if (row["ReplacementCarBuyMonth"].ToString() != "")
            {
                model.ReplacementCarBuyMonth = int.Parse(row["ReplacementCarBuyMonth"].ToString());
            }
            model.ReplacementCarColor = row["ReplacementCarColor"].ToString();
            if (row["ReplacementCarUsedMiles"].ToString() != "")
            {
                model.ReplacementCarUsedMiles = decimal.Parse(row["ReplacementCarUsedMiles"].ToString());
            }
            if (row["SalePrice"].ToString() != "")
            {
                model.SalePrice = decimal.Parse(row["SalePrice"].ToString());
            }
            if (row["ReplacementCarLocationID"].ToString() != "")
            {
                model.ReplacementCarLocationID = int.Parse(row["ReplacementCarLocationID"].ToString());
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
        public int Insert(Entities.OrderRelpaceCarLog model)
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
					new SqlParameter("@ReplacementCarId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@ReplacementCarLocationID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal,9)                   
                                        };
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
            parameters[19].Value = model.ReplacementCarId;
            parameters[20].Value = model.ReplacementCarBuyYear;
            parameters[21].Value = model.ReplacementCarBuyMonth;
            parameters[22].Value = model.ReplacementCarColor;
            parameters[23].Value = model.ReplacementCarUsedMiles;
            parameters[24].Value = model.ReplacementCarLocationID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.SalePrice;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderRelpaceCarLog model)
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
					new SqlParameter("@ReplacementCarId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@ReplacementCarLocationID", SqlDbType.Int,4),
             new SqlParameter("@SalePrice" ,SqlDbType.Decimal,9),
                                        new SqlParameter("@DealerID", SqlDbType.Int,4)};
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
            parameters[19].Value = model.ReplacementCarId;
            parameters[20].Value = model.ReplacementCarBuyYear;
            parameters[21].Value = model.ReplacementCarBuyMonth;
            parameters[22].Value = model.ReplacementCarColor;
            parameters[23].Value = model.ReplacementCarUsedMiles;
            parameters[24].Value = model.ReplacementCarLocationID;
            parameters[25].Value = model.SalePrice;
            parameters[26].Value = model.DealerID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_INSERT, parameters);
            model.RecID = (long)parameters[0].Value;
            return model.RecID;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderRelpaceCarLog model)
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
					new SqlParameter("@ReplacementCarId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@ReplacementCarLocationID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal,9)                    
                                        };
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
            parameters[19].Value = model.ReplacementCarId;
            parameters[20].Value = model.ReplacementCarBuyYear;
            parameters[21].Value = model.ReplacementCarBuyMonth;
            parameters[22].Value = model.ReplacementCarColor;
            parameters[23].Value = model.ReplacementCarUsedMiles;
            parameters[24].Value = model.ReplacementCarLocationID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.SalePrice;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderRelpaceCarLog model)
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
					new SqlParameter("@ReplacementCarId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@ReplacementCarLocationID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal,9)                   
                                        };
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
            parameters[19].Value = model.ReplacementCarId;
            parameters[20].Value = model.ReplacementCarBuyYear;
            parameters[21].Value = model.ReplacementCarBuyMonth;
            parameters[22].Value = model.ReplacementCarColor;
            parameters[23].Value = model.ReplacementCarUsedMiles;
            parameters[24].Value = model.ReplacementCarLocationID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.SalePrice;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECARLOG_DELETE, parameters);
        }
        #endregion


        /// <summary>
        /// 根据易湃订单ID，判断数据是否存在
        /// </summary>
        /// <param name="YPOrderID">易湃订单ID</param>
        /// <returns></returns>
        public bool IsExistsByYPOrderID(int YPOrderID)
        {
            string sql = "SELECT YPOrderID FROM dbo.OrderRelpaceCarLog WHERE YPOrderID=" + YPOrderID;
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
        public int GetMaxYPOrderID()
        {
            string sql = "SELECT ISNULL(MAX(YPOrderID),0) FROM dbo.OrderRelpaceCarLog";
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

