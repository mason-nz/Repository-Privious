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
    /// 数据访问类GroupOrderOrg。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-11-04 09:34:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class GroupOrderOrg : DataBase
    {
        #region Instance
        public static readonly GroupOrderOrg Instance = new GroupOrderOrg();
        #endregion

        #region const
        private const string P_GROUPORDERORG_SELECT = "p_GroupOrderOrg_Select";
        private const string P_GROUPORDERORG_INSERT = "p_GroupOrderOrg_Insert";
        private const string P_GROUPORDERORG_UPDATE = "p_GroupOrderOrg_Update";
        private const string P_GROUPORDERORG_DELETE = "p_GroupOrderOrg_Delete";
        #endregion

        #region Contructor
        protected GroupOrderOrg()
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
        public DataTable GetGroupOrderOrg(QueryGroupOrderOrg query, string order, int currentPage, int pageSize, out int totalCount)
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERORG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        public Entities.GroupOrderOrg GetGroupOrderOrgByOrderID(int OrderID)
        {
            string sql = string.Format("SELECT * FROM dbo.GroupOrderOrg WHERE OrderID={0}", OrderID);
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleGroupOrderOrg(ds.Tables[0].Rows[0]);
            }
            return null;
        }


        /// <summary>
        /// 获取最大易湃订单ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID()
        {
            string sql = "SELECT ISNULL(MAX(OrderID),0) FROM dbo.GroupOrderOrg";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }

        /// <summary>
        /// 根据易湃订单ID，判断数据是否存在
        /// </summary>
        /// <param name="YPOrderID">易湃订单ID</param>
        /// <returns></returns>
        public bool IsExistsByYPOrderID(int YPOrderID)
        {
            string sql = "SELECT OrderID FROM dbo.GroupOrderOrg WHERE OrderID=" + YPOrderID;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.GroupOrderOrg GetGroupOrderOrg(long RecID)
        {
            QueryGroupOrderOrg query = new QueryGroupOrderOrg();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrderOrg(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleGroupOrderOrg(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.GroupOrderOrg LoadSingleGroupOrderOrg(DataRow row)
        {
            Entities.GroupOrderOrg model = new Entities.GroupOrderOrg();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["OrderID"].ToString() != "")
            {
                model.OrderID = int.Parse(row["OrderID"].ToString());
            }
            if (row["OrderCode"].ToString() != "")
            {
                model.OrderCode = int.Parse(row["OrderCode"].ToString());
            }
            model.CustomerName = row["CustomerName"].ToString();
            model.CustomerTel = row["CustomerTel"].ToString();
            if (row["ProvinceID"].ToString() != "")
            {
                model.ProvinceID = int.Parse(row["ProvinceID"].ToString());
            }
            model.ProvinceName = row["ProvinceName"].ToString();
            if (row["CityID"].ToString() != "")
            {
                model.CityID = int.Parse(row["CityID"].ToString());
            }
            model.CityName = row["CityName"].ToString();
            if (row["OrderCreateTime"].ToString() != "")
            {
                model.OrderCreateTime = DateTime.Parse(row["OrderCreateTime"].ToString());
            }
            if (row["Price"].ToString() != "")
            {
                model.Price = decimal.Parse(row["Price"].ToString());
            }
            if (row["CarMasterID"].ToString() != "")
            {
                model.CarMasterID = int.Parse(row["CarMasterID"].ToString());
            }
            model.CarMasterName = row["CarMasterName"].ToString();
            if (row["CarSerialID"].ToString() != "")
            {
                model.CarSerialID = int.Parse(row["CarSerialID"].ToString());
            }
            model.CarSerialName = row["CarSerialName"].ToString();
            if (row["CarID"].ToString() != "")
            {
                model.CarID = int.Parse(row["CarID"].ToString());
            }
            model.CarName = row["CarName"].ToString();
            if (row["DealerID"].ToString() != "")
            {
                model.DealerID = int.Parse(row["DealerID"].ToString());
            }
            model.DealerName = row["DealerName"].ToString();
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
        public int Insert(Entities.GroupOrderOrg model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@Price", SqlDbType.Decimal,9),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.ProvinceName;
            parameters[7].Value = model.CityID;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.OrderCreateTime;
            parameters[10].Value = model.Price;
            parameters[11].Value = model.CarMasterID;
            parameters[12].Value = model.CarMasterName;
            parameters[13].Value = model.CarSerialID;
            parameters[14].Value = model.CarSerialName;
            parameters[15].Value = model.CarID;
            parameters[16].Value = model.CarName;
            parameters[17].Value = model.DealerID;
            parameters[18].Value = model.DealerName;
            parameters[19].Value = model.CreateTime;
            parameters[20].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERORG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrderOrg model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@Price", SqlDbType.Decimal,18),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100)};
					//new SqlParameter("@CreateTime", SqlDbType.DateTime),
					//new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.ProvinceName;
            parameters[7].Value = model.CityID;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.OrderCreateTime;
            parameters[10].Value = model.Price;
            parameters[11].Value = model.CarMasterID;
            parameters[12].Value = model.CarMasterName;
            parameters[13].Value = model.CarSerialID;
            parameters[14].Value = model.CarSerialName;
            parameters[15].Value = model.CarID;
            parameters[16].Value = model.CarName;
            parameters[17].Value = model.DealerID;
            parameters[18].Value = model.DealerName;
            //parameters[19].Value = model.CreateTime;
            //parameters[20].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDERORG_INSERT, parameters);
            return (long)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.GroupOrderOrg model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@Price", SqlDbType.Decimal,9),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.ProvinceName;
            parameters[7].Value = model.CityID;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.OrderCreateTime;
            parameters[10].Value = model.Price;
            parameters[11].Value = model.CarMasterID;
            parameters[12].Value = model.CarMasterName;
            parameters[13].Value = model.CarSerialID;
            parameters[14].Value = model.CarSerialName;
            parameters[15].Value = model.CarID;
            parameters[16].Value = model.CarName;
            parameters[17].Value = model.DealerID;
            parameters[18].Value = model.DealerName;
            parameters[19].Value = model.CreateTime;
            parameters[20].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERORG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrderOrg model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@Price", SqlDbType.Decimal,9),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.ProvinceID;
            parameters[6].Value = model.ProvinceName;
            parameters[7].Value = model.CityID;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.OrderCreateTime;
            parameters[10].Value = model.Price;
            parameters[11].Value = model.CarMasterID;
            parameters[12].Value = model.CarMasterName;
            parameters[13].Value = model.CarSerialID;
            parameters[14].Value = model.CarSerialName;
            parameters[15].Value = model.CarID;
            parameters[16].Value = model.CarName;
            parameters[17].Value = model.DealerID;
            parameters[18].Value = model.DealerName;
            parameters[19].Value = model.CreateTime;
            parameters[20].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDERORG_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERORG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDERORG_DELETE, parameters);
        }
        #endregion
    }
}

