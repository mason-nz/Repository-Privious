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
    //车型匹配统计数据（天汇总）
    public partial class StatCarMatchStatistics : DataBase
    {
        public static readonly StatCarMatchStatistics Instance = new StatCarMatchStatistics();

        public Tuple<List<Entities.Statistics.StatCarMatchSummary>, List<Entities.Statistics.StatCarMatchStatistics>>
          GetHeadArticleList(string beginTime, string endTime, StatArticleTypeEnum articleType)
        {
            var sql = $@"

                        --数据统计
                        SELECT  CMS.RecID ,
                                CMS.ChannelID ,
                                CMS.ChannelName ,
                                CMS.MatchArticleCount ,
                                CMS.UnMatchArticleCount ,
                                CMS.Category ,
                                CMS.BeginTime ,
                                CMS.EndTime ,
                                CMS.Status ,
                                CMS.CreateTime
                        FROM    dbo.Stat_CarMatchSummary AS CMS WITH ( NOLOCK )
                        WHERE   CMS.ChannelID >= 0
                                AND CMS.Category IS NULL
                                AND CMS.BeginTime = '{beginTime}'
                                AND CMS.EndTime = '{endTime}'

                        --数据统计

                        SELECT  CMS.RecID ,
                                CMS.ChannelID ,
                                CMS.ChannelName ,
                                CMS.MatchArticleCount ,
                                CMS.UnMatchArticleCount ,
                                CMS.Date ,
                                CMS.Status ,
                                CMS.CreateTime
                        FROM    dbo.Stat_CarMatchStatistics AS CMS WITH ( NOLOCK )
                        WHERE   CMS.ChannelID >= 0
                                AND CMS.Date BETWEEN '{beginTime}' AND '{endTime}'

                        ";

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return
                new Tuple<List<Entities.Statistics.StatCarMatchSummary>, List<Entities.Statistics.StatCarMatchStatistics>>(
                    DataTableToList<Entities.Statistics.StatCarMatchSummary>(data.Tables[0]),
                     DataTableToList<Entities.Statistics.StatCarMatchStatistics>(data.Tables[1]));
        }

        public List<Entities.Statistics.StatCarMatchSummary> GetStatCarMatchSummaries(
            StatSpiderQuery<Entities.Statistics.StatCarMatchSummary> query)
        {
            var sql = $@"
                        --车型匹配统计汇总
                        SELECT CMS.RecID ,
                               CMS.ChannelID ,
                               CMS.ChannelName ,
                               CMS.MatchArticleCount ,
                               CMS.UnMatchArticleCount ,
                               CMS.Category ,
                               CMS.BeginTime ,
                               CMS.EndTime ,
                               CMS.Status ,
                               CMS.CreateTime
	                           FROM dbo.Stat_CarMatchSummary CMS WITH(NOLOCK)
                        WHERE  CMS.Status = 0
";
            if (query.FilterChannelId != OperatorEnum.无)
            {
                sql += $" AND CMS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }
            if (query.FilterAutoCategory != OperatorEnum.无)
            {
                sql += $" AND CMS.Category {query.FilterAutoCategory.GetEnumDesc()} ";
            }

            if (!string.IsNullOrWhiteSpace(query.BeginTime))
            {
                sql += $" AND CMS.BeginTime = '{query.BeginTime}'";
            }
            if (!string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND CMS.EndTime = '{query.EndTime}'";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatCarMatchSummary>(data.Tables[0]);
        }

        public List<Entities.Statistics.StatCarMatchStatistics> GetStatCarMatchStatisticses(
           StatSpiderQuery<Entities.Statistics.StatCarMatchStatistics> query)
        {
            var sql = $@"
                        --抓取-数据统计
                        SELECT CMS.RecID ,
                               CMS.ChannelID ,
                               CMS.ChannelName ,
                               CMS.MatchArticleCount ,
                               CMS.UnMatchArticleCount ,
                               CMS.Date ,
                               CMS.Status ,
                               CMS.CreateTime
	                           FROM dbo.Stat_CarMatchStatistics AS CMS WITH(NOLOCK)
                        WHERE CMS.Status = 0
                        ";
            if ((int)query.FilterChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND CMS.ChannelID {query.FilterChannelId.GetEnumDesc()} 0 ";
            }

            if (!string.IsNullOrWhiteSpace(query.BeginTime) || !string.IsNullOrWhiteSpace(query.EndTime))
            {
                sql += $" AND CMS.Date BETWEEN '{query.BeginTime}' AND '{query.EndTime}'";
            }
            if ((int)query.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND CMS.ArticleType = {(int)query.ArticleType}";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Statistics.StatCarMatchStatistics>(data.Tables[0]);
        }
    }
}