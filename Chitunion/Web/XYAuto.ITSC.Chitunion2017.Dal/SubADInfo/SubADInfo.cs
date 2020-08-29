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
    public class SubADInfo : DataBase
    {
        public const string P_SubADInfo_SELECT = "p_SubADInfo_Select";

        #region Instance
        public static readonly SubADInfo Instance = new SubADInfo();
        #endregion


        #region 更新子工单金额
        /// <summary>
        /// 更新子工单金额
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="suborderid"></param>
        /// <returns></returns>
        public int UpdateTotalAmmount_SubADInfo(decimal amount,string suborderid)
        {
            int retval = 0;
            string sqlstr = string.Format("UPDATE dbo.SubADInfo SET TotalAmount={0} WHERE SubOrderID='{1}'",amount,suborderid);
            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);

            return retval;
        }
        #endregion
        #region Select
        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.SubADInfo GetModel(string SubOrderID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,OrderID,SubOrderID,MediaType,MediaID,TotalAmount,Status,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
            strSql.Append(" FROM SubADInfo ");
            strSql.Append(" WHERE SubOrderID=@SubOrderID");
            SqlParameter[] parameters = {
					new SqlParameter("@SubOrderID", SqlDbType.VarChar,20)
			};
            parameters[0].Value = SubOrderID;

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
        public Entities.SubADInfo DataRowToModel(DataRow row)
        {
            Entities.SubADInfo model = new Entities.SubADInfo();
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
                if (row["SubOrderID"] != null)
                {
                    model.SubOrderID = row["SubOrderID"].ToString();
                }
                
                if (row["MediaType"] != null && row["MediaType"].ToString() != "")
                {
                    model.MediaType = int.Parse(row["MediaType"].ToString());
                }

                if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                {
                    model.MediaID = int.Parse(row["MediaID"].ToString());
                }
                if (row["TotalAmount"] != null && row["TotalAmount"].ToString() != "")
                {
                    model.TotalAmount = decimal.Parse(row["TotalAmount"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
                }

                if (row["LastUpdateTime"] != null && row["LastUpdateTime"].ToString() != "")
                {
                    model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
                }
                if (row["LastUpdateUserID"] != null && row["LastUpdateUserID"].ToString() != "")
                {
                    model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
                }
            }
            return model;
        }

        #region 根据订单号查询得到一个对象实体
        public Entities.SubADInfo GetSubADInfo(string subOrderID)
        {
            QuerySubADInfo query = new QuerySubADInfo();
            query.SubOrderID = subOrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSubADInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSubADInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SubADInfo LoadSingleSubADInfo(DataRow row)
        {
            Entities.SubADInfo model = new Entities.SubADInfo();
            model.RecID = int.Parse(row["RecID"].ToString());
            model.OrderID = row["OrderID"].ToString();
            model.SubOrderID = row["SubOrderID"].ToString();

            if (row["MediaType"] != null && row["MediaType"].ToString() != "")
            {
                model.MediaType = int.Parse(row["MediaType"].ToString());
            }

            if (row["MediaID"] != null && row["MediaID"].ToString() != "")
            {
                model.MediaID = int.Parse(row["MediaID"].ToString());
            }
            if (row["TotalAmount"] != null && row["TotalAmount"].ToString() != "")
            {
                model.TotalAmount = decimal.Parse(row["TotalAmount"].ToString());
            }
            if (row["Status"] != null && row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }

            if (row["LastUpdateTime"] != null && row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            if (row["LastUpdateUserID"] != null && row["LastUpdateUserID"].ToString() != "")
            {
                model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
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
        public DataTable GetSubADInfo(QuerySubADInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID = '" + query.OrderID + "'";
            }
            if (query.SubOrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.SubOrderID = '" + query.SubOrderID + "'";
            }
            

            DateTime beginTime;
            if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            {
                where += " and a.CreateTime>='" + beginTime + "'";
            }
            DateTime endTime;
            if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            {
                where += " and a.CreateTime<'" + endTime.AddDays(1) + "'";
            }

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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        public DataTable GetSubADInfoByOrderID(string orderID)
        {
            string sqlstr = "SELECT * FROM dbo.SubADInfo WHERE OrderID=@OrderID ORDER BY CreateTime DESC";
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.NVarChar, 50)
                    };
            parameters[0].Value = orderID;           

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return ds.Tables[0];
        }
        #endregion
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(Entities.SubADInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),  
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),                                     
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID",SqlDbType.Int,4),
                    new SqlParameter("@TotalAmount",SqlDbType.Decimal,18),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@LastUpdateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.MediaType;
            parameters[3].Value = model.MediaID;
            parameters[4].Value = model.TotalAmount;
            parameters[5].Value = model.Status;          
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.LastUpdateTime;
            parameters[8].Value = model.LastUpdateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_Insert", parameters);
            return (string)parameters[0].Value;
        }
        #endregion
        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.SubADInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),  
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),                                     
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID",SqlDbType.Int,4),
                    new SqlParameter("@TotalAmount",SqlDbType.Decimal,18),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@LastUpdateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.SubOrderID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.MediaType;
            parameters[3].Value = model.MediaID;
            parameters[4].Value = model.TotalAmount;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.MediaType;
            parameters[7].Value = model.TotalAmount;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.LastUpdateTime;
            parameters[12].Value = model.LastUpdateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_Update", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string SubOrderID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SubOrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = SubOrderID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_Delete", parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "p_SubADInfo_Delete", parameters);
        }
        #endregion

        #region 根据主订单删除子订单
        /// <summary>
        /// 根据主订单删除子订单
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <returns></returns>
        public int DeleteByOrerID(string orderid)
        {
            string sqlstr = string.Format("DELETE dbo.SubADInfo WHERE OrderID ='{0}'",orderid);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 更改子订单状态
        /// <summary>
        /// 根据子订单号更改子订单状态
        /// </summary>
        /// <param name="orderid">子订单号</param>
        /// <param name="status">状态，具体值看订单状态枚举</param>
        /// <returns></returns>
        public int UpdateStatus_SubADInfo(string suborderid, int status)
        {
            string sqlstr = "UPDATE dbo.SubADInfo SET Status=" + status + " WHERE SubOrderID='" + suborderid + "'";
            int retval = 0;

            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return retval;
        }
        #endregion

        #region 根据主订单号更新子订单状态
        /// <summary>
        /// 根据主订单号更新子订单状态
        /// </summary>
        /// <param name="orderid">主订单</param>
        /// <param name="status">状态，具体值看订单状态枚举</param>
        /// <returns></returns>
        public int UpdateStatusByOrderID_SubADInfo(string orderid,int status)
        {
            string sqlstr = "UPDATE dbo.SubADInfo SET Status=" + status + " WHERE OrderID='" + orderid + "'";
            int retval = 0;

            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return retval;
        }
        #endregion

        #region 根据子订单状态修改项目状态(执行完毕、订单完成)
        /// <summary>
        /// 根据子订单状态修改项目状态(执行完毕、订单完成)
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <param name="status">执行完毕、订单完成</param>
        public int UpdateStatusADOrder_OrderID(string orderid, int status)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@Status", SqlDbType.Int, 4)};
            parameters[0].Value = orderid;
            parameters[1].Value = status;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_UpdateStatusADOrder", parameters);
        }
        #endregion

        #region 针对订单取消状态，所有子订单都取消后更改项目为取消状态
        /// <summary>
        /// 针对订单取消状态，所有子订单都取消后更改项目为取消状态)
        /// </summary>
        /// <param name="orderid">主订单号</param>
        public int UpdateStatusADOrder_OrderID(string orderid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@OrderID", SqlDbType.VarChar,20)};
            parameters[0].Value = orderid;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_UpdateStatusADOrder_Cancel", parameters);
        }
        #endregion

        #region AE 订单执行、取消、执行完毕、已完成 权限验证
        /// <summary>
        /// AE 订单执行、取消、执行完毕、已完成 权限验证
        /// </summary>
        /// <param name="UserID">AE的UserID</param>
        /// <param name="SubOrderID">订单号</param>
        /// <param name="Status">要设置的状态</param>
        /// <returns></returns>
        public string p_SubADInfoStatus_UpdatePrivilege(int UserID,string SubOrderID,int Status)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@SubOrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@Msg",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = UserID;
            parameters[1].Value = SubOrderID;
            parameters[2].Value = Status;
            parameters[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfoStatus_UpdatePrivilege", parameters);

            return (string)parameters[3].Value;
        }
        #endregion

        #region 子订单在设置完成状态时 判断是否有上传数据
        public string p_SubADInfoOrderFeedbackData_Select(string suborderid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SubOrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@Msg",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = suborderid;          
            parameters[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfoOrderFeedbackData_Select", parameters);

            return (string)parameters[1].Value; 
        }
        #endregion

        #region 根据订单号查询
        public void QuerySubADInfoBySubOrderID(string subOrderID, out int mediaType,out DataTable dtADOrderInfo, out DataTable dtMOI, out DataTable dtSubADInfo,out DataTable dtADDetail,out DataTable dtOperateInfo)
        {
            mediaType = -2;
            dtADOrderInfo = null;
            dtMOI = null;
            dtSubADInfo = null;
            dtADDetail = null;
            dtOperateInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@subOrderID",subOrderID),
                new SqlParameter("@MediaType",SqlDbType.Int,4)
            };
            parameters[1].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_QuerySubADInfoBySubOrderIDV1_1_8", parameters);
            mediaType = (int)parameters[1].Value;
            dtADOrderInfo = ds.Tables[0];
            dtMOI = ds.Tables[1];
            dtSubADInfo = ds.Tables[2];
            dtADDetail = ds.Tables[3];
            dtOperateInfo = ds.Tables[4];
        }
        #endregion
        #region 提交修改项目V1.1.4
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertV1_1_4(Entities.SubADInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID",SqlDbType.Int,4),
                    new SqlParameter("@TotalAmount",SqlDbType.Decimal,18),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@LastUpdateUserID",SqlDbType.Int,4),
                    new SqlParameter("@ChannelID",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.MediaType;
            parameters[3].Value = model.MediaID;
            parameters[4].Value = model.TotalAmount;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.LastUpdateTime;
            parameters[8].Value = model.LastUpdateUserID;
            parameters[9].Value = model.ChannelID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SubADInfo_InsertV1_1_4", parameters);
            return (string)parameters[0].Value;
        }
        #endregion
        #endregion
    }
}
