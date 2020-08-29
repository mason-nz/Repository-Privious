using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    public class GDTDuplicate : DataBase
    {
        #region Instance
        public static readonly GDTDuplicate Instance = new GDTDuplicate();
        #endregion

        public int Insert(Entities.GDT.GDTDuplicate entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_Duplicate(");
            strSql.Append("[ClueID],[Results],[Reason],[CreateTime]");
            strSql.Append(") values (");
            strSql.Append("@ClueID,@Results,@Reason,getdate()");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@ClueID",entity.ClueID),
                        new SqlParameter("@Results",entity.Results),
                        new SqlParameter("@Reason",entity.Reason),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}
