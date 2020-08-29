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
    /// 数据访问类WorkOrderRevert。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderRevert : DataBase
    {
        #region Instance
        public static readonly WorkOrderRevert Instance = new WorkOrderRevert();
        #endregion

        #region const
        private const string P_WORKORDERREVERT_SELECT = "p_WorkOrderRevert_Select";
        private const string P_WORKORDERREVERT_INSERT = "p_WorkOrderRevert_Insert";
        private const string P_WORKORDERREVERT_UPDATE = "p_WorkOrderRevert_Update";
        private const string P_WORKORDERREVERT_DELETE = "p_WorkOrderRevert_Delete";
        #endregion

        #region Contructor
        protected WorkOrderRevert()
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
        public DataTable GetWorkOrderRevert(QueryWorkOrderRevert query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.WORID != Constant.INT_INVALID_VALUE)
            {
                where += " and WORID=" + query.WORID;
            }

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " and orderID='" + StringHelper.SqlFilter(query.OrderID) + "'";
            }

            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " and CallID=" + query.CallID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERREVERT_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderRevert GetWorkOrderRevert(long WORID)
        {
            QueryWorkOrderRevert query = new QueryWorkOrderRevert();
            query.WORID = WORID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderRevert(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderRevert(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public DataTable GetWorkOrderRevertByOrderID(string OrderID, string OrderByStr)
        {
            QueryWorkOrderRevert query = new QueryWorkOrderRevert();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderRevert(query, OrderByStr, 1, 100000, out count);

            if (count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public Entities.WorkOrderRevert GetWorkOrderRevertByCallID(Int64 CallID)
        {
            QueryWorkOrderRevert query = new QueryWorkOrderRevert();
            query.CallID = CallID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderRevert(query, string.Empty, 1, 1, out count);

            if (count > 0)
            {
                return LoadSingleWorkOrderRevert(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderRevert LoadSingleWorkOrderRevert(DataRow row)
        {
            Entities.WorkOrderRevert model = new Entities.WorkOrderRevert();

            if (row["WORID"].ToString() != "")
            {
                model.WORID = long.Parse(row["WORID"].ToString());
            }
            model.OrderID = row["OrderID"].ToString();
            model.RevertContent = row["RevertContent"].ToString();
            model.CategoryName = row["CategoryName"].ToString();
            model.DataSource = row["DataSource"].ToString();
            model.CustName = row["CustName"].ToString();
            model.CRMCustID = row["CRMCustID"].ToString();
            model.ProvinceName = row["ProvinceName"].ToString();
            model.CityName = row["CityName"].ToString();
            model.CountyName = row["CountyName"].ToString();
            model.Contact = row["Contact"].ToString();
            model.ContactTel = row["ContactTel"].ToString();
            model.PriorityLevelName = row["PriorityLevelName"].ToString();
            model.LastProcessDate = row["LastProcessDate"].ToString();
            model.IsComplaintType = row["IsComplaintType"].ToString();
            model.Title = row["Title"].ToString();
            model.TagName = row["TagName"].ToString();
            model.WorkOrderStatus = row["WorkOrderStatus"].ToString();
            model.ReceiverID = row["ReceiverID"].ToString();
            model.ReceiverName = row["ReceiverName"].ToString();
            model.ReceiverDepartName = row["ReceiverDepartName"].ToString();
            model.IsSales = row["IsSales"].ToString();
            model.AttentionCarBrandName = row["AttentionCarBrandName"].ToString();
            model.AttentionCarSerialName = row["AttentionCarSerialName"].ToString();
            model.AttentionCarTypeName = row["AttentionCarTypeName"].ToString();
            model.SelectDealerID = row["SelectDealerID"].ToString();
            model.SelectDealerName = row["SelectDealerName"].ToString();
            model.NominateActivity = row["NominateActivity"].ToString();
            model.SaleCarBrandName = row["SaleCarBrandName"].ToString();
            model.SaleCarSerialName = row["SaleCarSerialName"].ToString();
            model.SaleCarTypeName = row["SaleCarTypeName"].ToString();
            model.IsReturnVisit = row["IsReturnVisit"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CallID"].ToString() != "")
            {
                model.CallID = Int64.Parse(row["CallID"].ToString());
            }
            if (row.Table.Columns.Contains("DemandID"))
            {
                if (row["DemandID"] != null)
                {
                    model.DemandID = row["DemandID"].ToString();
                }
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(Entities.WorkOrderRevert model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@WORID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@RevertContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CategoryName", SqlDbType.NVarChar,2000),
					new SqlParameter("@DataSource", SqlDbType.NVarChar,2000),
					new SqlParameter("@CustName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CRMCustID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CityName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CountyName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Contact", SqlDbType.NVarChar,2000),
					new SqlParameter("@ContactTel", SqlDbType.NVarChar,2000),
					new SqlParameter("@PriorityLevelName", SqlDbType.NVarChar,2000),
					new SqlParameter("@LastProcessDate", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsComplaintType", SqlDbType.NVarChar,2000),
					new SqlParameter("@Title", SqlDbType.NVarChar,2000),
					new SqlParameter("@TagName", SqlDbType.NVarChar,2000),
					new SqlParameter("@WorkOrderStatus", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsSales", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerID", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerName", SqlDbType.NVarChar,2000),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.CategoryName;
            parameters[4].Value = model.DataSource;
            parameters[5].Value = model.CustName;
            parameters[6].Value = model.CRMCustID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.CountyName;
            parameters[10].Value = model.Contact;
            parameters[11].Value = model.ContactTel;
            parameters[12].Value = model.PriorityLevelName;
            parameters[13].Value = model.LastProcessDate;
            parameters[14].Value = model.IsComplaintType;
            parameters[15].Value = model.Title;
            parameters[16].Value = model.TagName;
            parameters[17].Value = model.WorkOrderStatus;
            parameters[18].Value = model.ReceiverID;
            parameters[19].Value = model.ReceiverName;
            parameters[20].Value = model.ReceiverDepartName;
            parameters[21].Value = model.IsSales;
            parameters[22].Value = model.AttentionCarBrandName;
            parameters[23].Value = model.AttentionCarSerialName;
            parameters[24].Value = model.AttentionCarTypeName;
            parameters[25].Value = model.SelectDealerID;
            parameters[26].Value = model.SelectDealerName;
            parameters[27].Value = model.NominateActivity;
            parameters[28].Value = model.SaleCarBrandName;
            parameters[29].Value = model.SaleCarSerialName;
            parameters[30].Value = model.SaleCarTypeName;
            parameters[31].Value = model.IsReturnVisit;
            parameters[32].Value = model.CreateTime;
            parameters[33].Value = model.CreateUserID;
            parameters[34].Value = model.CallID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERREVERT_INSERT, parameters);
            return long.Parse(parameters[0].Value.ToString());
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.WorkOrderRevert model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@WORID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@RevertContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CategoryName", SqlDbType.NVarChar,2000),
					new SqlParameter("@DataSource", SqlDbType.NVarChar,2000),
					new SqlParameter("@CustName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CRMCustID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CityName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CountyName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Contact", SqlDbType.NVarChar,2000),
					new SqlParameter("@ContactTel", SqlDbType.NVarChar,2000),
					new SqlParameter("@PriorityLevelName", SqlDbType.NVarChar,2000),
					new SqlParameter("@LastProcessDate", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsComplaintType", SqlDbType.NVarChar,2000),
					new SqlParameter("@Title", SqlDbType.NVarChar,2000),
					new SqlParameter("@TagName", SqlDbType.NVarChar,2000),
					new SqlParameter("@WorkOrderStatus", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsSales", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerID", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerName", SqlDbType.NVarChar,2000),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.CategoryName;
            parameters[4].Value = model.DataSource;
            parameters[5].Value = model.CustName;
            parameters[6].Value = model.CRMCustID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.CountyName;
            parameters[10].Value = model.Contact;
            parameters[11].Value = model.ContactTel;
            parameters[12].Value = model.PriorityLevelName;
            parameters[13].Value = model.LastProcessDate;
            parameters[14].Value = model.IsComplaintType;
            parameters[15].Value = model.Title;
            parameters[16].Value = model.TagName;
            parameters[17].Value = model.WorkOrderStatus;
            parameters[18].Value = model.ReceiverID;
            parameters[19].Value = model.ReceiverName;
            parameters[20].Value = model.ReceiverDepartName;
            parameters[21].Value = model.IsSales;
            parameters[22].Value = model.AttentionCarBrandName;
            parameters[23].Value = model.AttentionCarSerialName;
            parameters[24].Value = model.AttentionCarTypeName;
            parameters[25].Value = model.SelectDealerID;
            parameters[26].Value = model.SelectDealerName;
            parameters[27].Value = model.NominateActivity;
            parameters[28].Value = model.SaleCarBrandName;
            parameters[29].Value = model.SaleCarSerialName;
            parameters[30].Value = model.SaleCarTypeName;
            parameters[31].Value = model.IsReturnVisit;
            parameters[32].Value = model.CreateTime;
            parameters[33].Value = model.CreateUserID;
            parameters[34].Value = model.CallID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERREVERT_INSERT, parameters);
            return long.Parse(parameters[0].Value.ToString());
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderRevert model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@WORID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@RevertContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CategoryName", SqlDbType.NVarChar,2000),
					new SqlParameter("@DataSource", SqlDbType.NVarChar,2000),
					new SqlParameter("@CustName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CRMCustID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CityName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CountyName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Contact", SqlDbType.NVarChar,2000),
					new SqlParameter("@ContactTel", SqlDbType.NVarChar,2000),
					new SqlParameter("@PriorityLevelName", SqlDbType.NVarChar,2000),
					new SqlParameter("@LastProcessDate", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsComplaintType", SqlDbType.NVarChar,2000),
					new SqlParameter("@Title", SqlDbType.NVarChar,2000),
					new SqlParameter("@TagName", SqlDbType.NVarChar,2000),
					new SqlParameter("@WorkOrderStatus", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsSales", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerID", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerName", SqlDbType.NVarChar,2000),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.WORID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.CategoryName;
            parameters[4].Value = model.DataSource;
            parameters[5].Value = model.CustName;
            parameters[6].Value = model.CRMCustID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.CountyName;
            parameters[10].Value = model.Contact;
            parameters[11].Value = model.ContactTel;
            parameters[12].Value = model.PriorityLevelName;
            parameters[13].Value = model.LastProcessDate;
            parameters[14].Value = model.IsComplaintType;
            parameters[15].Value = model.Title;
            parameters[16].Value = model.TagName;
            parameters[17].Value = model.WorkOrderStatus;
            parameters[18].Value = model.ReceiverID;
            parameters[19].Value = model.ReceiverName;
            parameters[20].Value = model.ReceiverDepartName;
            parameters[21].Value = model.IsSales;
            parameters[22].Value = model.AttentionCarBrandName;
            parameters[23].Value = model.AttentionCarSerialName;
            parameters[24].Value = model.AttentionCarTypeName;
            parameters[25].Value = model.SelectDealerID;
            parameters[26].Value = model.SelectDealerName;
            parameters[27].Value = model.NominateActivity;
            parameters[28].Value = model.SaleCarBrandName;
            parameters[29].Value = model.SaleCarSerialName;
            parameters[30].Value = model.SaleCarTypeName;
            parameters[31].Value = model.IsReturnVisit;
            parameters[32].Value = model.CreateTime;
            parameters[33].Value = model.CreateUserID;
            parameters[34].Value = model.CallID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERREVERT_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderRevert model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@WORID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.VarChar,20),
					new SqlParameter("@RevertContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CategoryName", SqlDbType.NVarChar,2000),
					new SqlParameter("@DataSource", SqlDbType.NVarChar,2000),
					new SqlParameter("@CustName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CRMCustID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CityName", SqlDbType.NVarChar,2000),
					new SqlParameter("@CountyName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Contact", SqlDbType.NVarChar,2000),
					new SqlParameter("@ContactTel", SqlDbType.NVarChar,2000),
					new SqlParameter("@PriorityLevelName", SqlDbType.NVarChar,2000),
					new SqlParameter("@LastProcessDate", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsComplaintType", SqlDbType.NVarChar,2000),
					new SqlParameter("@Title", SqlDbType.NVarChar,2000),
					new SqlParameter("@TagName", SqlDbType.NVarChar,2000),
					new SqlParameter("@WorkOrderStatus", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverID", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverName", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReceiverDepartName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsSales", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@AttentionCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerID", SqlDbType.NVarChar,2000),
					new SqlParameter("@SelectDealerName", SqlDbType.NVarChar,2000),
					new SqlParameter("@NominateActivity", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarBrandName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarSerialName", SqlDbType.NVarChar,2000),
					new SqlParameter("@SaleCarTypeName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8)};
            parameters[0].Value = model.WORID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.RevertContent;
            parameters[3].Value = model.CategoryName;
            parameters[4].Value = model.DataSource;
            parameters[5].Value = model.CustName;
            parameters[6].Value = model.CRMCustID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityName;
            parameters[9].Value = model.CountyName;
            parameters[10].Value = model.Contact;
            parameters[11].Value = model.ContactTel;
            parameters[12].Value = model.PriorityLevelName;
            parameters[13].Value = model.LastProcessDate;
            parameters[14].Value = model.IsComplaintType;
            parameters[15].Value = model.Title;
            parameters[16].Value = model.TagName;
            parameters[17].Value = model.WorkOrderStatus;
            parameters[18].Value = model.ReceiverID;
            parameters[19].Value = model.ReceiverName;
            parameters[20].Value = model.ReceiverDepartName;
            parameters[21].Value = model.IsSales;
            parameters[22].Value = model.AttentionCarBrandName;
            parameters[23].Value = model.AttentionCarSerialName;
            parameters[24].Value = model.AttentionCarTypeName;
            parameters[25].Value = model.SelectDealerID;
            parameters[26].Value = model.SelectDealerName;
            parameters[27].Value = model.NominateActivity;
            parameters[28].Value = model.SaleCarBrandName;
            parameters[29].Value = model.SaleCarSerialName;
            parameters[30].Value = model.SaleCarTypeName;
            parameters[31].Value = model.IsReturnVisit;
            parameters[32].Value = model.CreateTime;
            parameters[33].Value = model.CreateUserID;
            parameters[34].Value = model.CallID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERREVERT_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long WORID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@WORID", SqlDbType.BigInt)};
            parameters[0].Value = WORID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERREVERT_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long WORID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@WORID", SqlDbType.BigInt)};
            parameters[0].Value = WORID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERREVERT_DELETE, parameters);
        }
        #endregion

    }
}

