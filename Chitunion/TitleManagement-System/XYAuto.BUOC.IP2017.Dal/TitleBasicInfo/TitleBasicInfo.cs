using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.TitleBasicInfo
{
    public class TitleBasicInfo : DataBase
    {
        public readonly static TitleBasicInfo Instance = new TitleBasicInfo();
        public Entities.TitleBasicInfo.TitleBasicInfo GetModelByTypeAndName(int type, string name)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    dbo.TitleBasicInfo TBI
                            WHERE   TBI.Type = @Type
                                    AND TBI.Name = @Name;");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Type",type),
                        new SqlParameter("@Name",name),
                        };

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return DataTableToEntity<Entities.TitleBasicInfo.TitleBasicInfo>(obj.Tables[0]);
        }
        public List<Entities.TitleBasicInfo.TitleBasicInfo> SelectAll()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    dbo.TitleBasicInfo TBI
                            WHERE   TBI.Status = 0;");
            
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString());
            return DataTableToList<Entities.TitleBasicInfo.TitleBasicInfo>(obj.Tables[0]);
        }
    }
}
