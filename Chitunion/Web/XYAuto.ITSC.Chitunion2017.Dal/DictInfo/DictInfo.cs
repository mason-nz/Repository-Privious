using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.DictInfo
{
    public class DictInfo : DataBase
    {
        public readonly static DictInfo Instance = new DictInfo();

        public DataTable GetInfoByType(int type)
        {
            var strSql = new StringBuilder();

            strSql.Append($@"
                            SELECT  DictId Id ,
                                    DictName Name ,
                                    OrderNum ,
                                    CreateTime
                            FROM    Chitunion2017.dbo.DictInfo
                            WHERE   DictType = {type}
                            ORDER BY OrderNum;");

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString());
            return ds.Tables[0];
        }
        public DataTable GetSharingPlatform(int type)
        {
            var strSql = new StringBuilder();

            strSql.Append($@"
                            SELECT  DictId Id ,
                                    DictName Name ,
                                    OrderNum ,
                                    CreateTime
                            FROM    Chitunion2017.dbo.DictInfo
                            WHERE   DictType = {type} AND Status = 0
                            ORDER BY OrderNum;");

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString());
            return ds.Tables[0];
        }
    }
}
