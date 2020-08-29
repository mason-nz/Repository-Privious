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
    /// 数据访问类OrderRelpaceCar。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 05:58:24 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderRelpaceCar : DataBase
    {
        #region Instance
        public static readonly OrderRelpaceCar Instance = new OrderRelpaceCar();
        #endregion

        #region const
        private const string P_ORDERRELPACECAR_SELECT = "p_OrderRelpaceCar_Select";
        private const string P_ORDERRELPACECAR_INSERT = "p_OrderRelpaceCar_Insert";
        private const string P_ORDERRELPACECAR_UPDATE = "p_OrderRelpaceCar_Update";
        private const string P_ORDERRELPACECAR_DELETE = "p_OrderRelpaceCar_Delete";
        #endregion

        #region Contructor
        protected OrderRelpaceCar()
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
        public DataTable GetOrderRelpaceCar(QueryOrderRelpaceCar query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 条件

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID=" + query.TaskID;
            }

            if (query.IsUpdate != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID IN "
                        + "("
                        + "SELECT TaskID FROM dbo.UpdateOrderData u "
                        + "WHERE u.TaskID=dbo.OrderRelpaceCar.TaskID AND u.IsUpdate=-1 AND u.UpdateType=-1 And APIType=1"
                        + ")";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECAR_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderRelpaceCar GetOrderRelpaceCar(long TaskID)
        {
            QueryOrderRelpaceCar query = new QueryOrderRelpaceCar();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderRelpaceCar(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderRelpaceCar(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public List<Entities.OrderRelpaceCar> GetOrderRelpaceCarList(Entities.QueryOrderRelpaceCar query)
        {
            List<Entities.OrderRelpaceCar> list = new List<Entities.OrderRelpaceCar>();

            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderRelpaceCar(query, string.Empty, 1, 99999999, out count);
            if (count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleOrderRelpaceCar(dr));
                }
            }
            else
            {
                return null;
            }
            return list;
        }
        private Entities.OrderRelpaceCar LoadSingleOrderRelpaceCar(DataRow row)
        {
            Entities.OrderRelpaceCar model = new Entities.OrderRelpaceCar();

            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
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
            if (row["ProvinceID"].ToString() != "")
            {
                model.ProvinceID = int.Parse(row["ProvinceID"].ToString());
            }
            if (row["CityID"].ToString() != "")
            {
                model.CityID = int.Parse(row["CityID"].ToString());
            }
            if (row["CountyID"].ToString() != "")
            {
                model.CountyID = int.Parse(row["CountyID"].ToString());
            }
            if (row["AreaID"].ToString() != "")
            {
                model.AreaID = int.Parse(row["AreaID"].ToString());
            }
            model.UserAddress = row["UserAddress"].ToString();
            if (row["OrderCreateTime"].ToString() != "")
            {
                model.OrderCreateTime = DateTime.Parse(row["OrderCreateTime"].ToString());
            }
            model.OrderRemark = row["OrderRemark"].ToString();
            if (row["CarMasterID"].ToString() != "")
            {
                model.CarMasterID = int.Parse(row["CarMasterID"].ToString());
            }
            if (row["CarSerialID"].ToString() != "")
            {
                model.CarSerialID = int.Parse(row["CarSerialID"].ToString());
            }
            if (row["CarTypeID"].ToString() != "")
            {
                model.CarTypeID = int.Parse(row["CarTypeID"].ToString());
            }
            if (row["CarPrice"].ToString() != "")
            {
                model.CarPrice = decimal.Parse(row["CarPrice"].ToString());
            }
            model.CarColor = row["CarColor"].ToString();
            if (row["RepCarMasterID"].ToString() != "")
            {
                model.RepCarMasterID = int.Parse(row["RepCarMasterID"].ToString());
            }
            if (row["RepCarSerialID"].ToString() != "")
            {
                model.RepCarSerialID = int.Parse(row["RepCarSerialID"].ToString());
            }
            if (row["RepCarTypeId"].ToString() != "")
            {
                model.RepCarTypeId = int.Parse(row["RepCarTypeId"].ToString());
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
            if (row["RepCarProvinceID"].ToString() != "")
            {
                model.RepCarProvinceID = int.Parse(row["RepCarProvinceID"].ToString());
            }
            if (row["RepCarCityID"].ToString() != "")
            {
                model.RepCarCityID = int.Parse(row["RepCarCityID"].ToString());
            }
            if (row["RepCarCountyID"].ToString() != "")
            {
                model.RepCarCountyID = int.Parse(row["RepCarCountyID"].ToString());
            }
            model.DMSMemberCode = row["DMSMemberCode"].ToString();
            model.DMSMemberName = row["DMSMemberName"].ToString();
            model.CallRecord = row["CallRecord"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["DealerID"].ToString() != "")
            {
                model.DealerID = int.Parse(row["DealerID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.OrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarTypeID", SqlDbType.Int,4),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@RepCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@RepCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@RepCarTypeId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@RepCarProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCityID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCountyID", SqlDbType.Int,4),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal,9)
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CountyID;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.UserAddress;
            parameters[12].Value = model.OrderCreateTime;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarMasterID;
            parameters[15].Value = model.CarSerialID;
            parameters[16].Value = model.CarTypeID;
            parameters[17].Value = model.CarPrice;
            parameters[18].Value = model.CarColor;
            parameters[19].Value = model.RepCarMasterID;
            parameters[20].Value = model.RepCarSerialID;
            parameters[21].Value = model.RepCarTypeId;
            parameters[22].Value = model.ReplacementCarBuyYear;
            parameters[23].Value = model.ReplacementCarBuyMonth;
            parameters[24].Value = model.ReplacementCarColor;
            parameters[25].Value = model.ReplacementCarUsedMiles;
            parameters[26].Value = model.RepCarProvinceID;
            parameters[27].Value = model.RepCarCityID;
            parameters[28].Value = model.RepCarCountyID;
            parameters[29].Value = model.DMSMemberCode;
            parameters[30].Value = model.DMSMemberName;
            parameters[31].Value = model.CallRecord;
            parameters[32].Value = model.Status;
            parameters[33].Value = model.CreateTime;
            parameters[34].Value = model.SalePrice;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECAR_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.OrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarTypeID", SqlDbType.Int,4),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@RepCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@RepCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@RepCarTypeId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@RepCarProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCityID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCountyID", SqlDbType.Int,4),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal,9),                  
                    new SqlParameter("@DealerID", SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CountyID;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.UserAddress;
            parameters[12].Value = model.OrderCreateTime;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarMasterID;
            parameters[15].Value = model.CarSerialID;
            parameters[16].Value = model.CarTypeID;
            parameters[17].Value = model.CarPrice;
            parameters[18].Value = model.CarColor;
            parameters[19].Value = model.RepCarMasterID;
            parameters[20].Value = model.RepCarSerialID;
            parameters[21].Value = model.RepCarTypeId;
            parameters[22].Value = model.ReplacementCarBuyYear;
            parameters[23].Value = model.ReplacementCarBuyMonth;
            parameters[24].Value = model.ReplacementCarColor;
            parameters[25].Value = model.ReplacementCarUsedMiles;
            parameters[26].Value = model.RepCarProvinceID;
            parameters[27].Value = model.RepCarCityID;
            parameters[28].Value = model.RepCarCountyID;
            parameters[29].Value = model.DMSMemberCode;
            parameters[30].Value = model.DMSMemberName;
            parameters[31].Value = model.CallRecord;
            parameters[32].Value = model.Status;
            parameters[33].Value = model.CreateTime;
            parameters[34].Value = model.SalePrice;
            parameters[35].Value = model.DealerID;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERRELPACECAR_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarTypeID", SqlDbType.Int,4),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@RepCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@RepCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@RepCarTypeId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@RepCarProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCityID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCountyID", SqlDbType.Int,4),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal,9)                     
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CountyID;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.UserAddress;
            parameters[12].Value = model.OrderCreateTime;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarMasterID;
            parameters[15].Value = model.CarSerialID;
            parameters[16].Value = model.CarTypeID;
            parameters[17].Value = model.CarPrice;
            parameters[18].Value = model.CarColor;
            parameters[19].Value = model.RepCarMasterID;
            parameters[20].Value = model.RepCarSerialID;
            parameters[21].Value = model.RepCarTypeId;
            parameters[22].Value = model.ReplacementCarBuyYear;
            parameters[23].Value = model.ReplacementCarBuyMonth;
            parameters[24].Value = model.ReplacementCarColor;
            parameters[25].Value = model.ReplacementCarUsedMiles;
            parameters[26].Value = model.RepCarProvinceID;
            parameters[27].Value = model.RepCarCityID;
            parameters[28].Value = model.RepCarCountyID;
            parameters[29].Value = model.DMSMemberCode;
            parameters[30].Value = model.DMSMemberName;
            parameters[31].Value = model.CallRecord;
            parameters[32].Value = model.Status;
            parameters[33].Value = model.CreateTime;
            parameters[34].Value = model.SalePrice;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECAR_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@YPOrderID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,64),
					new SqlParameter("@UserPhone", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMobile", SqlDbType.NVarChar,64),
					new SqlParameter("@UserMail", SqlDbType.NVarChar,64),
					new SqlParameter("@UserGender", SqlDbType.Int,4),
					new SqlParameter("@ProvinceID", SqlDbType.Int,4),
					new SqlParameter("@CityID", SqlDbType.Int,4),
					new SqlParameter("@CountyID", SqlDbType.Int,4),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@UserAddress", SqlDbType.NVarChar,1024),
					new SqlParameter("@OrderCreateTime", SqlDbType.SmallDateTime),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CarMasterID", SqlDbType.Int,4),
					new SqlParameter("@CarSerialID", SqlDbType.Int,4),
					new SqlParameter("@CarTypeID", SqlDbType.Int,4),
					new SqlParameter("@CarPrice", SqlDbType.Decimal,9),
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@RepCarMasterID", SqlDbType.Int,4),
					new SqlParameter("@RepCarSerialID", SqlDbType.Int,4),
					new SqlParameter("@RepCarTypeId", SqlDbType.Int,4),
					new SqlParameter("@ReplacementCarBuyYear", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarBuyMonth", SqlDbType.SmallInt,2),
					new SqlParameter("@ReplacementCarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@ReplacementCarUsedMiles", SqlDbType.Decimal,9),
					new SqlParameter("@RepCarProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCityID", SqlDbType.Int,4),
					new SqlParameter("@RepCarCountyID", SqlDbType.Int,4),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                                          new SqlParameter("@SalePrice", SqlDbType.Decimal,9)   
                                        };
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.YPOrderID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.UserPhone;
            parameters[4].Value = model.UserMobile;
            parameters[5].Value = model.UserMail;
            parameters[6].Value = model.UserGender;
            parameters[7].Value = model.ProvinceID;
            parameters[8].Value = model.CityID;
            parameters[9].Value = model.CountyID;
            parameters[10].Value = model.AreaID;
            parameters[11].Value = model.UserAddress;
            parameters[12].Value = model.OrderCreateTime;
            parameters[13].Value = model.OrderRemark;
            parameters[14].Value = model.CarMasterID;
            parameters[15].Value = model.CarSerialID;
            parameters[16].Value = model.CarTypeID;
            parameters[17].Value = model.CarPrice;
            parameters[18].Value = model.CarColor;
            parameters[19].Value = model.RepCarMasterID;
            parameters[20].Value = model.RepCarSerialID;
            parameters[21].Value = model.RepCarTypeId;
            parameters[22].Value = model.ReplacementCarBuyYear;
            parameters[23].Value = model.ReplacementCarBuyMonth;
            parameters[24].Value = model.ReplacementCarColor;
            parameters[25].Value = model.ReplacementCarUsedMiles;
            parameters[26].Value = model.RepCarProvinceID;
            parameters[27].Value = model.RepCarCityID;
            parameters[28].Value = model.RepCarCountyID;
            parameters[29].Value = model.DMSMemberCode;
            parameters[30].Value = model.DMSMemberName;
            parameters[31].Value = model.CallRecord;
            parameters[32].Value = model.Status;
            parameters[33].Value = model.CreateTime;
            parameters[34].Value = model.SalePrice;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERRELPACECAR_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECAR_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERRELPACECAR_DELETE, parameters);
        }
        #endregion

        public bool IsExistsByTaskID(long TaskID)
        {
            string sql = "SELECT TaskID FROM dbo.OrderRelpaceCar WHERE TaskID=" + TaskID;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}

