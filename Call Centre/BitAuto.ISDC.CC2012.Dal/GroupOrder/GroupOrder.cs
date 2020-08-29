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
    /// 数据访问类GroupOrder。
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
    public class GroupOrder : DataBase
    {
        #region Instance
        public static readonly GroupOrder Instance = new GroupOrder();
        #endregion

        #region const
        private const string P_GROUPORDER_SELECT = "p_GroupOrder_Select";
        private const string P_GROUPORDER_INSERT = "p_GroupOrder_Insert";
        private const string P_GROUPORDER_UPDATE = "p_GroupOrder_Update";
        private const string P_GROUPORDER_DELETE = "p_GroupOrder_Delete";
        #endregion

        #region Contructor
        protected GroupOrder()
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
        public DataTable GetGroupOrder(QueryGroupOrder query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 条件

            if (query.CustomerTel != Constant.STRING_INVALID_VALUE)
            {
                where += " AND CustomerTel='" + StringHelper.SqlFilter(query.CustomerTel) + "'";
            }

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID=" + query.TaskID.ToString();
            }
            if (query.IsUpdate != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID IN "
                        + "("
                        + "SELECT TaskID FROM dbo.UpdateOrderData u "
                        + "WHERE u.TaskID=dbo.GroupOrder.TaskID AND u.IsUpdate=-1 AND u.UpdateType=-1 AND u.APIType=4 "
                        + ") ";
            }
            #endregion

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        public DataTable GetGroupOrder(string wheresql)
        {

            DataSet ds;

            string sql = "SELECT * FROM dbo.GroupOrder WHERE 1=1 " + wheresql;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);

            return ds.Tables[0];
        }
        #endregion

        #region 获取订单查看页客户基本信息
        /// <summary>
        /// 获取订单查看页客户基本信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public Entities.GroupOrder GetGroupOrderCustInfo(long TaskID)
        {
            string sqlStr = "SELECT  gorder.* ,fa.ReasonName,task.TaskStatus,task.AssignUserID FROM dbo.GroupOrderTask task " +//"SELECT  gorder.* ,car.UserName,fa.ReasonName,task.TaskStatus,task.AssignUserID FROM dbo.GroupOrderTask task " +
                            "LEFT JOIN dbo.GroupOrder gorder ON task.TaskID = gorder.TaskID " +
                //"LEFT JOIN dbo.CustBasicInfo cust ON gorder.CustomerName = cust.CustName "+
                //"AND gorder.CustomerTel IN (SELECT  Tel FROM    dbo.CustTel tel WHERE   tel.CustID = cust.CustID )" +
                //"LEFT JOIN dbo.BuyCarInfo car ON cust.CustID = car.CustID " +
                            "LEFT JOIN dbo.GO_FailureReason fa ON gorder.FailReasonID=fa.RecID " +
                            " WHERE task.TaskID=@TaskID ";
            SqlParameter parameter = new SqlParameter("TaskID", TaskID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleGroupOrder2(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.GroupOrder GetGroupOrder(long TaskID)
        {
            QueryGroupOrder query = new QueryGroupOrder();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrder(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleGroupOrder(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.GroupOrder LoadSingleGroupOrder2(DataRow row)
        {
            Entities.GroupOrder model = new Entities.GroupOrder();

            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
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
            if (row["UserGender"].ToString() != "")
            {
                model.UserGender = int.Parse(row["UserGender"].ToString());
            }
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
            if (row["AreaID"].ToString() != "")
            {
                model.AreaID = row["AreaID"].ToString(); //int.Parse(row["AreaID"].ToString());
            }
            if (row["OrderCreateTime"].ToString() != "")
            {
                model.OrderCreateTime = DateTime.Parse(row["OrderCreateTime"].ToString());
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
            model.CallRecord = row["CallRecord"].ToString();
            if (row["IsReturnVisit"].ToString() != "")
            {
                model.IsReturnVisit = int.Parse(row["IsReturnVisit"].ToString());
            }
            if (row["OrderPrice"].ToString() != "")
            {
                model.OrderPrice = decimal.Parse(row["OrderPrice"].ToString());
            }
            if (row["FailReasonID"].ToString() != "")
            {
                model.FailReasonID = int.Parse(row["FailReasonID"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            if (row["LastUpdateUserID"].ToString() != "")
            {
                model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
            }
            if (row["UserName"].ToString() != "")
            {
                model.UserName = row["UserName"].ToString();
            }
            if (row["ReasonName"].ToString() != "")
            {
                model.FailReason = row["ReasonName"].ToString();
            }
            if (row["TaskStatus"].ToString() != "")
            {
                model.TaskStatus = int.Parse(row["TaskStatus"].ToString());
            }
            if (row["AssignUserID"].ToString() != "")
            {
                model.AssignUserID = int.Parse(row["AssignUserID"].ToString());
            }
            if (row["CustID"].ToString() != "")
            {
                model.CustID = row["CustID"].ToString();
            }
            if (row["WantCarMasterID"].ToString() != "")
            {
                model.WantCarMasterID = int.Parse(row["WantCarMasterID"].ToString());
            }
            if (row["WantCarSerialID"].ToString() != "")
            {
                model.WantCarSerialID = int.Parse(row["WantCarSerialID"].ToString());
            }
            if (row["WantCarID"].ToString() != "")
            {
                model.WantCarID = int.Parse(row["WantCarID"].ToString());
            }
            if (row["PlanBuyCarTime"].ToString() != "")
            {
                model.PlanBuyCarTime = int.Parse(row["PlanBuyCarTime"].ToString());
            }

            if (row["WantCarMasterName"].ToString() != "")
            {
                model.WantCarMasterName = row["WantCarMasterName"].ToString();
            }
            if (row["WantCarSerialName"].ToString() != "")
            {
                model.WantCarSerialName = row["WantCarSerialName"].ToString();
            }
            if (row["WantCarName"].ToString() != "")
            {
                model.WantCarName = row["WantCarName"].ToString();
            }
            return model;
        }
        private Entities.GroupOrder LoadSingleGroupOrder(DataRow row)
        {
            Entities.GroupOrder model = new Entities.GroupOrder();

            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
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
            if (row["UserGender"].ToString() != "")
            {
                model.UserGender = int.Parse(row["UserGender"].ToString());
            }
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
            if (row["AreaID"].ToString() != "")
            {
                model.AreaID = row["AreaID"].ToString(); //int.Parse(row["AreaID"].ToString());
            }
            if (row["OrderCreateTime"].ToString() != "")
            {
                model.OrderCreateTime = DateTime.Parse(row["OrderCreateTime"].ToString());
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
            model.CallRecord = row["CallRecord"].ToString();
            if (row["IsReturnVisit"].ToString() != "")
            {
                model.IsReturnVisit = int.Parse(row["IsReturnVisit"].ToString());
            }
            if (row["OrderPrice"].ToString() != "")
            {
                model.OrderPrice = decimal.Parse(row["OrderPrice"].ToString());
            }
            if (row["FailReasonID"].ToString() != "")
            {
                model.FailReasonID = int.Parse(row["FailReasonID"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            if (row["LastUpdateUserID"].ToString() != "")
            {
                model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
            }
            if (row["CustID"].ToString() != "")
            {
                model.CustID = row["CustID"].ToString();
            }
            if (row["UserName"].ToString() != "")
            {
                model.UserName = row["UserName"].ToString();
            }
            if (row["WantCarMasterID"].ToString() != "")
            {
                model.WantCarMasterID = int.Parse(row["WantCarMasterID"].ToString());
            }
            if (row["WantCarSerialID"].ToString() != "")
            {
                model.WantCarSerialID = int.Parse(row["WantCarSerialID"].ToString());
            }
            if (row["WantCarID"].ToString() != "")
            {
                model.WantCarID = int.Parse(row["WantCarID"].ToString());
            }
            if (row["PlanBuyCarTime"].ToString() != "")
            {
                model.PlanBuyCarTime = int.Parse(row["PlanBuyCarTime"].ToString());
            }

            if (row["WantCarMasterName"].ToString() != "")
            {
                model.WantCarMasterName = row["WantCarMasterName"].ToString();
            }
            if (row["WantCarSerialName"].ToString() != "")
            {
                model.WantCarSerialName = row["WantCarSerialName"].ToString();
            }
            if (row["WantCarName"].ToString() != "")
            {
                model.WantCarName = row["WantCarName"].ToString();
            }

            return model;
        }


        public List<Entities.GroupOrder> GetGroupOrderList(Entities.QueryGroupOrder query)
        {
            List<Entities.GroupOrder> list = new List<Entities.GroupOrder>();

            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrder(query, string.Empty, 1, 99999999, out count);
            if (count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleGroupOrder(dr));
                }
            }
            else
            {
                return null;
            }
            return list;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.GroupOrder model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.VarChar,50),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,18),
					new SqlParameter("@FailReasonID", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
                    new SqlParameter("@UserName",SqlDbType.NVarChar,200),
                    new SqlParameter("@CustID",SqlDbType.VarChar,20),
                    new SqlParameter("@WantCarMasterID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarSerialID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarMasterName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarSerialName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarName",SqlDbType.NVarChar,100),
                    new SqlParameter("@PlanBuyCarTime",SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.UserGender;
            parameters[6].Value = model.ProvinceID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CityName;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.OrderCreateTime;
            parameters[12].Value = model.CarMasterID;
            parameters[13].Value = model.CarMasterName;
            parameters[14].Value = model.CarSerialID;
            parameters[15].Value = model.CarSerialName;
            parameters[16].Value = model.CarID;
            parameters[17].Value = model.CarName;
            parameters[18].Value = model.DealerID;
            parameters[19].Value = model.DealerName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.IsReturnVisit;
            parameters[22].Value = model.OrderPrice;
            parameters[23].Value = model.FailReasonID;
            parameters[24].Value = model.CreateUserID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.LastUpdateTime;
            parameters[27].Value = model.LastUpdateUserID;
            parameters[28].Value = model.UserName;
            parameters[29].Value = model.CustID;
            parameters[30].Value = model.WantCarMasterID;
            parameters[31].Value = model.WantCarSerialID;
            parameters[32].Value = model.WantCarID;
            parameters[33].Value = model.WantCarMasterName;
            parameters[34].Value = model.WantCarSerialName;
            parameters[35].Value = model.WantCarName;
            parameters[36].Value = model.PlanBuyCarTime;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDER_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrder model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID",  SqlDbType.VarChar,50),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,18),
					new SqlParameter("@FailReasonID", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
                                        new SqlParameter("@UserName",SqlDbType.NVarChar,200),
                                        new SqlParameter("@CustID",SqlDbType.VarChar,20),
                                        new SqlParameter("@WantCarMasterID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarSerialID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarMasterName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarSerialName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarName",SqlDbType.NVarChar,100),
                    new SqlParameter("@PlanBuyCarTime",SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.UserGender;
            parameters[6].Value = model.ProvinceID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CityName;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.OrderCreateTime;
            parameters[12].Value = model.CarMasterID;
            parameters[13].Value = model.CarMasterName;
            parameters[14].Value = model.CarSerialID;
            parameters[15].Value = model.CarSerialName;
            parameters[16].Value = model.CarID;
            parameters[17].Value = model.CarName;
            parameters[18].Value = model.DealerID;
            parameters[19].Value = model.DealerName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.IsReturnVisit;
            parameters[22].Value = model.OrderPrice;
            parameters[23].Value = model.FailReasonID;
            parameters[24].Value = model.CreateUserID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.LastUpdateTime;
            parameters[27].Value = model.LastUpdateUserID;
            parameters[28].Value = model.UserName;
            parameters[29].Value = model.CustID;
            parameters[30].Value = model.WantCarMasterID;
            parameters[31].Value = model.WantCarSerialID;
            parameters[32].Value = model.WantCarID;
            parameters[33].Value = model.WantCarMasterName;
            parameters[34].Value = model.WantCarSerialName;
            parameters[35].Value = model.WantCarName;
            parameters[36].Value = model.PlanBuyCarTime;
            int ret = (int)SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDER_INSERT, parameters);
            if (ret > 0)
            {
                return model.TaskID;
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.GroupOrder model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.VarChar,50),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,9),
					new SqlParameter("@FailReasonID", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
                    new SqlParameter("@UserName",SqlDbType.NVarChar,200),
                    new SqlParameter("@CustID",SqlDbType.VarChar,20),
                    new SqlParameter("@WantCarMasterID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarSerialID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarMasterName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarSerialName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarName",SqlDbType.NVarChar,100),
                    new SqlParameter("@PlanBuyCarTime",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.UserGender;
            parameters[6].Value = model.ProvinceID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CityName;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.OrderCreateTime;
            parameters[12].Value = model.CarMasterID;
            parameters[13].Value = model.CarMasterName;
            parameters[14].Value = model.CarSerialID;
            parameters[15].Value = model.CarSerialName;
            parameters[16].Value = model.CarID;
            parameters[17].Value = model.CarName;
            parameters[18].Value = model.DealerID;
            parameters[19].Value = model.DealerName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.IsReturnVisit;
            parameters[22].Value = model.OrderPrice;
            parameters[23].Value = model.FailReasonID;
            parameters[24].Value = model.CreateUserID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.LastUpdateTime;
            parameters[27].Value = model.LastUpdateUserID;
            parameters[28].Value = model.UserName;
            parameters[29].Value = model.CustID;
            parameters[30].Value = model.WantCarMasterID;
            parameters[31].Value = model.WantCarSerialID;
            parameters[32].Value = model.WantCarID;
            parameters[33].Value = model.WantCarMasterName;
            parameters[34].Value = model.WantCarSerialName;
            parameters[35].Value = model.WantCarName;
            parameters[36].Value = model.PlanBuyCarTime;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDER_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrder model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@OrderCode", SqlDbType.Int,4),
					new SqlParameter("@CustomerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CustomerTel", SqlDbType.NVarChar,50),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@ProvinceName", SqlDbType.NVarChar,100),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CityName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.VarChar,50),
					new SqlParameter("@OrderCreateTime", SqlDbType.DateTime),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarMasterName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialName", SqlDbType.NVarChar,100),
					new SqlParameter("@CarID", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,100),
					new SqlParameter("@DealerID", SqlDbType.Int,4),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,100),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsReturnVisit", SqlDbType.Int,4),
					new SqlParameter("@OrderPrice", SqlDbType.Decimal,9),
					new SqlParameter("@FailReasonID", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
                    new SqlParameter("@UserName",SqlDbType.NVarChar,200),
                    new SqlParameter("@CustID",SqlDbType.VarChar,20),
                    new SqlParameter("@WantCarMasterID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarSerialID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarID",SqlDbType.Int,4),
                    new SqlParameter("@WantCarMasterName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarSerialName",SqlDbType.NVarChar,100),
                    new SqlParameter("@WantCarName",SqlDbType.NVarChar,100),
                    new SqlParameter("@PlanBuyCarTime",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.CustomerName;
            parameters[4].Value = model.CustomerTel;
            parameters[5].Value = model.UserGender;
            parameters[6].Value = model.ProvinceID;
            parameters[7].Value = model.ProvinceName;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CityName;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.OrderCreateTime;
            parameters[12].Value = model.CarMasterID;
            parameters[13].Value = model.CarMasterName;
            parameters[14].Value = model.CarSerialID;
            parameters[15].Value = model.CarSerialName;
            parameters[16].Value = model.CarID;
            parameters[17].Value = model.CarName;
            parameters[18].Value = model.DealerID;
            parameters[19].Value = model.DealerName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.IsReturnVisit;
            parameters[22].Value = model.OrderPrice;
            parameters[23].Value = model.FailReasonID;
            parameters[24].Value = model.CreateUserID;
            parameters[25].Value = model.CreateTime;
            parameters[26].Value = model.LastUpdateTime;
            parameters[27].Value = model.LastUpdateUserID;
            parameters[28].Value = model.UserName;
            parameters[29].Value = model.CustID;
            parameters[30].Value = model.WantCarMasterID;
            parameters[31].Value = model.WantCarSerialID;
            parameters[32].Value = model.WantCarID;
            parameters[33].Value = model.WantCarMasterName;
            parameters[34].Value = model.WantCarSerialName;
            parameters[35].Value = model.WantCarName;
            parameters[36].Value = model.PlanBuyCarTime;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDER_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDER_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDER_DELETE, parameters);
        }
        #endregion


    }
}

