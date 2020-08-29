using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.BatchLabelHistory
{
    public class BatchLabelHistory:DataBase
    {
        public static readonly BatchLabelHistory Instance = new BatchLabelHistory();

        public int Insert(Entities.BatchLabelHistory.BatchLabelHistory entity)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"INSERT  dbo.BatchLabelHistory
                                ( BatchMediaID ,
                                  TitleID ,
                                  Type ,
                                  Name ,
                                  CreateTime ,
                                  CreateUserID
                                )
                        VALUES  ( @BatchMediaID , -- BatchMediaID - int
                                  @TitleID , -- TitleID - int
                                  @Type , -- Type - bit
                                  @Name , -- Name - varchar(100)
                                  GETDATE() , -- CreateTime - datetime
                                  @CreateUserID  -- CreateUserID - int
                                )");
            sbSql.Append(";SELECT SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@BatchMediaID",entity.BatchMediaID),
                        new SqlParameter("@TitleID",entity.TitleID),
                        new SqlParameter("@Type",entity.Type),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@CreateUserID",entity.CreateUserID)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int InsertBatch(string strSql)
        {
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
    }
}
