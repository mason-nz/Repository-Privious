using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CC_MagazineReturn : DataBase
    {
        #region Instance
        public static readonly CC_MagazineReturn Instance = new CC_MagazineReturn();
        #endregion

        #region const
        private const string P_CC_MAGAZINERETURN_SELECT = "p_CC_MagazineReturn_Select";
        private const string P_CC_MAGAZINERETURN_INSERT = "p_CC_MagazineReturn_Insert";
        private const string P_CC_MAGAZINERETURN_UPDATE = "p_CC_MagazineReturn_Update";
        private const string P_CC_MAGAZINERETURN_DELETE = "p_CC_MagazineReturn_Delete";
        #endregion

        #region Contructor
        protected CC_MagazineReturn()
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
        public DataTable GetCC_MagazineReturn(QueryCC_MagazineReturn query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.DMSMemberID != Constant.STRING_INVALID_VALUE)
            {
                where += "And DMSMemberID='" + StringHelper.SqlFilter(query.DMSMemberID) + "'";
            }
            if (query.Title != Constant.STRING_INVALID_VALUE)
            {
                where += "And Title='" + StringHelper.SqlFilter(query.Title) + "'";
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

            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_MAGAZINERETURN_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        public DataTable GetDistinctTitle()
        {
            string sql = string.Format("SELECT DISTINCT Title FROM CC_MagazineReturn WHERE Status=0 ORDER BY Title");
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CC_MagazineReturn GetCC_MagazineReturn(int RecID)
        {
            QueryCC_MagazineReturn query = new QueryCC_MagazineReturn();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCC_MagazineReturn(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCC_MagazineReturn(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CC_MagazineReturn LoadSingleCC_MagazineReturn(DataRow row)
        {
            Entities.CC_MagazineReturn model = new Entities.CC_MagazineReturn();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            if (row["ContactID"].ToString() != "")
            {
                model.ContactID = int.Parse(row["ContactID"].ToString());
            }
            model.DMSMemberID = row["DMSMemberID"].ToString();
            model.Title = row["Title"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CMDID"].ToString() != "")
            {
                model.CMDID = int.Parse(row["CMDID"].ToString());
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
        public int Insert(Entities.CC_MagazineReturn model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@ContactID", SqlDbType.Int,4),
					new SqlParameter("@DMSMemberID", SqlDbType.VarChar,50),
					new SqlParameter("@Title", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CMDID", SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.ContactID;
            parameters[3].Value = model.DMSMemberID;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CMDID;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_MAGAZINERETURN_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CC_MagazineReturn model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@ContactID", SqlDbType.Int,4),
					new SqlParameter("@DMSMemberID", SqlDbType.VarChar,50),
					new SqlParameter("@Title", SqlDbType.VarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CMDID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CustID;
            parameters[2].Value = model.ContactID;
            parameters[3].Value = model.DMSMemberID;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CMDID;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_MAGAZINERETURN_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(ConnectionStrings_CRM, CommandType.StoredProcedure, P_CC_MAGAZINERETURN_DELETE, parameters);
        }
        #endregion



    }
}
