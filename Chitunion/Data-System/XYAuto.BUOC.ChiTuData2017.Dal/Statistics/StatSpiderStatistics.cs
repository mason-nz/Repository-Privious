using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Statistics
{
    //抓取数据统计（天汇总）
    public partial class StatSpiderStatistics : DataBase
    {
        public static readonly StatSpiderStatistics Instance = new StatSpiderStatistics();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Statistics.StatSpiderStatistics entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Stat_SpiderStatistics(");
            strSql.Append("ChannelID,ChannelName,ArticleType,ArticleCount,AccountCount,Date,Status,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@ChannelID,@ChannelName,@ArticleType,@ArticleCount,@AccountCount,@Date,@Status,@CreateTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@ChannelID",entity.ChannelId),
                        new SqlParameter("@ChannelName",entity.ChannelName),
                        new SqlParameter("@ArticleType",entity.ArticleType),
                        new SqlParameter("@ArticleCount",entity.ArticleCount),
                        new SqlParameter("@AccountCount",entity.AccountCount),
                        new SqlParameter("@Date",entity.Date),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public Tuple<List<Entities.Statistics.StatSpiderSummary>, List<Entities.Statistics.StatSpiderStatistics>>
            GetHeadArticleList(string beginTime, string endTime, StatArticleTypeEnum articleType)
        {
            var sql = $@"

                        --抓取-数据统计
                        SELECT  RecID ,
                                ChannelID ,
                                ChannelName ,
                                SceneId ,
                                SceneName ,
                                Category ,
                                ArticleType ,
                                ArticleCount ,
                                AccountCount ,
                                BeginTime ,
                                EndTime
                        FROM    dbo.Stat_SpiderSummary
                        WHERE   SceneId = 0
                                AND ChannelID >= 0
                                AND Category IS NULL
                                AND [ArticleType] = {(int)articleType}
                                AND BeginTime = '{beginTime}'
                                AND EndTime = '{endTime}'

                        --抓取-数据统计
                        SELECT  SS.RecID ,
                                SS.ChannelID ,
                                SS.ChannelName ,
                                SS.ArticleType ,
                                SS.ArticleCount ,
                                SS.AccountCount ,
                                SS.Date
                        FROM    dbo.Stat_SpiderStatistics AS SS WITH ( NOLOCK )
                        WHERE   SS.ArticleType = {(int)articleType}
                                AND SS.ChannelID > 0
                                AND SS.Date BETWEEN '{beginTime}' AND '{endTime}'
                                AND SS.Status = 0

                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return
                new Tuple<List<Entities.Statistics.StatSpiderSummary>, List<Entities.Statistics.StatSpiderStatistics>>(
                    DataTableToList<Entities.Statistics.StatSpiderSummary>(data.Tables[0]),
                     DataTableToList<Entities.Statistics.StatSpiderStatistics>(data.Tables[1]));
        }

        public List<Entities.Statistics.StatSpiderSummary> GetStatSpiderSummaries(
            StatSpiderQuery<Entities.Statistics.StatSpiderSummary> query)
        {
            var sql = $@"

                        --抓取-数据统计
                        SELECT  RecID ,
                                ChannelID ,
                                ChannelName ,
                                SceneId ,
                                SceneName ,
                                Category ,
                                ArticleType ,
                                ArticleCount ,
                                AccountCount ,
                                BeginTime ,
                                EndTime
                        FROM    dbo.Stat_SpiderSummary
                        WHERE   Status = 0
";
            if (query.FilterChannelId != OperatorEnum.无)
            {
                sql += $" AND ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }
            if (query.FilterSceneId != OperatorEnum.无)
            {
                sql += $" AND SceneId {query.FilterSceneId.GetEnumDesc()} 0 ";
            }
            if (query.FilterAutoCategory != OperatorEnum.无)
            {
                sql += $" AND Category {query.FilterAutoCategory.GetEnumDesc()} ";
            }
            if (!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                sql += $" AND BeginTime = '{query.BeginTime}'";
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND EndTime = '{query.EndTime}'";
            }
            if (query.ArticleType != StatArticleTypeEnum.None)
            {
                sql += $" AND ArticleType = {(int)query.ArticleType}";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatSpiderSummary>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatSpiderStatistics> GetStatSpiderStatisticses(
            StatSpiderQuery<Entities.Statistics.StatSpiderStatistics> query)
        {
            var sql = $@"
                        --抓取-数据统计
                        SELECT  SS.RecID ,
                                SS.ChannelID ,
                                SS.ChannelName ,
                                SS.ArticleType ,
                                SS.ArticleCount ,
                                SS.AccountCount ,
                                SS.Date
                        FROM    dbo.Stat_SpiderStatistics AS SS WITH ( NOLOCK )
                        WHERE  SS.Status = 0
                        ";
            if ((int)query.FilterChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND SS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }
            if ((int)query.FilterSceneId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND SS.SceneId {query.FilterSceneId.GetEnumDesc()} 0 ";
            }
            if (!string.IsNullOrWhiteSpace(query.BeginTime) || !string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND SS.Date BETWEEN '{query.BeginTime}' AND '{query.EndTime}'";
            }
            if ((int)query.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND SS.ArticleType = {(int)query.ArticleType}";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatSpiderStatistics>(data.Tables[0]);
        }

        public Tuple<List<Entities.Statistics.StatSpiderSummary>, List<Entities.Statistics.StatSpiderStatistics>>
            GetDataViewHeadArticleList(string beginTime, string endTime, StatArticleTypeEnum articleType)
        {
            var sql = $@"

                        --抓取-数据统计
                        SELECT  RecID ,
                                ChannelID ,
                                ChannelName ,
                                Category ,
                                SceneId ,
                                SceneName ,
                                ArticleType ,
                                ArticleCount ,
                                AccountCount ,
                                BeginTime ,
                                EndTime
                        FROM    dbo.Stat_SpiderSummary
                        WHERE   SceneId = 0
                                AND ChannelID >= 0
                                AND Category IS NULL
                                AND [ArticleType] = {(int)articleType}
                                AND BeginTime = '{beginTime}'
                                AND EndTime = '{endTime}'

                        --抓取-数据统计
                        SELECT  SS.RecID ,
                                SS.ChannelID ,
                                SS.ChannelName ,
                                SS.ArticleType ,
                                SS.ArticleCount ,
                                SS.AccountCount ,
                                SS.Date
                        FROM    dbo.Stat_SpiderStatistics AS SS WITH ( NOLOCK )
                        WHERE   SS.Status = 0
                                AND SS.ChannelID >= 0
                                AND SS.ArticleType = {(int)articleType}
                                AND SS.Date BETWEEN '{beginTime}' AND '{endTime}'
                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return
                new Tuple<List<Entities.Statistics.StatSpiderSummary>, List<Entities.Statistics.StatSpiderStatistics>>(
                    DataTableToList<Entities.Statistics.StatSpiderSummary>(data.Tables[0]),
                     DataTableToList<Entities.Statistics.StatSpiderStatistics>(data.Tables[1]));
        }
    }
}