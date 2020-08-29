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
    /// 数据访问类OrderNewCar。
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
    public class OrderNewCar : DataBase
    {
        #region Instance
        public static readonly OrderNewCar Instance = new OrderNewCar();
        #endregion

        #region const
        private const string P_ORDERNEWCAR_SELECT = "p_OrderNewCar_Select";
        private const string P_ORDERNEWCAR_INSERT = "p_OrderNewCar_Insert";
        private const string P_ORDERNEWCAR_UPDATE = "p_OrderNewCar_Update";
        private const string P_ORDERNEWCAR_DELETE = "p_OrderNewCar_Delete";
        #endregion

        #region Contructor
        protected OrderNewCar()
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
        /// <param name="totalCount">OrderType 1新车；3试驾  可以为空，默认是1</param>
        /// <returns>集合</returns>
        public DataTable GetOrderNewCar(QueryOrderNewCar query, string order, int currentPage, int pageSize, out int totalCount, int OrderType = 0)
        {
            string where = string.Empty;

            #region 条件

            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID=" + query.TaskID.ToString();
            }
            if (query.IsUpdate != Constant.INT_INVALID_VALUE)
            {
                where += " AND TaskID IN "
                        + "("
                        + "SELECT TaskID FROM dbo.UpdateOrderData u "
                        + "WHERE u.TaskID=dbo.OrderNewCar.TaskID AND u.IsUpdate=-1 AND u.UpdateType=-1 AND u.APIType=1 "
                        + ")  And OrderType=" + OrderType.ToString();
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCAR_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        public bool IsExistsByTaskID(long TaskID)
        {
            string sql = "SELECT TaskID FROM dbo.OrderNewCar WHERE TaskID=" + TaskID;
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
        public Entities.OrderNewCar GetOrderNewCar(long TaskID)
        {
            QueryOrderNewCar query = new QueryOrderNewCar();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderNewCar(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleOrderNewCar(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public List<Entities.OrderNewCar> GetOrderNewCarList(Entities.QueryOrderNewCar query, int OrderType = 0)
        {
            List<Entities.OrderNewCar> list = new List<Entities.OrderNewCar>();

            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderNewCar(query, string.Empty, 1, 99999999, out count, OrderType);
            if (count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleOrderNewCar(dr));
                }
            }
            else
            {
                return null;
            }
            return list;
        }

        private Entities.OrderNewCar LoadSingleOrderNewCar(DataRow row)
        {
            Entities.OrderNewCar model = new Entities.OrderNewCar();

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
            model.CarColor = row["CarColor"].ToString();
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
        public void Insert(Entities.OrderNewCar model)
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
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@DealerID", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4)};
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
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.DMSMemberCode;
            parameters[19].Value = model.DMSMemberName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.Status;
            parameters[22].Value = model.CreateTime;
            parameters[23].Value = model.DealerID;
            parameters[24].Value = model.OrderType;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCAR_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.OrderNewCar model)
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
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@DealerID", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4)};
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
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.DMSMemberCode;
            parameters[19].Value = model.DMSMemberName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.Status;
            parameters[22].Value = model.CreateTime;
            parameters[23].Value = model.DealerID;
            parameters[24].Value = model.OrderType;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERNEWCAR_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.OrderNewCar model)
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
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
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
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.DMSMemberCode;
            parameters[19].Value = model.DMSMemberName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.Status;
            parameters[22].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCAR_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderNewCar model)
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
					new SqlParameter("@CarColor", SqlDbType.NVarChar,16),
					new SqlParameter("@DMSMemberCode", SqlDbType.VarChar,50),
					new SqlParameter("@DMSMemberName", SqlDbType.NVarChar,256),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
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
            parameters[17].Value = model.CarColor;
            parameters[18].Value = model.DMSMemberCode;
            parameters[19].Value = model.DMSMemberName;
            parameters[20].Value = model.CallRecord;
            parameters[21].Value = model.Status;
            parameters[22].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ORDERNEWCAR_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCAR_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ORDERNEWCAR_DELETE, parameters);
        }
        #endregion




        public static DataTable GetModifyCar()
        {
            DataTable retDt = null;

            string sqlStr1 = "SELECT [类型]='新车', [易湃订单ID]=l.YPOrderID ,"
                               + "[订购者姓名]= l.UserName ,"
                               + "[订购者手机]=l.UserMobile ,"
                               + "[订购者电话]= l.UserPhone ,"
                               + "[原车款ID]=l.CarID ,"
                               + "[修改后的车型ID]=n.CarTypeID"
                               + " FROM    OrderNewCarLog l"
                               + " LEFT JOIN OrderNewCar n ON l.YPOrderID = n.YPOrderID"
                               + " WHERE   l.CarID <> n.CarTypeID";

            string sqlStr2 = "SELECT [类型]='置换', [易湃订单ID]=l.YPOrderID ,"
                                + "[订购者姓名]= l.UserName ,"
                                + "[订购者手机]=l.UserMobile ,"
                                + "[订购者电话]= l.UserPhone ,"
                                + "[原车款ID]=l.CarID ,"
                                + "[修改后的车型ID]=n.CarTypeID"
                                + " FROM    OrderRelpaceCarLog l"
                                + " LEFT JOIN OrderRelpaceCar n ON l.YPOrderID = n.YPOrderID"
                                + " WHERE   l.CarID <> n.CarTypeID";

            DataSet dtnewCar = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr1);
            DataSet dtnewReplace = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr2);

            if (dtnewCar != null && dtnewCar.Tables.Count > 0)
            {
                retDt = dtnewCar.Tables[0].Copy();
            }


            if (dtnewReplace != null && dtnewReplace.Tables.Count > 0)
            {
                if (retDt != null)
                {
                    retDt.Merge(dtnewReplace.Tables[0]);
                }
                else
                {
                    retDt = dtnewReplace.Tables[0].Copy();
                }
            }


            return retDt;

        }
    }
}

