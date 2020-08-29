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
    public class CartScheduleInfo : DataBase
    {
        public const string P_CartScheduleInfo_SELECT = "p_CartScheduleInfo_Select";

        #region Instance
        public static readonly CartScheduleInfo Instance = new CartScheduleInfo();
        #endregion

        #region Select
        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.CartScheduleInfo GetModel(int RecID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,CartID,BeginTime,CreateTime,CreateUserID ");
            strSql.Append(" FROM dbo.CartScheduleInfo ");
            strSql.Append(" WHERE RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)
			};
            parameters[0].Value = RecID;

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
        public Entities.CartScheduleInfo DataRowToModel(DataRow row)
        {
            Entities.CartScheduleInfo model = new Entities.CartScheduleInfo();
            if (row != null)
            {
                if (row["RecID"] != null && row["RecID"].ToString() != "")
                {
                    model.RecID = int.Parse(row["RecID"].ToString());
                }               
                if (row["CartID"] != null && row["CartID"].ToString() != "")
                {
                    model.CartID = int.Parse(row["CartID"].ToString());
                }
                if (row["BeginTime"] != null && row["BeginTime"].ToString() != "")
                {
                    model.BeginTime = DateTime.Parse(row["BeginTime"].ToString());
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

        #region 得到一个对象实体
        public Entities.CartScheduleInfo GetCartScheduleInfo(int cartid)
        {
            QueryCartScheduleInfo query = new QueryCartScheduleInfo();
            query.CartID = cartid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCartScheduleInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCartScheduleInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CartScheduleInfo LoadSingleCartScheduleInfo(DataRow row)
        {
            return DataRowToModel(row);
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
        public DataTable GetCartScheduleInfo(QueryCartScheduleInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
          
            if (query.CartID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CartID = " + query.CartID;
            }
            
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CreateUserID = " + query.CreateUserID;
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartScheduleInfo_Select", parameters);
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
        public int Insert(Entities.CartScheduleInfo model)
        {
            SqlParameter[] parameters = { 
                    new SqlParameter("@RecID", SqlDbType.Int,4),                                                             
                    new SqlParameter("@CartID",SqlDbType.Int,4), 
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)                 
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CartID;
            parameters[2].Value = model.BeginTime;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartScheduleInfo_Insert", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Update(Entities.CartScheduleInfo model)
        {
            string sqlstr = string.Format("UPDATE dbo.CartScheduleInfo SET BeginTime={0} WHERE RecID={1}",model.BeginTime,model.RecID);
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
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
            string sqlstr = "DELETE FROM dbo.CartScheduleInfo WHERE RecID=RecID";

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }
        #endregion        
        #region 根据CartID查询
        public DataTable QueryByCartID(int cartID)
        {
            string sqlstr = @"SELECT BeginTime BeginData,EndTime EndData FROM dbo.CartScheduleInfo WHERE CartID=@CartID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@CartID",cartID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);

            return ds.Tables[0];
        }
        #endregion
    }
}
