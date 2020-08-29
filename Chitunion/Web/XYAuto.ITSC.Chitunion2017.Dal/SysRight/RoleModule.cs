using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using System.Data.SqlClient;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.SysRight
{
    public class RoleModule:DataBase
    {
        #region Instance
        public static readonly RoleModule Instance = new RoleModule();
        #endregion

        #region Contructor
        protected RoleModule()
        {
        }
        #endregion

        #region const
        public const string P_ROLEMODULE_SELECT = "p_RoleModule_select";
        public const string P_ROLEMODULE_UPDATE = "p_RoleModule_update";
        public const string P_ROLEMODULE_INSERT = "p_RoleModule_insert";
        public const string P_ROLEMODULE_INSERT_ALL = "P_ROLEMODULE_INSERT_ALL";
        public const string P_ROLEMODULE_EXPORT = "p_RoleModule_Export";
        #endregion

        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryRoleModule">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>用户表集合</returns>
        public DataTable GetRoleModule(QueryRoleModule queryRoleModule, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            if (queryRoleModule.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + queryRoleModule.Status.ToString();
            }
            if (queryRoleModule.ModuleID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ModuleID='" + SqlFilter(queryRoleModule.ModuleID) + "'";
            }
            if (queryRoleModule.RoleID != Constant.STRING_INVALID_VALUE)
            {
                where += " and RoleID='" + SqlFilter(queryRoleModule.RoleID) + "'";
            }
            if (queryRoleModule.SysID != Constant.STRING_INVALID_VALUE)
            {
                where += " and SysID='" + SqlFilter(queryRoleModule.SysID) + "'";
            }

            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.NVarChar,1000),
                    new SqlParameter("@order", SqlDbType.NVarChar,1000),
                    new SqlParameter("@page", SqlDbType.Int,4),
                    new SqlParameter("@pagesize", SqlDbType.Int,4),
                    new SqlParameter("@TotalRecorder", SqlDbType.Int,4)
             };
            parameters[0].Value = where;
            parameters[1].Value = string.Empty;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEMODULE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            //totalCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }

        public int InsertRoleModuleAll(string roleId, string moduleIds, string sysID)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@roleId", SqlDbType.VarChar,50),
                    new SqlParameter("@moduleIDs", SqlDbType.VarChar,8000),
                    new SqlParameter("@SysID", SqlDbType.VarChar,50)
                    };

            //绑定存储过程参数
            parameters[0].Value = roleId;
            parameters[1].Value = SqlFilter(moduleIds);
            parameters[2].Value = SqlFilter(sysID);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ROLEMODULE_INSERT_ALL, parameters);
        }
    }
}
