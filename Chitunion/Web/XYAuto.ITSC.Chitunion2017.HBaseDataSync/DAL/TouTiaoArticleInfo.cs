using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.DAL
{
    public class TouTiaoArticleInfo : XYAuto.ITSC.Chitunion2017.Dal.DataBase
    {
        public static readonly TouTiaoArticleInfo Instance = new TouTiaoArticleInfo();
        private string Conn_BaseData = ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];

        public DataTable GetData(int topNum)
        {
            string sql = string.Format(@"
SELECT TOP {0} Id,
          UserId ,
          UserName ,
          Title ,
          Url ,
          Content ,
          Category ,
          Tags ,
          CopyRight ,
          Discription ,
          Cover ,
          ReadNum ,
          ComNum ,
          PublishTime ,
          CreateTime
FROM dbo.TouTiaoArticleInfo
WHERE UserId!=51025535398 And IsClean = 0
ORDER BY CreateTime DESC", topNum);
            DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        public int UpdateStatusByIDs(string recids, int status)
        {
            BLL.Loger.Log4Net.Info($"更新这些今日头条TouTiaoArticleInfo表中的主键ID[{recids}],的状态为：{status}");
            string sql = string.Format(@"Update TouTiaoArticleInfo Set IsClean={0} WHERE Id in ({1})", status, StringHelper.SqlFilter(recids));
            int r = SqlHelper.ExecuteNonQuery(Conn_BaseData, CommandType.Text, sql);
            return r;
        }
    }
}
