using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //赤兔用户，广点通代理商子客户（简称子客）之间的关联表
    public partial class GdtAccountRelation : DataBase
    {
        public static readonly GdtAccountRelation Instance = new GdtAccountRelation();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtAccountRelation entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_AccountRelation(");
            strSql.Append("UserId,AccountId,CreateUserId,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@UserId,@AccountId,@CreateUserId,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserId",entity.UserId),
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public Entities.GDT.GdtAccountRelation GetInfo(int userId)
        {
            var sql = @"
                        SELECT TOP 1 [Id]
                              ,[UserId]
                              ,[AccountId]
                              ,[CreateUserId]
                              ,[CreateTime]
                        FROM [dbo].[GDT_AccountRelation]
                        WHERE UserId = @UserId
                    ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserId",userId)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.GDT.GdtAccountRelation>(data.Tables[0]);
        }
    }
}