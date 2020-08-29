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
    //初筛数据统计（天汇总）
    public partial class StatPrimaryStatistics : DataBase
    {
        public static readonly StatPrimaryStatistics Instance = new StatPrimaryStatistics();

        public List<Entities.Statistics.StatPrimarySummary> GetStatPrimarySummaries(
           StatSpiderQuery<Entities.Statistics.StatPrimarySummary> query)
        {
            var sql = $@"
                        --初筛汇总
                        SELECT  SPS.RecID ,
                                SPS.ChannelID ,
                                SPS.ChannelName ,
                                SPS.SceneId ,
                                SPS.SceneName ,
                                SPS.AAScoreType ,
                                SPS.ConditionId ,
                                SPS.ConditionName ,
                                SPS.ArticleCount ,
                                SPS.AccountCount ,
                                SPS.BeginTime ,
                                SPS.EndTime ,
                                SPS.Status ,
                                SPS.CreateTime ,
                                DC.DictName AS AAScoreTypeName
                        FROM    dbo.Stat_PrimarySummary AS SPS WITH ( NOLOCK )
                                LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = SPS.AAScoreType
                        WHERE  SPS.Status = 0
";
            if (query.FilterChannelId != OperatorEnum.无)
            {
                sql += $" AND SPS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }
            if (query.FilterSceneId != OperatorEnum.无)
            {
                sql += $" AND SPS.SceneId {query.FilterSceneId.GetEnumDesc()} 0 ";
            }
            if (query.FilterScoreType != OperatorEnum.无)
            {
                sql += $" AND SPS.AAScoreType {query.FilterScoreType.GetEnumDesc()} 0 ";
            }
            if (query.ScoreType != ScoreTypeEnum.无)
            {
                sql += $" AND DC.DictType ={(int)query.ScoreType} ";
            }
            if (!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                sql += $" AND SPS.BeginTime = '{query.BeginTime}'";
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND SPS.EndTime = '{query.EndTime}'";
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatPrimarySummary>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatPrimaryStatistics> GetStatPrimaryStatisticses(
          StatSpiderQuery<Entities.Statistics.StatPrimaryStatistics> query)
        {
            var sql = $@"
                        --初筛-数据统计

                        SELECT  SPS.RecID ,
                                SPS.ChannelID ,
                                SPS.ChannelName ,
                                SPS.ConditionId ,
                                SPS.ConditionName ,
                                SPS.ArticleCount ,
                                SPS.Date ,
                                SPS.Status ,
                                SPS.CreateTime
                        FROM    dbo.Stat_PrimaryStatistics AS SPS WITH ( NOLOCK )
                        WHERE   SPS.Status = 0
                        ";
            if ((int)query.FilterChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND SPS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }

            if (!string.IsNullOrWhiteSpace(query.BeginTime) || !string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND SPS.Date BETWEEN '{query.BeginTime}' AND '{query.EndTime}'";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatPrimaryStatistics>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatPrimarySummary> GetStatPrimarySummariesZhuanHua(string beginTime, string endTime)
        {
            var sql = $@"

                        --文章初筛可用转化
                        SELECT * FROM
                        (
                        --累计统计
                        SELECT  SPA.RecID ,
                                SPA.ChannelID ,
                                SPA.ChannelName ,
                                SPA.SceneId ,
                                SPA.SceneName ,
                                SPA.AAScoreType ,
                                SPA.ConditionId ,
                                SPA.ConditionName ,
                                SPA.ArticleCount ,
                                SPA.AccountCount ,
                                SPA.BeginTime ,
                                SPA.EndTime ,
                                SPA.Status ,
                                SPA.CreateTime ,
                                1 AS TypeId
                        FROM    dbo.Stat_PrimarySummary AS SPA WITH ( NOLOCK )
                        WHERE   SPA.Status = 0
                                AND SPA.SceneId = 0
                                AND SPA.ChannelID >= 0
                                --AND SPA.ChannelName IS NOT NULL
                                AND SPA.BeginTime = '{beginTime}'
                                AND SPA.EndTime = '{endTime}'
                                AND SPA.AAScoreType = 0
                        UNION
                        --可用入库(包含累计统计，渠道)
                        SELECT  SPA.RecID ,
                                SPA.ChannelID ,
                                SPA.ChannelName ,
                                SPA.SceneId ,
                                SPA.SceneName ,
                                SPA.AAScoreType ,
                                SPA.ConditionId ,
                                SPA.ConditionName ,
                                SPA.ArticleCount ,
                                SPA.AccountCount ,
                                SPA.BeginTime ,
                                SPA.EndTime ,
                                SPA.Status ,
                                SPA.CreateTime ,
                                2 AS TypeId
                        FROM    dbo.Stat_PrimarySummary AS SPA WITH ( NOLOCK )
                        WHERE   SPA.Status = 0
                                AND SPA.SceneId = 0
                                AND SPA.ChannelID > 0
                                AND SPA.BeginTime = '{beginTime}'
                                AND SPA.EndTime = '{endTime}'
                                AND SPA.ConditionId = 77001
                                AND SPA.AAScoreType = 0
		                        ) AS T
                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatPrimarySummary>(data.Tables[0]);
        }
    }
}