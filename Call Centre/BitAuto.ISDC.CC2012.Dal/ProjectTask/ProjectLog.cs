using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class ProjectLog : DataBase
    {
        public static ProjectLog Instance = new ProjectLog();

        /// 查询项目日志
        /// <summary>
        /// 查询项目日志
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public DataTable GetProjectLog(long projectid, int pageindex, int pagesize, out int total)
        {
            string sql = @"SELECT *,(SELECT TrueName  FROM SysRightsManager.dbo.UserInfo WHERE UserID=ProjectLog.CreateUserID) AS TrueName 
                                    YanFaFROM dbo.ProjectLog WHERE ProjectID=" + projectid;
            //查询详情
            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = sql;
            parameters[1].Value = "CreateTime";
            parameters[2].Value = pageindex;
            parameters[3].Value = pagesize;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_Page", parameters);
            total = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
    }
}
