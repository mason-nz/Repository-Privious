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
    //机洗数据统计（天汇总）
    public partial class StatAutoStatistics : DataBase
    {
        public static readonly StatAutoStatistics Instance = new StatAutoStatistics();

        public Tuple<List<Entities.Statistics.StatAutoSummary>, List<Entities.Statistics.StatAutoStatistics>>
           GetHeadArticleList(string beginTime, string endTime, StatArticleTypeEnum articleType)
        {
            var sql = $@"

                        --机洗入库-数据统计
                        SELECT SAS.RecID ,
                               SAS.ChannelID ,
                               SAS.ChannelName ,
                               SAS.SceneId ,
                               SAS.SceneName ,
                               SAS.AAScoreType ,
                               SAS.ArticleType ,
                               SAS.ArticleCount ,
                               SAS.AccountCount ,
                               SAS.BeginTime ,
                               SAS.EndTime ,
                               SAS.StorageArticleCount ,
                               SAS.StorageAccountCount ,
                               SAS.Category ,
                               SAS.Status ,
                               SAS.CreateTime
	                           FROM DBO.Stat_AutoSummary AS SAS WITH(NOLOCK)
                        WHERE   SAS.Status = 0
                                AND SAS.SceneId = 0
                                AND SAS.ChannelID >= 0
                                AND SAS.Category IS NULL
                                AND SAS.AAScoreType = 0
                                AND SAS.[ArticleType] = {(int)articleType}
                                AND SAS.BeginTime = '{beginTime}'
                                AND SAS.EndTime = '{endTime}'

                        --机洗入库-数据统计

                        SELECT SAS.RecID ,
                               SAS.ChannelID ,
                               SAS.ChannelName ,
                               SAS.ArticleType ,
                               SAS.ArticleCount ,
                               SAS.AccountCount ,
                               SAS.StorageArticleCount ,
                               SAS.StorageAccountCount ,
                               SAS.Date ,
                               SAS.Status ,
                               SAS.CreateTime
                        FROM DBO.Stat_AutoStatistics AS SAS WITH(NOLOCK)
                        WHERE   SAS.Status = 0
                                AND SAS.ChannelID >= 0
                                AND SAS.ArticleType = {(int)articleType}
                                AND SAS.Date BETWEEN '{beginTime}' AND '{endTime}'
                                AND SAS.Status = 0

                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return
                new Tuple<List<Entities.Statistics.StatAutoSummary>, List<Entities.Statistics.StatAutoStatistics>>(
                    DataTableToList<Entities.Statistics.StatAutoSummary>(data.Tables[0]),
                     DataTableToList<Entities.Statistics.StatAutoStatistics>(data.Tables[1]));
        }

        public List<Entities.Statistics.StatAutoSummary> GetStatAutoSummaries(
           StatSpiderQuery<Entities.Statistics.StatAutoSummary> query)
        {
            var sql = $@"

                        --抓取-数据统计
                        SELECT SAS.RecID ,
                               SAS.ChannelID ,
                               SAS.ChannelName ,
                               SAS.SceneId ,
                               SAS.SceneName ,
                               SAS.AAScoreType ,
                               SAS.ArticleType ,
                               SAS.ArticleCount ,
                               SAS.AccountCount ,
                               SAS.BeginTime ,
                               SAS.EndTime ,
                               SAS.StorageArticleCount ,
                               SAS.StorageAccountCount ,
                               SAS.Category ,
                               DC.DictName AS AAScoreTypeName
                                FROM dbo.Stat_AutoSummary AS SAS WITH(NOLOCK)
	                            LEFT JOIN dbo.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = SAS.AAScoreType
                        WHERE   SAS.Status = 0
";
            if (query.FilterChannelId != OperatorEnum.无)
            {
                sql += $" AND SAS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }
            if (query.FilterSceneId != OperatorEnum.无)
            {
                sql += $" AND SAS.SceneId {query.FilterSceneId.GetEnumDesc()} 0 ";
            }
            if (query.ScoreType != ScoreTypeEnum.无)
            {
                sql += $" AND DC.DictType = {(int)query.ScoreType} ";
            }
            if (query.FilterAutoCategory != OperatorEnum.无)
            {
                sql += $" AND SAS.Category {query.FilterAutoCategory.GetEnumDesc()} ";
            }
            if (query.FilterScoreType != OperatorEnum.无)
            {
                sql += $" AND SAS.AAScoreType {query.FilterScoreType.GetEnumDesc()} 0";
            }
            if (!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                sql += $" AND SAS.BeginTime = '{query.BeginTime}'";
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND SAS.EndTime = '{query.EndTime}'";
            }
            if (query.ArticleType != StatArticleTypeEnum.None)
            {
                sql += $" AND SAS.ArticleType = {(int)query.ArticleType}";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatAutoSummary>(data.Tables[0]);
        }

        /// <summary>
        /// tab-机洗入库-转化数据（查出来了全部）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Entities.Statistics.StatAutoSummary> GetStatAutoSummariesZhuanHua(
            StatSpiderQuery<Entities.Statistics.StatAutoSummary> query)
        {
            var sql = $@"
                    --转化-
                    SELECT  T.TypeId ,
                            T.ChannelID,
                            T.ChannelName ,
                            T.ArticleCount ,
                            T.AccountCount
                    FROM    ( SELECT    1 AS TypeId ,--抓取
                                        SS.ChannelID,
                                        ChannelName ,
                                        ArticleCount ,
                                        AccountCount
                              FROM      dbo.Stat_SpiderSummary AS SS WITH ( NOLOCK )
                              WHERE     SS.SceneId = 0
                                        --AND SS.ChannelID > 0
                                        AND SS.Category IS NULL
                                        AND SS.[ArticleType] = {(int)query.ArticleType}
                                        AND SS.BeginTime = '{query.BeginTime}'
                                        AND SS.EndTime = '{query.EndTime}'
                              UNION
                    --机洗入库的统计
                              SELECT    2 AS TypeId ,--机洗入库
                                        SAS.ChannelID,
                                        ChannelName ,
                                        StorageArticleCount as ArticleCount,
                                        StorageAccountCount as AccountCount
                              FROM      dbo.Stat_AutoSummary AS SAS WITH ( NOLOCK )
                              WHERE     SceneId = 0
                                        --AND SAS.ChannelID > 0
                                        AND SAS.AAScoreType = 0
                                        AND SAS.Category IS NULL
                                        AND SAS.[ArticleType] = {(int)query.ArticleType}
                                        AND SAS.BeginTime = '{query.BeginTime}'
                                        AND SAS.EndTime = '{query.EndTime}'
                            ) AS T
                    ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatAutoSummary>(data.Tables[0]);
        }
    }
}