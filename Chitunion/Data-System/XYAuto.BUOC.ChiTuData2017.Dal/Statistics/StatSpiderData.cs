using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Statistics
{
    //抓取数据明细
    public partial class StatSpiderData : DataBase
    {
        public static readonly StatSpiderData Instance = new StatSpiderData();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Statistics.StatSpiderData entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Stat_SpiderData(");
            strSql.Append("ArticleID,Title,Url,ArticleType,ChannelID,ChannelName,ArticlePublishTime,ArticleSpiderTime,SceneId,SceneName,AccountName,Status,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@ArticleID,@Title,@Url,@ArticleType,@ChannelID,@ChannelName,@ArticlePublishTime,@ArticleSpiderTime,@SceneId,@SceneName,@AccountName,@Status,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@ArticleID",entity.ArticleId),
                        new SqlParameter("@Title",entity.Title),
                        new SqlParameter("@Url",entity.Url),
                        new SqlParameter("@ArticleType",entity.ArticleType),
                        new SqlParameter("@ChannelID",entity.ChannelId),
                        new SqlParameter("@ChannelName",entity.ChannelName),
                        new SqlParameter("@ArticlePublishTime",entity.ArticlePublishTime),
                        new SqlParameter("@ArticleSpiderTime",entity.ArticleSpiderTime),
                        new SqlParameter("@SceneId",entity.SceneId),
                        new SqlParameter("@SceneName",entity.SceneName),
                        new SqlParameter("@AccountName",entity.AccountName),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }
    }
}