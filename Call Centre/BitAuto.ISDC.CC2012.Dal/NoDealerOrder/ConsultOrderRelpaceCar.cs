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
    /// 数据访问类ConsultOrderRelpaceCar。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:30 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ConsultOrderRelpaceCar : DataBase
    {
        #region Instance
        public static readonly ConsultOrderRelpaceCar Instance = new ConsultOrderRelpaceCar();
        #endregion

        #region const
        private const string P_CONSULTORDERRELPACECAR_SELECT = "p_ConsultOrderRelpaceCar_Select";
        private const string P_CONSULTORDERRELPACECAR_INSERT = "p_ConsultOrderRelpaceCar_Insert";
        private const string P_CONSULTORDERRELPACECAR_UPDATE = "p_ConsultOrderRelpaceCar_Update";
        private const string P_CONSULTORDERRELPACECAR_DELETE = "p_ConsultOrderRelpaceCar_Delete";
        #endregion

        #region Contructor
        protected ConsultOrderRelpaceCar()
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
        public DataTable GetConsultOrderRelpaceCar(QueryConsultOrderRelpaceCar query, string order, int currentPage, int pageSize, out int totalCount)
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ConsultOrderRelpaceCar GetConsultOrderRelpaceCar(int RecID)
        {
            QueryConsultOrderRelpaceCar query = new QueryConsultOrderRelpaceCar();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetConsultOrderRelpaceCar(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleConsultOrderRelpaceCar(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ConsultOrderRelpaceCar LoadSingleConsultOrderRelpaceCar(DataRow row)
        {
            Entities.ConsultOrderRelpaceCar model = new Entities.ConsultOrderRelpaceCar();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            if (row["WantBrandId"].ToString() != "")
            {
                model.WantBrandId = int.Parse(row["WantBrandId"].ToString());
            }
            if (row["WantSerialId"].ToString() != "")
            {
                model.WantSerialId = int.Parse(row["WantSerialId"].ToString());
            }
            if (row["WantNameID"].ToString() != "")
            {
                model.WantNameID = int.Parse(row["WantNameID"].ToString());
            }
            model.WantCarColor = row["WantCarColor"].ToString();
            model.WantDealerName = row["WantDealerName"].ToString();
            model.WantDealerCode = row["WantDealerCode"].ToString();
            model.CallRecord = row["CallRecord"].ToString();
            if (row["OldBrandId"].ToString() != "")
            {
                model.OldBrandId = int.Parse(row["OldBrandId"].ToString());
            }
            if (row["OldSerialId"].ToString() != "")
            {
                model.OldSerialId = int.Parse(row["OldSerialId"].ToString());
            }
            if (row["OldNameID"].ToString() != "")
            {
                model.OldNameID = int.Parse(row["OldNameID"].ToString());
            }
            model.OldCarColor = row["OldCarColor"].ToString();
            model.RegisterDateYear = row["RegisterDateYear"].ToString();
            model.RegisterDateMonth = row["RegisterDateMonth"].ToString();
            if (row["RegisterProvinceID"].ToString() != "")
            {
                model.RegisterProvinceID = int.Parse(row["RegisterProvinceID"].ToString());
            }
            if (row["RegisterCityID"].ToString() != "")
            {
                model.RegisterCityID = int.Parse(row["RegisterCityID"].ToString());
            }
            if (row["RegisterCountyID"].ToString() != "")
            {
                model.RegisterCountyID = int.Parse(row["RegisterCountyID"].ToString());
            }
            if (row["Mileage"].ToString() != "")
            {
                model.Mileage = decimal.Parse(row["Mileage"].ToString());
            }
            if (row["PresellPrice"].ToString() != "")
            {
                model.PresellPrice = decimal.Parse(row["PresellPrice"].ToString());
            }
            model.OrderRemark = row["OrderRemark"].ToString();
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
        public int Insert(Entities.ConsultOrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@WantBrandId", SqlDbType.Int,4),
					new SqlParameter("@WantSerialId", SqlDbType.Int,4),
					new SqlParameter("@WantNameID", SqlDbType.Int,4),
					new SqlParameter("@WantCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@WantDealerName", SqlDbType.VarChar,200),
					new SqlParameter("@WantDealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@OldBrandId", SqlDbType.Int,4),
					new SqlParameter("@OldSerialId", SqlDbType.Int,4),
					new SqlParameter("@OldNameID", SqlDbType.Int,4),
					new SqlParameter("@OldCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@RegisterDateYear", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterDateMonth", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RegisterCityID", SqlDbType.Int,4),
					new SqlParameter("@Mileage", SqlDbType.Decimal,4),
					new SqlParameter("@PresellPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@RegisterCountyID", SqlDbType.Int,4)                   
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.WantBrandId;
            parameters[3].Value = model.WantSerialId;
            parameters[4].Value = model.WantNameID;
            parameters[5].Value = model.WantCarColor;
            parameters[6].Value = model.WantDealerName;
            parameters[7].Value = model.WantDealerCode;
            parameters[8].Value = model.CallRecord;
            parameters[9].Value = model.OldBrandId;
            parameters[10].Value = model.OldSerialId;
            parameters[11].Value = model.OldNameID;
            parameters[12].Value = model.OldCarColor;
            parameters[13].Value = model.RegisterDateYear;
            parameters[14].Value = model.RegisterDateMonth;
            parameters[15].Value = model.RegisterProvinceID;
            parameters[16].Value = model.RegisterCityID;
            parameters[17].Value = model.Mileage;
            parameters[18].Value = model.PresellPrice;
            parameters[19].Value = model.OrderRemark;
            parameters[20].Value = model.CreateTime;
            parameters[21].Value = model.CreateUserID;
            parameters[22].Value = model.RegisterCountyID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ConsultOrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@WantBrandId", SqlDbType.Int,4),
					new SqlParameter("@WantSerialId", SqlDbType.Int,4),
					new SqlParameter("@WantNameID", SqlDbType.Int,4),
					new SqlParameter("@WantCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@WantDealerName", SqlDbType.VarChar,200),
					new SqlParameter("@WantDealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@OldBrandId", SqlDbType.Int,4),
					new SqlParameter("@OldSerialId", SqlDbType.Int,4),
					new SqlParameter("@OldNameID", SqlDbType.Int,4),
					new SqlParameter("@OldCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@RegisterDateYear", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterDateMonth", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RegisterCityID", SqlDbType.Int,4),
					new SqlParameter("@Mileage", SqlDbType.Decimal,4),
					new SqlParameter("@PresellPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@RegisterCountyID", SqlDbType.Int,4)                  
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.WantBrandId;
            parameters[3].Value = model.WantSerialId;
            parameters[4].Value = model.WantNameID;
            parameters[5].Value = model.WantCarColor;
            parameters[6].Value = model.WantDealerName;
            parameters[7].Value = model.WantDealerCode;
            parameters[8].Value = model.CallRecord;
            parameters[9].Value = model.OldBrandId;
            parameters[10].Value = model.OldSerialId;
            parameters[11].Value = model.OldNameID;
            parameters[12].Value = model.OldCarColor;
            parameters[13].Value = model.RegisterDateYear;
            parameters[14].Value = model.RegisterDateMonth;
            parameters[15].Value = model.RegisterProvinceID;
            parameters[16].Value = model.RegisterCityID;
            parameters[17].Value = model.Mileage;
            parameters[18].Value = model.PresellPrice;
            parameters[19].Value = model.OrderRemark;
            parameters[20].Value = model.CreateTime;
            parameters[21].Value = model.CreateUserID;
            parameters[22].Value = model.RegisterCountyID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ConsultOrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@WantBrandId", SqlDbType.Int,4),
					new SqlParameter("@WantSerialId", SqlDbType.Int,4),
					new SqlParameter("@WantNameID", SqlDbType.Int,4),
					new SqlParameter("@WantCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@WantDealerName", SqlDbType.VarChar,200),
					new SqlParameter("@WantDealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@OldBrandId", SqlDbType.Int,4),
					new SqlParameter("@OldSerialId", SqlDbType.Int,4),
					new SqlParameter("@OldNameID", SqlDbType.Int,4),
					new SqlParameter("@OldCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@RegisterDateYear", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterDateMonth", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RegisterCityID", SqlDbType.Int,4),
					new SqlParameter("@Mileage", SqlDbType.Decimal,4),
					new SqlParameter("@PresellPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@RegisterCountyID", SqlDbType.Int,4)                   
                                        };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.WantBrandId;
            parameters[3].Value = model.WantSerialId;
            parameters[4].Value = model.WantNameID;
            parameters[5].Value = model.WantCarColor;
            parameters[6].Value = model.WantDealerName;
            parameters[7].Value = model.WantDealerCode;
            parameters[8].Value = model.CallRecord;
            parameters[9].Value = model.OldBrandId;
            parameters[10].Value = model.OldSerialId;
            parameters[11].Value = model.OldNameID;
            parameters[12].Value = model.OldCarColor;
            parameters[13].Value = model.RegisterDateYear;
            parameters[14].Value = model.RegisterDateMonth;
            parameters[15].Value = model.RegisterProvinceID;
            parameters[16].Value = model.RegisterCityID;
            parameters[17].Value = model.Mileage;
            parameters[18].Value = model.PresellPrice;
            parameters[19].Value = model.OrderRemark;
            parameters[20].Value = model.CreateTime;
            parameters[21].Value = model.CreateUserID;
            parameters[22].Value = model.RegisterCountyID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ConsultOrderRelpaceCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@WantBrandId", SqlDbType.Int,4),
					new SqlParameter("@WantSerialId", SqlDbType.Int,4),
					new SqlParameter("@WantNameID", SqlDbType.Int,4),
					new SqlParameter("@WantCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@WantDealerName", SqlDbType.VarChar,200),
					new SqlParameter("@WantDealerCode", SqlDbType.VarChar,50),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@OldBrandId", SqlDbType.Int,4),
					new SqlParameter("@OldSerialId", SqlDbType.Int,4),
					new SqlParameter("@OldNameID", SqlDbType.Int,4),
					new SqlParameter("@OldCarColor", SqlDbType.VarChar,16),
					new SqlParameter("@RegisterDateYear", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterDateMonth", SqlDbType.VarChar,10),
					new SqlParameter("@RegisterProvinceID", SqlDbType.Int,4),
					new SqlParameter("@RegisterCityID", SqlDbType.Int,4),
					new SqlParameter("@Mileage", SqlDbType.Decimal,4),
					new SqlParameter("@PresellPrice", SqlDbType.Decimal,9),
					new SqlParameter("@OrderRemark", SqlDbType.NVarChar,1024),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@RegisterCountyID", SqlDbType.Int,4)                   
                                        };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.WantBrandId;
            parameters[3].Value = model.WantSerialId;
            parameters[4].Value = model.WantNameID;
            parameters[5].Value = model.WantCarColor;
            parameters[6].Value = model.WantDealerName;
            parameters[7].Value = model.WantDealerCode;
            parameters[8].Value = model.CallRecord;
            parameters[9].Value = model.OldBrandId;
            parameters[10].Value = model.OldSerialId;
            parameters[11].Value = model.OldNameID;
            parameters[12].Value = model.OldCarColor;
            parameters[13].Value = model.RegisterDateYear;
            parameters[14].Value = model.RegisterDateMonth;
            parameters[15].Value = model.RegisterProvinceID;
            parameters[16].Value = model.RegisterCityID;
            parameters[17].Value = model.Mileage;
            parameters[18].Value = model.PresellPrice;
            parameters[19].Value = model.OrderRemark;
            parameters[20].Value = model.CreateTime;
            parameters[21].Value = model.CreateUserID;
            parameters[17].Value = model.RegisterCountyID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTORDERRELPACECAR_DELETE, parameters);
        }
        #endregion

    }
}

