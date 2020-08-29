using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using XYAuto.Utils.Config;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Common.Dal
{
    public class LogInfo : DataBase
    {
        public static readonly LogInfo Instance = new LogInfo();

        /// <summary>
        /// 增加一条日志数据
        /// </summary>
        public int InsertLog(int LogModuleID, int ActionType, string Content, int UserID, string IP, DateTime CreateTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LogInfo(");
            strSql.Append("Module,ActionType,Content,UserID,IP,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@Module,@ActionType,@Content,@UserID,@IP,@CreateTime)");
            SqlParameter[] parameters = {
					new SqlParameter("@Module", SqlDbType.Int,4),
					new SqlParameter("@ActionType", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.VarChar,2000),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = LogModuleID;
            parameters[1].Value = ActionType;
            parameters[2].Value = Content;
            parameters[3].Value = UserID;
            parameters[4].Value = IP;
            parameters[5].Value = CreateTime;

            return SqlHelper.ExecuteNonQuery(SYSCONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);

        }
    }
}
