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
    /// 数据访问类CRMCustForNextVisit。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-04-17 10:45:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CRMCustForNextVisit : DataBase
    {
        #region Instance
        public static readonly CRMCustForNextVisit Instance = new CRMCustForNextVisit();
        #endregion

        #region const
        private const string P_CRMCUSTFORNEXTVISIT_SELECT = "p_CRMCustForNextVisit_Select";
        private const string P_CRMCUSTFORNEXTVISIT_INSERT = "p_CRMCustForNextVisit_Insert";
        private const string P_CRMCUSTFORNEXTVISIT_UPDATE = "p_CRMCustForNextVisit_Update";
        private const string P_CRMCUSTFORNEXTVISIT_DELETE = "p_CRMCustForNextVisit_Delete";
        #endregion

        #region Contructor
        protected CRMCustForNextVisit()
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
        public DataTable GetCRMCustForNextVisit(QueryCRMCustForNextVisit query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.CrmCustID != Constant.STRING_INVALID_VALUE)
            {
                where += " and CrmCustID='" + StringHelper.SqlFilter(query.CrmCustID) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and UserID='" + query.UserID + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CRMCustForNextVisit GetCRMCustForNextVisit(string CrmCustID, int userid)
        {
            QueryCRMCustForNextVisit query = new QueryCRMCustForNextVisit();
            query.CrmCustID = CrmCustID;
            query.UserID = userid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCRMCustForNextVisit(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCRMCustForNextVisit(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CRMCustForNextVisit LoadSingleCRMCustForNextVisit(DataRow row)
        {
            Entities.CRMCustForNextVisit model = new Entities.CRMCustForNextVisit();

            model.CrmCustID = row["CrmCustID"].ToString();
            model.UserID = int.Parse(row["UserID"].ToString());
            model.NextVisitDate = row["NextVisitDate"].ToString();
            if (row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            if (row["LastUpdateUserID"].ToString() != "")
            {
                model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
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
        public void Insert(Entities.CRMCustForNextVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@NextVisitDate", SqlDbType.VarChar,10),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CrmCustID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.NextVisitDate;
            parameters[3].Value = model.LastUpdateTime;
            parameters[4].Value = model.LastUpdateUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_INSERT, parameters);
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CRMCustForNextVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@NextVisitDate", SqlDbType.VarChar,10),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CrmCustID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.NextVisitDate;
            parameters[3].Value = model.LastUpdateTime;
            parameters[4].Value = model.LastUpdateUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_INSERT, parameters);
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CRMCustForNextVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@NextVisitDate", SqlDbType.VarChar,10),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CrmCustID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.NextVisitDate;
            parameters[3].Value = model.LastUpdateTime;
            parameters[4].Value = model.LastUpdateUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CRMCustForNextVisit model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
					new SqlParameter("@NextVisitDate", SqlDbType.VarChar,10),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.CrmCustID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.NextVisitDate;
            parameters[3].Value = model.LastUpdateTime;
            parameters[4].Value = model.LastUpdateUserID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string CrmCustID, int userid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50)};
            parameters[0].Value = CrmCustID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string CrmCustID, int userid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50)};
            parameters[0].Value = CrmCustID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CRMCUSTFORNEXTVISIT_DELETE, parameters);
        }
        #endregion

        /// 清空错误数据
        /// <summary>
        /// 清空错误数据
        /// </summary>
        public void ClearErrorDataByCust()
        {
            string sql = @"delete  from crmcustfornextvisit
                                    where   not exists ( select map.custid ,
                                                                map.userid
                                                            from   crm2009.dbo.custusermapping as map
                                                                where 1=1
                                                                and map.custid = crmcustfornextvisit.crmcustid
                                                                and map.userid = crmcustfornextvisit.userid )";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}

