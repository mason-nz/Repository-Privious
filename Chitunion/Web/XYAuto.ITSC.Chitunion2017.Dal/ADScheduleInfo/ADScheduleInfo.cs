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
    public class ADScheduleInfo : DataBase
    {
        public const string P_ADScheduleInfo_SELECT = "p_ADScheduleInfo_Select";

        #region Instance
        public static readonly ADScheduleInfo Instance = new ADScheduleInfo();
        #endregion

        #region Select
        /// <summary>
        /// 根据广告位detailid获取CPD排期信息
        /// </summary>
        /// <param name="addetailid"></param>
        /// <returns></returns>
        public DataTable GetADScheduleInfo_ByADDetailID(int addetailid)
        {
            string sqlstr = "SELECT MediaID,PubID,BeginData,EndData,CreateTime FROM dbo.ADScheduleInfo WHERE ADDetailID=" + addetailid;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text,sqlstr);
            
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.ADScheduleInfo GetModel(int ADSID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 ADSID,ADDetailID,OrderID,SubOrderID,MediaID,PubID,BeginData,EndData,CreateTime,CreateUserID");
            strSql.Append(" FROM ADScheduleInfo ");
            strSql.Append(" WHERE ADSID=@ADSID");
            SqlParameter[] parameters = {
					new SqlParameter("@ADSID", SqlDbType.Int,4)
			};
            parameters[0].Value = ADSID;

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
        public Entities.ADScheduleInfo DataRowToModel(DataRow row)
        {
            Entities.ADScheduleInfo model = new Entities.ADScheduleInfo();
            if (row != null)
            {
                if (row["ADSID"] != null && row["ADSID"].ToString() != "")
                {
                    model.ADSID = int.Parse(row["ADSID"].ToString());
                }
                if (row["ADDetailID"] != null && row["ADDetailID"].ToString() != "")
                {
                    model.ADDetailID = int.Parse(row["ADDetailID"].ToString());
                }
                if (row["OrderID"] != null && row["OrderID"].ToString() != "")
                {
                    model.OrderID = row["OrderID"].ToString();
                }
                if (row["SubOrderID"] != null && row["SubOrderID"].ToString() != "")
                {
                    model.SubOrderID = row["SubOrderID"].ToString();
                }
                if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                {
                    model.MediaID = int.Parse(row["MediaID"].ToString());
                }
                if (row["PubID"] != null && row["PubID"].ToString() != "")
                {
                    model.PubID = int.Parse(row["PubID"].ToString());
                }
                if (row["BeginData"] != null && row["BeginData"].ToString() != "")
                {
                    model.BeginData = DateTime.Parse(row["BeginData"].ToString());
                }
                if (row["EndData"] != null && row["EndData"].ToString() != "")
                {
                    model.EndData = DateTime.Parse(row["EndData"].ToString());
                }                                
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
                }
                
            }
            return model;
        }

        #region 根据订单号查询得到一个对象实体
        public Entities.ADScheduleInfo GetADScheduleInfo(int ADSID)
        {
            QueryADScheduleInfo query = new QueryADScheduleInfo();
            query.ADSID = ADSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetADScheduleInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleADScheduleInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ADScheduleInfo LoadSingleADScheduleInfo(DataRow row)
        {
            Entities.ADScheduleInfo model = new Entities.ADScheduleInfo();
            model.ADSID = int.Parse(row["ADSID"].ToString());

            if (row["ADSID"] != null && row["ADSID"].ToString() != "")
            {
                model.ADSID = int.Parse(row["ADSID"].ToString());
            }
            if (row["ADDetailID"] != null && row["ADDetailID"].ToString() != "")
            {
                model.ADDetailID = int.Parse(row["ADDetailID"].ToString());
            }
            if (row["OrderID"] != null && row["OrderID"].ToString() != "")
            {
                model.OrderID = row["OrderID"].ToString();
            }
            if (row["SubOrderID"] != null && row["SubOrderID"].ToString() != "")
            {
                model.SubOrderID = row["SubOrderID"].ToString();
            }
            if (row["MediaID"] != null && row["MediaID"].ToString() != "")
            {
                model.MediaID = int.Parse(row["MediaID"].ToString());
            }
            if (row["PubID"] != null && row["PubID"].ToString() != "")
            {
                model.PubID = int.Parse(row["PubID"].ToString());
            }
            if (row["BeginData"] != null && row["BeginData"].ToString() != "")
            {
                model.BeginData = DateTime.Parse(row["BeginData"].ToString());
            }
            if (row["EndData"] != null && row["EndData"].ToString() != "")
            {
                model.EndData = DateTime.Parse(row["EndData"].ToString());
            }
            if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
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
        public DataTable GetADScheduleInfo(QueryADScheduleInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.ADDetailID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.ADDetailID = " + query.ADDetailID;
            }

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID = '" + query.OrderID + "'";
            }
            if (query.SubOrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.SubOrderID = '" + query.SubOrderID + "'";
            }
            if (query.MediaID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.MediaID = " + query.MediaID;
            }
            if (query.PubID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.PubID = " + query.PubID;
            }
           

            //DateTime beginTime;
            //if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            //{
            //    where += " and a.CreateTime>='" + beginTime + "'";
            //}
            //DateTime endTime;
            //if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            //{
            //    where += " and a.CreateTime<'" + endTime.AddDays(1) + "'";
            //}

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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADScheduleInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Entities.ADScheduleInfo model)
        {
            SqlParameter[] parameters = {                                                       
                    new SqlParameter("@ADDetailID",SqlDbType.Int,4),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@MediaID",SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@BeginData",SqlDbType.Date),
                    new SqlParameter("@EndData",SqlDbType.Date),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)                 
                                        };
            parameters[0].Value = model.ADDetailID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.SubOrderID;
            parameters[3].Value = model.MediaID;
            parameters[4].Value = model.PubID;
            parameters[5].Value = model.BeginData;
            parameters[6].Value = model.EndData;
            parameters[7].Value = model.CreateUserID;

            int retval = 0;
            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADScheduleInfo_Insert", parameters);
            return retval;
        }
        #endregion
        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.ADScheduleInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ADSID",SqlDbType.Int,4),
                    new SqlParameter("@ADDetailID",SqlDbType.Int,4),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@MediaID",SqlDbType.Int,4), 
					new SqlParameter("@PubID", SqlDbType.Int,4),
                    new SqlParameter("@BeginData",SqlDbType.Date),
                    new SqlParameter("@EndData",SqlDbType.Date)
                                        };
            parameters[0].Value = model.ADSID;
            parameters[1].Value = model.ADDetailID;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.SubOrderID;
            parameters[4].Value = model.MediaID;
            parameters[5].Value = model.PubID;
            parameters[6].Value = model.BeginData;
            parameters[7].Value = model.EndData;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADScheduleInfo_Update", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 根据广告位ID删除所属的排期信息
        /// </summary>
        public int Delete(int ADDetailID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ADDetailID", SqlDbType.Int,4)};
            parameters[0].Value = ADDetailID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADScheduleInfo_Delete", parameters);
        }
        /// <summary>
        /// 根据广告位ID删除所属的排期信息
        /// </summary>
        public int Delete(SqlTransaction sqltran, int ADDetailID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ADDetailID", SqlDbType.VarChar,50)};
            parameters[0].Value = ADDetailID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "p_ADScheduleInfo_Delete", parameters);
        }
        #endregion

        #region 根据主订单号删除排期信息
        /// <summary>
        /// 根据主订单号删除排期信息
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        public int DeleteByOrderID(string orderid)
        {
            string sqlstr = string.Format("DELETE FROM dbo.ADScheduleInfo WHERE OrderID='{0}'",orderid);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 根据子订单明细ID查询所属排期信息
        /// <summary>
        /// 根据子订单明细ID查询所属排期信息
        /// </summary>
        /// <param name="Detailid">子订单明细ID</param>
        /// <returns></returns>
        public DataTable GetADScheduleInfoByDetailID(int Detailid)
        {
            string sqlstr = string.Format("SELECT ADSID,ADDetailID,OrderID,SubOrderID,MediaID,PubID,BeginData,EndData,CreateTime,CreateUserID FROM dbo.ADScheduleInfo WHERE ADDetailID={0}", Detailid);

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return ds.Tables[0];
        }
        #endregion

        #region 批量插入
        public void Insert_BulkCopyToDB(DataTable dt)
        {            
            SqlBulkCopyByDataTable(CONNECTIONSTRINGS, "ADScheduleInfo", dt, 365);
        }
        #endregion
        #region 根据广告位ID查询
        public DataTable QueryByADDetailID(int detailID)
        {
            string sqlstr = @"SELECT BeginData,EndData FROM dbo.ADScheduleInfo WHERE ADDetailID=@ADDetailID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ADDetailID",detailID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);

            return ds.Tables[0];
        }
        #endregion
    }
}
