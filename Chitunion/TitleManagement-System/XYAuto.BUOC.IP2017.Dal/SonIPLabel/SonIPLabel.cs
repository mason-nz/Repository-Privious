using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.SonIPLabel
{
    public class SonIPLabel : DataBase
    {
        public static readonly SonIPLabel Instance = new SonIPLabel();

        public int Insert(Entities.SonIPLabel.SonIPLabel entity)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"INSERT  dbo.SonIPLabel
                                    ( SubIPID ,
                                      Name ,
                                      CreateTime ,
                                      CreateUserID,
                                      BatchMediaID
                                    )
                            VALUES  ( @SubIPID , -- SubIPID - int
                                      @Name , -- Name - varchar(100)
                                      GETDATE() , -- CreateTime - datetime
                                      @CreateUserID,  -- CreateUserID - int
                                      @BatchMediaID
                                    )");
            sbSql.Append(";SELECT SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@SubIPID",entity.SubIPID),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@BatchMediaID",entity.BatchMediaID)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}
