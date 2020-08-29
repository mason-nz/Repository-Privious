using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Statistics
{
    //人工清洗数据统计（天汇总）
    public partial class StatArtificialStatistics : DataBase
    {
        public static readonly StatArtificialStatistics Instance = new StatArtificialStatistics();

        /// <summary>
        /// 人工清洗数据统计-头部文章清洗入库
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="articleTypeEnum"></param>
        /// <returns></returns>
        public Tuple<List<Entities.Statistics.StatArtificialSummary>, List<Entities.Statistics.StatArtificialSummary>>
            GetStatArtificialHeadSummaries(string beginTime, string endTime, StatArticleTypeEnum articleTypeEnum)
        {
            var sql = $@"
                        --累计统计
                        SELECT  SAS.RecID ,
                                SAS.ChannelID ,
                                SAS.ChannelName ,
                                SAS.Category,
                                SAS.SceneId ,
                                SAS.SceneName ,
                                SAS.AAScoreType ,
                                SAS.ArticleType ,
                                SAS.ConditionId ,
                                SAS.ConditionName ,
                                SAS.ArticleCount ,
                                SAS.AccountCount ,
                                SAS.BeginTime ,
                                SAS.EndTime ,
                                SAS.Status ,
                                SAS.CreateTime
                        FROM    dbo.Stat_ArtificialSummary AS SAS WITH ( NOLOCK )
                        WHERE   SAS.Status = 0
                                AND SAS.SceneId = 0
		                        AND SAS.ChannelID =0
                                AND SAS.AAScoreType = 0
		                        AND SAS.ArticleType = {(int)articleTypeEnum}
                                AND SAS.Category IS NULL
                                AND SAS.BeginTime = '{beginTime}'
                                AND SAS.EndTime = '{endTime}'

                        SELECT  SAS.RecID ,
                                SAS.ChannelID ,
                                SAS.ChannelName ,
                                SAS.Category,
                                SAS.SceneId ,
                                SAS.SceneName ,
                                SAS.AAScoreType ,
                                SAS.ArticleType ,
                                SAS.ConditionId ,
                                SAS.ConditionName ,
                                SAS.ArticleCount ,
                                SAS.AccountCount ,
                                SAS.BeginTime ,
                                SAS.EndTime ,
                                SAS.Status ,
                                SAS.CreateTime
                        FROM    dbo.Stat_ArtificialSummary AS SAS WITH ( NOLOCK )
                        WHERE   SAS.Status = 0
                                AND SAS.SceneId = 0
		                        AND SAS.ChannelID >0
                                AND SAS.Category IS NULL
		                        AND SAS.ArticleType = {(int)articleTypeEnum}
                                AND SAS.BeginTime = '{beginTime}'
                                AND SAS.EndTime = '{endTime}'
                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<List<StatArtificialSummary>, List<StatArtificialSummary>>(
                DataTableToList<Entities.Statistics.StatArtificialSummary>(data.Tables[0]),
                DataTableToList<Entities.Statistics.StatArtificialSummary>(data.Tables[1]));
        }

        public List<Entities.Statistics.StatArtificialSummary> GetStatArtificialSummaries(
          StatSpiderQuery<Entities.Statistics.StatArtificialSummary> query)
        {
            var sql = $@"
                        --人工清洗数据汇总

                        SELECT  SAS.RecID ,
                                SAS.ChannelID ,
                                SAS.ChannelName ,
                                SAS.Category,
                                SAS.SceneId ,
                                SAS.SceneName ,
                                SAS.AAScoreType ,
                                SAS.ArticleType ,
                                SAS.ConditionId ,
                                SAS.ConditionName ,
                                SAS.ArticleCount ,
                                SAS.AccountCount ,
                                SAS.BeginTime ,
                                SAS.EndTime ,
                                SAS.Status ,
                                SAS.CreateTime,
                                DC.DictName AS AAScoreTypeName
                        FROM    dbo.Stat_ArtificialSummary AS SAS WITH ( NOLOCK )
                                LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = SAS.AAScoreType
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
            if (query.FilterScoreType != OperatorEnum.无)
            {
                sql += $" AND SAS.AAScoreType {query.FilterScoreType.GetEnumDesc()} 0 ";
            }
            if (query.FilterArticleType != OperatorEnum.无)
            {
                sql += $" AND SAS.ArticleType {query.FilterArticleType.GetEnumDesc()} 0 ";
            }
            if (query.FilterAutoCategory != OperatorEnum.无)
            {
                sql += $" AND SAS.Category {query.FilterAutoCategory.GetEnumDesc()} ";
            }
            if (query.ScoreType != ScoreTypeEnum.无)
            {
                sql += $" AND DC.DictType ={(int)query.ScoreType} ";
            }
            if (!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                sql += $" AND SAS.BeginTime = '{query.BeginTime}'";
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND SAS.EndTime = '{query.EndTime}'";
            }
            if ((int)query.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND SAS.ArticleType = {(int)query.ArticleType}";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatArtificialSummary>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatArtificialStatistics> GetStatArtificialStatisticses(
        StatSpiderQuery<Entities.Statistics.StatArtificialStatistics> query)
        {
            var sql = $@"
                        --人工清洗-数据统计
                        SELECT SAS.RecID ,
                               SAS.ChannelID ,
                               SAS.ChannelName ,
                               SAS.ConditionId ,
                               SAS.ConditionName ,
                               SAS.ArticleType ,
                               SAS.ArticleCount ,
                               SAS.AccountCount ,
                               SAS.Date ,
                               SAS.Status ,
                               SAS.CreateTime
                                FROM dbo.Stat_ArtificialStatistics AS SAS WITH(NOLOCK)
                        WHERE SAS.Status =0
                        ";
            if ((int)query.FilterChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND SAS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }

            if (!string.IsNullOrWhiteSpace(query.BeginTime) || !string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND SAS.Date BETWEEN '{query.BeginTime}' AND '{query.EndTime}'";
            }
            if (query.ArticleType != StatArticleTypeEnum.None)
            {
                sql += $" AND SAS.ArticleType = {(int)query.ArticleType}";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatArtificialStatistics>(data.Tables[0]);
        }
    }
}