using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Statistics
{
    //抓取数据汇总（时间周期汇总）
    public partial class StatSpiderSummary : DataBase
    {
        public static readonly StatSpiderSummary Instance = new StatSpiderSummary();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Statistics.StatSpiderSummary entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Stat_SpiderSummary(");
            strSql.Append("ChannelID,ChannelName,SceneId,SceneName,ArticleType,ArticleCount,AccountCount,BeginTime,EndTime,Status,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@ChannelID,@ChannelName,@SceneId,@SceneName,@ArticleType,@ArticleCount,@AccountCount,@BeginTime,@EndTime,@Status,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@ChannelID",entity.ChannelId),
                        new SqlParameter("@ChannelName",entity.ChannelName),
                        new SqlParameter("@SceneId",entity.SceneId),
                        new SqlParameter("@SceneName",entity.SceneName),
                        new SqlParameter("@ArticleType",entity.ArticleType),
                        new SqlParameter("@ArticleCount",entity.ArticleCount),
                        new SqlParameter("@AccountCount",entity.AccountCount),
                        new SqlParameter("@BeginTime",entity.BeginTime),
                        new SqlParameter("@EndTime",entity.EndTime),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }
    }
}