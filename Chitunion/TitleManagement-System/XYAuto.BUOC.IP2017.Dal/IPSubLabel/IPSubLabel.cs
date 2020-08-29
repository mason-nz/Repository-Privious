using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.IPSubLabel
{
    public class IPSubLabel : DataBase
    {
        public static readonly IPSubLabel Instance = new IPSubLabel();

        public int Insert(Entities.IPSubLabel.IPSubLabel entity)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"INSERT  dbo.IPSubLabel
                                    ( LabelID ,
                                      TitleID ,
                                      CreateTime ,
                                      CreateUserID,
                                      BatchMediaID
                                    )
                            VALUES  ( @LabelID , -- LabelID - int
                                      @TitleID , -- TitleID - int
                                      GETDATE() , -- CreateTime - datetime
                                      @CreateUserID,  -- CreateUserID - int
                                      @BatchMediaID
                                    )");
            sbSql.Append(";SELECT SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@LabelID",entity.LabelID),
                        new SqlParameter("@TitleID",entity.TitleID),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@BatchMediaID",entity.BatchMediaID)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}
