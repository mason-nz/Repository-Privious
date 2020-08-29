using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    public class AuditInfo : DataBase
    {


        public static readonly AuditInfo Instance = new AuditInfo();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.AuditInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [dbo].[AuditInfo]
                                   ([RelationType]
                                   ,[RelationId]
                                   ,[AuditStatus]
                                   ,[RejectMsg]
                                   ,[CreateTime]
                                   ,[CreateUserId])
                             VALUES
                                   (@RelationType
                                   ,@RelationId
                                   ,@AuditStatus
                                   ,@RejectMsg
                                   ,@CreateTime
                                   ,@CreateUserId)");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@RelationType",entity.RelationType),
                        new SqlParameter("@RelationId",entity.RelationId),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        new SqlParameter("@RejectMsg",entity.RejectMsg),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserId",entity.CreateUserId)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

    }
}
