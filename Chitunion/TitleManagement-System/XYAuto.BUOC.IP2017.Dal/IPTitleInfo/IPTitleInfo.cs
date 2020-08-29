using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.IPTitleInfo
{
    public class IPTitleInfo : DataBase
    {
        public static readonly IPTitleInfo Instance = new IPTitleInfo();
        public DataTable GetSubIPByPID(int parentID)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT T.TitleID DictId,
                                    T.Name DictName
                             FROM   dbo.IPTitleInfo I
                                    INNER JOIN dbo.TitleBasicInfo T ON I.SubIP = T.TitleID
                                                                       AND T.Type = 65005
                             WHERE  PIP = {parentID}
                             GROUP BY T.TitleID ,
                                    T.Name;");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public int InsertIpInfo(string Name, int Type, int UserID)
        {
            string strSql = $@"INSERT  dbo.TitleBasicInfo
        (Name,
          Type,
          Status,
          CreateTime,
          CreateUserID
        )
VALUES('{Name}', --Name - varchar(50)
          {Type}, --Type - int
          0, --Status - int
          GETDATE(), --CreateTime - datetime
          {UserID}-- CreateUserID - int
        ) select @@identity";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        public int InsertIPTitleInfo(int PIP, int SubPIP,int TitleID, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.IPTitleInfo (PIP,SubIP,TitleID,Status,CreateTime,CreateUserID) VALUES ");
            DateTime dtNow = DateTime.Now;
            sb.AppendFormat(" ({0},{1},{2},{3},'{4}',{5}), ", PIP, SubPIP, TitleID, 0, dtNow, UserID);

            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
    }
}
