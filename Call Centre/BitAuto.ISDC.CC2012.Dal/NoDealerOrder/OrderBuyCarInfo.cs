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
    /// 数据访问类OrderBuyCarInfo。
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
    public class OrderBuyCarInfo : DataBase
    {
        #region Instance
        public static readonly OrderBuyCarInfo Instance = new OrderBuyCarInfo();
        #endregion

        #region const
        private const string P_ORDERBUYCARINFO_SELECT = "p_OrderBuyCarInfo_Select";
        private const string P_ORDERBUYCARINFO_INSERT = "p_OrderBuyCarInfo_Insert";
        private const string P_ORDERBUYCARINFO_UPDATE = "p_OrderBuyCarInfo_Update";
        private const string P_ORDERBUYCARINFO_DELETE = "p_OrderBuyCarInfo_Delete";
        #endregion

        #region Contructor
        protected OrderBuyCarInfo()
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
        public DataTable GetOrderBuyCarInfo(QueryOrderBuyCarInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " and TaskID=" + query.TaskID + "";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderBuyCarInfo GetOrderBuyCarInfo(long TaskID)
        {
            QueryOrderBuyCarInfo query = new QueryOrderBuyCarInfo();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderBuyCarInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderBuyCarInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.OrderBuyCarInfo LoadSingleOrderBuyCarInfo(DataRow row)
        {
            Entities.OrderBuyCarInfo model = new Entities.OrderBuyCarInfo();

            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
            }
            if (row["Type"].ToString() != "")
            {
                model.Type = int.Parse(row["Type"].ToString());
            }
            if (row["Age"].ToString() != "")
            {
                model.Age = int.Parse(row["Age"].ToString());
            }
            model.IDCard = row["IDCard"].ToString();
            if (row["Vocation"].ToString() != "")
            {
                model.Vocation = int.Parse(row["Vocation"].ToString());
            }
            if (row["Marriage"].ToString() != "")
            {
                model.Marriage = int.Parse(row["Marriage"].ToString());
            }
            if (row["Income"].ToString() != "")
            {
                model.Income = int.Parse(row["Income"].ToString());
            }
            if (row["CarBrandId"].ToString() != "")
            {
                model.CarBrandId = int.Parse(row["CarBrandId"].ToString());
            }
            if (row["CarSerialId"].ToString() != "")
            {
                model.CarSerialId = int.Parse(row["CarSerialId"].ToString());
            }
            if (row["CarTypeID"].ToString() != "")
            {
                model.CarTypeID = int.Parse(row["CarTypeID"].ToString());
            }
            model.CarName = row["CarName"].ToString();
            if (row["IsAttestation"].ToString() != "")
            {
                model.IsAttestation = int.Parse(row["IsAttestation"].ToString());
            }
            if (row["DriveAge"].ToString() != "")
            {
                model.DriveAge = int.Parse(row["DriveAge"].ToString());
            }
            model.UserName = row["UserName"].ToString();
            model.CarNo = row["CarNo"].ToString();
            model.Remark = row["Remark"].ToString();
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
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.OrderBuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.Int,4),
					new SqlParameter("@Income", SqlDbType.Int,4),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsAttestation", SqlDbType.Int,4),
					new SqlParameter("@DriveAge", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@CarNo", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@CarTypeID", SqlDbType.Int,4)
                    
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.Age;
            parameters[3].Value = model.IDCard;
            parameters[4].Value = model.Vocation;
            parameters[5].Value = model.Marriage;
            parameters[6].Value = model.Income;
            parameters[7].Value = model.CarBrandId;
            parameters[8].Value = model.CarSerialId;
            parameters[9].Value = model.CarName;
            parameters[10].Value = model.IsAttestation;
            parameters[11].Value = model.DriveAge;
            parameters[12].Value = model.UserName;
            parameters[13].Value = model.CarNo;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.Status;
            parameters[16].Value = model.CreateTime;
            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.LastModifyTime;
            parameters[19].Value = model.LastModifyUserID;
            parameters[20].Value = model.CarTypeID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.OrderBuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.Int,4),
					new SqlParameter("@Income", SqlDbType.Int,4),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsAttestation", SqlDbType.Int,4),
					new SqlParameter("@DriveAge", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@CarNo", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.Age;
            parameters[3].Value = model.IDCard;
            parameters[4].Value = model.Vocation;
            parameters[5].Value = model.Marriage;
            parameters[6].Value = model.Income;
            parameters[7].Value = model.CarBrandId;
            parameters[8].Value = model.CarSerialId;
            parameters[9].Value = model.CarName;
            parameters[10].Value = model.IsAttestation;
            parameters[11].Value = model.DriveAge;
            parameters[12].Value = model.UserName;
            parameters[13].Value = model.CarNo;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.Status;
            parameters[16].Value = model.CreateTime;
            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.LastModifyTime;
            parameters[19].Value = model.LastModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderBuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.Int,4),
					new SqlParameter("@Income", SqlDbType.Int,4),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsAttestation", SqlDbType.Int,4),
					new SqlParameter("@DriveAge", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@CarNo", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@CarTypeID", SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.Age;
            parameters[3].Value = model.IDCard;
            parameters[4].Value = model.Vocation;
            parameters[5].Value = model.Marriage;
            parameters[6].Value = model.Income;
            parameters[7].Value = model.CarBrandId;
            parameters[8].Value = model.CarSerialId;
            parameters[9].Value = model.CarName;
            parameters[10].Value = model.IsAttestation;
            parameters[11].Value = model.DriveAge;
            parameters[12].Value = model.UserName;
            parameters[13].Value = model.CarNo;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.Status;
            parameters[16].Value = model.CreateTime;
            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.LastModifyTime;
            parameters[19].Value = model.LastModifyUserID;
            parameters[20].Value = model.CarTypeID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderBuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.Int,4),
					new SqlParameter("@Income", SqlDbType.Int,4),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsAttestation", SqlDbType.Int,4),
					new SqlParameter("@DriveAge", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@CarNo", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.Age;
            parameters[3].Value = model.IDCard;
            parameters[4].Value = model.Vocation;
            parameters[5].Value = model.Marriage;
            parameters[6].Value = model.Income;
            parameters[7].Value = model.CarBrandId;
            parameters[8].Value = model.CarSerialId;
            parameters[9].Value = model.CarName;
            parameters[10].Value = model.IsAttestation;
            parameters[11].Value = model.DriveAge;
            parameters[12].Value = model.UserName;
            parameters[13].Value = model.CarNo;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.Status;
            parameters[16].Value = model.CreateTime;
            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.LastModifyTime;
            parameters[19].Value = model.LastModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERBUYCARINFO_DELETE, parameters);
        }
        #endregion

    }
}

