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
    /// 数据访问类ConsultNewCar。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:09 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ConsultNewCar : DataBase
    {
        #region Instance
        public static readonly ConsultNewCar Instance = new ConsultNewCar();
        #endregion

        #region const
        private const string P_CONSULTNEWCAR_SELECT = "p_ConsultNewCar_Select";
        private const string P_CONSULTNEWCAR_INSERT = "p_ConsultNewCar_Insert";
        private const string P_CONSULTNEWCAR_UPDATE = "p_ConsultNewCar_Update";
        private const string P_CONSULTNEWCAR_DELETE = "p_ConsultNewCar_Delete";
        #endregion

        #region Contructor
        protected ConsultNewCar()
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
        public DataTable GetConsultNewCar(QueryConsultNewCar query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID.ToString();
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTNEWCAR_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ConsultNewCar GetConsultNewCar(int RecID)
        {
            QueryConsultNewCar query = new QueryConsultNewCar();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetConsultNewCar(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleConsultNewCar(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ConsultNewCar LoadSingleConsultNewCar(DataRow row)
        {
            Entities.ConsultNewCar model = new Entities.ConsultNewCar();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            if (row["CarBrandId"].ToString() != "")
            {
                model.CarBrandId = int.Parse(row["CarBrandId"].ToString());
            }
            if (row["CarSerialId"].ToString() != "")
            {
                model.CarSerialId = int.Parse(row["CarSerialId"].ToString());
            }
            model.CarName = row["CarName"].ToString();
            model.DealerName = row["DealerName"].ToString();
            model.BuyCarBudget = row["BuyCarBudget"].ToString();
            model.Activity = row["Activity"].ToString();
            if (row["BuyCarTime"].ToString() != "")
            {
                model.BuyCarTime = int.Parse(row["BuyCarTime"].ToString());
            }
            if (row["BuyOrDisplace"].ToString() != "")
            {
                model.BuyOrDisplace = int.Parse(row["BuyOrDisplace"].ToString());
            }
            model.CallRecord = row["CallRecord"].ToString();
            if (row["AcceptTel"].ToString() != "")
            {
                model.AcceptTel = int.Parse(row["AcceptTel"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }

            model.DealerCode = row["DealerCode"].ToString();
            model.DealerName = row["DealerName"].ToString();

            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ConsultNewCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCarBudget", SqlDbType.NVarChar,50),
					new SqlParameter("@Activity", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCarTime", SqlDbType.Int,4),
					new SqlParameter("@BuyOrDisplace", SqlDbType.Int,4),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@AcceptTel", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@DealerCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CarID", SqlDbType.Int,4)
                                        };

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.CarBrandId;
            parameters[3].Value = model.CarSerialId;
            parameters[4].Value = model.CarName;
            parameters[5].Value = model.DealerName;
            parameters[6].Value = model.BuyCarBudget;
            parameters[7].Value = model.Activity;
            parameters[8].Value = model.BuyCarTime;
            parameters[9].Value = model.BuyOrDisplace;
            parameters[10].Value = model.CallRecord;
            parameters[11].Value = model.AcceptTel;
            parameters[12].Value = model.CreateTime;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.DealerCode;
            parameters[15].Value = model.CarID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTNEWCAR_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ConsultNewCar model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CarBrandId", SqlDbType.Int,4),
					new SqlParameter("@CarSerialId", SqlDbType.Int,4),
					new SqlParameter("@CarName", SqlDbType.NVarChar,200),
					new SqlParameter("@DealerName", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCarBudget", SqlDbType.NVarChar,50),
					new SqlParameter("@Activity", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCarTime", SqlDbType.Int,4),
					new SqlParameter("@BuyOrDisplace", SqlDbType.Int,4),
					new SqlParameter("@CallRecord", SqlDbType.NVarChar,2000),
					new SqlParameter("@AcceptTel", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.CarBrandId;
            parameters[3].Value = model.CarSerialId;
            parameters[4].Value = model.CarName;
            parameters[5].Value = model.DealerName;
            parameters[6].Value = model.BuyCarBudget;
            parameters[7].Value = model.Activity;
            parameters[8].Value = model.BuyCarTime;
            parameters[9].Value = model.BuyOrDisplace;
            parameters[10].Value = model.CallRecord;
            parameters[11].Value = model.AcceptTel;
            parameters[12].Value = model.CreateTime;
            parameters[13].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTNEWCAR_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONSULTNEWCAR_DELETE, parameters);
        }
        #endregion

        #region GetMaxID

        // <summary>
        /// 取当前最大值
        /// </summary>
        /// <returns></returns>
        public int GetCurrMaxID()
        {
            int maxid = 0;

            string sqlStr = "select max([RecID]) FROM [CustHistoryInfo]";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                int intVal = 0;
                if (int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out intVal))
                {
                    maxid = intVal;
                }
                else
                {
                    maxid = 0;
                }
            }
            else
            {
                maxid = 0;
            }

            return maxid;
        }

        #endregion

    }
}

