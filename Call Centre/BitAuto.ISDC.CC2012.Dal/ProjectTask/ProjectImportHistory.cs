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
    public class ProjectImportHistory : DataBase
    {
        #region Instance
        public static readonly ProjectImportHistory Instance = new ProjectImportHistory();
        #endregion

        #region const
        private const string P_PROJECTIMPORTHISTORY_SELECT = "p_ProjectImportHistory_Select";
        private const string P_PROJECTIMPORTHISTORY_INSERT = "p_ProjectImportHistory_Insert";
        #endregion


        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectImportHistory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@ImportNumber", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.ImportNumber;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTIMPORTHISTORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectImportHistory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@ImportNumber", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.ImportNumber;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTIMPORTHISTORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        public int Delete(SqlTransaction sqltran,int ProjectID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ProjectID", SqlDbType.Int)};
            parameters[0].Value = ProjectID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "p_ProjectImportHistory_DeleteByProjectID", parameters);
        }

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
        public DataTable GetProjectImportHistory(QueryProjectImportHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectID=" + query.ProjectID + "";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTIMPORTHISTORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion
    }
}
