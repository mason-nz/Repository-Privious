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
    /// 数据访问类BuyCarInfo。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class BuyCarInfo : DataBase
    {
        #region Instance
        public static readonly BuyCarInfo Instance = new BuyCarInfo();
        #endregion

        #region const
        private const string P_BUYCARINFO_SELECT = "p_BuyCarInfo_Select";
        private const string P_BUYCARINFO_INSERT = "p_BuyCarInfo_Insert";
        private const string P_BUYCARINFO_UPDATE = "p_BuyCarInfo_Update";
        private const string P_BUYCARINFO_DELETE = "p_BuyCarInfo_Delete";
        private const string P_Page = "P_Page";
        #endregion

        #region Contructor
        protected BuyCarInfo()
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
        public DataTable GetBuyCarInfo(QueryBuyCarInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query != null && query.RecID != 0 && query.RecID != -2)
            {
                where += " and RecID=" + query.RecID;
            }
            if (query != null && !string.IsNullOrEmpty(query.CustID))
            {
                where += " and CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query != null && Convert.ToInt32(query.Type) != -2)
            {
                where += " and [Type]=" + query.Type;
            }
            where += " and status=0";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_BUYCARINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.BuyCarInfo GetBuyCarInfo(string CustID)
        {
            QueryBuyCarInfo query = new QueryBuyCarInfo();
            query.CustID = CustID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetBuyCarInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleBuyCarInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.BuyCarInfo LoadSingleBuyCarInfo(DataRow row)
        {
            Entities.BuyCarInfo model = new Entities.BuyCarInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
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
            model.Marriage = int.Parse(row["Marriage"].ToString());
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
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.BuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.NVarChar,200),
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
                    new SqlParameter("@CarTypeID", SqlDbType.Int,4)           
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Age;
            parameters[4].Value = model.IDCard;
            parameters[5].Value = model.Vocation;
            parameters[6].Value = model.Marriage;
            parameters[7].Value = model.Income;
            parameters[8].Value = model.CarBrandId;
            parameters[9].Value = model.CarSerialId;
            parameters[10].Value = model.CarName;
            parameters[11].Value = model.IsAttestation;
            parameters[12].Value = model.DriveAge;
            parameters[13].Value = model.UserName;
            parameters[14].Value = model.CarNo;
            parameters[15].Value = model.Remark;
            parameters[16].Value = model.Status;
            parameters[17].Value = model.CreateTime;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CarTypeID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_BUYCARINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }


        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction tran, Entities.BuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.NVarChar,200),
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
                    new SqlParameter("@CarTypeID", SqlDbType.Int,4)   };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Age;
            parameters[4].Value = model.IDCard;
            parameters[5].Value = model.Vocation;
            parameters[6].Value = model.Marriage;
            parameters[7].Value = model.Income;
            parameters[8].Value = model.CarBrandId;
            parameters[9].Value = model.CarSerialId;
            parameters[10].Value = model.CarName;
            parameters[11].Value = model.IsAttestation;
            parameters[12].Value = model.DriveAge;
            parameters[13].Value = model.UserName;
            parameters[14].Value = model.CarNo;
            parameters[15].Value = model.Remark;
            parameters[16].Value = model.Status;
            parameters[17].Value = model.CreateTime;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CarTypeID;

            SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, P_BUYCARINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.BuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.Int),
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
                    new SqlParameter("@CarTypeID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Age;
            parameters[4].Value = model.IDCard;
            parameters[5].Value = model.Vocation;
            parameters[6].Value = model.Marriage;
            parameters[7].Value = model.Income;
            parameters[8].Value = model.CarBrandId;
            parameters[9].Value = model.CarSerialId;
            parameters[10].Value = model.CarName;
            parameters[11].Value = model.IsAttestation;
            parameters[12].Value = model.DriveAge;
            parameters[13].Value = model.UserName;
            parameters[14].Value = model.CarNo;
            parameters[15].Value = model.Remark;
            parameters[16].Value = model.Status;
            parameters[17].Value = model.CreateTime;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CarTypeID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_BUYCARINFO_UPDATE, parameters);
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqlTran, Entities.BuyCarInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@IDCard", SqlDbType.VarChar,50),
					new SqlParameter("@Vocation", SqlDbType.Int,4),
					new SqlParameter("@Marriage", SqlDbType.Int),
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
                    new SqlParameter("@CarTypeID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.Age;
            parameters[4].Value = model.IDCard;
            parameters[5].Value = model.Vocation;
            parameters[6].Value = model.Marriage;
            parameters[7].Value = model.Income;
            parameters[8].Value = model.CarBrandId;
            parameters[9].Value = model.CarSerialId;
            parameters[10].Value = model.CarName;
            parameters[11].Value = model.IsAttestation;
            parameters[12].Value = model.DriveAge;
            parameters[13].Value = model.UserName;
            parameters[14].Value = model.CarNo;
            parameters[15].Value = model.Remark;
            parameters[16].Value = model.Status;
            parameters[17].Value = model.CreateTime;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CarTypeID;

            return SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, P_BUYCARINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string CustID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,20)};
            parameters[0].Value = CustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_BUYCARINFO_DELETE, parameters);
        }
        #endregion
        /// <summary>
        /// 取所有汽车品牌
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarBrand()
        {
            string sqlstr = "select distinct Brandid,name from car_brand order by name";
            DataTable dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null).Tables[0];
            return dt;
        }
        /// <summary>
        /// 取所有汽车车型
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarSerial()
        {
            string sqlstr = "select distinct serialid,name from Car_Serial order by name";
            DataTable dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null).Tables[0];
            return dt;
        }

        /// <summary>
        /// 取所有品牌和车型信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetALLCarBrandAndSerial(string name, string order, int currentPage, int pageSize, out int totalCount)
        {
            string sqlstr = " select distinct serialid,car_brand.name bname,dbo.Car_Serial.Name sname YanFaFROM car_brand,Car_Serial "
                     + "WHERE dbo.Car_Brand.BrandID=dbo.Car_Serial.BrandID and (car_brand.name like '%" + StringHelper.SqlFilter(name) + "%' or Car_Serial.Name like '%" + StringHelper.SqlFilter(name) + "%') ";

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sqlstr;
            parameters[1].Value = order;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_Page, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据品牌id，取车系
        /// </summary>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public DataTable GetCarSerial(int brandid)
        {
            DataTable dt = null;
            string sqlstr = "select distinct serialid,name from Car_Serial where brandid=@brandid";
            SqlParameter[] parameters = {
					new SqlParameter("@brandid", SqlDbType.Int)};
            parameters[0].Value = brandid;
            dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, parameters).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据车系，取车系ID
        /// </summary>
        /// <param name="brandid"></param>
        /// <returns></returns>
        public DataTable GetCarSerialByName(string CarSerialName)
        {
            DataTable dt = null;
            string sqlstr = "select distinct serialid,name from Car_Serial where seoname like '%" + StringHelper.SqlFilter(CarSerialName) + "%'";
            dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null).Tables[0];
            return dt;
        }


        /// <summary>
        /// 根据品牌名称取汽车品牌
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarBrandByName(string name)
        {
            string sqlstr = "select distinct Brandid,name from car_brand where name like '%" + StringHelper.SqlFilter(name) + "%'";
            DataTable dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, null).Tables[0];
            return dt;
        }

        #region 刘学文 2012-8-6 获取品牌名称、车系名称

        // 根据品牌id，取品牌名称 
        public DataTable GetCarBrandName(int brandid)
        {
            DataTable dt = null;
            string sqlstr = "SELECT  top 1 Name FROM car_brand WHERE BrandID=@brandid";
            SqlParameter[] parameters = {
					new SqlParameter("@brandid", SqlDbType.Int)};
            parameters[0].Value = brandid;
            dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, parameters).Tables[0];
            return dt;
        }

        // 根据车系id，取车系名称 
        public DataTable GetCarSerialName(int serialid)
        {
            DataTable dt = null;
            string sqlstr = "select top 1  name from Car_Serial where serialid=@serialid";
            SqlParameter[] parameters = {
					new SqlParameter("@serialid", SqlDbType.Int)};
            parameters[0].Value = serialid;
            dt = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sqlstr, parameters).Tables[0];
            return dt;
        }

        #endregion


    }
}

