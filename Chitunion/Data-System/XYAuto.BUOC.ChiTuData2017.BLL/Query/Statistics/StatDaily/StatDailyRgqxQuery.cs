/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 15:40:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily
{
    public class StatDailyRgqxQuery
          : PublishInfoQueryClient<ReqDailyDto, RespDailyRgqxDto>
    {
        public StatDailyRgqxQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDailyRgqxDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT  SAS1.ChannelID ,
                                    SAS1.Date ,
                                    SAS1.ChannelName ,
		                            DC.DictName AS ArticleTypeName,
                                    ( SELECT    STUFF(( SELECT  '|'
                                                                + CAST(SAS2.ConditionId AS VARCHAR(10))+ ',1,'+ ISNULL(CAST(SAS2.ArticleCount AS VARCHAR(10)),0)+','+CONVERT(VARCHAR(10),SAS2.Date)
                                                                + '#'
									                            + CAST(SAS2.ConditionId AS VARCHAR(10)) + ',2,' + ISNULL(CAST(SAS2.AccountCount AS VARCHAR(10)),0)+','+CONVERT(VARCHAR(10),SAS2.Date)
                                                        FROM    dbo.Stat_ArtificialStatistics AS SAS2 WITH ( NOLOCK )
                                                        WHERE   SAS2.ChannelID > 0
                                                                AND SAS2.ConditionId > 0
                                                                AND SAS2.Date BETWEEN '{0}' AND '{1}'
                                                                AND SAS2.ChannelID = SAS1.ChannelID
                                                                #StatInfoWhere#
                                                      FOR
                                                        XML PATH('')
                                                      ), 1, 1, '')
                                    ) AS StatInfo
                            FROM    dbo.Stat_ArtificialStatistics AS SAS1 WITH ( NOLOCK )
                            LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = SAS1.ArticleType
                            WHERE   SAS1.ChannelID > 0
                                    AND SAS1.ConditionId > 0
                                    AND SAS1.Date BETWEEN '{0}' AND '{1}'

                        ", RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());
            var statInfoWhere = new StringBuilder();
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAS1.ChannelID = {RequetQuery.ChannelId}");
                statInfoWhere.AppendFormat($" AND SAS2.ChannelID = {RequetQuery.ChannelId}");
            }

            if (RequetQuery.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAS1.ArticleType = {RequetQuery.ArticleType}");
                statInfoWhere.AppendFormat($" AND SAS2.ArticleType = {RequetQuery.ArticleType}");
            }

            sbSql.AppendFormat(@" GROUP BY SAS1.ChannelID ,
                                    SAS1.ChannelName ,
		                            DC.DictName,
                                    SAS1.Date ");

            sbSql.Replace("#StatInfoWhere#", statInfoWhere.ToString());

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDailyRgqxDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " Date DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespDailyRgqxDto> GetResult(List<RespDailyRgqxDto> resultList, QueryPageBase<RespDailyRgqxDto> query)
        {
            if (!resultList.Any())
            {
                return base.GetResult(resultList, query);
            }
            resultList.ForEach(SetStatInfo);

            return base.GetResult(resultList, query);
        }

        private void SetStatInfo(RespDailyRgqxDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.StatInfo))
            {
                return;
            }
            var list = StatDailyCsQuery.GetArrayValue(dto.StatInfo);
            var usable = list.Where(s => s.ConditionId == (int)ConditionTypeEnum.可用 && s.Date.Equals(dto.Date.ToString("yyyy-MM-dd"))).ToList();
            var statKeyValueQuery = usable.FirstOrDefault(s => s.TypeId == 2);
            if (statKeyValueQuery != null)
                dto.AccountCount = statKeyValueQuery.Count;
            var firstOrDefault = usable.FirstOrDefault(s => s.TypeId == 1);
            if (firstOrDefault != null)
                dto.ArticleCount = firstOrDefault.Count;

            var toBody = list.Where(s => s.ConditionId == (int)ConditionTypeEnum.置为腰 && s.Date.Equals(dto.Date.ToString("yyyy-MM-dd"))).ToList();
            var tobodyArticle = toBody.FirstOrDefault(s => s.TypeId == 1);
            if (tobodyArticle != null)
                dto.ToBodyArticleCount = tobodyArticle.Count;
            var tobodyAccount = toBody.FirstOrDefault(s => s.TypeId == 2);
            if (tobodyAccount != null)
                dto.ToBodyAccountCount = tobodyAccount.Count;

            var unable = list.Where(s => s.ConditionId == (int)ConditionTypeEnum.作废 && s.Date.Equals(dto.Date.ToString("yyyy-MM-dd"))).ToList();
            var unableArticle = unable.FirstOrDefault(s => s.TypeId == 1);
            if (unableArticle != null)
                dto.NotUseArticleCount = unableArticle.Count;
            var unableyAccount = unable.FirstOrDefault(s => s.TypeId == 2);
            if (unableyAccount != null)
                dto.NotUseAccountCount = unableyAccount.Count;
        }
    }
}